using Spire.Pdf;
using Spire.Pdf.Texts;
using Spire.Pdf.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace WpfApp1
{
    internal class pdfToTable
    {
        public static Tuple<DataSet, Dictionary<string, string>> ReadPdf(string fileName, DataSet dataSet, Dictionary<string, string> dictionary)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);
            doc = RemoveWatermark(doc);

            string directoryPath = $@"{Environment.CurrentDirectory}\tempPDfFolder";
            string masterFilePath = $@"{Environment.CurrentDirectory}\tempMasterPDfFolder";
            string filePatn = Path.Combine(masterFilePath, "ReplaceTextInPage.pdf");
            if (!Directory.Exists(masterFilePath)) { Directory.CreateDirectory(masterFilePath); }
            if (!Directory.Exists(directoryPath)) { Directory.CreateDirectory(directoryPath); }
            doc.SaveToFile(filePatn);
            doc.Dispose();

            splitPdf splitPdf = new splitPdf();
            splitPdf.SplitPdf(filePatn, directoryPath);
            int tableCount = 0;
            foreach (var file in Directory.GetFiles(directoryPath, "*.pdf"))
            {
                doc.LoadFromFile(file);
                string text = ExtractText(doc);
                string[] lineParts = text.Split('\n');
                if (lineParts != null)
                {
                    foreach (string line in lineParts)
                    {
                        string[] parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim().ToLower().Replace(" ", string.Empty);
                            string value = parts[1].Trim();
                            dictionary.Add(key, value);
                        }
                    }
                }
                foreach (DataTable dataTable in ExtractPdfTable(doc, dataSet.Tables.Count).Tables)
                {
                    if (!dataSet.Tables.Contains(dataTable.TableName.ToString()))
                    {
                        DataTable copiedTable = dataTable.Copy();
                        dataSet.Tables.Add(copiedTable);
                    }
                }
            }
            DataSet dataSetCompareTable = compareTable(dataSet);
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
            string text = string.Empty;

            foreach (PdfPageBase page in doc.Pages)
            {
                PdfTextExtractor textExtractor = new PdfTextExtractor(page);
                PdfTextExtractOptions extractOptions = new PdfTextExtractOptions();
                extractOptions.IsExtractAllText = true;

                if (text == string.Empty)
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
        private static DataSet ExtractPdfTable(PdfDocument doc, int tableCount)
        {
            DataSet ds = new DataSet();
            PdfTableExtractor extractor = new PdfTableExtractor(doc);


            for (int pageIndex = 0; pageIndex < doc.Pages.Count; pageIndex++)

            {
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
                        bool zeroValue = true;
                        if (dt.Rows.Count - 1 != -1)
                        {
                            DataRow longRow = dt.Rows[dt.Rows.Count - 1];

                            foreach (DataColumn dataColumn in longRow.Table.Columns)
                            {
                                if (longRow[dataColumn].ToString().Length > 0)
                                {
                                    zeroValue = false;
                                    break;
                                }
                            }
                        }
                        if (zeroValue && dt.Rows.Count - 1 != -1) { dt.Rows.RemoveAt(dt.Rows.Count - 1); }
                        ds.Tables.Add(dt);
                        tableCount++; 
                    }
                }
            }

            return ds;
        }
        private static DataSet compareTable(DataSet ds)
        {
            for (int tablesCount = ds.Tables.Count - 1; tablesCount >= 0; tablesCount--)
            {
                if (tablesCount > 0)
                {
                    string nameCurrentTable = $"table{tablesCount}";
                    string nameOldTable = $"table{tablesCount - 1}";
                    Console.Write($"{nameCurrentTable} - {nameOldTable}");
                    if (ds.Tables[nameCurrentTable].Columns.Count == ds.Tables[nameOldTable].Columns.Count) //если совпадают количество колонк
                    {
                        if (ds.Tables[nameCurrentTable].Rows.Count - 1 > -1 && ds.Tables[nameOldTable].Rows.Count - 1 > -1)// проверка на наличие строк в таблицах
                        {
                            DataRow firstRow = ds.Tables[nameCurrentTable].Rows[0];
                            string firstRowString = checkValueType(firstRow);
                            DataRow longRow = ds.Tables[nameOldTable].Rows[ds.Tables[nameOldTable].Rows.Count - 1];
                            string longRowString = checkValueType(longRow);
                            if (longRowString == firstRowString) // проверка совпадения типов данных
                            {
                                Console.WriteLine($"триггер - 3");
                                var headerRow = new object[ds.Tables[nameCurrentTable].Columns.Count];
                                for (int col = 0; col < ds.Tables[nameCurrentTable].Columns.Count; col++)
                                {
                                    headerRow[col] = ds.Tables[nameCurrentTable].Columns[col].ColumnName;
                                }
                                ds.Tables[nameOldTable].Rows.Add(headerRow);

                                foreach (DataRow row in ds.Tables[nameCurrentTable].Rows)
                                {
                                    ds.Tables[nameOldTable].Rows.Add(row.ItemArray);
                                }
                            }
                            else { Console.WriteLine($"{longRowString} - {firstRowString}"); }
                        }
                        else if (ds.Tables[nameCurrentTable].Rows.Count - 1 == -1 || ds.Tables[nameOldTable].Rows.Count - 1 == -1)
                        {
                            Console.WriteLine($"шапка строка");
                            var headerRow = new object[ds.Tables[nameCurrentTable].Columns.Count];
                            for (int col = 0; col < ds.Tables[nameCurrentTable].Columns.Count; col++)
                            {
                                headerRow[col] = ds.Tables[nameCurrentTable].Columns[col].ColumnName;
                            }
                            ds.Tables[nameOldTable].Rows.Add(headerRow);

                            foreach (DataRow row in ds.Tables[nameCurrentTable].Rows)
                            {
                                ds.Tables[nameOldTable].Rows.Add(row.ItemArray);
                            }
                        }

                    }
                }
            }
            foreach (DataTable dataTable in ds.Tables)
            {


                string nameTable = renameTable(dataTable);
                if (nameTable != string.Empty)
                {
                    int countTable = 1;
                    string newNameTable = nameTable;

                    if (ds.Tables.Contains(newNameTable))
                    {
                        while (ds.Tables.Contains(newNameTable))
                        {
                            newNameTable = $"{nameTable}_{countTable}";
                            countTable++;
                        }
                    }
                    dataTable.TableName = newNameTable;
                }
            }

            return ds;
        }
        private static string renameTable(DataTable dataTable1)
        {
            string tableName = string.Empty;
            string collumsRows = string.Empty;
            foreach (DataColumn column in dataTable1.Columns)
            {
                if (collumsRows == string.Empty) { collumsRows = column.ColumnName; } else { collumsRows = $"{collumsRows}{column.ColumnName}"; }

            }

            collumsRows = collumsRows.Replace(" ", string.Empty).Replace("\n", string.Empty);
            if (collumsRows == "АдресДатаначалаДатаокончания") { tableName = "Адрес (место нахождения) организации"; }
            else if (collumsRows == "ФИО/УправляющаякомпанияИННДатарожденияДатаначалаДатаокончанияДатаначаладисквалификацииДатаокончаниядисквалификации") { tableName = "Руководитель"; }
            else if (collumsRows == "ОКВЭДРасшифровкаДатаначаладействия") { tableName = "ОКВЭД"; }
            else if (collumsRows == "ТипучредителяФИО/НаименованиеИННДоляучастияДатасозданияДатазакрытия") { tableName = "Учредитель"; }
            else if (collumsRows == "НаименованиеИННАдрескомпанииРольвкомпанииДоляучастия,руб.ДатаначаладействияДатаокончаниядействия") { tableName = "Сведения об участии юридического лица в российских и иностранных компаниях"; }
            else if (collumsRows == "ДатапостановкинаучётДатаснятиясучётаНалоговыйорган") { tableName = "История изменений сведений о постановке на учет в НО"; }
            else if (collumsRows == "адресIPКол-воНП,сдававшихотчетностьсданногоIP-адреса") { tableName = "Сведения об IP – адресах"; }
            else if (collumsRows == "ТелефонКоличествоНП,указавшихданныйномертелефона") { tableName = "Сведения о телефонных номерах"; }
            else if (collumsRows == "НаименованиеналоговогоагентаИННналоговогоагентаПериодполучениядоходаСуммаполученногодохода,тыс.руб.") { tableName = "Данные о численности сотрудников по справкам 2-НДФЛ"; }
            else if (collumsRows == "НомерККТАдресрегистрацииККТНаименованиеместаустановкиДатарегистрацииККТДатаснятияККТ") { tableName = "ККТ"; }
            else if (collumsRows == "НаименованиеКППАдресДатасозданияДатазакрытия") { tableName = "Филиалы, представительства, иные обособленные подразделения"; }
            else if (collumsRows == "Наименованиепоказателя2021202220232024") { tableName = "Показатели НБО"; }
            else if (collumsRows == "НалогиНачислено,тыс.руб.Уменьшено,тыс.руб.Уплачено,тыс.руб.") { tableName = "Начисления/Уплата по налогам"; }
            else if (collumsRows == "ПериодСуммавычетовпоНДС(тыс.руб.)СуммаНДСпореализации(тыс.руб.)СуммаНДС(журналвыставленныхсчетов-фактур),тыс.руб.СуммаНДС(журналполученныхсчетов-фактур),тыс.руб.ДоляНДСкуплате/квозмещению") { tableName = "Сработавшие риски по данному НП"; }
            else if (collumsRows == "НомерсчетаБанкВалютаДатаоткрытияДатазакрытияИнформацияодействующихрешенияхОбщаясуммаоперацийподебету,рубОбщаясуммаоперацийпокредиту,руб.") { tableName = "Счета"; }
            else if (collumsRows == "КодПоказатель2021202220232024") { tableName = "Сведения об основных признаках Однодневок и технических компаний"; }
            else if (collumsRows == "2021Column1Column2Column32022Column4Column5Column62023Column7Column8Column92024Column10Column11Column12") { tableName = "СУР"; }
            else if (collumsRows == "ПериодРегистрационныйномерНалоговаядекларацияпоналогунаприбыльорганизацийНалоговаядекларацияпоналогу,уплачиваемомувсвязисприменениемупрощеннойсистемыналогообложенияНалоговаядекларацияпоединомусельскохозяйственномуналогуБухгалтерская(финансовая)отчетность") { tableName = "Импорт_Экспорт"; }
            else if (collumsRows == "НаименованиеконтрагентаИННконтрагентаКритериивзаимосвязи") { tableName = "Взаимосвязи"; }
            else if (collumsRows == "ТипдокументаНомердокументаДатадокумента")
            {
                int dopros = 0; int istreb = 0;
                foreach (DataRow row in dataTable1.Rows)
                {
                    string rowString = row[0].ToString().Trim().ToLower().Replace(" ", string.Empty).Replace("\n", string.Empty);
                    if (rowString == "требованиеопредставлениидокументов(информации)") { istreb += 1; }
                    else if (rowString == "поручениеобистребованиидокументов(информации)") { istreb += 1; }
                    else if (rowString == "сопроводительноеписьмоонаправлениидокументов,истребованныхпопоручению") { istreb += 1; }
                    else if (rowString == "повесткаовызовенадопроссвидетеля") { dopros += 1; }
                    else if (rowString == "уведомлениеоневозможностидопросасвидетеляипринятыхмерах") { dopros += 1; }
                    else if (rowString == "протоколдопросасвидетеля") { dopros += 1; }
                }
                if (istreb > dopros) { tableName = "Истребование"; }
                else if (istreb < dopros) { tableName = "Допросы"; }
                else { tableName = "Иное"; }

            }





            else
            {
                Console.WriteLine($"{dataTable1.TableName} {collumsRows}");
            }
            return tableName;
        }

        private static string checkValueType(DataRow firstRow)
        {
            string typesString = string.Empty;
            foreach (DataColumn column in firstRow.Table.Columns)
            {

                DateTime date;
                string cellValue = firstRow[column].ToString();
                string typeName = string.Empty;

                typeName = "string";
                if (int.TryParse(cellValue, out _))
                {
                    typeName = "int";
                }
                else if (double.TryParse(cellValue, out _))
                {
                    typeName = "double";
                }
                else if (DateTime.TryParse(cellValue, out _))
                {
                    typeName = "DateTime";
                }
                else if (long.TryParse(cellValue, out _))
                {
                    typeName = "long";
                }

                typesString += string.IsNullOrEmpty(typesString) ? typeName : "-" + typeName;
            }
            return typesString;
        }
    }


}