using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Data;
using System.Data.SqlClient;
using WebBaqs.Data.AppJson;
using WebBaqs.Data.Dates;
using WebBaqs.Data.Procedures;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.BAQS
{
    public class Jobs
    {

        public static void GetJobs(string option)
        {
            if (option == null)
            {
                try
                {
                    string deleteAll = RemoveData.DeleteAllJobs();
                    if (deleteAll == "OK")
                    {
                        SaveJobs_2(LoadJsonData.Job_url(), "GENERAL", DateTime.Now, DateTime.Now);
                    }
                    else
                    {
                        InsertLog.Insert("Jobs", "GENERAL", DateTime.Now, DateTime.Now, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.SaveFileJobs("Jobs general", ex);
                    ErrorLog.SendMail("Jobs time general", ex);
                }

            }
            else
            {
                try
                {
                    string strOption = option.ToUpper();

                    if (strOption == "MONTHLY")
                    {
                        var month = CalculateDates.Monthly();
                        DateTime startDate = month.Item1;
                        DateTime finalDate = month.Item2;
                        bool isCorrect = month.Item3;

                        //Fechas prueba
                        //DateTime startDate = new DateTime(2020, 03, 19);
                        //DateTime finalDate = new DateTime(2020, 04, 10);

                        if (isCorrect)
                        {
                            string deleteMonth = RemoveData.DeleteByDatesJobs(startDate, finalDate);

                            if (deleteMonth == "OK")
                            {
                                SaveJobs_2(LoadJsonData.Job_url() + "?$filter=JobHead_CreateDate ge datetime\'" + startDate.ToString("yyyy-MM-dd") + "\' and JobHead_CreateDate le datetime\'" + finalDate.ToString("yyyy-MM-dd") + "\'", strOption, startDate, finalDate);
                            }
                            else
                            {
                                InsertLog.Insert("Jobs", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Jobs", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
                        }
                    }
                    else if (strOption == "WEEKLY")
                    {
                        var weekly = CalculateDates.Weekly();
                        DateTime startWeek = weekly.Item1;
                        DateTime finalWeek = weekly.Item2;
                        bool isCorrect = weekly.Item3;


                        //Fechas prueba
                        //DateTime startWeek = new DateTime(2020, 03, 19);
                        //DateTime finalWeek = new DateTime(2020, 04, 10);
                        if (isCorrect)
                        {
                            string deleteWeek = RemoveData.DeleteByDatesJobs(startWeek, finalWeek);

                            if (deleteWeek == "OK")
                            {
                                SaveJobs_2(LoadJsonData.Job_url() + "?$filter=JobHead_CreateDate ge datetime\'" + startWeek.ToString("yyyy-MM-dd") + "\' and JobHead_CreateDate le datetime\'" + finalWeek.ToString("yyyy-MM-dd") + "\'", strOption, startWeek, finalWeek);
                            }
                            else
                            {
                                InsertLog.Insert("Jobs", strOption, startWeek, finalWeek, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Jobs", strOption, startWeek, finalWeek, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
                        }


                    }
                    else if (strOption == "DAILY")
                    {
                        //Fecha prueba
                        // DateTime yesterday = new DateTime(2020, 03, 19);
                        DateTime yesterday = DateTime.Now.AddDays(-1);
                        string deleteDay = RemoveData.DeleteDayJobs(yesterday);

                        if (deleteDay == "OK")
                        {
                            SaveJobs_2(LoadJsonData.Job_url() + "?$filter=JobHead_CreateDate eq datetime\'" + yesterday.ToString("yyyy-MM-dd") + "\'", strOption, yesterday, yesterday);
                        }
                        else
                        {
                            InsertLog.Insert("Jobs", strOption, yesterday, yesterday, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                        }
                    }
                    else
                    {
                        ErrorLog.CustomResgistration(strOption + " was not found", "Check that the petition is well written.");
                        ErrorLog.SendPersonalizedMail(strOption + " was not found", "Check that the petition is well written.");

                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.SaveFileJobs("Jobs", ex);
                    ErrorLog.SendMail("Jobs", ex);
                }

            }
        }

        public static void SaveJobs(string url, string option, DateTime startDate, DateTime finalDate)
        {
            try
            {
                var client = new RestClient(url);
                client.Authenticator = new NtlmAuthenticator(LoadCredentials.Email(), LoadCredentials.Password());
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Accept", "application/json");

                IRestResponse response = client.Execute(request);
                var content = response.Content;
                int index = content.IndexOf(Environment.NewLine);
                string newText = content.Substring(index + Environment.NewLine.Length);
                int index2 = newText.IndexOf(Environment.NewLine);
                string newText2 = newText.Substring(index2 + Environment.NewLine.Length);
                newText2 = newText2.Remove(newText2.LastIndexOf(Environment.NewLine));
                newText2 = "[" + newText2.Replace(Environment.NewLine, "");
                DataTable dsTopics = JsonConvert.DeserializeObject<DataTable>(newText2);


                if (dsTopics.Rows.Count > 0)
                {
                    SqlConnection con = new SqlConnection(LoadJsonData.ConnetionString());
                    SqlBulkCopy objBulk = new SqlBulkCopy(con);
                    objBulk.DestinationTableName = "JOBS";

                    objBulk.ColumnMappings.Add("JobHead_CreateDate", "JobHead_CreateDate");
                    objBulk.ColumnMappings.Add("OrderHed_OrderNum", "OrderHed_OrderNum");
                    objBulk.ColumnMappings.Add("OrderHed_OrderDate", "OrderHed_OrderDate");
                    objBulk.ColumnMappings.Add("Customer_CustID", "Customer_CustID");
                    objBulk.ColumnMappings.Add("Customer_Name", "Customer_Name");
                    objBulk.ColumnMappings.Add("OrderDtl_PartNum", "OrderDtl_PartNum");
                    objBulk.ColumnMappings.Add("OrderDtl_OrderLine", "OrderDtl_OrderLine");
                    objBulk.ColumnMappings.Add("OrderRel_OrderRelNum", "OrderRel_OrderRelNum");
                    objBulk.ColumnMappings.Add("JobHead_DueDate", "JobHead_DueDate");
                    objBulk.ColumnMappings.Add("JobProd_JobNum", "JobProd_JobNum");
                    objBulk.ColumnMappings.Add("JobProd_ProdQty", "JobProd_ProdQty");
                    objBulk.ColumnMappings.Add("JobHead_JobClosed", "JobHead_JobClosed");
                    objBulk.ColumnMappings.Add("JobHead_JobComplete", "JobHead_JobComplete");
                    objBulk.ColumnMappings.Add("JobHead_JobCompletionDate", "JobHead_JobCompletionDate");
                    objBulk.ColumnMappings.Add("JobHead_LastChangedOn", "JobHead_LastChangedOn");
                    objBulk.ColumnMappings.Add("JobHead_LastChangedBy", "JobHead_LastChangedBy");

                    con.Open();
                    objBulk.WriteToServer(dsTopics);
                    con.Close();

                    UpdateDate.updateJobs(1);
                    SuccessfulLog.SaveFile("Jobs");
                    InsertLog.Insert("Jobs", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                }
                else
                {
                    UpdateDate.updateJobs(1);
                    SuccessfulLog.SaveFile("Jobs");
                    InsertLog.Insert("Jobs", option, startDate, finalDate, "Successful at " + DateTime.Now, "Empty data table.");
                }


            }
            catch (Exception ex)
            {
                ErrorLog.SaveFileJobs("Jobs", ex);
                ErrorLog.SendMail("Jobs", ex);
                InsertLog.Insert("Jobs", option, startDate, finalDate, "Resgistration failed at " + DateTime.Now, "Data record failed.");
            }
        }

        public static void SaveJobs_2(string url, string option, DateTime startDate, DateTime finalDate)
        {
            try
            {
                var client = new RestClient(url);
                client.Authenticator = new NtlmAuthenticator(LoadCredentials.Email(), LoadCredentials.Password());
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Accept", "application/json");

                IRestResponse response = client.Execute(request);
                var content = response.Content;
                int index = content.IndexOf(Environment.NewLine);
                string newText = content.Substring(index + Environment.NewLine.Length);
                int index2 = newText.IndexOf(Environment.NewLine);
                string newText2 = newText.Substring(index2 + Environment.NewLine.Length);
                newText2 = newText2.Remove(newText2.LastIndexOf(Environment.NewLine));
                newText2 = "[" + newText2.Replace(Environment.NewLine, "");
                DataTable dsTopics = JsonConvert.DeserializeObject<DataTable>(newText2);
                bool result = false;

                if (dsTopics.Rows.Count > 0)
                {
                    foreach (DataRow r in dsTopics.Rows)
                    {
                        try
                        {

                            string sqltxt = "INSERT INTO JOBS(JobHead_CreateDate, OrderHed_OrderNum, OrderHed_OrderDate, Customer_CustID, Customer_Name, OrderDtl_PartNum, OrderDtl_OrderLine, OrderRel_OrderRelNum, JobHead_DueDate, " +
                                     " JobProd_JobNum, JobProd_ProdQty, JobHead_JobClosed, JobHead_JobComplete, JobHead_JobCompletionDate, JobHead_LastChangedOn, JobHead_LastChangedBy, Last_RUN) " +
                                    "  VALUES(@JobHead_CreateDate," + r["OrderHed_OrderNum"] + ", @OrderHed_OrderDate,'" + r["Customer_CustID"] + "','" + r["Customer_Name"] + "','" + r["OrderDtl_PartNum"] + "'," + r["OrderDtl_OrderLine"] + "," +
                                       r["OrderRel_OrderRelNum"] + ",@JobHead_DueDate,'" + r["JobProd_JobNum"] + "'," + r["JobProd_ProdQty"] + ",'" + r["JobHead_JobClosed"] + "','" +
                                             r["JobHead_JobComplete"] + "',@JobHead_JobCompletionDate, @JobHead_LastChangedOn, @JobHead_LastChangedBy, @Last_RUN )";


                            var sqlcon = new SqlConnection(LoadJsonData.ConnetionString());
                            var comando = new SqlCommand(sqltxt, sqlcon);

                            try
                            {
                                comando.Connection.Open();
                                comando.Parameters.Clear();
                                comando.Parameters.AddWithValue("@JobHead_CreateDate", r["JobHead_CreateDate"]);
                                comando.Parameters.AddWithValue("@OrderHed_OrderDate", r["OrderHed_OrderDate"]);
                                comando.Parameters.AddWithValue("@JobHead_DueDate", r["JobHead_DueDate"]);
                                comando.Parameters.AddWithValue("@JobHead_JobCompletionDate", r["JobHead_JobCompletionDate"]);
                                comando.Parameters.AddWithValue("@JobHead_LastChangedOn", r["JobHead_LastChangedOn"]);
                                comando.Parameters.AddWithValue("@JobHead_LastChangedBy", r["JobHead_LastChangedBy"]);
                                comando.Parameters.AddWithValue("@Last_RUN", DateTime.Now);
                                comando.ExecuteNonQuery();
                                comando.Connection.Close();
                                result = true;
                                // UpdateDate.updateJobs(1);
                                //  SuccessfulLog.SaveFile("Jobs");                                
                                //  InsertLog.Insert("Jobs", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                            }
                            catch (Exception ex)
                            {
                                comando.Connection.Close();



                                string sqltxt2 = "INSERT INTO [dbo].[error]( part_num, record, name_table, error, run_at, query) VALUES (@part_num, @record, @name_table, @error, @run_at, @query)";
                                var comando2 = new SqlCommand(sqltxt2, sqlcon);

                                try
                                {
                                    string records = r["OrderHed_OrderNum"] + "," + r["OrderHed_OrderDate"] + "," + r["Customer_CustID"] + "," + r["Customer_Name"] + "," + r["OrderDtl_PartNum"] + "," + r["OrderDtl_OrderLine"] + "," +
                                       r["OrderRel_OrderRelNum"] + "," + r["JobHead_CreateDate"] + "," + r["JobHead_DueDate"] + "," + r["JobProd_JobNum"] + "," + r["JobProd_ProdQty"] + "," + r["JobHead_JobClosed"] + "," +
                                             r["JobHead_JobComplete"] + "," + r["JobHead_JobCompletionDate"] + ", " + r["JobHead_LastChangedOn"] + ", " + r["JobHead_LastChangedBy"];

                                    comando2.Connection.Open();
                                    comando2.Parameters.Clear();

                                    comando2.Parameters.AddWithValue("@part_num", "N/A");
                                    comando2.Parameters.AddWithValue("@record", records);
                                    comando2.Parameters.AddWithValue("@name_table", "JOBS");
                                    comando2.Parameters.AddWithValue("@error", ex.Message);
                                    comando2.Parameters.AddWithValue("@run_at", DateTime.Now);
                                    comando2.Parameters.AddWithValue("@query", sqltxt);
                                    comando2.ExecuteNonQuery();
                                    comando2.Connection.Close();
                                    ErrorLog.SaveFileJobs("Job", ex);
                                    result = false;
                                }
                                catch (Exception ex2)
                                {
                                    comando2.Connection.Close();
                                    ErrorLog.SaveFileJobs("Job", ex2);
                                    result = false;
                                    //       ErrorLog.SendMail("Job", ex2);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.SaveFileJobs("Job", ex);
                            result = false;
                            // ErrorLog.SendMail("Job", ex);
                        }
                    }
                    if (result == true)
                    {
                        UpdateDate.updateJobs(1);
                        SuccessfulLog.SaveFileJobs("Jobs");
                        InsertLog.Insert("Jobs", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                    }
                    else
                    {
                        ErrorLog.SendPersonalizedMail("Jobs", "Hubo un problema al realizar el traspaso de datos.");
                    }
                }
                else
                {
                    UpdateDate.updateJobs(1);
                    SuccessfulLog.SaveFileJobs("Jobs");
                    InsertLog.Insert("Jobs", option, startDate, finalDate, "Successful at " + DateTime.Now, "Empty data table.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFileJobs("Job", ex);
                ErrorLog.SendMail("Job", ex);
                InsertLog.Insert("Jobs", option, startDate, finalDate, "Resgistration failed at " + DateTime.Now, "Error: " + ex.Message);
            }
        }
    }
}
