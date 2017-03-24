using PersonalPortal.Content;
using PersonalPortal.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalPortal.Controllers
{
    public class Sort : ApiController
    {
        private delegate void Solution(int[] items);

        [HttpGet]
        [HttpPost]
        public AlgorithmModel EfficiencyTesting(int ArrayLength, int CycleIndex)
        {
            AlgorithmModel model = new AlgorithmModel();
            CycleIndex = CycleIndex == 0 ? 1000 : CycleIndex;
            const int resultCount = 1000;  //数据量过大时屏蔽结果以防止浏览器崩溃
            model.AlgorithmName = string.Format("排序算法在长度为{0}的数组下{1}次循环效率测试", ArrayLength, CycleIndex);
            int[] array = GetRandomArray(ArrayLength);
            model.DynamicProgrammingList.Add(new AlgorithmModel.Item()
            {
                Name = "原始数组",
                Result = string.Join(", ", ArrayLength > resultCount ? new int[0] : array)
            });
            List<Task> taskList = new List<Task>();
            foreach (Solution solution in new Solution[] {
                new Solution(Content.Algorithm.Sort.BubbleSort),
                new Solution(Content.Algorithm.Sort.StraightInsertionSort),
                new Solution(Content.Algorithm.Sort.ShellsSort),
                new Solution(Content.Algorithm.Sort.SimpleSelectionSort),
                new Solution(Content.Algorithm.Sort.BinarySelectionSort),
                new Solution(Content.Algorithm.Sort.MergeSort),
                new Solution(Content.Algorithm.Sort.QuickSort),
                new Solution(Content.Algorithm.Sort.ArraySort),
                new Solution(Content.Algorithm.Sort.ListSort)})
            {
                taskList.Add(Task.Run(() =>
                {
                    DateTime NowDate = DateTime.Now;
                    int[] result = array.ToArray();
                    for (int i = 0; i < CycleIndex; i++)
                    {
                        if (i == 0)
                        {
                            solution.Invoke(result);
                        }
                        else
                        {
                            solution.Invoke(array.ToArray());
                        }
                    }
                    model.DynamicProgrammingList.Add(new AlgorithmModel.Item()
                    {
                        Name = ((InfoAttribute)solution.Method.GetCustomAttributes(false).Where(x => x.GetType() == typeof(InfoAttribute)).First()).Name,
                        UsedTime = (DateTime.Now - NowDate).ToString("ssfff") + "毫秒",
                        Result = string.Join(", ", ArrayLength > resultCount ? new int[0] : result)
                    });
                }));
            }
            Task.WaitAll(taskList.ToArray());
            int tempTime;
            model.DynamicProgrammingList = model.DynamicProgrammingList.OrderBy(x => int.TryParse(x.UsedTime?.Replace("毫秒", ""), out tempTime) ? tempTime : 0).ToList();
            return model;
        }

        private int[] GetRandomArray(int length)
        {
            int[] array = new int[length];
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                array[i] = rand.Next(0, length * 10);
            }
            return array;
        }
    }
}