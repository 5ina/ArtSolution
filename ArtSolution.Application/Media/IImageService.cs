using Abp.Application.Services;

namespace ArtSolution.Media
{
    public interface IImageService: IApplicationService
    {
        /// <summary>
        /// 新增图片
        /// </summary>
        /// <param name="images"></param>
        /// <param name="isBuildThumbnail"></param>
        /// <returns></returns>
        string UploadImage(byte[] images, string path = "Upload", bool isBuildThumbnail = false);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool DeleteImage(string url);

        /// <summary>
        /// 移除图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool RemoveImage(string path);
    }
}
