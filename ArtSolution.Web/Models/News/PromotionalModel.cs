using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.News;
using System;

namespace ArtSolution.Web.Models.News
{

    [AutoMap(typeof(Promotional))]
    public class PromotionalModel : EntityDto
    {
        public string Name { get; set; }
        public string PromotionalImage { get; set; }

        public string Title { get; set; }
        
        public string Keywords { get; set; }
        public string Description { get; set; }
                
        public bool Published { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string PromotionalPath { get; set; }
    }
}