using System.Collections.Generic;
using static PersonalPortal.Content.DataCheck;

namespace PersonalPortal.Models.ApplyModels
{
    /// <summary>
    /// 01背包参数模型
    /// </summary>
    public class DynamicProgrammingView
    {
        [NonEmpty(Name = "背包容量")]
        public int Capacity { get; set; }

        /// <summary>
        /// 背包信息
        /// </summary>
        public List<Item> DynamicProgrammingList { get; set; }
        public class Item
        {
            /// <summary>
            /// 物品名称
            /// </summary>
            public string ItemName { get; set; }

            [NonEmpty(Name = "物品重量")]
            [NumberInterval(Min = 1, Name = "物品重量")]
            public int Weight { get; set; }

            [NonEmpty(Name = "物品价值")]
            [NumberInterval(Min = 1, Name = "物品价值")]
            public int Value { get; set; }
        }
    }
}