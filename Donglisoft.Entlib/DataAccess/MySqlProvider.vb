
Imports MySql
Imports MySql.Data
Imports MySql.Data.MySqlClient
Namespace DataAccess
    Public Class MySqlProvider
        Public Shared Function GetConnectionString(ByVal ServerIP As String, _
                                                   ByVal ServiceName As String, _
                                                   ByVal UserName As String, _
                                                   ByVal Password As String, _
                                                   Optional ByVal Port As String = "3306") As String
            Dim sConn As String = "server={0};user id={1}; password={2}; database={3}; pooling=false;port={4}"
            Return String.Format(sConn, ServerIP, UserName, Password, ServiceName, Port)
        End Function

        Public Shared Function GetDBType(ByVal VBType As Type, Optional ByVal Length As Integer = 0, _
                                         Optional ByVal Identity As Boolean = False) As MySqlDbType
            Select Case VBType.Name.ToLower()
                Case "int64", "long", "uint64", "ulong"
                    Return MySqlDbType.Int64
                Case "integer", "int32", "uint32"
                    Return MySqlDbType.Int32
                Case "int16", "short", "uint16", "ushort"
                    Return MySqlDbType.Int16
                Case "byte"
                    Return MySqlDbType.Byte
                Case "boolean"
                    Return MySqlDbType.Bit
                Case "decimal"
                    Return MySqlDbType.Decimal
                Case "double"
                    Return MySqlDbType.Double
                Case "single"
                    Return MySqlDbType.Float
                Case "string"
                    Return MySqlDbType.String
                Case "byte()"
                    Return MySqlDbType.Blob
                Case "datetime", "date"
                    Return MySqlDbType.Datetime
            End Select
        End Function

        Public Shared Function GetVBType(ByVal DBType As Integer) As String
            Select Case DBType
                Case MySqlDbType.Int64
                    Return "Int64"
                Case MySqlDbType.Blob, MySqlDbType.MediumBlob, MySqlDbType.LongBlob, MySqlDbType.TinyBlob, _
                MySqlDbType.Timestamp
                    Return "Byte()"
                Case MySqlDbType.Bit
                    Return "Boolean"
                Case MySqlDbType.String, MySqlDbType.VarChar, MySqlDbType.VarString
                    Return "String"
                Case MySqlDbType.Decimal
                    Return "Decimal"
                Case MySqlDbType.Date, MySqlDbType.Datetime, MySqlDbType.Newdate
                    Return "DateTime"
                Case MySqlDbType.Decimal
                    Return "Decimal"
                Case MySqlDbType.Double
                    Return "Double"
                Case MySqlDbType.Int32
                    Return "Integer"
                Case MySqlDbType.Int16
                    Return "Int16"
                Case MySqlDbType.Enum, MySqlDbType.Geometry, MySqlDbType.Set
                    Return "Object"
                Case Else
                    Return "String"
            End Select
        End Function

        Public Shared Function GetVBType(ByVal DBType As String) As String
            Dim mytype As MySqlDbType
            mytype = [Enum].Parse(GetType(MySqlDbType), DBType, True)
            Return GetVBType(mytype)
        End Function

        Public Shared Function GetMySqlType(ByVal Source As String) As MySqlDbType
            Dim strSource As String = Source.Split("(")(0).ToLower()
            If strSource = "int" Then
                Return MySqlDbType.Int32
            Else
                Return [Enum].Parse(GetType(MySqlDbType), strSource, True)
            End If

        End Function

        Public Shared Function GetMySqlIntType(ByVal Source As String) As Integer
            Return GetMySqlType(Source)
        End Function

        Public Shared Function GetAllTables(Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(UserDBProvider.MySql, ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("show tables")
            Return dt
        End Function

        Public Shared Function GetColumns(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(UserDBProvider.MySql, ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("describe " & TableName)
            dt.Columns("Field").ColumnName = "COLUMN_NAME"
            dt.Columns("Type").ColumnName = "DATA_TYPE"

            Return dt
        End Function

        Public Shared Function GetPKeys(ByVal TableName As String, _
                                        Optional ByVal ConnectionString As String = "") As DataTable
            Dim dt As DataTable = GetColumns(TableName, ConnectionString)
            Dim dt1 As DataTable = dt.Clone()
            dt1.Rows.Clear()
            For Each row As DataRow In dt.Rows
                If row.Item("Key").ToString() = "PRI" Then dt1.Rows.Add(row.ItemArray)
            Next
            Return dt1
        End Function

        Public Shared Function GetFKeys_Children(ByVal TableName As String, _
                                           Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(UserDBProvider.MySql, ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("select * from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where REFERENCED_TABLE_NAME ='" & TableName & "'")
            Return dt
        End Function

        Public Shared Function GetFKeys_Parents(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(UserDBProvider.MySql, ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("select * from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME ='" & TableName & "' and REFERENCED_TABLE_NAME<>''")
            Return dt
        End Function

        Public Shared Function IsIdentity(ByVal TableName As String, ByVal ColumnName As String, _
                                          Optional ByVal ConnectionString As String = "") As Boolean
            Dim dt As DataTable = GetColumns(TableName, ConnectionString)
            For Each row As DataRow In dt.Rows
                If row.Item("COLUMN_NAME").ToString().ToLower() = ColumnName.ToLower() And row.Item("Extra").ToString() = "auto_increment" Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Class
End Namespace
