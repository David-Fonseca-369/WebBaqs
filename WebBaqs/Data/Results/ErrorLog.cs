using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using WebBaqs.Data.AppJson;

namespace WebBaqs.Data.Results
{
    public class ErrorLog
    {

        public static void SaveFile(string name, Exception ex)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Error: " + ex.Message);
                    sw.Close(); //
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Error: " + ex.Message + "\r\n");


            }
        }
        public static void SendMail(string subject, Exception ex)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");

            string body = "<html><head><Font Face = \"Calibri \" Color = \"Black\" Size=\"2\"><body><p> " + "Hora: " + time + " " + subject + " Error: " + ex.Message + "<p>" + Dns.GetHostName() + " <P></Body></html>";

            MailMessage correo = new MailMessage();
            correo.From = new MailAddress(LoadJsonData.SenderMail(), LoadJsonData.SenderTitle());

            ///
            SqlConnection cn = new SqlConnection(LoadJsonData.ConnetionString());
            cn.Open();
            SqlCommand cmd = new SqlCommand("users", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            List<string> users = new List<string>();

            try
            {
                while (reader.Read())
                {
                    users.Add(reader["email"].ToString());

                }
            }
            catch (Exception ex2)
            {
                SaveFile("Lista de usuarios, para correo", ex2);
            }

            foreach (string address in users)
            {

                correo.To.Add(address);
            }

            correo.Subject = subject;
            correo.Body = body;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = LoadJsonData.SMTP();
            try
            {
                smtp.Send(correo);
            }
            catch (Exception e)
            {
                SaveFile("Correo", e);
            }
        }
        public static void CustomResgistration(string name, string description)
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Error: " + description);
                    sw.Close(); //
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Error: " + description + "\r\n");


            }
        }
        public static void SendPersonalizedMail(string subject, string description)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");

            string body = "<html><head><Font Face = \"Calibri \" Color = \"Black\" Size=\"2\"><body><p> " + "Hora: " + time + " " + subject + " Error: " + description + "<p>" + Dns.GetHostName() + " <P></Body></html>";

            MailMessage correo = new MailMessage();
            correo.From = new MailAddress(LoadJsonData.SenderMail(), LoadJsonData.SenderTitle());

            ///
            SqlConnection cn = new SqlConnection(LoadJsonData.ConnetionString());
            cn.Open();
            SqlCommand cmd = new SqlCommand("users", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            List<string> users = new List<string>();

            try
            {
                while (reader.Read())
                {
                    users.Add(reader["email"].ToString());

                }
            }
            catch (Exception ex2)
            {
                SaveFile("Lista de usuarios, para correo", ex2);
            }

            foreach (string address in users)
            {

                correo.To.Add(address);
            }

            correo.Subject = subject;
            correo.Body = body;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = LoadJsonData.SMTP();
            try
            {
                smtp.Send(correo);
            }
            catch (Exception e)
            {
                SaveFile("Correo", e);
            }
        }
        public static void SaveFileJobs(string name, Exception ex)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "Jobs_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Error: " + ex.Message);
                    sw.Close(); //
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Error: " + ex.Message + "\r\n");


            }
        }
        public static void SaveFileLaborTime(string name, Exception ex)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "LaborTime_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Error: " + ex.Message);
                    sw.Close(); //
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Error: " + ex.Message + "\r\n");


            }
        }
        public static void SaveFileJobMaterials(string name, Exception ex)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "JobMaterials_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Error: " + ex.Message);
                    sw.Close(); //
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Error: " + ex.Message + "\r\n");


            }
        }
        public static void SaveFilePartCost(string name, Exception ex)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "PartCost_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Error: " + ex.Message);
                    sw.Close(); //
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Error: " + ex.Message + "\r\n");


            }
        }
        public static void SaveFileWIPReconciliation(string name, string message)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string route = @"" + LoadJsonData.Route() + "WIPReconciliation_Fecha_" + date + ".txt";


            if (File.Exists(route))
            {
                Debug.WriteLine("Existe");

                using (StreamWriter sw = File.AppendText(route))
                {
                    sw.WriteLine("Hora: " + time + " " + name + " Error: " + message);
                    sw.Close(); //
                }
            }
            else
            {
                File.WriteAllText(route, "Hora: " + time + " " + name + " Error: " + message + "\r\n");


            }
        }
    }
}
