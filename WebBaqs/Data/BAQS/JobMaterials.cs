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
    public class JobMaterials
    {
        public static void GetJobMaterials(string option)
        {
            if (option == null)
            {
                try
                {
                    string deleteAll = RemoveData.DeleteAllJobMaterials();

                    if (deleteAll == "OK")
                    {
                        SaveJobMaterials(LoadJsonData.JobMaterials_url(), "GENERAL", DateTime.Now, DateTime.Now);
                    }
                    else
                    {
                        InsertLog.Insert("Job materials", "GENERAL", DateTime.Now, DateTime.Now, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                    }

                }
                catch (Exception ex)
                {
                    ErrorLog.SaveFileJobMaterials("Job materials", ex);
                    ErrorLog.SendMail("Job materials", ex);
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
                        bool iscorrect = month.Item3;


                        if (iscorrect)
                        {
                            ////Fechas prueba
                            //DateTime startDate = new DateTime(2020, 03, 19);
                            //DateTime finalDate = new DateTime(2020, 04, 10);

                            string deleteMonth = RemoveData.DeleteByDatesJobMaterials(startDate, finalDate);
                            if (deleteMonth == "OK")
                            {
                                SaveJobMaterials(LoadJsonData.JobMaterials_url() + "?$filter=JobHead_CreateDate ge datetime\'" + startDate.ToString("yyyy-MM-dd") + "\' and JobHead_CreateDate le datetime\'" + finalDate.ToString("yyyy-MM-dd") + "\'", strOption, startDate, finalDate);
                            }
                            else
                            {
                                InsertLog.Insert("Job materials", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Job materials", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
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
                            ////Fechas prueba
                            //DateTime startWeek = new DateTime(2020, 03, 19);
                            //DateTime finalWeek = new DateTime(2020, 04, 10);


                            string deleteWeek = RemoveData.DeleteByDatesJobMaterials(startWeek, finalWeek);

                            if (deleteWeek == "OK")
                            {
                                SaveJobMaterials(LoadJsonData.JobMaterials_url() + "?$filter=JobHead_CreateDate ge datetime\'" + startWeek.ToString("yyyy-MM-dd") + "\' and JobHead_CreateDate le datetime\'" + finalWeek.ToString("yyyy-MM-dd") + "\'", strOption, startWeek, finalWeek);
                            }
                            else
                            {
                                InsertLog.Insert("Job materials", strOption, startWeek, finalWeek, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Job materials", strOption, startWeek, finalWeek, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
                        }


                    }
                    else if (strOption == "DAILY")
                    {
                        DateTime yesterday = DateTime.Now.AddDays(-1);

                        //Fecha prueba
                        //DateTime yesterday = new DateTime(2020, 03, 19);

                        string deleteDay = RemoveData.DeleteDayJobMaterials(yesterday);

                        if (deleteDay == "OK")
                        {
                            SaveJobMaterials(LoadJsonData.JobMaterials_url() + "?$filter=JobHead_CreateDate eq datetime\'" + yesterday.ToString("yyy-MM-dd") + "\'", strOption, yesterday, DateTime.Now);
                        }
                        else
                        {
                            InsertLog.Insert("Job materials", strOption, yesterday, DateTime.Now, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
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
                    ErrorLog.SaveFileJobMaterials("Job materials", ex);
                    ErrorLog.SendMail("Job materials", ex);

                }

            }
        }

        static void SaveJobMaterials(string url, string option, DateTime startDate, DateTime finalDate)
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
                    objBulk.DestinationTableName = "JOBS_MATERIALS";

                    objBulk.ColumnMappings.Add("JobHead_CreateDate", "JobHead_CreateDate");
                    objBulk.ColumnMappings.Add("JobHead_JobNum", "JobHead_JobNum");
                    objBulk.ColumnMappings.Add("JobHead_JobComplete", "JobHead_JobComplete");
                    objBulk.ColumnMappings.Add("JobHead_JobClosed", "JobHead_JobClosed");
                    objBulk.ColumnMappings.Add("JobOper_OpComplete", "JobOper_OpComplete");
                    objBulk.ColumnMappings.Add("JobMtl_MtlSeq", "JobMtl_MtlSeq");
                    objBulk.ColumnMappings.Add("JobMtl_AssemblySeq", "JobMtl_AssemblySeq");
                    objBulk.ColumnMappings.Add("JobMtl_PartNum", "JobMtl_PartNum");
                    objBulk.ColumnMappings.Add("JobMtl_Description", "JobMtl_Description");
                    objBulk.ColumnMappings.Add("JobMtl_IUM", "JobMtl_IUM");
                    objBulk.ColumnMappings.Add("JobMtl_RequiredQty", "JobMtl_RequiredQty");
                    objBulk.ColumnMappings.Add("JobMtl_IssuedQty", "JobMtl_IssuedQty");
                    objBulk.ColumnMappings.Add("JobMtl_TotalCost", "JobMtl_TotalCost");
                    objBulk.ColumnMappings.Add("JobMtl_IssuedComplete", "JobMtl_IssuedComplete");
                    objBulk.ColumnMappings.Add("JobOper_OprSeq", "JobOper_OprSeq");
                    objBulk.ColumnMappings.Add("JobOper_OpDesc", "JobOper_OpDesc");
                    objBulk.ColumnMappings.Add("JobMtl_BackFlush", "JobMtl_BackFlush");
                    objBulk.ColumnMappings.Add("JobMtl_BuyIt", "JobMtl_BuyIt");
                    objBulk.ColumnMappings.Add("JobMtl_Ordered", "JobMtl_Ordered");
                    objBulk.ColumnMappings.Add("OrderRel_ReqDate", "OrderRel_ReqDate");

                    cn.Open();
                    objBulk.WriteToServer(dsTopics);
                    cn.Close();

                    UpdateDate.updateJobs(3);
                    SuccessfulLog.SaveFileJobMaterials("Job materials");
                    InsertLog.Insert("Job materials", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                }
                else
                {
                    UpdateDate.updateJobs(3);
                    SuccessfulLog.SaveFileJobMaterials("Job materials");
                    InsertLog.Insert("Job materials", option, startDate, finalDate, "Successful at " + DateTime.Now, "Empty data table.");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.SaveFileJobMaterials("Job Materials", ex);
                ErrorLog.SendMail("Job Materials", ex);
                InsertLog.Insert("Job materials", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
            }
        }

    }
}
