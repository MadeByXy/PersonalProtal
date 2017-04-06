using PersonalPortal.Content;
using PersonalPortal.Content.Library;
using PersonalPortal.ResultModels.Models;
using System;
using System.Data;
using static PersonalPortal.ResultModels.Models.IndexModel;

namespace PersonalPortal.Controllers
{
    public class Index : ApiController
    {
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