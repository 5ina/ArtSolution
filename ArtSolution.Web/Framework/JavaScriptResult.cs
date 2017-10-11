namespace ArtSolution.Web.Framework
{
    public enum HtmlMessageTypeEnum
    {
        /// <summary>
        /// 只有标题的消息
        /// </summary>
        Basic = 0,
        /// <summary>
        /// 带有内容的消息
        /// </summary>
        Context = 1,
        /// <summary>
        /// 成功
        /// </summary>
        Success =2,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,

        /// <summary>
        /// 时间
        /// </summary>
        Timer= 4,
    }

    public static class MessageExtensions
    {
        public static string GetScripts(this HtmlMessageTypeEnum type, string title, string context, int timer)
        {
            var script = string.Empty;
            switch (type)
            {
                default:
                case HtmlMessageTypeEnum.Basic:
                    script = "swal({\"" + title + "\"})";
                    return script;
                case HtmlMessageTypeEnum.Timer:
                    script = "swal({title: \"" + title + "\",text: \"" + context + "\",timer : " + timer + " ,showConfirmButton :false })";
                    return script;

            }
        }
    }
}