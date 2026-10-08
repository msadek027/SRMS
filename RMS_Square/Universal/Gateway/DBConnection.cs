using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Systems.Universal;

namespace RMS_Square.DAL.Gateway
{
    public class DBConnection
    {
        string connectionString = "";
        public DBConnection()
        {
            SAConnStrReader();
        }
        public string SAConnStrReader()
        {
          

            connectionString = new DataBaseConnection().SAConnStrReader();
            return connectionString;        
        }
    }
}