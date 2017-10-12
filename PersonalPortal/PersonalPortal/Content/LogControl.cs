using System;
using System.IO;
using System.Text;

namespace PersonalPortal
{
    /// <summary>
    /// 日志记录工具
    /// </summary>
    public class LogControl
    {
        private FileInfo ErrorFile;

        /// <summary>
        /// <see cref="LogControl"/>的实例
        /// </summary>
        /// <param name="controllerName">日志名称</param>
        public LogControl(string controllerName)
        {
            string dicPath = AppDomain.CurrentDomain.BaseDirectory + @"\ErrorLog\";
            DirectoryInfo dic = new DirectoryInfo(dicPath);
            if (!dic.Exists)
                dic.Create();
            ErrorFile = new FileInfo(dicPath + controllerName + ".txt");
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="args">日志内容(格式化参数)</param>
        public void WriteLog(string text, params object[] args)
        {
            lock (ErrorFile)
            {
                FileStream fileStream;
                if (ErrorFile.CreationTime < DateTime.Now.AddDays(-10))
                {
                    ErrorFile.Delete();
                    fileStream = ErrorFile.Create();
                    ErrorFile.Refresh();
                }
                else
                {
                    fileStream = ErrorFile.Open(FileMode.Open);
                    new StreamReader(fileStream, Encoding.UTF8).ReadToEnd();
                }
                try
                {
                    string originalText = string.Format(text, args ?? new object[0]) + "\r\n";
                    byte[] bytes = Encoding.UTF8.GetBytes(originalText);
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Flush();
                }
                catch { throw; }
                finally
                {
                    fileStream.Close();
                }
            }
        }
    }
}