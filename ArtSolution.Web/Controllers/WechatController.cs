using Abp.AutoMapper;
using Abp.Runtime.Caching;
using ArtSolution.Authentication;
using ArtSolution.Authentication.Dto;
using ArtSolution.Common;
using ArtSolution.Customers;
using ArtSolution.Discount;
using ArtSolution.Domain.Customers;
using ArtSolution.Names;
using ArtSolution.Security;
using ArtSolution.Web.Framework;
using ArtSolution.Web.Models.WeChat;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ArtSolution.Web.Controllers
{
    public class WechatController : ArtSolutionControllerBase
    {
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerService _customerService;
        private readonly LoginManager _loginManager;
        private readonly ICouponService _couponService;
        private readonly ICustomerRewardService _rewardService;
        public WechatController(ISettingService settingService,
            ICacheManager cacheManager,
            ICustomerService customerService,
            ICouponService couponService, 
            ICustomerRewardService rewardService,
            LoginManager loginManager)
        {
            this._cacheManager = cacheManager;
            this._settingService = settingService;
            this._customerService = customerService;
            this._couponService = couponService;
            this._rewardService = rewardService;
            this._loginManager = loginManager;
        }


        #region Utilities
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion


        public ActionResult OpenApi(string echoStr, string signature, string timestamp, string nonce)
        {
            var token = _settingService.GetSettingByKey<string>(WeChatSettingNames.Token);
            if (Request.HttpMethod.ToUpper() == "POST")
            {
                //微信服务器对接口消息
                Logger.Debug("微信服务器对接口消息");
                using (Stream stream = HttpContext.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    var postString = Encoding.UTF8.GetString(postBytes);
                    Handle(postString);
                    return Content("");
                }

            }
            else//微信进行的Get测试(开发者认证)
            {
                Logger.Debug("开发者认证");
                return WxAuth(token);
            }

        }


        public ActionResult ValidToken(string echoStr, string signature, string timestamp, string nonce)
        {
            var Token = _settingService.GetSettingByKey<string>(WeChatSettingNames.Token);
            string[] ArrTmp = { Token, timestamp, nonce };

            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");


            if (tmpStr.ToLower() == signature.ToLower())
            {
                return Content(echoStr);

            }
            return Content("err");
        }

        private string SendTextMessage(WxMessage wxmessage, string content)
        {
            string result = string.Format(Message, wxmessage.FromUserName, wxmessage.ToUserName, DateTime.Now.Ticks, content);
            return result;
        }

        /**
         * snsapi_base
         * **/
        public ActionResult OAuth()
        {
            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);

            string code = Request.QueryString["code"];
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    OAuthToken oauthToken = Framework.HttpUtility.Get<OAuthToken>(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appId, appSecret, code));

                    var accessToken = _cacheManager.GetCache(ArtSolutionConsts.CACHE_ACCESS_TOKEN)
                        .Get(ArtSolutionConsts.CACHE_ACCESS_TOKEN, () => GetAccessToken(appId, appSecret));
                    var token = "";
                    if (accessToken != null && !string.IsNullOrEmpty(accessToken.access_token))
                    {
                        token = accessToken.access_token;
                    }

                    if (oauthToken != null && !string.IsNullOrEmpty(oauthToken.openid))
                    {
                        OAuthUserInfo userInfo = Framework.HttpUtility.Get<OAuthUserInfo>(string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", token, oauthToken.openid), Logger);
                        //OAuthUserInfo userInfo = HttpUtility.Get<OAuthUserInfo>(string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", token, oauthToken.openid));
                        if (userInfo != null)
                        {
                            try
                            {
                                Logger.Debug("openid" + userInfo.openid);
                                    var customer = _customerService.GetCustomerByOpenId(userInfo.openid);
                                
                                #region 假设用户不存在
                                if (customer == null || customer.Id == 0) // 判定用户是否存在
                                {
                                    var salt = CommonHelper.GenerateCode(6);
                                    var _encryptionService = Abp.Dependency.IocManager.Instance.Resolve<IEncryptionService>();
                                    var password = _encryptionService.CreatePasswordHash("123456", salt);
                                    
                                    customer = new Customer
                                    {
                                        Mobile = "",
                                        Password = password,
                                        OpenId = userInfo.openid,
                                        Promoter = 0,
                                        CustomerRoleId = (int)CustomerRole.Buyer,
                                        NickName = userInfo.nickname,
                                        PasswordSalt = salt,
                                        IsSubscribe = userInfo.subscribe == 1,
                                        CreationTime = DateTime.Now,
                                        LastModificationTime = DateTime.Now,
                                        VerificationCode = ""
                                    };
                                    customer.Id = _customerService.CreateCustomer(customer);
                                    if (customer.IsSubscribe)
                                    {
                                        //优惠券处理
                                        AddNewCustomerConpon(customer);
                                        //积分处理

                                        //订阅
                                        SendMessageToUserFirstSub(customer);
                                    }
                                }
                                #endregion

                                #region 用户存在

                                else if(userInfo.subscribe == 1)
                                {
                                    customer.NickName = userInfo.nickname;
                                    customer.SaveCustomerAttribute<string>(CustomerAttributeNames.Avatar, userInfo.headimgurl);
                                    customer.SaveCustomerAttribute<int>(CustomerAttributeNames.Sex, userInfo.sex);
                                    customer.NickName = userInfo.nickname;
                                    _customerService.UpdateCustomer(customer);
                                }
                                var dto = customer.MapTo<CustomerDto>();
                                var identity = _loginManager.CreateUserIdentity(dto);

                                //用户登录
                                AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
                                #endregion
                            }
                            catch (Exception e)
                            {
                                Logger.Debug("错误信息:" + e.Message);
                                Logger.Debug("错误内容：" + userInfo.openid + userInfo.nickname + userInfo.subscribe);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.Message);
            }
            return Redirect(Request.QueryString["state"]);
        }

        //被动回复用户消息
        public string Message
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";
            }
        }


        private T GetValue<T>(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            request.Timeout = 2000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

            string result = sr.ReadToEnd();
            Logger.Debug("result:" + result);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public AccessToken GetAccessToken(string appId, string appSecret)
        {

            AccessToken token = HttpUtility.Get<AccessToken>(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret));

            Logger.Debug(string.Format("appId：{0}，appSecret：{1}，token:{2}", appId, appSecret, token.access_token));

            return token;
        }

        [HttpGet]
        public ActionResult JsApi_Ticket()
        {
            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);

            var token = _cacheManager.GetCache(ArtSolutionConsts.CACHE_ACCESS_TOKEN).Get(ArtSolutionConsts.CACHE_ACCESS_TOKEN, () => GetAccessToken(appId, appSecret));
            var tickent = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token.access_token);

            JSApiTicketModel oauthToken = HttpUtility.Get<JSApiTicketModel>(tickent);
            if (oauthToken == null)
                Logger.Debug("oauthToken为NULL");
            var nonceStr = CommonHelper.GenerateNonceStr();
            var timestamp = CommonHelper.GetTimeStamp();

            var url = this.Request.Url.Host + this.Request.Url.PathAndQuery;
            var urlPath = "jsapi_ticket=" + oauthToken.ticket + "&noncestr" + nonceStr + "&timestamp=" + timestamp + "&url=" + url;
            var signature = CommonHelper.EncryptToSHA1(urlPath);
            var jsonData = new
            {
                appId = appId,
                timestamp = CommonHelper.GetTimeStamp(),
                nonceStr = CommonHelper.GenerateNonceStr(),
                signature = signature
            };
            return AbpJson(data: jsonData, behavior: JsonRequestBehavior.AllowGet);

        }


        #region 
        /// <summary>
        /// 处理信息并应答
        /// </summary>
        private void Handle(string postStr)
        {
            Framework.WeChat.WechatRequest wr = new Framework.WeChat.WechatRequest(postStr);
            var eventStr = wr.LoadEvent(Logger);
            Hashtable parameters;
            switch (eventStr)
            {
                //case "scan"://关注
                case "subscribe":
                    parameters = wr.LoadXml();
                    NewCustomer(parameters);
                    break;
                case "unsubscribe":
                    parameters = wr.LoadXml();
                    UnSubScribte(parameters);
                    break;
                default:
                    break;
            }            
        }
        #endregion

        #region 微信验证
        public ActionResult WxAuth(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Content("");
            }
            string echoString = HttpContext.Request.QueryString["echostr"];
            string signature = HttpContext.Request.QueryString["signature"];
            string timestamp = HttpContext.Request.QueryString["timestamp"];
            string nonce = HttpContext.Request.QueryString["nonce"];
            if (CheckSignature(token, signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    return Content(echoString);
                }
            }
            return Content("");
        }
        /// <summary>
        /// 验证微信签名
        /// </summary>
        public bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 扫面新增用户
        private Customer NewCustomer(Hashtable parameters)
        {
            var openId = parameters["FromUserName"].ToString();
            var customer = _customerService.GetCustomerByOpenId(openId);
            var code = CommonHelper.GenerateCode(6);
            
            if (customer == null || customer.Id == 0) // 判定用户是否存在
            {
                var _encryptionService = Abp.Dependency.IocManager.Instance.Resolve<IEncryptionService>();
                var password = _encryptionService.CreatePasswordHash("123456", code);
                var promoter = parameters["EventKey"].ToString().Replace("qrscene_", "");
                customer = new Customer
                {
                    Mobile = "",
                    Password = password,
                    OpenId = openId,
                    Promoter = Convert.ToInt32(promoter),
                    CustomerRoleId = (int)CustomerRole.Buyer,
                    NickName = "",
                    PasswordSalt = code,
                    IsSubscribe = true,
                };
                Logger.Debug("新增了用户信息：" + customer.OpenId);
                customer.Id = _customerService.CreateCustomer(customer);
                Logger.Debug("新增了用户信息：" + customer.Id);

                Logger.Debug("开始");
                //优惠券处理
                AddNewCustomerConpon(customer);

                Logger.Debug("新增了优惠券");

                //发送消息
                SendMessageToUserFirstSub(customer);

                //积分处理
            }

            return customer;

        }
        #endregion

        #region Coupon

        /// <summary>
        /// 新增8元优惠券
        /// </summary>
        /// <param name="customer"></param>
        private void AddNewCustomerConpon(Customer customer)
        {
            _couponService.InsertCoupon(new Domain.Discounts.Coupon {
                Amount= 8,
                CreationTime =DateTime.Now,
                CustomerId = customer.Id,
                DiscountCode =CommonHelper.GenerateCouponCode(),
                Name = "新手优惠券",
                Used = false,
                OrderId = 0,                
            });
        }
        #endregion

        #region Reward 积分
        /// <summary>
        /// 首次关注赠送积分
        /// </summary>
        /// <param name="customer"></param>
        private void AddRewardByCustomer(Customer customer)
        {

            var enabledReward = _settingService.GetSettingByKey<bool>(RewardSettingNames.Enabled);

            if (enabledReward)
            {
                var rewardForRegister = _settingService.GetSettingByKey<int>(RewardSettingNames.PointsForRegistration);
                var reward = customer.GetCustomerAttributeValue<int>(CustomerAttributeNames.Reward);
                _rewardService.InsertRewardHistory(new CustomerReward
                {
                    CreationTime = DateTime.Now,
                    CustomerId = customer.Id,
                    Message = "新用户首次关注赠送",
                    Total = reward,
                    Points = rewardForRegister,
                });

                customer.SaveCustomerAttribute<int>(CustomerAttributeNames.Reward, reward + rewardForRegister);
            }
        }

        #endregion


        #region 取消关注
        private void UnSubScribte(Hashtable parameters)
        {
            var openId = parameters["FromUserName"].ToString();
            var customer = _customerService.GetCustomerByOpenId(openId);

            if (customer != null)
            {
                customer.IsSubscribe = false;
                _customerService.UpdateCustomer(customer);
            }
        }
        #endregion


        /// <summary>
        /// 首次关注发送的消息
        /// </summary>
        /// <param name="customer"></param>
        private void SendMessageToUserFirstSub(Customer customer)
        {
            Logger.Debug("首次订阅开始");
            var model = new WeChatMessageModel();
            model.msgtype = "news";
            model.touser = customer.OpenId;
            model.news.articles.Add(new WeChatMessageModel.ArticlesItem
            {
                title = "同学你好，初次见面请收下见面礼",
                url = "http://www.bb-girl.cn/Customer/CouponList",
                picurl = "http://www.bb-girl.cn/images/hb.png",
                description = "礼轻情意重,赶紧用了吧。"
            });

            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);
            string msgUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
            var token = GetAccessToken( appId, appSecret);
            string url = string.Format(msgUrl, token.access_token);

            HttpWebResponseUtility client = new HttpWebResponseUtility();

            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            client.CreatePostHttpResponse(url: url, data: msg);

        }
    }


    public class HttpUtility
    {
        public static T Get<T>(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            request.Timeout = 4000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

            string result = sr.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static HttpPostResultClass Post(string url,Dictionary<string,string> param)
        {

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();

                foreach (var item in param)
                {
                    values[item.Key] = item.Value;
                }                

                var response = client.UploadValues(url, values);

                var responseString = Encoding.Default.GetString(response);

                var result = JsonConvert.DeserializeObject<HttpPostResultClass>(responseString);
                return result;
            }            
        }
        
    }

    public class HttpPostResultClass {
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }

}