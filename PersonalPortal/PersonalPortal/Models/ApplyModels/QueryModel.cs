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
        public string Url { get; set; }

        /// <summary>
        /// 请求Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string QueryType { get; set; }
    }
}