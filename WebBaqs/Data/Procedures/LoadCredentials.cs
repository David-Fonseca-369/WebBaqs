using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using WebBaqs.Data.ConnectionToSql;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.Procedures
{
    public class LoadCredentials
    {

        public static string Email()
        {

            string email = "";

            SqlConnection sqlCon = new SqlConnection();

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("load_user", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", 1);
                sqlCon.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {

                    email = reader["email"].ToString();

                }
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Cargar email para credenciales", ex);

                Debug.WriteLine(ex.Message);
            }

            return email;

        }

        public static string Password()
        {

            string password = "";

            SqlConnection sqlCon = new SqlConnection();

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("load_user", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", 1);
                sqlCon.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {

                    password = reader["pass"].ToString();

                }
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Cargar password para credenciales", ex);
                ErrorLog.SendMail("Cargar password para credenciales", ex);

                Debug.WriteLine(ex.Message);
            }

            return password;

        }
    }
}
