using Newtonsoft.Json.Linq;
using PersonalPortal.Models.Enum;
using static PersonalPortal.Content.DataCheck;

namespace PersonalPortal.Models.ApplyModels
{
    /// <summary>
    /// 请求Api模型
    /// </summary>
    public class QueryModel
    {
        /// <summary>
        /// 请求Api地址
        /// </summary>
        [NonEmpty(Name = "请求Api地址")]
        public string Url { get; set; }

        /// <summary>
        /// 请求Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public JObject Headers { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public QueryTypeEnum QueryType { get; set; }
    }
}