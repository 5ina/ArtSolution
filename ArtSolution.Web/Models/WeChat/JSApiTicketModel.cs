namespace ArtSolution.Web.Models.WeChat
{
    public class JSApiTicketModel
    {        
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }

        public override string ToString()
        {
            return this.ticket;
        }


    }
}