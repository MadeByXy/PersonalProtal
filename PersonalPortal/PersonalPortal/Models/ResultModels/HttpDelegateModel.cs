using System.Net;

namespace PersonalPortal.Models.ResultModels
{
    /// <summary>
    /// Http委托返回实体
    /// </summary>
    public class HttpDelegateModel
    {
        /// <summary>
        /// Body数据
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Hearder数据
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Http响应码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Http响应信息
        /// </summary>
        public string StatusMessage { get; set; }
    }
}