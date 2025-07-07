using RMS_Square.Areas.Regulatory.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Square.Areas.Regulatory.Controllers
{
    public class TabCompanyController : Controller
    {
        TabCompanyDAO primaryDAO = new TabCompanyDAO();
        public ActionResult frmTabCompany()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return Redirect(string.Format("~/Home/frmHome"));
        }
        [HttpPost]
        public ActionResult ViewModeCompany(string CompanyCode, string ButtonEvent)
        {
            object data;
            switch (ButtonEvent)
            {
                case "Entry License Info":
                    data = primaryDAO.CompanyDeatils(CompanyCode);
                    break;



                default:
                    data = primaryDAO.CompanyDeatils(CompanyCode);
                    break;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AccessPrivilege(string RoleId,string PageNo)
        {
            object data = primaryDAO.AccessPrivilege(RoleId, PageNo);

          
            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}