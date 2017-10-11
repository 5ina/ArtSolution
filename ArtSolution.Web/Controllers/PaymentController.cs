using ArtSolution.Common;
using ArtSolution.Customers;
using ArtSolution.Orders;
using ArtSolution.Web.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace ArtSolution.Web.Controllers
{
    public class PaymentController : ArtSolutionControllerBase
    {

        #region ctor && Fields

        /// <summary>
        /// 统一支付接口
        /// </summary>
        const string UnifiedPayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 网页授权接口
        /// </summary>
        const string access_tokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token";

        /// <summary>
        /// 微信订单查询接口
        /// </summary>
        const string OrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        private readonly IOrderService _orderService;
        private readonly ISettingService _settingService;
        private readonly ICustomerService _customerService;


        public PaymentController(IOrderService orderService, 
            ISettingService settingService,
            ICustomerService customerService)
        {
            this._orderService = orderService;
            this._settingService = settingService;
            this._customerService = customerService;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// 时间截，自1970年以来的秒数
        /// </summary>
        [NonAction]
        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="sParams"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        private string getsign(SortedDictionary<string, string> sParams, string key)
        {
            int i = 0;
            string sign = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sParams)
            {
                if (temp.Value == "" || temp.Value == null || temp.Key.ToLower() == "sign")
                {
                    continue;
                }
                i++;
                sb.Append(temp.Key.Trim() + "=" + temp.Value.Trim() + "&");
            }
            sb.Append("key=" + key.Trim() + "");
            string signkey = sb.ToString();
            sign = CommonHelper.EncryptToSHA1(signkey);

            return sign;
        }

        [NonAction]
        private string PostXmlToUrl(string url, string postData)
        {
            string returnmsg = "";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                returnmsg = wc.UploadString(url, "POST", postData);
            }
            return returnmsg;
        }

        [NonAction]
        public string getPrepay_id(UnifiedOrderModel order, string key)
        {
            string prepay_id = "";
            string post_data = getUnifiedOrderXml(order, key);
            string request_data = PostXmlToUrl(UnifiedPayUrl, post_data);
            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                if (k.Key == "prepay_id")
                {
                    prepay_id = k.Value;
                    break;
                }
            }
            return prepay_id;
        }

        public WeChatOrderDetailModel getOrderDetail(PaymentModel model, string key)
        {
            string post_data = getQueryOrderXml(model, key);
            string request_data = PostXmlToUrl(OrderQueryUrl, post_data);
            WeChatOrderDetailModel orderdetail = new WeChatOrderDetailModel();
            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                switch (k.Key)
                {
                    case "retuen_code":
                        orderdetail.result_code = k.Value;
                        break;
                    case "return_msg":
                        orderdetail.return_msg = k.Value;
                        break;
                    case "appid":
                        orderdetail.appid = k.Value;
                        break;
                    case "mch_id":
                        orderdetail.mch_id = k.Value;
                        break;
                    case "nonce_str":
                        orderdetail.nonce_str = k.Value;
                        break;
                    case "sign":
                        orderdetail.sign = k.Value;
                        break;
                    case "result_code":
                        orderdetail.result_code = k.Value;
                        break;
                    case "err_code":
                        orderdetail.err_code = k.Value;
                        break;
                    case "err_code_des":
                        orderdetail.err_code_des = k.Value;
                        break;
                    case "trade_state":
                        orderdetail.trade_state = k.Value;
                        break;
                    case "device_info":
                        orderdetail.device_info = k.Value;
                        break;
                    case "openid":
                        orderdetail.openid = k.Value;
                        break;
                    case "is_subscribe":
                        orderdetail.is_subscribe = k.Value;
                        break;
                    case "trade_type":
                        orderdetail.trade_type = k.Value;
                        break;
                    case "bank_type":
                        orderdetail.bank_type = k.Value;
                        break;
                    case "total_fee":
                        orderdetail.total_fee = k.Value;
                        break;
                    case "coupon_fee":
                        orderdetail.coupon_fee = k.Value;
                        break;
                    case "fee_type":
                        orderdetail.fee_type = k.Value;
                        break;
                    case "transaction_id":
                        orderdetail.transaction_id = k.Value;
                        break;
                    case "out_trade_no":
                        orderdetail.out_trade_no = k.Value;
                        break;
                    case "attach":
                        orderdetail.attach = k.Value;
                        break;
                    case "time_end":
                        orderdetail.time_end = k.Value;
                        break;
                    default:
                        break;
                }
            }
            return orderdetail;
        }

        protected SortedDictionary<string, string> GetInfoFromXml(string xmlstring)
        {
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstring);
                XmlElement root = doc.DocumentElement;
                int len = root.ChildNodes.Count;
                for (int i = 0; i < len; i++)
                {
                    string name = root.ChildNodes[i].Name;
                    if (!sParams.ContainsKey(name))
                    {
                        sParams.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                    }
                }
            }
            catch { }
            return sParams;
        }

        /// <summary>
        /// 微信统一下单接口xml参数整理
        /// </summary>
        /// <param name="order">微信支付参数实例</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        protected string getUnifiedOrderXml(UnifiedOrderModel order, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", order.appid);
            sParams.Add("attach", order.attach);
            sParams.Add("body", order.body);
            sParams.Add("device_info", order.device_info);
            sParams.Add("mch_id", order.mch_id);
            sParams.Add("nonce_str", order.nonce_str);
            sParams.Add("notify_url", order.notify_url);
            sParams.Add("openid", order.openid);
            sParams.Add("out_trade_no", order.out_trade_no);
            sParams.Add("spbill_create_ip", order.spbill_create_ip);
            sParams.Add("total_fee", order.total_fee.ToString());
            sParams.Add("trade_type", order.trade_type);
            order.sign = getsign(sParams, key);
            sParams.Add("sign", order.sign);

            //拼接成XML请求数据
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString());
            byte[] byteArray = Encoding.UTF8.GetBytes(return_string);
            return_string = Encoding.GetEncoding("GBK").GetString(byteArray);
            return return_string;

        }

        /// <summary>
        /// 微信订单查询接口XML参数整理
        /// </summary>
        /// <param name="model">微信订单查询参数实例</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        protected string getQueryOrderXml(PaymentModel model, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", model.appid);
            sParams.Add("mch_id", model.mch_id);
            sParams.Add("transaction_id", model.transaction_id);
            sParams.Add("out_trade_no", model.out_trade_no);
            sParams.Add("nonce_str", model.nonce_str);
            model.sign = getsign(sParams, key);
            sParams.Add("sign", model.sign);

            //拼接成XML请求数据
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString().TrimEnd(','));
            return return_string;
        }
        #endregion

        #region Method

        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        //public ActionResult Pay(int orderId)
        //{
        //    var order = _orderService.GetOrderById(orderId);

        //    try
        //    {
        //        string paySignKey = _settingService.GetSettingByKey<string>(WeChatSettingNames.Token);
        //        string AppSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);
        //        string mch_id = _settingService.GetSettingByKey<string>(WeChatSettingNames.MchId);
        //        string AppId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
        //        string notifyUrl = _settingService.GetSettingByKey<string>(WeChatSettingNames.NotifyUrl);

        //        string post_data = "appid=" + AppId + "&secret=" + AppSecret  + "&grant_type=authorization_code";                
                
        //        var customer = _customerService.GetCustomerId(Convert.ToInt32(AbpSession.UserId));
                
        //        var nonceStr = CommonHelper.GenerateCode(16);

        //        UnifiedOrderModel model = new UnifiedOrderModel();
        //        model.appid = AppId;
        //        model.attach = order.OrderSn;
        //        model.body = order.OrderRemarks;
        //        model.device_info = "";
        //        model.mch_id = mch_id;
        //        model.nonce_str = nonceStr;
        //        model.notify_url = notifyUrl;
        //        model.openid = customer.OpenId;
        //        model.out_trade_no = order.OrderSn;
        //        model.trade_type = "JSAPI";
        //        model.spbill_create_ip = HttpContext.Request.UserHostAddress;
        //        //model.total_fee = Convert.ToInt32(order.OrderTotal * 100);
        //        model.total_fee = 1;

        //        var prepay_id = getPrepay_id(model, paySignKey);

        //        var timestamp = CommonHelper.GetTimeStamp();

        //        SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
        //        sParams.Add("appId", AppId);
        //        sParams.Add("timeStamp", timestamp);
        //        sParams.Add("nonceStr", nonceStr);
        //        sParams.Add("package", "prepay_id=" + prepay_id);
        //        sParams.Add("signType", "MD5");
        //        var paySign = getsign(sParams, paySignKey);

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.ToString());
        //    }

        //    Response.Redirect("http://你的网站/App/Pay/pay.aspx?showwxpaytitle=1&appId=" +  + "&timeStamp=" + timeStamp + "&nonceStr=" + nonceStr + "&prepay_id=" + prepay_id + "&signType=MD5&paySign=" + paySign + "&OrderID=" + OrderID);

        //}


        //public ActionResult Notify()
        //{

        //}
        #endregion
    }

}