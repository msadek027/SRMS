using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Square.Areas.Regulatory.Controllers
{
    public class TabNarcoticsController : Controller
    {
        //
        // GET: /Regulatory/TabNarcotics/
        public ActionResult frmTabNarcotics()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return Redirect(string.Format("~/Home/frmHome"));
        }
	}
}