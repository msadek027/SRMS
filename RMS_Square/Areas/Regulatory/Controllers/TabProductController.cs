using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Square.Areas.Regulatory.Controllers
{
    public class TabProductController : Controller
    {
        //
        // GET: /Regulatory/TabProduct/
        public ActionResult frmTabProduct()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult ViewMode(string EmpID, string ButtonEvent, string ViewMode)
        //{
        //    object data;
        //    switch (ButtonEvent)
        //    {
        //        case "EmpContactAddress":
        //            data = primaryDAO.GetEmployeeBasic(EmpID, ViewMode);
        //            break;

        

        //        default:
        //            data = primaryDAO.GetEmployeeBasic(EmpID, ViewMode);
        //            break;
        //    }
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
	}
}