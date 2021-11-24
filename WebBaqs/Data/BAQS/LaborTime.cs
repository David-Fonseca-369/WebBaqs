using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using WebBaqs.Data.AppJson;
using WebBaqs.Data.Dates;
using WebBaqs.Data.Procedures;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.BAQS
{
    public class LaborTime
    {
        public static void GetLaborTime(string option)
        {

            if (option == null)
            {
                try
                {
                    string deleteAll = RemoveData.DeleteAllLaborTime();

                    if (deleteAll == "OK")
                    {
                        //Este es el filtro general 
                        //SaveLaborTime(LoadJsonData.LaborTime_url(), "GENERAL", DateTime.Now, DateTime.Now); //Anterior 
                        int currentYear = DateTime.Now.Year;
                        string fisrtDayOfTheYear = new DateTime(currentYear, 1, 1).ToString("MM/dd/yyyy");
                        SaveLaborTime(LoadJsonData.LaborTime_url()+ "?Prm_FInicial='"+fisrtDayOfTheYear+"'&Prm_FFinal='"+DateTime.Now.ToString("MM/dd/yyyy")+"'", "GENERAL", DateTime.Now, DateTime.Now); //Anterior 
                    }
                    else
                    {
                        InsertLog.Insert("Labor time", "GENERAL", DateTime.Now, DateTime.Now, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.SaveFileLaborTime("Labor time general", ex);
                    ErrorLog.SendMail("Labor time general", ex);
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


                        if (isCorrect)
                        {
                            string deleteMonth = RemoveData.DeleteByDatesLaborTime(startDate, finalDate);

                            if (deleteMonth == "OK")
                            {
                                //Anterior
                                //SaveLaborTime(LoadJsonData.LaborTime_url() + "?$filter=LaborDtl_ClockInDate ge datetime\'" + startDate.ToString("yyyy-MM-dd") + "\' and LaborDtl_ClockInDate le datetime\'" + finalDate.ToString("yyyy-MM-dd") + "\'", strOption, startDate, finalDate);
                                //Funcionando
                                SaveLaborTime(LoadJsonData.LaborTime_url() + "?Prm_FInicial='" + startDate.ToString("MM/dd/yyyy") + "'&Prm_FFinal='" + finalDate.ToString("MM/dd/yyyy") + "'", strOption, startDate, finalDate);

                                //Estatica
                                //SaveLaborTime(LoadJsonData.LaborTime_url() + "?Prm_FInicial='"+"03/01/2021"+"'&Prm_FFinal='"+ "03/01/2021"+"'", strOption, startDate, finalDate);
                            }
                            else
                            {
                                //Insertando en base de datos
                               InsertLog.Insert("Labor time", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Labor time", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
                        }


                    }
                    else if (strOption == "WEEKLY")
                    {

                        var weekly = CalculateDates.Weekly();
                        DateTime startWeek = weekly.Item1;
                        DateTime finalWeek = weekly.Item2;
                        bool isCorrect = weekly.Item3;

                        if (isCorrect)
                        {
                            string deleteWeek = RemoveData.DeleteByDatesLaborTime(startWeek, finalWeek);
                            if (deleteWeek == "OK")
                            {
                                //Anterior
                                //SaveLaborTime(LoadJsonData.LaborTime_url() + "?$filter=LaborDtl_ClockInDate ge datetime\'" + startWeek.ToString("yyyy-MM-dd") + "\' and LaborDtl_ClockInDate le datetime\'" + finalWeek.ToString("yyyy-MM-dd") + "\'", strOption, startWeek, finalWeek);
                                SaveLaborTime(LoadJsonData.LaborTime_url() + "?Prm_FInicial='"+startWeek.ToString("MM/dd/yyyy")+"'&Prm_FFinal='"+finalWeek.ToString("MM/dd/yyyy")+"'", strOption, startWeek, finalWeek);
                            }
                            else
                            {
                                InsertLog.Insert("Labor time", strOption, startWeek, finalWeek, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Labor time", strOption, startWeek, finalWeek, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
                        }

                    }
                    else if (strOption == "DAILY")
                    {
                        DateTime yesterday = DateTime.Now.AddDays(-1);
                        string deleteDay = RemoveData.DeleteDayLaborTime(yesterday);

                        if (deleteDay == "OK")
                        {
                            //Anterior
                            //SaveLaborTime(LoadJsonData.LaborTime_url() + "?$filter=LaborDtl_ClockInDate eq datetime\'" + yesterday.ToString("yyy-MM-dd") + "\'", strOption, yesterday, yesterday);
                            SaveLaborTime(LoadJsonData.LaborTime_url() + "?Prm_FInicial='"+yesterday.ToString("MM/dd/yyyy")+"'&Prm_FFinal='"+DateTime.Now.ToString("MM/dd/yyyy")+"'", strOption, yesterday, yesterday);
                        }
                        else
                        {
                            InsertLog.Insert("Labor time", strOption, yesterday, yesterday, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                        }
                    }
                    else if (strOption == "TIMER")
                    {
                        string deleteToday = RemoveData.DeleteDayLaborTime(DateTime.Now);

                        if (deleteToday == "OK")
                        {
                            //SaveLaborTime(LoadJsonData.LaborTime_url() + "?$filter=LaborDtl_ClockInDate eq datetime\'" + DateTime.Now.ToString("yyyy-MM-dd") + "\'", strOption, DateTime.Now, DateTime.Now);
                            //Anterior
                            //SaveLabortime_Timer(LoadJsonData.LaborTime_url() + "?$filter=LaborDtl_ClockInDate eq datetime\'" + DateTime.Now.ToString("yyyy-MM-dd") + "\'", strOption, DateTime.Now, DateTime.Now);
                            SaveLabortime_Timer(LoadJsonData.LaborTime_url() + "?Prm_FInicial='"+DateTime.Now.ToString("MM/dd/yyyy")+"'&Prm_FFinal='"+DateTime.Now.ToString("MM/dd/yyyy")+"'", strOption, DateTime.Now, DateTime.Now);
                        }
                        else
                        {
                            InsertLog.Insert("Labor time", strOption, DateTime.Now, DateTime.Now, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
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
                    ErrorLog.SaveFileLaborTime("Labor Time", ex);
                    ErrorLog.SendMail("Labor Time", ex);
                }
            }
        }
        static void SaveLaborTime(string url, string option, DateTime startDate, DateTime finalDate)
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
                //Debug.WriteLine(content);
                int index = content.IndexOf(Environment.NewLine);
                string newText = content.Substring(index + Environment.NewLine.Length);
                int index2 = newText.IndexOf(Environment.NewLine);
                string newText2 = newText.Substring(index2 + Environment.NewLine.Length);
                newText2 = newText2.Remove(newText2.LastIndexOf(Environment.NewLine));
                newText2 = "[" + newText2.Replace(Environment.NewLine, "");

                DataTable dsTopics = JsonConvert.DeserializeObject<DataTable>(newText2);

                if (dsTopics.Rows.Count > 0)
                {
                    SqlConnection cn = new SqlConnection(LoadJsonData.ConnetionString());
                    SqlBulkCopy objBulk = new SqlBulkCopy(cn);
                    objBulk.DestinationTableName = "Labor_Time";

                    objBulk.ColumnMappings.Add("LaborDtl_LaborDtlSeq", "LaborDtl_LaborDtlSeq");
                    objBulk.ColumnMappings.Add("EmpBasic_Name", "EmpBasic_Name");
                    objBulk.ColumnMappings.Add("LaborDtl_EmployeeNum", "LaborDtl_EmployeeNum");
                    objBulk.ColumnMappings.Add("LaborDtl_OpCode", "LaborDtl_OpCode");
                    objBulk.ColumnMappings.Add("LaborDtl_JobNum", "LaborDtl_JobNum");
                    objBulk.ColumnMappings.Add("LaborDtl_ClockInDate", "LaborDtl_ClockInDate");
                    objBulk.ColumnMappings.Add("LaborDtl_DspClockInTime", "LaborDtl_DspClockInTime");
                    objBulk.ColumnMappings.Add("LaborDtl_DspClockOutTime", "LaborDtl_DspClockOutTime");
                    objBulk.ColumnMappings.Add("LaborDtl_LaborRate", "LaborDtl_LaborRate");
                    objBulk.ColumnMappings.Add("LaborDtl_BurdenRate", "LaborDtl_BurdenRate");
                    objBulk.ColumnMappings.Add("LaborDtl_LaborHrs", "LaborDtl_LaborHrs");
                    objBulk.ColumnMappings.Add("LaborDtl_BurdenHrs", "LaborDtl_BurdenHrs");
                    objBulk.ColumnMappings.Add("Calculated_LaborCost", "Calculated_LaborCost");
                    objBulk.ColumnMappings.Add("Calculated_BurdenCost", "Calculated_BurdenCost");
                    objBulk.ColumnMappings.Add("LaborDtl_ResourceGrpID", "LaborDtl_ResourceGrpID");
                    objBulk.ColumnMappings.Add("LaborDtl_ResourceID", "LaborDtl_ResourceID");
                    objBulk.ColumnMappings.Add("LaborDtl_CreateDate", "LaborDtl_CreateDate");
                    objBulk.ColumnMappings.Add("LaborDtl_ApprovedDate", "LaborDtl_ApprovedDate");
                    objBulk.ColumnMappings.Add("LaborDtl_ChangeDate", "LaborDtl_ChangeDate");
                    objBulk.ColumnMappings.Add("LaborDtl_ChangedBy", "LaborDtl_ChangedBy");
                    objBulk.ColumnMappings.Add("LaborDtl_CreateTime", "LaborDtl_CreateTime");
                    objBulk.ColumnMappings.Add("LaborDtl_LaborQty", "LaborDtl_LaborQty");
                    objBulk.ColumnMappings.Add("LaborDtl_OprSeq", "LaborDtl_OprSeq");

                    cn.Open();
                    objBulk.WriteToServer(dsTopics);
                    cn.Close();

                    UpdateDate.updateJobs(2);
                    SuccessfulLog.SaveFileLaborTime("Labor Time");
                    //InsertLog.Insert("Labor time", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                }
                else
                {
                    UpdateDate.updateJobs(2);
                    SuccessfulLog.SaveFileLaborTime("Labor Time");
                    InsertLog.Insert("Labor time", option, startDate, finalDate, "Successful at " + DateTime.Now, "Empty data table.");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.SaveFileLaborTime("Labor Time", ex);
                ErrorLog.SendMail("Labor Time", ex);
                InsertLog.Insert("Labor time", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
            }

        }

        static void SaveLabortime_Timer(string url, string option, DateTime startDate, DateTime finalDate)
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
                            string sqltxt = "INSERT INTO Labor_Time(LaborDtl_LaborDtlSeq, EmpBasic_Name, LaborDtl_EmployeeNum, LaborDtl_OpCode, LaborDtl_JobNum, LaborDtl_ClockInDate, LaborDtl_DspClockInTime, LaborDtl_DspClockOutTime, LaborDtl_LaborRate, LaborDtl_BurdenRate, LaborDtl_LaborHrs," +
                                "LaborDtl_BurdenHrs, Calculated_LaborCost, Calculated_BurdenCost,  LaborDtl_ResourceGrpID, LaborDtl_ResourceID, LaborDtl_CreateDate, LaborDtl_ApprovedDate, LaborDtl_ChangeDate," +
                                "LaborDtl_ChangedBy, LaborDtl_CreateTime, LaborDtl_LaborQty, LaborDtl_OprSeq, LAST_RUN) " +
                                "VALUES('" + r["LaborDtl_LaborDtlSeq"] + "','" + r["EmpBasic_Name"] + "','" + r["LaborDtl_EmployeeNum"] + "','" + r["LaborDtl_OpCode"] + "','" + r["LaborDtl_JobNum"] + "', @LaborDtl_ClockInDate," +
                                "'" + r["LaborDtl_DspClockInTime"] + "','" + r["LaborDtl_DspClockOutTime"] + "','" + r["LaborDtl_LaborRate"] + "','" + r["LaborDtl_BurdenRate"] + "','" + r["LaborDtl_LaborHrs"] + "','" + r["LaborDtl_BurdenHrs"] +
                                "','" + r["Calculated_LaborCost"] + "','" + r["Calculated_BurdenCost"] + "','" + r["LaborDtl_ResourceGrpID"] + "','" + r["LaborDtl_ResourceID"] + "', @LaborDtl_CreateDate, @LaborDtl_ApprovedDate, @LaborDtl_ChangeDate," +
                                "'" + r["LaborDtl_ChangedBy"] + "','" + r["LaborDtl_CreateTime"] + "','" + r["LaborDtl_LaborQty"] + "','" + r["LaborDtl_OprSeq"] + "', @LAST_RUN)";

                            var sqlcon = new SqlConnection(LoadJsonData.ConnetionString());
                            var command = new SqlCommand(sqltxt, sqlcon);


                            try
                            {
                                command.Connection.Open();
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@LaborDtl_ClockInDate", r["LaborDtl_ClockInDate"]);
                                command.Parameters.AddWithValue("@LaborDtl_CreateDate", r["LaborDtl_CreateDate"]);
                                command.Parameters.AddWithValue("@LaborDtl_ApprovedDate", r["LaborDtl_ApprovedDate"]);
                                command.Parameters.AddWithValue("@LaborDtl_ChangeDate", r["LaborDtl_ChangeDate"]);
                                command.Parameters.AddWithValue("@LAST_RUN", DateTime.Now);
                                command.ExecuteNonQuery();
                                command.Connection.Close();
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                command.Connection.Close();


                                string sqltxt2 = "INSERT INTO [dbo].[error]( part_num, record, name_table, error, run_at, query) VALUES (@part_num, @record, @name_table, @error, @run_at, @query)";
                                var comando2 = new SqlCommand(sqltxt2, sqlcon);
                                try
                                {
                                    string records = r["LaborDtl_LaborDtlSeq"] + "," + r["EmpBasic_Name"] + "," + r["LaborDtl_EmployeeNum"] + "," + r["LaborDtl_OpCode"] + "," + r["LaborDtl_JobNum"]
                                + "," + r["LaborDtl_DspClockInTime"] + "," + r["LaborDtl_DspClockOutTime"] + "," + r["LaborDtl_LaborRate"] + "," + r["LaborDtl_BurdenRate"] + "," + r["LaborDtl_LaborHrs"] + "," + r["LaborDtl_BurdenHrs"]
                                 + "," + r["Calculated_LaborCost"] + "," + r["Calculated_BurdenCost"] + "," + r["LaborDtl_ResourceGrpID"] + "," + r["LaborDtl_ResourceID"] + "," +
                                 r["LaborDtl_ChangedBy"] + "," + r["LaborDtl_CreateTime"] + "," + r["LaborDtl_LaborQty"] + "," + r["LaborDtl_OprSeq"];

                                    comando2.Connection.Open();
                                    comando2.Parameters.Clear();

                                    comando2.Parameters.AddWithValue("@part_num", "N/A");
                                    comando2.Parameters.AddWithValue("@record", records);
                                    comando2.Parameters.AddWithValue("@name_table", "Labor_Time");
                                    comando2.Parameters.AddWithValue("@error", ex.Message);
                                    comando2.Parameters.AddWithValue("@run_at", DateTime.Now);
                                    comando2.Parameters.AddWithValue("@query", sqltxt);
                                    comando2.ExecuteNonQuery();
                                    comando2.Connection.Close();
                                    ErrorLog.SaveFileLaborTime("Labor time", ex);
                                    result = false;
                                }
                                catch (Exception ex2)
                                {
                                    comando2.Connection.Close();
                                    ErrorLog.SaveFileLaborTime("Labor time", ex2);
                                    result = false;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.SaveFileLaborTime("Labor time", ex);
                            result = false;
                        }
                    }
                    if (result == true)
                    {
                        UpdateDate.updateJobs(1);
                        SuccessfulLog.SaveFileLaborTime("Labor time");
                        InsertLog.Insert("Labor time", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                    }
                    else
                    {
                        ErrorLog.SendPersonalizedMail("Labor Time", "Hubo un problema al realizar el traspaso de datos.");
                    }
                }
                else
                {
                    UpdateDate.updateJobs(2);
                    SuccessfulLog.SaveFileLaborTime("Labor time");
                    InsertLog.Insert("Labor time", option, startDate, finalDate, "Successful at " + DateTime.Now, "Empty data table.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.SaveFileLaborTime("Labor time", ex);
                ErrorLog.SendMail("Labor time", ex);
                InsertLog.Insert("Labor time", option, startDate, finalDate, "Resgistration failed at " + DateTime.Now, "Error: " + ex.Message);
            }

        }
    }
}
