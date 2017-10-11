using Abp.Application.Services;
using ArtSolution.Domain.Configuration;
using System;
using System.Collections.Generic;

namespace ArtSolution.Common
{
    /// <summary>
    /// 配置服务
    /// </summary>
    public interface ISettingService : IApplicationService
    {
        /// <summary>
        /// 存储配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SaveSetting(string key, object value);

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="key"></param>
        void DeleteSetting(string key);
        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="key"></param>
        void DeleteSetting(int id);

        /// <summary>
        /// 根据主键获取配置信息
        /// </summary>
        /// <param name="settingId"></param>
        /// <returns></returns>
        Setting GetSettingById(int settingId);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="setting"></param>
        void UpdateSetting(Setting setting);

        /// <summary>
        /// 获取所有配置
        /// </summary>
        /// <returns></returns>
        IList<Setting> GetAllSettings();

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <typeparam name="TPropType"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TPropType GetSettingByKey<TPropType>(string key) where TPropType : IComparable;
    }
}
