using Neo.IronLua;
using PersonalPortal.Content;
using PersonalPortal.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalPortal.Controllers
{
    public class Spider : ApiController
    {
        /// <summary>
        /// 获取爬虫列表
        /// </summary>
        [HttpGet]
        [HttpPost]
        public SpiderModel.SpiderList GetSpiderList()
        {
            SpiderModel.SpiderList spiderList = new SpiderModel.SpiderList();
            using (Lua lua = new Lua())
            {
                dynamic global = lua.CreateEnvironment();
                Extands.LoadLua(global, "Spider");
                foreach (var item in global.SpiderList)
                {
                    spiderList.SpiderItems.Add(new SpiderModel.SpiderItem() { Name = item.Value.Name, Api = item.Value.Api });
                }
            }
            return spiderList;
        }

        /// <summary>
        /// 执行爬虫
        /// </summary>
        [HttpGet]
        [HttpPost]
        public string InvokeSpider(string spiderName)
        {
            var spider = GetSpiderList().SpiderItems.Where(x => x.Name == spiderName);

            if (spider.Count() == 0)
            {
                return "未读取到该爬虫信息";
            }
            else
            {
                new Content.Spider(spider.First().Api).InvokeSpider();
                return "爬虫系统已启动";
            }
        }
    }
}