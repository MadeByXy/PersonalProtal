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
            DataTable data = DataBase.ExecuteSql<DataTable>(
                "select * from shortCut where shortCutIp= :userIp limit 0, :size"
                , new Parameter { Name = "userIp", Value = userIp }
                , new Parameter { Name = "size", Value = size });

            if (data.Rows.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    DataBase.ExecuteSql("insert into shortCut (shortCutIp) values (:userIp)"
                        , new Parameter { Name = "userIp", Value = userIp });
                }

                return GetShortCut(userIp, size);
            }

            return new ShortCutModel()
            {
                ShortCutList = DataConversion.ToEntity<ShortCutItem>(data)
            };
        }

        /// <summary>
        /// 添加快捷方式链接
        /// </summary>
        [HttpGet]
        [HttpPost]
        public string SetShortCut(ShortCutView data, string userIp)
        {
            return DataBase.ExecuteSql<bool>(
                "update shortCut set shortCutName = :shortCutName, shortCutUrl = '{1}' where id = :id and shortCutIp = :userIp"
                , new Parameter { Name = "shortCutName", Value = data.ShortCutName }
                , new Parameter { Name = "shortCutUrl", Value = data.ShortCutUrl }
                , new Parameter { Name = "shortCutName", Value = data.Id }
                , new Parameter { Name = "userIp", Value = userIp }).ToString().ToLower();
        }

        /// <summary>
        /// 获取页面是否使用网页背景动画
        /// </summary>
        [HttpGet]
        [HttpPost]
        public string GetBackGround(string userIp)
        {
            if (!DataBase.ExecuteSql<bool>("select 1 from BackGround where backGroundIp = :userIp"
                , new Parameter { Name = "userIp", Value = userIp }))
            {
                DataBase.ExecuteSql("insert into BackGround (backGroundIp, backgroundStatus) values (:userIp, 1)"
                      , new Parameter { Name = "userIp", Value = userIp });
            }
            return (DataBase.ExecuteSql<string>(
                "select backgroundStatus from BackGround where backGroundIp = :userIp"
                , new Parameter { Name = "userIp", Value = userIp }) == "1").ToString().ToLower();
        }

        /// <summary>
        /// 设置页面是否使用网页背景动画
        /// </summary>
        [HttpGet]
        [HttpPost]
        public string SetBackGround(string userIp, bool on)
        {
            DataBase.ExecuteSql(
                "update BackGround set backgroundStatus = :on where backGroundIp = :userIp"
                , new Parameter { Name = "userIp", Value = userIp }
                , new Parameter { Name = "on", Value = on ? "1" : "0" });
            return "true";
        }

        /// <summary>
        /// 委托请求以解决js的跨域问题
        /// </summary>
        [HttpGet]
        public string DelegateQuery(QueryModel query)
        {
            try
            {
                return Network.GetHtml(query);
            }
            catch (Exception e) { return e.ToString(); }

        }
    }
}