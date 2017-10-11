using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Api.Models.Account
{
    public class LoginModel :EntityDto
    {
        [Required,MaxLength(11)]
        public string Mobile { get; set; }
    }
}
