Imports Spire.Pdf
Imports Spire.Pdf.Texts
Module pdfToText
    Function ExtractTextFromPdf(file As Byte()) As Dictionary(Of String, String)
        Dim dic As New Dictionary(Of String, String)
        dic.Add("result", "error")
        dic.Add("value", Nothing)
        Try
            Dim doc As New PdfDocument
            doc.LoadFromBytes(file)
            Dim text = ExtractText(doc)
            dic("result") = "ok"
            dic("value") = text
        Catch ex As Exception
            dic("result") = "error"
            dic("value") = ex.Message
        End Try
        Return dic
    End Function

    Private Function ExtractText(doc As PdfDocument) As String
        Dim text As String = String.Empty
        For Each page As PdfPageBase In doc.Pages
            Dim textExtractor As New PdfTextExtractor(page)
            Dim extractOptions As New PdfTextExtractOptions()
            extractOptions.IsExtractAllText = True
            If text = String.Empty Then
                text = textExtractor.ExtractText(extractOptions)
            Else
                text &= vbLf & textExtractor.ExtractText(extractOptions)
            End If
        Next
        Return text
    End Function
End Module
