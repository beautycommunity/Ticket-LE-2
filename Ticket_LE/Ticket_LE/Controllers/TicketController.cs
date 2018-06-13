using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket_LE.DATA;
using Ticket_LE.Models;
using PagedList;
using RestSharp;
using RestSharp.Deserializers;
using Newtonsoft.Json;

namespace Ticket_LE.Controllers
{
    public class TicketController : Controller
    {
        string userOnline;
        // GET: Ticket
        public ActionResult Index(int page = 1, string seach = "", string type = "")
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            Detail cc = new Detail();
            //cc.Pos = Pos;
            cc.seach = seach;
            cc.type = type;
            cc.STCODE = userOnline;

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/Ticketlist");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(cc);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<Ticket> items = JsonConvert.DeserializeObject<List<Ticket>>(json);

            // ------------------------------------------------------------------------------------

            //IQueryable<VW_TICKET> View_Ticket;
            //seach = seach.Trim();
            //type = type.Trim();

            Data_UserDataContext Con = new Data_UserDataContext();
            DB_LEDataContext db = new DB_LEDataContext();
            List<Ticket> lstTicket = new List<Ticket>();
            ViewBag.Type = new SelectList(db.MAS_SSes, "SS_ID", "SS_NAME");

            var User = (from xx in Con.MAS_USERs
                        where xx.STCODE == userOnline
                        select xx).FirstOrDefault();

            if (User.D_ID != 10)
            {
                if(items != null)
                {
                    foreach (var item in items)
                    {
                        Ticket ux = new Ticket();

                        ux.ID = item.ID;
                        ux.TICKETNO = item.TICKETNO;
                        ux.DETAIL = item.DETAIL;
                        ux.CREATEDATE = DateTime.Parse(item.CREATEDATE.ToString()).ToShortDateString();
                        ux.CREATETIME = DateTime.Parse(item.CREATETIME.ToString()).ToLongTimeString();
                        ux.CRE_NICKNAME = item.CRE_NICKNAME;
                        ux.DEP = item.DEP;
                        ux.SSID = item.SSID;
                        ux.SSNAME = item.SSNAME;

                        lstTicket.Add(ux);
                    }
                }
            }
            else
            {
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        Ticket ux = new Ticket();

                        ux.ID = item.ID;
                        ux.TICKETNO = item.TICKETNO;
                        ux.DETAIL = item.DETAIL;
                        ux.CREATEDATE = DateTime.Parse(item.CREATEDATE.ToString()).ToShortDateString();
                        ux.CREATETIME = DateTime.Parse(item.CREATETIME.ToString()).ToLongTimeString();
                        ux.CRE_NICKNAME = item.CRE_NICKNAME;
                        ux.DEP = item.DEP;
                        ux.SSID = item.SSID;
                        ux.SSNAME = item.SSNAME;

                        lstTicket.Add(ux);
                    }
                }
            }

            ViewBag.DP = User.D_ID;
            ViewBag.A_ID = User.A_ID;
            ViewBag.WordSearch = seach;
            ViewBag.typeSearch = type;

            return View(lstTicket.ToPagedList(page, 10));
        }

        //Get : Create Ticket
        public ActionResult CreateTicket()
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/CreateTicketShow");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            //request.AddJsonBody(cc);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<TicketModels> items = JsonConvert.DeserializeObject<List<TicketModels>>(json);
            var Ans = items.FirstOrDefault();

            // ------------------------------------------------------------------------------------
            TicketModels valus = new TicketModels();

            List<CheckBox> list = new List<CheckBox>();

            ViewBag.Count = Ans.GetCheck.Count();
            ViewBag.Check = ViewBag.Count / 2;

            foreach (var item in Ans.GetCheck)
            {                     
                CheckBox ux = new CheckBox();
                ux.ID = item.ID;
                ux.Doc = item.Doc;
                ux.Type = item.Type;

                list.Add(ux);
            }

            valus.GetCheck = list;
      
            return View(valus);
        }

        //POST: Create Ticket
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateTicket(TicketModels newItem)
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            newItem.STCODE = userOnline;
            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/CreateTicket");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(newItem);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<Detail> items = JsonConvert.DeserializeObject<List<Detail>>(json);

            return RedirectToAction("Index", "Ticket");
        }

        public ActionResult TicketDetail(TicketModels Tic, int TicketId,string Url)
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            Tic.STCODE = userOnline;
            Tic.TicketId = TicketId;
            Tic.Url = Url;

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/TicketDetail");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(Tic);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<TicketModels> items = JsonConvert.DeserializeObject<List<TicketModels>>(json);
            var Ans = items.FirstOrDefault();

            // ------------------------------------------------------------------------------------

            TicketModels valus = new TicketModels();

            ViewBag.DP = Ans.DP;
            ViewBag.A_ID = Ans.A_ID;
            ViewBag.Back = Ans.Back;

            List<CheckBox> lstSub = new List<CheckBox>();

            int row = 1;
            foreach (var item in Ans.Detail)
            {
                CheckBox ux = new CheckBox();

                ux.row = row;
                ux.Doc = item.Doc;
                ux.NAME = item.NAME;
                ux.Type = item.Type;

                lstSub.Add(ux);
                row++;
            }

            valus.Detail = lstSub;

            Ticket lis = new Ticket();

            lis.TICKETNO = Ans.TicketSub.TICKETNO;
            lis.DETAIL = Ans.TicketSub.DETAIL;
            lis.CREATEDATE = DateTime.Parse(Ans.TicketSub.CREATEDATE.ToString()).ToShortDateString();
            lis.CREATETIME = DateTime.Parse(Ans.TicketSub.CREATETIME.ToString()).ToLongTimeString();
            lis.CRE_NICKNAME = Ans.TicketSub.CRE_NICKNAME;
            lis.DEP = Ans.TicketSub.DEP;
            lis.SSID = Ans.TicketSub.SSID;
            lis.SSNAME = Ans.TicketSub.SSNAME;
            lis.NAME_OPEN = Ans.TicketSub.NAME_OPEN;
            lis.DATE_OPEN = DateTime.Parse(Ans.TicketSub.DATE_OPEN.ToString()).ToShortDateString();
            lis.NAME_HDEP = Ans.TicketSub.NAME_HDEP;

            if (Ans.TicketSub.DATE_HDEP != null)
            {
                lis.DATE_HDEP = DateTime.Parse(Ans.TicketSub.DATE_HDEP.ToString()).ToShortDateString();
            }

            lis.NAME_RECEIVE = Ans.TicketSub.NAME_RECEIVE;
            if (Ans.TicketSub.DATE_RECEIVE != null)
            {
                lis.DATE_RECEIVE = DateTime.Parse(Ans.TicketSub.DATE_RECEIVE.ToString()).ToShortDateString();
            }

            lis.NAME_CLOSE = Ans.TicketSub.NAME_CLOSE;
            if (Ans.TicketSub.DATE_CLOSE != null)
            {
                lis.DATE_CLOSE = DateTime.Parse(Ans.TicketSub.DATE_CLOSE.ToString()).ToShortDateString();
            }

            lis.APP_ID = Ans.TicketSub.APP_ID;
            valus.TicketSub = lis;


            ViewBag.TicNo = TicketId;


            return View(valus);
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                Detail cc = new Detail();
                cc.STCODE = userOnline;
                cc.Ticket_ID = Id;

                //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
                var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/TicketDelete");

                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(cc);
                var response = restClient.Execute(request);
                var json = response.Content;

                JsonDeserializer deserial = new JsonDeserializer();
                List<Detail> items = JsonConvert.DeserializeObject<List<Detail>>(json);

                // ------------------------------------------------------------------------------------
            }

            catch
            {

            }

            return RedirectToAction("Index", "Ticket");
        }

        public ActionResult TicketReceive(int Id)
        {
            try
            {
                if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

                Detail cc = new Detail();
                cc.STCODE = userOnline;
                cc.Ticket_ID = Id;

                //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
                var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/TicketReceive");

                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(cc);
                var response = restClient.Execute(request);
                var json = response.Content;

                JsonDeserializer deserial = new JsonDeserializer();
                List<Detail> items = JsonConvert.DeserializeObject<List<Detail>>(json);

                // ------------------------------------------------------------------------------------
            }
            catch   
            {

            }

            return RedirectToAction("TicketDetail", "Ticket", new { TicketId = Id });
        }

        public ActionResult ApproveTicket(int page = 1, string seach = "", string type = "")
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            Detail cc = new Detail();
            cc.STCODE = userOnline;
            //cc.Ticket_ID = Id;
            cc.seach = seach;
            cc.type = type;

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/ApproveTicket");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(cc);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<TicketModels> items = JsonConvert.DeserializeObject<List<TicketModels>>(json);
            var Ans = items.FirstOrDefault();

            // ------------------------------------------------------------------------------------
            DB_LEDataContext db = new DB_LEDataContext();
            ViewBag.Type = new SelectList(db.MAS_SSes, "SS_ID", "SS_NAME"); ;
            ViewBag.DP = Ans.DP;

            List<Ticket> lstTicket = new List<Ticket>();

            if (Ans.TicketDetail != null)
            {
                foreach (var item in Ans.TicketDetail)
                {
                    Ticket ux = new Ticket();

                    ux.ID = item.ID;
                    ux.TICKETNO = item.TICKETNO;
                    ux.DETAIL = item.DETAIL;
                    ux.CREATEDATE = DateTime.Parse(item.CREATEDATE.ToString()).ToShortDateString();
                    ux.CREATETIME = DateTime.Parse(item.CREATETIME.ToString()).ToLongTimeString();
                    ux.CRE_NICKNAME = item.CRE_NICKNAME;
                    ux.DEP = item.DEP;
                    ux.SSID = item.SSID;
                    ux.SSNAME = item.SSNAME;

                    lstTicket.Add(ux);
                }
            } 
 
            ViewBag.WordSearch = Ans.WordSearch;
            ViewBag.typeSearch = Ans.typeSearch;

            return View(lstTicket.ToPagedList(page, 10));
        }

        public ActionResult Approve(int Id)
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            Detail cc = new Detail();
            cc.STCODE = userOnline;
            cc.Ticket_ID = Id;

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/Approve");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(cc);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<Detail> items = JsonConvert.DeserializeObject<List<Detail>>(json);

            // ------------------------------------------------------------------------------------
            //Data_UserDataContext Con = new Data_UserDataContext();
            //var User = (from xx in Con.MAS_USERs
            //                join yy in Con.MAS_DEPs on xx.D_ID equals yy.DP_ID
            //                where xx.STCODE == userOnline
            //                select new { xx, yy }).FirstOrDefault();

            //using (DB_LEDataContext Context = new DB_LEDataContext())
            //{
            //    var insert_Approve = Context.TASK_MAINs.Where(s => s.ID == Id).FirstOrDefault();

            //    insert_Approve.HDEP_NAME = User.xx.FNAME + " " + User.xx.LNAME;
            //    insert_Approve.HDEP_DATE = DateTime.Now;
            //    insert_Approve.APPROVE_ID = 2;
            //    Context.SubmitChanges();
            //}

            return RedirectToAction("TicketDetail", "Ticket", new { TicketId = Id });
        }

        public ActionResult TicketClose(int Id)
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            Detail cc = new Detail();
            cc.STCODE = userOnline;
            cc.Ticket_ID = Id;

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/TicketClose");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(cc);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<Detail> items = JsonConvert.DeserializeObject<List<Detail>>(json);

            // ------------------------------------------------------------------------------------

            //Data_UserDataContext Con = new Data_UserDataContext();
            //var User = (from xx in Con.MAS_USERs
            //            join yy in Con.MAS_DEPs on xx.D_ID equals yy.DP_ID
            //            where xx.STCODE == userOnline
            //            select new { xx, yy }).FirstOrDefault();

            //using (DB_LEDataContext Context = new DB_LEDataContext())
            //{
            //    var insert_Close = Context.TASK_MAINs.Where(s => s.ID == Id).FirstOrDefault();

            //    insert_Close.CLOSE_NAME = User.xx.FNAME + " " + User.xx.LNAME;
            //    insert_Close.CLOSE_DATE = DateTime.Now;
            //    insert_Close.APPROVE_ID = 4;
            //    insert_Close.STATUS = "3";
            //    Context.SubmitChanges();
            //}

            return RedirectToAction("TicketDetail", "Ticket", new { TicketId = Id });
        }

        //private string ticketNo()
        //{
        //    string runNo = "LE"; //IT17000009
        //    string strRun = "";
        //    string yy = DateTime.Now.Year.ToString();
        //    string mm = DateTime.Now.Month.ToString();
        //    int intRun = 1;


        //    yy = yy.Substring(yy.Length - 2, 2);
        //    if (mm.Length == 1) { mm = "0" + mm; }

        //    runNo = runNo + yy + mm;

        //    using (DB_LEDataContext Context = new DB_LEDataContext())
        //    {
        //        try
        //        {
        //            var queryX = Context.TASK_MAINs.OrderByDescending(s => s.TICKETNO)
        //            .Where(s => s.TICKETNO.Contains(runNo))
        //            .FirstOrDefault();
        //            strRun = queryX.TICKETNO;
        //        }
        //        catch
        //        {
        //            strRun = "LE18010000";
        //        }

        //    }

        //    strRun = strRun.Substring(strRun.Length - 4, 4);
        //    intRun = Int32.Parse(strRun);
        //    intRun = intRun + 1;

        //    strRun = intRun.ToString();

        //    switch (strRun.Length)
        //    {
        //        case 1:
        //            strRun = "000" + strRun;
        //            break;
        //        case 2:
        //            strRun = "00" + strRun;
        //            break;
        //        case 3:
        //            strRun = "0" + strRun;
        //            break;
        //    }

        //    runNo = runNo + strRun;

        //    return runNo;
        //}

        /*------------------------------------------------------------- Login ------------------------------------------------*/

        public bool chkSesionUser()
        {
            bool chk = true;

            userOnline = GetCookie();

            if (userOnline == string.Empty)
            {
                try
                {
                    userOnline = Session["User"].ToString();
                    if (userOnline.Length < 1)
                    {
                        chk = false;
                    }
                }
                catch
                {
                    chk = false;
                }
            }

            using (Data_UserDataContext Context = new Data_UserDataContext())
            {
                try
                {
                    var sql = (from xx in Context.MAS_USERs
                               where xx.STCODE == userOnline
                               select xx).FirstOrDefault();

                    if (sql != null)
                    {
                        Session["SharedName"] = "สวัสดี " + sql.FNAME;
                        Session["Name"] = sql.FNAME;
                    }
                    else
                    {
                        Session["SharedName"] = "เข้าสู่ระบบ";
                    }
                }
                catch
                {
                    Session["SharedName"] = "เข้าสู่ระบบ";
                }
            }

            return chk;
        }

        private void SetCookie(string User)
        {
            try
            {
                Request.Cookies["bbStcode"].Value = User;
            }
            catch
            {
                System.Web.HttpCookie BeautyCookies = new System.Web.HttpCookie("bbStcode");
                BeautyCookies.Value = User;
                BeautyCookies.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Add(BeautyCookies);
            }
            //Request.Cookies["bbStcode"].Value = User;   
        }

        private string GetCookie()
        {
            string cookievalue = string.Empty;

            if (Request.Cookies["bbStcode"] != null)
            {
                cookievalue = Request.Cookies["bbStcode"].Value.ToString();
            }

            return cookievalue;
        }

        private void RemoveCookie()
        {
            if (Request.Cookies["bbStcode"] != null)
            {
                Response.Cookies["bbStcode"].Expires = DateTime.Now.AddDays(-1);
            }
        }

        //------------------------------------------------------------------------------------------------------------

        public ActionResult Print(TicketModels Tic, int TicketId)
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            Tic.STCODE = userOnline;
            Tic.TicketId = TicketId;
            //Tic.Url = Url;

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/Print");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(Tic);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<TicketModels> items = JsonConvert.DeserializeObject<List<TicketModels>>(json);
            var Ans = items.FirstOrDefault();

            // ------------------------------------------------------------------------------------

            TicketModels valus = new TicketModels();

            ViewBag.DP = Ans.DP;
            ViewBag.DPNAME = Ans.DPNAME;
            ViewBag.FULLNAME = Ans.FULLNAME;

            List<CheckBox> lstSub = new List<CheckBox>();

            int row = 1;
            foreach (var item in Ans.Detail)
            {
                CheckBox ux = new CheckBox();

                ux.row = row;
                ux.Doc = item.Doc;
                ux.NAME = item.NAME;
                ux.Type = item.Type;

                lstSub.Add(ux);
                row++;
            }

            valus.Detail = lstSub;

            Ticket lis = new Ticket();

            lis.TICKETNO = Ans.TicketSub.TICKETNO;
            lis.DETAIL = Ans.TicketSub.DETAIL;
            lis.CREATEDATE = DateTime.Parse(Ans.TicketSub.CREATEDATE.ToString()).ToShortDateString();
            lis.CREATETIME = DateTime.Parse(Ans.TicketSub.CREATETIME.ToString()).ToLongTimeString();
            lis.CRE_NICKNAME = Ans.TicketSub.CRE_NICKNAME;
            lis.DEP = Ans.TicketSub.DEP;
            lis.SSID = Ans.TicketSub.SSID;
            lis.SSNAME = Ans.TicketSub.SSNAME;
            lis.NAME_OPEN = Ans.TicketSub.NAME_OPEN;
            lis.DATE_OPEN = DateTime.Parse(Ans.TicketSub.DATE_OPEN.ToString()).ToShortDateString();
            lis.NAME_HDEP = Ans.TicketSub.NAME_HDEP;

            if (Ans.TicketSub.DATE_HDEP != null)
            {
                lis.DATE_HDEP = DateTime.Parse(Ans.TicketSub.DATE_HDEP.ToString()).ToShortDateString();
            }

            lis.NAME_RECEIVE = Ans.TicketSub.NAME_RECEIVE;
            if (Ans.TicketSub.DATE_RECEIVE != null)
            {
                lis.DATE_RECEIVE = DateTime.Parse(Ans.TicketSub.DATE_RECEIVE.ToString()).ToShortDateString();
            }

            lis.NAME_CLOSE = Ans.TicketSub.NAME_CLOSE;
            if (Ans.TicketSub.DATE_CLOSE != null)
            {
                lis.DATE_CLOSE = DateTime.Parse(Ans.TicketSub.DATE_CLOSE.ToString()).ToShortDateString();
            }

            valus.TicketSub = lis;

            ViewBag.TicNo = TicketId;

            return View(valus);
        }

        public ActionResult Manage(int page = 1, string sh = "")
        {
            if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/Ticket/Index" }); }

            UserViewModels value = new UserViewModels();
            value.Page = page;
            value.sh = sh;

            return View(value);
        }

        public ActionResult Manage_Partial(int page = 1, string sh = "")
        {
            Detail cc = new Detail();
            cc.STCODE = userOnline;
            cc.sh = sh;
            //cc.Ticket_ID = Id;

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/Manage_Partial");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(cc);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<ListUserLogin> items = JsonConvert.DeserializeObject<List<ListUserLogin>>(json);
            var Ans = items.FirstOrDefault();

            // ------------------------------------------------------------------------------------
            Data_UserDataContext DropD = new Data_UserDataContext();
            var dep = (from xx in DropD.MAS_USER_As
                       orderby xx.US_ID
                       select xx).GroupBy(x => x.ANAME).Select(grp => grp.First());
            ViewBag.Dep = new SelectList(dep, "US_ID", "ANAME");

            List<USER_LOGIN> list = new List<USER_LOGIN>();

            if (Ans.Userloginid != null)
            {
                foreach (var dx in Ans.Userloginid)
                {
                    USER_LOGIN ux = new USER_LOGIN();

                    ux.ID = dx.ID;
                    ux.STCODE = dx.STCODE;
                    ux.FULLNAME = dx.FULLNAME;
                    ux.DEP = dx.DEP;
                    ux.A_NAME = dx.A_NAME;

                    list.Add(ux);
                    //PhoneModels.RowPhone.Add(ipn);
                }
            }
            ViewBag.sh = sh;

            return PartialView(list.ToPagedList(page, 20));
        }

        public ActionResult Edit(int Id,string STCODE, string Dep)
        {
            Detail cc = new Detail();
            cc.Dep = Dep;
            cc.Ticket_ID = Id;

            //cc.Ticket_ID = Id;

            //var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_OP/api/TicketOP/Ticketlist");
            var restClient = new RestClient("http://5cosmeda.homeunix.com:89/Ticket_LE/api/TicketLE/Edit");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(cc);
            var response = restClient.Execute(request);
            var json = response.Content;

            JsonDeserializer deserial = new JsonDeserializer();
            List<ListUserLogin> items = JsonConvert.DeserializeObject<List<ListUserLogin>>(json);
            var Ans = items.FirstOrDefault();

            // ------------------------------------------------------------------------------------
            //using (Data_UserDataContext Context = new Data_UserDataContext())
            //{
            //    var sql = (from xx in Context.MAS_USERs
            //              where xx.US_ID == Id
            //              select xx).FirstOrDefault();

            //    sql.A_ID = byte.Parse(Dep);
            //    Context.SubmitChanges();
            //}
            return RedirectToAction("Manage", "Ticket", new { sh = STCODE });
        }
    }
}