using PersonalPortal.Content;
using PersonalPortal.ResultModels.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PersonalPortal
{
    /// <summary>
    /// Controllers 的摘要说明
    /// </summary>
    public class Controller : IHttpHandler
    {
        /// <summary>
        /// 控制器集合
        /// </summary>
        public static Dictionary<string, Type> ControllerDic = GetController();

        /// <summary>
        /// 控制器地址请求名称
        /// </summary>
        private const string ControllerKey = "Controller";

        private enum ReturnTypeEnum
        {
            Json,
            Xml
        }

        public void ProcessRequest(HttpContext context)
        {
            //获取结果返回类型

            ReturnTypeEnum returnType = ReturnTypeEnum.Json;
            context.Response.ContentType = "application/json;";
            foreach (string mime in context.Request.AcceptTypes)
            {
                if (mime.Contains("application/xml"))
                {
                    returnType = ReturnTypeEnum.Xml;
                    context.Response.ContentType = "application/xml;";
                }
            }

            //解决Ajax跨域问题
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            foreach (string item in new string[] { "GET", "POST", "OPTIONS" })
            {
                context.Response.Headers.Add("Access-Control-Allow-Methods", item);
            }
            context.Response.Headers.Add("Access-Control-Allow-Headers", "content-type");

            string result;
            context.Response.StatusCode = GetResult(context, returnType, out result);
            context.Response.Write(result);
            context.Response.End();
            return;
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        private static Dictionary<string, object> GetParams(HttpContext context, string postParams)
        {
            Dictionary<string, object> paramsDic = new Dictionary<string, object>();
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    foreach (string key in context.Request.QueryString.AllKeys)
                    {
                        if (!string.IsNullOrEmpty(key) && key != ControllerKey)
                        {
                            paramsDic.Add(key, DataCheck.SqlDefense(context.Request.QueryString[key]));
                        }
                    }
                    break;
                case "POST":
                    foreach (string param in postParams.Split('&'))
                    {
                        if (!string.IsNullOrEmpty(param))
                        {
                            string[] array = param.Split('=');
                            if (array.Length == 2)
                            {
                                string key = array[0], value = array[1];
                                if (key != ControllerKey)
                                {
                                    if (paramsDic.ContainsKey(key))
                                    {
                                        paramsDic[key] += "," + DataCheck.SqlDefense(value);
                                    }
                                    else
                                    {
                                        paramsDic.Add(key, DataCheck.SqlDefense(value));
                                    }
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return paramsDic;
        }

        /// <summary>
        /// 获取请求结果
        /// </summary>
        /// <returns>HTTP状态码</returns>
        private static int GetResult(HttpContext context, ReturnTypeEnum returnType, out string result)
        {
            //获取请求地址
            string[] requestUrl = context.Request.QueryString[ControllerKey]?.Split('/');

            result = "";
            if (requestUrl == null || requestUrl.Length < 2)
            {
                result = GetResult(
                    GetErrorModel("找不到与请求 URI匹配的 HTTP 资源。", "请求的格式错误。"),
                    returnType);
                return 404;
            }

            string controllerName = requestUrl[0], methodName = requestUrl[1], requestMode = context.Request.HttpMethod;
            Type requestType;
            switch (requestMode)
            {
                case "GET":
                    requestType = typeof(HttpGetAttribute);
                    break;
                case "POST":
                    requestType = typeof(HttpPostAttribute);
                    break;
                case "OPTIONS":
                    return 204;
                default:
                    result = GetResult(
                        GetErrorModel("请求方式错误", string.Format("请求的资源不支持 http 方法“{0}”。", requestMode)),
                        returnType);
                    return 405;
            }

            if (ControllerDic.ContainsKey(controllerName))
            {
                MethodInfo method = ControllerDic[controllerName].GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (method == null)
                {
                    result = GetResult(
                        GetErrorModel("找不到与请求 URI匹配的 HTTP 资源。", string.Format("在控制器“{0}”上找不到与名称“{1}”匹配的操作。", controllerName, methodName)),
                        returnType);
                    return 404;
                }
                else
                {
                    if (method.GetCustomAttributes(requestType, false).Length == 0)
                    {
                        result = GetResult(
                            GetErrorModel("请求方式错误", string.Format("请求的资源不支持 http 方法“{0}”。", requestMode)),
                            returnType);
                        return 405;
                    }
                    else
                    {
                        string postParams;
                        using (Stream stream = context.Request.InputStream)
                        {
                            StreamReader reader = new StreamReader(stream);
                            postParams = reader.ReadToEnd();
                        }
                        Dictionary<string, object> paramsDic = new Dictionary<string, object>();
                        try
                        {
                            if (method.GetParameters().Length == 0)
                            {
                                //无参方法
                                result = GetResult(
                                    method.Invoke(Activator.CreateInstance(ControllerDic[controllerName]), null)
                                    , returnType);
                                return 200;
                            }
                            else
                            {
                                //获取请求参数
                                try
                                {
                                    paramsDic = GetParams(context, postParams);
                                }
                                catch (Exception e)
                                {
                                    result = GetResult(
                                        GetErrorModel("接口调用参数错误。", e.Message)
                                        , returnType);
                                    return 200;
                                }

                                //设置方法参数
                                List<object> objectList = new List<object>();
                                foreach (ParameterInfo parameterInfo in method.GetParameters())
                                {
                                    object param = DataConversion.ToEntity(parameterInfo, paramsDic);
                                    string message;
                                    if (DataCheck.AttributeCheck(param, out message))
                                    {
                                        objectList.Add(param);
                                    }
                                    else
                                    {
                                        result = GetResult(
                                            GetErrorModel("请求参数错误", message)
                                            , returnType);
                                        return 200;
                                    }
                                }

                                result = GetResult(
                                    method.Invoke(Activator.CreateInstance(ControllerDic[controllerName]), objectList.ToArray())
                                    , returnType);
                                return 200;
                            }
                        }
                        catch (Exception e)
                        {
                            new LogControl(controllerName).WriteLog(
                                "接口名称：{0}\r\n错误时间：{1}\r\n错误原因：{2}\r\n接口请求参数：\r\n{3}\r\n"
                                , methodName
                                , DateTime.Now.ToString("MM-dd HH:mm:ss")
                                , e
                                , string.Join("&", paramsDic.Select(x => string.Format("{0}={1}", x.Key, x.Value))));

                            result = GetResult(
                                GetErrorModel("接口发生错误。", string.Format("{0}/{1}接口内部发生错误。", controllerName, methodName))
                                , returnType);
                            return 500;
                        }
                    }
                }
            }
            else
            {
                result = GetResult(
                    GetErrorModel("找不到与请求 URI匹配的 HTTP 资源。", string.Format("未找到与名为“{0}”的控制器匹配的类型。", controllerName))
                    , returnType);
                return 404;
            }
        }

        private static ErrorModel GetErrorModel(string message, string detail)
        {
            ErrorModel erroModel = new ErrorModel();
            erroModel.Message = message;
            erroModel.MessageDetail = detail;
            return erroModel;
        }

        private static string GetResult(object data, ReturnTypeEnum returnType)
        {
            switch (returnType)
            {
                case ReturnTypeEnum.Json:
                    return DataConversion.ToJson(data).ToString();
                case ReturnTypeEnum.Xml:
                    return DataConversion.ToXml(data);
                default:
                    return null;
            }
        }

        private static Dictionary<string, Type> GetController()
        {
            Dictionary<string, Type> controllerDic = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            Assembly assembly = Assembly.GetAssembly(typeof(ApiController));
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterface(typeof(ApiController).Name) != null)
                {
                    controllerDic.Add(type.Name, type);
                }
            }
            return controllerDic;
        }

        public bool IsReusable { get { return false; } }
    }
}