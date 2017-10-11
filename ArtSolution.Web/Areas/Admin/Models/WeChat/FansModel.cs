using System.Collections.Generic;

namespace ArtSolution.Web.Areas.Admin.Models.WeChat
{
    public class FansModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OpenId data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string next_openid { get; set; }
        public class OpenId
        {
            /// <summary>
            /// 
            /// </summary>
            public List<string> openid { get; set; }
        }
    }

}