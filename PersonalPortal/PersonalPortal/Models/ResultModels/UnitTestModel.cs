using System;
using System.Collections;
using System.Collections.Generic;

namespace PersonalPortal.Models.ResultModels
{
    public class UnitTestModel
    {
        /// <summary>
        /// 说明模型
        /// </summary>
        public class Comment
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 说明
            /// </summary>
            public string Comments { get; set; }
        }

        /// <summary>
        /// 命名空间模型
        /// </summary>
        public class NameSpace : Comment, IEqualityComparer<NameSpace>
        {
            /// <summary>
            /// 命名空间列表
            /// </summary>
            public List<NameSpace> NameSpaces { get; set; } = new List<NameSpace>();

            public bool Equals(NameSpace x, NameSpace y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(NameSpace obj)
            {
                return base.GetHashCode();
            }
        }

        /// <summary>
        /// 方法模型
        /// </summary>
        public class Method : Comment
        {
            /// <summary>
            /// 结果类型
            /// </summary>
            public string ReturnType { get; set; }
            /// <summary>
            /// 结果说明
            /// </summary>
            public string ReturnComments { get; set; }
            /// <summary>
            /// 参数列表
            /// </summary>
            public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        }

        /// <summary>
        /// 参数列表
        /// </summary>
        public class Parameter : Comment
        {
            /// <summary>
            /// 参数类型
            /// </summary>
            public string ParameterType { get; set; }
        }
    }
}