﻿using RMS_Square.Areas.Regulatory.Models.BEL;
using RMS_Square.DAL.Gateway;
using RMS_Square.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Systems.Models;

namespace RMS_Square.Areas.Regulatory.Models.DAO
{
    public class CompanyLicenseDAO : ReturnData
    {
        private DBConnection _dbConn = null;
        private DBHelper _dbHelper = null;
        private IDGenerated _idGenerated = null;
        public CompanyLicenseDAO()
        {
            _dbConn = new DBConnection();
            _dbHelper = new DBHelper();
            _idGenerated = new IDGenerated();
        }
       
        public bool SaveUpdate(CompanyLicenseBEL model, string userId)
        {
            try
            {
                var query = new StringBuilder();
                if (model.CLID > 0)
                {
                    //U for update
                    // MaxID = model.CompLicenseSlNo;
                    ReturnMaxID = model.CLID;
                    MaxID = model.CompLicenseSlNo;
                    RefNo = model.RevisionNo;
                    IUMode = "U";
                    query.Append(" UPDATE COMPANY_LICENSE SET COMPANY_CODE='" + model.CompanyCode + "', LICENSE_NO='" + model.LicenseNo + "',SUBMISSION_TYPE='" + model.SubmissionType + "',");
                    query.Append(" SUBMISSION_DATE =(TO_DATE('" + model.SubmissionDate + "','dd/MM/yyyy')), INSPECTION_DATE =(TO_DATE('" + model.InspectionDate + "','dd/MM/yyyy')),");
                    query.Append(" VALID_UPTO =(TO_DATE('" + model.ValidUpto + "','dd/MM/yyyy')), APPROVAL_DATE =(TO_DATE('" + model.ApprovalDate + "','dd/MM/yyyy')), NOTIFICATION_DAYS ='" + model.AlarmDays + "',");
                    query.Append(" UPDATE_DATE =(TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "','dd/MM/yyyy HH24:mi:ss')),UPDATE_BY='" + userId + "'");
                    query.Append(" WHERE CLID='" + model.CLID + "'");
                }
                else
                { //I for Insert  
                    ReturnMaxID = _idGenerated.getMAXSL("COMPANY_LICENSE", "CLID");
                    MaxID = _idGenerated.getMAXID("COMPANY_LICENSE", "COMP_LICENSE_SLNO", "fm000000000");
                    string strCount = _dbHelper.GetValue("SELECT COUNT(1) AS RevisionNo FROM COMPANY_LICENSE  WHERE IS_DELETE='N' AND COMPANY_CODE='" + model.CompanyCode + "' GROUP BY COMPANY_CODE");
                    if (!string.IsNullOrEmpty(strCount))
                    {
                        RefNo = strCount;
                        model.SubmissionType = "Renewal";
                    }
                    else
                    {
                        RefNo = "0";
                        model.SubmissionType = "License";
                    }
                    
                    IUMode = "I";
                    query.Append(" INSERT INTO COMPANY_LICENSE(CLID,COMP_LICENSE_SLNO,COMPANY_CODE,LICENSE_NO,REVISION_NO,SUBMISSION_TYPE,SUBMISSION_DATE,INSPECTION_DATE,VALID_UPTO,APPROVAL_DATE,NOTIFICATION_DAYS,SET_BY,SET_ON,IS_DELETE) ");
                    query.Append(" VALUES( '" + ReturnMaxID + "','" + MaxID + "','" + model.CompanyCode + "','" + model.LicenseNo + "','" + RefNo + "','" + model.SubmissionType + "',(TO_DATE('" + model.SubmissionDate + "','dd/MM/yyyy')),(TO_DATE('" + model.InspectionDate + "','dd/MM/yyyy')),(TO_DATE('" + model.ValidUpto + "','dd/MM/yyyy')),(TO_DATE('" + model.ApprovalDate + "','dd/MM/yyyy')),");
                    query.Append(" '" + model.AlarmDays + "','" + userId + "',(TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "','dd/MM/yyyy HH24:mi:ss'))");
                    query.Append(" ,'N')");
                }
                if (_dbHelper.CmdExecute(_dbConn.SAConnStrReader(), query.ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception errorException)
            {
                throw errorException;
            }
        }

        public IList<CompanyLicenseBEL> GetAllInfo(CompanyLicenseBEL model, string orderBy)
        {
            var query = new StringBuilder();
            query.Append(" SELECT CL.CLID, CL.COMP_LICENSE_SLNO,CL.REVISION_NO,CL.COMPANY_CODE,CL.LICENSE_NO,CL.SUBMISSION_TYPE,TO_CHAR(CL.SUBMISSION_DATE, 'dd/mm/yyyy')SUBMISSION_DATE ,");
            query.Append(" TO_CHAR(CL.INSPECTION_DATE, 'dd/mm/yyyy')INSPECTION_DATE,TO_CHAR(CL.VALID_UPTO, 'dd/mm/yyyy')VALID_UPTO,TO_CHAR(CL.APPROVAL_DATE, 'dd/mm/yyyy')APPROVAL_DATE,CL.NOTIFICATION_DAYS,TO_CHAR(CL.SET_ON, 'dd/mm/yyyy')SET_ON,");
            query.Append(" C.COMPANY_NAME,C.ADDRESS");
            query.Append(" FROM COMPANY_LICENSE CL");
            query.Append(" LEFT JOIN  COMPANY_INFO C ON C.COMPANY_CODE=CL.COMPANY_CODE");
            query.Append(" WHERE IS_DELETE <>'Y'");
            
            if(!string.IsNullOrEmpty(model.CompanyCode))
            {
                query.Append(" AND  CL.COMPANY_CODE='{0}'");
            }
           
            if (!string.IsNullOrEmpty(model.LicenseNo))
            {
                query.Append(" AND  C.LICENSE_NO='{1}'");
            }
            if (!string.IsNullOrEmpty(model.SubmissionType) && !model.SubmissionType.Equals("All"))
            {
                query.Append(" AND  CL.SUBMISSION_TYPE='{2}'");
            }
            //if (!string.IsNullOrEmpty(model.AlarmDays))
            //{
            //    query.Append(" AND ROUND(((CL.VALID_UPTO)-(SELECT  SYSDATE FROM DUAL)),0) < {3}");//  TRUNC((SELECT  SYSDATE + {3} FROM DUAL))>TRUNC (CL.VALID_UPTO) 
            //}
            if (!string.IsNullOrEmpty(model.FromDate) && !string.IsNullOrEmpty(model.ToDate))
            {
                query.Append(" AND TO_DATE(CL.SUBMISSION_DATE,'dd/mm/yyyy') BETWEEN TO_DATE('" + model.FromDate + "','dd/MM/yyyy') AND TO_DATE('" + model.ToDate + "','dd/MM/yyyy') ");
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                query.Append(" ORDER BY  CL.CLID "+ orderBy);
            }
            DataTable dt = _dbHelper.GetDataTable(_dbConn.SAConnStrReader(),string.Format(query.ToString(),model.CompanyCode,model.LicenseNo,model.SubmissionType));

            var item = (from DataRow row in dt.Rows
                        select new CompanyLicenseBEL
                        {

                            CLID = Convert.ToInt64(row["CLID"]),
                            CompLicenseSlNo = row["COMP_LICENSE_SLNO"].ToString(),
                            CompanyCode = row["COMPANY_CODE"].ToString(),
                            CompanyName = row["COMPANY_NAME"].ToString(),
                            Address = row["ADDRESS"].ToString(),
                            LicenseNo = row["LICENSE_NO"].ToString(),
                            SubmissionType = row["SUBMISSION_TYPE"].ToString(),
                            SubmissionDate = row["SUBMISSION_DATE"].ToString(),
                            InspectionDate = row["INSPECTION_DATE"].ToString(),
                            ValidUpto = row["VALID_UPTO"].ToString(),
                            ApprovalDate = row["APPROVAL_DATE"].ToString(),
                            AlarmDays = row["NOTIFICATION_DAYS"].ToString(),
                            RevisionDate = row["SET_ON"].ToString(),
                            RevisionNo = row["REVISION_NO"].ToString()
                        }).ToList();
            return item;
        }
        public IList<CompanyLicenseBEL> GetAllInfo(CompanyLicenseBEL model,string CompanyCode, string orderBy)
        {
            var query = new StringBuilder();
            query.Append(" SELECT CL.CLID, CL.COMP_LICENSE_SLNO,CL.REVISION_NO,CL.COMPANY_CODE,CL.LICENSE_NO,CL.SUBMISSION_TYPE,TO_CHAR(CL.SUBMISSION_DATE, 'dd/mm/yyyy')SUBMISSION_DATE ,");
            query.Append(" TO_CHAR(CL.INSPECTION_DATE, 'dd/mm/yyyy')INSPECTION_DATE,TO_CHAR(CL.VALID_UPTO, 'dd/mm/yyyy')VALID_UPTO,TO_CHAR(CL.APPROVAL_DATE, 'dd/mm/yyyy')APPROVAL_DATE,CL.NOTIFICATION_DAYS,TO_CHAR(CL.SET_ON, 'dd/mm/yyyy')SET_ON,");
            query.Append(" C.COMPANY_NAME,C.ADDRESS");
            query.Append(" FROM COMPANY_LICENSE CL");
            query.Append(" LEFT JOIN  COMPANY_INFO C ON C.COMPANY_CODE=CL.COMPANY_CODE");
            query.Append(" WHERE IS_DELETE <>'Y'");

            if (!string.IsNullOrEmpty(model.CompanyCode))
            {
                query.Append(" AND  CL.COMPANY_CODE='{0}'");
            }

            if (!string.IsNullOrEmpty(model.LicenseNo))
            {
                query.Append(" AND  C.LICENSE_NO='{1}'");
            }
            if (!string.IsNullOrEmpty(model.SubmissionType) && !model.SubmissionType.Equals("All"))
            {
                query.Append(" AND  CL.SUBMISSION_TYPE='{2}'");
            }
            //if (!string.IsNullOrEmpty(model.AlarmDays))
            //{
            //    query.Append(" AND ROUND(((CL.VALID_UPTO)-(SELECT  SYSDATE FROM DUAL)),0) < {3}");//  TRUNC((SELECT  SYSDATE + {3} FROM DUAL))>TRUNC (CL.VALID_UPTO) 
            //}
            if (!string.IsNullOrEmpty(model.FromDate) && !string.IsNullOrEmpty(model.ToDate))
            {
                query.Append(" AND TO_DATE(CL.SUBMISSION_DATE,'dd/mm/yyyy') BETWEEN TO_DATE('" + model.FromDate + "','dd/MM/yyyy') AND TO_DATE('" + model.ToDate + "','dd/MM/yyyy') ");
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query.Append(" AND CL.COMPANY_CODE='" + CompanyCode + "'");
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query.Append(" ORDER BY  CL.CLID " + orderBy);
            }
            DataTable dt = _dbHelper.GetDataTable(_dbConn.SAConnStrReader(), string.Format(query.ToString(), model.CompanyCode, model.LicenseNo, model.SubmissionType));

            var item = (from DataRow row in dt.Rows
                        select new CompanyLicenseBEL
                        {

                            CLID = Convert.ToInt64(row["CLID"]),
                            CompLicenseSlNo = row["COMP_LICENSE_SLNO"].ToString(),
                            CompanyCode = row["COMPANY_CODE"].ToString(),
                            CompanyName = row["COMPANY_NAME"].ToString(),
                            Address = row["ADDRESS"].ToString(),
                            LicenseNo = row["LICENSE_NO"].ToString(),
                            SubmissionType = row["SUBMISSION_TYPE"].ToString(),
                            SubmissionDate = row["SUBMISSION_DATE"].ToString(),
                            InspectionDate = row["INSPECTION_DATE"].ToString(),
                            ValidUpto = row["VALID_UPTO"].ToString(),
                            ApprovalDate = row["APPROVAL_DATE"].ToString(),
                            AlarmDays = row["NOTIFICATION_DAYS"].ToString(),
                            RevisionDate = row["SET_ON"].ToString(),
                            RevisionNo = row["REVISION_NO"].ToString()
                        }).ToList();
            return item;
        }
        public IList<CompanyLicenseBEL> GetCompanyExpireLicense(CompanyLicenseBEL model, string orderBy)
        {
            var query = new StringBuilder();
            query.Append(" SELECT A.CLID, A.COMP_LICENSE_SLNO,A.REVISION_NO,A.COMPANY_CODE,A.LICENSE_NO,A.SUBMISSION_TYPE,A.SUBMISSION_DATE ,");
            query.Append(" ROUND(((NVL(A.VALID_UPTO, TO_DATE('12/31/2099', 'mm/dd/yyyy')))-(SELECT  SYSDATE FROM DUAL)),0)DateDiff,");
            query.Append(" A.INSPECTION_DATE,TO_CHAR(A.VALID_UPTO, 'dd/mm/yyyy')VALID_UPTO,A.APPROVAL_DATE,A.NOTIFICATION_DAYS,A.SET_ON, A.COMPANY_NAME,A.ADDRESS FROM");
            query.Append(" ( SELECT CL.CLID, CL.COMP_LICENSE_SLNO,CL.REVISION_NO,CL.COMPANY_CODE,CL.LICENSE_NO,CL.SUBMISSION_TYPE,");
            query.Append(" TO_CHAR(CL.SUBMISSION_DATE, 'dd/mm/yyyy')SUBMISSION_DATE , TO_CHAR(CL.INSPECTION_DATE, 'dd/mm/yyyy')INSPECTION_DATE,CL.VALID_UPTO,");
            query.Append(" TO_CHAR(CL.APPROVAL_DATE, 'dd/mm/yyyy')APPROVAL_DATE,CL.NOTIFICATION_DAYS,TO_CHAR(CL.SET_ON, 'dd/mm/yyyy')SET_ON, C.COMPANY_NAME,C.ADDRESS");
            query.Append(" FROM COMPANY_LICENSE CL LEFT JOIN  COMPANY_INFO C ON C.COMPANY_CODE=CL.COMPANY_CODE WHERE IS_DELETE <>'Y' ");
            query.Append(" ) A INNER JOIN ( SELECT COMPANY_CODE, MAX(REVISION_NO) AS MaxRvNo ");
            query.Append(" FROM COMPANY_LICENSE GROUP BY COMPANY_CODE) B");
            query.Append(" ON A.COMPANY_CODE=B.COMPANY_CODE AND A.REVISION_NO=B.MaxRvNo");
            query.Append(" WHERE 1=1 ");
           
            if (!string.IsNullOrEmpty(model.CompanyCode))
            {
                query.Append(" AND  A.COMPANY_CODE='{0}'");
            }
            if (!string.IsNullOrEmpty(model.SubmissionType) && !model.SubmissionType.Equals("All"))
            {
                query.Append(" AND  A.SUBMISSION_TYPE='{1}'");
            }
            if (!string.IsNullOrEmpty(model.AlarmDays))
            {
                query.Append(" AND ROUND(((NVL(A.VALID_UPTO, TO_DATE('12/31/2099', 'mm/dd/yyyy')))-(SELECT  SYSDATE FROM DUAL)),0) <= {2}");
            }
            if (!string.IsNullOrEmpty(model.FromDate) && !string.IsNullOrEmpty(model.ToDate))
            {
                query.Append(" AND A.VALID_UPTO BETWEEN TO_DATE('" + General.SetDateStrYYYYMMDD(model.FromDate) + "','yyyy/mm/dd') AND TO_DATE('" + General.SetDateStrYYYYMMDD(model.ToDate) + "','yyyy/mm/dd') ");
            }
            if (string.IsNullOrEmpty(model.CompanyCode) && string.IsNullOrEmpty(model.AlarmDays) && string.IsNullOrEmpty(model.FromDate) && string.IsNullOrEmpty(model.ToDate))
            {
                query.Append(" AND ROUND(((NVL(A.VALID_UPTO, TO_DATE('12/31/2099', 'mm/dd/yyyy')))-(SELECT  SYSDATE FROM DUAL)),0) <= A.NOTIFICATION_DAYS ");
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                query.Append(" ORDER BY  A.CLID " + orderBy);
            }
            DataTable dt = _dbHelper.GetDataTable(_dbConn.SAConnStrReader(), string.Format(query.ToString(), model.CompanyCode, model.SubmissionType, model.AlarmDays));

            try
            {
            var item = (from DataRow row in dt.Rows
                        select new CompanyLicenseBEL
                        {
                            CLID = Convert.ToInt64(row["CLID"]),
                            CompLicenseSlNo = row["COMP_LICENSE_SLNO"].ToString(),
                            CompanyCode = row["COMPANY_CODE"].ToString(),
                            CompanyName = row["COMPANY_NAME"].ToString(),
                            Address = row["ADDRESS"].ToString(),
                            LicenseNo = row["LICENSE_NO"].ToString(),
                            SubmissionType = row["SUBMISSION_TYPE"].ToString(),
                            SubmissionDate = row["SUBMISSION_DATE"].ToString(),
                            InspectionDate = row["INSPECTION_DATE"].ToString(),
                            ValidUpto = row["VALID_UPTO"].ToString(),
                            ApprovalDate = row["APPROVAL_DATE"].ToString(),
                            AlarmDays = row["NOTIFICATION_DAYS"].ToString(),
                            RevisionDate = row["SET_ON"].ToString(),
                            RevisionNo = row["REVISION_NO"].ToString(),
                            DateDiff = Convert.ToInt32(row["DateDiff"]),
                        }).ToList();
            return item;
                 }
            catch(Exception ex)
            {
                return null;
            }
        }
        
    }
}