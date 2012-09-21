Imports System.Data.OleDb
Imports System.Reflection
Imports System.IO
Imports System.Data

    ''' <summary>
    ''' Microsoft Access database operations class.
    ''' </summary>
    Partial Public Class AccessHelper
        Inherits HelperBase
        Private _accessFPath As String = get_defualt_dbpath()

        Public Overridable Property AccessFPath() As String
            Get
                Return _accessFPath
            End Get
            Set(ByVal value As String)
                _accessFPath = value
            End Set
        End Property

        Public Sub New()
            Me.Connection = New OleDbConnection()
            Command = Connection.CreateCommand()
        End Sub

        Public Overrides Sub Open()
            MyBase.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0;data source=" & AccessFPath
            MyBase.Open()
        End Sub

        Public Sub New(ByVal accessfpath As String)
            Me.AccessFPath = accessfpath
            Open()
        End Sub

        Public Function AddParameter(ByVal ParameterName As String, ByVal type As OleDbType, ByVal value As Object) As OleDbParameter
            Return AddParameter(ParameterName, type, value, ParameterDirection.Input)
        End Function

        Public Function AddParameter(ByVal ParameterName As String, ByVal type As OleDbType, ByVal value As Object, ByVal direction As ParameterDirection) As OleDbParameter
            Dim param As New OleDbParameter(ParameterName, type)
            param.Value = value
            param.Direction = direction
            Command.Parameters.Add(param)
            Return param
        End Function

        Public Function AddParameter(ByVal ParameterName As String, ByVal type As OleDbType, ByVal size As Integer, ByVal value As Object) As OleDbParameter
            Return AddParameter(ParameterName, type, size, value, ParameterDirection.Input)
        End Function

        Public Function AddParameter(ByVal ParameterName As String, ByVal type As OleDbType, ByVal size As Integer, ByVal value As Object, ByVal direction As ParameterDirection) As OleDbParameter
            Dim param As New OleDbParameter(ParameterName, type, size)
            param.Direction = direction
            param.Value = value
            Command.Parameters.Add(param)
            Return param
        End Function

        Public Sub AddRangeParameters(ByVal parameters As OleDbParameter())
            Command.Parameters.AddRange(parameters)
        End Sub


    End Class


