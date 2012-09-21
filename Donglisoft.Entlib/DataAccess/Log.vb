
Namespace Common
    Public Enum EnumLogType
        Common = 0
        Database = 1
    End Enum
    ''' <summary>
    ''' 应用程序日志处理类
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Log
        
        Public LogID As Integer
        Public LogDate As DateTime
        Public LogType As Integer
        Public SqlText As String
        Public ModuleName As String
        Public LogInfo As String

        Public Shared Sub WriteLog(ByVal MyLog As Log)
            '
            'If Not MyLog Is Nothing Then MyLog.Save()
            Debug.WriteLine(MyLog.ModuleName & ":" & MyLog.LogInfo)
        End Sub

        Public Shared Sub WriteLog_Common(ByVal ModuleName As String, ByVal LogInfo As String)
            'Dim MyLog As New Log()
            'MyLog.LogDate = Now
            'MyLog.LogType = EnumLogType.Common
            'MyLog.ModuleName = ModuleName
            'MyLog.LogInfo = LogInfo
            'MyLog.Save()
            Debug.WriteLine(ModuleName & ":" & LogInfo)
        End Sub

        Public Shared Sub WriteLog_Database(ByVal ModuleName As String, ByVal SqlText As String, ByVal LogInfo As String)
            'Dim MyLog As New Log()
            'MyLog.LogDate = Now
            'MyLog.LogType = EnumLogType.Database
            'MyLog.ModuleName = ModuleName
            'MyLog.LogInfo = LogInfo
            'MyLog.SqlText = SqlText
            'MyLog.Save()
            Debug.WriteLine(ModuleName & ":" & LogInfo & ":" & SqlText)
        End Sub

        Public Shared Function GetLog(ByVal LogID As Integer) As Log
            'Dim MyLog As New Log()
            'MyLog.LogID = LogID
            'MyLog.Reload()
            'If MyLog.IsPersisted Then
            '    Return MyLog
            'Else
            '    Return Nothing
            'End If
            Return Nothing
        End Function

    End Class
End Namespace
