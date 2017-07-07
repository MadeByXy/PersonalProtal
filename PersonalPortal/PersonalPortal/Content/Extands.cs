using Neo.IronLua;
using System;
using System.IO;

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
    }
}
