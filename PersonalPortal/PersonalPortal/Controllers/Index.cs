using PersonalPortal.Content;
using PersonalPortal.Models.ApplyModels;
using PersonalPortal.ResultModels.Models;
using System;
using System.Data;
using XYZZ.Library;

namespace PersonalPortal.Controllers
{
    public class Index : ApiController
    {
        /// <summary>
        /// 获取快捷方式链接
        /// </summary>
        [HttpGet]
        [HttpPost]
        public ShortCutModel GetShortCut(string userIp, int size)
        {
            if (size == 0) { size = 8; }
            DataTable data = DataBase.ExecuteSql<DataTable>("select * from shortCut where shortCutIp='{0}' limit 0, {1}", userIp, size);
            if (data.Rows.Count == 0)
            {
                for (int i = 0; i < 10; i++) { DataBase.ExecuteSql("insert into shortCut (shortCutIp) values ('{0}')", userIp); }
                data = DataBase.ExecuteSql<DataTable>("select * from shortCut where shortCutIp='{0}' limit 0, {1}", userIp, size);
            }
            ShortCutModel model = new ShortCutModel();
            model.ShortCutList = DataConversion.ToEntity<ShortCutItem>(data);
            return model;
        }

        /// <summary>
        /// 添加快捷方式链接
        /// </summary>
        [HttpGet]
        [HttpPost]
        public string SetShortCut(ShortCutView data, string userIp)
        {
            return DataBase.ExecuteSql<bool>("update shortCut set shortCutName='{0}', shortCutUrl='{1}' where ID={2} and shortCutIp='{3}'"
                 , data.ShortCutName, data.ShortCutUrl, data.Id, userIp) ? "true" : "false";
        }

        /// <summary>
        /// 获取页面是否使用网页背景动画
        /// </summary>
        [HttpGet]
        [HttpPost]
        public string GetBackGround(string userIp)
        {
            if (!DataBase.ExecuteSql<bool>("select 1 from BackGround where backGroundIp = '{0}'", userIp))
            {
                DataBase.ExecuteSql("insert into BackGround (backGroundIp, backgroundStatus) values ('{0}', 1)", userIp);
            }
            return DataBase.ExecuteSql<string>("select backgroundStatus from BackGround where backGroundIp = '{0}'", userIp) == "1" ? "true" : "false";
        }

        /// <summary>
        /// 设置页面是否使用网页背景动画
        /// </summary>
        [HttpGet]
        [HttpPost]
        public string SetBackGround(string userIp, bool on)
        {
            DataBase.ExecuteSql("update BackGround set backgroundStatus = {1} where backGroundIp = '{0}'", userIp, on ? "1" : "0");
            return "true";
        }

        /// <summary>
        /// 委托请求以解决js的跨域问题
        /// </summary>
        [HttpGet]
        public string DelegateQuery(string url)
        {
            try
            {
                return Network.GetHtml(url, null);
            }
            catch (Exception e) { return e.ToString(); }

        }
    }
}