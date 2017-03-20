using System;
using System.IO;
using System.Text;

namespace PersonalPortal
{
    public class LogControl
    {
        private FileInfo ErrorFile;

        public LogControl(string controllerName)
        {
            string dicPath = AppDomain.CurrentDomain.BaseDirectory + @"\ErrorLog\";
            DirectoryInfo dic = new DirectoryInfo(dicPath);
            if (!dic.Exists)
                dic.Create();
            ErrorFile = new FileInfo(dicPath + controllerName + ".txt");
        }

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
                string originalText = string.Format(text, args) + "\r\n";
                byte[] bytes = Encoding.UTF8.GetBytes(originalText);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Flush();
                fileStream.Close();
            }
        }
    }
}