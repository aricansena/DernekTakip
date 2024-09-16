using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class AidatT
    {

        public string AidatKodu { get; set; }

        public string Aidat { get; set; }

        public string Odenen { get; set; }

        public string Borc { get; set; }

        public DateTime Tarih { get; set; }

        public string BorcluMu { get; set; }

        string connectionString = ConnectionHelper.ConnectionString;

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

        public DataTable GetAidatTList()
        {
            DataTable result = new DataTable();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbDataAdapter adtr = new OleDbDataAdapter("SELECT AidatT.*,AidatDurum.BorcluMu,AidatDurum.Tarih,Kullanici.Kimlik,Kullanici.Ad,Kullanici.Soyad,Kullanici.AktifPasif,Kullanici.Email,Kullanici.Telefon FROM(( AidatT INNER JOIN AidatDurum ON AidatT.AidatKodu = AidatDurum.AidatKodu )INNER JOIN Kullanici ON AidatDurum.Kimlik = Kullanici.Kimlik)", con);
                adtr.Fill(result);
                con.Close();
            }
            return result;
        }
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

        public void UpdateAidatT(string AidatKodu, string Aidat, string Odenen, DateTime Tarih, string BorcluMu, string Kimlik)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbCommand cmd2 = new OleDbCommand("UPDATE AidatDurum SET  Tarih='" + Tarih + "', BorcluMu='" + BorcluMu + "' WHERE AidatKodu= '" + AidatKodu + "'", con);
                OleDbCommand cmd = new OleDbCommand("UPDATE AidatT SET Aidat='" + Aidat + "', Odenen='" + Odenen + "' WHERE AidatKodu= '" + AidatKodu + "'", con);
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                con.Close();
                GetAidatTList();
            }
        }

     

        public DataTable GetMonthlyIncomeData(int year, int month)
        {
            DataTable monthlyIncomeData = GetAidatTList();
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("Tarih", typeof(DateTime));
            resultTable.Columns.Add("Odenen", typeof(double));

            foreach (DataRow row in monthlyIncomeData.Rows)
            {
                DateTime date = Convert.ToDateTime(row["Tarih"]);

                if (date.Year == year && date.Month == month)
                {
                    double income = Convert.ToDouble(row["Odenen"]);
                    resultTable.Rows.Add(date, income);
                }
            }

            return resultTable;
        }

        public DataTable GetYearlyIncomeData(int year)
        {
            DataTable yearlyIncomeData = GetAidatTList();
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("Tarih", typeof(DateTime));
            resultTable.Columns.Add("Odenen", typeof(double));

            foreach (DataRow row in yearlyIncomeData.Rows)
            {
                DateTime date = Convert.ToDateTime(row["Tarih"]);

                if (date.Year == year)
                {
                    double income = Convert.ToDouble(row["Odenen"]);
                    resultTable.Rows.Add(date, income);
                }
            }

            return resultTable;
        }




    }
}
