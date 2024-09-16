using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.OleDb;
using System.Data;




namespace BL
{
    public class deneme
    {
        private DAL.Kullanici d_kullanici;
        private DAL.Kullanici kullaniciarama = new Kullanici();
        public deneme()
        {
            d_kullanici = new Kullanici();
        }
        public DataTable GetMemberList()
        {
            return d_kullanici.GetMemberList();
        }
       
        public void InsertMember(string kimlik, string ad, string soyad, string kanGrubu, string sehir, string email, string telefon, string aktifPasif,string AidatKodu)
        {
            d_kullanici.InsertKullanici(kimlik, ad, soyad, kanGrubu, sehir, email, telefon, aktifPasif,AidatKodu);
        }
        public void DeleteMember(string kimlik)
        {
            d_kullanici.DeleteKullanici(kimlik);
        }
        public void KanGrubuList(string kg,DataTable dt)
        {
            kullaniciarama.KanGrubuList(kg, dt);
        }
        public void AktifPasifList(string ap, DataTable dt)
        {
            kullaniciarama.AktifPasifList(ap, dt);
        }

        public void kullanici(string kimlik, DataTable dt)
        {
            kullaniciarama.kullaniciarama(kimlik, dt);
        }
        public void UpdateMember(string kimlik, string ad, string soyad, string kanGrubu, string sehir, string email, string telefon, string aktifPasif)
        {
            d_kullanici.UpdateKullanici(kimlik, ad, soyad, kanGrubu, sehir, email, telefon, aktifPasif);
        }
    }
}

