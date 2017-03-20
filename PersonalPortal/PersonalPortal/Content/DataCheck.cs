﻿using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace PersonalPortal.Content
{
    /// <summary>
    /// 数据合法性验证
    /// </summary>
    public static class DataCheck
    {
        /// <summary>
        /// 参数防注入
        /// </summary>
        /// <param name="text">数据</param>
        /// <returns></returns>
        public static string SqlDefense(string text)
        {
            try
            {
                text = HttpUtility.UrlDecode(text);
                text = Regex.Replace(text, @"^'|([^\\])'", @"$1\'");
                return text;
            }
            catch
            {
                throw new Exception("参数解密失败");
            }
        }

        /// <summary>
        /// 验证数据是否合法
        /// </summary>
        /// <param name="parameter">数据</param>
        /// <param name="resultMessage">提示信息</param>
        /// <returns>验证结果</returns>
        public static bool AttributeCheck(object parameter, out string resultMessage)
        {
            resultMessage = "";
            if (parameter == null)
            {
                return true;
            }
            else
            {
                foreach (PropertyInfo property in parameter.GetType().GetProperties())
                {
                    foreach (Attribute checkAttribute in Attribute.GetCustomAttributes(property))
                    {
                        if (!((ICheck)checkAttribute).Check(property.GetValue(parameter, null), ref resultMessage))
                        {
                            return false;
                        }
                    }
                }
                resultMessage = "sucess";
                return true;
            }
        }

        private interface ICheck
        {
            /// <summary>
            /// 字段名称
            /// </summary>
            string Name { get; set; }

            /// <summary>
            /// 验证数据是否合法
            /// </summary>
            /// <param name="value">值</param>
            /// <param name="resultMessage">错误信息</param>
            /// <returns></returns>
            bool Check(object value, ref string resultMessage);
        }

        /// <summary>
        /// 数字区间检测
        /// </summary>
        public class NumberIntervalAttribute : Attribute, ICheck
        {
            private string name;
            private double min, max;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public double Min
            {
                get { return min; }
                set { min = value; }
            }
            public double Max
            {
                get { return max; }
                set { max = value; }
            }

            public bool Check(object value, ref string resultMessage)
            {
                double realValue = double.Parse(value.ToString());
                if (value == null || (Min <= realValue && Max >= realValue))
                {
                    return true;
                }
                else
                {
                    resultMessage = string.Format("{0}超出范围", Name);
                    return false;
                }
            }
        }

        /// <summary>
        /// 时间区间检测
        /// </summary>
        public class DateIntervalAttribute : Attribute, ICheck
        {
            private string earliest, latest, name;
            private DateTime rarliestDate, latestDate;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            private DateTime RarliestDate
            {
                get { return rarliestDate; }
                set { rarliestDate = value; }
            }
            private DateTime LatestDate
            {
                get { return latestDate; }
                set { latestDate = value; }
            }
            public string Earliest
            {
                get { return earliest; }
                set
                {
                    earliest = value;
                    RarliestDate = DateTime.Parse(earliest);
                }
            }
            public string Latest
            {
                get { return latest; }
                set
                {
                    latest = value;
                    LatestDate = DateTime.Parse(latest);
                }
            }

            public bool Check(object value, ref string resultMessage)
            {
                if (value == null || (RarliestDate <= (DateTime)value && LatestDate >= (DateTime)value))
                {
                    return true;
                }
                else
                {
                    resultMessage = string.Format("{0}超出范围", Name);
                    return false;
                }
            }
        }

        /// <summary>
        /// 时间格式检测
        /// </summary>
        public class DateAttribute : Attribute, ICheck
        {
            private string name;
            private string[] format;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            /// <summary>
            /// 允许的时间格式
            /// </summary>
            public string[] Format
            {
                get { return format; }
                set { format = value; }
            }

            public bool Check(object value, ref string resultMessage)
            {
                try
                {
                    if (value != null)
                    {
                        DateTime.ParseExact(value.ToString(), Format, null, DateTimeStyles.None);
                    }
                    return true;
                }
                catch
                {
                    resultMessage = string.Format("{0}格式错误", Name);
                    return false;
                }
            }
        }

        /// <summary>
        /// Email格式检测
        /// </summary>
        public class EmailAttribute : Attribute, ICheck
        {
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public bool Check(object value, ref string resultMessage)
            {
                if (value == null || Regex.IsMatch(value.ToString(), @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?"))
                {
                    return true;
                }
                else
                {
                    resultMessage = string.Format("{0}格式错误", Name);
                    return false;
                }
            }
        }

        /// <summary>
        /// 枚举类型检测
        /// </summary>
        public class EnumAttribute : Attribute, ICheck
        {
            private string name;
            private Type enumType;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public Type EnumType
            {
                get { return enumType; }
                set { enumType = value; }
            }
            public bool Check(object value, ref string resultMessage)
            {
                if (value == null || (value.GetType() == typeof(string) && string.IsNullOrEmpty(value.ToString())) || Enum.IsDefined(EnumType, value))
                {
                    return true;
                }
                else
                {
                    resultMessage = string.Format("{0}格式错误", Name);
                    return false;
                }
            }
        }

        /// <summary>
        /// 非空检测
        /// </summary>
        public class NonEmptyAttribute : Attribute, ICheck
        {
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public bool Check(object value, ref string resultMessage)
            {
                if (value != null && value != DataConversion.DefaultValue(value.GetType()))
                {
                    return true;
                }
                else
                {
                    resultMessage = string.Format("{0}不能为空", Name);
                    return false;
                }
            }
        }
    }
}