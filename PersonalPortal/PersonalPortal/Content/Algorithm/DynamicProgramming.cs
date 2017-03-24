using PersonalPortal.Models.ApplyModels;
using PersonalPortal.Models.ResultModels;
using System;

namespace PersonalPortal.Content.Algorithm
{
    /// <summary>
    /// 动态规划算法
    /// </summary>
    public class DynamicProgramming
    {
        #region 动态规划
        [Info(Name = "动态规划")]
        public int Dp(DynamicProgrammingView data)
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
        public int Dp_Recursion(DynamicProgrammingView data)
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
        public int Devide(DynamicProgrammingView data)
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
    }
}