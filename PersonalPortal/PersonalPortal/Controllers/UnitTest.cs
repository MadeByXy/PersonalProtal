using Newtonsoft.Json.Linq;
using PersonalPortal.Content;
using PersonalPortal.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Xml;

namespace PersonalPortal.Controllers
{
    public class UnitTest : ApiController
    {
        /// <summary>
        /// 获取所有命名空间（读取注释未完成）
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
                    foreach (string key in new string[] { "Microsoft", "System", "mscorlib", "Newtonsoft" })
                    {
                        //屏蔽列表，后续可单独建张表
                        if (assembly.FullName.Contains(key))
                        {
                            goto Continue;
                        }
                    }

                    XmlDocument comments = GetDocument(assembly);
                    result.Add(new UnitTestModel.NameSpace()
                    {
                        Name = assembly.GetName().Name,
                        Comments = comments["assembly"]?["name"]?.Value,
                        NameSpaces = assembly.GetTypes().Select(type => new UnitTestModel.NameSpace()
                        {
                            Name = type.Namespace,
                            Comments = comments["members"]?[string.Format("member name=\"T: {0}\"", type.FullName)]?.Value,// "暂未完成命名空间注释"
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
        /// 获取命名空间下所有类（读取注释未完成）
        /// </summary>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Method> GetClass(Models.ApplyModels.UnitTestModel.TestModel data)
        {
            var result = new List<UnitTestModel.Method>();
            foreach (Assembly assembly in GetAssemblies())
            {
                if (assembly.GetName().Name != data.AssemblyName) { continue; }
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace != data.NameSpaceName) { continue; }

                    foreach (ConstructorInfo info in type.GetConstructors())
                    {
                        result.Add(new UnitTestModel.Method()
                        {
                            Name = type.Name,
                            FullName = string.Format("{0}({1})", type.Name, string.Join(", ", info.GetParameters()?.Select(x => x.ParameterType.FullName).ToArray())),
                            Comments = "暂未完成类注释",
                            Parameters = info.GetParameters()?.Select(x => new UnitTestModel.Parameter()
                            {
                                Comments = "暂未完成类参数注释",
                                Name = x.Name,
                                ParameterType = x.ParameterType.Name
                            }).ToList()
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取类下所有方法（读取注释未完成）
        /// </summary>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Method> GetMethod(Models.ApplyModels.UnitTestModel.TestModel data)
        {
            var result = new List<UnitTestModel.Method>();
            foreach (Assembly assembly in GetAssemblies())
            {
                if (assembly.GetName().Name != data.AssemblyName) { continue; }
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace != data.NameSpaceName || type.Name != data.ClassName) { continue; }
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        try
                        {
                            result.Add(new UnitTestModel.Method()
                            {
                                Comments = "暂未完成方法注释",
                                Name = method.Name,
                                FullName = method.ToString(),
                                ReturnComments = "暂未完成方法返回值注释",
                                ReturnType = method.ReturnType.Name,
                                Parameters = method.GetParameters().Select(parameter => new UnitTestModel.Parameter
                                {
                                    Name = parameter.Name,
                                    Comments = "暂未完成方法参数注释",
                                    ParameterType = parameter.ParameterType.Name
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
                            //遍历实例方法以确定实例
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
            string filePath = string.Format(@"{0}\bin\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, assembly.GetName().Name);
            if (File.Exists(filePath))
            {
                doc.Load(filePath);
            }
            return doc;
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