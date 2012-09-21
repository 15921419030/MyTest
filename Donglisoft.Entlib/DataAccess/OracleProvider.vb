
Imports Oracle
Imports Oracle.DataAccess
Imports Oracle.DataAccess.Client

Namespace DataAccess
    Public Class OracleProvider
        Public Shared Function GetConnectionString(ByVal ServerIP As String, _
                                                   ByVal ServiceName As String, _
                                                   ByVal UserName As String, _
                                                   ByVal Password As String, _
                                                   Optional ByVal Port As String = "1521") As String
            Dim sConn As String = "Data Source=(DESCRIPTION=" _
               + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))" _
               + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));" _
               + "User Id={3};Password={4};"
            Return String.Format(sConn, ServerIP, Port, ServiceName, UserName, Password)
        End Function

        Public Shared Function GetDBType(ByVal VBType As Type, Optional ByVal Length As Integer = 0, _
                                         Optional ByVal Identity As Boolean = False) As OracleDbType
            Select Case VBType.Name.ToLower()
                Case "int64", "long", "uint64", "ulong"
                    Return OracleDbType.Int64
                Case "integer", "int32", "uint32"
                    Return OracleDbType.Int32
                Case "int16", "short", "uint16", "ushort"
                    Return OracleDbType.Int16
                Case "byte"
                    Return OracleDbType.Byte
                Case "boolean"
                    Return OracleDbType.Byte
                Case "decimal"
                    Return OracleDbType.Decimal
                Case "double"
                    Return OracleDbType.Double
                Case "single"
                    Return OracleDbType.Single
                Case "string"
                    Return OracleDbType.NVarchar2
                Case "byte()"
                    Return OracleDbType.Raw
                Case "datetime", "date"
                    Return OracleDbType.Date
            End Select
        End Function

        Public Shared Function GetVBType(ByVal DBType As Integer) As String
            Select Case DBType
                Case OracleDbType.Decimal
                    Return "Decimal"
                Case OracleDbType.Blob, OracleDbType.Clob
                    Return "Byte()"
                Case OracleDbType.NClob
                    Return "OracleString"
                Case OracleDbType.BFile
                    Return "OracleBFile"
                Case OracleDbType.Raw, OracleDbType.LongRaw
                    Return "Byte()"
                Case OracleDbType.Byte
                    Return "Byte"
                Case OracleDbType.Date, OracleDbType.TimeStamp, OracleDbType.TimeStampLTZ, OracleDbType.TimeStampTZ
                    Return "DateTime"
                Case OracleDbType.Double
                    Return "Double"
                Case OracleDbType.Int64, OracleDbType.Long
                    Return "Int64"
                Case OracleDbType.Int32
                    Return "Integer"
                Case OracleDbType.Int16
                    Return "Int16"
                Case OracleDbType.Single
                    Return "Single"
                Case Else
                    Return "String"
            End Select
        End Function

        Public Shared Function GetVBType(ByVal DBType As String) As String
            Dim mytype As OracleDbType
            mytype = [Enum].Parse(GetType(OracleDbType), DBType, True)
            Return GetVBType(mytype)
        End Function

        Public Shared Function GetOracleType(ByVal Source As String) As OracleDbType
            If Source = "NUMBER" Then
                Return OracleDbType.Decimal
            ElseIf Source = "FLOAT" Then
                Return OracleDbType.Single
            Else
                Return [Enum].Parse(GetType(OracleDbType), Source, True)
            End If
        End Function

        Public Shared Function GetOracleIntType(ByVal Source As String) As Integer
            Return GetOracleType(Source)
        End Function

        Public Shared Function GetAllTables(Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(UserDBProvider.Oracle, ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("select * from cat where TABLE_TYPE='TABLE' or TABLE_TYPE='VIEW'")
            '查看當前用戶下的表:
            'select * from tab
            'select * from cat
            'select * from user_tables

            '查看所有的表：
            'select * from all_tables where Owner=''
            'select * from dba_tables

            '查看當前用戶下的表的列：
            'select * from user_tab_columns

            '查看所有表的列：
            'select * from all_tab_columns
            'select * from dba_tab_columns
            'select tname,cname,coltype,width from col where tname='表名'
            Return dt
        End Function

        Public Shared Function GetColumns(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(UserDBProvider.Oracle, ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("select tname TABLE_NAME,cname COLUMN_NAME,coltype DATA_TYPE,width from col where tname='" & _
                                                   TableName & "'")
            Return dt
        End Function

        Public Shared Function GetPKeys(ByVal TableName As String, _
                                        Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(UserDBProvider.Oracle, ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("select   *   from   user_cons_columns " + _
                                                    "where   constraint_name   =   " + _
                                                    "(select   constraint_name   from   user_constraints " + _
                                                    "where   table_name   =   '" + TableName + _
                                                    "'  and   constraint_type   ='P')")
            Return dt
        End Function

        Public Shared Function GetFKeys_Children(ByVal TableName As String, _
                                           Optional ByVal ConnectionString As String = "") As DataTable
            Dim dt As DataTable = GetColumns(TableName, ConnectionString)
            Return dt
        End Function

        Public Shared Function GetFKeys_Parents(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable

            'Dim dt As DataTable = GetColumns(TableName, ConnectionString)
            'Return dt
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(UserDBProvider.Oracle, ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("select   *   from   user_cons_columns " + _
                                                    "where   constraint_name   =   " + _
                                                    "(select   constraint_name   from   user_constraints " + _
                                                    "where   table_name   =   '" + TableName + _
                                                    "'  and   constraint_type   ='R')")
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

