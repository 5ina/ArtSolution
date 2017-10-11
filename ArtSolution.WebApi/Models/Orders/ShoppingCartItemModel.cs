using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;
using System;

namespace ArtSolution.Api.Models.Orders
{
    [AutoMap(typeof(ShoppingCartItem))]
    public class ShoppingCartItemModel:EntityDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
