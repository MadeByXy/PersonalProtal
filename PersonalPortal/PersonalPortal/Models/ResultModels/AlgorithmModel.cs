using System;
using System.Collections.Generic;

namespace PersonalPortal.Models.ResultModels
{
    public class InfoAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 算法测试结果模型
    /// </summary>
    public class AlgorithmModel
    {
        /// <summary>
        /// 测试算法名称
        /// </summary>
        public string AlgorithmName { get; set; }
        public List<Item> DynamicProgrammingList { get; set; } = new List<Item>();
        public class Item
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 用时
            /// </summary>
            public string UsedTime { get; set; }
            /// <summary>
            /// 结果
            /// </summary>
            public string Result { get; set; }
        }
    }
}