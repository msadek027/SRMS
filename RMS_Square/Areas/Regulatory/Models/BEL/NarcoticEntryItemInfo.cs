using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_Square.Areas.Regulatory.Models.BEL
{
    public class NarcoticEntryItemInfo
    {
        public decimal NarcoticEntrySl { get; set; }
        public decimal GenericBrandId { get; set; } // FK to NarcoticSetupInfo

        public string ItemName { get; set; }
        public string FiscalYear { get; set; }
        public decimal? AnnualQuota { get; set; }
        public string SubmissionType { get; set; }
        public decimal? SubmissionQuantity { get; set; }
        public decimal? ApprovedQuantity { get; set; }
        public string RecordStatus { get; set; }

        // DGDA Dates
        public DateTime? DgdaReceiveDate { get; set; }
        public DateTime? DgdaSubmissionDate { get; set; }
        public DateTime? DgdaRecommendationDate { get; set; }

        // DNC Local Office Dates
        public DateTime? RecSendDate { get; set; }
        public DateTime? InsReceiveDate { get; set; }

        // DNC Divisional Office Dates
        public DateTime? DivSendDate { get; set; }
        public DateTime? DivNarcRecvDate { get; set; }

        // DNC Head Office Dates
        public DateTime? DncSendDate { get; set; }
        public DateTime? NarcApvlDate { get; set; }

        public string SetBy { get; set; }
        public DateTime? SetOn { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        // Navigation properties
        public NarcoticSetupInfo GenericBrand { get; set; }
        public List<DocumentUpload> Documents { get; set; }
    }
    public class NarcoticSetupInfo
    {
        public decimal NarcoticSetupSl { get; set; } // NUMBER type maps to decimal in .NET
        public string GenericName { get; set; }
        public string SetBy { get; set; }
        public DateTime? SetOn { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        // Navigation property
        public List<NarcoticEntryItemInfo> EntryItems { get; set; }
    }


}