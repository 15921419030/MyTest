Imports System.Collections.Generic
Imports System.Text
Imports System.Configuration
Imports MySql.Data.MySqlClient
Imports System.Data


    Partial Public Class MySqlHelper
        Inherits HelperBase
        Public Sub New()
            ConnectionString = default_connection_str
            Connection = New MySqlConnection()
            Command = Connection.CreateCommand()
        End Sub

        Public Sub New(ByVal ConnectionStringsIndex As Integer)
            ConnectionString = ConfigurationManager.ConnectionStrings(ConnectionStringsIndex).ConnectionString
            Connection = New MySqlConnection()
            Command = Connection.CreateCommand()
        End Sub

        Public Sub New(ByVal ConnectionString As String)
            Me.ConnectionString = ConnectionString
            Connection = New MySqlConnection()
            Command = Connection.CreateCommand()
        End Sub

        Public Overrides Sub Open()
            MyBase.Open()
        End Sub

        Public Function AddParameter(ByVal ParameterName As String, ByVal type As MySqlDbType, ByVal value As Object) As MySqlParameter
            Return AddParameter(ParameterName, type, value, ParameterDirection.Input)
        End Function

        Public Function AddParameter(ByVal ParameterName As String, ByVal type As MySqlDbType, ByVal value As Object, ByVal direction As ParameterDirection) As MySqlParameter
            Dim param As New MySqlParameter(ParameterName, type)
            param.Value = value
            param.Direction = direction
            Command.Parameters.Add(param)
            Return param
        End Function

        Public Function AddParameter(ByVal ParameterName As String, ByVal type As MySqlDbType, ByVal size As Integer, ByVal value As Object) As MySqlParameter
            Return AddParameter(ParameterName, type, size, value, ParameterDirection.Input)
        End Function

        Public Function AddParameter(ByVal ParameterName As String, ByVal type As MySqlDbType, ByVal size As Integer, ByVal value As Object, ByVal direction As ParameterDirection) As MySqlParameter
            Dim param As New MySqlParameter(ParameterName, type, size)
            param.Direction = direction
            param.Value = value
            Command.Parameters.Add(param)
            Return param
        End Function

        Public Sub AddRangeParameters(ByVal parameters As MySqlParameter())
            Command.Parameters.AddRange(parameters)
        End Sub


    End Class
