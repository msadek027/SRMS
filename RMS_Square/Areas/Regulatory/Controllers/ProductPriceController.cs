﻿using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using RMS_Square.Areas.Regulatory.Models.BEL;
using RMS_Square.Areas.Regulatory.Models.DAO;
using RMS_Square.DAL.Common;
using RMS_Square.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Systems.ActionFilter;
using Systems.Controllers;
using Systems.Models;

namespace RMS_Square.Areas.Regulatory.Controllers
{
    public class ProductPriceController : ControllerController
    {
        private FileDetailModel _fileModel = null;
        private static string _serverFilePath = string.Empty;
        private ProductPriceDAO _dalObj = null;

        public ProductPriceController()
        {
            _serverFilePath = Utility.GetServerPath();
            _dalObj = new ProductPriceDAO();
        }
        //
        // GET: /Regulatory/ProductPrice/
        [ActionAuth]
        public ActionResult frmProductPrice()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.PageAccess = GetAccessLevel(Session["ROLEID"] as string, "156");
                return View();
            }
            return Redirect(string.Format("~/Home/frmHome"));
        }
        [HttpPost]
        public ActionResult frmProductPrice(ProductPriceBEL model)
        {
            try
            {
                if (_dalObj.SaveUpdate(model, userId: Session["UserID"] as string))
                {
                    return Json(new { ID = _dalObj.ReturnMaxID, SlNo = _dalObj.MaxID, Mode = _dalObj.IUMode, Status = "Yes", RevisionNo = _dalObj.RefNo });
                }
                return View();
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
        public ActionResult UploadFile(string refLevel1, string refLevel2, string fileSize, string refNo)
        {

            var obj = PutUploadFile(refLevel1, refLevel2, fileSize, _serverFilePath, (int)Enums.E_FormFileType.PriceInfo, refNo);
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

                var model = new ProductPriceBEL();
                if (isSave)
                {
                    DataTable dt = _dalObj.GetFileRefno((int)Enums.E_FormFileType.PriceInfo, refLevel1);
                    bool isAll = false;
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["BL"].ToString()) > 0 && Convert.ToInt32(dt.Rows[0]["I"].ToString()) > 0 && Convert.ToInt32(dt.Rows[0]["IHA"].ToString()) > 0 && Convert.ToInt32(dt.Rows[0]["J"].ToString()) > 0)
                        {
                            isAll = true;
                        }
                    }
                    if (isAll)//
                    {
                        model.ID = Convert.ToInt64(refLevel1);
                        model.ReceivedDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        _dalObj.UpdateFileRelatedInfo(model, userId: Session["UserID"] as string);
                    }
                }

                return Json(new { msgType = "FUS", ReceiveDate = model.ReceivedDate, FileList = GetFileByParameters(_fileModel).OrderByDescending(o => o.FileID) }, JsonRequestBehavior.AllowGet);
            
            }
            else if (obj.Item1 == "L")
            {
                return Json(new { msgType = "FLI", FileList = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { msgType = "FUE", FileList = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetFileByRefId(string refLevel1, string refLevel2)
        {
            _fileModel = new FileDetailModel();
            _fileModel.FileType = (int)Enums.E_FormFileType.PriceInfo;
            _fileModel.RefLevel1 = refLevel1;
            _fileModel.RefLevel2 = refLevel2;
            return Json(GetFileByParameters(_fileModel).OrderBy(o => o.FileID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAllInfo(string companyCode)
        {
            var data = _dalObj.GetAllInfo(new ProductPriceBEL(), companyCode,orderBy: "DESC");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
          [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllProductFromAnnex()
        {
            var data = _dalObj.GetAllProductFromAnnex(orderBy: "DESC"); 
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [ActionAuth]
        public ActionResult frmProductPriceView()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return Redirect(string.Format("~/Home/frmHome"));
        }
        [HttpPost]
        public ActionResult GetInfoByParams(ProductPriceBEL model,string companyCode)
        {
            var dMaster = _dalObj.GetAllInfo(model,companyCode, orderBy: "DESC");
            if (dMaster.Any())
            {
                _fileModel = new FileDetailModel();
                var refL1 = dMaster.FirstOrDefault().ID;
                _fileModel.RefLevel1 = refL1.ToString();
                _fileModel.FileType = (int)Enums.E_FormFileType.PriceInfo;
                var dLevel1 = GetFileByParameters(_fileModel).OrderBy(o => o.FileID);
                return Json(new { dataMaster = dMaster, dataLevel1 = dLevel1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { dataMaster = "", dataLevel1 = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult frmProductPriceRpt()
        {
            if (Session["UserID"] != null)
            {
                Session["FormNameTitle"] = "Product Price Report";
                return View();
            }
            return Redirect(string.Format("~/Home/frmHome"));
        }
        [HttpPost]
        public ActionResult frmProductPriceRpt(ReportModel model)
        {
            if (Session["UserId"] != null)
            {
                var reportDocument = new ReportDocument();
                var rptPath = Server.MapPath("~/Reports");
                DataTable dt = new DataTable();

                string downFileName = string.Empty;
                string fromTodate = "_From_" + model.FromDate + "_To_" + model.ToDate + "_";
                dt = _dalObj.GetProductPriceForReport(model, "ASC");
                rptPath = rptPath + "/ProductPriceRpt.rpt";
                string reportName = "ProductPriceRpt" + "_" + fromTodate;
                reportDocument.Load(rptPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();
                reportDocument.SetParameterValue("CompanyName", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DevBy", Session["DEV_BY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate ?? "");
                reportDocument.SetParameterValue("ToDate", model.ToDate ?? "");
                reportDocument.SetParameterValue("PriceChangeType", model.PriceChangeType ?? "");
                reportDocument.SetParameterValue("ChooseOption", model.ChooseOption ?? "");
                reportDocument.SetParameterValue("PriceCategory", model.PriceCategory ?? "");
                reportDocument.SetParameterValue("PriceType", model.PriceType ?? "");
                downFileName = reportName + DateTime.Now.ToString("yyyyMMdd'_'HHmmss");
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                return View();
            }
            return Redirect(string.Format("~/Home/frmHome"));
        }
    }
}