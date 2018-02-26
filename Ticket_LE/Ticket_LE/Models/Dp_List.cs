using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ticket_LE.Models
{
    public class Dp_List
    {
        public SelectList JT_LIST { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกประเภทงาน")]
        [Display(Name = "ประเภท")]
        public int? JT_ID { get; set; }

        public string STCODE { get; set; }

    }
}