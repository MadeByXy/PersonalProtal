using PersonalPortal.Content;
using PersonalPortal.Content.Library;
using PersonalPortal.Models.ResultModels;
using System.Data;

namespace PersonalPortal.Controllers
{
    public class Project
    {
        /// <summary>
        /// 获取作品集项目
        /// </summary>
        public ProjectModel GetProject()
        {
            return new ProjectModel()
            {
                ProjectList = DataConversion.ToEntity<ProjectItem>(DataBase.ExecuteSql<DataTable>("select * from Project where projectType='project'")),
                GameList = DataConversion.ToEntity<ProjectItem>(DataBase.ExecuteSql<DataTable>("select * from Project where projectType='project'"))
            };
        }
    }
}