using Abp.WebApi.Controllers;
using ArtSolution.Catalog;
using ArtSolution.Api.Models.Catalog;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace ArtSolution.Api.Catalog
{
    public class CategoryApiService : AbpApiController, ICategoryApiService
    {
        #region Ctor && Field

        private readonly ICategoryService _categoryService;

        public CategoryApiService(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        #endregion

        #region Method
        public ResultMessage<CategoryModel> GetAllCategories(int pageIndex, int pageSize)
        {
            var categories = _categoryService.GetAllCategories(pageIndex: pageIndex, pageSize: pageIndex);
            var models = categories.Items.MapTo<List<CategoryModel>>();
            return new ResultMessage<CategoryModel>(models);
        }

        public ResultMessage<CategoryModel> GetCategoriesByParentId(int parentId)
        {
            var categories = _categoryService.GetCategoriesByParentId(parentId);
            var models = categories.MapTo<List<CategoryModel>>();
            return new ResultMessage<CategoryModel>(models);
        }

        public ResultMessage<CategoryModel> GetCategoryById(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            var model = category.MapTo<CategoryModel>();
            return new ResultMessage<CategoryModel>(model);
        }
        #endregion
    }
}
