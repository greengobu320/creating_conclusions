Imports System.IO
Imports System.IO.Compression

Module Gzip
    Function CompressText(text As String) As Byte()
        Using memoryStream As New MemoryStream()
            Using gzipStream As New GZipStream(memoryStream, CompressionMode.Compress)
                Using writer As New StreamWriter(gzipStream)
                    writer.Write(text)
                End Using
            End Using
            Return memoryStream.ToArray()
        End Using
    End Function
End Module
