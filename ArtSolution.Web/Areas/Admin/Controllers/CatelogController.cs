using Abp.AutoMapper;
using Abp.UI;
using ArtSolution.Catalog;
using ArtSolution.Domain.Catalog;
using ArtSolution.Web.Areas.Admin.Models.Catalog;
using ArtSolution.Web.Framework.Controllers;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class CatelogController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly ICategoryService _categoryService;


        public CatelogController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        #endregion

        #region Utilities
        [NonAction]
        protected void PrepareCategoryModel(CategoryModel model)
        {
            if (model == null)
                throw new UserFriendlyException("model");

            var parents = _categoryService.GetAllCategories(parentId: 0);
            foreach (var item in parents.Items)
            {
                model.AvailableParentCategories.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Selected = false,
                    Text = item.Name,
                });
            }
            model.AvailableParentCategories.Insert(0,new SelectListItem
            {
                Value = "0",
                Selected = true,
                Text = "顶级类别",
            });
        }

        #endregion

        #region Method

        public ActionResult List()
        {
            var model = new CategoryListModel();
            return View(model);
        }


        [HttpPost]
        public ActionResult List(DataSourceRequest command, CategoryListModel model)
        {
            var categories = _categoryService.GetAllCategories(categoryName: model.Keywords,
                showHidden: true,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);
            var jsonData = new DataSourceResult
            {
                Data = categories.Items.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Parent = x.ParentId == 0 ? "--" : _categoryService.GetCategoryById(x.ParentId).Name,                    
                    Published = x.Published ? "发布" : "未发布",
                    DisplayOrder = x.DisplayOrder,
                    CreationTime = x.CreationTime
                }).ToList(),
            };
            return AbpJson(jsonData);
        }

        public ActionResult Create()
        {
            var model = new CategoryModel();
            PrepareCategoryModel(model);
            model.DisplayOrder = 999;
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(CategoryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Category>();
                entity.IsDeleted = false;
                model.Id = _categoryService.InsertCategory(entity);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }

            PrepareCategoryModel(model);
            return View(model);
        }


        public ActionResult Edit(int id)
        {

            var category = _categoryService.GetCategoryById(id);
            var model = category.MapTo<CategoryModel>();
            PrepareCategoryModel(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(CategoryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryService.GetCategoryById(model.Id);
                category = model.MapTo<CategoryModel, Category>(category);
                _categoryService.UpdateCategory(category);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            PrepareCategoryModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _categoryService.DeleteCatgory(id);
                return AbpJson("true");
            }
            catch (Exception e)
            {
                return AbpJson(e.Message);
            }

        }

        #endregion
    }
}