using Abp.Application.Services;

namespace ArtSolution.Media
{
    /// <summary>
    /// 阿里云图片服务
    /// </summary>
    public interface IOssService : IApplicationService
    {
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool DeleteImage(string fileName);

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="images"></param>
        /// <param name="isBuildThumbnail">是否生成缩略图</param>
        /// <returns></returns>
        string UploadImage(byte[] images, bool isBuildThumbnail = false, string upload = "");
    }
}
