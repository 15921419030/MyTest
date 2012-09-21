
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Collections



Partial Public Class SqlServerHelper
    Public Shared ReadOnly default_connection_str As String = ConfigurationManager.ConnectionStrings("SqlServerHelper").ConnectionString

    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

    Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Dim cmd As New SqlCommand()
        Using conn As New SqlConnection(connectionString)
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function

    Public Shared Function ExecuteNonQuery(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteNonQuery(ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteNonQueryProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteNonQuery(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Dim cmd As New SqlCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim val As Integer = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteNonQuery(ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Integer
        Dim cmd As New SqlCommand()
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters)
        Dim val As Integer = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteReader(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As SqlDataReader
        Dim cmd As New SqlCommand()
        Dim conn As New SqlConnection(connectionString)
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            Return rdr
        Catch
            conn.Close()
            Throw
        End Try
    End Function

    Public Shared Function ExecuteReader(ByVal conn As SqlConnection, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As SqlDataReader
        Return ExecuteReader(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteReader(ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As SqlDataReader
        Return ExecuteReader(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteReaderProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As SqlParameter()) As SqlDataReader
        Return ExecuteReader(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteReader(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As SqlDataReader
        Return ExecuteReader(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Object
        Dim cmd As New SqlCommand()
        Using connection As New SqlConnection(connectionString)
            PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function

    Public Shared Function ExecuteScalar(ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Object
        Return ExecuteScalar(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalarProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As SqlParameter()) As Object
        Return ExecuteScalar(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Object
        Return ExecuteScalar(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As Object
        Dim cmd As New SqlCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim val As Object = cmd.ExecuteScalar()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Sub CacheParameters(ByVal cacheKey As String, ByVal ParamArray commandParameters As SqlParameter())
        parmCache(cacheKey) = commandParameters
    End Sub

    Public Shared Function GetCachedParameters(ByVal cacheKey As String) As SqlParameter()
        Dim cachedParms As SqlParameter() = DirectCast(parmCache(cacheKey), SqlParameter())
        If cachedParms Is Nothing Then
            Return Nothing
        End If
        Dim clonedParms As SqlParameter() = New SqlParameter(cachedParms.Length) {}

        Dim i As Integer = 0, j As Integer = cachedParms.Length
        While i < j
            clonedParms(i) = DirectCast((DirectCast(cachedParms(i), ICloneable)).Clone(), SqlParameter)
            i += 1
        End While
        Return clonedParms
    End Function

    Private Shared Sub PrepareCommand(ByVal cmd As SqlCommand, ByVal conn As SqlConnection, ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms As SqlParameter())
        If conn.State <> ConnectionState.Open Then
            conn.Open()
        End If
        cmd.Connection = conn
        cmd.CommandText = cmdText
        If Not trans Is Nothing Then
            cmd.Transaction = trans
        End If
        cmd.CommandType = cmdType
        If Not cmdParms Is Nothing Then
            For Each parm As SqlParameter In cmdParms
                cmd.Parameters.Add(parm)
            Next
        End If
    End Sub

    Public Shared Function ReadTable(ByVal transaction As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As DataTable
        Dim cmd As New SqlCommand()
        PrepareCommand(cmd, transaction.Connection, transaction, cmdType, cmdText, commandParameters)
        Dim dt As DataTable = HelperBase.ReadTable(cmd)
        cmd.Parameters.Clear()
        Return dt
    End Function

    Public Shared Function GetConnection() As SqlConnection
        Return New SqlConnection(default_connection_str)
    End Function

    Public Shared Function ReadTable(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As DataTable
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Return ReadTable(connection, cmdType, cmdText, commandParameters)
        End Using
    End Function
    Public Shared Function ReadTable(ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As DataTable
        Return ReadTable(CommandType.Text, cmdText, commandParameters)
    End Function
    Public Shared Function ReadTable(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As DataTable
        Return ReadTable(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ReadTable(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As SqlParameter()) As DataTable
        Dim cmd As New SqlCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim dt As DataTable = HelperBase.ReadTable(cmd)
        cmd.Parameters.Clear()
        Return dt
    End Function

    Public Shared Function CreateInputParameter(ByVal paramName As String, ByVal dbtype As SqlDbType, ByVal value As Object) As SqlParameter
        Return CreateParameter(ParameterDirection.Input, paramName, dbtype, 0, value)
    End Function
    Public Shared Function CreateInputParameter(ByVal paramName As String, ByVal dbtype As SqlDbType, ByVal size As Integer, ByVal value As Object) As SqlParameter
        Return CreateParameter(ParameterDirection.Input, paramName, dbtype, size, value)
    End Function

    Public Shared Function CreateOutputParameter(ByVal paramName As String, ByVal dbtype As SqlDbType) As SqlParameter
        Return CreateParameter(ParameterDirection.Output, paramName, dbtype, 0, DBNull.Value)
    End Function

    Public Shared Function CreateOutputParameter(ByVal paramName As String, ByVal dbtype As SqlDbType, ByVal size As Integer) As SqlParameter
        Return CreateParameter(ParameterDirection.Output, paramName, dbtype, size, DBNull.Value)
    End Function

    Public Shared Function CreateParameter(ByVal direction As ParameterDirection, ByVal paramName As String, ByVal dbtype As SqlDbType, ByVal size As Integer, ByVal value As Object) As SqlParameter
        Dim param As New SqlParameter(paramName, dbtype, size)
        param.Value = value
        param.Direction = direction
        Return param
    End Function
End Class

