using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Catalog;
using Abp.Domain.Repositories;

namespace ArtSolution.Catalog
{
    public class CategoryService: ArtSolutionAppServiceBase,ICategoryService
    {

        #region Ctor && Field

        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }


        #endregion

        #region Method

        public void DeleteCatgory(int categoryId)
        {
            try
            {
                _categoryRepository.Delete(categoryId);
            }
            catch { }
        }

        public IPagedResult<Category> GetAllCategories(string categoryName = "", 
            int? parentId = null, bool showHidden = false,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _categoryRepository.GetAll();

            if (!showHidden)
                query = query.Where(c => c.Published && !c.IsDeleted);

            if (!String.IsNullOrWhiteSpace(categoryName))
                query = query.Where(c => c.Name.Contains(categoryName));

            if (parentId.HasValue)
                query = query.Where(x => x.ParentId == parentId.Value);

            query = query.OrderBy(c => c.DisplayOrder);
            
            return new PagedResult<Category>(query, pageIndex, pageSize);
        }

        public IList<Category> GetCategoriesByParentId(int parentId, bool showHidden = false)
        {
            var query = _categoryRepository.GetAll();

            query = query.Where(x => x.ParentId == parentId);
            if (!showHidden)
                query = query.Where(c => !c.IsDeleted && c.Published);
            query = query.OrderByDescending(c => c.DisplayOrder);

            return query.ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            try
            {
                return _categoryRepository.Get(categoryId);
            }
            catch {
                return null;
            }
        }

        public int InsertCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

           return _categoryRepository.InsertAndGetId(category);
        }

        public void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            _categoryRepository.Update(category);
        }
        #endregion
    }
}
