using System.Collections.Generic;

namespace PersonalPortal.ResultModels.Models
{
    public class ShortCutModel
    {
        /// <summary>
        /// 标签页信息
        /// </summary>
        public List<ShortCutItem> ShortCutList { get; set; }
    }
    public class ShortCutItem
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        public string ShortCutIp { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string ShortCutName { get; set; }
        /// <summary>
        /// 标签链接
        /// </summary>
        public string ShortCutUrl { get; set; }
    }
}