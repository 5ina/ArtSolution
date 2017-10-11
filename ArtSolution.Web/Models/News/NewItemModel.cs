using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.News;
using System;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.News
{

    [AutoMap(typeof(NewItem))]
    public class NewItemModel : EntityDto
    {
        public string Title { get; set; }
        
        public string ShortDescription { get; set; }
        
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        
        public string FullDescription { get; set; }
        
        public DateTime PublishDateTime { get; set; }
    }


    [AutoMap(typeof(NewItem))]
    public class SimpleNewItemModel : EntityDto
    {
        public string Title { get; set; }

        public string ShortDescription { get; set; }
        
        public DateTime PublishDateTime { get; set; }
    }

    /// <summary>
    /// 商城新闻实体
    /// </summary>
    public class ShopNewItemListModel
    {
        public ShopNewItemListModel()
        {
            this.Items = new List<SimpleNewItemModel>();
        }

        public IList<SimpleNewItemModel> Items { get; set; }

        /// <summary>
        /// 总个数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 是否存在下一页
        /// </summary>
        public bool HasNext { get; set; }

        /// <summary>
        /// 是否存在上一页
        /// </summary>
        public bool HasPrev { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int Pages { get; set; }
    }
}