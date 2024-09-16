using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BL;
//using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
//using iText.Kernel.Geom;
using iText.Layout.Borders;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using System.Net;
using System.Net.Mail;
using Microsoft.SqlServer.Server;
using DAL;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Document = iText.Layout.Document;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using iText.StyledXmlParser.Jsoup.Nodes;
using ZedGraph;
using System.Diagnostics;
using System.Runtime.CompilerServices;



namespace PL
{
    public partial class Form0 : System.Windows.Forms.Form
    {
        private DAL.AidatT dataAccess = new DAL.AidatT(); 
        private BL.deneme pl_deneme;
        private BL.AidatDeneme pl_AidatDeneme;
        private BL.EmailManager emailManager;
        private BL.AidatDeneme aidatBL;

        public Form0()
        {
            pl_deneme = new BL.deneme();
            pl_AidatDeneme = new BL.AidatDeneme();
            emailManager = new BL.EmailManager();
            aidatBL = new BL.AidatDeneme();

            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.CellClick += dataGridView1_CellClick;

            dataGridView5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView5.ReadOnly = true;
            dataGridView5.CellClick += dataGridView5_CellClick;

            
        }

  
      
        private void Form0_Load(object sender, EventArgs e)
        {
            AidatListele();
            KisiListele();
            BorcluListele();
            CreateMonthlyGraph();
            CreateYearlyGraph();
        }

        private void KisiListele()
        {
            DataTable dt = pl_deneme.GetMemberList();
            dataGridView1.DataSource = dt;
        }
        private void AidatListele()
        {
            DataTable dtt = pl_AidatDeneme.GetAidatTList();
            dataGridView5.DataSource = dtt;
        }
        private void BorcluListele()
        {
            DataTable dtt = pl_AidatDeneme.GetBorcluListele();
            dataGridView7.DataSource = dtt;
        }
        private void pdf_Click(object sender, EventArgs e)
        {
            try
            {
                PDFManager pdfManager = new PDFManager();
                DataTable debtors = dataAccess.GetBorcluListele();
                string pdfFilePath = @"C:\\Users\\senaa\\OneDrive\\Masaüstü\\GP\borclu.pdf";
                pdfManager.CreateDebtorsPDF(debtors, pdfFilePath);
                System.Diagnostics.Process.Start(pdfFilePath);
                MessageBox.Show("Borçlu kişilerin listesi PDF olarak kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form giris = new Form();
            {
                giris.StartPosition = FormStartPosition.CenterScreen;
                giris.Show();
            }
        }

        private void uyeekle_Click_1(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int x;
            x = rnd.Next(99999, 999999);
            string y = "#" + x;
            pl_deneme.InsertMember(tc.Text, ad.Text, soyad.Text, kangrubu.Text, sehir.Text, email.Text, telefon.Text, aktifpasif.Text, y);
            KisiListele();
            AidatListele();
        }
        private void uyesil_Click(object sender, EventArgs e)
        {
            pl_deneme.DeleteMember(dataGridView1.CurrentRow.Cells["Kimlik"].Value.ToString());
            KisiListele();
            AidatListele();
            BorcluListele();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            DataTable dtt = new DataTable();
            pl_deneme.kullanici(shrlistele.Text, dtt);
            dataGridView3.DataSource = dtt;
           
        }
        private void kglistele_TextChanged(object sender, EventArgs e)
        {
           
            DataTable dtt = new DataTable();
            pl_deneme.KanGrubuList(kglistele.Text, dtt);
            dataGridView2.DataSource = dtt;
      
        }
        private void aplistele_TextChanged(object sender, EventArgs e)
        {
            
            DataTable dtt = new DataTable();
            pl_deneme.AktifPasifList(aplistele.Text, dtt);
            dataGridView4.DataSource = dtt;
        
        }
        private void uyeguncelle_Click(object sender, EventArgs e)
        {
            try
            {
                pl_deneme.UpdateMember(tc.Text, ad.Text, soyad.Text, kangrubu.Text, sehir.Text, email.Text, telefon.Text, aktifpasif.Text);

                ClearTextBoxes();
                KisiListele();
                AidatListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private string GetValueFromCell(DataGridViewRow row, string columnName)
        {
            if (row.Cells[columnName].Value != null)
            {
                return row.Cells[columnName].Value.ToString();
            }
            return string.Empty;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                // Satırdaki hücre değerlerini TextBox'lara yerleştir
                tc.Text = GetValueFromCell(row, "Kimlik");
                ad.Text = GetValueFromCell(row, "Ad");
                soyad.Text = GetValueFromCell(row, "Soyad");
                kangrubu.Text = GetValueFromCell(row, "KanGrubu");
                sehir.Text = GetValueFromCell(row, "Sehir");
                email.Text = GetValueFromCell(row, "Email");
                telefon.Text = GetValueFromCell(row, "Telefon");
                aktifpasif.Text = GetValueFromCell(row, "AktifPasif");
            }
        }
        private void ClearTextBoxes()
        {
            tc.Text = string.Empty;
            ad.Text = string.Empty;
            soyad.Text = string.Empty;
            kangrubu.Text = string.Empty;
            sehir.Text = string.Empty;
            email.Text = string.Empty;
            telefon.Text = string.Empty;
            aktifpasif.Text = string.Empty;
        }
        private void aidatguncelle_Click(object sender, EventArgs e)
        {
            try
            {
                pl_AidatDeneme.UpdateAidatT(AidatKodu.Text, Aidat.Text, Odenen.Text, dateTimePicker1.Value, Borclumu.Text, tc.Text);
                ClearTextBoxes2();
                AidatListele();
                BorcluListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView5.Rows.Count)
            {
                DataGridViewRow row = dataGridView5.Rows[e.RowIndex];
                AidatKodu.Text = GetValueFromCell(row, "AidatKodu");
                Aidat.Text = GetValueFromCell(row, "Aidat");
                Odenen.Text = GetValueFromCell(row, "Odenen");
                Borc.Text = GetValueFromCell(row, "Borc");
                Borclumu.Text = GetValueFromCell(row, "BorcluMu");
            }
        }
        private void ClearTextBoxes2()
        {
            AidatKodu.Text = string.Empty;
            Aidat.Text = string.Empty;
            Odenen.Text = string.Empty;
            Borc.Text = string.Empty;
            Borclumu.Text = string.Empty;
        }
  
        MailMessage eposta = new MailMessage();
        private void mailgonder_Click(object sender, EventArgs e)
        {
            try
            {
                string toAddress = textBoxEposta.Text;
                string subject = textBoxKonu.Text;
                string body = textBoxIcerik.Text;
                emailManager.SendCustomEmail(toAddress, subject, body);
                MessageBox.Show("E-posta başarıyla gönderildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Tarihlerarasilistele_Click(object sender, EventArgs e)
        {

            DataTable dtt = new DataTable();
            OleDbDataAdapter adt = new OleDbDataAdapter("SELECT Kullanici.*,AidatDurum.Tarih,AidatT.Aidat,AidatT.Odenen,AidatT.Borc,AidatDurum.BorcluMu FROM ((AidatT INNER JOIN AidatDurum ON AidatT.AidatKodu = AidatDurum.AidatKodu) INNER JOIN Kullanici ON AidatDurum.Kimlik = Kullanici.Kimlik) WHERE Tarih BETWEEN @tarih1 AND @tarih2", con);
            adt.SelectCommand.Parameters.AddWithValue("@tarih1", dateTimePickerson);
            adt.SelectCommand.Parameters.AddWithValue("@tarih2", dateTimePickerbas);
            con.Open();
            adt.Fill(dtt);
            dataGridView6.DataSource = dtt;
            con.Close();

        }

        // Bu connection'ı sadece Tarihlerarası listelemede kullandık. DateTime veritipinde sıkıntı yaşadığımız için Katmanlı Mimari ile yapamadık.

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\senaa\\OneDrive\\Masaüstü\\GP\\DernekTakip\\DernekTakip\\bin\\Debug\\DernekTakip.accdb");
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void borclumail_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable debtors = dataAccess.GetBorcluListele();
                emailManager.SendEmailToDebtors(debtors);
                MessageBox.Show("Borçlu kişilere e-posta başarıyla gönderildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string logFilePath = @"D:\GPsenagüncel\GP\error_log.txt";
            }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
        ZedGraph.ZedGraphControl zedGraphMonthly = new ZedGraph.ZedGraphControl();
        ZedGraph.ZedGraphControl zedGraphYearly = new ZedGraph.ZedGraphControl();
        private void UpdateGraphAylar()
        {
            // Önce mevcut grafiği temizleyin
            zedGraphMonthly.GraphPane.CurveList.Clear();

            // Yeni verileri alın
            PointPairList updatedData = aidatBL.GetMonthlyIncomeGraphData();


            // Yeni verilerle grafiği güncelleyin
            ZedGraph.BarItem barMonthly = zedGraphMonthly.GraphPane.AddBar("Aidat Geliri", updatedData, System.Drawing.Color.Green);

            // Yeniden çizim işlemlerini gerçekleştirin
            zedGraphMonthly.AxisChange();
            zedGraphMonthly.Invalidate();
        }
        private void UpdateGraphYıllar()
        {
            // Önce mevcut grafiği temizleyin
            zedGraphYearly.GraphPane.CurveList.Clear();

            // Yeni verileri alın
            PointPairList updatedData = aidatBL.GetYearlyIncomeGraphData();


            // Yeni verilerle grafiği güncelleyin
            ZedGraph.BarItem barYearly = zedGraphYearly.GraphPane.AddBar("Aidat Geliri", updatedData, System.Drawing.Color.Green);

            // Yeniden çizim işlemlerini gerçekleştirin
            zedGraphYearly.AxisChange();
            zedGraphYearly.Invalidate();
        }
        private void CreateMonthlyGraph()
        {
            // Aylık gelir verilerini al
            PointPairList monthlyIncomeData = aidatBL.GetMonthlyIncomeGraphData();

            // ZedGraphControl oluştur

            zedGraphMonthly.Dock = DockStyle.Fill;
            tabPageMonthly.Controls.Add(zedGraphMonthly);

            // Grafiği oluştur
            ZedGraph.GraphPane graphPaneMonthly = zedGraphMonthly.GraphPane;
            graphPaneMonthly.Title.Text = "Aylık Aidat Gelirleri";
            graphPaneMonthly.XAxis.Title.Text = "Tarih";
            graphPaneMonthly.YAxis.Title.Text = "Aidat Geliri";

            // Grafiği doldur
            ZedGraph.BarItem barMonthly = graphPaneMonthly.AddBar("Aidat Geliri", monthlyIncomeData, System.Drawing.Color.Green);

            // Tarih eksenine özel etiketler ekleyin (isteğe bağlı)
            graphPaneMonthly.XAxis.Scale.Format = "MM/yyyy";
            graphPaneMonthly.XAxis.Type = ZedGraph.AxisType.Date;

            // Grafiği güncelle
            zedGraphMonthly.AxisChange();
            zedGraphMonthly.Invalidate();

        }
        private void CreateYearlyGraph()
        {
            // Yıllık aidat gelirlerini al
            PointPairList yearlyIncomeData = aidatBL.GetYearlyIncomeGraphData();

            // ZedGraphControl oluştur

            zedGraphYearly.Dock = DockStyle.Fill;
            tabPageYearly.Controls.Add(zedGraphYearly);

            // Grafiği oluştur
            ZedGraph.GraphPane graphPaneYearly = zedGraphYearly.GraphPane;
            graphPaneYearly.Title.Text = "Yıllık Aidat Gelirleri";
            graphPaneYearly.XAxis.Title.Text = "Yıllar";
            graphPaneYearly.YAxis.Title.Text = "Aidat Geliri";

            // Grafiği doldur
            ZedGraph.BarItem barYearly = graphPaneYearly.AddBar("Aidat Geliri", yearlyIncomeData, System.Drawing.Color.Green);

            // Tarih eksenine özel etiketler ekleyin (isteğe bağlı)
            graphPaneYearly.XAxis.Scale.Format = "yyyy";
            graphPaneYearly.XAxis.Type = ZedGraph.AxisType.Date;

            // Grafiği güncelle
            zedGraphYearly.AxisChange();
            zedGraphYearly.Invalidate();
        }
        private void InitializeTabs()
        {
            tabPageMonthly = new TabPage("Aylık Grafik");
            tabPageYearly = new TabPage("Yıllık Grafik");

            tabControl1.TabPages.Add(tabPageMonthly);
            tabControl1.TabPages.Add(tabPageYearly);
        }
    }
}
