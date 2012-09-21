Imports System.Collections.Generic
Imports System.Text
Imports System.Configuration
Imports System.Collections
Imports MySql.Data.MySqlClient
Imports System.Data



Partial Public Class MySqlHelper

#Region "Mysql Connector Net Methods"


    Private Enum CharClass As Byte
        None
        Quote
        Backslash
    End Enum

    Const stringOfBackslashChars As String = "\¥Š₩∖﹨＼"
    Const stringOfQuoteChars As String = """'`´ʹʺʻʼˈˊˋ˙̀́‘’‚′‵❛❜＇"

    Shared charClassArray As CharClass() = makeCharClassArray()

    Private Shared Function makeCharClassArray() As CharClass()

        Dim a As CharClass() = New CharClass(65536) {}
        For Each c As Char In stringOfBackslashChars
            a(Val(c)) = CharClass.Backslash
        Next
        For Each c As Char In stringOfQuoteChars
            a(Val(c)) = CharClass.Quote
        Next
        Return a
    End Function

    Private Shared Function needsQuoting(ByVal s As String) As Boolean
        For Each c As Char In s
            If charClassArray(Val(c)) <> CharClass.None Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Escapes the string.
    ''' </summary>
    ''' <param name="value">The string to escape</param>
    ''' <returns>The string with all quotes escaped.</returns>
    Public Shared Function EscapeString(ByVal value As String) As String
        If Not needsQuoting(value) Then
            Return value
        End If

        Dim sb As New StringBuilder()

        For Each c As Char In value
            Dim charClass__1 As CharClass = charClassArray(Val(c))
            If charClass__1 <> CharClass.None Then
                sb.Append("\")
            End If
            sb.Append(c)
        Next
        Return sb.ToString()
    End Function

    Public Shared Function DoubleQuoteString(ByVal value As String) As String
        If Not needsQuoting(value) Then
            Return value
        End If

        Dim sb As New StringBuilder()
        For Each c As Char In value
            Dim charClass__1 As CharClass = charClassArray(Val(c))
            If charClass__1 = CharClass.Quote Then
                sb.Append(c)
            ElseIf charClass__1 = CharClass.Backslash Then
                sb.Append("\")
            End If
            sb.Append(c)
        Next
        Return sb.ToString()
    End Function


#End Region

    Public Shared ReadOnly default_connection_str As String = ConfigurationManager.ConnectionStrings("MySqlHelper").ConnectionString

    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

    Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Integer
        Dim cmd As New MySqlCommand()
        Using conn As New MySqlConnection(connectionString)
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function

    Public Shared Function ExecuteNonQuery(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteNonQuery(ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteNonQueryProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As MySqlParameter()) As Integer
        Return ExecuteNonQuery(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteNonQuery(ByVal connection As MySqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Integer
        Dim cmd As New MySqlCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim val As Integer = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteNonQuery(ByVal trans As MySqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Integer
        Dim cmd As New MySqlCommand()
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters)
        Dim val As Integer = cmd.ExecuteNonQuery()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Function ExecuteReader(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As MySqlDataReader
        Dim cmd As New MySqlCommand()
        Dim conn As New MySqlConnection(connectionString)
        Try
            PrepareCommand(cmd, conn, Nothing, cmdType, cmdText, commandParameters)
            Dim rdr As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            Return rdr
        Catch
            conn.Close()
            Throw
        End Try
    End Function

    Public Shared Function ExecuteReader(ByVal conn As MySqlConnection, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As MySqlDataReader
        Return ExecuteReader(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteReader(ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As MySqlDataReader
        Return ExecuteReader(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteReaderProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As MySqlParameter()) As MySqlDataReader
        Return ExecuteReader(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteReader(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As MySqlDataReader
        Return ExecuteReader(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Object
        Dim cmd As New MySqlCommand()
        Using connection As New MySqlConnection(connectionString)
            PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function

    Public Shared Function ExecuteScalar(ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Object
        Return ExecuteScalar(default_connection_str, CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalarProc(ByVal StoredProcedureName As String, ByVal ParamArray commandParameters As MySqlParameter()) As Object
        Return ExecuteScalar(default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Object
        Return ExecuteScalar(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ExecuteScalar(ByVal connection As MySqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As Object
        Dim cmd As New MySqlCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim val As Object = cmd.ExecuteScalar()
        cmd.Parameters.Clear()
        Return val
    End Function

    Public Shared Sub CacheParameters(ByVal cacheKey As String, ByVal ParamArray commandParameters As MySqlParameter())
        parmCache(cacheKey) = commandParameters
    End Sub

    Public Shared Function GetCachedParameters(ByVal cacheKey As String) As MySqlParameter()
        Dim cachedParms As MySqlParameter() = DirectCast(parmCache(cacheKey), MySqlParameter())
        If cachedParms Is Nothing Then
            Return Nothing
        End If
        Dim clonedParms As MySqlParameter() = New MySqlParameter(cachedParms.Length - 1) {}
        Dim i As Integer = 0, j As Integer = cachedParms.Length
        While i < j
            clonedParms(i) = DirectCast(DirectCast(cachedParms(i), ICloneable).Clone(), MySqlParameter)
            i += 1
        End While
        Return clonedParms
    End Function

    Private Shared Sub PrepareCommand(ByVal cmd As MySqlCommand, ByVal conn As MySqlConnection, ByVal trans As MySqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms As MySqlParameter())
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
            For Each parm As MySqlParameter In cmdParms
                cmd.Parameters.Add(parm)
            Next
        End If
    End Sub

    Public Shared Function ReadTable(ByVal transaction As MySqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As DataTable
        Dim cmd As New MySqlCommand()
        PrepareCommand(cmd, transaction.Connection, transaction, cmdType, cmdText, commandParameters)
        Dim dt As DataTable = HelperBase.ReadTable(cmd)
        cmd.Parameters.Clear()
        Return dt
    End Function

    Public Shared Function GetConnection() As MySqlConnection
        Return New MySqlConnection(default_connection_str)
    End Function

    Public Shared Function ReadTable(ByVal connectionString As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As DataTable
        Using connection As New MySqlConnection(connectionString)
            connection.Open()
            Return ReadTable(connection, cmdType, cmdText, commandParameters)
        End Using
    End Function
    Public Shared Function ReadTable(ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As DataTable
        Return ReadTable(default_connection_str, cmdType, cmdText, commandParameters)
    End Function

    Public Shared Function ReadTable(ByVal connection As MySqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As DataTable
        Dim cmd As New MySqlCommand()
        PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
        Dim dt As DataTable = HelperBase.ReadTable(cmd)
        cmd.Parameters.Clear()
        Return dt
    End Function
    Public Shared Function ReadTable(ByVal cmdText As String, ByVal ParamArray commandParameters As MySqlParameter()) As DataTable
        Return ReadTable(CommandType.Text, cmdText, commandParameters)
    End Function

    Public Shared Function CreateInputParameter(ByVal paramName As String, ByVal dbtype As MySqlDbType, ByVal value As Object) As MySqlParameter
        Return CreateParameter(ParameterDirection.Input, paramName, dbtype, 0, value)
    End Function
    Public Shared Function CreateInputParameter(ByVal paramName As String, ByVal dbtype As MySqlDbType, ByVal size As Integer, ByVal value As Object) As MySqlParameter
        Return CreateParameter(ParameterDirection.Input, paramName, dbtype, size, value)
    End Function

    Public Shared Function CreateOutputParameter(ByVal paramName As String, ByVal dbtype As MySqlDbType) As MySqlParameter
        Return CreateParameter(ParameterDirection.Output, paramName, dbtype, 0, DBNull.Value)
    End Function

    Public Shared Function CreateOutputParameter(ByVal paramName As String, ByVal dbtype As MySqlDbType, ByVal size As Integer) As MySqlParameter
        Return CreateParameter(ParameterDirection.Output, paramName, dbtype, size, DBNull.Value)
    End Function

    Public Shared Function CreateParameter(ByVal direction As ParameterDirection, ByVal paramName As String, ByVal dbtype As MySqlDbType, ByVal size As Integer, ByVal value As Object) As MySqlParameter
        Dim param As New MySqlParameter(paramName, dbtype, size)
        param.Value = value
        param.Direction = direction
        Return param
    End Function

End Class


