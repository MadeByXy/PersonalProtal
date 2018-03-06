using Neo.IronLua;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
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

        ///C#生成MD5的方法
        public static string ToMd5(this string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = Encoding.UTF8.GetBytes(text);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }

        /// <summary>
        /// 通过特性值查找指定节点
        /// </summary>
        /// <param name="xmlElement">XML节点</param>
        /// <param name="attributeName">特性名称</param>
        /// <param name="attributeValue">特性值</param>
        /// <returns></returns>
        public static XmlElement GetByAttribute(this XmlElement xmlElement, string attributeName, string attributeValue)
        {
            foreach (var item in xmlElement.ChildNodes)
            {
                if (!(item is XmlElement)) { continue; }

                var xml = item as XmlElement;
                if (xml.HasAttribute(attributeName) && xml.GetAttribute(attributeName) == attributeValue)
                {
                    return xml;
                }
            }
            return null;
        }
    }
}
