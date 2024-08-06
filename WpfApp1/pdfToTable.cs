using System;
using System.Collections.Generic;
using System.Data;
using Spire.Pdf;
using Spire.Pdf.Texts;
using Spire.Pdf.Utilities;

namespace WpfApp1
{
    internal class pdfToTable
    {
        public static Tuple<DataSet, Dictionary<string, string>> ReadPdf(string fileName, DataSet dataSet, Dictionary<string, string> dictionary)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);
            doc = RemoveWatermark(doc);

            string text = ExtractText(doc);
            string[] lineParts = text.Split('\n');

            if (lineParts != null)
            {
                foreach (string line in lineParts)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();
                        dictionary.Add(key, value);
                    }
                }
            }

            dataSet = ExtractPdfTable(doc);
            doc.SaveToFile("ReplaceTextInPage.pdf");
            doc.Dispose();

            return Tuple.Create(dataSet, dictionary);
        }
        private static PdfDocument RemoveWatermark(PdfDocument doc)
        {
            PdfTextReplaceOptions textReplaceOptions = new PdfTextReplaceOptions();
            textReplaceOptions.ReplaceType = PdfTextReplaceOptions.ReplaceActionType.IgnoreCase;
            textReplaceOptions.ReplaceType = PdfTextReplaceOptions.ReplaceActionType.WholeWord;
            textReplaceOptions.ReplaceType = PdfTextReplaceOptions.ReplaceActionType.AutofitWidth;

            foreach (PdfPageBase page in doc.Pages)
            {
                PdfTextReplacer textReplacer = new PdfTextReplacer(page);
                textReplacer.ReplaceAllText("Трутнева Кристина Алексеевна - 5901 - 5901-00-751@regions.tax.nalog.ru", "0000");
                textReplacer.ReplaceAllText("Шаманьков Кирилл Анатольевич - 7447 - 7447-00-565@regions.tax.nalog.ru", "0000");
                
            }

            return doc;
        }
        private static string ExtractText(PdfDocument doc)
        {
            string text = "";

            foreach (PdfPageBase page in doc.Pages)
            {
                PdfTextExtractor textExtractor = new PdfTextExtractor(page);
                PdfTextExtractOptions extractOptions = new PdfTextExtractOptions();
                extractOptions.IsExtractAllText = true;

                if (text == "")
                {
                    text = textExtractor.ExtractText(extractOptions);
                }
                else
                {
                    text = text + '\n' + textExtractor.ExtractText(extractOptions);
                }
            }

            return text;
        }
        private static DataSet ExtractPdfTable(PdfDocument doc)
        {
            DataSet ds = new DataSet();
            PdfTableExtractor extractor = new PdfTableExtractor(doc);
            int tableCount = 0;

            for (int pageIndex = 0; pageIndex < doc.Pages.Count; pageIndex++)

            {
                Console.WriteLine($"{pageIndex} {doc.Pages.Count}");
                PdfTable[] tableList = extractor.ExtractTable(pageIndex);

                if (tableList != null && tableList.Length > 0)
                {
                    foreach (PdfTable table in tableList)
                    {
                        DataTable dt = new DataTable();
                        dt.TableName = $"table{tableCount}";

                        int row = table.GetRowCount();
                        int column = table.GetColumnCount();

                        for (int i = 0; i < row; i++)
                        {
                            for (int j = 0; j < column; j++)
                            {
                                string text = table.GetText(i, j).Replace(Environment.NewLine, " ");

                                if (i == 0)
                                {
                                    DataColumn dataColumn = new DataColumn();
                                    while (dt.Columns.Contains(text))
                                    {
                                        text = text + "^";
                                    }
                                    dataColumn.ColumnName = text;
                                    dt.Columns.Add(dataColumn);
                                }
                                else
                                {
                                    if (dt.Rows.Count - 1 < i)
                                    {
                                        dt.Rows.Add();
                                    }
                                    dt.Rows[i - 1][j] = text;
                                }
                            }
                        }
                        ds.Tables.Add(dt);
                        tableCount++;
                    }
                }
            }

            return ds;
        }
    }
}