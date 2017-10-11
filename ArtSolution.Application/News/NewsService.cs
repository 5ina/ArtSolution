using Abp.Domain.Repositories;
using ArtSolution.Domain.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;

namespace ArtSolution.News
{
    public class NewsService:ArtSolutionAppServiceBase, INewsService
    {

        #region Ctor && Field

        private readonly IRepository<NewItem> _newRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManage;
        public NewsService(IRepository<NewItem> newRepository, IUnitOfWorkManager unitOfWorkManage)
        {
            this._newRepository = newRepository;
            this._unitOfWorkManage = unitOfWorkManage;
        }

        #endregion

        #region Method

        public void DeleteNews(int newId)
        {
            _newRepository.Delete(newId);
        }

        public NewItem GetNewsById(int newsId)
        {
            if (newsId > 0)
                return _newRepository.Get(newsId);

            return null;
        }

        public IPagedResult<NewItem> GetAllNews(bool showHidden = false, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (_unitOfWorkManage.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = _newRepository.GetAll();
                if (!showHidden)
                {
                    query = query.Where(n => !n.IsDeleted);
                    query = query.Where(n => n.PublishDateTime < DateTime.Now);
                }

                query = query.OrderByDescending(n => n.PublishDateTime);
                return new PagedResult<NewItem>(query, pageIndex, pageSize);
            }
        }

        public int InsertNews(NewItem item)
        {
            if (item == null)
                throw new ArgumentNullException("News");

            return _newRepository.InsertAndGetId(item);
        }

        public void UpdateNews(NewItem item)
        {
            if (item == null)
                throw new ArgumentNullException("News");

            _newRepository.Update(item);
        }

        #endregion
    }
}
