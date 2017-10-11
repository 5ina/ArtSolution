using System.ComponentModel;

namespace ArtSolution.Domain.Common
{
    /// <summary>
    /// 选项
    /// </summary>
    public enum AttributeType : int
    {
        /// <summary>
        /// 选项
        /// </summary>
        [Description("选择")]
        Option = 0,
        /// <summary>
        /// 自定义文本
        /// </summary>
        [Description("文本")]
        CustomText = 10,
    }
}
