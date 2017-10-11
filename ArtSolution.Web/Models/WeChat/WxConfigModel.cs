namespace ArtSolution.Web.Models.WeChat
{
    public class WxConfigModel
    {
        public string appId { get; set; }
        public string timestamp { get; set; }
        public string noncestr { get; set; }
        public string signature { get; set; }

        public string ticket { get; set; }
        public string value { get; set; }
        public string urlPath { get; set; }
        
    }
}