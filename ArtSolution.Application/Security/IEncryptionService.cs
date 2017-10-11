using Abp.Application.Services;

namespace ArtSolution.Security
{
    /// <summary>
    /// 加解密服务
    /// </summary>
    public interface IEncryptionService: IApplicationService
    {
        /// <summary>
        /// 创建秘钥键
        /// </summary>
        /// <param name="size">长度</param>
        /// <returns>Salt key</returns>
        string CreateSaltKey(int size);

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="saltkey">秘钥键</param>
        /// <param name="passwordFormat">密码加密方式，散列算法</param>
        /// <returns>Password hash</returns>
        string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1");

        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="plainText">文本</param>
        /// <param name="encryptionPrivateKey">加密密钥</param>
        /// <returns>加密后的文本</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cipherText">加密后的文本</param>
        /// <param name="encryptionPrivateKey">加密秘钥</param>
        /// <returns>解密后的文本</returns>
        string DecryptText(string cipherText, string encryptionPrivateKey = "");
    }
}
