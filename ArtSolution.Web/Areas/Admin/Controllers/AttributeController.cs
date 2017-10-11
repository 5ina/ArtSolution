using Abp.AutoMapper;
using Abp.UI;
using ArtSolution.Catalog;
using ArtSolution.Domain.Catalog;
using ArtSolution.Web.Areas.Admin.Models.Catalog;
using ArtSolution.Web.Framework.Controllers;
using ArtSolution.Web.Framework.DataGrids;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class AttributeController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly IAttributeService _attributeService;
        private readonly IProductAttributeService _productAttribtettributeService;


        public AttributeController(IAttributeService attributeService,
            IProductAttributeService productAttribtettributeService)
        {
            this._attributeService = attributeService;
            this._productAttribtettributeService = productAttribtettributeService;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        protected void PrepareAttributeModel(AttributeModel model)
        {
            if (model == null)
                throw new UserFriendlyException("model");
            
        }
        #endregion

        #region Method

        public ActionResult List()
        {
            var model = new AttributeListModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, AttributeListModel model)
        {
            var attributes = _attributeService.GetAttributes(keywords: model.Keywords,
                                            pageIndex: command.Start,
                                            pageSize: command.Length);

            var jsonData = new DataSourceResult
            {
                data = attributes.Items.Select(a => new
                {
                    Id = a.Id,
                    Name = a.Name,
                    DisplayOrder = a.DisplayOrder,
                    WithOrder = _productAttribtettributeService.GetProductAttributesByAttributeId(a.Id).Count(),
                }).ToList(),
            };
            return AbpJson(jsonData);
        }

        public ActionResult Create(int categoryId = 0)
        {
            var model = new AttributeModel();
            PrepareAttributeModel(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(AttributeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Attribute>();
                model.Id = _attributeService.InsertAttribute(entity);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            PrepareAttributeModel(model);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _attributeService.GetAttributeById(id);
            var model = entity.MapTo<AttributeModel>();
            PrepareAttributeModel(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(AttributeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Attribute>();
                _attributeService.UpdateAttribute(entity);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            PrepareAttributeModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateValue(AttributeValueModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<AttributeValue>();
                _attributeService.InsertAttributeValue(entity);
                return AbpJson("ok");
            }
            return AbpJson("error");
        }

        [HttpPost]
        public ActionResult ValueList(int attributeId)
        {
            var list = _attributeService.GetAttributeValuesByAttributeId(attributeId: attributeId);

            var jsonData = new DataSourceResult
            {
                data = list
            };
            return AbpJson(jsonData);
        }

        [HttpPost]
        public ActionResult DeleteValue(int id)
        {
            _attributeService.DeleteAttributeValue(id);
            return AbpJson("ok");
        }
        #endregion
    }
}