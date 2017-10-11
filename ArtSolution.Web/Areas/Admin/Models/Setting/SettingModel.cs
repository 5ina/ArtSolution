using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    [AutoMap(typeof(ArtSolution.Domain.Configuration.Setting))]
    public class SettingModel:EntityDto
    {
        public string Name { get; set; }
        
        public string Value { get; set; }
    }
}