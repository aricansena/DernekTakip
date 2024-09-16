using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL;
using System.Net.Mail;
using System.Net;
using System.Data.OleDb;
using ZedGraph;

namespace BL
{
    public class AidatDeneme
    {
        public DAL.AidatT d_AidatT;
        private DAL.AidatT aidatT;
        private AidatT aidatBL;



        public AidatDeneme()
        {
            d_AidatT = new AidatT();
            d_AidatT = new DAL.AidatT();
            aidatT = new DAL.AidatT();

        }
        public DataTable GetAidatTList()
        {
            return d_AidatT.GetAidatTList();
        }
        public DataTable GetBorcluListele()
        {
            return d_AidatT.GetBorcluListele();
        }
        public void UpdateAidatT(string AidatKodu, string Aidat, string Odenen, DateTime Tarih,  string BorcluMu,string Kimlik)
        {
            d_AidatT.UpdateAidatT(AidatKodu, Aidat,  Odenen, Tarih,BorcluMu,Kimlik);
        }

     

        public DataTable GetYearlyIncomeData()
        {
            // Yıllık gelir verilerini almak için veritabanından gerekli sorguları yapın
            // Bu kısmı kendi veritabanınıza ve mantığınıza göre özelleştirebilirsiniz
            throw new NotImplementedException();
        }

       
      

        public PointPairList GetMonthlyIncomeGraphData()
        {
            DataTable monthlyIncomeData = GetAidatTList();

            PointPairList resultPointList = new PointPairList();

            foreach (DataRow row in monthlyIncomeData.Rows)
            {
                foreach (DataColumn column in monthlyIncomeData.Columns)
                {
                    Console.WriteLine(column.ColumnName);
                }

                DateTime date = Convert.ToDateTime(row["Tarih"]);
                double income = Convert.ToDouble(row["Odenen"]);

                resultPointList.Add(new XDate(date), income);
            }

            return resultPointList;
        }

        public PointPairList GetYearlyIncomeGraphData()
        {
            DataTable yearlyIncomeData = GetAidatTList();

            PointPairList resultPointList = new PointPairList();

            foreach (DataRow row in yearlyIncomeData.Rows)
            {
                foreach (DataColumn column in yearlyIncomeData.Columns)
                {
                    Console.WriteLine(column.ColumnName);
                }

                DateTime date = Convert.ToDateTime(row["Tarih"]);
                double income = Convert.ToDouble(row["Odenen"]);

                resultPointList.Add(new XDate(date), income);
            }

            return resultPointList;
        }

    }

    

}
