using Abp.Web.Security.AntiForgery;
using ArtSolution.Common;
using ArtSolution.Media;
using ArtSolution.Names;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{

    public class PictureController : ArtSolutionControllerAdminBase
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
        public ActionResult AsyncUploadImage()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["image"]))
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
                fileName = Request["image"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            var isLocalStorage = _settingService.GetSettingByKey<bool>(MediaSettingNames.IsLocalStorage);
            var url = string.Empty;
            if (isLocalStorage)
                url = _imageService.UploadImage(images: fileBinary, isBuildThumbnail: fileBinary.Length > 30000);
            else
                url = _ossService.UploadImage(images: fileBinary, isBuildThumbnail: fileBinary.Length > 30000);


            return Json(new
            {
                success = true,
                Url = url,
            });
        }

        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult AsyncUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla-		Request	{System.Web.HttpRequestWrapper}	System.Web.HttpRequestBase {System.Web.HttpRequestWrapper}

                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            //var url = _imageService.UploadImage(images: fileBinary, isBuildThumbnail: true);

            var isLocalStorage = _settingService.GetSettingByKey<bool>(MediaSettingNames.IsLocalStorage);
            var url = string.Empty;
            if (isLocalStorage)
                url = _imageService.UploadImage(images: fileBinary, isBuildThumbnail: true);
            else
                url = _ossService.UploadImage(images: fileBinary, isBuildThumbnail: true);

            return Json(new
            {
                success = true,
                Url = url,
            });
        }

        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult AsyncCkeditorUpload()
        {
            var file = Request.Files[0];

            Stream stream = file.InputStream;
            var fileExtension = Path.GetExtension(file.FileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var isLocalStorage = _settingService.GetSettingByKey<bool>(MediaSettingNames.IsLocalStorage);
            var url = string.Empty;
            if (isLocalStorage)
                url = _imageService.UploadImage(images: fileBinary, path: "Upload/Rich", isBuildThumbnail: true);
            else
                url = _ossService.UploadImage(images: fileBinary, isBuildThumbnail: true);
            
            string script = @"<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}');</script>";

            Response.Write(string.Format(script, Request.QueryString["CKEditorFuncNum"], url, ""));
            return null;
        }
    }
}