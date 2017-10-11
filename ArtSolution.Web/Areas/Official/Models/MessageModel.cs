using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.official;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Official.Models
{
    [AutoMap(typeof(MessageBoard))]
    public class MessageModel : EntityDto
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Message { get; set; }
    }
    
}