using System.Collections.Generic;

namespace PersonalPortal.ResultModels.Models
{
    public class IndexModel
    {
        /// <summary>
        /// 导航栏链接
        /// </summary>
        public List<NavigationItem> Navigation { get; set; } = new List<NavigationItem>();
        public class NavigationItem
        {
            /// <summary>
            /// 链接名称
            /// </summary>
            public string LinkName { get; set; }
            /// <summary>
            /// 链接地址
            /// </summary>
            public string LinkUrl { get; set; }
            /// <summary>
            /// 导航栏链接
            /// </summary>
            public List<NavigationItem> Navigation { get; set; } = new List<NavigationItem>();
        }
    }
}