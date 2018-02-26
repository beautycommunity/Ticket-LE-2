using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket_LE.DATA;
using Ticket_LE.Models;

namespace Ticket_LE.Controllers
{
    public class LoginController : Controller
    {
        string userOnline;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            userOnline = GetCookie();
            if (userOnline != string.Empty)
            {
                return RedirectToLocal(returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USER_LOGIN model, string returnUrl)
        {
            int LoginCase = 0;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (LoginBase(model))
            {
                LoginCase = 1;

                if (checkedepartment(model))
                {
                    LoginCase = 2;
                }

            }


            switch (LoginCase)
            {
                case 1: // Complete Redirect
                    Session["User"] = model.STCODE;
                    TempData["User"] = model.STCODE;
                    SetCookie(model.STCODE);
                    return RedirectToLocal(returnUrl);
                case 2: //Lock out
                    return RedirectToAction("updatedid", model);
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        private bool checkedepartment(USER_LOGIN model)
        {

            Data_UserDataContext Context = new Data_UserDataContext();

            var queryX = Context.MAS_USERs.Where(x => x.STCODE == model.STCODE && x.PASSWORD == model.PASSWORD).FirstOrDefault();

            if (queryX.D_ID == 0)
            {

                return true;

            }
            else
            {

                return false;

            }

        }

        public bool LoginBase(USER_LOGIN model)
        {
            bool OK = false;

            Data_UserDataContext Context = new Data_UserDataContext();

            var queryX = Context.MAS_USERs.Where(x => x.STCODE == model.STCODE && x.PASSWORD == model.PASSWORD).Count();

            if (queryX > 0)
            {

                OK = true;
            }

            return OK;
        }

        public ActionResult updatedid(USER_LOGIN model)
        {
            Dp_List ul = new Dp_List();
            Data_UserDataContext db = new Data_UserDataContext();

            ul.STCODE = model.STCODE;
            ul.JT_LIST = new SelectList(db.MAS_DEPs, "DP_ID", "DEPARTMENT");

            return View(ul);
        }

        [HttpPost]
        public ActionResult updatedid(Dp_List model)
        {

            //if (!chkSesionUser()) { return RedirectToAction("Login", "Login", new { returnUrl = "~/TicketApprove/Index" }); }
            using (Data_UserDataContext db = new Data_UserDataContext())
            {

                var q = (from pp in db.MAS_USERs
                         where pp.STCODE == model.STCODE
                         select pp).FirstOrDefault();

                q.D_ID = (int)model.JT_ID;

                db.SubmitChanges();

            }

            return RedirectToAction("Index", "Ticket");
        }



        // GET:


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Ticket");
        }

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
                HttpCookie BeautyCookies = new HttpCookie("bbStcode");
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

        public ActionResult Logout()
        {
            try
            {
                Session["User"] = null;
                Session["DP"] = null;
                Session["SharedName"] = null;
                RemoveCookie();
            }
            catch
            {
                Session["User"] = "";
                Session["DP"] = "";
                Session["SharedName"] = "";
                RemoveCookie();
                return RedirectToAction("Login", "Login");
            }

            return RedirectToAction("Login", "Login");
        }
    }
}