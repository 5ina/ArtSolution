namespace ArtSolution.Web.Models.WeChat
{
    public enum WeChatEvent
    {
        /// <summary>
        /// 非事件类型
        /// </summary>
        NOEVENT,
        /// <summary>
        /// 订阅
        /// </summary>
        SUBSCRIBE,
        /// <summary>
        /// 取消订阅
        /// </summary>
        UNSUBSCRIBE,
        /// <summary>
        /// 扫描带参数的二维码
        /// </summary>
        SCAN,
        /// <summary>
        /// 地理位置
        /// </summary>
        LOCATION,
        /// <summary>
        /// 单击按钮
        /// </summary>
        CLICK,
        /// <summary>
        /// 链接按钮
        /// </summary>
        VIEW,
        /// <summary>
        /// 扫码推事件
        /// </summary>
        SCANCODE_PUSH,
        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        SCANCODE_WAITMSG,
        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        PIC_SYSPHOTO,
        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        PIC_PHOTO_OR_ALBUM,
        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        PIC_WEIXIN,
        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        LOCATION_SELECT,
        /// <summary>
        /// 模板消息推送
        /// </summary>
        TEMPLATESENDJOBFINISH
    }
}