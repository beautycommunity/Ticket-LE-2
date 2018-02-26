using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticket_LE.Models
{
    public class TicketModels
    {
        public List<CheckBox> Detail { get; set; }
        public Ticket TicketSub { get; set; }
        public List<Ticket> TicketDetail { get; set; }
        public DetailTciket Add { get; set; }
        public List<CheckBox> GetCheck { get; set; }
    }

    public class CheckBox
    {
        public int row { get; set; }

        [Display(Name = "เอกสารที่ขอ")]
        public int ID { get; set; }

        [Display(Name = "วัตถุประสงค์ในการใช้เอกสาร")]
        public string NAME { get; set; }

        [Display(Name = "เอกสารที่ขอ")]
        public string Doc { get; set; }

        [Display(Name = "ประเภท")]
        public string Type { get; set; }

        public bool Checked { get; set; }
    }

    public class DetailTciket
    {
        [Display(Name = "วัตถุประสงค์ในการใช้เอกสาร")]
        public string Detail { get; set; }

        public bool Checked { get; set; }
    }

    public class Ticket
    {
        public int? ID { get; set; }

        [Display(Name = "เลขที่")]
        public string TICKETNO { get; set; }

        [Display(Name = "ผู้แจ้ง")]
        public string CRE_NICKNAME { get; set; }

        [Display(Name = "สถานะ")]
        public string SSNAME { get; set; }

        [Display(Name = "ไอดีสถานะ")]
        public int? SSID { get; set; }

        [Display(Name = "แผนก")]
        public string DEP { get; set; }

        [Display(Name = "วันที่")]
        public string CREATEDATE { get; set; }

        [Display(Name = "เวลา")]
        public string CREATETIME { get; set; }

        [Display(Name = "วัตถุประสงค์ในการใช้เอกสาร")]
        public string DETAIL { get; set; }

        public bool Checked { get; set; }

        [Display(Name = "ผู้ขอเอกสาร")]
        public string NAME_OPEN { get; set; }
        public string DATE_OPEN { get; set; }

        [Display(Name = "ผู้อนุมัติขอเอกสาร")]
        public string NAME_HDEP { get; set; }
        public string DATE_HDEP { get; set; }

        [Display(Name = "ผู้ส่งเอกสาร")]
        public string NAME_RECEIVE { get; set; }
        public string DATE_RECEIVE { get; set; }

        [Display(Name = "ผู้อนุมัติส่งเอกสาร")]
        public string NAME_CLOSE { get; set; }
        public string DATE_CLOSE { get; set; }

        public int? APP_ID { get; set; }
    }


}