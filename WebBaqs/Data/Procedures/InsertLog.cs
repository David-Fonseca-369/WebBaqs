﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebBaqs.Data.ConnectionToSql;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.Procedures
{
    public class InsertLog
    {
        public static string Insert(string baq, string typeExecution, DateTime startDate, DateTime finalDate, string result, string comments)
        {
            string rpta = "";
            SqlConnection sqlCon = new SqlConnection();
            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("insert_log", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@baq", SqlDbType.VarChar).Value = baq;
                command.Parameters.Add("@typeExecution", SqlDbType.VarChar).Value = typeExecution;
                command.Parameters.Add("@startDate", SqlDbType.Date).Value = startDate;
                command.Parameters.Add("@finalDate", SqlDbType.Date).Value = finalDate;
                command.Parameters.Add("@result", SqlDbType.VarChar).Value = result;
                command.Parameters.Add("@comments", SqlDbType.VarChar).Value = comments;
                sqlCon.Open();
                rpta = command.ExecuteNonQuery() == 1 ? "OK" : "No se pudo realizar el registro";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Inserción de datos a bitácora", ex);
                ErrorLog.SendMail("Inserción de datos a bitácora", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return rpta;
        } 
    }
}
