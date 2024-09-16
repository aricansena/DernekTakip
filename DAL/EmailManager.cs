using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;


namespace BL
{

    public class EmailManager
    {
        public void SendEmailToDebtors(DataTable debtors)
        {
            foreach (DataRow debtor in debtors.Rows)
            {
                string userEmail = debtor["Email"].ToString();
                string debtorName = debtor["ad"].ToString();
                SendEmail(userEmail, debtorName);
            }
        }
        private void SendEmail(string toAddress, string debtorName)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("derneksincap@gmail.com");
                mail.To.Add(toAddress);
                mail.Subject = "Borç Bildirimi";
                mail.Body = $"Sayın {debtorName},\n\nBorcunuz bulunmaktadır. Lütfen en kısa sürede ödeme yapınız.";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential("derneksincap@gmail.com", "wpmseginkrdhexdc");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
        string connectionString = ConnectionHelper.ConnectionString;
        public DataTable GetBorcluListele()
        {
            DataTable result = new DataTable();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbDataAdapter adt = new OleDbDataAdapter("Select AidatT.*,Kullanici.* From((AidatT INNER JOIN AidatDurum ON AidatT.AidatKodu = AidatDurum.AidatKodu )INNER JOIN Kullanici ON AidatDurum.Kimlik = Kullanici.Kimlik) WHERE Borc>0", con);
                adt.Fill(result);
                con.Close();
            }
            return result;
        }
        public void SendCustomEmail(string toAddress, string subject, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("derneksincap@gmail.com");
                mail.To.Add(toAddress);
                mail.Subject = subject;
                mail.Body = body;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential("derneksincap@gmail.com", "wpmseginkrdhexdc");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
    public class ConnectionHelper
    {
        public static string ConnectionString
        {
            get
            {
                return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\senaa\\OneDrive\\Masaüstü\\GP\\DernekTakip\\DernekTakip\\bin\\Debug\\DernekTakip.accdb";
            }
        }
    }
}
