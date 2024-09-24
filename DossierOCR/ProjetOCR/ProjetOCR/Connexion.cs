using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProjetOCR
{
   public class Connexion
    {
        public static SqlConnection con = new SqlConnection(@"Data Source=(local)\GNR;Initial Catalog=ParamsOCR;Integrated Security=True");

        public void OpenCon()
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }
        public void CloseCon()
        {
            if (con.State != ConnectionState.Closed)
            {
                con.Close();
            }
        }
    }
}
