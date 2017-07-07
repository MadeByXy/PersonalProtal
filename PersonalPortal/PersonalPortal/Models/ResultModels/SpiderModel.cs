using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PersonalPortal.Models.ResultModels
{
    public class SpiderModel
    {
        public class SpiderList
        {
            public List<SpiderItem> SpiderItems { get; set; } = new List<SpiderItem>();
        }

        public class SpiderItem
        {
            public string Name { get; set; }
            [XmlIgnore]
            [IgnoreDataMember]
            public string Api { get; set; }
        }
    }
}