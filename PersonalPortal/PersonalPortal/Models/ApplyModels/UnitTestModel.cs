using Newtonsoft.Json.Linq;

namespace PersonalPortal.Models.ApplyModels
{
    public class UnitTestModel
    {
        /// <summary>
        /// 测试请求参数
        /// </summary>
        public class TestModel
        {
            /// <summary>
            /// 程序集名称
            /// </summary>
            public string AssemblyName { get; set; }
            /// <summary>
            /// 命名空间
            /// </summary>
            public string NameSpaceName { get; set; }
            /// <summary>
            /// 类名称
            /// </summary>
            public string ClassName { get; set; }
            /// <summary>
            /// 类名称(带参数)
            /// </summary>
            public string ClassFullName { get; set; }
            /// <summary>
            /// 方法名称(带参数)
            /// </summary>
            public string MethodFullName { get; set; }
            /// <summary>
            /// 类实例化参数
            /// </summary>
            public JObject ClassParameters { get; set; }
            /// <summary>
            /// 方法参数集
            /// </summary>
            public JArray MethodParameters { get; set; }
            /// <summary>
            /// 并发数量
            /// </summary>
            public int Concurrent { get; set; }
        }
    }
}