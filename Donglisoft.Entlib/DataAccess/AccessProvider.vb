
Imports System
Imports System.Data.OleDb
Imports Donglisoft.Entlib.Common

Namespace DataAccess
    Public Class AccessProvider
        Public Shared Function GetConnectionString(ByVal AccessFilePath As String, _
                                                   Optional ByVal Password As String = "") As String

            'Provider=Microsoft.Jet.OLEDB.4.0;'+
            'Data Source='+MyApp.GetAppPath+'clbiiidb.mdb;Persist Security Info=False'+
            ';Jet OLEDB:Database Password=h98t7gg78g67x54klfdc';
            Dim sConn As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=""{0}"";Persist Security Info=false;"
            sConn &= "Jet OLEDB:Database Password={1}"
            Return String.Format(sConn, AccessFilePath, Password)
        End Function

        Public Shared Function GetDBType(ByVal VBType As Type, Optional ByVal Length As Integer = 0, _
                                         Optional ByVal Identity As Boolean = False) As OleDbType
            Select Case VBType.Name.ToLower()
                Case "int64", "long", "uint64", "ulong"
                    Return OleDbType.BigInt
                Case "integer", "int32", "uint32"
                    Return OleDbType.Integer
                Case "int16", "short", "uint16", "ushort"
                    Return OleDbType.SmallInt
                Case "byte"
                    Return OleDbType.TinyInt
                Case "boolean"
                    Return OleDbType.Boolean
                Case "decimal"
                    Return OleDbType.Decimal
                Case "double"
                    Return OleDbType.Double
                Case "single"
                    Return OleDbType.Single
                Case "string"
                    Return OleDbType.VarChar
                Case "byte()"
                    Return OleDbType.Binary
                Case "datetime", "date"
                    Return OleDbType.Date
            End Select
        End Function

        Public Shared Function GetVBType(ByVal DBType As OleDbType) As String
            Select Case DBType
                Case OleDbType.BigInt
                    Return "Int64"
                Case OleDbType.Binary
                    Return "Byte()"
                Case OleDbType.Boolean
                    Return "Boolean"
                Case OleDbType.BSTR, OleDbType.Char
                    Return "String"
                Case OleDbType.Currency
                    Return "Decimal"
                Case OleDbType.Date, OleDbType.DBDate, OleDbType.DBTime, OleDbType.DBTimeStamp, OleDbType.Filetime
                    Return "DateTime"
                Case OleDbType.Decimal
                    Return "Decimal"
                Case OleDbType.Double
                    Return "Double"
                Case OleDbType.Guid
                    Return "Guid"
                Case OleDbType.IDispatch
                    Return "Object"
                Case OleDbType.Integer
                    Return "Integer"
                Case OleDbType.LongVarBinary
                    Return "Byte()"
                Case OleDbType.LongVarChar
                    Return "String"
                Case OleDbType.LongVarWChar
                    Return "String"
                Case OleDbType.Numeric
                    Return "Decimal"
                Case OleDbType.Single
                    Return "Single"
                Case OleDbType.SmallInt
                    Return "Int16"
                Case OleDbType.TinyInt
                    Return "SByte"
                Case OleDbType.UnsignedBigInt
                    Return "UInt64"
                Case OleDbType.UnsignedInt
                    Return "UInt32"
                Case OleDbType.UnsignedSmallInt
                    Return "UInt16"
                Case OleDbType.UnsignedTinyInt
                    Return "Byte"
                Case OleDbType.VarBinary
                    Return "Byte()"
                Case OleDbType.VarChar
                    Return "String"
                Case OleDbType.Variant
                    Return "Object"
                Case OleDbType.VarNumeric
                    Return "Decimal"
                Case OleDbType.VarWChar
                    Return "String"
                Case OleDbType.WChar
                    Return "String"
                Case Else
                    Return "String"
            End Select
        End Function

        Public Shared Function GetVBType(ByVal DBType As String) As String
            Dim mytype As OleDbType
            mytype = [Enum].Parse(GetType(SqlDbType), DBType, True)
            Return GetVBType(mytype)
        End Function


        Public Shared Function GetAllTables(Optional ByVal ConnectionString As String = "") As DataTable
            Dim conn As OleDbConnection
            If ConnectionString.Trim = "" Then
                conn = New OleDbConnection(Settings.DBContext.GetConnectionString())
            Else
                conn = New OleDbConnection(ConnectionString)
            End If

            conn.Open()
            Dim dt As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, _
                                                           New Object() {Nothing, Nothing, Nothing, "table"})
            conn.Close()
            Return dt
        End Function

        Public Shared Function GetColumns(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim conn As OleDbConnection
            If ConnectionString.Trim = "" Then
                conn = New OleDbConnection(Settings.DBContext.GetConnectionString())
            Else
                conn = New OleDbConnection(ConnectionString)
            End If

            conn.Open()
            Dim dt As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, _
                                                           New Object() {Nothing, Nothing, TableName, Nothing})
            conn.Close()
            Return dt
        End Function

        Public Shared Function GetPKeys(ByVal TableName As String, _
                                        Optional ByVal ConnectionString As String = "") As DataTable
            Dim conn As OleDbConnection
            If ConnectionString.Trim = "" Then
                conn = New OleDbConnection(Settings.DBContext.GetConnectionString())
            Else
                conn = New OleDbConnection(ConnectionString)
            End If

            conn.Open()
            Dim dt As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, _
                                                           New Object() {Nothing, Nothing, TableName})
            conn.Close()
            Return dt
        End Function

        Public Shared Function GetFKeys_Children(ByVal TableName As String, _
                                           Optional ByVal ConnectionString As String = "") As DataTable
            Dim conn As OleDbConnection
            If ConnectionString.Trim = "" Then
                conn = New OleDbConnection(Settings.DBContext.GetConnectionString())
            Else
                conn = New OleDbConnection(ConnectionString)
            End If

            conn.Open()
            Dim dt As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, _
                                                           New Object() {Nothing, Nothing, TableName, Nothing})
            conn.Close()
            Return dt
        End Function

        Public Shared Function GetFKeys_Parents(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim conn As OleDbConnection
            If ConnectionString.Trim = "" Then
                conn = New OleDbConnection(Settings.DBContext.GetConnectionString())
            Else
                conn = New OleDbConnection(ConnectionString)
            End If
            conn.Open()
            Dim dt As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, _
                                                           New Object() {Nothing, Nothing, TableName, Nothing})
            conn.Close()
            Return dt
        End Function

        Public Shared Function IsIdentity(ByVal TableName As String, ByVal ColumnName As String, _
                                          Optional ByVal ConnectionString As String = "") As Boolean
            'Dim uda As UserDBAgent
            'If ConnectionString.Trim() = "" Then
            '    uda = New UserDBAgent(UserDBProvider.Access, _
            '                          Settings.DBContext.GetConnectionString())
            'Else
            '    uda = New UserDBAgent(UserDBProvider.Access, ConnectionString)
            'End If
            'Dim dt As DataTable = uda.GetDataTable("Select " & ColumnName & " from " & TableName & " where 1=0")
            'Return dt.Columns(ColumnName).AutoIncrement
            Return False
        End Function
    End Class

End Namespace