using System.ComponentModel;

namespace ArtSolution
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditStatus :int
    {
        [Description("未处理")]
        None  = 0 ,

        [Description("审核通过")]
        Audited = 5,

        [Description("审核失败")]
        Lose = -1,
    }
}
