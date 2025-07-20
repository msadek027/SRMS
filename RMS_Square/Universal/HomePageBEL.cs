using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_Square.Universal
{
    public class HomePageBEL
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public Int64 Recipe { get; set; }

        public Int64 DTL { get; set; }
        public Int64 Prod_Reg { get; set; }
        public Int64 Price { get; set; }
        public Int64 MA { get; set; }
        public Int64 Amendment { get; set; }

        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string Remarks { get; set; }
        public string SubmissionDate { get; set; }

    }
}