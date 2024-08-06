using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DGV1Data> DGV1DataList { get; set; }
        public ObservableCollection<string> DGV1Options { get; set; }
        public ObservableCollection<DGV2Data> DGV2DataList { get; set; }
        public ObservableCollection<string> DGV2Options { get; set; }
        public ObservableCollection<DGV3Data> DGV3DataList { get; set; }
        public ObservableCollection<string> DGV3Options { get; set; }
        private DataTable JobExcel = new DataTable();
        private string settingsDirectoryPath = Path.Combine(Environment.CurrentDirectory, "mySettings");
        private string pdfCollectorsPath = Path.Combine(Environment.CurrentDirectory, "pdfCollectors");
        private Dictionary<string, DataSet> masterDataset = new Dictionary<string, DataSet>();
        private Dictionary<string, Dictionary<string, string>> masterDictionary = new Dictionary<string, Dictionary<string, string>>();


        public MainWindow()
        {
            InitializeComponent();

            if (!Directory.Exists(settingsDirectoryPath))
            {
                Directory.CreateDirectory(settingsDirectoryPath);
            }
            createDGV();
            helperText(0, "Нет данных для отображения, загрузите цепочку из файла Excel");
            helperText(1, "Нет данных для отображения, загрузите цепочку из файла Excel");
            helperText(2, "Нет данных для отображения, загрузите цепочку из файла Excel");
        }
        private void createDGV()
        {

            DGV1DataList = new ObservableCollection<DGV1Data>
            {
                new DGV1Data { IsChecked = false, Text = "Применяемые налоговые режимы", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Данные о численности сотрудников по справкам 2-НДФЛ", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Данные о численности сотрудников, работающих более года", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Лицензии", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Филиалы", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Количество ККТ", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Сведения о банкротстве", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Сведения об IP - адресах", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Сведения о телефонных номерах", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Участие НП в схемах уклонения", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Показатели НБО", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Сведения об основных признаках Однодневок и технических компаний", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Недвижимое имущество", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Сведения о транспортных средствах", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Сведения о земельной собственности", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Истребование", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Вне рамок", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Иные мероприятия", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Допрос", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Поручение", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Повестка", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Протокол", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Иное", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "История изменений руководителей", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "История изменений учредителей", SelectedOption = "Все" },
                new DGV1Data { IsChecked = true, Text = "Руководитель, учредитель квартал", SelectedOption = "Строка" },
                new DGV1Data { IsChecked = true, Text = "Руководитель, учредитель за год", SelectedOption = "Строка" }
            };

            DGV1Options = new ObservableCollection<string>
            {
                "Таблица",
                "Строка",
                "Все"
            };

            DGV2DataList = new ObservableCollection<DGV2Data>
            {
                new DGV2Data { IsChecked = false, Text = "Применяемые налоговые режимы", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Данные о численности сотрудников по справкам 2-НДФЛ", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Данные о численности сотрудников, работающих более года", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Лицензии", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Филиалы", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Количество ККТ", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Сведения о банкротстве", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Сведения об IP - адресах", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Сведения о телефонных номерах", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Участие НП в схемах уклонения", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Показатели НБО", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Сведения об основных признаках Однодневок и технических компаний", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Недвижимое имущество", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Сведения о транспортных средствах", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Сведения о земельной собственности", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Истребование", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Вне рамок", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Иные мероприятия", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Допрос", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Поручение", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Повестка", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Протокол", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Иное", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "История изменений руководителей", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "История изменений учредителей", SelectedOption = "Все" },
                new DGV2Data { IsChecked = true, Text = "Руководитель, учредитель квартал", SelectedOption = "Строка" },
                new DGV2Data { IsChecked = true, Text = "Руководитель, учредитель за год", SelectedOption = "Строка" }
            };

            DGV2Options = new ObservableCollection<string>
            {
                "Таблица",
                "Строка",
                "Все"
            };
            DGV3DataList = new ObservableCollection<DGV3Data>
            {
                new DGV3Data { IsChecked = false, Text = "Применяемые налоговые режимы", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Данные о численности сотрудников по справкам 2-НДФЛ", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Данные о численности сотрудников, работающих более года", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Лицензии", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Филиалы", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Количество ККТ", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Сведения о банкротстве", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Сведения об IP - адресах", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Сведения о телефонных номерах", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Участие НП в схемах уклонения", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Показатели НБО", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Сведения об основных признаках Однодневок и технических компаний", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Недвижимое имущество", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Сведения о транспортных средствах", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Сведения о земельной собственности", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Истребование", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Вне рамок", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Иные мероприятия", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Допрос", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Поручение", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Повестка", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Протокол", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Иное", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "История изменений руководителей", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "История изменений учредителей", SelectedOption = "Все" },
                new DGV3Data { IsChecked = true, Text = "Руководитель, учредитель квартал", SelectedOption = "Строка" },
                new DGV3Data { IsChecked = true, Text = "Руководитель, учредитель за год", SelectedOption = "Строка" }
            };

            DGV3Options = new ObservableCollection<string>
            {
                "Таблица",
                "Строка",
                "Все"
            };

            DGV1.ItemsSource = DGV1DataList;
            DGV2.ItemsSource = DGV2DataList;
            DGV3.ItemsSource = DGV3DataList;
            DataContext = this;


        }
        private void helperText(int number, string text)
        {
            if (number == 0)
            {
                Dispatcher.Invoke(() =>
                    sourceHelper.Content = text
                    );
            }
            else if (number == 1)
            {
                Dispatcher.Invoke(() =>
                       beneficiaryHelper.Content = text
                       );
            }
            else if (number == 2)
            {
                Dispatcher.Invoke(() =>
                       transitHelper.Content = text
                       );
            }
        }
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.CurrentDirectory.ToString();
            dialog.Filter = "Excel Files|*.xlsx";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                JobExcel.Clear();
                ExcelToDataTable ExcelToDataTable = new ExcelToDataTable();
                JobExcel.Merge(ExcelToDataTable.ReadExcel(dialog.FileName));
                createJobTable(JobExcel);
            }


        }
        private void createDGV1(DataTable dt)
        {
            DataTable DGV1table = dt.Clone();
            DGV1table.Columns.Add("СТАТУС");
            foreach (var item in DGV1DataList)
            {
                if (item.IsChecked)
                {
                    DGV1table.Columns.Add(item.Text);
                }
            }
            int nullCount = 0;
            int trueCount = 0;
            foreach (DataRow row in dt.Rows)
            {
                string value = row["ВИД"].ToString().ToLower();

                if (value.StartsWith("ин"))
                {
                    DataRow dr = DGV1table.NewRow();
                    dr["ВИД"] = row["ВИД"].ToString();
                    dr["ТИП"] = row["ТИП"].ToString();
                    dr["ДАННЫЕ"] = row["ДАННЫЕ"].ToString();
                    dr["ПЕРИОД"] = row["ПЕРИОД"].ToString();
                    dr["ЦЕПОЧКА"] = row["ЦЕПОЧКА"].ToString();
                    if (!File.Exists($@"{pdfCollectorsPath}\{row["ДАННЫЕ"].ToString()}_{row["ТИП"].ToString()}.pdf"))
                    { dr["СТАТУС"] = "-"; nullCount++; }
                    else { dr["СТАТУС"] = "+"; trueCount++; }
                    DGV1table.Rows.Add(dr);
                }
            }
            Datagridsource.ItemsSource = DGV1table.DefaultView;
            Dispatcher.Invoke(() => sourceHelper.Content = $"Список критериев для источника обновлен! Необходимо собрать {nullCount} документов, собрано {trueCount}");
        }
        private void createDGV2(DataTable dt)
        {
            DataTable DGV2table = dt.Clone();
            DGV2table.Columns.Add("СТАТУС");
            foreach (var item in DGV2DataList)
            {
                if (item.IsChecked)
                {
                    DGV2table.Columns.Add(item.Text);
                }
            }
            int nullCount = 0;
            int trueCount = 0;
            foreach (DataRow row in dt.Rows)
            {
                string value = row["ВИД"].ToString().ToLower();

                if (value.StartsWith("вп"))
                {
                    DataRow dr = DGV2table.NewRow();
                    dr["ВИД"] = row["ВИД"].ToString();
                    dr["ТИП"] = row["ТИП"].ToString();
                    dr["ДАННЫЕ"] = row["ДАННЫЕ"].ToString();
                    dr["ПЕРИОД"] = row["ПЕРИОД"].ToString();
                    dr["ЦЕПОЧКА"] = row["ЦЕПОЧКА"].ToString();
                    if (!File.Exists($@"{pdfCollectorsPath}\{row["ДАННЫЕ"].ToString()}_{row["ТИП"].ToString()}.pdf"))
                    { dr["СТАТУС"] = "-"; nullCount++; }
                    else { dr["СТАТУС"] = "+"; trueCount++; }
                    DGV2table.Rows.Add(dr);
                }
            }
            Datagridbeneficiary.ItemsSource = DGV2table.DefaultView;
            Dispatcher.Invoke(() => beneficiaryHelper.Content = $"Список критериев для выгодоприобретателей обновлен! Необходимо собрать {nullCount} документов, собрано {trueCount}");
        }
        private void createDGV3(DataTable dt)
        {
            DataTable DGV3table = dt.Clone();
            DGV3table.Columns.Add("СТАТУС");
            foreach (var item in DGV2DataList)
            {
                if (item.IsChecked)
                {
                    DGV3table.Columns.Add(item.Text);
                }
            }
            int nullCount = 0;
            int trueCount = 0;
            foreach (DataRow row in dt.Rows)
            {
                string value = row["ВИД"].ToString().ToLower();

                if (value.StartsWith("тр"))
                {
                    DataRow dr = DGV3table.NewRow();
                    dr["ВИД"] = row["ВИД"].ToString();
                    dr["ТИП"] = row["ТИП"].ToString();
                    dr["ДАННЫЕ"] = row["ДАННЫЕ"].ToString();
                    dr["ПЕРИОД"] = row["ПЕРИОД"].ToString();
                    dr["ЦЕПОЧКА"] = row["ЦЕПОЧКА"].ToString();
                    
                    if (!File.Exists($@"{pdfCollectorsPath}\{row["ДАННЫЕ"].ToString()}_{row["ТИП"].ToString()}.pdf"))
                    { dr["СТАТУС"] = "-"; nullCount++; }
                    else { dr["СТАТУС"] = "+"; trueCount++; }
                    DGV3table.Rows.Add(dr);
                }
            }
            Datagridtransit.ItemsSource = DGV3table.DefaultView;
            Dispatcher.Invoke(() => transitHelper.Content = $"Список критериев для транзитеров обновлен! Необходимо собрать {nullCount} документов, собрано {trueCount}");
        }
        private void createJobTable(DataTable dt) { createDGV1(dt); createDGV2(dt); createDGV3(dt); }
        private void createDoc_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("--------------DV1--------------");

            foreach (var item in DGV1DataList)
            {
                Debug.WriteLine($"IsChecked: {item.IsChecked}, Text: {item.Text}, SelectedOption: {item.SelectedOption}");
            }

            Debug.WriteLine("--------------DV2--------------");
            foreach (var item in DGV2DataList)
            {
                Debug.WriteLine($"IsChecked: {item.IsChecked}, Text: {item.Text}, SelectedOption: {item.SelectedOption}");
            }

            Debug.WriteLine("--------------DV3--------------");

            foreach (var item in DGV3DataList)
            {
                Debug.WriteLine($"IsChecked: {item.IsChecked}, Text: {item.Text}, SelectedOption: {item.SelectedOption}");
            }


        }
        #region "save&load json"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(DGV1DataList, Formatting.Indented);
            File.WriteAllText($@"{settingsDirectoryPath}\DGV1Data.json", json);
            MessageBox.Show("Сохранение DGV1 выполнено.");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (File.Exists($@"{settingsDirectoryPath}\DGV1Data.json"))
            {
                string json = File.ReadAllText($@"{settingsDirectoryPath}\DGV1Data.json");
                DGV1DataList = JsonConvert.DeserializeObject<ObservableCollection<DGV1Data>>(json);
                DGV1.ItemsSource = DGV1DataList;
                MessageBox.Show("Загрузка DGV1 выполнена.");
            }
            else
            {
                MessageBox.Show("Файл DGV1Data.json не найден.");
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(DGV2DataList, Formatting.Indented);
            File.WriteAllText($@"{settingsDirectoryPath}\DGV2Data.json", json);
            MessageBox.Show("Сохранение DGV2 выполнено.");
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (File.Exists($@"{settingsDirectoryPath}\DGV2Data.json"))
            {
                string json = File.ReadAllText($@"{settingsDirectoryPath}\DGV2Data.json");
                DGV2DataList = JsonConvert.DeserializeObject<ObservableCollection<DGV2Data>>(json);
                DGV2.ItemsSource = DGV2DataList;
                MessageBox.Show("Загрузка DGV2 выполнена.");
            }
            else
            {
                MessageBox.Show("Файл DGV2Data.json не найден.");
            }
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(DGV3DataList, Formatting.Indented);
            File.WriteAllText($@"{settingsDirectoryPath}\DGV3Data.json", json);
            MessageBox.Show("Сохранение DGV3 выполнено.");
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (File.Exists($@"{settingsDirectoryPath}\DGV3Data.json"))
            {
                string json = File.ReadAllText($@"{settingsDirectoryPath}\DGV3Data.json");
                DGV3DataList = JsonConvert.DeserializeObject<ObservableCollection<DGV3Data>>(json);
                DGV3.ItemsSource = DGV3DataList;
                MessageBox.Show("Загрузка DGV3 выполнена.");
            }
            else
            {
                MessageBox.Show("Файл DGV3Data.json не найден.");
            }
        }
        #endregion
        #region "Обновление списков таблиц по условиям"
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (JobExcel != null && JobExcel.Rows.Count > 0) { createDGV1(JobExcel); }
            else { Dispatcher.Invoke(() => sourceHelper.Content = "Для обновления критериев загрузите файл цепочек"); }
        }
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (JobExcel != null && JobExcel.Rows.Count > 0) { createDGV2(JobExcel); }
            else { Dispatcher.Invoke(() => beneficiaryHelper.Content = "Для обновления критериев загрузите файл цепочек"); }

        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (JobExcel != null && JobExcel.Rows.Count > 0) { createDGV3(JobExcel); }
            else { Dispatcher.Invoke(() => transitHelper.Content = "Для обновления критериев загрузите файл цепочек"); }

        }
        #endregion
        #region "pdfToTable"
              private DataTable datagridToDatatabel(DataGrid dg)
        {
            DataTable dataTable = new DataTable();
            foreach (var column in dg.Columns)
            {
                if (!dataTable.Columns.Contains(column.Header.ToString())) { dataTable.Columns.Add(column.Header.ToString()); }
            }

            foreach (var item in dg.Items)
            {
                DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    DataRow dataRow = dataTable.NewRow();
                    foreach (var column in dg.Columns)
                    {
                        if (column.GetCellContent(row) is TextBlock cellContent)
                        {
                            dataRow[column.Header.ToString()] = cellContent.Text;
                        }
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }
        #endregion
        #region "Создание файлов задания"
        Dictionary<string, string> jobToPdfFileForTable(DataGrid dg, string iniFilePath)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("result", "error");
            keyValuePairs.Add("value", "");
            try
            {
                IniFile iniFile = new IniFile(iniFilePath);
                string innlist = "";
                DataTable dataTable = datagridToDatatabel(dg);

                if (dataTable != null)
                {
                    if (dataTable.Columns.Contains("СТАТУС") && dataTable.Columns.Contains("ДАННЫЕ"))
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (row["СТАТУС"].ToString() == "-")
                            {
                                if (innlist == "")
                                {
                                    innlist = row["ДАННЫЕ"].ToString();
                                }
                                else
                                {
                                    innlist = $"{innlist}/{row["ДАННЫЕ"].ToString()}";
                                }
                            }
                        }

                        iniFile.WriteValue("main", "inn", innlist);
                        keyValuePairs["result"] = "ok";
                        keyValuePairs["value"] = innlist;
                        return keyValuePairs;
                    }
                    else
                    {
                        keyValuePairs["value"] = "Обязательные столбцы не найдены в DataTable."; throw new Exception("Обязательные столбцы не найдены в DataTable.");
                    }
                }
                else
                {
                    keyValuePairs["value"] = "Не удалось преобразовать DataGrid в DataTable."; throw new Exception("Не удалось преобразовать DataGrid в DataTable.");
                }
            }
            catch (Exception ex)
            {
                keyValuePairs["value"] = ex.Message;
                keyValuePairs["result"] = "error";
                return keyValuePairs;
            }
        }
        private void Button_Click_9(object sender, RoutedEventArgs e)

        {
            string iniFilePath = $@"{Environment.CurrentDirectory}\ini\source.ini";
            if (!Directory.Exists($@"{Environment.CurrentDirectory}\ini"))
            {
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\ini");
            }
            if (!File.Exists(iniFilePath))
            {
                File.Create(iniFilePath);
            }

            Dictionary <string,string> result = jobToPdfFileForTable(Datagridsource, iniFilePath);
            if (result["result"] == "ok") { if (result["value"] != "") { helperText(0, "Создание файла задания завершено");  buttonSource.Visibility = Visibility.Visible; } else { helperText(0, "Нет условий для создания файла задания"); buttonSource.Visibility = Visibility.Collapsed; } } else { helperText(0, result["value"]); }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            string iniFilePath = $@"{Environment.CurrentDirectory}\ini\beneficiary.ini";
            if (!Directory.Exists($@"{Environment.CurrentDirectory}\ini"))
            {
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\ini");
            }
            if (!File.Exists(iniFilePath))
            {
                File.Create(iniFilePath);
            }
            Dictionary<string, string> result = jobToPdfFileForTable(Datagridbeneficiary, iniFilePath);
            if (result["result"] == "ok") {  if (result["value"] != "") { helperText(1, "Создание файла задания завершено"); buttonBeneficiary.Visibility = Visibility.Visible; } else { helperText(1, "Нет условий для создания файла задания"); buttonBeneficiary.Visibility = Visibility.Collapsed; } } else { helperText(1, result["value"]); }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            string iniFilePath = $@"{Environment.CurrentDirectory}\ini\transit.ini";
            if (!Directory.Exists($@"{Environment.CurrentDirectory}\ini"))
            {
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\ini");
            }
            if (!File.Exists(iniFilePath))
            {
                File.Create(iniFilePath);
            }

            Dictionary<string, string> result = jobToPdfFileForTable(Datagridtransit, iniFilePath);
            if (result["result"] == "ok") { if (result["value"] != "") { helperText(2, "Создание файла задания завершено");  buttonTransit.Visibility = Visibility.Visible; } else { helperText(2, "Нет условий для создания файла задания"); buttonTransit.Visibility = Visibility.Collapsed; } } else { helperText(2, result["value"]); }
        }

        #endregion
        #region "Запуск сборщиков"
        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Запустить ПО для сбора PDF файлов?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Логика при выборе "Да"
            }
            else
            {
                // Логика при выборе "Нет" или закрытии окна
            }
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Запустить ПО для сбора PDF файлов?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Логика при выборе "Да"
            }
            else
            {
                // Логика при выборе "Нет" или закрытии окна
            }
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Запустить ПО для сбора PDF файлов?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Логика при выборе "Да"
            }
            else
            {
                // Логика при выборе "Нет" или закрытии окна
            }
        }
        #endregion

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            DataTable dataTable=datagridToDatatabel(Datagridsource) as DataTable;

            foreach (DataRow row in dataTable.Rows) {
                if (!masterDataset.Keys.Contains(row["ДАННЫЕ"].ToString()) & !masterDictionary.Keys.Contains(row["ДАННЫЕ"].ToString()) & row["СТАТУС"].ToString()=="+")
                {
                    string fileName = $@"{pdfCollectorsPath}\{row["ДАННЫЕ"].ToString()}_{row["ТИП"].ToString()}.pdf";
                    DataSet inputDataSet = new DataSet();
                    Dictionary<string, string> inputDictionary = new Dictionary<string, string>();
                    var result = pdfToTable.ReadPdf(fileName, inputDataSet, inputDictionary);
                    DataSet updatedDataSet = result.Item1;
                    Dictionary<string, string> updatedDictionary = result.Item2;
                    //masterDataset.Add(row["ДАННЫЕ"].ToString(), updatedDataSet);
                   // masterDictionary.Add(row["ДАННЫЕ"].ToString(), updatedDictionary);
                    foreach (DataTable dataTable1 in updatedDataSet.Tables)
                    {                       
                        foreach (DataColumn column in dataTable1.Columns)
                        {
                            column.ColumnName= column.ColumnName.ToString().Replace(Environment.NewLine,string.Empty ).Replace("\n", string.Empty).Replace(" ",string.Empty );

                        }                        
                    }

                    foreach (DataTable dataTable1 in updatedDataSet.Tables)
                    {
                        string collumsRows = "";
                        foreach (DataColumn column in dataTable1.Columns)
                        {
                            if (collumsRows == "") { collumsRows = column.ColumnName; } else { collumsRows = $"{collumsRows}{column.ColumnName}"; }
                           
                        }
                        if (collumsRows == "АдресДатаначалаДатаокончания") { dataTable1.TableName = "История изменения адреса регистрации";}
                        else if (collumsRows == "ФИО/УправляющаякомпанияИННДатарожденияДатаначалаДатаокончанияДатаначаладисквалификацииДатаокончаниядисквалификации") { dataTable1.TableName = "Информация по руководителям"; }
                        else if (collumsRows == "ОКВЭДРасшифровкаДатаначаладействия") { dataTable1.TableName = "ОКВЭД"; }
                        else if (collumsRows == "ТипучредителяФИО/НаименованиеИННДоляучастияДатасозданияДатазакрытия") { dataTable1.TableName = "Учредитель"; }
                        else if (collumsRows == "НаименованиеИННАдрескомпанииРольвкомпанииДоляучастия,руб.ДатаначаладействияДатаокончаниядействия") { dataTable1.TableName = "Сведения об участии юридического лица в российских и иностранных компаниях"; }
                        else if (collumsRows == "ДатапостановкинаучётДатаснятиясучётаНалоговыйорган") { dataTable1.TableName = "История изменений сведений о постановке на учет в НО"; }
                        else if (collumsRows == "адресIPКол-воНП,сдававшихотчетностьсданногоIP-адреса") { dataTable1.TableName = "Сведения об IP – адресах";}

                        Console.WriteLine($"{dataTable1.TableName} {collumsRows}");
                    }
                 
                    Console.WriteLine();
                }
            }
            
        }
    }

    public class DGV1Data : System.ComponentModel.INotifyPropertyChanged
    {
        private bool _isChecked;
        private string _text;
        private string _selectedOption;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                if (_selectedOption != value)
                {
                    _selectedOption = value;
                    OnPropertyChanged(nameof(SelectedOption));
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    public class DGV2Data : System.ComponentModel.INotifyPropertyChanged
    {
        private bool _isChecked;
        private string _text;
        private string _selectedOption;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                if (_selectedOption != value)
                {
                    _selectedOption = value;
                    OnPropertyChanged(nameof(SelectedOption));
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
    public class DGV3Data : System.ComponentModel.INotifyPropertyChanged
    {
        private bool _isChecked;
        private string _text;
        private string _selectedOption;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                if (_selectedOption != value)
                {
                    _selectedOption = value;
                    OnPropertyChanged(nameof(SelectedOption));
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

}
