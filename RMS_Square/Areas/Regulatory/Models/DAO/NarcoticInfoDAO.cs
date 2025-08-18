using RMS_Square.Areas.Regulatory.Models.BEL;
using RMS_Square.DAL.Gateway;
using RMS_Square.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace RMS_Square.Areas.Regulatory.Models.DAO
{
    public class NarcoticInfoDAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();

        public bool SaveUpdate(NarcoticSetupInfo master, string userId)
        {
            try
            {
                string Qry = "";
                string setOndate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                if (master.NarcoticSetupSl == null || master.NarcoticSetupSl == 0)
                {//I for Insert  
                    MaxID = idGenerated.getMAXID("NARCOTIC_SETUP_INFO", "NARCOTIC_SETUP_SL", "fm0000");
                    IUMode = "I";
                    Qry = "Insert into NARCOTIC_SETUP_INFO(NARCOTIC_SETUP_SL,GENERIC_NAME, " +
                          " SET_BY,SET_ON)" +
                          "values(" + MaxID + ", '" + master.GenericName + "','" + userId + "',TO_DATE('" + setOndate + "','dd/MM/yyyy HH24:mi:ss'))";
                }
                else // Update
                {
                    MaxID = master.NarcoticSetupSl.ToString();
                    IUMode = "U";
                    Qry = "UPDATE NARCOTIC_SETUP_INFO SET " +
                          "GENERIC_NAME = '" + master.GenericName.Replace("'", "''") + "', " +
                          "UPDATED_BY = '" + userId + "', " +
                          "UPDATED_DATE = TO_DATE('" + setOndate + "','dd/MM/yyyy HH24:mi:ss') " +
                          "WHERE NARCOTIC_SETUP_SL = " + MaxID;
                }

                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
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

        public bool SaveUpdateTab3(NarcoticEntryItemInfo master, string userId)
        {
            try
            {
                string Qry = "";
                string setOndate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                // Generate MaxID for NARCOTIC_ENTRY_SL
                MaxID = idGenerated.getMAXID("NARCOTIC_ENTRY_ITEM_INFO", "NARCOTIC_ENTRY_SL", "fm0000");
                IUMode = "I";
                if (master.NarcoticEntrySl == 0) // Insert
                {
                    Qry = "INSERT INTO NARCOTIC_ENTRY_ITEM_INFO (NARCOTIC_ENTRY_SL, GENERIC_BRAND_ID, ITEM_NAME, FISCAL_YEAR, ANNUAL_QUOTA, SUBMISSION_TYPE, " +
                          "SUBMISSION_QUANTITY, APPROVED_QUANTITY, RECORD_STATUS, DGDA_RECEIVE_DATE, DGDA_SUBMISSION_DATE, DGDA_RECOMMENDATION_DATE, " +
                          "REC_SEND_DATE, INS_RECEIVE_DATE, DIV_SEND_DATE, DIV_NARC_RECV_DATE, DNC_SEND_DATE, NARC_APVL_DATE,COMPANY_CODE, SET_BY, SET_ON) VALUES (" +
                          MaxID + "," +
                          master.GenericBrandId + "," +
                          (master.ItemName != null ? "'" + master.ItemName + "'" : "NULL") + "," +
                          (master.FiscalYear != null ? "'" + master.FiscalYear + "'" : "NULL") + "," +
                          (master.AnnualQuota != null ? master.AnnualQuota.ToString() : "NULL") + "," +
                          (master.SubmissionType != null ? "'" + master.SubmissionType + "'" : "NULL") + "," +
                          (master.SubmissionQuantity != null ? master.SubmissionQuantity.ToString() : "NULL") + "," +
                          (master.ApprovedQuantity != null ? master.ApprovedQuantity.ToString() : "NULL") + "," +
                          (master.RecordStatus != null ? "'" + master.RecordStatus + "'" : "NULL") + "," +
                          (master.DgdaReceiveDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DgdaReceiveDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          (master.DgdaSubmissionDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DgdaSubmissionDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          (master.DgdaRecommendationDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DgdaRecommendationDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          (master.RecSendDate != null ? "TO_DATE('" + Convert.ToDateTime(master.RecSendDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          (master.InsReceiveDate != null ? "TO_DATE('" + Convert.ToDateTime(master.InsReceiveDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          (master.DivSendDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DivSendDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          (master.DivNarcRecvDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DivNarcRecvDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          (master.DncSendDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DncSendDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          (master.NarcApvlDate != null ? "TO_DATE('" + Convert.ToDateTime(master.NarcApvlDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                           (master.CompanyCode != null ? "'" + master.CompanyCode.Replace("'", "''") + "'" : "NULL") + "," +
                          "'" + userId + "', TO_DATE('" + setOndate + "','dd/MM/yyyy HH24:mi:ss'))";
                }
                else // Update
                {
                    MaxID = master.NarcoticEntrySl.ToString();
                    IUMode = "U";

                    Qry = "UPDATE NARCOTIC_ENTRY_ITEM_INFO SET " +
                          "GENERIC_BRAND_ID = " + master.GenericBrandId + "," +
                          "ITEM_NAME = " + (master.ItemName != null ? "'" + master.ItemName.Replace("'", "''") + "'" : "NULL") + "," +
                          "FISCAL_YEAR = " + (master.FiscalYear != null ? "'" + master.FiscalYear + "'" : "NULL") + "," +
                          "ANNUAL_QUOTA = " + (master.AnnualQuota != null ? master.AnnualQuota.ToString() : "NULL") + "," +
                          "SUBMISSION_TYPE = " + (master.SubmissionType != null ? "'" + master.SubmissionType + "'" : "NULL") + "," +
                          "SUBMISSION_QUANTITY = " + (master.SubmissionQuantity != null ? master.SubmissionQuantity.ToString() : "NULL") + "," +
                          "APPROVED_QUANTITY = " + (master.ApprovedQuantity != null ? master.ApprovedQuantity.ToString() : "NULL") + "," +
                          "RECORD_STATUS = " + (master.RecordStatus != null ? "'" + master.RecordStatus + "'" : "NULL") + "," +
                          "DGDA_RECEIVE_DATE = " + (master.DgdaReceiveDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DgdaReceiveDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "DGDA_SUBMISSION_DATE = " + (master.DgdaSubmissionDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DgdaSubmissionDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "DGDA_RECOMMENDATION_DATE = " + (master.DgdaRecommendationDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DgdaRecommendationDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "REC_SEND_DATE = " + (master.RecSendDate != null ? "TO_DATE('" + Convert.ToDateTime(master.RecSendDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "INS_RECEIVE_DATE = " + (master.InsReceiveDate != null ? "TO_DATE('" + Convert.ToDateTime(master.InsReceiveDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "DIV_SEND_DATE = " + (master.DivSendDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DivSendDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "DIV_NARC_RECV_DATE = " + (master.DivNarcRecvDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DivNarcRecvDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "DNC_SEND_DATE = " + (master.DncSendDate != null ? "TO_DATE('" + Convert.ToDateTime(master.DncSendDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "NARC_APVL_DATE = " + (master.NarcApvlDate != null ? "TO_DATE('" + Convert.ToDateTime(master.NarcApvlDate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')" : "NULL") + "," +
                          "COMPANY_CODE = " + (master.CompanyCode != null ? "'" + master.CompanyCode.Replace("'", "''") + "'" : "NULL") + "," +
                          "UPDATED_BY = '" + userId + "', " +
                          "UPDATED_DATE = TO_DATE('" + setOndate + "','dd/MM/yyyy HH24:mi:ss') " +
                          "WHERE NARCOTIC_ENTRY_SL = " + MaxID;
                }
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<NarcoticSetupInfo> GetNarcoticLists()
        {
            string Qry = "SELECT NARCOTIC_SETUP_SL, GENERIC_NAME, SET_ON, SET_BY " +
                         "FROM NARCOTIC_SETUP_INFO " +
                         "ORDER BY NARCOTIC_SETUP_SL ASC";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<NarcoticSetupInfo> item;

            item = (from DataRow row in dt.Rows
                    select new NarcoticSetupInfo
                    {
                        NarcoticSetupSl = row["NARCOTIC_SETUP_SL"] != DBNull.Value ? Convert.ToDecimal(row["NARCOTIC_SETUP_SL"]) : 0,
                        GenericName = row["GENERIC_NAME"].ToString(),
                        SetOn = row["SET_ON"] != DBNull.Value ? Convert.ToDateTime(row["SET_ON"]) : (DateTime?)null,
                        SetBy = row["SET_BY"].ToString(),
                    }).ToList();

            return item;
        }

        public List<NarcoticSetupInfo> GetItemList()
        {
            string Qry = "SELECT NARCOTIC_SETUP_SL, GENERIC_NAME, SET_ON, SET_BY " +
                         "FROM NARCOTIC_SETUP_INFO " + // Added space here
                         "ORDER BY NARCOTIC_SETUP_SL ASC";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<NarcoticSetupInfo> item;

            item = (from DataRow row in dt.Rows
                    select new NarcoticSetupInfo
                    {
                        NarcoticSetupSl = row["NARCOTIC_SETUP_SL"] != DBNull.Value ? Convert.ToDecimal(row["NARCOTIC_SETUP_SL"]) : 0,
                        GenericName = row["GENERIC_NAME"].ToString(),
                        SetOn = row["SET_ON"] != DBNull.Value ? Convert.ToDateTime(row["SET_ON"]) : (DateTime?)null,
                        SetBy = row["SET_BY"].ToString(),
                    }).ToList();

            return item;
        }

        public List<NarcoticSetupInfo> GetViewLists()
        {
            const string query = @"
                SELECT NARCOTIC_SETUP_SL, GENERIC_NAME, SET_ON, SET_BY 
                FROM NARCOTIC_SETUP_INFO 
                ORDER BY NARCOTIC_SETUP_SL ASC";

            try
            {
                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), query);

                return dt.AsEnumerable().Select(row => new NarcoticSetupInfo
                {
                    NarcoticSetupSl = row.Field<decimal?>("NARCOTIC_SETUP_SL") ?? 0,
                    GenericName = row.Field<string>("GENERIC_NAME") ?? string.Empty,
                    SetOn = row.Field<DateTime?>("SET_ON"),
                    SetBy = row.Field<string>("SET_BY") ?? string.Empty
                }).ToList();
            }
            catch (Exception ex)
            {
                throw; // Re-throw to be handled by the controller
            }
        }

        public List<NarcoticEntryItemInfo> GetEntryInfoLists()
        {
            const string query = @" SELECT NARCOTIC_ENTRY_SL,
                                        GENERIC_BRAND_ID,
                                        ITEM_NAME,
                                        FISCAL_YEAR,
                                        ANNUAL_QUOTA,
                                        SUBMISSION_TYPE,
                                        SUBMISSION_QUANTITY,
                                        APPROVED_QUANTITY,
                                        RECORD_STATUS,
                                        DGDA_RECEIVE_DATE,
                                        DGDA_SUBMISSION_DATE,
                                        DGDA_RECOMMENDATION_DATE,
                                        REC_SEND_DATE,
                                        INS_RECEIVE_DATE,
                                        DIV_SEND_DATE,
                                        DIV_NARC_RECV_DATE,
                                        DNC_SEND_DATE,
                                        NARC_APVL_DATE,
                                        SET_BY,
                                        SET_ON,
                                        UPDATED_DATE,
                                        UPDATED_BY
                                    FROM NARCOTIC_ENTRY_ITEM_INFO
                                    ORDER BY NARCOTIC_ENTRY_SL ASC";

            try
            {
                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), query);

                return dt.AsEnumerable().Select(row => new NarcoticEntryItemInfo
                {
                    NarcoticEntrySl = row.Field<decimal?>("NARCOTIC_ENTRY_SL") ?? 0,
                    GenericBrandId = row.Field<decimal?>("GENERIC_BRAND_ID") ?? 0,
                    ItemName = row.Field<string>("ITEM_NAME") ?? string.Empty,
                    FiscalYear = row.Field<string>("FISCAL_YEAR") ?? string.Empty,
                    AnnualQuota = row.Field<decimal?>("ANNUAL_QUOTA"),
                    SubmissionType = row.Field<string>("SUBMISSION_TYPE") ?? string.Empty,
                    SubmissionQuantity = row.Field<decimal?>("SUBMISSION_QUANTITY"),
                    ApprovedQuantity = row.Field<decimal?>("APPROVED_QUANTITY"),
                    RecordStatus = row.Field<string>("RECORD_STATUS") ?? string.Empty,

                    DgdaReceiveDate = row.Field<DateTime?>("DGDA_RECEIVE_DATE"),
                    DgdaSubmissionDate = row.Field<DateTime?>("DGDA_SUBMISSION_DATE"),
                    DgdaRecommendationDate = row.Field<DateTime?>("DGDA_RECOMMENDATION_DATE"),
                    RecSendDate = row.Field<DateTime?>("REC_SEND_DATE"),
                    InsReceiveDate = row.Field<DateTime?>("INS_RECEIVE_DATE"),
                    DivSendDate = row.Field<DateTime?>("DIV_SEND_DATE"),
                    DivNarcRecvDate = row.Field<DateTime?>("DIV_NARC_RECV_DATE"),
                    DncSendDate = row.Field<DateTime?>("DNC_SEND_DATE"),
                    NarcApvlDate = row.Field<DateTime?>("NARC_APVL_DATE"),

                    SetBy = row.Field<string>("SET_BY") ?? string.Empty,
                    SetOn = row.Field<DateTime?>("SET_ON"),
                    UpdatedDate = row.Field<DateTime?>("UPDATED_DATE"),
                    UpdatedBy = row.Field<string>("UPDATED_BY") ?? string.Empty

                }).ToList();
            }
            catch (Exception ex)
            {
                throw; // Re-throw to be handled by the controller
            }
        }

        public bool SaveDocumentUpload(DocumentUpload model, string userId)
        {
            try
            {
                string query = @"INSERT INTO DOCUMENT_UPLOAD 
                        (DOCUMENT_SL, ENTRY_INFO_ID, DOCUMENT_NAME, FILE_PATH, UPLOADED_DATE, UPLOADED_BY)
                        VALUES
                        (:DOCUMENT_SL, :ENTRY_INFO_ID, :DOCUMENT_NAME, :FILE_PATH, SYSDATE, :UPLOADED_BY)";

                if (model.DocumentSl == 0)
                {
                    string maxId = idGenerated.getMAXID("DOCUMENT_UPLOAD", "DOCUMENT_SL", "fm0000");
                    model.DocumentSl = Convert.ToDecimal(maxId);
                }

                using (OracleConnection conn = new OracleConnection(dbConn.SAConnStrReader()))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("DOCUMENT_SL", model.DocumentSl));
                        cmd.Parameters.Add(new OracleParameter("ENTRY_INFO_ID", model.EntryInfoId));
                        cmd.Parameters.Add(new OracleParameter("DOCUMENT_NAME", model.DocumentName));
                        cmd.Parameters.Add(new OracleParameter("FILE_PATH", model.FilePath));
                        cmd.Parameters.Add(new OracleParameter("UPLOADED_BY", userId));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (implement logging framework here)
                // System.Diagnostics.Debug.WriteLine($"Error in SaveDocumentUpload: {ex.Message}");
                return false;
            }
        }



    }
}