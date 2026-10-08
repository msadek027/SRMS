using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Square.Areas.Regulatory.Controllers
{
    public class DeleteOperationController : Controller
    {
        //
        // GET: /Regulatory/DeleteOperation/
        public ActionResult frmDeleteOperation()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return Redirect(string.Format("~/Home/frmHome"));
        }
	}
}