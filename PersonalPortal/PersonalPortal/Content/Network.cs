using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using PersonalPortal.Models.ApplyModels;

namespace PersonalPortal
{
    /// <summary>
    /// 网络工具
    /// </summary>
    public class Network
    {
        public enum HttpMethod
        {
            GET,
            POST
        }
        public enum SubmitMethod
        {
            FORM,
            JSON
        }

        /// <summary>
        /// 从指定Url获取HTML
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方式</param>
        /// <param name="submitMethod">参数提交方式</param>
        /// <returns>页面返回值</returns>
        public static string GetHtml(string url, Dictionary<string, string> parameters, HttpMethod method = HttpMethod.GET, SubmitMethod submitMethod = SubmitMethod.FORM)
        {
            HttpWebRequest request;
            parameters = parameters ?? new Dictionary<string, string>();
            string param = string.Join("&", parameters.Select(x => string.Format("{0}={1}", x.Key, HttpUtility.HtmlEncode(x.Value.Replace("\r\n", "")))));
            switch (method)
            {
                case HttpMethod.GET:
                    if (param != "")
                    {
                        url += "?" + param;
                    }
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = method.ToString();
                    request.ContentType = "application/x-www-form-urlencoded";
                    break;
                case HttpMethod.POST:
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = method.ToString();
                    string postParam = param;
                    switch (submitMethod)
                    {
                        case SubmitMethod.FORM:
                            request.ContentType = "application/x-www-form-urlencoded";
                            break;
                        case SubmitMethod.JSON:
                            request.ContentType = "application/json";
                            JObject jObject = new JObject();
                            foreach (string key in parameters.Keys)
                            {
                                jObject.Add(key, parameters[key]);
                            }
                            postParam = jObject.ToString();
                            break;
                    }
                    byte[] requestBytes = Encoding.UTF8.GetBytes(postParam);
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                    requestStream.Close();
                    break;
                default:
                    throw new Exception();
            }

            request.Headers["charset"] = "UTF-8";
            request.Accept = "application/json";
            request.Headers["X-Requested-With"] = "XMLHttpRequest";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";
            request.Headers["Cookie"] = param;
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Date = DateTime.Now;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(),
                Encoding.GetEncoding(string.IsNullOrWhiteSpace(response.CharacterSet) ? "UTF-8" : response.CharacterSet)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 从指定Url获取HTML
        /// </summary>
        /// <param name="query">请求参数</param>
        /// <returns></returns>
        public static HttpWebResponse GetHtml(QueryModel query)
        {

            HttpWebRequest request;
            if (!string.IsNullOrEmpty(query.Body) && query.QueryType == "GET")
            {
                query.Url += "?" + query.Body;
            }
            request = WebRequest.Create(query.Url) as HttpWebRequest;
            request.Headers.Clear();
            request.Method = query.QueryType;

            foreach (var jToken in query.Headers)
            {
                string key = jToken.Key?.ToString()?.Trim(), value = jToken.Value?.ToString()?.Trim();
                switch (key?.ToLower())
                {
                    case null: case "": break;
                    case "accept": request.Accept = value; break;
                    case "connection": request.Connection = value; break;
                    case "content-length": request.ContentLength = Convert.ToInt64(value); break;
                    case "content-type": request.ContentType = value; break;
                    case "expect": request.Expect = value; break;
                    case "date": request.Date = Convert.ToDateTime(value); break;
                    case "if-modified-since": request.IfModifiedSince = Convert.ToDateTime(value); break;
                    case "referer": request.Referer = value; break;
                    case "transfer-encoding": request.TransferEncoding = value; break;
                    case "user-agent": request.UserAgent = value; break;
                    case "host": request.Host = value; break;
                    default: request.Headers.Add(key, value); break;
                }
            }

            switch (query.QueryType)
            {
                case "POST":
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(query.Body)))
                    {
                        using (Stream stream = request.GetRequestStream())
                        {
                            memoryStream.WriteTo(stream);
                        }
                    }
                    break;
                default: break;
            }

            try
            {
                return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw e;
                }
                else
                {
                    return e.Response as HttpWebResponse;
                }
            }
        }
    }
}
