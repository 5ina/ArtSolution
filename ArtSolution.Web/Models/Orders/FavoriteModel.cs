using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;
using System;

namespace ArtSolution.Web.Models.Orders
{

    [AutoMap(typeof(Favorite))]
    public class FavoriteModel : EntityDto
    {
        public int CustomerId { get; set; }
        
        public int ProductId { get; set; }
        
        public string ProductName { get; set; }
        
        public string ProductImage { get; set; }

        public DateTime CreationTime { get; set; }
    }
}