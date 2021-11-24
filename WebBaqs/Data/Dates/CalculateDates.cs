using System;
using WebBaqs.Data.Results;

namespace WebBaqs.Data.Dates
{
    public class CalculateDates
    {
        public static Tuple<DateTime, DateTime, bool> Monthly()
        {
            try
            {
                DateTime today = DateTime.Now;
                today = today.AddDays(-1);

                DateTime startDate = new DateTime(today.Year, today.Month, 1);
                DateTime finalDate;
                if (today.Month + 1 < 13)
                {
                    finalDate = new DateTime(today.Year, today.Month + 1, 1).AddDays(-1);
                }
                else
                {
                    finalDate = new DateTime(today.Year + 1, 1, 1).AddDays(-1);
                }
                return Tuple.Create(startDate, finalDate, true);

            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Calcular mes", ex);
                ErrorLog.SendMail("Calcular mes", ex);

                return Tuple.Create(DateTime.Now, DateTime.Now, false);
            }
        }

        public static Tuple<DateTime, DateTime, bool> Weekly()
        {
            try
            {
                DateTime startWeek;
                DateTime finalWeek = DateTime.Now;
                int delta = DayOfWeek.Monday - finalWeek.DayOfWeek;
                if (delta > 0)
                {
                    delta -= 7;
                    startWeek = finalWeek.AddDays(delta);
                }
                else
                {
                    startWeek = finalWeek.AddDays(-7);
                }

                //quito un día ya que estima 7 dás  y el septimo es el día actual, por lo tanto me arroja un error por parte de EPICOR al filtrar datos con el dia actual.
                finalWeek = finalWeek.AddDays(-1);


                return Tuple.Create(startWeek, finalWeek, true);

            }
            catch (Exception ex)
            {
                ErrorLog.SaveFile("Calcular semana", ex);
                ErrorLog.SendMail("Calcular semana", ex);

                return Tuple.Create(DateTime.Now, DateTime.Now, false);
            }
        }
    }
}
