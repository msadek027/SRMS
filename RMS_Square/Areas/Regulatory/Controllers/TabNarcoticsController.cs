using RMS_Square.Areas.Regulatory.Models.BEL;
using RMS_Square.Areas.Regulatory.Models.DAO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Systems.Controllers;
using Systems.Models;

namespace RMS_Square.Areas.Regulatory.Controllers
{
    public class TabNarcoticsController : ControllerController
    {
        private FileDetailModel _fileModel = null;
        private static string _serverFilePath = string.Empty;
        private NarcoticInfoDAO _dalObj = null;
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

        public ActionResult UploadFile(string refLevel1, string refLevel2, string fileSize, string refNo)
        {
            try
            {
                var obj = PutUploadFile(refLevel1, refLevel2, fileSize, _serverFilePath, (int)Enums.E_FormFileType.NarcoticsEntryInfo, refNo);
                if (obj.Item1 == "S")
                {
                    _fileModel = new FileDetailModel();
                    _fileModel.FileName = obj.Item2.FileName;
                    _fileModel.FileCode = obj.Item2.FileCode;
                    _fileModel.FileSize = obj.Item2.FileSize;
                    _fileModel.FileType = obj.Item2.FileType;
                    _fileModel.RefNo = obj.Item2.RefNo;
                    _fileModel.RefLevel1 = obj.Item2.RefLevel1;
                    _fileModel.RefLevel2 = obj.Item2.RefLevel2;
                    _fileModel.Extention = obj.Item2.Extention;

                    bool isSave = SaveUploadFileInfo(_fileModel, Session["UserID"] as string);

                    if (isSave)
                    {
                        return Json(new { msgType = "FUS", Status = "Yes", FileList = GetFileByParameters(_fileModel).OrderByDescending(o => o.FileID) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { msgType = "FUE", Status = "Error saving file information to database", FileList = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (obj.Item1 == "L")
                {
                    return Json(new { msgType = "FLI", Status = "File size limit exceeded", FileList = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msgType = "FUE", Status = "Error uploading file", FileList = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { msgType = "FUE", Status = "Error: " + ex.Message, FileList = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetFileByRefId(string refLevel1, string refLevel2)
        {
            _fileModel = new FileDetailModel();
            _fileModel.FileType = (int)Enums.E_FormFileType.MeetingInfo;
            _fileModel.RefLevel1 = refLevel1;
            _fileModel.RefLevel2 = refLevel2;
            return Json(GetFileByParameters(_fileModel).OrderBy(o => o.FileID), JsonRequestBehavior.AllowGet);
        }
	}
}