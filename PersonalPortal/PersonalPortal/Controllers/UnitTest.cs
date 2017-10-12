using Newtonsoft.Json.Linq;
using PersonalPortal.Content;
using PersonalPortal.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Xml;
using XYZZ.Library;

namespace PersonalPortal.Controllers
{
    /// <summary>
    /// 单元测试Api
    /// </summary>
    public class UnitTest : ApiController
    {
        /// <summary>
        /// 获取所有命名空间
        /// </summary>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.NameSpace> GetNameSpace()
        {
            var result = new List<UnitTestModel.NameSpace>();
            foreach (Assembly assembly in GetAssemblies())
            {
                try
                {
                    foreach (string key in DataBase.ExecuteSql<DataTable>(
                        "select blockValue from blockList where blockType = 'Assembly'").Select("1 = 1").Select(
                        row => row["blockValue"].ToString()))
                    {
                        //屏蔽列表
                        if (assembly.FullName.Contains(key))
                        {
                            goto Continue;
                        }
                    }

                    result.Add(new UnitTestModel.NameSpace()
                    {
                        Name = assembly.GetName().Name,
                        Comments = assembly.GetName().Name,
                        NameSpaces = assembly.GetTypes().Select(type => new UnitTestModel.NameSpace()
                        {
                            Name = type.Namespace,
                            Comments = type.Namespace,
                        }).Where(x => !string.IsNullOrEmpty(x.Name)).Distinct(new UnitTestModel.NameSpace()).ToList()
                    });

                    //跳转
                    Continue:
                    {
                        continue;
                    }
                }
                catch (ReflectionTypeLoadException) { }
            }
            return result;
        }

        /// <summary>
        /// 获取命名空间下所有类
        /// </summary>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Method> GetClass(Models.ApplyModels.UnitTestModel.TestModel data)
        {
            var result = new List<UnitTestModel.Method>();
            foreach (Assembly assembly in GetAssemblies())
            {
                if (assembly.GetName().Name != data.AssemblyName) { continue; }

                XmlDocument comments = GetDocument(assembly);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace != data.NameSpaceName) { continue; }

                    foreach (string key in DataBase.ExecuteSql<DataTable>(
                        "select blockValue from blockList where blockType = 'Class'").Select("1 = 1").Select(
                        row => row["blockValue"].ToString()))
                    {
                        //屏蔽列表
                        if (type.FullName.Contains(key))
                        {
                            goto Continue;
                        }
                    }

                    var members = comments["doc"]?["members"];
                    if (type.GetConstructors().Length > 0)
                    {
                        foreach (ConstructorInfo info in type.GetConstructors())
                        {
                            var fullName = string.Format("{0}({1})", type.Name, string.Join(", ", info.GetParameters()?.Select(x => x.ParameterType.FullName).ToArray()));
                            var instanceName = string.Format("{0}.#ctor({1})", type.FullName, string.Join(", ", info.GetParameters()?.Select(x => x.ParameterType.FullName).ToArray()));
                            result.Add(new UnitTestModel.Method()
                            {
                                Name = type.Name,
                                FullName = fullName,
                                Comments = members?.GetByAttribute("name", string.Format("T:{0}", type.FullName))?["summary"]?.InnerText.Trim() ?? fullName,
                                Parameters = info.GetParameters()?.Select(par => new UnitTestModel.Parameter()
                                {
                                    Comments = members?.GetByAttribute("name", string.Format("M:{0}", instanceName))?.GetByAttribute("name", par.Name).InnerText.Trim(),
                                    Name = par.Name,
                                    ParameterType = par.ParameterType.Name
                                }).ToList()
                            });
                        }
                    }
                    else
                    {
                        //静态类没有实例化方法
                        result.Add(new UnitTestModel.Method()
                        {
                            Name = type.Name,
                            FullName = type.Name,
                            Comments = members?.GetByAttribute("name", string.Format("T:{0}", type.FullName))?["summary"]?.InnerText.Trim() ?? type.Name
                        });
                    }

                    //跳转
                    Continue:
                    {
                        continue;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取类下所有方法
        /// </summary>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Method> GetMethod(Models.ApplyModels.UnitTestModel.TestModel data)
        {
            var result = new List<UnitTestModel.Method>();
            foreach (Assembly assembly in GetAssemblies())
            {
                if (assembly.GetName().Name != data.AssemblyName) { continue; }

                XmlDocument comments = GetDocument(assembly);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace != data.NameSpaceName || type.Name != data.ClassName) { continue; }

                    var members = comments["doc"]?["members"];
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        try
                        {
                            var commentName = string.Format("M:{0}", GetCommentName(method));
                            result.Add(new UnitTestModel.Method()
                            {
                                Comments = members?.GetByAttribute("name", commentName)?["summary"]?.InnerText.Trim(),
                                Name = method.Name,
                                FullName = method.ToString(),
                                ReturnComments = method.ReturnType == typeof(void) ?
                                    "该方法无返回值" :
                                    members?.GetByAttribute("name", commentName)?["returns"]?.InnerText.Trim(),
                                ReturnType = method.ReturnType.Name,
                                Parameters = method.GetParameters().Select(par => new UnitTestModel.Parameter
                                {
                                    Name = par.Name,
                                    Comments = members?.GetByAttribute("name", commentName)?.GetByAttribute("name", par.Name)?.InnerText.Trim(),
                                    ParameterType = par.ParameterType.Name
                                }).ToList()
                            });
                        }
                        catch (ReflectionTypeLoadException) { }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 方法单元测试
        /// </summary>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Test> Test(Models.ApplyModels.UnitTestModel.TestModel data)
        {
            var result = new List<UnitTestModel.Test>();
            foreach (Assembly assembly in GetAssemblies())
            {
                if (assembly.GetName().Name != data.AssemblyName) { continue; }
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace != data.NameSpaceName || type.Name != data.ClassName) { continue; }
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (method.ToString() != data.MethodFullName) { continue; }

                        object instance = null;
                        string[] typeNames = Regex.Match(data.ClassFullName, @"\(([^\)]*)\)").Groups[1].Value.Split(',');
                        List<object> classParamsList = new List<object>();
                        foreach (ConstructorInfo info in type.GetConstructors())
                        {
                            //遍历实例化方法以确定实例
                            ParameterInfo[] pars = info.GetParameters();
                            for (int i = 0; i < pars.Length; i++)
                            {
                                try
                                {
                                    if (pars[i].ParameterType.FullName != typeNames[i].Trim())
                                    {
                                        throw null;
                                    }
                                    else
                                    {
                                        classParamsList.Add(DataConversion.ToEntity(pars[i], data.ClassParameters));
                                    }
                                }
                                catch { goto Continue; }
                            }

                            instance = Activator.CreateInstance(type, classParamsList.ToArray());
                            break;

                            Continue:
                            {
                                classParamsList.Clear();
                                continue;
                            }
                        }

                        foreach (JToken parameter in data.MethodParameters)
                        {
                            UnitTestModel.Test test = new UnitTestModel.Test();
                            DateTime startTime = DateTime.Now;

                            try
                            {
                                List<object> paramsList = new List<object>();

                                //构造参数
                                foreach (ParameterInfo parameterInfo in method.GetParameters())
                                {
                                    paramsList.Add(DataConversion.ToEntity(parameterInfo, parameter as JObject));
                                }

                                test.Result = DataConversion.ToJson(method.Invoke(
                                    instance,
                                    paramsList.ToArray())).ToString();
                                test.IsFinish = true;
                            }
                            catch (Exception e)
                            {
                                test.IsFinish = false;
                                test.Error.Add((e.InnerException ?? e).ToString());
                            }
                            test.UseTime = (DateTime.Now - startTime).TotalMilliseconds.ToString(".##");

                            result.Add(test);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取程序集下注释
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        private XmlDocument GetDocument(Assembly assembly)
        {
            XmlDocument doc = new XmlDocument();
            string local = assembly.CodeBase.Replace("file:///", "").Replace("/", "\\");
            string filePath = local.Replace(new FileInfo(local).Extension, ".xml");
            if (File.Exists(filePath))
            {
                doc.Load(filePath);
            }
            return doc;
        }

        /// <summary>
        /// 获取方法注释名称
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns></returns>
        private string GetCommentName(MethodInfo method)
        {
            Match regex = Regex.Match(method.ToString(), @"\[([^\[]*)\]\(");

            string name = string.Format("{0}.{1}{2}({3})",
                method.ReflectedType.FullName,
                method.Name,
                regex.Success ? string.Format("``{0}", regex.Groups[1].Value.Split(',').Length) : "",
                string.Join(",", method.GetParameters().Select(par =>
                    new Func<string>(() =>
                    {
                        if (par.ParameterType.IsGenericType)
                        {
                            //泛型要进行转换才能获取到注释
                            if (string.IsNullOrEmpty(regex.Groups[1].Value))
                            {
                                //字典
                                return Regex.Replace(par.ParameterType.ToString(), "`[0-9]*", "").Replace("[", "{").Replace("]", "}");
                            }
                            else
                            {
                                string parName = par.ParameterType.Name;
                                string[] array = regex.Groups[1].Value.Split(',');
                                for (int i = 0; i < array.Length; i++)
                                {
                                    parName = parName.Replace(array[i], string.Format("``{0}", i));
                                }
                                return parName;
                            }
                        }
                        if (par.ParameterType.IsEnum)
                        {
                            return par.ParameterType.ToString().Replace("+", ".");
                        }
                        return par.ParameterType.ToString();
                    }).Invoke()
                ).ToArray()));
            return name;
        }

        /// <summary>
        /// 获取所有程序集
        /// </summary>
        /// <returns></returns>
        private List<Assembly> GetAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (Assembly assembly in BuildManager.GetReferencedAssemblies())
            {
                assemblies.Add(assembly);
            }
            return assemblies.OrderBy(x => x.GetName().Name).ToList();
        }
    }
}