using Newtonsoft.Json.Linq;
using PersonalPortal.Content;
using PersonalPortal.Models.ApplyModels;
using PersonalPortal.Models.ResultModels;
using PersonalPortal.ResultModels.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                "update shortCut set shortCutName = :shortCutName, shortCutUrl = :shortCutUrl where id = :id and shortCutIp = :userIp"
                , new Parameter { Name = "shortCutName", Value = data.ShortCutName }
                , new Parameter { Name = "shortCutUrl", Value = data.ShortCutUrl }
                , new Parameter { Name = "id", Value = data.Id }
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
        /// 翻译接口
        /// </summary>
        /// <param name="from">翻译源语言</param>
        /// <param name="to">译文语言</param>
        /// <param name="q">请求翻译query</param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public string Translate(string from, string to, string q)
        {
            const string appid = "20180306000131905";
            const string key = "JJ7D5nyJppWiMixmNoNI";
            string salt = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            try
            {
                return Network.GetHtml("http://api.fanyi.baidu.com/api/trans/vip/translate", new Dictionary<string, string>()
                {
                    { "from", from },
                    { "to", to },
                    { "q", Uri.EscapeDataString(q) },
                    { "appid", appid },
                    { "salt", salt },
                    { "sign", (appid + q + salt + key).ToMd5() },
                });
            }
            catch (Exception e) { return e.ToString(); }
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

        /// <summary>
        /// 委托请求以解决js的跨域问题
        /// </summary>
        /// <param name="query">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpDelegateModel DelegateHttp(QueryModel query)
        {
            HttpDelegateModel model = new HttpDelegateModel();
            try
            {
                var response = Network.GetHtml(query);

                model.StatusCode = (int)response.StatusCode;
                model.StatusMessage = response.StatusDescription;

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        model.Body = reader.ReadToEnd();
                    }
                }

                try
                {
                    //如果结果为Json, 刷版式
                    model.Body = JToken.Parse(model.Body).ToString();
                }
                catch { }

                using (MemoryStream stream = new MemoryStream(response.Headers.ToByteArray()))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        model.Header = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                model.StatusMessage = e.Message;
            }
            return model;
        }
    }
}