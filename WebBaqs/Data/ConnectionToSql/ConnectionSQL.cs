using System;
using System.Data.SqlClient;
using WebBaqs.Data.AppJson;

namespace WebBaqs.Data.ConnectionToSql
{
    public class ConnectionSQL
    {
        private static ConnectionSQL con = null;

        private ConnectionSQL()
        { }

        public SqlConnection CreateConnection()
        {
            SqlConnection Cadena = new SqlConnection();
            try
            {
            
                Cadena.ConnectionString = LoadJsonData.ConnetionString(); 
            }
            catch (Exception ex)
            {

                Cadena = null;
                throw ex;
            }
            return Cadena;
        }
        public static ConnectionSQL getInstancia()
        {
            if (con == null)
            {
                con = new ConnectionSQL();
            }
            return con;
        }
    }
}
