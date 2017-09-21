using PersonalPortal.Content;
using PersonalPortal.Models.ResultModels;
using System.Data;
using XYZZ.Library;

namespace PersonalPortal.Controllers
{
    public class PersonalData : ApiController
    {
        [HttpGet]
        public PersonalDataModel GetPersonalData(int personId)
        {
            DataTable data = DataBase.ExecuteSql<DataTable>(
                @"select pd.*,
                         int(datediff('m', birth, now) / 12) as age,
                         int(datediff('m', entryTime, now) / 12) as jobExperience
                    from personalData pd
                   where personId = :personId",
                new Parameter { Name = "personId", Value = personId });
            if (data.Rows.Count == 0)
            {
                return new PersonalDataModel();
            }
            else
            {
                var result = DataConversion.ToEntity<PersonalDataModel>(data)[0];
                result.ContactList = DataConversion.ToEntity<PersonalDataModel.ContactItem>(DataBase.ExecuteSql<DataTable>(
                    "select * from personalContact where personId = :personId"
                    , new Parameter { Name = "personId", Value = personId }));

                result.SkillList = DataConversion.ToEntity<PersonalDataModel.SkillItem>(DataBase.ExecuteSql<DataTable>(
                    "select skillName, format$(skilllevel,'0%') as skillLevel from personalSkill where personId = :personId"
                    , new Parameter { Name = "personId", Value = personId }));

                result.JobList = DataConversion.ToEntity<PersonalDataModel.JobItem>(DataBase.ExecuteSql<DataTable>(
                    @"select companyName,
                             jobTitle,
                             format(startDate, 'yyyy年mm月') as startTime,
                             iif(isnull(endDate), '至今', format(endDate, 'yyyy年mm月')) as endTime,
                             description
                        from personalJob
                       where personId = :personId",
                    new Parameter { Name = "personId", Value = personId }));
                return result;
            }
        }
    }
}