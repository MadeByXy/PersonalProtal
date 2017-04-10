using System.Collections.Generic;

namespace PersonalPortal.Models.ResultModels
{
    public class ProjectModel
    {
        /// <summary>
        /// 应用项目
        /// </summary>
        public List<ProjectItem> ProjectList = new List<ProjectItem>();
        /// <summary>
        /// 游戏项目(Unity)
        /// </summary>
        public List<ProjectItem> GameList = new List<ProjectItem>();
    }

    public class ProjectItem
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目地址
        /// </summary>
        public string ProjectLocal { get; set; }
    }
}