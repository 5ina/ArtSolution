using System.Collections.Generic;
using ArtSolution.Domain.Catalog;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using ArtSolution.Media;
using System;

namespace ArtSolution.Catalog
{
    public class ProductImagesService : ArtSolutionAppServiceBase, IProductImagesService
    {


        #region Ctor && Field

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductImage> _imageRepository;
        private readonly IImageService _imageService;

        public ProductImagesService(IRepository<Product> productRepository,
                                    IRepository<ProductImage> imageRepository,
                                    IImageService imageService)
        {
            this._productRepository = productRepository;
            this._imageRepository = imageRepository;
            this._imageService = imageService;
        }
        #endregion

        #region Method
        public void ClearImages(int productId)
        {
            _imageRepository.Delete(i => i.ProductId == productId);
        }

        public void DeleteImage(int imageId)
        {
            _imageRepository.Delete(imageId);
        }

        
        public void InsertImage(ProductImage image)
        {            
            _imageRepository.Insert(image);
        }

        public ProductImage GetProductImageById(int imageId)
        {
            if (imageId > 0)
                return _imageRepository.Get(imageId);
            return null;
        }

        public IList<ProductImage> GetProductImagesByProductId(int productId)
        {
            return _imageRepository.GetAllList(i => i.ProductId == productId);
        }
        #endregion
    }
}
