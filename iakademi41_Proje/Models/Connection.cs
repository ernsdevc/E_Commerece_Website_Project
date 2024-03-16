using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iakademi41_Proje.Models
{
    public class Connection
    {
        public static SqlConnection baglanti
        {
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=MACHINEX;Database=iakademi41proje_DB;Trusted_Connection=True");
                return sqlcon;
            }
        }
    }
}