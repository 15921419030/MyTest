
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Donglisoft.Entlib
Imports Donglisoft.Entlib.Common

Namespace DataAccess

    Public Class Sql_Table_Info
        Inherits DataObject
        Public TABLE_QUALIFIER As String
        Public TABLE_OWNER As String
        Public TABLE_NAME As String
        Public TABLE_TYPE As String
    End Class

    Public Class Sql_Column_Info
        Inherits DataObject
        Public TABLE_QUALIFIER As String
        Public TABLE_OWNER As String
        Public TABLE_NAME As String
        Public COLUMN_NAME As String
        Public TYPE_NAME As String
        Public PRECISION As Integer
        Public LENGTH As String
        Public NULLABLE As Integer
        Public IS_NULLABLE As Boolean
    End Class

    Public Class Sql_PKeys_Info
        Inherits DataObject
        Public TABLE_QUALIFIER As String
        Public TABLE_OWNER As String
        Public TABLE_NAME As String
        Public COLUMN_NAME As String
        Public PK_NAME As String
    End Class

    Public Class MsSqlProvider
        Public Shared Function GetConnectionString(ByVal ServerAddr As String, ByVal Database As String, _
                                                   ByVal UserID As String, ByVal Password As String) As String
            Dim sConn As String = "Data Source={0};Initial Catalog={1};Persist Security Info=False;User ID={2};Password={3};Pooling=False"
            Return String.Format(sConn, ServerAddr, Database, UserID, Password)
        End Function

        Public Shared Function GetDBType(ByVal VBType As Type, Optional ByVal Length As Integer = 0, _
                                         Optional ByVal Identity As Boolean = False) As SqlDbType
            Select Case VBType.Name.ToLower()
                Case "int64", "long", "uint64", "ulong"
                    Return SqlDbType.BigInt
                Case "integer", "int32", "uint32"
                    Return SqlDbType.Int
                Case "int16", "short", "uint16", "ushort"
                    Return SqlDbType.SmallInt
                Case "byte"
                    Return SqlDbType.TinyInt
                Case "boolean"
                    Return SqlDbType.Bit
                Case "decimal"
                    Return SqlDbType.Decimal
                Case "double"
                    Return SqlDbType.Float
                Case "single"
                    Return SqlDbType.Real
                Case "string"
                    If Length <= 4000 Then
                        Return SqlDbType.NVarChar
                    Else
                        Return SqlDbType.NText
                    End If
                Case "byte()"
                    If Length <= 8000 Then
                        Return SqlDbType.VarBinary
                    Else
                        Return SqlDbType.Image
                    End If
                Case "datetime", "date"
                    If Identity Then
                        Return SqlDbType.Timestamp
                    Else
                        Return SqlDbType.Date
                    End If
            End Select
        End Function

        Public Shared Function GetVBType(ByVal DBType As SqlDbType) As String
            Select Case DBType
                Case SqlDbType.BigInt
                    Return "Int64"
                Case SqlDbType.Binary
                    Return "Byte()"
                Case SqlDbType.Bit
                    Return "Boolean"
                Case SqlDbType.Char
                    Return "String"
                Case SqlDbType.Date, SqlDbType.DateTime, SqlDbType.DateTime2, SqlDbType.SmallDateTime
                    Return "DateTime"
                Case SqlDbType.Decimal
                    Return "Decimal"
                Case SqlDbType.Float
                    Return "Double"
                Case SqlDbType.Image
                    Return "Byte()"
                Case SqlDbType.Int
                    Return "Integer"
                Case SqlDbType.Money, SqlDbType.SmallMoney
                    Return "Decimal"
                Case SqlDbType.NChar
                    Return "String"
                Case SqlDbType.NText
                    Return "String"
                Case SqlDbType.NVarChar
                    Return "String"
                Case SqlDbType.Real
                    Return "Single"
                Case SqlDbType.SmallInt
                    Return "Int16"
                Case SqlDbType.Text
                    Return "String"
                    'Case SqlDbType.Structured
                    'Case SqlDbType.Time
                Case SqlDbType.Timestamp
                    Return "Byte()"
                Case SqlDbType.TinyInt
                    Return "Byte"
                    'Case SqlDbType.Udt
                Case SqlDbType.UniqueIdentifier
                    Return "Guid"
                Case SqlDbType.VarBinary
                    Return "Byte()"
                Case SqlDbType.VarChar
                    Return "String"
                Case SqlDbType.Variant
                    Return "Object"
                    'Case SqlDbType.Xml
                Case Else
                    Return "String"
            End Select
        End Function

        Public Shared Function GetVBType(ByVal DBType As String) As String
            Dim mytype As SqlDbType
            Dim info() As String = DBType.Split(" ")
            If info.Length <= 0 Then
                mytype = SqlDbType.NVarChar
            Else
                If info(0).ToLower() = "numeric" Then
                    mytype = SqlDbType.Decimal
                ElseIf info(0).ToLower = "sql_variant" Then
                    mytype = SqlDbType.Variant
                Else
                    mytype = [Enum].Parse(GetType(SqlDbType), info(0), True)
                End If
            End If
            Return GetVBType(mytype)
        End Function

        Public Shared Function GetAllTables(Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(ConnectionString)
            End If
            Dim params As New List(Of IDataParameter)
            params.Add(uda.CreateParameter("@TABLE_TYPE", "'TABLE','VIEW'"))
            params.Add(uda.CreateParameter("@TABLE_OWNER", "dbo"))
            Dim dt As DataTable = uda.GetProcData("sp_tables", params).Tables(0)
            Return dt
        End Function

        Public Shared Function GetColumns(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(ConnectionString)
            End If
            Dim params As New List(Of IDataParameter)
            params.Add(uda.CreateParameter("@TABLE_NAME", TableName))
            Dim dt As DataTable = uda.GetProcData("sp_columns", params).Tables(0)
            Return dt
        End Function

        Public Shared Function GetPKeys(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(ConnectionString)
            End If
            Dim params As New List(Of IDataParameter)
            params.Add(uda.CreateParameter("@TABLE_NAME", TableName))
            Dim dt As DataTable = uda.GetProcData("sp_pkeys", params).Tables(0)
            Return dt
        End Function

        Public Shared Function GetFKeys_Children(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(ConnectionString)
            End If
            Dim params As New List(Of IDataParameter)
            params.Add(uda.CreateParameter("@pktable_name", TableName))
            Dim dt As DataTable = uda.GetProcData("sp_fkeys", params).Tables(0)
            Return dt
        End Function

        Public Shared Function GetFKeys_Parents(ByVal TableName As String, _
                                          Optional ByVal ConnectionString As String = "") As DataTable
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(ConnectionString)
            End If
            Dim params As New List(Of IDataParameter)
            params.Add(uda.CreateParameter("@fktable_name", TableName))
            Dim dt As DataTable = uda.GetProcData("sp_fkeys", params).Tables(0)
            Return dt
        End Function

        Public Shared Function GetDataTypeInfo(ByVal Sql_Data_Type As Integer, _
                                               Optional ByVal ConnectionString As String = "") As String
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(ConnectionString)
            End If
            Dim params As New List(Of IDataParameter)
            params.Add(uda.CreateParameter("@data_type", Sql_Data_Type))
            Dim dt As DataTable = uda.GetProcData("sp_datatype_info", params).Tables(0)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("TYPE_NAME").ToString()
            Else
                Return "varchar"
            End If
        End Function

        Public Shared Function IsIdentity(ByVal TableName As String, ByVal ColumnName As String, _
                                      Optional ByVal ConnectionString As String = "") As Boolean
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(ConnectionString)
            End If
            Dim dt As DataTable = uda.GetDataTable("select columnproperty(object_id('" & TableName & "'), " & _
                                                   "'" & ColumnName & "','IsIdentity') IsIdentity")
            Try
                Return dt.Rows(0)("IsIdentity") = 1
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetVersion(Optional ByVal ConnectionString As String = "") As Integer
            Dim uda As UserDBAgent
            If ConnectionString.Trim() = "" Then
                uda = New UserDBAgent()
            Else
                uda = New UserDBAgent(ConnectionString)
            End If
            Dim dt As DataTable = Nothing
            Try
                dt = uda.GetDataTable("select SERVERPROPERTY('productversion') VERSION")
            Catch ex As Exception

            End Try

            If Not dt Is Nothing AndAlso dt.Rows.Count = 1 Then
                Try
                    Dim intV As Integer = CInt(dt.Rows(0)(0).ToString().Split(".")(0))
                    Select Case intV
                        Case 8
                            Return 2000
                        Case 9
                            Return 2005
                        Case 10
                            Return 2008
                        Case Else
                            Return 9000
                    End Select
                Catch ex As Exception
                    Return 7
                End Try
            Else
                Return 7
            End If
        End Function
    End Class

    
End Namespace