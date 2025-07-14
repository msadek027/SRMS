using RMS_Square.Areas.Regulatory.Models.BEL;
using RMS_Square.Areas.Regulatory.Models.DAO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Square.Areas.Regulatory.Controllers
{
    public class TabNarcoticsController : Controller
    {
        NarcoticInfoDAO primaryDAO = new NarcoticInfoDAO();
        //
        // GET: /Regulatory/TabNarcotics/
        public ActionResult frmTabNarcotics()
        {
            return View();
        }

        [HttpPost]
        public ActionResult frmTabNarcotics(NarcoticSetupInfo master)
        {
            try
            {

                string userId = Session["UserID"] as string;
                if (primaryDAO.SaveUpdate(master, userId))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmRole");
            }
            catch (Exception e)
            {
                if (e.Message.Substring(0, 9) == "ORA-00001")
                    return Json(new { Status = "Error:ORA-00001,Data already exists!" });//Unique Identifier.
                else if (e.Message.Substring(0, 9) == "ORA-02292")
                    return Json(new { Status = "Error:ORA-02292,Data already exists!" });//Child Record Found.
                else if (e.Message.Substring(0, 9) == "ORA-12899")
                    return Json(new { Status = "Error:ORA-12899,Data Value Too Large!" });//Value Too Large.
                else
                    return Json(new { Status = "! Error : Error Code:" + e.Message.Substring(0, 9) });//Other Wise Error Found

            }
        }

        [HttpPost]
        public ActionResult SaveTab3EntryItem(NarcoticEntryItemInfo model)
        {
            try
            {
                string userId = Session["UserID"] as string;
                if (primaryDAO.SaveUpdateTab3(model, userId))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes", GenericBrandId = model.GenericBrandId});
                }
                else
                {
                    return Json(new { Status = "Failed to save entry item!" });
                }
            }
            catch (Exception e)
            {
                if (e.Message.StartsWith("ORA-00001"))
                    return Json(new { Status = "Error:ORA-00001, Data already exists!" });
                else if (e.Message.StartsWith("ORA-02292"))
                    return Json(new { Status = "Error:ORA-02292, Child record found!" });
                else if (e.Message.StartsWith("ORA-12899"))
                    return Json(new { Status = "Error:ORA-12899, Data value too large!" });
                else
                    return Json(new { Status = "! Error : " + e.Message });
            }
        }

        [HttpPost]
        public ActionResult GetNarcoticItems(string CompanyCode, string ButtonEvent)
        {
            var data = primaryDAO.GetNarcoticLists();
               return Json(data, JsonRequestBehavior.AllowGet);
       } 

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllItems()
        {
            var data = new NarcoticInfoDAO().GetItemList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet] // Explicitly specify this is a GET endpoint
        public ActionResult GetViewItems()
        {
            try
            {
                var data = primaryDAO.GetViewLists();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred while fetching narcotic items" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet] // Explicitly specify this is a GET endpoint
        public ActionResult GetEntryInfoItems()
        {
            try
            {
                var data = primaryDAO.GetEntryInfoLists();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred while fetching narcotic items" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UploadNarcoticFile(decimal entryInfoId, string documentName)
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var upload = Request.Files[file];
                    if (upload != null && upload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        string folderPath = Server.MapPath("~/UploadedFiles/Narcotics");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        string filePath = Path.Combine(folderPath, fileName);
                        upload.SaveAs(filePath);

                        // Save in DB
                        DocumentUpload model = new DocumentUpload()
                        {
                            EntryInfoId = entryInfoId,
                            DocumentName = documentName,
                            FilePath = "/UploadedFiles/Narcotics/" + fileName
                        };

                        string userId = Session["UserID"] as string;
                        bool saved = primaryDAO.SaveDocumentUpload(model, userId);

                        if (!saved)
                        {
                            return Json(new { msgType = "FUE" });
                        }
                    }
                }

                return Json(new { msgType = "FUS" });
            }
            catch (Exception ex)
            {
                return Json(new { msgType = "FUE", error = ex.Message });
            }
        }

	}
}