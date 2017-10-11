namespace ArtSolution.Web.Models.Customers
{
    public class CustomerCenterModel
    {
        public int CustomerId { get; set; }

        public string CustomerAvatar { get; set; }
        public string CustomerName { get; set; }

        public bool BindMobile { get; set; }

        /// <summary>
        /// 是否推广人
        /// </summary>
        public bool Promoter { get; set; }
    }
}