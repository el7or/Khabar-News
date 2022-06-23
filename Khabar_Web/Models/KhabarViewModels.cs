using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Khabar_Web.Models.Validations_AR;

namespace Khabar_Web.Models
{
    public enum ListNewsType
    {
        CategoryNews,
        SourceNews
    }

    public class UserVM
    {
        public int FUserPK { get; set; }

        public string displayname { get; set; }

        [Required_AR]
        [DataType(DataType.EmailAddress)]
        [Display(Name ="البريد الإلكتروني")]
        public string email { get; set; }

        [Required_AR]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Compare("password",ErrorMessage ="كلمة المرور وتأكيدها غير متطابقين !")]
        [Display(Name = "تأكيد كلمة المرور")]
        public string passwordConfirm { get; set; }
    }

    public  class CategoryVM
    {
        public int CatID { get; set; }

        [Display(Name = "التصنيف")]
        public string Title { get; set; }

        [Display(Name = "التوصيف")]
        public string Info { get; set; }

        [Display(Name = "أيقونة التصنيف")]
        public string Image { get; set; }

        [Display(Name = "عدد المصادر")]
        public int? SourceCount { get; set; }

        [Display(Name = "عدد الأخبار")]
        public int? NewsCount { get; set; }
    }

    public  class SourceVM
    {        
        public int SrcID { get; set; }

        [Display(Name = "المصدر")]
        public string Title { get; set; }

        [Display(Name = "التوصيف")]
        public string Info { get; set; }

        [Display(Name = "أيقونة المصدر")]
        public string Image { get; set; }

        [Display(Name = "عدد الأخبار")]
        public int NewsCount { get; set; }

        public bool isFollow { get; set; }
    }

    public class NewsVM
    {
        public int NewsID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public long SeenCount { get; set; }
        public string ExternalURL { get; set; }
        public DateTime? Timing { get; set; }
        public string SourceTitle { get; set; }
    }
}