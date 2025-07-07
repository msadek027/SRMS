using RMS_Square.Areas.Regulatory.Models.BEL;
using RMS_Square.DAL.Common;
using RMS_Square.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMS_Square.Areas.Regulatory.Models.DAO
{
    public class TabCompanyDAO: ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        DateFormat dateFormat = new DateFormat();

    
        public object CompanyDeatils(string CompanyCode)
        {
            string Qry = "SELECT COMPANY_CODE,COMPANY_NAME,ADDRESS,LICENSE_NO,CONTACT_NO,EMAIL_ID,FACILITY,LICENSE_NO from COMPANY_INFO";
            if (CompanyCode != "" && CompanyCode != null )
            {
                Qry = Qry + " Where COMPANY_CODE ='" + CompanyCode + "'";
            }      

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TabCompanyBEO> item;
            item = (from DataRow row in dt.Rows
                    select new TabCompanyBEO
                    {
                        CompanyCode = row["COMPANY_CODE"].ToString(),
                        CompanyName = row["COMPANY_NAME"].ToString(),
                        Address = row["ADDRESS"].ToString(),
                        ContactNo = row["CONTACT_NO"].ToString(),
                        EmailId = row["EMAIL_ID"].ToString(),
                        Facility = row["FACILITY"].ToString(),
                        LicenseNo = row["LICENSE_NO"].ToString()
                    

                    }).ToList();
            return item;
        }

        public string AccessPrivilege(string RoleId,string PageNo)
        {
            string qry = "SELECT ACCESSLEVEL FROM SA_PAGE_ACCESS_INFO WHERE ROLEID='" + RoleId + "' AND FORMID='" + PageNo + "'";
            string value = dbHelper.GetValueFn(dbConn.SAConnStrReader(), qry);
     
            return value;
        }
    }
}