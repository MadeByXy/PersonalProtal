using PersonalPortal.Content;
using PersonalPortal.Models.ResultModels;
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
            foreach (Assembly assembly in BuildManager.GetReferencedAssemblies())
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
                        NameSpaces = assembly.GetTypes().Select(type => new UnitTestModel.NameSpace()
                        {
                            Name = type.Namespace
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
        /// 获取命名空间下所有方法（未完成）
        /// </summary>
        /// <param name="NameSpace">命名空间</param>
        [HttpGet]
        [HttpPost]
        public List<UnitTestModel.Method> GetMethod(string NameSpace)
        {
            return new List<UnitTestModel.Method>();
        }
    }
}