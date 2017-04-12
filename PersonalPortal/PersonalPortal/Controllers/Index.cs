using PersonalPortal.Content;
using PersonalPortal.Content.Library;
using PersonalPortal.Models.ApplyModels;
using PersonalPortal.ResultModels.Models;
using System;
using System.Data;

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
            DataTable data = DataBase.ExecuteSql<DataTable>("select top {1} * from shortCut where shortCutIp='{0}'", userIp, size);
            if (data.Rows.Count == 0)
            {
                for (int i = 0; i < 10; i++) { DataBase.ExecuteSql("insert into shortCut (shortCutIp) values ('{0}')", userIp); }
                data = DataBase.ExecuteSql<DataTable>("select top {1} * from shortCut where shortCutIp='{0}'", userIp, size);
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