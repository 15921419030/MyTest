Imports System.Collections.Generic
Imports System.Text
Imports System.Data.OracleClient
Imports System.Data
Imports System.Configuration
Imports System.Collections

Partial Public Class OracleHelper

    Public Shared ReadOnly default_connection_str As String = ConfigurationManager.ConnectionStrings("OracleHelper").ConnectionString

    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

    Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Integer
        Dim cmd As New OracleCommand()
        Using conn As New OracleConnection(connectionString)
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function

    Public Shared Function GetConnection() As OracleConnection
        Return New OracleConnection(default_connection_str)
    End Function

    Public Shared Function ExecuteNonQuery(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteNonQuery(ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteNonQueryProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As OracleParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteNonQuery(ByVal connection As OracleConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Integer
        Dim cmd As New OracleCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim val As Integer = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteNonQuery(ByVal trans As OracleTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Integer
        Dim cmd As New OracleCommand()
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters)
        Dim val As Integer = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteReader(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As OracleDataReader
        Dim cmd As New OracleCommand()
        Dim conn As New OracleConnection(connectionString)
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim rdr As OracleDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            Return rdr
        Catch
            conn.Close()
            Throw
        End Try
    End Function

    Public Shared Function ExecuteReader(ByVal conn As OracleConnection, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As OracleDataReader
        Return ExecuteReader(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteReader(ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As OracleDataReader
        Return ExecuteReader(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteReaderProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As OracleParameter()) As OracleDataReader
        Return ExecuteReader(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteReader(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As OracleDataReader
        Return ExecuteReader(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Object
        Dim cmd As New OracleCommand()
        Using connection As New OracleConnection(connectionString)
            PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function

    Public Shared Function ExecuteScalar(ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Object
        Return ExecuteScalar(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalarProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As OracleParameter()) As Object
        Return ExecuteScalar(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Object
        Return ExecuteScalar(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal connection As OracleConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As Object
        Dim cmd As New OracleCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim val As Object = cmd.ExecuteScalar()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Sub CacheParameters(ByVal cacheKey As String, ByVal ParamArray commandParameters As OracleParameter())
        parmCache(cacheKey) = commandParameters
    End Sub

    Public Shared Function GetCachedParameters(ByVal cacheKey As String) As OracleParameter()
        Dim cachedParms As OracleParameter() = DirectCast(parmCache(cacheKey), OracleParameter())
        If cachedParms Is Nothing Then
            Return Nothing
        End If
        Dim clonedParms As OracleParameter() = New OracleParameter(cachedParms.Length - 1) {}
        Dim i As Integer = 0, j As Integer = cachedParms.Length
        While i < j
            clonedParms(i) = DirectCast(DirectCast(cachedParms(i), ICloneable).Clone(), OracleParameter)
            i += 1
        End While
        Return clonedParms
    End Function

    Private Shared Sub PrepareCommand(ByVal cmd As OracleCommand, ByVal conn As OracleConnection, ByVal trans As OracleTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms As OracleParameter())
        If conn.State <> ConnectionState.Open Then
            conn.Open()
        End If
        cmd.Connection = conn
        cmd.CommandText = cmdText
        If trans IsNot Nothing Then
            cmd.Transaction = trans
        End If
        cmd.CommandType = cmdType
        If cmdParms IsNot Nothing Then
            For Each parm As OracleParameter In cmdParms
                cmd.Parameters.Add(parm)
            Next
        End If
    End Sub




    Public Shared Function ReadTable(ByVal transaction As OracleTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As DataTable
        Dim cmd As New OracleCommand()
        PrepareCommand(cmd, transaction.Connection, transaction, cmdType, cmdText, commandParameters)
        Dim dt As DataTable = HelperBase.ReadTable(cmd)
        cmd.Parameters.Clear()
        Return dt
    End Function

    Public Shared Function ReadTable(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As DataTable
        Using connection As New OracleConnection(connectionString)
            connection.Open()
            Return ReadTable(connection, cmdType, cmdText, commandParameters)
        End Using
    End Function
    Public Shared Function ReadTable(ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As DataTable
        Return ReadTable(CommandType.Text, cmdText, commandParameters)
    End Function
    Public Shared Function ReadTable(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As DataTable
        Return ReadTable(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ReadTable(ByVal connection As OracleConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As OracleParameter()) As DataTable
        Dim cmd As New OracleCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim dt As DataTable = HelperBase.ReadTable(cmd)
        cmd.Parameters.Clear()
        Return dt
    End Function

    Public Shared Function CreateInputParameter(ByVal paramName As String, ByVal dbtype As OracleType, ByVal value As Object) As OracleParameter
        Return CreateParameter(ParameterDirection.Input, paramName, dbtype, 0, value)
    End Function
    Public Shared Function CreateInputParameter(ByVal paramName As String, ByVal dbtype As OracleType, ByVal size As Integer, ByVal value As Object) As OracleParameter
        Return CreateParameter(ParameterDirection.Input, paramName, dbtype, size, value)
    End Function

    Public Shared Function CreateOutputParameter(ByVal paramName As String, ByVal dbtype As OracleType) As OracleParameter
        Return CreateParameter(ParameterDirection.Output, paramName, dbtype, 0, DBNull.Value)
    End Function

    Public Shared Function CreateOutputParameter(ByVal paramName As String, ByVal dbtype As OracleType, ByVal size As Integer) As OracleParameter
        Return CreateParameter(ParameterDirection.Output, paramName, dbtype, size, DBNull.Value)
    End Function

    Public Shared Function CreateParameter(ByVal direction As ParameterDirection, ByVal paramName As String, ByVal dbtype As OracleType, ByVal size As Integer, ByVal value As Object) As OracleParameter
        Dim param As New OracleParameter(paramName, dbtype, size)
        param.Value = value
        param.Direction = direction
        Return param
    End Function






End Class


