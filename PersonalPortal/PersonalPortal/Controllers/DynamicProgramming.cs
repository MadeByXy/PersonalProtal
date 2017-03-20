using PersonalPortal.Content;
using PersonalPortal.Models.ApplyModels;
using PersonalPortal.Models.ResultModels;
using System;
using System.Collections.Generic;

namespace PersonalPortal.Controllers
{
    public class DynamicProgramming : ApiController
    {
        delegate int Solution(DynamicProgrammingView data);

        /// <summary>
        /// 01背包算法测试
        /// </summary>
        [HttpGet]
        [HttpPost]
        public AlgorithmModel AlgorithmTesting()
        {
            DynamicProgrammingView data = new DynamicProgrammingView()
            {
                Capacity = 10,
                DynamicProgrammingList = new List<DynamicProgrammingView.Item>()
                {
                    new DynamicProgrammingView.Item() { ItemName="A", Value=6, Weight=2 },
                    new DynamicProgrammingView.Item() { ItemName="B", Value=3, Weight=2 },
                    new DynamicProgrammingView.Item() { ItemName="C", Value=5, Weight=6 },
                    new DynamicProgrammingView.Item() { ItemName="D", Value=4, Weight=5 },
                    new DynamicProgrammingView.Item() { ItemName="E", Value=6, Weight=4 }
                }
            };
            AlgorithmModel model = new AlgorithmModel();
            model.AlgorithmName = "01背包算法效率测试";
            DateTime NowDate;
            foreach (Solution solution in new Solution[] { new Solution(Dp), new Solution(Dp_Recursion), new Solution(Devide) })
            {
                NowDate = DateTime.Now;
                int result = 0;
                for (int i = 0; i <= 100000; i++)
                {
                    result = solution.Invoke(data);
                }
                model.DynamicProgrammingList.Add(new AlgorithmModel.Item()
                {
                    Name = ((InfoAttribute)solution.Method.GetCustomAttributes(false)[0]).Name,
                    UsedTime = (DateTime.Now - NowDate).Milliseconds + "毫秒",
                    Result = result
                });
            }
            return model;
        }

        #region 动态规划
        [Info(Name = "动态规划")]
        private int Dp(DynamicProgrammingView data)
        {
            int[,] resultArray = new int[data.DynamicProgrammingList.Count, data.Capacity];
            resultArray[0, 0] = 0;
            for (int capacity = 0; capacity < data.Capacity; capacity++)
            {
                for (int index = 0; index < data.DynamicProgrammingList.Count; index++)
                {
                    DynamicProgrammingView.Item item = data.DynamicProgrammingList[index];
                    resultArray[index, capacity] = Math.Max(resultArray[Math.Max(index - 1, 0), capacity],
                        capacity >= item.Weight ? (index > 0 ? (resultArray[index - 1, capacity - item.Weight] + item.Value) : item.Value) : 0);
                }
            }
            return resultArray[data.DynamicProgrammingList.Count - 1, data.Capacity - 1];
        }
        #endregion

        #region 动态规划(递归)
        int[,] ValueArray;
        [Info(Name = "动态规划(递归)")]
        private int Dp_Recursion(DynamicProgrammingView data)
        {
            ValueArray = new int[data.DynamicProgrammingList.Count, data.Capacity];
            return Dp_Recursion(data, data.DynamicProgrammingList.Count - 1, data.Capacity - 1);
        }

        private int Dp_Recursion(DynamicProgrammingView data, int index, int capacity)
        {
            if (index < 0 || capacity < 0)
            {
                return 0;
            }
            if (ValueArray[index, capacity] == 0)
            {
                DynamicProgrammingView.Item item = data.DynamicProgrammingList[index];
                ValueArray[index, capacity] = Math.Max(Dp_Recursion(data, index - 1, capacity),
                    capacity >= item.Weight ? (Dp_Recursion(data, index - 1, capacity - item.Weight) + item.Value) : 0);
            }
            return ValueArray[index, capacity];
        }
        #endregion

        #region 分治
        [Info(Name = "分治")]
        private int Devide(DynamicProgrammingView data)
        {
            return Devide(data, data.DynamicProgrammingList.Count - 1, data.Capacity - 1);
        }

        private int Devide(DynamicProgrammingView data, int index, int capacity)
        {
            if (index < 0 || capacity < 0)
            {
                return 0;
            }
            DynamicProgrammingView.Item item = data.DynamicProgrammingList[index];
            return Math.Max(Devide(data, index - 1, capacity),
                capacity >= item.Weight ? (Devide(data, index - 1, capacity - item.Weight) + item.Value) : 0);
        }
        #endregion

        private class InfoAttribute : Attribute
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
        }
    }
}