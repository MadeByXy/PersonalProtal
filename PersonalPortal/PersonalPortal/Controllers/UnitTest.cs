using Newtonsoft.Json.Linq;
using PersonalPortal.Content;
using PersonalPortal.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

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

                    result.Add(new UnitTestModel.NameSpace()
                    {
                        Name = assembly.GetName().Name,
                        Comments = "暂未完成程序集注释",
                        NameSpaces = assembly.GetTypes().Select(type => new UnitTestModel.NameSpace()
                        {
                            Name = type.Namespace,
                            Comments = "暂未完成命名空间注释"
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
        /// <param name="AssemblyName">程序集名称</param>
        /// <param name="NameSpaceName">命名空间</param>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Method> GetClass(string AssemblyName, string NameSpaceName)
        {
            var result = new List<UnitTestModel.Method>();
            foreach (Assembly assembly in GetAssemblies())
            {
                if (assembly.GetName().Name != AssemblyName) { continue; }
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace != NameSpaceName) { continue; }

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
        /// <param name="AssemblyName">程序集名称</param>
        /// <param name="NameSpaceName">命名空间</param>
        /// <param name="ClassName">类名称</param>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Method> GetMethod(string AssemblyName, string NameSpaceName, string ClassName)
        {
            var result = new List<UnitTestModel.Method>();
            foreach (Assembly assembly in GetAssemblies())
            {
                if (assembly.GetName().Name != AssemblyName) { continue; }
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace != NameSpaceName || type.Name != ClassName) { continue; }
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
        /// 方法单元测试（未完成）
        /// </summary>
        /// <param name="AssemblyName">程序集名称</param>
        /// <param name="NameSpaceName">命名空间</param>
        /// <param name="ClassName">类名称</param>
        /// <param name="MethodFullName">方法全名称</param>
        /// <param name="MethodParameters">参数列表</param>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Test> Test(string AssemblyName, string NameSpaceName, string ClassName, string MethodFullName, JArray ClassParameters, JArray MethodParameters)
        {
            var result = new List<UnitTestModel.Test>();
            foreach (Assembly assembly in GetAssemblies())
            {
                if (assembly.GetName().Name != AssemblyName) { continue; }
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace != NameSpaceName || type.Name != ClassName) { continue; }
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (method.ToString() != MethodFullName) { continue; }
                        foreach (JToken parameter in MethodParameters)
                        {
                            try
                            {
                                method.Invoke(method.IsStatic ? Activator.CreateInstance(type) : null,
                                    parameter.Where(x => method.GetParameters().Select(p => p.Name).Contains(x.ToString())).Select(x => x.ToString() as object).ToArray());
                            }
                            catch { }
                        }
                    }
                }
            }
            return result;
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