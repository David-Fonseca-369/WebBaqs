using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebBaqs.Data.ConnectionToSql;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.Procedures
{
    public class RemoveData
    {
        //JOBS
        public static string DeleteAllJobs()
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteJobs", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                sqlCon.Open();
                result = command.ExecuteNonQuery() == -1 ? "OK" : "Not.";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar datos de tabla en Jobs", ex);
                ErrorLog.SendMail("Eliminar datos de tabla en Jobs", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        public static string DeleteByDatesJobs(DateTime startDate, DateTime finalDate)
        {

            SqlConnection sqlCon = new SqlConnection();
            string rpta = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteByDates_Jobs", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("start_date", SqlDbType.Date).Value = startDate;
                command.Parameters.AddWithValue("final_date", SqlDbType.Date).Value = finalDate;
                sqlCon.Open();

                rpta = command.ExecuteNonQuery() >= 0 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por fechas en Jobs", ex);
                ErrorLog.SendMail("Eliminar por fechas en Jobs", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return rpta;

        }

        public static string DeleteDayJobs(DateTime day)
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteDay_Jobs", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("date", SqlDbType.Date).Value = day;
                sqlCon.Open();

                result = command.ExecuteNonQuery() >= 0 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por dias en Jobs", ex);
                ErrorLog.SendMail("Eliminar por dias en Jobs", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        //Labor_Time
        public static string DeleteByDatesLaborTime(DateTime startDate, DateTime finalDate)
        {

            SqlConnection sqlCon = new SqlConnection();
            string rpta = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteByDates_laborTime", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("start_date", SqlDbType.Date).Value = startDate.ToString("dd/MM/yyyy");
                //command.Parameters.AddWithValue("final_date", SqlDbType.Date).Value = finalDate.ToString("dd/MM/yyyy");
                command.Parameters.AddWithValue("start_date", SqlDbType.Date).Value = startDate;
                command.Parameters.AddWithValue("final_date", SqlDbType.Date).Value = finalDate;
                sqlCon.Open();

                rpta = command.ExecuteNonQuery() >= 0 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por fechas en Labor Time", ex);
                ErrorLog.SendMail("Eliminar por fechas en Labor Time", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return rpta;

        }

        public static string DeleteDayLaborTime(DateTime day)
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteDay_laborTime", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("date", SqlDbType.Date).Value = day;
                sqlCon.Open();

                result = command.ExecuteNonQuery() >= 0 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por dias en Labor time", ex);
                ErrorLog.SendMail("Eliminar por dias en Labor time", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        public static string DeleteAllLaborTime()
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteLaborTime", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                sqlCon.Open();
                // int resultPrueba = command.ExecuteNonQuery();
                result = command.ExecuteNonQuery() == -1 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar datos de tabla en Labor time", ex);
                ErrorLog.SendMail("Eliminar datos de tabla en Labor time", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        //JobMaterials
        public static string DeleteByDatesJobMaterials(DateTime startDate, DateTime finalDate)
        {

            SqlConnection sqlCon = new SqlConnection();
            string rpta = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteByDates_JobMaterials", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("start_date", SqlDbType.Date).Value = startDate;
                command.Parameters.AddWithValue("final_date", SqlDbType.Date).Value = finalDate;
                sqlCon.Open();

                rpta = command.ExecuteNonQuery() >= 0 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por fechas en Job materials", ex);
                ErrorLog.SendMail("Eliminar por fechas en Job materials", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return rpta;

        }

        public static string DeleteDayJobMaterials(DateTime day)
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteDay_JobMaterials", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("date", SqlDbType.Date).Value = day;
                sqlCon.Open();

                result = command.ExecuteNonQuery() >= 0 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por dias en Job materials", ex);
                ErrorLog.SendMail("Eliminar por dias en Job materials", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        public static string DeleteAllJobMaterials()
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteJobMaterials", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                sqlCon.Open();
                // int resultPrueba = command.ExecuteNonQuery();
                result = command.ExecuteNonQuery() == -1 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar datos de tabla en Job materials", ex);
                ErrorLog.SendMail("Eliminar datos de tabla en Job materials", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        //PartCost
        public static string DeleteByDatesPartCost(DateTime startDate, DateTime finalDate)
        {

            SqlConnection sqlCon = new SqlConnection();
            string rpta = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteByDates_PartCost", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("start_date", SqlDbType.Date).Value = startDate;
                command.Parameters.AddWithValue("final_date", SqlDbType.Date).Value = finalDate;
                sqlCon.Open();

                rpta = command.ExecuteNonQuery() >= 0 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por fechas en Part cost", ex);
                ErrorLog.SendMail("Eliminar por fechas en Part cost", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return rpta;

        }

        public static string DeleteDayPartCost(DateTime day)
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteDay_PartCost", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("date", SqlDbType.Date).Value = day;
                sqlCon.Open();

                result = command.ExecuteNonQuery() >= 0 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por dias en Part cost", ex);
                ErrorLog.SendMail("Eliminar por dias en Part cost", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        public static string DeleteAllPartCost()
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deletePartCost", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                sqlCon.Open();
                // int resultPrueba = command.ExecuteNonQuery();
                result = command.ExecuteNonQuery() == -1 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar datos de tabla en Part cost", ex);
                ErrorLog.SendMail("Eliminar datos de tabla en Part cost", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }


        //WIPReconciliation

        public static bool DeleteByDatesWIPReconciliation(DateTime startDate, DateTime finalDate, string option)
        {

            SqlConnection sqlCon = new SqlConnection();
            bool rpta = false;

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteByDates_WIPReconciliation", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("start_date", SqlDbType.Date).Value = startDate;
                command.Parameters.AddWithValue("final_date", SqlDbType.Date).Value = finalDate;
                sqlCon.Open();

                rpta = command.ExecuteNonQuery() >= 0 ? true : false;
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por fechas en WIP Reconciliation", ex);
                ErrorLog.SendMail("Eliminar por fechas en WIP Reconciliation", ex);
                InsertLog.Insert("WIP Reconciliation", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);

            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return rpta;

        }

        public static bool DeleteDayWIPReconciliation(DateTime day, string option)
        {
            SqlConnection sqlCon = new SqlConnection();
            bool result = false;

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteDay_WIPReconciliation", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("date", SqlDbType.Date).Value = day;
                sqlCon.Open();

                result = command.ExecuteNonQuery() >= 0 ? true : false;
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por dias en WIP Reconciliation", ex);
                ErrorLog.SendMail("Eliminar por dias en WIP Reconciliation", ex);
                InsertLog.Insert("WIP Reconciliation", option, day, day, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        public static string DeleteAllWIPReconciliation()
        {
            SqlConnection sqlCon = new SqlConnection();
            string result = "";

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteWIPReconciliation", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                sqlCon.Open();
                // int resultPrueba = command.ExecuteNonQuery();
                result = command.ExecuteNonQuery() == -1 ? "OK" : "Not";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar datos de tabla en WIP Reconciliation", ex);
                ErrorLog.SendMail("Eliminar datos de tabla en WIP Reconciliation", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return result;
        }

        //Booking 
        public static async Task<bool> DeleteAllBooking()
        {
            SqlConnection sqlCon = new SqlConnection();
            bool result = false;

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteBooking", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                await sqlCon.OpenAsync();
                result = await command.ExecuteNonQueryAsync() == -1 ? true : false;
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar datos de tabla en Booking", ex);
                ErrorLog.SendMail("Eliminar datos de tabla en Booking", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) await sqlCon.CloseAsync();
            }
            return result;
        }
        public static async Task<bool> DeleteByDatesBooking(DateTime startDate, DateTime finalDate)
        {

            SqlConnection sqlCon = new SqlConnection();
            bool rpta = false;

            try
            {
                sqlCon = ConnectionSQL.getInstancia().CreateConnection();
                SqlCommand command = new SqlCommand("deleteByDates_Booking", sqlCon);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("start_date", SqlDbType.Date).Value = startDate;
                command.Parameters.AddWithValue("final_date", SqlDbType.Date).Value = finalDate;
                await sqlCon.OpenAsync();

                rpta = await command.ExecuteNonQueryAsync() >= 0 ? true : false;
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Eliminar por fechas en Booking", ex);
                ErrorLog.SendMail("Eliminar por fechas en Booking", ex);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) await sqlCon.CloseAsync();
            }
            return rpta;
        }
    }
}
