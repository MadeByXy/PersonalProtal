using PersonalPortal.Content;
using PersonalPortal.Content.Library;
using PersonalPortal.Models.ApplyModels;
using PersonalPortal.ResultModels.Models;
using System;
using System.Data;
using static PersonalPortal.ResultModels.Models.IndexModel;

namespace PersonalPortal.Controllers
{
    public class Index : ApiController
    {
        /// <summary>
        /// 获取标题链接
        /// </summary>
        [HttpGet]
        [HttpPost]
        public IndexModel GetIndex()
        {
            IndexModel model = new IndexModel();
            DataTable data = DataBase.ExecuteSql<DataTable>("select * from navigation");
            foreach (DataRow row in data.Select("parentId = 0"))
            {
                NavigationItem item = new NavigationItem()
                {
                    LinkName = row["linkName"].ToString(),
                    LinkUrl = row["linkUrl"].ToString()
                };
                DataRow[] rows = data.Select("parentId = " + row["id"]);
                if (rows.Length != 0)
                {
                    item.Navigation = DataConversion.ToEntity<NavigationItem>(rows.CopyToDataTable());
                }
                model.Navigation.Add(item);
            }
            return model;
        }

        /// <summary>
        /// 获取快捷方式链接
        /// </summary>
        [HttpGet]
        [HttpPost]
        public ShortCutModel GetShortCut(string userIp)
        {
            DataTable data = DataBase.ExecuteSql<DataTable>("select * from shortCut where shortCutIp='{0}'", userIp);
            if (data.Rows.Count == 0)
            {
                for (int i = 0; i < 8; i++) { DataBase.ExecuteSql("insert into shortCut (shortCutIp) values ('{0}')", userIp); }
                data = DataBase.ExecuteSql<DataTable>(string.Format("select * from shortCut where shortCutIp='{0}'", userIp));
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