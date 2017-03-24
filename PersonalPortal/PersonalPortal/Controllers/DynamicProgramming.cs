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
        public AlgorithmModel EfficiencyTesting()
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
            var dp = new PersonalPortal.Content.Algorithm.DynamicProgramming();

            foreach (Solution solution in new Solution[] { new Solution(dp.Dp), new Solution(dp.Dp_Recursion), new Solution(dp.Devide) })
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
                    Result = result.ToString()
                });
            }
            return model;
        }
    }
}