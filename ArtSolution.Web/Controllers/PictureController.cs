using Abp.Web.Security.AntiForgery;
using ArtSolution.Common;
using ArtSolution.Media;
using ArtSolution.Names;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Controllers
{
    public class PictureController : ArtSolutionControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IOssService _ossService;
        private readonly ISettingService _settingService;

        public PictureController(IImageService imageService,
            IOssService ossService,
            ISettingService settingService)
        {
            this._imageService = imageService;
            this._ossService = ossService;
            this._settingService = settingService;
        }


        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult AsyncUploadImage(string imageName)
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request[imageName]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("文件不存在");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla-Request	{System.Web.HttpRequestWrapper}	System.Web.HttpRequestBase {System.Web.HttpRequestWrapper}

                stream = Request.InputStream;
                fileName = Request[imageName];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            var isLocalStorage = _settingService.GetSettingByKey<bool>(MediaSettingNames.IsLocalStorage);
            var url = string.Empty;
            if (isLocalStorage)
                url = _imageService.UploadImage(images: fileBinary, path: "Loan", isBuildThumbnail: fileBinary.Length > 30000);
            else
                url = _ossService.UploadImage(images: fileBinary, isBuildThumbnail: false, upload: "loan");


            return Json(new
            {
                success = true,
                Url = url,
            });
        }
    }
}