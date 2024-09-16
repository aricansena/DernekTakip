using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;



namespace DAL
{
    public class Kullanici
    {
        public string Kimlik { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string KanGrubu { get; set; }
        public string Sehir { get; set; }
        public string AktifPasif { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }

        public DataTable GetMemberList()
        {

            DataTable dt = new DataTable();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbDataAdapter adtr = new OleDbDataAdapter("SELECT * FROM Kullanici", con);
                adtr.Fill(dt);
                con.Close();
            }
            return dt;
        }

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

        public void InsertKullanici(string kimlik, string ad, string soyad, string kanGrubu, string sehir, string email, string telefon, string aktifPasif, string AidatKodu)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("insert into Kullanici(Kimlik,Ad,Soyad,KanGrubu,Sehir,Email,Telefon,AktifPasif,AidatKodu) values ('" + kimlik + "','" + ad + "','" + soyad + "','" + kanGrubu + "', '" + sehir + "','" + email + "','" + telefon + "','" + aktifPasif + "','" + AidatKodu + "')", con);

                OleDbCommand cmd2 = new OleDbCommand("insert into AidatDurum(Kimlik,AidatKodu) values ('" + kimlik + "','" + AidatKodu + "')", con);
                OleDbCommand cmd3 = new OleDbCommand("insert into AidatT(AidatKodu) values ('" + AidatKodu + "')", con);

                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                con.Close();

            }
        }

        public void DeleteKullanici(string kimlik)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("delete * from Kullanici where Kimlik='" + kimlik + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }
        public void kullaniciarama(string sehir, DataTable dt)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbDataAdapter adtr = new OleDbDataAdapter("SELECT * FROM Kullanici where Sehir Like '" + sehir + "%'", con);
                adtr.Fill(dt);

            }
        }

        public void KanGrubuList(string kg, DataTable dt)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbDataAdapter adtr = new OleDbDataAdapter("SELECT * FROM Kullanici where KanGrubu Like '" + kg + "%'", con);
                adtr.Fill(dt);

            }
        }
        public void AktifPasifList(string ap, DataTable dt)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbDataAdapter adtr = new OleDbDataAdapter("SELECT * FROM Kullanici where AktifPasif Like '" + ap + "%'", con);
                adtr.Fill(dt);
            }
        }

        public void UpdateKullanici(string kimlik, string ad, string soyad, string kanGrubu, string sehir, string email, string telefon, string aktifPasif)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE Kullanici SET Kimlik='" + kimlik + "', Ad='" + ad + "', Soyad='" + soyad + "', KanGrubu='" + kanGrubu + "', Sehir='" + sehir + "', Email='" + email + "', Telefon='" + telefon + "', AktifPasif='" + aktifPasif + "' WHERE Kimlik='" + kimlik + "'", con);

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }


    }

  
}


