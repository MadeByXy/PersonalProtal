﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

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
        /// 获取HTML
        /// </summary>
        /// <returns></returns>
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
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(response.CharacterSet)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
