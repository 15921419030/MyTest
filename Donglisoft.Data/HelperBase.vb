
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Data.Common
Imports System.Data

Public MustInherit Class HelperBase
    Implements IDisposable

    Friend isOpen As Boolean = False
    Private conn As DbConnection
    Private cmd As DbCommand

    Public Overridable Property Connection() As DbConnection
        Get
            Return conn
        End Get
        Set(ByVal value As DbConnection)
            conn = value
        End Set
    End Property
    Public Property Command() As DbCommand
        Get
            Return cmd
        End Get
        Set(ByVal value As DbCommand)
            cmd = value
        End Set
    End Property

    Private connection_str As String

    Public Property ConnectionString() As String
        Get
            Return connection_str
        End Get
        Set(ByVal value As String)
            connection_str = value
        End Set
    End Property


    Public Function ExecuteStoredProcedure(ByVal StoredProcedureName As String) As Integer
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = StoredProcedureName
        Return cmd.ExecuteNonQuery()
    End Function

    Public Function ExecuteNoneQuery() As Integer
        Return cmd.ExecuteNonQuery()
    End Function

    Public Function ExecuteScalarString() As String
        Return cmd.ExecuteScalar().ToString()
    End Function

    Public Function ExecuteScalarInt() As Integer
        Return Convert.ToInt32(cmd.ExecuteScalar())
    End Function

    Public Shared Function ReadTable(ByVal cmd As DbCommand) As DataTable
        Dim dt As New DataTable()
        Dim reader As DbDataReader = Nothing
        Try
            reader = cmd.ExecuteReader()
            Dim fieldc As Integer = reader.FieldCount

            For i As Integer = 0 To fieldc - 1
                Dim dc As New DataColumn(reader.GetName(i), reader.GetFieldType(i))
                dt.Columns.Add(dc)
            Next

            While reader.Read()
                Dim dr As DataRow = dt.NewRow()
                Dim n As Integer = 0
                For i As Integer = 0 To fieldc - 1
                    dr(i) = reader(i)
                Next
                dt.Rows.Add(dr)
            End While

            Return dt

        Finally
            If Not reader Is Nothing Then
                reader.Close()
            End If
        End Try
    End Function

    Public Function ReadTable() As DataTable
        Return HelperBase.ReadTable(cmd)
        '
        '                DataTable dt=new DataTable();
        '            DbDataReader reader = null;
        '            try
        '            {
        '                reader = cmd.ExecuteReader();
        '                int fieldc=reader.FieldCount;
        '                for (int i = 0; i < fieldc; i++)
        '                {
        '                    DataColumn dc = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
        '                    dt.Columns.Add(dc);
        '                }
        '                while (reader.Read())
        '                {
        '                    DataRow dr = dt.NewRow();
        '                    for (int i = 0; i < fieldc; i++)
        '                    {
        '                        dr[i] = reader[i];
        '                    }
        '                    dt.Rows.Add(dr);
        '                }
        '                return dt;
        '            }
        '            finally
        '            {
        '                if (reader != null) reader.Close();
        '            }

    End Function

    Public Overridable Sub Open()
        conn.ConnectionString = ConnectionString
        conn.Open()
        isOpen = True
    End Sub

    Public Overridable Sub Close()
        If isOpen AndAlso Not conn Is Nothing Then
            conn.Close()
        End If
    End Sub

    Public Sub Dispose()
        Close()
    End Sub

    Public Sub Dispose1() Implements System.IDisposable.Dispose

    End Sub
End Class
