using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalPortal.Models.ResultModels
{
    /// <summary>
    /// 个人资料
    /// </summary>
    public class PersonalDataModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birth { get; set; }
        /// <summary>
        /// 工作职位
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public string EntryTime { get; set; }
        /// <summary>
        /// 工作年限
        /// </summary>
        public int JobExperience { get; set; }
        /// <summary>
        /// 毕业学校
        /// </summary>
        public string College { get; set; }
        /// <summary>
        /// 学位
        /// </summary>
        public string Degree { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 户口所在城市
        /// </summary>
        public string AccountCity { get; set; }
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string PoliticsStatus { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber { get; set; }
        /// <summary>
        /// 婚姻状态
        /// </summary>
        public string MaritalStatus { get; set; }
        /// <summary>
        /// 个人概述
        /// </summary>
        public string AboutMe { get; set; }
        /// <summary>
        /// 个人联系方式
        /// </summary>
        public List<ContactItem> ContactList { get; set; }
        /// <summary>
        /// 个人技能
        /// </summary>
        public List<SkillItem> SkillList { get; set; }
        /// <summary>
        /// 工作经历
        /// </summary>
        public List<JobItem> JobList { get; set; }

        public class ContactItem
        {
            /// <summary>
            /// 联系方式类型
            /// </summary>
            public string ContactType { get; set; }
            /// <summary>
            /// 联系方式
            /// </summary>
            public string ContactValue { get; set; }
        }
        public class SkillItem
        {
            public string SkillName { get; set; }
            public string SkillLevel { get; set; }
        }
        public class JobItem
        {
            /// <summary>
            /// 公司名称
            /// </summary>
            public string CompanyName{ get; set; }
            /// <summary>
            /// 职位名称
            /// </summary>
            public string JobTitle { get; set; }
            /// <summary>
            /// 工作开始时间
            /// </summary>
            public string StartTime { get; set; }
            /// <summary>
            /// 工作结束时间
            /// </summary>
            public string EndTime { get; set; }
            /// <summary>
            /// 工作描述
            /// </summary>
            public string Description { get; set; }
        }
    }
}