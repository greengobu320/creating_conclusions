Imports System.Net
Imports System.Text
Imports System.Security.Policy
Imports System.Text.RegularExpressions
Imports HtmlAgilityPack
Imports Newtonsoft.Json
Imports System.Web
Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports Excel = Microsoft.Office.Interop.Excel

Imports System.Threading
Imports Newtonsoft.Json.Linq
Imports Spire.Pdf.General


Public Class Form1
#Region "перпеменные логирования"
    Dim logLoc As New Object
    Dim dtLog As New DataTable
#End Region
#Region "Переменные для работы с сайтом"
    Friend courtsDictionary As New Dictionary(Of String, String)
    Friend dateStart As String
    Friend dateEnd As String
    Friend coursString As String
#End Region
#Region "Переменные синхронизации"
    Friend timeWait As Integer
    Private synchronization As Boolean = False
    Private tickCount = 0
#End Region
#Region "переменные SQL"
    Public odbcName As String
#End Region
    Dim Preference As New Preference()
    Dim DownloadDataSite As New DownloadDataSite()
    Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавить код инициализации после вызова InitializeComponent().
        ToolStripStatusLabel1.Text = String.Empty
        ToolStripStatusLabel2.Text = String.Empty
        loadCour()
        dtLog.Columns.Add("date")
        dtLog.Columns.Add("level")
        dtLog.Columns.Add("system")
        dtLog.Columns.Add("message")
    End Sub
    Sub loadCour()
        courtsDictionary.Add("Верховный Суд РФ", "VS")
        courtsDictionary.Add("Высший Арбитражный Суд РФ", "VAS")
        courtsDictionary.Add("АС Волго-Вятского округа", "VASVVO")
        courtsDictionary.Add("АС Восточно-Сибирского округа", "VASVSO")
        courtsDictionary.Add("АС Дальневосточного округа", "FASDVO")
        courtsDictionary.Add("АС Западно-Сибирского округа", "FASZSO")
        courtsDictionary.Add("АС Московского округа", "FASMO")
        courtsDictionary.Add("АС Поволжского округа", "FASPO")
        courtsDictionary.Add("АС Северо-Западного округа", "FASSZO")
        courtsDictionary.Add("АС Северо-Кавказского округа", "FASSKO")
        courtsDictionary.Add("АС Уральского округа", "FASUO")
        courtsDictionary.Add("АС Центрального округа", "FASCO")
        courtsDictionary.Add("1 арбитражный апелляционный суд", "1AAS")
        courtsDictionary.Add("2 арбитражный апелляционный суд", "2AAS")
        courtsDictionary.Add("3 арбитражный апелляционный суд", "3AAS")
        courtsDictionary.Add("4 арбитражный апелляционный суд", "4AAS")
        courtsDictionary.Add("5 арбитражный апелляционный суд", "5AAS")
        courtsDictionary.Add("6 арбитражный апелляционный суд", "6AAS")
        courtsDictionary.Add("7 арбитражный апелляционный суд", "7AAS")
        courtsDictionary.Add("8 арбитражный апелляционный суд", "8AAS")
        courtsDictionary.Add("9 арбитражный апелляционный суд", "9AAS")
        courtsDictionary.Add("10 арбитражный апелляционный суд", "10AAS")
        courtsDictionary.Add("11 арбитражный апелляционный суд", "11AAS")
        courtsDictionary.Add("12 арбитражный апелляционный суд", "12AAS")
        courtsDictionary.Add("13 арбитражный апелляционный суд", "13AAS")
        courtsDictionary.Add("14 арбитражный апелляционный суд", "14AAS")
        courtsDictionary.Add("15 арбитражный апелляционный суд", "15AAS")
        courtsDictionary.Add("16 арбитражный апелляционный суд", "16AAS")
        courtsDictionary.Add("17 арбитражный апелляционный суд", "17AAS")
        courtsDictionary.Add("18 арбитражный апелляционный суд", "18AAS")
        courtsDictionary.Add("19 арбитражный апелляционный суд", "19AAS")
        courtsDictionary.Add("20 арбитражный апелляционный суд", "20AAS")
        courtsDictionary.Add("21 арбитражный апелляционный суд", "21AAS")
        courtsDictionary.Add("АС Алтайского края", "ALTAI-KRAI")
        courtsDictionary.Add("АС Амурской области", "AMURAS")
        courtsDictionary.Add("АС Архангельской области", "ARHANGELSK")
        courtsDictionary.Add("АС Астраханской области", "ASTRAHAN")
        courtsDictionary.Add("АС Белгородской области", "BELGOROD")
        courtsDictionary.Add("АС Брянской области", "BRYANSK")
        courtsDictionary.Add("АС Владимирской области", "VLADIMIR")
        courtsDictionary.Add("АС Волгоградской области", "VOLGOGRAD")
        courtsDictionary.Add("АС Вологодской области", "VOLOGDA")
        courtsDictionary.Add("АС Воронежской области", "VORONEJ")
        courtsDictionary.Add("АС города Москвы", "MSK")
        courtsDictionary.Add("АС города Санкт-Петербурга и Ленинградской области", "SPB")
        courtsDictionary.Add("АС города Севастополя", "SEVASTOPOL")
        courtsDictionary.Add("АС Донецкой Народной Республики", "DNR")
        courtsDictionary.Add("АС Еврейской автономной области", "EAO")
        courtsDictionary.Add("АС Забайкальского края", "CHITA")
        courtsDictionary.Add("АС Запорожской области", "ZAPOROZHYE")
        courtsDictionary.Add("АС Ивановской области", "IVANOVO")
        courtsDictionary.Add("АС Иркутской области", "IRKUTSK")
        courtsDictionary.Add("АС Кабардино-Балкарской Республики", "ASKB")
        courtsDictionary.Add("АС Калининградской области", "KALININGRAD")
        courtsDictionary.Add("АС Калужской области", "KALUGA")
        courtsDictionary.Add("АС Камчатского края", "KAMCHATKA")
        courtsDictionary.Add("АС Карачаево-Черкесской Республики", "ASKCHR")
        courtsDictionary.Add("АС Кемеровской области", "KEMEROVO")
        courtsDictionary.Add("АС Кировской области", "KIROV")
        courtsDictionary.Add("АС Коми-Пермяцкого АО", "KOMI-PERM")
        courtsDictionary.Add("АС Костромской области", "KOSTROMA")
        courtsDictionary.Add("АС Краснодарского края", "KRASNODAR")
        courtsDictionary.Add("АС Красноярского края", "KRANSOYARSK")
        courtsDictionary.Add("АС Курганской области", "KURGAN")
        courtsDictionary.Add("АС Курской области", "KURSK")
        courtsDictionary.Add("АС Липецкой области", "LIPETSK")
        courtsDictionary.Add("АС Луганской Народной Республики", "LNR")
        courtsDictionary.Add("АС Магаданской области", "MAGADAN")
        courtsDictionary.Add("АС Московской области", "ASMO")
        courtsDictionary.Add("АС Мурманской области", "MURMANSK")
        courtsDictionary.Add("АС Нижегородской области", "NNOV")
        courtsDictionary.Add("АС Новгородской области", "NOVGOROD")
        courtsDictionary.Add("АС Новосибирской области", "NOVOSIB")
        courtsDictionary.Add("АС Омской области", "OMSK")
        courtsDictionary.Add("АС Оренбургской области", "ORENBURG")
        courtsDictionary.Add("АС Орловской области", "OREL")
        courtsDictionary.Add("АС Пензенской области", "PENZA")
        courtsDictionary.Add("АС Пермского края", "PERM")
        courtsDictionary.Add("АС Приморского края", "PRIMKRAY")
        courtsDictionary.Add("АС Псковской области", "PSKOV")
        courtsDictionary.Add("АС Республики Адыгея", "ADYG")
        courtsDictionary.Add("АС Республики Алтай", "ALTAI")
        courtsDictionary.Add("АС Республики Башкортостан", "UFA")
        courtsDictionary.Add("АС Республики Бурятия", "BURYATIA")
        courtsDictionary.Add("АС Республики Дагестан", "MAHACHKALA")
        courtsDictionary.Add("АС Республики Ингушетия", "INGUSHETIA")
        courtsDictionary.Add("АС Республики Калмыкия", "KALMYK")
        courtsDictionary.Add("АС Республики Карелия", "KARELIA")
        courtsDictionary.Add("АС Республики Коми", "KOMI")
        courtsDictionary.Add("АС Республики Крым", "KRYM")
        courtsDictionary.Add("АС Республики Марий Эл", "MARI-EL")
        courtsDictionary.Add("АС Республики Мордовия", "ASRM")
        courtsDictionary.Add("АС Республики Саха", "YAKUTSK")
        courtsDictionary.Add("АС Республики Северная Осетия", "ALANIA")
        courtsDictionary.Add("АС Республики Татарстан", "TATARSTAN")
        courtsDictionary.Add("АС Республики Тыва", "TYVA")
        courtsDictionary.Add("АС Республики Хакасия", "KHAKASIA")
        courtsDictionary.Add("АС Ростовской области", "ROSTOV")
        courtsDictionary.Add("АС Рязанской области", "RYAZAN")
        courtsDictionary.Add("АС Самарской области", "SAMARA")
        courtsDictionary.Add("АС Саратовской области", "SARATOV")
        courtsDictionary.Add("АС Сахалинской области", "SAKHALIN")
        courtsDictionary.Add("АС Свердловской области", "EKATERINBURG")
        courtsDictionary.Add("АС Смоленской области", "SMOLENSK")
        courtsDictionary.Add("АС Ставропольского края", "STAVROPOL")
        courtsDictionary.Add("АС Тамбовской области", "TAMBOV")
        courtsDictionary.Add("АС Тверской области", "TVER")
        courtsDictionary.Add("АС Томской области", "TOMSK")
        courtsDictionary.Add("АС Тульской области", "TULA")
        courtsDictionary.Add("АС Тюменской области", "TUMEN")
        courtsDictionary.Add("АС Удмуртской Республики", "UDMURTIYA")
        courtsDictionary.Add("АС Ульяновской области", "ULYANOVSK")
        courtsDictionary.Add("АС Хабаровского края", "KHABAROVSK")
        courtsDictionary.Add("АС Ханты-Мансийского АО", "HMAO")
        courtsDictionary.Add("АС Херсонской области", "KHERSON")
        courtsDictionary.Add("АС Челябинской области", "CHEL")
        courtsDictionary.Add("АС Чеченской Республики", "CHECHNYA")
        courtsDictionary.Add("АС Чувашской Республики", "CHUVASHIA")
        courtsDictionary.Add("АС Чукотского АО", "CHUKOTKA")
        courtsDictionary.Add("АС Ямало-Ненецкого АО", "YAMAL")
        courtsDictionary.Add("АС Ярославской области", "YAROSLAVL")
        courtsDictionary.Add("ПСП Арбитражного суда Пермского края", "KUDIMKAR")
        courtsDictionary.Add("ПСП Арбитражный суд Архангельской области", "NARYANMAR")
        courtsDictionary.Add("Суд по интеллектуальным правам", "SIP")
    End Sub
    Sub logMessage(level As String, system As String, message As String)

        SyncLock logLoc
            dtLog.Rows.Add(Now, level, system, message)
            ToolStripStatusLabel2.Text = $"Записей {dtLog.Rows.Count}"
        End SyncLock

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        logMessage("низкий", "система", "загрузка системы")
        KryptonDataGridView1.DataSource = dtLog
        logMessage("низкий", "система", "загрузка настроек")
        Preference.LoadPreference()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not synchronization Then
            tickCount += 1
            Dim remainingTime = (timeWait * 60) - Math.Round((tickCount / 60), 0)
            Invoke(Sub() ToolStripStatusLabel1.Text = $"До синхронизации осталось: {remainingTime}")
            If remainingTime <= 0 Then
                synchronization = True
            End If
        End If
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click

    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        DownloadDataSite.DownloadDataSite("01.01.2016", "01.08.2024", "--все--", odbcName)
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        settingsSQL.ShowDialog()
    End Sub
End Class
Friend Class Preference
    Sub LoadPreference()
        If GetSetting("NewBa", "Preference", "dateStart", "0") = "0" Then
            Form1.logMessage("критичный", "загрузка настроек", "нет начальной даты в настройках. Проведите настройку и перегрузите приложение")
            Form1.Timer1.Enabled = False
            Form1.ToolStripStatusLabel1.Text = $"Синхронизация не возможна"
        Else
            Form1.dateStart = GetSetting("NewBa", "Preference", "dateStart", "0")
            Form1.logMessage("низкий", "загрузка настроек", $"начальная дата {Form1.dateStart}")

        End If
        If GetSetting("NewBa", "Preference", "dateEnd", "0") = "0" Then
            Form1.logMessage("критичный", "загрузка настроек", "нет конечной даты в настройках. Проведите настройку и перегрузите приложение")
            Form1.Timer1.Enabled = False
            Form1.ToolStripStatusLabel1.Text = $"Синхронизация не возможна"
        Else

            Form1.dateEnd = GetSetting("NewBa", "Preference", "dateEnd", "0")
            Form1.logMessage("низкий", "загрузка настроек", $"конечная дата {Form1.dateEnd}")
        End If
        If GetSetting("NewBa", "Preference", "cour", "0") = "0" Then
            Form1.logMessage("критичный", "загрузка настроек", "нет перечня судов в настройках. Проведите настройку и перегрузите приложение")
            Form1.Timer1.Enabled = False
            Form1.ToolStripStatusLabel1.Text = $"Синхронизация не возможна"
        Else
            Form1.logMessage("низкий", "загрузка настроек", "перечень судов загружен")
            Form1.coursString = GetSetting("NewBa", "Preference", "coursString", "0")
        End If
        If GetSetting("NewBa", "Preference", "timeWait", "0") = "0" Then
            Form1.logMessage("критичный", "загрузка настроек", "нет интервала загрузки. Проведите настройку и перегрузите приложение")
            Form1.Timer1.Enabled = False
            Form1.ToolStripStatusLabel1.Text = $"Синхронизация не возможна"
        Else
            Form1.timeWait = CInt(GetSetting("NewBa", "Preference", "timeWait", "0"))
            Form1.logMessage("низкий", "загрузка настроек", $"интервал загрузки {Form1.timeWait} час")
        End If
        If GetSetting("NewBa", "Preference", "odbcName", "0") = "0" Then
            Form1.logMessage("критичный", "загрузка настроек", "нет прописанного сервера ODBC. Проведите настройку и перегрузите приложение")
            Form1.Timer1.Enabled = False
            Form1.ToolStripStatusLabel1.Text = $"Синхронизация не возможна"
        Else
            Form1.odbcName = GetSetting("NewBa", "Preference", "odbcName", "0")
            Form1.logMessage("низкий", "загрузка настроек", $"Cервер ODBC {Form1.odbcName}")
        End If

    End Sub
    Sub SavePreference(dateStart, dateEnd, cour, timeWait)
        SaveSetting("NewBa", "Preference", "dateStart", dateStart)
        SaveSetting("NewBa", "Preference", "dateEnd", dateEnd)
        SaveSetting("NewBa", "Preference", "cour", cour)
        SaveSetting("NewBa", "Preference", "timeWait", timeWait)
    End Sub
End Class
Friend Class DownloadDataSite
    Friend Sub DownloadDataSite(startDateString As String, endDateString As String, cour As String, odbcName As String)
        Dim parallelOptions As New ParallelOptions()
        parallelOptions.MaxDegreeOfParallelism = 4 ' Set the maximum number of concurrent tasks
        Dim th As New Threading.Thread(Sub()
                                           If cour = "--все--" Then
                                               Dim startDate As Date = Date.ParseExact(startDateString, "dd.MM.yyyy", Nothing)
                                               Dim endDate As Date = Date.ParseExact(endDateString, "dd.MM.yyyy", Nothing)
                                               Parallel.ForEach(Form1.courtsDictionary, parallelOptions, Sub(courtEntry)
                                                                                                             Dim courString = courtEntry.Value
                                                                                                             Dim currentInterval As DateTime = startDate
                                                                                                             While currentInterval < endDate
                                                                                                                 Dim periodChek As Integer = CheckAndUpdateDataPeriodTable(courString, currentInterval.ToString("yyyy-MM-ddTHH:mm:ss"), currentInterval.AddHours(2).ToString("yyyy-MM-ddTHH:59:59"), odbcName)
                                                                                                                 ' Console.WriteLine($"{periodChek} начата загрузка дел для {courString} за {currentInterval.ToString("yyyy-MM-ddTHH:mm:ss")} {currentInterval.AddHours(2).ToString("yyyy-MM-ddTHH:59:59")}")

                                                                                                                 If periodChek = 0 Or periodChek = 2 Then
                                                                                                                     ' Console.WriteLine($"начата загрузка дел для {courString} за {currentInterval.ToString("yyyy-MM-ddTHH:mm:ss")} {currentInterval.AddHours(2).ToString("yyyy-MM-ddTHH:59:59")}")
                                                                                                                     Try
                                                                                                                         Dim OnePage = loadDataOnePageForCour(currentInterval.ToString("yyyy-MM-ddTHH:mm:ss"), currentInterval.AddHours(2).ToString("yyyy-MM-ddTHH:59:59"), courString)
                                                                                                                         Dim errorInteger As Integer = OnePage.Item1
                                                                                                                         Dim dtOnePage As DataTable = OnePage.Item2
                                                                                                                         Dim errorValue As String = OnePage.Item3
                                                                                                                         If errorInteger = 1 Then
                                                                                                                             '' ошибки при выполнении запросов
                                                                                                                             Dim sqlRow = $"update PeriodTable set statusLoad=0, messageStatus='{errorValue}', dateLoad=GETDATE()
                                                                                                                           WHERE (Cour='{courString}' AND dateFrom='{currentInterval.ToString("yyyy-MM-ddTHH:mm:ss")}'AND dateTo='{currentInterval.AddHours(2).ToString("yyyy-MM-ddTHH:59:59")}')"
                                                                                                                             sqlCommand(sqlRow, odbcName)
                                                                                                                         Else
                                                                                                                             If dtOnePage.Rows.Count - 1 > -1 Then
                                                                                                                                 Dim sqlRow = $"update PeriodTable set statusLoad=1, messageStatus='получено строк {dtOnePage.Rows.Count}', dateLoad=GETDATE()
                                                                                                                           WHERE (Cour='{courString}' AND dateFrom='{currentInterval.ToString("yyyy-MM-ddTHH:mm:ss")}'AND dateTo='{currentInterval.AddHours(2).ToString("yyyy-MM-ddTHH:59:59")}')"
                                                                                                                                 sqlCommand(sqlRow, odbcName)
                                                                                                                                 Parallel.ForEach(dtOnePage.AsEnumerable(), parallelOptions, Sub(row)
                                                                                                                                                                                                 Dim IdCase = row("Ссылка на дело").replace("https://kad.arbitr.ru/Card/", "")
                                                                                                                                                                                                 Dim Numbercase = row("Номер дела").replace(" ", "")
                                                                                                                                                                                                 Dim CaseCheck As Integer = CheckAndUpdateDataDeloTable(Numbercase, odbcName)
                                                                                                                                                                                                 If CaseCheck = 0 Or CaseCheck = 2 Then
                                                                                                                                                                                                     'Form1.logMessage("средний", "загрузка документов", $"начата загрузка документов для {Numbercase}")
                                                                                                                                                                                                     Dim twoPage = load_slave_data(IdCase, Numbercase)
                                                                                                                                                                                                     Dim errorIntegerTwoPage As Integer = twoPage.Item1
                                                                                                                                                                                                     Dim dtTwoPage As DataTable = twoPage.Item2
                                                                                                                                                                                                     Dim errorValueTwoPage As String = twoPage.Item3
                                                                                                                                                                                                     If errorIntegerTwoPage = 1 Then
                                                                                                                                                                                                         ''' ошибка при получении данных
                                                                                                                                                                                                         Dim sqlRowSlave = $"update DeloTable set statusLoad=0, messageStatus='{errorValueTwoPage}', dateLoad=GETDATE() WHERE (numberDelo='{Numbercase}')"
                                                                                                                                                                                                         sqlCommand(sqlRow, odbcName)
                                                                                                                                                                                                     Else
                                                                                                                                                                                                         Dim sqlRowSlave = $"update DeloTable set statusLoad=1, messageStatus='документов в деле {dtTwoPage.Rows.Count}', dateLoad=GETDATE() WHERE (numberDelo='{Numbercase}')"
                                                                                                                                                                                                         sqlCommand(sqlRow, odbcName)
                                                                                                                                                                                                     End If
                                                                                                                                                                                                     If dtTwoPage.Rows.Count - 1 > -1 Then
                                                                                                                                                                                                         saveDate_DowloadFile(row, dtTwoPage, odbcName)
                                                                                                                                                                                                     End If
                                                                                                                                                                                                 End If
                                                                                                                                                                                             End Sub)
                                                                                                                             Else
                                                                                                                                 '' установка значение для периода 1
                                                                                                                                 Dim sqlRow = $"update PeriodTable set statusLoad=1, messageStatus='Нет данных за период', dateLoad=GETDATE()
                                                                                                                           WHERE (Cour='{courString}' AND dateFrom='{currentInterval.ToString("yyyy-MM-ddTHH:mm:ss")}'AND dateTo='{currentInterval.AddHours(2).ToString("yyyy-MM-ddTHH:59:59")}') "
                                                                                                                                 sqlCommand(sqlRow, odbcName)
                                                                                                                             End If
                                                                                                                         End If

                                                                                                                     Catch ex As Exception
                                                                                                                         '' установка значение для периода 0
                                                                                                                         Form1.logMessage("критичный", "загрузка данных", $"Ошибка загрузки для {courString}: {ex.Message}")
                                                                                                                     End Try
                                                                                                                 End If
                                                                                                                 'Continue While


                                                                                                                 currentInterval = currentInterval.AddHours(3)
                                                                                                             End While
                                                                                                         End Sub)
                                           Else

                                           End If
                                       End Sub)
        th.Start()
    End Sub
#Region "сохранение данных"
    Sub saveDate_DowloadFile(row As DataRow, DocumetTable As DataTable, OdbcName As String)
        Dim dtCaseNumber As New DataTable
        dtCaseNumber.Columns.Add("Истец")
        dtCaseNumber.Columns.Add("Ответчик")
        dtCaseNumber.Columns.Add("Адрес Ответчик")
        dtCaseNumber.Columns.Add("ИНН Ответчик")
        dtCaseNumber.Columns.Add("Судья")
        dtCaseNumber.Columns.Add("Текущая инстанция")
        dtCaseNumber.Columns.Add("Номер дела")
        dtCaseNumber.Columns.Add("Дата дела")
        dtCaseNumber.Columns.Add("Ссылка на дело")
        dtCaseNumber.Columns.Add("результат")
        dtCaseNumber.Rows.Add(row.ItemArray)
        insertOrSlaveDate(DocumetTable, OdbcName)
        Dim parallelOptions As New ParallelOptions()
        parallelOptions.MaxDegreeOfParallelism = 1 ' Set the maximum number of concurrent tasks
        Parallel.ForEach(DocumetTable.AsEnumerable(), parallelOptions, Sub(rowDocumetTable)
                                                                           If rowDocumetTable("FileName") IsNot DBNull.Value AndAlso rowDocumetTable("FileName") <> "" Then
                                                                               Dim key As String = sql.CalcHash($"{DocumetTable.TableName.ToString}{rowDocumetTable("CaseId")}{rowDocumetTable("InstanceId")}{rowDocumetTable("DocumentTypeId")}{rowDocumetTable("ContentTypesIds")}{rowDocumetTable("PublishDate")}{rowDocumetTable("FileName")}")
                                                                               If CheckFileDownload(key, OdbcName) = 0 Then
                                                                                   Dim GetLinkResult As Dictionary(Of String, Object)
                                                                                   GetLinkResult = getLink($"https://kad.arbitr.ru/Document/Pdf/{rowDocumetTable("CaseId")}/{rowDocumetTable("Id")}/{rowDocumetTable("FileName")}?isAddStamp=True", rowDocumetTable("FileName"))
                                                                                   If GetLinkResult("result") <> "error" Then
                                                                                       InsertIntoDocDownload(DocumetTable.TableName.ToString, rowDocumetTable("CaseId").ToString(), rowDocumetTable("InstanceId").ToString(), rowDocumetTable("DocumentTypeId").ToString(), rowDocumetTable("ContentTypesIds").ToString(), Convert.ToDateTime(rowDocumetTable("PublishDate")), rowDocumetTable("FileName").ToString(), key, My.Computer.Name.ToString(), 0, "ok", OdbcName)
                                                                                       InsertIntoFileTable(GetLinkResult, key, rowDocumetTable("FileName"), OdbcName)
                                                                                       InsertCaseNumbers(row, OdbcName)
                                                                                   Else
                                                                                       InsertIntoDocDownload(DocumetTable.TableName.ToString, rowDocumetTable("CaseId").ToString(), rowDocumetTable("InstanceId").ToString(), rowDocumetTable("DocumentTypeId").ToString(), rowDocumetTable("ContentTypesIds").ToString(), Convert.ToDateTime(rowDocumetTable("PublishDate")), rowDocumetTable("FileName").ToString(), key, My.Computer.Name.ToString(), 1, GetLinkResult("value"), OdbcName)
                                                                                       InsertCaseNumbers(row, OdbcName)
                                                                                   End If
                                                                               End If
                                                                           End If
                                                                       End Sub)
    End Sub
    Sub GetLink00(link As String, fileName As String)
        Try
            Dim request As HttpWebRequest = CType(WebRequest.Create(link), HttpWebRequest)
            request.Method = "GET"
            request.Headers.Add("authority", "kad.arbitr.ru")
            request.Headers.Add("scheme", "https")
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"
            request.Headers.Add("cache-Control", "no-cache")
            request.Headers.Add("cookie", "__ddg1_=u9xSiSmcJfkN5pU5y100; ASP.NET_SessionId=clhl0y3bjuudvli4hlmhv0xq; CUID=c4a3fab4-05be-4737-b413-76447d3d5a7d:l12CVyOIATzhdkPiY4g5ag==; pr_fp=8d68911b87a0ed8a0530991644bed9731a78d9f7920edb957a2a2c89c1cb2644; wasm=9004d5a262ad9a5c814120d4f8d6facd; rcid=6ff31af9-9d45-490f-affd-023793510319; KadLVCards=%d0%9083-2639%2f2023~%d0%9072-14201%2f2023")
            request.Headers.Add("pragma", "no-cache")
            request.Headers.Add("sec-fetch-dest", "document")
            request.Headers.Add("sec-fetch-Mode", "navigate")
            request.Headers.Add("sec-fetch-site", "none")
            request.Headers.Add("sec-fetch-user", "?1")
            request.Headers.Add("upgrade-insecure-requests", "1")
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 (Chromium GOST) Safari/537.36"
            request.Timeout = 10000
            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using responseStream As Stream = response.GetResponseStream()
                    Using reader As New StreamReader(responseStream)
                        Dim responseText As String = reader.ReadToEnd()
                        Dim htmlDoc As New HtmlDocument()
                        htmlDoc.LoadHtml(responseText)
                        Dim tokenElement As HtmlNode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='token']")
                        Dim tokenValue As String = If(tokenElement IsNot Nothing, tokenElement.GetAttributeValue("value", ""), Nothing)
                        Dim tokenHtml As HtmlNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='salto']")
                        Dim token As String = If(tokenHtml IsNot Nothing, tokenHtml.InnerText, Nothing)
                        If tokenValue IsNot Nothing AndAlso token IsNot Nothing Then
                            Dim bodyStringOld As String = CalcHash($"{tokenValue}{token}")
                            Dim bodyString = $"token={tokenValue}&hash={bodyStringOld}"
                            DownloadFile(link, fileName, bodyString)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Form1.logMessage("критический", "скачивание файла", "ошибка при скачивании файла")
            Console.WriteLine("An error occurred: " & ex.Message)
        End Try
    End Sub
    Function getLink(link As String, fileName As String) As Dictionary(Of String, Object)
        Dim result As New Dictionary(Of String, Object)
        result.Add("result", "error")
        result.Add("value", "")
        Dim tryCount = 3
        While tryCount <= 4
            Try
                Using client As New WebClient()
                    client.Headers.Add("authority", "kad.arbitr.ru")
                    client.Headers.Add("method", "GET")
                    client.Headers.Add("path", link.Replace("https://kad.arbitr.ru", ""))
                    client.Headers.Add("scheme", "https")
                    client.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9")
                    client.Headers.Add("cache-Control", "no-cache")
                    client.Headers.Add("cookie", "__ddg1_=u9xSiSmcJfkN5pU5y100; ASP.NET_SessionId=clhl0y3bjuudvli4hlmhv0xq; CUID=c4a3fab4-05be-4737-b413-76447d3d5a7d:l12CVyOIATzhdkPiY4g5ag==; pr_fp=8d68911b87a0ed8a0530991644bed9731a78d9f7920edb957a2a2c89c1cb2644; wasm=9004d5a262ad9a5c814120d4f8d6facd; rcid=6ff31af9-9d45-490f-affd-023793510319; KadLVCards=%d0%9083-2639%2f2023~%d0%9072-14201%2f2023")
                    client.Headers.Add("pragma", "no-cache")
                    client.Headers.Add("sec-fetch-dest", "document")
                    client.Headers.Add("sec-fetch-Mode", "navigate")
                    client.Headers.Add("sec-fetch-site", "none")
                    client.Headers.Add("sec-fetch-user", "?1")
                    client.Headers.Add("upgrade-insecure-requests", "1")
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 (Chromium GOST) Safari/537.36")
                    Dim response = client.DownloadString(link)
                    Dim htmlDoc As New HtmlDocument()
                    Using reader As New StringReader(response)
                        htmlDoc.Load(reader)
                        Dim tokenElement As HtmlNode = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='token']")
                        Dim tokenValue As String = If(tokenElement IsNot Nothing, tokenElement.GetAttributeValue("value", ""), Nothing)
                        Dim tokenHtml As HtmlNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='salto']")
                        Dim token As String = If(tokenHtml IsNot Nothing, tokenHtml.InnerText, Nothing)
                        If tokenValue IsNot Nothing AndAlso token IsNot Nothing Then
                            Dim bodyString = $"token={tokenValue}&hash={CalcHash($"{tokenValue}{token}")}"
                            result = DownloadFile(link, fileName, bodyString)
                            If result("result") = "error" Then
                                tryCount += 1
                                If tryCount = 3 Then
                                    Console.WriteLine($"---------{tryCount}-----------")
                                    Console.WriteLine($"token: {tokenValue} salto:{token}")
                                    Console.WriteLine($"MD5: {CalcHash($"{tokenValue}{token}")}")
                                    Console.WriteLine($"bodyString: token={tokenValue}&hash={CalcHash($"{tokenValue}{token}")}")
                                    Console.WriteLine(link)
                                    Console.WriteLine(fileName)
                                End If
                                Thread.Sleep(1000)
                            End If
                        End If
                    End Using
                End Using
            Catch ex As Exception
                tryCount += 1
                Console.WriteLine(ex.Message)
                Thread.Sleep(1000)
            End Try
        End While
        Return result
    End Function
    Function DownloadFile(link As String, fileName As String, bodyString As String) As Dictionary(Of String, Object)
        Dim errorString As String
        Dim tryCount As Integer = 0
        Dim result As New Dictionary(Of String, Object)
        result.Add("result", "error")
        result.Add("value", "")
        While tryCount <= 3
            Try
                Dim request As HttpWebRequest = CType(WebRequest.Create(link), HttpWebRequest)
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.Headers.Add("Cookie", "__ddg1_=o7Nym5rKozYjs2WvlWiA; ASP.NET_SessionId=dvsgd3hvcyryao5ipwgw2ymq; CUID=5b66a4a3-5c7d-4fdc-881a-2d1fa81f1494:SNQhVznRyGAy94uuls6OSw==; rcid=1084fef5-d143-4ae5-92c1-71d3eb02f7ea; pr_fp=8d68911b87a0ed8a0530991644bed9731a78d9f7920edb957a2a2c89c1cb2644; wasm=5c9c10c4ef0e243e85ff6ed1091ff2f5")
                request.Headers.Add("authority", "kad.arbitr.ru")
                request.Headers.Add("Method", "POST")
                request.Headers.Add("path", Replace(link, "https://kad.arbitr.ru", ""))
                request.Headers.Add("scheme", "https")
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"
                request.Headers.Add("cache-Control", "no-cache")
                request.Headers.Add("origin", "https://kad.arbitr.ru")
                request.Headers.Add("pragma", "no-cache")
                request.Referer = link
                request.Headers.Add("sec-fetch-dest", "document")
                request.Headers.Add("sec-fetch-Mode", "navigate")
                request.Headers.Add("sec-fetch-site", "same-origin")
                request.Headers.Add("upgrade-insecure-requests", "1")
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 (Chromium GOST) Safari/537.36"
                ' Установка времени ожидания (тайм-аут)
                request.Timeout = 50000 ' 50 секунд
                ' Запись данных в запрос
                Dim requestData As Byte() = Encoding.UTF8.GetBytes(bodyString)
                request.ContentLength = requestData.Length
                Using requestStream As Stream = request.GetRequestStream()
                    requestStream.Write(requestData, 0, requestData.Length)
                End Using
                ' Получение ответа
                Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                    If response.StatusCode = HttpStatusCode.OK Then
                        Using responseStream As Stream = response.GetResponseStream()
                            Dim pdfContent As Byte() = ReadStream(responseStream)
                            Dim textDic As Dictionary(Of String, String) = pdfToText.ExtractTextFromPdf(pdfContent)
                            If textDic("result") = "ok" Then
                                Dim compressedText = CompressText(textDic("value"))
                                result("result") = "ok"
                                result("value") = compressedText
                                Return result
                            Else
                                result("result") = "error"
                                result("value") = textDic("value").ToString()
                                Return result
                            End If
                        End Using
                    End If
                End Using
            Catch ex As Exception
                tryCount += 1
                Thread.Sleep(1000)
                errorString = ex.Message
                result("result") = "error"
                result("value") = errorString
            End Try
        End While
        Return result
    End Function
    Private Function ReadStream(stream As Stream) As Byte()
        Using memoryStream As New MemoryStream()
            stream.CopyTo(memoryStream)
            Return memoryStream.ToArray()
        End Using
    End Function

    Function downLadFile1(link As String, fileName As String, bodyString As String)
        Dim errorString As String
        Dim TryCount = 0
        While TryCount <= 3
            Try
                Using client As New WebClient()
                    client.Headers(HttpRequestHeader.ContentType) = "application/x-www-form-urlencoded"
                    client.Headers(HttpRequestHeader.Cookie) = "__ddg1_=o7Nym5rKozYjs2WvlWiA; ASP.NET_SessionId=dvsgd3hvcyryao5ipwgw2ymq; CUID=5b66a4a3-5c7d-4fdc-881a-2d1fa81f1494:SNQhVznRyGAy94uuls6OSw==; rcid=1084fef5-d143-4ae5-92c1-71d3eb02f7ea; pr_fp=8d68911b87a0ed8a0530991644bed9731a78d9f7920edb957a2a2c89c1cb2644; wasm=5c9c10c4ef0e243e85ff6ed1091ff2f5"
                    client.Headers.Add("authority", "kad.arbitr.ru")
                    client.Headers.Add("Method", "POST")
                    client.Headers.Add("path", Replace(link, "https://kad.arbitr.ru", ""))
                    client.Headers.Add("scheme", "https")
                    client.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9")
                    client.Headers.Add("cache-Control", "no-cache")
                    client.Headers.Add("origin", "https://kad.arbitr.ru")
                    client.Headers.Add("pragma", "no-cache")
                    client.Headers.Add("referer", link)
                    client.Headers.Add("sec-fetch-dest", "document")
                    client.Headers.Add("sec-fetch-Mode", "navigate")
                    client.Headers.Add("sec-fetch-site", "same-origin")
                    client.Headers.Add("upgrade-insecure-requests", "1")
                    client.Headers(HttpRequestHeader.UserAgent) = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 (Chromium GOST) Safari/537.36"
                    Dim responseBytes As Byte() = client.UploadData(link, "POST", System.Text.Encoding.UTF8.GetBytes(bodyString))
                    File.WriteAllBytes("D:\pdf\" & fileName, responseBytes)
                    errorString = "ok"
                End Using
            Catch ex As Exception
                TryCount += 1
                Threading.Thread.Sleep(1000)
                errorString = ex.Message
            End Try
        End While
        If errorString <> "ok" Then
            Console.WriteLine($"Ошибка скачивания файла {errorString}")
        End If
    End Function

    Function CalcHash(ByVal input As String) As String
        Dim md5 As MD5 = MD5.Create()
        Dim inputBytes As Byte() = Encoding.UTF8.GetBytes(input)

        Dim hashBytes As Byte() = md5.ComputeHash(inputBytes)

        Dim sb As New StringBuilder()
        For i As Integer = 0 To hashBytes.Length - 1
            sb.Append(hashBytes(i).ToString("x2"))
        Next

        Return sb.ToString()
    End Function
#End Region
#Region "Первая страница"
    Private Function loadDataOnePageForCour(DateFrom, DateTo, courString) As Tuple(Of Integer, DataTable, String)
        Dim dt As New DataTable
        dt.Columns.Add("Истец")
        dt.Columns.Add("Ответчик")
        dt.Columns.Add("Адрес Ответчик")
        dt.Columns.Add("ИНН Ответчик")
        dt.Columns.Add("Судья")
        dt.Columns.Add("Текущая инстанция")
        dt.Columns.Add("Номер дела")
        dt.Columns.Add("Дата дела")
        dt.Columns.Add("Ссылка на дело")
        dt.Columns.Add("результат")
        Dim errorString As String = ""
        Dim errorValue = ""
        Dim errorInteger = 0
        Dim arbitr_url As String = "https://kad.arbitr.ru/Kad/SearchInstances"
        Dim countPage = 1
        Dim totalPage = 1
        While countPage <= totalPage
            Using s As New WebClient()
                s.Headers.Add("accept", "*/*")
                s.Headers.Add("accept-language", "ru,en;q=0.9")
                s.Headers.Add("content-type", "application/json")
                s.Headers.Add("sec-ch-ua", Chr(34) & "Chromium" & Chr(34) & ";v=" & Chr(34) & "118" & Chr(34) & ", " & Chr(34) & "YaBrowser" & Chr(34) & ";v=" & Chr(34) & "23.11" & Chr(34) & ", " & Chr(34) & "Not=A?Brand" & Chr(34) & ";v=" & Chr(34) & "99" & Chr(34) & ", " & Chr(34) & "Yowser" & Chr(34) & ";v=" & Chr(34) & "2.5" & Chr(34))
                s.Headers.Add("sec-ch-ua-mobile", "?0")
                s.Headers.Add("sec-ch-ua-platform", Chr(34) & "Windows" & Chr(34))
                s.Headers.Add("sec-fetch-dest", "empty")
                s.Headers.Add("sec-fetch-mode", "cors")
                s.Headers.Add("sec-fetch-site", "same-origin")
                s.Headers.Add("x-date-format", "iso")
                s.Headers.Add("x-requested-with", "XMLHttpRequest")
                s.Headers.Add("cookie", "__ddg1_=9gFKcd7lVvDnnO1Vt1vI; ASP.NET_SessionId=j4p1cezh03n013rpssyaenvx; CUID=0be0ebf6-f43f-4dcd-ba4f-729261ef2e62:MfsNh9jDCBOABo7CsXWm0Q==; _ga=GA1.2.514265384.1703129978; tmr_lvid=702394ce207f318db1e3a3c6460f669c; tmr_lvidTS=1703129978821; _ym_uid=1703129979947556729; _ym_d=1703129979; pr_fp=53e5e157001cb673ed49ab6e5af587e1dfe1739e8015965e7a64863436e604ab; _ga_5C6XL8NQPW=GS1.2.1712032118.2.0.1712032118.0.0.0; _gid=GA1.2.318748716.1712719752; KadLVCards=; Notification_All=c24c93cff13444f18c876266168e77a6_1712815200000_shown; _ga_Q2V7P901XE=GS1.2.1712808585.22.0.1712808585.0.0.0; tmr_detect=1%7C1712808585279; _ga_EYS41HMRV3=GS1.2.1712808585.22.0.1712808585.60.0.0; _ym_isad=1; domain_sid=51ZVlIPYbbzlnFVgwpXoo%3A1712808587591; wasm=0f7c2fcb1580137fd5b25b4d4e6d1f7c; rcid=ba5e3d52-c24b-4c86-8dea-f7f726bbb7fc; _gat=1; _ga_9582CL89Y6=GS1.2.1712808584.22.1.1712810184.43.0.0")
                s.Headers.Add("Referer", "https://kad.arbitr.ru/")
                s.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin")
                Dim requestBody As String = $"{{'Page':{countPage},'Count':25,'CaseType':'B','Courts':['{courString}'],'DateFrom':'{DateFrom}','DateTo':'{DateTo}','Sides':[],'Judges':[],'CaseNumbers':[],'WithVKSInstances':false}}"
                requestBody = requestBody.Replace("|", ChrW(34)).Replace("'", ChrW(34))
                Dim tryCount = 0
                Dim responseBytes As Byte() = s.UploadData(arbitr_url, "POST", Encoding.UTF8.GetBytes(requestBody))
                Dim responseBody As String = Encoding.UTF8.GetString(responseBytes)
                While tryCount < 3
                    Try
                        If countPage = 1 Then
                            Dim htmlDoc As New HtmlDocument()
                            htmlDoc.LoadHtml(responseBody)
                            Dim documentsTotalCount As Integer = Convert.ToInt32(htmlDoc.DocumentNode.SelectSingleNode("//input[@id='documentsTotalCount']").GetAttributeValue("value", "0"))
                            Dim documentsPagesCount = Convert.ToInt32(htmlDoc.DocumentNode.SelectSingleNode("//input[@id='documentsPagesCount']").GetAttributeValue("value", "0"))
                            totalPage = documentsPagesCount
                        End If
                        dt.Merge(getDataDt(responseBody, dt.Clone()))
                        errorString = Nothing
                        Exit While
                    Catch ex As Exception
                        Console.WriteLine($"try: {tryCount} for:{requestBody}")
                        errorString = ex.Message
                        tryCount += 1
                        Threading.Thread.Sleep(1000)
                    End Try
                End While
            End Using
            If errorString IsNot Nothing Then
                errorInteger = 1
                errorValue = $"{errorValue}; ошибка получения данных для {courString} за период {DateFrom} по {DateTo} на странице {countPage}. Ошибка: {errorString}{vbCrLf}"
            End If
            countPage += 1
        End While
        loadDataOnePageForCour = New Tuple(Of Integer, DataTable, String)(errorInteger, dt, errorValue)
    End Function
    Private Function getDataDt(htmlString As String, dt As DataTable) As DataTable
        Dim htmlDoc As New HtmlDocument()
        htmlDoc.LoadHtml(htmlString)
        Dim tableRows As HtmlNodeCollection = htmlDoc.DocumentNode.SelectNodes("//tr")
        If tableRows IsNot Nothing Then
            For Each row As HtmlNode In tableRows
                Dim plaintiffs As New List(Of String)
                Dim cells As HtmlNodeCollection = row.SelectNodes("td")
                If cells IsNot Nothing AndAlso cells.Count = 4 Then

                    Dim истец As String = istec(cells(2).InnerHtml)
                    Dim ответчик As Dictionary(Of String, String) = otvet(cells(3).InnerHtml)
                    Dim судья = ""
                    Dim judgeNode As HtmlNode = cells(1).SelectSingleNode(".//div[@class='judge']")
                    If judgeNode IsNot Nothing Then
                        судья = judgeNode.InnerText.Trim()
                    End If
                    Dim текущая_инстанция = ""
                    Dim текущая_инстанция_Node As HtmlNode = cells(1).SelectSingleNode(".//div[2]")
                    If текущая_инстанция_Node IsNot Nothing Then
                        текущая_инстанция = текущая_инстанция_Node.InnerText.Trim()
                    End If
                    Dim номер_дела = cells(0).SelectSingleNode(".//a").InnerText.Trim()
                    Dim дата_дела = cells(0).SelectSingleNode(".//span").InnerText.Trim()
                    Dim ссылка_на_дело = cells(0).SelectSingleNode(".//a").GetAttributeValue("href", "").Trim()
                    Dim newrow As DataRow = dt.NewRow()
                    newrow("Истец") = истец
                    newrow("Ответчик") = ответчик("name")
                    newrow("Адрес Ответчик") = ответчик("address")
                    newrow("ИНН Ответчик") = ответчик("inn")
                    newrow("Судья") = судья
                    newrow("Текущая инстанция") = текущая_инстанция
                    newrow("Номер дела") = номер_дела
                    newrow("Дата дела") = дата_дела
                    newrow("Ссылка на дело") = ссылка_на_дело
                    dt.Rows.Add(newrow.ItemArray)
                End If
            Next
        End If
        Return dt
    End Function
    Private Function istec(htmlContent) As String
        Dim vulue As String = ""
        Dim htmlDocument As New HtmlDocument()
        htmlDocument.LoadHtml(htmlContent)
        Dim plaintiffsList As New List(Of String)
        Dim spanNodes = htmlDocument.DocumentNode.SelectNodes("//span[@class='js-rollover b-newRollover']")
        If spanNodes IsNot Nothing Then
            For Each spanNode In spanNodes
                Dim plaintiffNameNode = spanNode.SelectSingleNode(".//strong")
                Dim plaintiffAddressNode = spanNode.SelectSingleNode(".//span[@class='js-rolloverHtml']")
                If plaintiffNameNode IsNot Nothing AndAlso plaintiffAddressNode IsNot Nothing Then
                    Dim plaintiffName As String = plaintiffNameNode.InnerText.Trim().ToString().Replace("&quot;", String.Empty)
                    Dim plaintiffAddressContent As String = plaintiffAddressNode.InnerText.Trim()
                    Dim match = Regex.Match(plaintiffAddressContent, "(\d{6},[\s\S]*)$")
                    Dim plaintiffAddress As String = If(match.Success, match.Groups(1).Value.Trim(), "Адрес отсутствует")

                    plaintiffsList.Add($"Истец:{plaintiffName}|Адрес: {plaintiffAddress}")
                End If
            Next
            For Each plaintiff In plaintiffsList
                vulue = vulue & $"; {plaintiff}"
            Next
            While vulue.IndexOf("  ") <> -1
                vulue = vulue.Replace("  ", "")
            End While
        End If
        istec = vulue
    End Function
    Private Function otvet(htmlCode) As Dictionary(Of String, String)
        Dim dic As New Dictionary(Of String, String)
        dic.Add("name", "")
        dic.Add("address", "")
        dic.Add("inn", "")
        Dim htmlDoc As New HtmlAgilityPack.HtmlDocument()
        htmlDoc.LoadHtml(htmlCode)
        Dim name As String = ""
        Dim inn As String = ""
        Dim detailedInfo As String = ""
        Dim personNode As HtmlAgilityPack.HtmlNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='b-container']/div/span")
        If personNode IsNot Nothing Then
            Dim strongNode As HtmlAgilityPack.HtmlNode = personNode.SelectSingleNode("//span[@class='js-rolloverHtml']/strong")
            name = strongNode.InnerText.Trim()
            If name = "" Then
                Dim strongNode1 As HtmlNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='js-rolloverHtml']/strong")
                name = strongNode.InnerText.Trim()
            End If

            Dim additionalInfoNode As HtmlAgilityPack.HtmlNode = personNode.SelectSingleNode("./span[@class='js-rolloverHtml']")
            If additionalInfoNode IsNot Nothing Then
                detailedInfo = additionalInfoNode.InnerText.Trim()
                ' You can parse detailedInfo to extract address and INN here
                Dim innIndex As Integer = detailedInfo.IndexOf("ИНН:")
                If innIndex <> -1 Then
                    inn = detailedInfo.Substring(innIndex + 5).Trim()
                End If
            End If
        End If
        detailedInfo = detailedInfo.Replace(name, "").Replace($"ИНН: {inn}", "")
        While detailedInfo.IndexOf("  ") <> -1
            detailedInfo = detailedInfo.Replace("  ", "")
        End While
        detailedInfo = detailedInfo.Replace(Environment.NewLine, "")
        dic("name") = name.Replace(Environment.NewLine, "").Trim().Replace(";", "").Replace("&", "").Replace("quota", "")
        dic("address") = detailedInfo
        dic("inn") = inn
        otvet = dic
    End Function
#End Region
#Region "Вторая страница"
    Friend Function load_slave_data(IdCase As String, Numbercase As String) As Tuple(Of Integer, DataTable, String)
        Dim errorString = ""
        Dim loc As New Object
        Dim dtMaster As New DataTable
        dtMaster.TableName = Numbercase
        dtMaster.Columns.Add("CaseId")
        dtMaster.Columns.Add("InstanceId")
        dtMaster.Columns.Add("Id")
        dtMaster.Columns.Add("InstStage")
        dtMaster.Columns.Add("DocStage")
        dtMaster.Columns.Add("FinishInstance")
        dtMaster.Columns.Add("PublishDate")
        dtMaster.Columns.Add("DisplayDate")
        dtMaster.Columns.Add("PublishDisplayDate")
        dtMaster.Columns.Add("AppealDate")
        dtMaster.Columns.Add("IsSimpleJustice")
        dtMaster.Columns.Add("IncomingNumProcessed")
        dtMaster.Columns.Add("ReasonDocumentId")
        dtMaster.Columns.Add("Content")
        dtMaster.Columns.Add("GeneralDecisionType")
        dtMaster.Columns.Add("DecisionType")
        dtMaster.Columns.Add("DecisionTypeName")
        dtMaster.Columns.Add("ClaimSum")
        dtMaster.Columns.Add("RecoverySum")
        dtMaster.Columns.Add("IsStart")
        dtMaster.Columns.Add("IsPresidiumSessionEvent")
        dtMaster.Columns.Add("SignatureInfo")
        dtMaster.Columns.Add("Judges")
        dtMaster.Columns.Add("Declarers")
        dtMaster.Columns.Add("LinkedSideIds")
        dtMaster.Columns.Add("FileName")
        dtMaster.Columns.Add("OriginalActFileName")
        dtMaster.Columns.Add("AdditionalInfo")
        dtMaster.Columns.Add("SystemDocumentType")
        dtMaster.Columns.Add("CompensationAmount")
        dtMaster.Columns.Add("DeadlineDate")
        dtMaster.Columns.Add("CanSeeDocPostItem")
        dtMaster.Columns.Add("SimpleJusticeFileState")
        dtMaster.Columns.Add("Signer")
        dtMaster.Columns.Add("AppealedDocuments")
        dtMaster.Columns.Add("AppealState")
        dtMaster.Columns.Add("AppealDescription")
        dtMaster.Columns.Add("Comment")
        dtMaster.Columns.Add("InstanceLevel")
        dtMaster.Columns.Add("Addressee")
        dtMaster.Columns.Add("IsDeleted")
        dtMaster.Columns.Add("DelReason")
        dtMaster.Columns.Add("ViewsCount")
        dtMaster.Columns.Add("DelDate")
        dtMaster.Columns.Add("CanBeDeleted")
        dtMaster.Columns.Add("DocSession")
        dtMaster.Columns.Add("WithAttachment")
        dtMaster.Columns.Add("AttachmentCount")
        dtMaster.Columns.Add("HasSignature")
        dtMaster.Columns.Add("AcceptMAID")
        dtMaster.Columns.Add("RosRegNum")
        dtMaster.Columns.Add("Date")
        dtMaster.Columns.Add("Type")
        dtMaster.Columns.Add("IsAct")
        dtMaster.Columns.Add("HearingDate")
        dtMaster.Columns.Add("DocumentTypeId")
        dtMaster.Columns.Add("ActualDate")
        dtMaster.Columns.Add("ContentTypesIds")
        dtMaster.Columns.Add("ContentTypes")
        dtMaster.Columns.Add("DocumentTypeName")
        dtMaster.Columns.Add("CrocId")
        dtMaster.Columns.Add("SourceSystem")
        dtMaster.Columns.Add("HearingPlace")
        dtMaster.Columns.Add("CourtTag")
        dtMaster.Columns.Add("CourtName")
        dtMaster.Columns.Add("UseShortCourtName")

        Dim caseInfoDic = load1(IdCase, Numbercase)
        If caseInfoDic("result") <> "ok" Then
            errorString = $"ошибка при получении инстанций дела для {IdCase}: {TryCast(caseInfoDic("value"), String)}"
        Else
            Dim caseInfo = caseInfoDic("value")
            If caseInfo IsNot Nothing Then

                Dim CaseCategoryDispute = caseInfo.CaseCategoryDispute
                Dim CaseNumber = caseInfo.CaseNumber
                Dim CaseState = caseInfo.CaseState
                Dim CaseType = caseInfo.CaseType
                Dim SinceStart = caseInfo.SinceStart
                Dim Id = caseInfo.Id

                For Each instace In caseInfo.Instances
                    Dim cour = instace.Court
                    Dim Name = instace.Court.name
                    Dim Tag = instace.Court.Tag
                    Dim Url = instace.Court.Url
                    Dim countPage = 1
                    Dim totalPage = 1
                    While countPage <= totalPage
                        Dim searhUri = $"https://kad.arbitr.ru/Kad/InstanceDocumentsPage?_=1698292912925&id={instace.id}&caseId={Id}&withProtocols=true&perPage=30&page={countPage}"
                        Dim searhRef = $"ref:https://kad.arbitr.ru/Card/{Id}"
                        Dim input As String = Numbercase
                        Dim encodedString As String = HttpUtility.UrlEncode(input, System.Text.Encoding.UTF8)
                        Dim TwoPage = load2(searhUri, searhRef, encodedString, dtMaster.Clone())
                        totalPage = TwoPage.Item1
                        dtMaster.Merge(TwoPage.Item2)
                        If Len(TwoPage.Item3) > 3 Then errorString = $"{errorString} {vbCrLf} {TwoPage.Item3}"
                        countPage += 1
                    End While

                Next
            Else
            End If
        End If
        If Len(errorString) > 3 Then
            load_slave_data = New Tuple(Of Integer, DataTable, String)(1, dtMaster, errorString)
        Else
            load_slave_data = New Tuple(Of Integer, DataTable, String)(0, dtMaster, errorString)
        End If
    End Function
    Private Function load1(IdCase, Numbercase)
        Dim dic As New Dictionary(Of String, Object)
        dic.Add("result", "error")
        dic.Add("value", Nothing)
        Try
            Numbercase = HttpUtility.UrlEncode(Numbercase, System.Text.Encoding.UTF8)
            Dim arbitr_url As String = $"https://kad.arbitr.ru/Card/{IdCase}"

            Using s As New WebClient()
                s.Headers.Add("accept", "*/*")
                s.Headers.Add("accept-language", "ru,en;q=0.9")
                s.Headers.Add("content-type", "application/json")
                s.Headers.Add("sec-ch-ua", Chr(34) & "Chromium" & Chr(34) & ";v=" & Chr(34) & "118" & Chr(34) & ", " & Chr(34) & "YaBrowser" & Chr(34) & ";v=" & Chr(34) & "23.11" & Chr(34) & ", " & Chr(34) & "Not=A?Brand" & Chr(34) & ";v=" & Chr(34) & "99" & Chr(34) & ", " & Chr(34) & "Yowser" & Chr(34) & ";v=" & Chr(34) & "2.5" & Chr(34))
                s.Headers.Add("sec-ch-ua-mobile", "?0")
                s.Headers.Add("sec-ch-ua-platform", Chr(34) & "Windows" & Chr(34))
                s.Headers.Add("sec-fetch-dest", "empty")
                s.Headers.Add("sec-fetch-mode", "cors")
                s.Headers.Add("sec-fetch-site", "same-origin")
                s.Headers.Add("x-date-format", "iso")
                s.Headers.Add("x-requested-with", "XMLHttpRequest")
                s.Headers.Add("cookie", $"__ddg1_=95rFUyBZiF9XtmaD0dyQ; CUID=218370cb-ab7b-41ba-866a-f45abda05a95:8tbbisEPj0b33t1H5hD1Kw==; _ga=GA1.2.982175830.1706625216; tmr_lvid=87b40db4c05df8eec25a242a7a74448c; tmr_lvidTS=1706625216494; _ym_uid=1706625217569477494; _ym_d=1706625217; ASP.NET_SessionId=y5ikmm43ua1rh0euznsmri0m; pr_fp=d6e0407bcd294a37c9e09024256941a20857fe8f2a131154120e6e20d345eed7; rcid=60e98279-36cf-4b72-897d-0f8917619137; KadLVCards={Numbercase}; _gid=GA1.2.1890868385.1708330515; _ym_isad=2; _gat=1; _gat_FrontEndTracker=1; _dc_gtm_UA-157906562-1=1; _ga_9582CL89Y6=GS1.2.1708358245.19.0.1708358245.60.0.0; _ga_EYS41HMRV3=GS1.2.1708358245.16.0.1708358245.60.0.0; _ga_Q2V7P901XE=GS1.2.1708358245.16.0.1708358245.0.0.0; tmr_detect=0%7C1708358247620; wasm=8eb64d9ec4f3dd3b7b77577a4a134514")
                s.Headers.Add("Referer", "https://kad.arbitr.ru/")
                s.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin")

                ' Execute the request and get response
                Dim responseBytes As Byte() = s.UploadData(arbitr_url, "POST", Encoding.UTF8.GetBytes(""))
                Dim responseBody As String = Encoding.UTF8.GetString(responseBytes)

                ' Check the HTTP response code

                Dim va = JsonConvert.DeserializeObject(Of DesLoad1.Root)(responseBody)
                dic("result") = "ok"
                dic("value") = va.Result.CaseInfo
                Return dic
            End Using
        Catch webEx As WebException
            ' Handle web exceptions (e.g., HTTP errors)
            dic("result") = "error"
            dic("value") = $"WebException: {webEx.Message}"
        Catch ex As Exception
            ' Handle general exceptions
            dic("result") = "error"
            dic("value") = $"Exception: {ex.Message}"
        End Try

        ' Return value in case of error
        Return dic
    End Function

    Private Function load2(url, ref, req, dtMaster) As Tuple(Of Integer, DataTable, String)
        Dim tryCount = 0
        Dim numberPage = 0
        Dim errorString = ""
        While tryCount < 3
            Try
                Using client As New WebClient()
                    client.Headers.Add("authority", "kad.arbitr.ru")
                    client.Headers.Add("method", "GET")
                    client.Headers.Add("path", Replace(url, "https://kad.arbitr.ru", ""))
                    client.Headers.Add("scheme", "https")
                    client.Headers.Add("accept", "application/json, text/javascript, */*")
                    client.Headers.Add("cache-Control", "no-cache")
                    client.Headers.Add("content-type", "application/json")
                    client.Headers.Add("cookie", "__ddg1_=UoTy61YeX47lHlAOvpgh; ASP.NET_SessionId=oqj3yqnr431p51hjdjfeknco; CUID=5b241496-a8a4-4fe4-9341-c43a5e92a45e:rT2/Nib57t/nbkYXnJlOqg==; pr_fp=8d68911b87a0ed8a0530991644bed9731a78d9f7920edb957a2a2c89c1cb2644; rcid=68af84df-b4d4-4ec4-9c10-11f40aba1977; wasm=9dae24fecc21afccf72314d0ec430b31; KadLVCards=" & req)
                    client.Headers.Add("pragma", "no-cache")
                    client.Headers.Add("referer", ref)
                    client.Headers.Add("sec-fetch-dest", "empty")
                    client.Headers.Add("sec-fetch-Mode", "cors")
                    client.Headers.Add("sec-fetch-site", "same-origin")
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 (Chromium GOST) Safari/537.36")
                    client.Headers.Add("x-requested-with", "XMLHttpRequest")
                    Dim responseBytes As Byte() = client.DownloadData(url)
                    Dim responseString As String = System.Text.Encoding.UTF8.GetString(responseBytes)

                    Dim va = JsonConvert.DeserializeObject(Of desLoad2.Root)(responseString)
                    For Each values In va.Result.Items
                        Dim row As DataRow = dtMaster.NewRow()
                        Dim tempString = ""
                        row("CaseId") = values.CaseId
                        row("InstanceId") = values.InstanceId
                        row("Id") = values.Id
                        row("InstStage") = values.InstStage
                        row("DocStage") = values.DocStage
                        row("FinishInstance") = values.FinishInstance
                        row("PublishDate") = values.PublishDate
                        row("DisplayDate") = values.DisplayDate
                        row("PublishDisplayDate") = values.PublishDisplayDate
                        row("AppealDate") = values.AppealDate
                        row("IsSimpleJustice") = values.IsSimpleJustice
                        row("IncomingNumProcessed") = values.IncomingNumProcessed
                        row("ReasonDocumentId") = values.ReasonDocumentId
                        row("Content") = values.Content
                        row("GeneralDecisionType") = values.GeneralDecisionType
                        row("DecisionType") = values.DecisionType
                        row("DecisionTypeName") = values.DecisionTypeName
                        row("ClaimSum") = values.ClaimSum
                        row("RecoverySum") = values.RecoverySum
                        row("IsStart") = values.IsStart
                        row("IsPresidiumSessionEvent") = values.IsPresidiumSessionEvent
                        row("SignatureInfo") = values.SignatureInfo
                        row("Judges") = values.Judges.Count
                        row("Declarers") = values.Declarers.Count '
                        tempString = ""

                        If values.LinkedSideIds IsNot Nothing Then
                            For Each slaverow In values.LinkedSideIds
                                If tempString = "" Then
                                    tempString = slaverow
                                Else
                                    tempString = tempString & ";" & slaverow
                                End If
                            Next
                        End If
                        row("LinkedSideIds") = tempString
                        row("FileName") = values.FileName
                        row("OriginalActFileName") = values.OriginalActFileName
                        row("AdditionalInfo") = values.AdditionalInfo
                        row("SystemDocumentType") = values.SystemDocumentType
                        row("CompensationAmount") = values.CompensationAmount
                        row("DeadlineDate") = values.DeadlineDate
                        row("CanSeeDocPostItem") = values.CanSeeDocPostItem
                        row("SimpleJusticeFileState") = values.SimpleJusticeFileState
                        row("Signer") = values.Signer
                        row("AppealedDocuments") = values.AppealedDocuments
                        row("AppealState") = values.AppealState
                        row("AppealDescription") = values.AppealDescription
                        row("Comment") = values.Comment
                        row("InstanceLevel") = values.InstanceLevel
                        row("Addressee") = values.Addressee
                        row("IsDeleted") = values.IsDeleted
                        row("DelReason") = values.DelReason
                        row("ViewsCount") = values.ViewsCount
                        row("DelDate") = values.DelDate
                        row("CanBeDeleted") = values.CanBeDeleted
                        row("DocSession") = values.DocSession
                        row("WithAttachment") = values.WithAttachment
                        row("AttachmentCount") = values.AttachmentCount
                        row("HasSignature") = values.HasSignature
                        row("AcceptMAID") = values.AcceptMAID
                        row("RosRegNum") = values.RosRegNum
                        row("Date") = values.Date1
                        row("Type") = values.Type
                        row("IsAct") = values.IsAct
                        row("HearingDate") = values.HearingDate
                        row("DocumentTypeId") = values.DocumentTypeId
                        row("ActualDate") = values.ActualDate
                        tempString = ""
                        For Each slaverow In values.ContentTypesIds
                            If tempString = "" Then
                                tempString = slaverow
                            Else
                                tempString = tempString & ";" & slaverow
                            End If
                        Next
                        row("ContentTypesIds") = tempString
                        tempString = ""
                        For Each slaverow In values.ContentTypes
                            If tempString = "" Then
                                tempString = slaverow
                            Else
                                tempString = tempString & ";" & slaverow
                            End If
                        Next
                        row("ContentTypes") = tempString
                        row("DocumentTypeName") = values.DocumentTypeName
                        row("CrocId") = values.CrocId
                        row("SourceSystem") = values.SourceSystem
                        row("HearingPlace") = values.HearingPlace
                        row("CourtTag") = values.CourtTag
                        row("CourtName") = values.CourtName
                        row("UseShortCourtName") = values.UseShortCourtName
                        dtMaster.Rows.Add(row.ItemArray)
                    Next
                    numberPage = CInt(va.Result.PagesCount)
                    errorString = ""
                End Using
                Return New Tuple(Of Integer, DataTable, String)(numberPage, dtMaster, errorString)
            Catch ex As Exception
                errorString = ex.Message
                tryCount += 1
                Threading.Thread.Sleep(1000)
            End Try
        End While
        Return New Tuple(Of Integer, DataTable, String)(numberPage, dtMaster, errorString)
    End Function
    Private Function вккк(responseBody, dtMaster)
        Dim va = JsonConvert.DeserializeObject(Of desLoad2.Root)(responseBody)
        For Each values In va.Result.Items
            Dim row As DataRow = dtMaster.NewRow()
            Dim tempString = ""
            row("CaseId") = values.CaseId
            row("InstanceId") = values.InstanceId
            row("Id") = values.Id
            row("InstStage") = values.InstStage
            row("DocStage") = values.DocStage
            row("FinishInstance") = values.FinishInstance
            row("PublishDate") = values.PublishDate
            row("DisplayDate") = values.DisplayDate
            row("PublishDisplayDate") = values.PublishDisplayDate
            row("AppealDate") = values.AppealDate
            row("IsSimpleJustice") = values.IsSimpleJustice
            row("IncomingNumProcessed") = values.IncomingNumProcessed
            row("ReasonDocumentId") = values.ReasonDocumentId
            row("Content") = values.Content
            row("GeneralDecisionType") = values.GeneralDecisionType
            row("DecisionType") = values.DecisionType
            row("DecisionTypeName") = values.DecisionTypeName
            row("ClaimSum") = values.ClaimSum
            row("RecoverySum") = values.RecoverySum
            row("IsStart") = values.IsStart
            row("IsPresidiumSessionEvent") = values.IsPresidiumSessionEvent
            row("SignatureInfo") = values.SignatureInfo
            row("Judges") = values.Judges.Count
            row("Declarers") = values.Declarers.Count '
            tempString = ""
            If values.LinkedSideIds IsNot Nothing Then
                For Each slaverow In values.LinkedSideIds
                    If tempString = "" Then
                        tempString = slaverow
                    Else
                        tempString = tempString & ";" & slaverow
                    End If
                Next
            End If
            row("LinkedSideIds") = tempString
            row("FileName") = values.FileName
            row("OriginalActFileName") = values.OriginalActFileName
            row("AdditionalInfo") = values.AdditionalInfo
            row("SystemDocumentType") = values.SystemDocumentType
            row("CompensationAmount") = values.CompensationAmount
            row("DeadlineDate") = values.DeadlineDate
            row("CanSeeDocPostItem") = values.CanSeeDocPostItem
            row("SimpleJusticeFileState") = values.SimpleJusticeFileState
            row("Signer") = values.Signer
            row("AppealedDocuments") = values.AppealedDocuments
            row("AppealState") = values.AppealState
            row("AppealDescription") = values.AppealDescription
            row("Comment") = values.Comment
            row("InstanceLevel") = values.InstanceLevel
            row("Addressee") = values.Addressee
            row("IsDeleted") = values.IsDeleted
            row("DelReason") = values.DelReason
            row("ViewsCount") = values.ViewsCount
            row("DelDate") = values.DelDate
            row("CanBeDeleted") = values.CanBeDeleted
            row("DocSession") = values.DocSession
            row("WithAttachment") = values.WithAttachment
            row("AttachmentCount") = values.AttachmentCount
            row("HasSignature") = values.HasSignature
            row("AcceptMAID") = values.AcceptMAID
            row("RosRegNum") = values.RosRegNum
            row("Date") = values.Date1
            row("Type") = values.Type
            row("IsAct") = values.IsAct
            row("HearingDate") = values.HearingDate
            row("DocumentTypeId") = values.DocumentTypeId
            row("ActualDate") = values.ActualDate
            tempString = ""
            For Each slaverow In values.ContentTypesIds
                If tempString = "" Then
                    tempString = slaverow
                Else
                    tempString = tempString & ";" & slaverow
                End If
            Next
            row("ContentTypesIds") = tempString
            tempString = ""
            For Each slaverow In values.ContentTypes
                If tempString = "" Then
                    tempString = slaverow
                Else
                    tempString = tempString & ";" & slaverow
                End If
            Next
            row("ContentTypes") = tempString
            row("DocumentTypeName") = values.DocumentTypeName
            row("CrocId") = values.CrocId
            row("SourceSystem") = values.SourceSystem
            row("HearingPlace") = values.HearingPlace
            row("CourtTag") = values.CourtTag
            row("CourtName") = values.CourtName
            row("UseShortCourtName") = values.UseShortCourtName
            dtMaster.Rows.Add(row.ItemArray)
        Next
        Return va.Result.PagesCount
    End Function

#End Region
#Region "Дессириализация"
    Class DesLoad1
        Friend Class CaseInfo
            Public Property Id As String
            Public Property CaseNumber As String
            Public Property SidesCount As Integer
            Public Property RegistrationDate As DateTime
            Public Property IsSimpleJustice As Boolean
            Public Property SimpleJusticeCode As Object
            Public Property Instances As List(Of Instance)
            Public Property Sides As Sides
            Public Property CaseTypeMCode As String
            Public Property CaseType As String
            Public Property CaseCategoryDispute As String
            Public Property CaseState As String
            Public Property SinceStart As String
            Public Property SubscriptionId As Object
        End Class

        Friend Class Court
            Public Property Id As String
            Public Property Tag As String
            Public Property Name As String
            Public Property Url As String
            Public Property IsCroc As Boolean
        End Class

        Friend Class Court2
            Public Property Tag As String
        End Class

        Friend Class Declarer
            Public Property Id As String
            Public Property Name As String
            Public Property Address As Object
            Public Property INN As Object
            Public Property BirthDate As Object
            Public Property BirthPlace As Object
            Public Property Snils As Object
            Public Property SideType As Integer
            Public Property OrganizationForm As Object
            Public Property SubjectCategories As List(Of Object)
            Public Property ContainsBflResolution As Boolean
        End Class

        Friend Class FinalDocument
            Public Property Id As String
            Public Property IsDeleted As Boolean
            Public Property DocumentTypeId As String
            Public Property IsCroc As Boolean
            Public Property ContentTypes As List(Of String)
            Public Property ContentTypesStr As List(Of String)
            Public Property FileName As String
            Public Property DocumentDate As DateTime
            Public Property PublishDate As DateTime
            Public Property DecisionTypeName As String
            Public Property CanBeDownloaded As Boolean
            Public Property HasFile As Boolean
            Public Property SignatureInfo As List(Of SignatureInfo)
            Public Property OriginalActFileName As String
            Public Property DocumentDateString As String
        End Class

        Friend Class Instance
            Public Property Id As String
            Public Property InstanceNumber As String
            Public Property InstanceLevel As Integer
            Public Property InstanceWeight As Double
            Public Property Court As Court
            Public Property Judges As List(Of Judge)
            Public Property StartDocument As StartDocument
            Public Property FinalDocument As FinalDocument
            Public Property IsFinished As Boolean
            Public Property SessionStateString As String
            Public Property NextInstanceEvent As String
            Public Property RegistrationDate As DateTime
            Public Property IncomingDate As DateTime
            Public Property IsFromMainCase As Boolean?
            Public Property LiveVideos As List(Of Object)
            Public Property ActionWithVideo As Integer
            Public Property SessionState As Integer
            Public Property IncommingNumber As String
        End Class

        Friend Class Judge
            Public Property Id As String
            Public Property Name As String
            Public Property FinalJudges As Boolean
        End Class

        Friend Class Participant
            Public Property Id As String
            Public Property Name As String
            Public Property Address As String
            Public Property INN As String
            Public Property BirthDate As Object
            Public Property BirthPlace As String
            Public Property Snils As String
            Public Property SideType As Integer
            Public Property OrganizationForm As String
            Public Property SubjectCategories As List(Of String)
            Public Property ContainsBflResolution As Boolean
        End Class

        Friend Class Result
            Public Property CaseInfo As CaseInfo
            Public Property AdminInfo As Object
            Public Property CanDownloadArchives As Boolean
            Public Property SjDocsAvailable As Boolean
            Public Property CaseMaterialsAvailable As Boolean
            Public Property IsSupervisor As Boolean
            Public Property IsAdminDeleted As Boolean
            Public Property DocTypes As Object
            Public Property CaseTypeCode As String
            Public Property Courts As List(Of Court)
            Public Property CourtTags As List(Of String)
            Public Property IsNeedMobileVer As Boolean
        End Class

        Friend Class Root
            Public Property Result As Result
            Public Property Message As String
            Public Property Success As Boolean
            Public Property ServerDate As DateTime
        End Class

        Friend Class Sides
            Public Property Participants As List(Of Participant)
            Public Property IsUserSupervisor As Boolean
            Public Property IsAdminDeleted As Boolean
            Public Property CaseId As String
        End Class

        Friend Class SignatureInfo
            Public Property Id As String
            Public Property Organization As String
            Public Property Status As Integer
            Public Property Owner As String
            Public Property OwnerPost As String
            Public Property DateCheck As DateTime
            Public Property DateValidUntil As Object
            Public Property EffectiveDate As Object
            Public Property OwnerEmail As Object
            Public Property OwnerAddress As Object
            Public Property Issuer As String
            Public Property VerifyErrorMessage As Object
            Public Property SignatureId As Integer
            Public Property EffectiveDateString As String
            Public Property DateValidUntilString As String
            Public Property DateCheckString As String
        End Class

        Friend Class StartDocument
            Public Property Id As String
            Public Property DocumentTypeId As String
            Public Property DocumentType As String
            Public Property DocumentContentTypeId As String
            Public Property DocumentContentType As String
            Public Property DocLevel As Integer
            Public Property DocumentDate As DateTime
            Public Property CaseId As String
            Public Property InstanceId As String
            Public Property IsKodeks As Integer
            Public Property IncomingNum As String
            Public Property Declarers As List(Of Declarer)
            Public Property FileName As String
        End Class

    End Class
    Class desLoad2
        Friend Class Declarer
            Public Property Id As String
            Public Property OrganizationId As String
            Public Property Organization As String
            Public Property Address As String
            Public Property Inn As String
            Public Property Ogrn As Object
            Public Property Type As Integer
        End Class

        Friend Class Item
            Public Property CaseId As String
            Public Property InstanceId As String
            Public Property Id As String
            Public Property InstStage As Integer
            Public Property DocStage As Integer
            Public Property FinishInstance As Integer
            Public Property PublishDate As DateTime?
            Public Property DisplayDate As String
            Public Property PublishDisplayDate As String
            Public Property AppealDate As DateTime?
            Public Property IsSimpleJustice As Boolean
            Public Property IncomingNumProcessed As String
            Public Property ReasonDocumentId As String
            Public Property Content As Object
            Public Property GeneralDecisionType As Integer
            Public Property DecisionType As String
            Public Property DecisionTypeName As String
            Public Property ClaimSum As Double
            Public Property RecoverySum As Double
            Public Property IsStart As Boolean
            Public Property IsPresidiumSessionEvent As Boolean
            Public Property SignatureInfo As List(Of SignatureInfo)
            Public Property Judges As List(Of Judge)
            Public Property Declarers As List(Of Declarer)
            Public Property LinkedSideIds As List(Of String)
            Public Property FileName As String
            Public Property OriginalActFileName As String
            Public Property AdditionalInfo As String
            Public Property SystemDocumentType As Integer
            Public Property CompensationAmount As Integer
            Public Property DeadlineDate As Object
            Public Property CanSeeDocPostItem As Boolean
            Public Property SimpleJusticeFileState As Integer
            Public Property Signer As Object
            Public Property AppealedDocuments As Object
            Public Property AppealState As Object
            Public Property AppealDescription As String
            Public Property Comment As String
            Public Property InstanceLevel As Integer
            Public Property Addressee As Object
            Public Property IsDeleted As Boolean
            Public Property DelReason As Object
            Public Property ViewsCount As Integer
            Public Property DelDate As Object
            Public Property CanBeDeleted As Boolean
            Public Property DocSession As Object
            Public Property WithAttachment As Boolean
            Public Property AttachmentCount As Integer
            Public Property HasSignature As Boolean
            Public Property AcceptMAID As Object
            Public Property RosRegNum As Object
            Public Property Date1 As DateTime
            Public Property Type As Integer
            Public Property IsAct As Boolean
            Public Property HearingDate As DateTime?
            Public Property DocumentTypeId As String
            Public Property ActualDate As DateTime
            Public Property ContentTypesIds As List(Of String)
            Public Property ContentTypes As List(Of String)
            Public Property DocumentTypeName As String
            Public Property CrocId As Object
            Public Property SourceSystem As Integer
            Public Property HearingPlace As String
            Public Property CourtTag As String
            Public Property CourtName As String
            Public Property UseShortCourtName As Boolean
        End Class

        Friend Class Judge
            Public Property Id As String
            Public Property Name As String
            Public Property Role As String
            Public Property Group As String
        End Class

        Friend Class Result
            Public Property Page As Integer
            Public Property PageSize As Integer
            Public Property TotalCount As Integer
            Public Property PagesCount As Integer
            Public Property Items As List(Of Item)
            Public Property Count As Integer
        End Class

        Friend Class Root
            Public Property Result As Result
            Public Property Message As String
            Public Property Success As Boolean
            Public Property ServerDate As DateTime
        End Class

        Friend Class SignatureInfo
            Public Property Id As String
            Public Property Organization As String
            Public Property Status As Integer
            Public Property Owner As String
            Public Property OwnerPost As Object
            Public Property DateCheck As DateTime
            Public Property DateValidUntil As Object
            Public Property EffectiveDate As Object
            Public Property OwnerEmail As Object
            Public Property OwnerAddress As Object
            Public Property Issuer As String
            Public Property VerifyErrorMessage As Object
            Public Property SignatureId As Integer
            Public Property EffectiveDateString As String
            Public Property DateValidUntilString As String
            Public Property DateCheckString As String
        End Class

    End Class


#End Region
End Class




