using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace ArtSolution
{
    public class ResultMessage<T> where T : EntityDto
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="error"></param>
        public ResultMessage(string error)
        {
            this.ResultCode = "400";
            this.Message = error;
        }

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="entity"></param>
        public ResultMessage(T entity)
        {
            this.Body = new List<T>();
            this.Body.Add(entity);
            this.ResultCode = "200";
            this.Message = "执行成功";
        }

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="entity"></param>
        public ResultMessage(IEnumerable<T> entities)
        {
            this.Body = new List<T>();
            this.Body.AddRange(entities);
            this.ResultCode = "200";
            this.Message = "执行成功";
        }

        /// <summary>
        /// 返回代码
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回的实体
        /// </summary>
        public List<T> Body { get; set; }
    }
}
