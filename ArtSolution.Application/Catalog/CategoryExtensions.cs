using ArtSolution.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolution.Catalog
{
    /// <summary>
    /// 类别扩展
    /// </summary>
    public static class CategoryExtensions
    {
        /// <summary>
        /// 格式化类的面包屑对象
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="categoryService">类别服务</param>
        /// <param name="separator">Separator</param>
        /// <returns>Formatted breadcrumb</returns>
        public static string GetFormattedBreadCrumb(this Category category,
            ICategoryService categoryService,
            string separator = ">>")
        {
            string result = string.Empty;

            var breadcrumb = GetCategoryBreadCrumb(category, categoryService, true);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = breadcrumb[i].Name;
                result = String.IsNullOrEmpty(result)
                    ? categoryName
                    : string.Format("{0} {1} {2}", result, separator, categoryName);
            }

            return result;
        }

        /// <summary>
        /// 格式化类的面包屑对象
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">所有类别</param>
        /// <param name="separator">Separator</param>
        /// <returns>Formatted breadcrumb</returns>
        public static string GetFormattedBreadCrumb(this Category category,
            IList<Category> allCategories,
            string separator = ">>")
        {
            string result = string.Empty;

            var breadcrumb = GetCategoryBreadCrumb(category, allCategories, true);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = breadcrumb[i].Name;
                result = String.IsNullOrEmpty(result)
                    ? categoryName
                    : string.Format("{0} {1} {2}", result, separator, categoryName);
            }

            return result;
        }

        /// <summary>
        /// 获取类的面包屑
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="categoryService">Category service</param>
        /// <param name="showHidden">是否显示隐藏的值</param>
        /// <returns>Category breadcrumb </returns>
        public static IList<Category> GetCategoryBreadCrumb(this Category category,
            ICategoryService categoryService,
            bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var result = new List<Category>();

            var alreadyProcessedCategoryIds = new List<int>();

            while (category != null && //not null
                !category.IsDeleted && //not deleted
                (showHidden || category.Published) && //published
                !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
            {
                result.Add(category);

                alreadyProcessedCategoryIds.Add(category.Id);

                category = categoryService.GetCategoryById(category.ParentId);
            }
            result.Reverse();
            return result;
        }



        /// <summary>
        /// 获取类的面包屑
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>Category breadcrumb </returns>
        public static IList<Category> GetCategoryBreadCrumb(this Category category,
            IList<Category> allCategories,
            bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var result = new List<Category>();

            //used to prevent circular references
            var alreadyProcessedCategoryIds = new List<int>();

            while (category != null && //not null
                !category.IsDeleted && //not deleted
                (showHidden || category.Published) && //published
                !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
            {
                result.Add(category);

                alreadyProcessedCategoryIds.Add(category.Id);

                category = (from c in allCategories
                            where c.Id == category.ParentId
                            select c).FirstOrDefault();
            }
            result.Reverse();
            return result;
        }
        
        //递归向上获取所有类别
        public static List<int> GetAllCategoriesRecursive(this int categoryId,
            ICategoryService categoryService)
        {
            var alreadyProcessedCategoryIds = new List<int>();
            var category = categoryService.GetCategoryById(categoryId);

            while (category != null && //not null
                !category.IsDeleted && //not deleted
                category.Published && //published
                !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
            {
                alreadyProcessedCategoryIds.Add(category.Id);

                category = categoryService.GetCategoryById(category.ParentId);
            }
            alreadyProcessedCategoryIds.Reverse();
            return alreadyProcessedCategoryIds;
        }
        
        //递归获取所有类别
        public static List<int> GetAllCategoriesRecursive(this Category category,
            ICategoryService categoryService)
        {
            var alreadyProcessedCategoryIds = new List<int>();

            alreadyProcessedCategoryIds.AddRange(GetAllChildsCategoriesRecursive(category, categoryService));
            while (category != null && //not null
                !category.IsDeleted && //not deleted
                category.Published && //published
                !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
            {
                alreadyProcessedCategoryIds.Add(category.Id);

                category = categoryService.GetCategoryById(category.ParentId);
            }



            alreadyProcessedCategoryIds.Reverse();
            return alreadyProcessedCategoryIds;
        }
        
        private static List<int> GetAllChildsCategoriesRecursive(this Category category,
            ICategoryService categoryService)
        {
            var alreadyProcessedCategoryIds = new List<int>();
            var childs = categoryService.GetCategoriesByParentId(category.Id);
            if (childs != null &&
                childs.Count > 0)
            {
                foreach (var child in childs)
                {
                    alreadyProcessedCategoryIds.Add(child.Id);
                    alreadyProcessedCategoryIds.AddRange(GetAllChildsCategoriesRecursive(child, categoryService));
                }

            }
            return alreadyProcessedCategoryIds;
        }

        /// <summary>
        /// 获取类别下的所有子类别
        /// </summary>
        /// <param name="category"></param>
        /// <param name="categoires"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public static List<Category> GetAllChildsCategoriesRecursive(this Category category,
             List<Category> categoires,bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var result = new List<Category>();
            var childs = categoires.Where(c => c.ParentId == category.Id &&
                                            (!showHidden || c.Published) &&
                                            !c.IsDeleted);

            return childs.ToList();
        }
    }
}
