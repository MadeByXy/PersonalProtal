using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalPortal.Content
{
    /// <summary>
    /// 指示该类为接口类
    /// </summary>
    public interface ApiController { }

    /// <summary>
    /// 指示接口支持 POST HTTP 方法
    /// </summary>
    public class HttpPostAttribute : Attribute { }

    /// <summary>
    /// 指示接口支持 POST GET 方法
    /// </summary>
    public class HttpGetAttribute : Attribute { }
}