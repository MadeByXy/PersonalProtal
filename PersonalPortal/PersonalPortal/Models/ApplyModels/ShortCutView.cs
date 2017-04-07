using static PersonalPortal.Content.DataCheck;

namespace PersonalPortal.Models.ApplyModels
{
    public class ShortCutView
    {
        [NonEmpty(Name = "标签ID")]
        public int Id { get; set; }

        [NonEmpty(Name = "标签名称")]
        public string ShortCutName { get; set; }

        [NonEmpty(Name = "标签链接")]
        public string ShortCutUrl { get; set; }
    }
}