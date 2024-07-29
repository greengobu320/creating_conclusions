Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic.Devices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Security.Policy

Module sql
    Friend Sub SaveToDatabase(fileName As String, compressedText As Byte(), status As String, subject As String)
        Dim connectionString As String = Form1.odbcName
        Using connection As New OdbcConnection(connectionString)
            connection.Open()
            Dim query As String = "INSERT INTO fileTable (status, subject, fileName, fileSave) VALUES (@status, @subject, @fileName, @fileSave)"
            Using command As New OdbcCommand(query, connection)
                command.Parameters.AddWithValue("@status", status)
                command.Parameters.AddWithValue("@subject", subject)
                command.Parameters.AddWithValue("@fileName", fileName)
                command.Parameters.AddWithValue("@fileSave", compressedText)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Friend Function CheckAndUpdateDataPeriodTable(Cour As String, dateFrom As String, dateTo As String, odbcName As String) As Integer
        Dim result As Integer = 3
        Dim connectionString As String = $"DSN={odbcName}"
        ' Console.WriteLine($"SELECT  1
        '    FROM PeriodTable -- WITH (UPDLOCK, HOLDLOCK) 
        '  WHERE (Cour = '{Cour}' AND dateFrom = '{dateFrom}' AND dateTo = '{dateTo}') and (dateLoad > DATEADD(DAY, -2, GETDATE()) or (statusLoad=0 or statusLoad = null))")
        Dim sql As String = $"DECLARE @result INT;
                               BEGIN TRANSACTION;
                                IF EXISTS ( SELECT 1 
                                    FROM PeriodTable -- WITH (UPDLOCK, HOLDLOCK) 
                                    WHERE (Cour = '{Cour}' AND dateFrom = '{dateFrom}' AND dateTo = '{dateTo}') and (dateLoad < DATEADD(DAY, -2, GETDATE()) or (statusLoad=0 or statusLoad = null)))
                                        BEGIN
                                            SET @result = 1;
                                        END
                                    ELSE
                                        BEGIN
                                            SET @result = 0;
                                        END
                                COMMIT;
                            IF @result = 0        
                                IF EXISTS (SELECT 1 
                                FROM PeriodTable 
                                WHERE Cour='{Cour}' 
                                AND dateFrom='{dateFrom}' 
                                AND dateTo='{dateTo}')
                                    BEGIN			
                                        UPDATE PeriodTable 
                                        SET dateLoad = GETDATE(), statusLoad=2, subject='{My.Computer.Name.ToString}', messageStatus=''  
                                        WHERE (Cour='{Cour}' AND dateFrom='{dateFrom}'AND dateTo='{dateTo}');
                                    END
                                ELSE
                                    BEGIN
	                                    SET @result = 2;
                                        INSERT INTO PeriodTable (dateFrom, dateTo, Cour, dateLoad, statusLoad,subject, messageStatus) 
                                        VALUES ('{dateFrom}', '{dateTo}', '{Cour}', GETDATE(), 2,'{My.Computer.Name.ToString}','');
                                    END
                            SELECT @result;"
        Try
            Using connection As New OdbcConnection(connectionString)
                Dim command As New OdbcCommand(sql, connection)
                connection.Open()
                result = command.ExecuteScalar()
            End Using
        Catch ex As Exception
            Console.WriteLine("-------------")
            Console.WriteLine(sql)
            Console.WriteLine($"dsn-{odbcName}")
            Console.WriteLine(ex.Message)
            Console.WriteLine("-------------")
        End Try

        Return CInt(result)
    End Function
    Friend Function CheckAndUpdateDataDeloTable(numberDelo As String, odbcName As String) As Integer
        Dim result As Integer = 3
        Dim connectionString As String = $"DSN={odbcName}"
        Dim sql As String = $"
DECLARE @result INT;
	BEGIN TRANSACTION;
		IF EXISTS ( SELECT 1 
			FROM DeloTable --WITH (UPDLOCK, HOLDLOCK) 
			WHERE (numberDelo = '{numberDelo}' AND ((dateLoad < DATEADD(DAY, -2, GETDATE())) OR (statusLoad=0 or statusLoad = null))))
			BEGIN
				SET @result = 1;
			END
		ELSE
			BEGIN
				SET @result = 0;
			END
	COMMIT;
	IF @result = 0        
        IF EXISTS (
            SELECT 1 
            FROM DeloTable 
            WHERE numberDelo='{numberDelo}' 
					)
			BEGIN			
				UPDATE DeloTable SET dateLoad = GETDATE(), statusLoad=2, messageStatus=''
				WHERE numberDelo='{numberDelo}' ;
			END
		ELSE
			BEGIN
				SET @result = 2;
				INSERT INTO DeloTable (numberDelo, dateLoad, subject, statusLoad, messageStatus) 
				VALUES ('{numberDelo}', GETDATE(), '{My.Computer.Name.ToString}', 2, '');
			END
SELECT @result;"
        Try
            Using connection As New OdbcConnection(connectionString)
                Dim command As New OdbcCommand(sql, connection)
                connection.Open()
                result = command.ExecuteScalar()
            End Using
        Catch ex As Exception
            Console.WriteLine("-------------")
            Console.WriteLine(sql)
            Console.WriteLine(ex.Message)
            Console.WriteLine("-------------")
        End Try
        Return CInt(result)
    End Function
    Friend Sub sqlCommand(sql As String, odbcName As String)
        Dim connectionString As String = $"DSN={odbcName}"
        Using connection As New OdbcConnection(connectionString)
            Dim command As New OdbcCommand(sql, connection)
            connection.Open()
            command.ExecuteNonQuery()
        End Using
    End Sub
    Private Function FormatDate(inputDate As Object) As String
        If IsDBNull(inputDate) OrElse String.IsNullOrWhiteSpace(inputDate.ToString()) Then
            Return "NULL" ' или какое-то другое значение по умолчанию
        End If

        Dim dateValue As DateTime
        If DateTime.TryParse(inputDate.ToString(), dateValue) Then
            Return "'" & dateValue.ToString("yyyy-MM-dd HH:mm:ss") & "'" ' возвращаем в нужном формате
        Else
            ' Если формат даты введен неправильно, вернуть NULL или выбросить ошибку
            Return "NULL"
        End If
    End Function

    Friend Sub InsertIntoFileTable(getLinkResult As Dictionary(Of String, Object), key As String, fileName As String, odbcName As String)
        Dim connectionString As String = $"DSN={odbcName}"
        Using connection As New OdbcConnection(connectionString)
            connection.Open()
            Dim status As Integer = 1
            Dim command As New OdbcCommand("INSERT INTO [dbo].[fileTable] ([keyDoc], [status], [subject], [fileName], [fileSave]) VALUES (?, ?, ?, ?, ?)", connection)
            Dim fileData As Byte() = Nothing
            If getLinkResult("result") = "ok" Then
                status = 0
                fileData = getLinkResult("value")
            End If
            Dim subject As String = My.Computer.Name.ToString
            command.Parameters.Clear()
            command.Parameters.AddWithValue("@keyDoc", key)
            command.Parameters.AddWithValue("@status", status)
            command.Parameters.AddWithValue("@subject", subject)
            command.Parameters.AddWithValue("@fileName", fileName)
            command.Parameters.AddWithValue("@fileSave", fileData)
            command.ExecuteNonQuery()
            connection.Close()
        End Using
    End Sub
    Friend Function CheckFileDownload(key As String, odbcName As String)
        Dim connectionString As String = $"DSN={odbcName}"
        Using connection As New OdbcConnection(connectionString)
            connection.Open()
            Dim existsCommand As New OdbcCommand("SELECT COUNT(*) FROM DocDownload WHERE keyDoc = ?", connection)
            existsCommand.Parameters.AddWithValue("@key", key)
            Dim exists As Integer = Convert.ToInt32(existsCommand.ExecuteScalar())
            Return exists
        End Using

    End Function

    Public Sub InsertCaseNumbers(row As DataRow, odbcName As String)
        Dim connectionString As String = $"DSN={odbcName}"
        Using connection As New OdbcConnection(connectionString)
            connection.Open()
            Dim commandText As String = $"INSERT INTO [CaseInformation] (Plaintiff, Defendant, DefendantAddress, DefendantINN, Judge, CurrentInstance, CaseNumber, CaseDate, CaseLink, Result) " &
                                         $"VALUES ('{row("Истец")}', '{row("Ответчик")}', '{row("Адрес Ответчик")}', '{row("ИНН Ответчик")}', '{row("Судья")}', '{row("Текущая инстанция")}', " &
                                         $"'{row("Номер дела")}', '{row("Дата дела"):yyyy-MM-dd}', '{row("Ссылка на дело")}', '{row("результат")}')"

            Using command As New OdbcCommand(commandText, connection)
                command.ExecuteNonQuery()
            End Using

        End Using
    End Sub

    Friend Function CalcHash(ByVal input As String) As String
        Dim md5 As MD5 = MD5.Create()
        Dim inputBytes As Byte() = Encoding.UTF8.GetBytes(input)
        Dim hashBytes As Byte() = md5.ComputeHash(inputBytes)
        Dim sb As New StringBuilder()
        For i As Integer = 0 To hashBytes.Length - 1
            sb.Append(hashBytes(i).ToString("x2"))
        Next
        Dim key As String = sb.ToString()
        key = $"{key.Substring(0, 8)}-{key.Substring(8, 4)}-{key.Substring(12, 4)}-{key.Substring(16, 4)}-{key.Substring(20, 12)}"

        Return key
    End Function

    Friend Sub insertOrSlaveDate(dtMaster As DataTable, odbcName As String)
        Dim connectionString As String = $"DSN={odbcName}"

        Using connection As New OdbcConnection(connectionString)
            connection.Open()

            For Each row As DataRow In dtMaster.Rows
                Dim key As String = CalcHash($"{dtMaster.TableName}{row("CaseId")}{row("InstanceId")}{row("DocumentTypeId")}{row("ContentTypesIds")}{row("PublishDisplayDate")}{row("FileName")}")
                Dim existsCommand As New OdbcCommand("SELECT COUNT(*) FROM LegalCases WHERE keyDoc = '" & key & "'", connection)
                Dim exists As Integer = Convert.ToInt32(existsCommand.ExecuteScalar())

                If exists = 0 Then
                    Dim insertQuery As String = "
    INSERT INTO LegalCases
    (CaseNumber, keyDoc, DateLoad, subject, CaseId, InstanceId, [Id], InstStage,
     DocStage, FinishInstance, PublishDate, DisplayDate, PublishDisplayDate, 
     AppealDate, IsSimpleJustice, IncomingNumProcessed, ReasonDocumentId, 
     Content, GeneralDecisionType, DecisionType, DecisionTypeName, ClaimSum, 
     RecoverySum, IsStart, IsPresidiumSessionEvent, SignatureInfo, Judges, 
     Declarers, LinkedSideIds, FileName, OriginalActFileName, AdditionalInfo, 
     SystemDocumentType, CompensationAmount, DeadlineDate, CanSeeDocPostItem, 
     SimpleJusticeFileState, Signer, AppealedDocuments, AppealState, 
     AppealDescription, Comment, InstanceLevel, Addressee, IsDeleted, 
     DelReason, ViewsCount, DelDate, CanBeDeleted, DocSession, WithAttachment, 
     AttachmentCount, HasSignature, AcceptMAID, RosRegNum, Date, Type, 
     IsAct, HearingDate, DocumentTypeId, ActualDate, ContentTypesIds, 
     ContentTypes, DocumentTypeName, CrocId, SourceSystem, HearingPlace, 
     CourtTag, CourtName, UseShortCourtName)
    VALUES 
    ('" & dtMaster.TableName & "', '" & key & "', '" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "', '" & My.Computer.Name.ToString() & "', '" &
     row("CaseId") & "', '" & row("InstanceId") & "', '" & row("Id") & "', 
     '" & row("InstStage") & "', '" & row("DocStage") & "', '" & row("FinishInstance") & "', 
     " & FormatDate(row("PublishDate")) & ", " & FormatDate(row("DisplayDate")) & ", " & FormatDate(row("PublishDisplayDate")) & ", 
     " & FormatDate(row("AppealDate")) & ", " & If(row("IsSimpleJustice").ToString() = "True", 1, 0) & ", 
     '" & row("IncomingNumProcessed") & "', '" & row("ReasonDocumentId") & "', 
     '" & row("Content") & "', '" &
     row("GeneralDecisionType") & "', '" & row("DecisionType") & "', '" & row("DecisionTypeName") & "', 
     '" & row("ClaimSum").replace(",", ".") & "', '" & row("RecoverySum").replace(",", ".") & "', " &
     If(row("IsStart").ToString() = "True", 1, 0) & ", " &
     If(row("IsPresidiumSessionEvent").ToString() = "True", 1, 0) & ", '" &
     row("SignatureInfo") & "', '" & row("Judges") & "', '" & row("Declarers") & "', 
     '" & row("LinkedSideIds") & "', '" & row("FileName") & "', '" &
     row("OriginalActFileName") & "', '" & row("AdditionalInfo") & "', 
     '" & row("SystemDocumentType") & "', 
     '" & row("CompensationAmount") & "', " & FormatDate(row("DeadlineDate")) & ", 
     " & If(row("CanSeeDocPostItem").ToString() = "True", 1, 0) & ", '" & row("SimpleJusticeFileState") & "', 
     '" & row("Signer") & "', '" & row("AppealedDocuments") & "', 
     '" & row("AppealState") & "', '" & row("AppealDescription") & "', 
     '" & row("Comment") & "', '" & row("InstanceLevel") & "', 
     '" & row("Addressee") & "', " &
     If(row("IsDeleted").ToString() = "True", 1, 0) & ", 
     '" & row("DelReason") & "', '" & row("ViewsCount") & "', 
     " & FormatDate(row("DelDate")) & ", " & If(row("CanBeDeleted").ToString() = "True", 1, 0) & ", 
     '" & row("DocSession") & "', " & If(row("WithAttachment").ToString() = "True", 1, 0) & ", 
     '" & row("AttachmentCount") & "', " &
     If(row("HasSignature").ToString() = "True", 1, 0) & ", 
     '" & row("AcceptMAID") & "', '" & row("RosRegNum") & "', 
     '" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "', 
     '" & row("Type") & "', " &
     If(row("IsAct").ToString() = "True", 1, 0) & ", 
     " & FormatDate(row("HearingDate")) & ", '" & row("DocumentTypeId") & "', 
     " & FormatDate(row("ActualDate")) & ", '" & row("ContentTypesIds") & "', 
     '" & row("ContentTypes") & "', '" & row("DocumentTypeName") & "', 
     '" & row("CrocId") & "', '" & row("SourceSystem") & "', 
     '" & row("HearingPlace") & "', '" & row("CourtTag") & "', 
     '" & row("CourtName") & "', " & If(row("UseShortCourtName").ToString() = "True", 1, 0) & ")"

                    Using command As New OdbcCommand(insertQuery, connection)
                        Try
                            command.ExecuteNonQuery()
                        Catch ex As Exception
                            ' Output error message with SQL query
                            Console.WriteLine($"Ошибка при выполнении запроса: {insertQuery}")
                            Console.WriteLine($"Описание ошибки: {ex.Message}")
                        End Try
                    End Using
                End If
            Next
        End Using
    End Sub

    Friend Sub InsertIntoDocDownload(numberDelo As String, caseId As String, instanceId As String, documentTypeId As String, contentTypesIds As String, publichDisplayDate As DateTime, fileName As String, keyDoc As String, subject As String, status As Integer, messageStatus As String, odbcName As String)
        Dim connectionString As String = $"DSN={odbcName}"
        Using connection As New OdbcConnection(connectionString)
            connection.Open()

            Dim query As String = "INSERT INTO [dbo].[DocDownload] ([numberDelo], [CaseId], [InstanceId], [DocumentTypeId], [ContentTypesIds], [PublichDisplayDate], [FileName], [KeyDoc], [subject], [status], [messageStatus], [dateLoad]) " &
                              "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"

            Using command As New OdbcCommand(query, connection)
                ' Adding parameters to the query
                command.Parameters.AddWithValue("?", numberDelo)
                command.Parameters.AddWithValue("?", caseId)
                command.Parameters.AddWithValue("?", instanceId)
                command.Parameters.AddWithValue("?", documentTypeId)
                command.Parameters.AddWithValue("?", contentTypesIds)
                command.Parameters.AddWithValue("?", publichDisplayDate)
                command.Parameters.AddWithValue("?", fileName)
                command.Parameters.AddWithValue("?", keyDoc)
                command.Parameters.AddWithValue("?", subject)
                command.Parameters.AddWithValue("?", status)
                command.Parameters.AddWithValue("?", messageStatus)
                command.Parameters.AddWithValue("?", Now)

                Try
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    ' Handle exceptions, e.g., log them
                    Console.WriteLine($"Error inserting data into DocDownload: {ex.Message}")
                End Try
            End Using

            connection.Close()
        End Using
    End Sub
End Module
