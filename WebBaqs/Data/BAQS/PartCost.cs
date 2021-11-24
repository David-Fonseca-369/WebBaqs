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
    public class PartCost
    {
        public static void GetPartCost(string option)
        {
            if (option == null)
            {
                try
                {
                    string deleteAll = RemoveData.DeleteAllPartCost();
                    if (deleteAll == "OK")
                    {
                        SavePartCost(LoadJsonData.PartCost_url(), "GENERAL", DateTime.Now, DateTime.Now);
                    }
                    else
                    {
                        InsertLog.Insert("Part cost", "GENERAL", DateTime.Now, DateTime.Now, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.SaveFilePartCost("Part cost general", ex);
                    ErrorLog.SendMail("Part cost general", ex);
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
                            ////Fechas prueba
                            //DateTime startDate = new DateTime(2020, 03, 19);
                            //DateTime finalDate = new DateTime(2020, 04, 10);

                            string deleteMonth = RemoveData.DeleteByDatesPartCost(startDate, finalDate);

                            if (deleteMonth == "OK")
                            {
                                SavePartCost(LoadJsonData.PartCost_url() + "?$filter=Part_ChangedOn ge datetime\'" + startDate.ToString("yyyy-MM-dd") + "\' and Part_ChangedOn le datetime\'" + finalDate.ToString("yyyy-MM-dd") + "\'", strOption, startDate, finalDate);
                            }
                            else
                            {
                                InsertLog.Insert("Part cost", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Part cost", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
                        }


                    }
                    else if (strOption == "WEEKLY")
                    {
                        var week = CalculateDates.Weekly();
                        DateTime startWeek = week.Item1;
                        DateTime finalWeek = week.Item2;
                        bool isCorrect = week.Item3;

                        if (isCorrect)
                        {
                            //Fechas prueba
                            //DateTime startWeek = new DateTime(2020, 03, 19);
                            //DateTime finalWeek = new DateTime(2020, 04, 10);

                            string deleteWeek = RemoveData.DeleteByDatesPartCost(startWeek, finalWeek);
                            if (deleteWeek == "OK")
                            {
                                SavePartCost(LoadJsonData.PartCost_url() + "?$filter=Part_ChangedOn ge datetime\'" + startWeek.ToString("yyyy-MM-dd") + "\' and Part_ChangedOn le datetime\'" + finalWeek.ToString("yyyy-MM-dd") + "\'", strOption, startWeek, finalWeek);
                            }
                            else
                            {
                                InsertLog.Insert("Part cost", strOption, startWeek, finalWeek, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Part cost", strOption, startWeek, finalWeek, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
                        }

                    }
                    else if (strOption == "DAILY")
                    {
                        DateTime yesterday = DateTime.Now.AddDays(-1);
                        //Fecha prueba
                        //DateTime yesterday = new DateTime(2020, 03, 19);

                        string deleteDay = RemoveData.DeleteDayPartCost(yesterday);

                        if (deleteDay == "OK")
                        {
                            SavePartCost(LoadJsonData.PartCost_url() + "?$filter=Part_ChangedOn eq datetime\'" + yesterday.ToString("yyyy-MM-dd") + "\'", strOption, yesterday, yesterday);
                        }
                        else
                        {
                            InsertLog.Insert("Part cost", strOption, yesterday, yesterday, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
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
                    ErrorLog.SaveFilePartCost("Part cost", ex);
                    ErrorLog.SendMail("Part cost", ex);

                }
            }
        }
        static void SavePartCost(string url, string option, DateTime startDate, DateTime finalDate)
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
                    SqlConnection cn = new SqlConnection(LoadJsonData.ConnetionString());
                    SqlBulkCopy objBulk = new SqlBulkCopy(cn);
                    objBulk.DestinationTableName = "PART_COST";

                    objBulk.ColumnMappings.Add("Part_ChangedOn", "Part_ChangedOn");
                    objBulk.ColumnMappings.Add("Part_PartNum", "Part_PartNum");
                    objBulk.ColumnMappings.Add("Part_PartDescription", "Part_PartDescription");
                    objBulk.ColumnMappings.Add("ProdGrup_Description", "ProdGrup_Description");
                    objBulk.ColumnMappings.Add("Part_ClassID", "Part_ClassID");
                    objBulk.ColumnMappings.Add("PartCost_AvgLaborCost", "PartCost_AvgLaborCost");
                    objBulk.ColumnMappings.Add("PartCost_AvgBurdenCost", "PartCost_AvgBurdenCost");
                    objBulk.ColumnMappings.Add("PartCost_AvgMaterialCost", "PartCost_AvgMaterialCost");
                    objBulk.ColumnMappings.Add("PartCost_AvgSubContCost", "PartCost_AvgSubContCost");
                    objBulk.ColumnMappings.Add("PartCost_AvgMtlBurCost", "PartCost_AvgMtlBurCost");
                    objBulk.ColumnMappings.Add("Calculated_AVGUnitCost", "Calculated_AVGUnitCost");
                    objBulk.ColumnMappings.Add("PartCost_StdLaborCost", "PartCost_StdLaborCost");
                    objBulk.ColumnMappings.Add("PartCost_StdBurdenCost", "PartCost_StdBurdenCost");
                    objBulk.ColumnMappings.Add("PartCost_StdMaterialCost", "PartCost_StdMaterialCost");
                    objBulk.ColumnMappings.Add("PartCost_StdSubContCost", "PartCost_StdSubContCost");
                    objBulk.ColumnMappings.Add("PartCost_StdMtlBurCost", "PartCost_StdMtlBurCost");
                    objBulk.ColumnMappings.Add("Part_NonStock", "Part_NonStock");
                    objBulk.ColumnMappings.Add("Part_TypeCode", "Part_TypeCode");
                    objBulk.ColumnMappings.Add("Calculated_StdUnitCost", "Calculated_StdUnitCost");
                    objBulk.ColumnMappings.Add("Part_CreatedOn", "Part_CreatedOn");

                    cn.Open();
                    objBulk.WriteToServer(dsTopics);
                    cn.Close();

                    UpdateDate.updateJobs(4);
                    SuccessfulLog.SaveFilePartCost("Part cost");
                    InsertLog.Insert("Part cost", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                }
                else
                {
                    UpdateDate.updateJobs(4);
                    SuccessfulLog.SaveFilePartCost("Part cost");
                    InsertLog.Insert("Part cost", option, startDate, finalDate, "Successful at " + DateTime.Now, "Empty data table.");
                }


            }
            catch (Exception ex)
            {
                ErrorLog.SaveFilePartCost("Part Cost", ex);
                ErrorLog.SendMail("Part Cost", ex);
                InsertLog.Insert("Part cost", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
            }
        }
    }
}
