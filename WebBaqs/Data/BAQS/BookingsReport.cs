using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebBaqs.Data.AppJson;
using WebBaqs.Data.Dates;
using WebBaqs.Data.Procedures;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.BAQS
{
    public class BookingsReport
    {
        //Debe correr el mes actual
        public static async Task GetBookingsReport(string option)
        {
            if (option == null)
            {
                //Elimnar y crear tabla general 
                try
                {
                    bool deleteAll = await RemoveData.DeleteAllBooking();
                    if (deleteAll)
                    {
                        int currentYear = DateTime.Now.Year;
                        string fisrtDayOfTheYear = new DateTime(currentYear, 1, 1).ToString("MM/dd/yyyy");
                        await SaveBoookingsReport(LoadJsonData.Booking_url() + "?Prm_FInicial='" + fisrtDayOfTheYear + "'&Prm_FFinal='" + DateTime.Now.ToString("MM/dd/yyyy") + "'", "GENERAL", DateTime.Now, DateTime.Now); //Anterior 
                    }
                    else
                    {
                        InsertLog.Insert("Booking", "GENERAL", DateTime.Now, DateTime.Now, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.SaveFileLaborTime("Booking general", ex);
                    ErrorLog.SendMail("Booking general", ex);
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
                            bool deleteMonth = await RemoveData.DeleteByDatesBooking(startDate, finalDate);
                            if (deleteMonth)
                            {
                               await SaveBoookingsReport(LoadJsonData.Booking_url() + "?Prm_FInicial='" + startDate.ToString("MM/dd/yyyy") + "'&Prm_FFinal='" + finalDate.ToString("MM/dd/yyyy") + "'", strOption, startDate, finalDate);
                            }
                            else
                            {
                                InsertLog.Insert("Booking", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Data deletion failed.");
                            }
                        }
                        else
                        {
                            InsertLog.Insert("Booking", strOption, startDate, finalDate, "Deletion failed at " + DateTime.Now, "Error when calculating dates.");
                        }
                    }                   
                }
                catch (Exception ex)
                {
                    //ErrorLog.SaveFileLaborTime("Labor Time", ex);
                    ErrorLog.SendMail("Booking", ex);
                }
            }
        }

        private static async Task SaveBoookingsReport(string url, string option, DateTime startDate, DateTime finalDate )
        {
            try
            {
                var client = new RestClient(url);
                client.Authenticator = new NtlmAuthenticator(LoadCredentials.Email(), LoadCredentials.Password());
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Accept", "application/json");

                IRestResponse response = await client.ExecuteAsync(request);
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
                    objBulk.DestinationTableName = "Booking";

                    objBulk.ColumnMappings.Add("OrderHed_Company", "OrderHed_Company");
                    objBulk.ColumnMappings.Add("CustGrup_GroupDesc", "CustGrup_GroupDesc");
                    objBulk.ColumnMappings.Add("OrderHed_OrderDate", "OrderHed_OrderDate");
                    objBulk.ColumnMappings.Add("OrderHed_NeedByDate", "OrderHed_NeedByDate");
                    objBulk.ColumnMappings.Add("OrderHed_OrderNum", "OrderHed_OrderNum");
                    objBulk.ColumnMappings.Add("Calculated_TotalRevenue", "Calculated_TotalRevenue");
                    objBulk.ColumnMappings.Add("Customer_Name", "Customer_Name");
                    objBulk.ColumnMappings.Add("ShipTo_Name", "ShipTo_Name");
                    objBulk.ColumnMappings.Add("ShipTo_City", "ShipTo_City");
                    objBulk.ColumnMappings.Add("ShipTo_State", "ShipTo_State");
                    objBulk.ColumnMappings.Add("SalesRep1_Name", "SalesRep1_Name");
                    objBulk.ColumnMappings.Add("SalesRep_Name", "SalesRep_Name");
                    objBulk.ColumnMappings.Add("OrderHed_PONum", "OrderHed_PONum");
                    objBulk.ColumnMappings.Add("SalesCat_Description", "SalesCat_Description");
                    objBulk.ColumnMappings.Add("Customer_Sector_c", "Customer_Sector_c");
                    objBulk.ColumnMappings.Add("Customer_Client_c", "Customer_Client_c");                    

                    await cn.OpenAsync();
                    await objBulk.WriteToServerAsync(dsTopics);
                    await cn.CloseAsync();

                    //Actualizar utlimo registro de Booking
                    //UpdateDate.updateJobs(2);

                    //Guardar archivo de textp
                    //SuccessfulLog.SaveFileLaborTime("Booking");

                    //Insertar registro en la DB
                    InsertLog.Insert("Booking", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                    Debug.WriteLine("Exito Booking");
                }
                else
                {
                    //UpdateDate.updateJobs(2);
                    //SuccessfulLog.SaveFileLaborTime("Labor Time");
                    InsertLog.Insert("Booking", option, startDate, finalDate, "Successful at " + DateTime.Now, "Empty data table.");
                    Debug.WriteLine("Fallo Booking");

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fallo Booking: {ex.Message}");
                //ErrorLog.SaveFileJobMaterials("Job Materials", ex);
                ErrorLog.SendMail("Booking", ex);
                InsertLog.Insert("Booking", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
            }

        }
    }
}
