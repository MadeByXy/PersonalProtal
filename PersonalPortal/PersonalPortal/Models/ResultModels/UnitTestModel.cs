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
            /// 方法全名
            /// </summary>
            public string FullName { get; set; }
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

        /// <summary>
        /// 测试结果列表
        /// </summary>
        public class Test
        {
            /// <summary>
            /// 用时（毫秒）
            /// </summary>
            public long UseTime { get; set; }
            /// <summary>
            /// 是否正确执行完成
            /// </summary>
            public bool IsOver { get; set; }
            /// <summary>
            /// 返回结果
            /// </summary>
            public string Result { get; set; }
            /// <summary>
            /// 建议信息
            /// </summary>
            public List<string> Info { get; set; } = new List<string>();
            /// <summary>
            /// 警告信息
            /// </summary>
            public List<string> Warning { get; set; } = new List<string>();
            /// <summary>
            /// 错误信息
            /// </summary>
            public List<string> Error { get; set; } = new List<string>();
        }
    }
}