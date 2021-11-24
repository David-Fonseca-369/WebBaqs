using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebBaqs.Data.AppJson;

namespace WebBaqs.Data.Results
{
    public class SuccessfulLog
    {
        public static void SaveFile(string name)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Successful registration.");
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Successful registration." + "\r\n");
            }
        }
        public static void SaveFileJobs(string name)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "Jobs_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Successful registration.");
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Successful registration." + "\r\n");
            }
        }
        public static void SaveFileLaborTime(string name)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "LaborTime_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Successful registration.");
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Successful registration." + "\r\n");
            }
        }
        public static void SaveFileJobMaterials(string name)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "JobMaterials_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Successful registration.");
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Successful registration." + "\r\n");
            }
        }
        public static void SaveFilePartCost(string name)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "PartCost_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Successful registration.");
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Successful registration." + "\r\n");
            }
        }
        public static void SaveFileWIPReconciliaton(string name)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "WIPReconciliaton_Fecha" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Successful registration.");
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Successful registration." + "\r\n");
            }
        }
    }
}
