using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebBaqs.Data.AppJson;
using WebBaqs.Data.Procedures;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.DataConversion
{
    public class ExcelToDataTable
    {
        public static Tuple<DataTable, bool> ConvertExcelToDatatable(string path, string option, DateTime startDate, DateTime finalDate)
        {
            bool hasHeaderRow = true;
            DataTable dt = new DataTable();
            int row_count = 0;
            string path2 = LoadJsonData.RouteBackUpBIUploadFiles() + "\\" + option + "InvWip_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";

            try
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Copy(path, path2);
                    }
                    catch (Exception ex)
                    {

                        ErrorLog.SaveFileWIPReconciliation("Excel a tabla de datos", "Error: no  se pudo copiar el archivo " + path + " " + ex.Message);
                        ErrorLog.SendPersonalizedMail("Excel a tabla de datos", "Error: no  se pudo copiar el archivo " + path + " " + ex.Message);

                        InsertLog.Insert("WIP Reconciliation", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);


                        return Tuple.Create(dt, false);
                    }
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        using (FileStream stream = File.OpenRead(path))
                        {
                            excelPackage.Load(stream);
                        }

                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.First();

                        if (worksheet.Dimension == null)
                        {
                            //si esta vacia
                            return Tuple.Create(dt, true);
                        }

                        //Cuento filas
                        for (int numRow = 1; ; numRow++)
                        {
                            string startColumn = worksheet.Cells[numRow, 1].Text;

                            if (string.IsNullOrEmpty(startColumn)) break;
                            row_count = numRow;
                        }

                        //Encabezado            
                        for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                        {
                            string columnName = "Column " + j;

                            var columnTypes = new HashSet<Type>();

                            for (int i = worksheet.Dimension.Start.Row + Convert.ToInt32(hasHeaderRow); i <= row_count; i++)
                            {
                                //Solo agregue el tipo si el valor de la celda no está vacío
                                if (worksheet.Cells[i, j].Value != null)
                                {
                                    columnTypes.Add(worksheet.Cells[i, j].Value.GetType());
                                }
                            }

                            var excelCell = worksheet.Cells[1, j].Value;

                            if (excelCell != null)
                            {
                                Type excelCellDataType = null;

                                // si hay una fila de encabezado, configure la siguiente celda para el tipo de datos y configure el nombre de la columna
                                if (hasHeaderRow == true)
                                {
                                    columnName = excelCell.ToString();
                                    // verifica si el nombre de la columna ya existe en la tabla de datos, si es así, crea un nombre único
                                    if (dt.Columns.Contains(columnName) == true)
                                    {
                                        columnName = columnName + "_" + j;
                                    }
                                }
                                // Selecciona el tipo de entrada para la columna
                                if (columnTypes.Count == 1)
                                {
                                    excelCellDataType = columnTypes.First();
                                }
                                else
                                {
                                    excelCellDataType = typeof(string);
                                }

                                // intenta determinar el tipo de datos para la columna (mirando la siguiente columna si hay una fila de encabezado)
                                if (excelCellDataType == typeof(DateTime))
                                {
                                    dt.Columns.Add(columnName, typeof(DateTime));
                                }
                                else if (excelCellDataType == typeof(Boolean))
                                {
                                    dt.Columns.Add(columnName, typeof(Boolean));
                                }
                                else if (excelCellDataType == typeof(Double))
                                {

                                    // determina si el valor es un decimal o int buscando un separador decimal
                                    // no es la solución más limpia, pero funciona ya que Excel siempre da un double
                                    if (excelCellDataType.ToString().Contains(".") || excelCellDataType.ToString().Contains(","))
                                    {
                                        dt.Columns.Add(columnName, typeof(Decimal));
                                    }
                                    else
                                    {
                                        dt.Columns.Add(columnName, typeof(Int64));
                                    }
                                }
                                else
                                {
                                    dt.Columns.Add(columnName, typeof(String));
                                }
                            }
                            else
                            {
                                dt.Columns.Add(columnName, typeof(String));
                            }
                        }

                        // comienza a agregar datos a la tabla de datos aquí haciendo un bucle en todas las filas y columnas                        
                        for (int i = worksheet.Dimension.Start.Row + Convert.ToInt32(hasHeaderRow); i <= row_count; i++)
                        {
                            // crea una nueva fila de tabla de datos
                            DataRow row = dt.NewRow();

                            // recorrer todas las columnas
                            for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                            {
                                var excelCell = worksheet.Cells[i, j].Value;

                                // agrega valor de celda a la tabla de datos
                                if (excelCell != null)
                                {
                                    try
                                    {
                                        row[j - 1] = excelCell;
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorLog.SaveFileWIPReconciliation("WIP Reconciliation", "Error: " + ex.Message);
                                        return Tuple.Create(dt, false);

                                    }
                                }
                            }
                            // agrega la nueva fila a la tabla de datos
                            dt.Rows.Add(row);
                        }
                    }
                }
                else
                {

                    ErrorLog.SaveFileWIPReconciliation("Excel a tabla de datos", "Error: El  archivo " + path + " no existe.");
                    ErrorLog.SendPersonalizedMail("Excel a tabla de datos", "Error: El  archivo " + path + " no existe.");
                    InsertLog.Insert("WIP Reconciliation", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: el archivo " + path + " no existe.");

                    return Tuple.Create(dt, false);
                }

                return Tuple.Create(dt, true);
            }
            catch (Exception ex)
            {

                ErrorLog.SaveFileWIPReconciliation("Excel a tabla de datos", "Error: " + ex.Message);
                ErrorLog.SendPersonalizedMail("Excel a tabla de datos", "Error: " + ex.Message);
                InsertLog.Insert("WIP Reconciliation", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
                return Tuple.Create(dt, false);
            }

        }
    }
}
