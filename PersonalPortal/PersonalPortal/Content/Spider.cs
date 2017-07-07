using Neo.IronLua;
using PersonalPortal.Content.Library;
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
                        while (true)
                        {
                            //遍历关键字
                            Uri url = new Uri(spider.Url.ToString().Replace("{keyWord}", keyWord).Replace("{page}", spider.CurrentPage));

                            string html = Network.GetHtml(url.ToString(), null);

                            spider.DataProcessing(html);

                            foreach (var item in spider.Results)
                            {
                                DataBase.ExecuteSql("insert into Spider (spiderUrl, spiderTitle, spiderData, spiderDate) values ('{0}', '{1}', '{2}', date())",
                                    item.SpiderUrl, item.SpiderTitle, item.SpiderData);
                            }

                            if (!spider.Continue) { break; }
                        }
                    }
                }
            });
        }
    }
}