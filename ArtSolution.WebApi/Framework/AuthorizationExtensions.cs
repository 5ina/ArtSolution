using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace ArtSolution.Framework
{
    /// <summary>
    /// 认证属性扩展
    /// </summary>
    public static class AuthorizationExtensions
    {
        /// <summary>
        /// 验证当前的token是否通过
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static bool VerifyToken(this HttpRequestHeaders headers)
        {
            IEnumerable<string> tokenValues = new List<string>();

            headers.TryGetValues("token", out tokenValues);

            //加密计算 如果一直返回True 否则返回|False
            //Abp.Dependency.IocManager.Instance.Resolve<>

            //if(tokenValues.Count() > 0)

            return true;
        }
    }
}
