Imports System.Collections.Generic
Imports System.Text
Imports System.Data.OracleClient
Imports System.Configuration
Imports System.Data



''' <summary>
''' SqlServer database operations class.
''' </summary>
Partial Public Class OracleHelper
    Inherits HelperBase
    Public Sub New()
        ConnectionString = default_connection_str
        Connection = New OracleConnection()
        Command = Connection.CreateCommand()
    End Sub

    Public Sub New(ByVal ConnectionStringsIndex As Integer)
        ConnectionString = ConfigurationManager.ConnectionStrings(ConnectionStringsIndex).ConnectionString
        Connection = New OracleConnection()
        Command = Connection.CreateCommand()
    End Sub

    Public Sub New(ByVal ConnectionString As String)
        Me.ConnectionString = ConnectionString
        Connection = New OracleConnection()
        Command = Connection.CreateCommand()
    End Sub

    Public Overrides Sub Open()
        MyBase.Open()
    End Sub

    Public Function AddParameter(ByVal ParameterName As String, ByVal type As OracleType, ByVal value As Object) As OracleParameter
        Return AddParameter(ParameterName, type, value, ParameterDirection.Input)
    End Function

    Public Function AddParameter(ByVal ParameterName As String, ByVal type As OracleType, ByVal value As Object, ByVal direction As ParameterDirection) As OracleParameter
        Dim param As New OracleParameter(ParameterName, type)
        param.Value = value
        param.Direction = direction
        Command.Parameters.Add(param)
        Return param
    End Function

    Public Function AddParameter(ByVal ParameterName As String, ByVal type As OracleType, ByVal size As Integer, ByVal value As Object) As OracleParameter
        Return AddParameter(ParameterName, type, size, value, ParameterDirection.Input)
    End Function

    Public Function AddParameter(ByVal ParameterName As String, ByVal type As OracleType, ByVal size As Integer, ByVal value As Object, ByVal direction As ParameterDirection) As OracleParameter
        Dim param As New OracleParameter(ParameterName, type, size)
        param.Direction = direction
        param.Value = value
        Command.Parameters.Add(param)
        Return param
    End Function

    Public Sub AddRangeParameters(ByVal parameters As OracleParameter())
        Command.Parameters.AddRange(parameters)
    End Sub
End Class

