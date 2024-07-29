Public Class settingsSQL
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveSetting("NewBa", "Preference", "odbcName", TextBox1.Text)
        Me.Close()

    End Sub
End Class