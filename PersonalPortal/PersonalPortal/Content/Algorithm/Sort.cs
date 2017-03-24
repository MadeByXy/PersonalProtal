using PersonalPortal.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalPortal.Content.Algorithm
{
    /// <summary>
    /// 排序
    /// </summary>
    public static class Sort
    {
        /// <summary>
        /// 冒泡排序
        /// </summary>
        [Info(Name = "冒泡排序")]
        public static void BubbleSort<T>(this T[] items) where T : IComparable
        {
            for (int i = 0; i < items.Length; i++)
            {
                for (int ii = 0; ii < items.Length - i - 1; ii++)
                {
                    if (items[ii].CompareTo(items[ii + 1]) > 0)
                    {
                        items.Exchange(ii, ii + 1);
                    }
                }
            }
        }

        /// <summary>
        /// 快速排序
        /// </summary>
        [Info(Name = "快速排序")]
        public static void QuickSort<T>(this T[] items) where T : IComparable
        {
            items.QuickSort(0, items.Length - 1);
        }

        /// <summary>
        /// 快速排序
        /// </summary>
        private static void QuickSort<T>(this T[] items, int start, int end) where T : IComparable
        {
            if (start >= end) { return; }
            int leftCursor = start, rightCursor = end;
            bool left = false;
            while (leftCursor < rightCursor)
            {
                left = !left;
                while (leftCursor < rightCursor && items[leftCursor].CompareTo(items[rightCursor]) <= 0)
                {
                    if (left) { rightCursor--; }
                    else { leftCursor++; }
                }
                items.Exchange(leftCursor, rightCursor);
            }
            items.QuickSort(start, leftCursor - 1);
            items.QuickSort(rightCursor + 1, end);
        }

        /// <summary>
        /// 直接插入排序
        /// </summary>
        [Info(Name = "直接插入排序")]
        public static void StraightInsertionSort<T>(this T[] items) where T : IComparable
        {
            List<T> tList = new List<T>();
            for (int i = 0; i < items.Length; i++)
            {
                int index = 0;
                while (index < i && items[i].CompareTo(tList[index]) > 0) { index++; }
                tList.Insert(index, items[i]);
            }
            for (int i = 0; i < tList.Count; i++)
            {
                items[i] = tList[i];
            }
        }

        /// <summary>
        /// 希尔排序
        /// </summary>
        [Info(Name = "希尔排序")]
        public static void ShellsSort<T>(this T[] items) where T : IComparable
        {
            int sequence = items.Length;
            while ((sequence /= 2) >= 1)
            {
                for (int i = 0; i < sequence; i++)
                {
                    List<T> tList = new List<T>();
                    for (int ii = i; ii < items.Length; ii += sequence)
                    {
                        tList.Add(items[ii]);
                    }
                    T[] temps = tList.ToArray();
                    temps.StraightInsertionSort();
                    int index = 0;
                    for (int ii = i; ii < items.Length; ii += sequence)
                    {
                        items[ii] = temps[index++];
                    }
                }
            }
        }

        /// <summary>
        /// 简单选择排序
        /// </summary>
        [Info(Name = "简单选择排序")]
        public static void SimpleSelectionSort<T>(this T[] items) where T : IComparable
        {
            for (int i = 0; i < items.Length; i++)
            {
                int index = i;
                T value = items[i];
                for (int ii = i; ii < items.Length; ii++)
                {
                    if (items[ii].CompareTo(value) < 0)
                    {
                        index = ii;
                        value = items[ii];
                    }
                }
                items.Exchange(i, index);
            }
        }

        /// <summary>
        /// 二元选择排序
        /// </summary>
        [Info(Name = "二元选择排序")]
        public static void BinarySelectionSort<T>(this T[] items) where T : IComparable
        {
            int left = 0, right = items.Length - 1;
            for (; left <= right; left++, right--)
            {
                int min = left, max = right;
                T minValue = items[left], maxValue = items[right];
                for (int ii = left; ii <= right; ii++)
                {
                    if (items[ii].CompareTo(minValue) <= 0)
                    {
                        min = ii;
                        minValue = items[ii];
                    }
                    if (items[ii].CompareTo(maxValue) > 0)
                    {
                        max = ii;
                        maxValue = items[ii];
                    }
                }
                items.Exchange(left, min);
                items.Exchange(right, max);
            }
        }

        /// <summary>
        /// 归并排序
        /// </summary>
        [Info(Name = "归并排序")]
        public static void MergeSort<T>(this T[] items) where T : IComparable
        {
            if (items.Length > 0)
            {
                var itemList = items.Select(x => new List<T> { x }).ToList();
                while (itemList.Count > 1)
                {
                    for (int i = 0; i < itemList.Count / 2; i++)
                    {
                        itemList[i].AddRange(itemList[i + 1]);
                        itemList[i].Sort();
                        itemList.RemoveAt(i + 1);
                    }
                }
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = itemList[0][i];
                }
            }
        }

        /// <summary>
        /// List内置排序
        /// </summary>
        [Info(Name = "List内置排序")]
        public static void ListSort<T>(this T[] items) where T : IComparable
        {
            List<T> itemList = items.ToList();
            itemList.Sort();
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = itemList[i];
            }
        }

        /// <summary>
        /// Array内置排序
        /// </summary>
        [Info(Name = "Array内置排序")]
        public static void ArraySort<T>(this T[] items) where T : IComparable
        {
            Array.Sort(items);
        }

        /// <summary>
        /// 交换数组元素位置
        /// </summary>
        public static void Exchange<T>(this T[] items, int a, int b)
        {
            if (a != b)
            {
                T temp = items[a];
                items[a] = items[b];
                items[b] = temp;
            }
        }
    }
}