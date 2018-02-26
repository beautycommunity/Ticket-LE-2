using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticket_LE.Models
{
    public class UserViewModels
    {
        public string sh { get; set; }
        public int Page {get;set;}
        public USER_LOGIN EditUser { get; set; }
        public List<USER_LOGIN> RowUser { get; set; }
    }

    public class USER_LOGIN
    {
        public int? ID { get; set; }

        [Display(Name = "รหัสพนักงาน")]
        public string STCODE { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่าน")]
        public string PASSWORD { get; set; }

        //[DataType(DataType.Password)]
        [Display(Name = "สิทธ์การใช้งาน")]
        public string A_NAME { get; set; }

        [Display(Name = "ชื่อ-นามสกุล")]
        public string FULLNAME { get; set; }

        [Display(Name = "แผนก")]
        public string DEP { get; set; }


    }
}