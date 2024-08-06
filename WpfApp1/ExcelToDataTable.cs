using System;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1
{
    internal class ExcelToDataTable

    {
        public DataTable ReadExcel(string filePath)
        {
            DataTable dt = new DataTable();
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            for (int i = 1; i <= xlRange.Columns.Count; i++)
            {
                dt.Columns.Add(xlRange.Cells[1, i].Value2.ToString());
            }
            // Чтение данных
            for (int i = 2; i <= xlRange.Rows.Count; i++)
            {
                DataRow row = dt.NewRow();
                for (int j = 1; j <= xlRange.Columns.Count; j++)
                {
                    if (xlRange.Cells[i, j].Value2 == null)
                    {
                        break;
                    }
                    row[j - 1] = xlRange.Cells[i, j].Value2;
                }
                dt.Rows.Add(row);
            }
            // Освобождение ресурсов
            xlWorkbook.Close();
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            return dt;
        }
    }
}
