using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    [AutoMap(typeof(ProductTag))]
    public class ProductTagModel : EntityDto
    {
        [DisplayName("标签名称")]
        public string Name { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        [DisplayName("权重")]
        [UIHint("DisplayOrder")]
        public int DisplayOrder { get; set; }


        public bool Enabled { get; set; }
        
        [UIHint("PictureUrl")]
        public string TagImage { get; set; }
    }
}