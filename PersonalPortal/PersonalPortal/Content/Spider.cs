using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PersonalPortal.Content
{
    public class Spider
    {
        private string LuaName { get; set; }
        public Spider(string luaName)
        {
            LuaName = luaName;
        }

        public void InvokeSpider()
        {
            Task.Run(() =>
            {
                using (Lua lua = new Lua())
                {
                    dynamic global = lua.CreateEnvironment();
                    Extands.LoadLua(global, LuaName);

                    var spider = global.Spider;
                    foreach (var keyWord in spider.KeyWord)
                    {
                        string html = Network.GetHtml(spider.Url + keyWord, null);
                        foreach (var regex in spider.Regex)
                        {
                            foreach (Match match in Regex.Matches(html, regex.Value))
                            {
                                var temp = match.Groups[0].Value;
                            }
                        }
                    }
                }
            });
        }
    }
}