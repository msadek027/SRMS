using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_Square.Areas.Regulatory.Models.BEL
{
    public class DocumentUpload
    {
        public decimal DocumentSl { get; set; }
        public decimal EntryInfoId { get; set; } // FK to NarcoticSetupInfo (as per your current FK)

        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UploadedBy { get; set; }

        // Navigation property
        public NarcoticSetupInfo EntryInfo { get; set; }
    }
}