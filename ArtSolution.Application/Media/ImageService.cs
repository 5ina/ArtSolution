using System;
using System.IO;
using System.Web.Hosting;

namespace ArtSolution.Media
{
    public class ImageService : ArtSolutionAppServiceBase, IImageService
    {
        
        #region Utilities
        private string GetImageName(string suffix = ".jgp")
        {
            var name = DateTime.Now.ToString("yyyyMMddhhmmssfff") + CommonHelper.GenerateCode(3);
            return string.Format("{0}{1}", name, suffix);
        }
        #endregion

        #region Method
        public bool DeleteImage(string url)
        {
            try
            {
                if (File.Exists(HostingEnvironment.MapPath(url)))
                    File.Delete(HostingEnvironment.MapPath(url));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string UploadImage(byte[] images,string path = "Upload" ,bool isBuildThumbnail = false)
        {
            string fileName = string.Format("{0}.jpg", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            string savePath = string.Format("/{0}/{1}/", path, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(HostingEnvironment.MapPath(savePath));
            File.WriteAllBytes(HostingEnvironment.MapPath(savePath + fileName), images);
            return string.Format("{0}{1}", savePath, fileName);
        }

        public bool RemoveImage(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            { return false; }
        }

        #endregion
    }
}
