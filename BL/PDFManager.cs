using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Net.Mail;
using System.Net;

namespace BL
{
    public class PDFManager
    {
        public void CreateDebtorsPDF(DataTable debtors, string filePath)
        {
            try
            {
                using (Document document = new Document())
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create)))
                    {
                        document.Open();
                        PdfPTable pdfTable = new PdfPTable(debtors.Columns.Count);

                        foreach (DataColumn column in debtors.Columns)
                        {
                            pdfTable.AddCell(new PdfPCell(new Phrase(column.ColumnName)));
                        }
                        foreach (DataRow row in debtors.Rows)
                        {
                            foreach (var cell in row.ItemArray)
                            {
                                pdfTable.AddCell(new PdfPCell(new Phrase(cell.ToString())));
                            }
                        }
                        document.Add(pdfTable);
                    }
                }
                Console.WriteLine("PDF başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }
    }
}

