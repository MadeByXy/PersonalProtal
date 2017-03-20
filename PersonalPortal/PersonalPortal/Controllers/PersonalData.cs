using PersonalPortal.Content;
using PersonalPortal.Content.Library;
using PersonalPortal.Models.ResultModels;
using System.Data;

namespace PersonalPortal.Controllers
{
    public class PersonalData : ApiController
    {
        [HttpGet]
        public PersonalDataModel GetPersonalData(int personId)
        {
            DataTable data = DataBase.ExecuteSql<DataTable>("SELECT pd.*, int(datediff('m',birth,now)/12) AS age, int(datediff('m',entryTime,now)/12) AS JobExperience FROM PersonalData pd WHERE personId={0}", personId);
            if (data.Rows.Count == 0)
            {
                return new PersonalDataModel();
            }
            else
            {
                var result = DataConversion.ToEntity<PersonalDataModel>(data)[0];
                result.ContactList = DataConversion.ToEntity<PersonalDataModel.ContactItem>(DataBase.ExecuteSql<DataTable>(
                    "SELECT * FROM PersonalContact WHERE personId={0}", personId));
                result.SkillList = DataConversion.ToEntity<PersonalDataModel.SkillItem>(DataBase.ExecuteSql<DataTable>(
                    "SELECT skillName, format$(skillLevel,'0%') AS skillLevel FROM PersonalSkill WHERE personId={0}", personId));
                result.JobList = DataConversion.ToEntity<PersonalDataModel.JobItem>(DataBase.ExecuteSql<DataTable>(
                    "SELECT companyName, jobTitle, format(startDate,'yyyy年MM月') as startTime, iif(isNull(endDate),'至今',format(endDate,'yyyy年MM月'))as endTime, description FROM PersonalJob WHERE personId={0}", personId));
                return result;
            }
        }
    }
}