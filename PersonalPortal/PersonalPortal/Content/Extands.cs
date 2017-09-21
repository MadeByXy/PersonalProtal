using Neo.IronLua;
using System;
using System.IO;
using XYZZ.Library;

namespace PersonalPortal.Content
{
    public static class Extands
    {
        public static void LoadLua(dynamic global, string fileName)
        {
            if (global is LuaGlobalPortable)
            {
                FileInfo luaFile = new FileInfo(string.Format(@"{0}\Lua\{1}.lua", AppDomain.CurrentDomain.BaseDirectory, fileName));
                if (luaFile.Exists)
                {
                    using (StreamReader stream = new StreamReader(luaFile.OpenRead()))
                    {
                        (global as LuaGlobalPortable).DoChunk(stream.ReadToEnd(), fileName);
                    }
                }
                else
                {
                    throw new FileNotFoundException("未找到指定的Lua文件");
                }
            }
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="text">待格式化字符串</param>
        /// <param name="parameters">格式化信息</param>
        public static string Format(this string text, params Parameter[] parameters)
        {
            foreach (Parameter parameter in parameters)
            {
                text = text.Replace(string.Format("{{{0}}}", parameter.Name), parameter.Value.ToString());
            }
            return text;
        }
    }
}
