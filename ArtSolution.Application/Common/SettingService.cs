using System;
using System.Collections.Generic;
using System.Linq;
using ArtSolution.Domain.Configuration;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;

namespace ArtSolution.Common
{
    public class SettingService : ArtSolutionAppServiceBase, ISettingService
    {
        #region Fields
        private readonly IRepository<Setting> _settingRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor


        public SettingService(IRepository<Setting> settingRepository, ICacheManager cacheManager)
        {
            this._settingRepository = settingRepository;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Nested classes

        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

        #endregion

        #region Method
        public void DeleteSetting(string key)
        {
            var setting = _settingRepository.FirstOrDefault(x => x.Name == key);
            setting.Value = "";
            _settingRepository.Update(setting);
            _cacheManager.GetCache(string.Format(ArtSolutionConsts.CACHE_SETTINGS, key))
                .Get(key, () => _settingRepository.FirstOrDefault(s => s.Name == key));
            
        }

        public IList<Setting> GetAllSettings()
        {
            var query = from s in _settingRepository.GetAll()
                        orderby s.Name
                        select s;
            var settings = query.ToList();
            return settings;
        }

        public void SaveSetting(string key, object value)
        {
            var setting = _settingRepository.FirstOrDefault(x => x.Name == key);
            if (setting == null)
            {
                setting = new Setting
                {
                    Name = key,
                    Value = value.ToString()
                };
                _settingRepository.Insert(setting);
            }
            else
            {
                setting.Value = value.ToString();
                _settingRepository.Update(setting);
            }
            _cacheManager.GetCache(string.Format(ArtSolutionConsts.CACHE_SETTINGS, key))
                .Get(key, () => _settingRepository.FirstOrDefault(s => s.Name == key));
        }

        public TPropType GetSettingByKey<TPropType>(string key) where TPropType : IComparable
        {
            var setting = _settingRepository.FirstOrDefault(x => x.Name == key);

            if (setting == null)
                return default(TPropType);
            return CommonHelper.To<TPropType>(setting.Value);
        }

        public Setting GetSettingById(int settingId)
        {
            return _settingRepository.Get(settingId);
        }

        public void UpdateSetting(Setting setting)
        {
            _settingRepository.Update(setting);
        }

        public void DeleteSetting(int id)
        {
            _settingRepository.Delete(id);
        }
        #endregion
    }
}
