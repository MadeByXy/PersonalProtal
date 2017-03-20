namespace PersonalPortal.ResultModels.Models
{
    public class ErrorModel
    {
        private string message, messageDetail;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        /// <summary>
        /// 详细错误信息
        /// </summary>
        public string MessageDetail
        {
            get { return messageDetail; }
            set { messageDetail = value; }
        }
    }
}