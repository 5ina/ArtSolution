﻿using Abp.UI;
using ArtSolution.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Xml;

namespace ArtSolution
{

    /// <summary>
    /// 公共类
    /// </summary>
    public static class CommonHelper
    {

        #region 验证

        /// <summary>
        /// 字符串不为Null
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        }


        /// <summary>
        /// 字符串最大长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="postfix">替代超过长度的字符串</param>
        /// <returns>如果未超过返回原有字符串，否则截取</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }


        /// <summary>
        /// 验证邮箱格式
        /// </summary>
        /// <param name="email">需要验证的邮箱</param>
        /// <returns>true 为正确格式/ false 非邮箱格式</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new UserFriendlyException("邮箱无效");
            }

            return output;
        }


        /// <summary>
        /// 验证手机号,座机
        /// </summary>
        /// <param name="mobile">需要验证的手机号码</param>
        /// <returns>true 为正确格式/ false 错误格式</returns>
        public static bool IsValidPhone(string mobile)
        {
            if (String.IsNullOrEmpty(mobile))
                return false;

            mobile = mobile.Trim();
            return Regex.IsMatch(mobile, @"^(\d{3,4}(\s|-)?)?\d{7,8}$");
            //var result = Regex.IsMatch(mobile, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            //return result;
        }


        /// <summary>
        /// 验证银行卡号
        /// </summary>
        /// <param name="mobile">需要验证的手机号码</param>
        /// <returns>true 为正确格式/ false 错误格式</returns>
        public static bool IsBankCardNo(string bankCardNo)
        {
            if (String.IsNullOrEmpty(bankCardNo))
                return false;

            bankCardNo = bankCardNo.Trim();
            return Regex.IsMatch(bankCardNo, @"^(\d{16}|\d{19}|\d{17})$");
        }


        /// <summary>
        /// 验证身份证号
        /// </summary>
        /// <param name="identityCard"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(string identityCard)
        {
            if (String.IsNullOrWhiteSpace(identityCard))
            {
                return false;
            }
            identityCard = identityCard.Trim();
            return Regex.IsMatch(identityCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|[X|x])$");
        }


        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>返回字符串</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }



        /// <summary>
        /// 返回指定的随机整数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// 确保只是数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串,非数字则为空</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }


        public static string GenerateString(int length)
        {
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];

            }
            return result;
        }
        public static string GenerateCode(int length)
        {
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];

            }
            return result;
        }

        /// <summary>
        /// 生成随机数（数字）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateNumber(int length)
        {
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string result = "";
            int n = Pattern.Length;
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];

            }
            return result;
        }

        /// <summary>
        /// 签名随即字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 生成编码
        /// </summary>
        /// <param name="strCode"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateCode(string strCode = "", int length = 20)
        {
            length = length - strCode.Length;

            return string.Format("{0}{1}", strCode, GenerateString(length));
        }


        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }


        /// <summary>
        /// 生成时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        #endregion

        #region ConvertTo
        public static TypeConverter GetCustomTypeConverter(Type type)
        {

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();
            if (type == typeof(Dictionary<int, int>))
                return new GenericDictionaryTypeConverter<int, int>();

            return TypeDescriptor.GetConverter(type);
        }

        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                TypeConverter destinationConverter = GetCustomTypeConverter(destinationType);
                TypeConverter sourceConverter = GetCustomTypeConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }


        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        public static T To<T>(object value)
        {
            return (T)To(value, typeof(T));
        }

        #endregion
        
        #region 
        public static string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            string saltAndPassword = String.Concat(password, saltkey);

            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("无法识别的哈希名称");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        /// <summary>
        /// sha1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptToSHA1(string str)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(str);
            byte[] str2 = SHA1.Create().ComputeHash(str1);// sha1.ComputeHash(str1);

            var sb = new StringBuilder();
            foreach (var t in str2)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion

        #region Convert
        public static string ConvertEnum(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            string result = string.Empty;
            foreach (var c in str)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }
        #endregion

        #region generate

        /// <summary>
        /// 生成订单号码
        /// </summary>
        /// <returns></returns>
        public static string GenerateOrderSN()
        {
            var orderSn = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddhhmmssffff"), GenerateString(2));
            return orderSn;

        }

        /// <summary>
        /// 生成退款订单号
        /// </summary>
        /// <returns></returns>
        public static string GenerateReturnOrderSN()
        {
            var orderSn = string.Format("TK-{0}{1}", DateTime.Now.ToString("yyyyMMddhhmmssffff"), GenerateString(2));
            return orderSn;

        }

        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string GetMD5(string encypStr)
        {
            MD5 md5 = MD5.Create();
            var sgin = string.Empty;

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(encypStr));

            var sb = new StringBuilder();
            foreach (byte b in s)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }


        /// <summary>
        /// 采用SHA-1算法加密字符串（小写）
        /// </summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <returns></returns>
        public static string GetSha1(string encypStr)
        {
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(encypStr));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
            
        }

        public static string GenerateCouponCode(string lastCode = "")
        {
            var firstCode = "COPB-";
            var code = string.Format("{0}{1}", firstCode, Guid.NewGuid().ToString(), lastCode);
            return code;
        }
        #endregion

        #region Url

        /// <summary>
        /// 对字符串进行URL编码
        /// </summary>
        /// <param name="instr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string UrlEncode(string instr, string charset)
        {
            //return instr;
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = System.Web.HttpUtility.UrlEncode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = System.Web.HttpUtility.UrlEncode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;
            }
        }
        #endregion
    }
}
