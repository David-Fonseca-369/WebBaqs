using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebBaqs.Data.AppJson;
using WebBaqs.Data.DataConversion;
using WebBaqs.Data.Dates;
using WebBaqs.Data.Procedures;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.BAQS
{
    public class WIPReconciliation
    {
        public static void GetWIPReconciliation(string opcion)
        {
            if (opcion != null)
            {
                try
                {
                    string strOption = opcion.ToUpper();
                    if (strOption == "MONTHLY")
                    {
                        var month = CalculateDates.Monthly();
                        DateTime startMonth = month.Item1;
                        DateTime finalMonth = month.Item2;
                        bool isCorrect = month.Item3;

                        if (isCorrect)
                        {
                            //Realizo la conversion de excel a dataTable.
                            var convertToDateTable = ExcelToDataTable.ConvertExcelToDatatable(LoadJsonData.RouteBIUploadFiles_Month(), strOption, startMonth, finalMonth);
                            DataTable tableWIPReconciliation = convertToDateTable.Item1;
                            bool verifyConversion = convertToDateTable.Item2;

                            //Si es correcta procedo a eliminar los datos del mes indicado y traspaso los obtenidos.
                            if (verifyConversion)
                            {
                                bool deleteMonth = RemoveData.DeleteByDatesWIPReconciliation(startMonth, finalMonth, strOption);

                                //Si la eliminacion es correcta, procedo a guardar los datos almacenados en el DataTable
                                if (deleteMonth)
                                {
                                    bool checkSaved = saveWIPReconciliation(LoadJsonData.RouteBIUploadFiles_Month(), strOption, startMonth, finalMonth, tableWIPReconciliation);

                                    //Si el traspaso de datos es correcto, procedo a eliminar el archivo.
                                    if (checkSaved)
                                    {
                                        try
                                        {
                                            File.Delete(LoadJsonData.RouteBIUploadFiles_Month());
                                            UpdateDate.updateJobs(5);
                                            SuccessfulLog.SaveFileWIPReconciliaton("WIP Reconciliation");
                                            InsertLog.Insert("WIP Reconciliation", strOption, startMonth, finalMonth, "Successful at " + DateTime.Now, "Succcessful registration.");

                                        }
                                        catch (Exception ex)
                                        {
                                            ErrorLog.SaveFileWIPReconciliation("WIP Reconciliation", "Error: " + ex.Message);
                                            ErrorLog.SendPersonalizedMail("WIP Reconciliation", "Error: " + ex.Message);

                                            //Vuelvo a llamar el guardado de error en la bitacora, debido a que si en la anterior hay un error, el proceso terminaría ahí.
                                            InsertLog.Insert("WIP Reconciliation", strOption, startMonth, finalMonth, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
                                        }
                                    }
                                }
                            }
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
                            //Realizo la conversion de excel a dataTable.
                            var convertToDateTable = ExcelToDataTable.ConvertExcelToDatatable(LoadJsonData.RouteBIUploadFiles_Week(), strOption, startWeek, finalWeek);
                            DataTable tableWIPReconciliation = convertToDateTable.Item1;
                            bool verifyConversion = convertToDateTable.Item2;

                            //Si es correcta procedo a eliminar los datos de la semana indicada y traspaso los obtenidos.
                            if (verifyConversion)
                            {
                                bool deleteWeek = RemoveData.DeleteByDatesWIPReconciliation(startWeek, finalWeek, strOption);

                                //Si la eliminacion es correcta, procedo a guardar los datos almacenados en el DataTable
                                if (deleteWeek)
                                {
                                    bool checkSaved = saveWIPReconciliation(LoadJsonData.RouteBIUploadFiles_Week(), strOption, startWeek, finalWeek, tableWIPReconciliation);

                                    //Si el traspaso de datos es correcto, procedo a eliminar el archivo.
                                    if (checkSaved)
                                    {
                                        try
                                        {
                                            File.Delete(LoadJsonData.RouteBIUploadFiles_Week());
                                            UpdateDate.updateJobs(5);
                                            SuccessfulLog.SaveFileWIPReconciliaton("WIP Reconciliation");
                                            InsertLog.Insert("WIP Reconciliation", strOption, startWeek, finalWeek, "Successful at " + DateTime.Now, "Succcessful registration.");

                                        }
                                        catch (Exception ex)
                                        {
                                            ErrorLog.SaveFileWIPReconciliation("WIP Reconciliation", "Error: " + ex.Message);
                                            ErrorLog.SendPersonalizedMail("WIP Reconciliation", "Error: " + ex.Message);

                                            //Vuelvo a llamar el guardado de error en la bitacora, debido a que si en la anterior hay un error, el proceso terminaría ahí.
                                            InsertLog.Insert("WIP Reconciliation", strOption, startWeek, finalWeek, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else if (strOption == "DAILY")
                    {
                        //Día anterior
                        DateTime yesterday = DateTime.Now.AddDays(-1);


                        //Realizo la conversion de excel a dataTable.
                        var convertToDateTable = ExcelToDataTable.ConvertExcelToDatatable(LoadJsonData.RouteBIUploadFiles_Day(), strOption, yesterday, yesterday);
                        DataTable tableWIPReconciliation = convertToDateTable.Item1;
                        bool verifyConversion = convertToDateTable.Item2;

                        //Si es correcta procedo a eliminar los datos de la semana indicada y traspaso los obtenidos.
                        if (verifyConversion)
                        {
                            bool deleteDay = RemoveData.DeleteDayWIPReconciliation(yesterday, strOption);

                            //Si la eliminacion es correcta, procedo a guardar los datos almacenados en el DataTable
                            if (deleteDay)
                            {
                                bool checkSaved = saveWIPReconciliation(LoadJsonData.RouteBIUploadFiles_Week(), strOption, yesterday, yesterday, tableWIPReconciliation);

                                //Si el traspaso de datos es correcto, procedo a eliminar el archivo.
                                if (checkSaved)
                                {
                                    try
                                    {
                                        File.Delete(LoadJsonData.RouteBIUploadFiles_Day());
                                        UpdateDate.updateJobs(5);
                                        SuccessfulLog.SaveFileWIPReconciliaton("WIP Reconciliation");
                                        InsertLog.Insert("WIP Reconciliation", strOption, yesterday, yesterday, "Successful at " + DateTime.Now, "Succcessful registration.");

                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorLog.SaveFileWIPReconciliation("WIP Reconciliation", "Error: " + ex.Message);
                                        ErrorLog.SendPersonalizedMail("WIP Reconciliation", "Error: " + ex.Message);

                                        //Vuelvo a llamar el guardado de error en la bitacora, debido a que si en la anterior hay un error, el proceso terminaría ahí.
                                        InsertLog.Insert("WIP Reconciliation", strOption, yesterday, yesterday, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorLog.SaveFileWIPReconciliation("WIP Reconciliation", "Error: " + ex.Message);
                    ErrorLog.SendPersonalizedMail("WIP Reconciliation", "Error: " + ex.Message);
                    InsertLog.Insert("WIP Reconciliation", opcion.ToUpper(), DateTime.Now, DateTime.Now, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);

                }
            }
        }

        public static bool saveWIPReconciliation(string path, string option, DateTime startDate, DateTime finalDate, DataTable tableWIPReconciliation)
        {
            try
            {
                if (tableWIPReconciliation.Rows.Count > 0)
                {
                    SqlConnection cn = new SqlConnection(LoadJsonData.ConnetionString());
                    SqlBulkCopy sqlBulk = new SqlBulkCopy(cn);

                    sqlBulk.DestinationTableName = "WIPReconciliation";
                    sqlBulk.ColumnMappings.Add("LblAccount", "LblAccount");
                    sqlBulk.ColumnMappings.Add("TxtAccount", "TxtAccount");
                    sqlBulk.ColumnMappings.Add("TxtHDate", "TxtHDate");
                    sqlBulk.ColumnMappings.Add("TxtTrnType", "TxtTrnType");
                    sqlBulk.ColumnMappings.Add("TxtTrnPosted", "TxtTrnPosted");
                    sqlBulk.ColumnMappings.Add("TxtJobCallField", "TxtJobCallField");
                    sqlBulk.ColumnMappings.Add("TxtDBTranAmt", "TxtDBTranAmt");
                    sqlBulk.ColumnMappings.Add("TxtCRTranAmt", "TxtCRTranAmt");
                    sqlBulk.ColumnMappings.Add("TxtTmpReference", "TxtTmpReference");
                    sqlBulk.ColumnMappings.Add("TxtHDate2", "TxtHDate2");
                    sqlBulk.ColumnMappings.Add("TxtTmpPartNum", "TxtTmpPartNum");
                    sqlBulk.ColumnMappings.Add("TxtGrp2Credit", "TxtGrp2Credit");
                    sqlBulk.ColumnMappings.Add("TxtGrp2Debit", "TxtGrp2Debit");
                    sqlBulk.ColumnMappings.Add("LblGrandTotalL", "LblGrandTotalL");
                    sqlBulk.ColumnMappings.Add("TxtSumDBTranAmt", "TxtSumDBTranAmt");
                    sqlBulk.ColumnMappings.Add("TxtSumCRTranAmt", "TxtSumCRTranAmt");

                    cn.Open();
                    sqlBulk.WriteToServer(tableWIPReconciliation);
                    cn.Close();
                    return true;
                }
                else
                {
                    UpdateDate.updateJobs(5);
                    ErrorLog.SaveFileWIPReconciliation("WIP Reconciliation", "Error: Tabla vacía.");
                    ErrorLog.SendPersonalizedMail("WIP Reconciliation", "Error: Tabla vacía en " + path);
                    InsertLog.Insert("WIP Reconciliaton", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: empty table.");
                    return false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error insertar" + ex.Message);
                ErrorLog.SaveFileWIPReconciliation("WIP Reconciliation", "Error " + ex.Message);
                ErrorLog.SendPersonalizedMail("WIP Reconciliation", "Error " + ex.Message);
                InsertLog.Insert("WIP Reconciliation", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
                return false;
            }
        }

    }
}
