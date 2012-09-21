
Imports System
Imports System.Web
Imports Donglisoft.Entlib.DataAccess
Imports System.Configuration

Namespace Settings
    Public Class DBContext
        Private _DBType As UserDBProvider
        Private _ConnectionString As String
        Private Shared _DefaultContext As DBContext

        Property DBType() As UserDBProvider
            Get
                Return _DBType
            End Get
            Set(ByVal value As UserDBProvider)
                _DBType = value
            End Set
        End Property

        Property ConnectionString() As String
            Get
                Return _ConnectionString
            End Get
            Set(ByVal value As String)
                _ConnectionString = value
            End Set
        End Property

        Public Shared Sub AddDatabase(ByVal ConnectionString As String, _
                                      Optional ByVal DBType As UserDBProvider = UserDBProvider.MSSqlServer)
            _DefaultContext = New DBContext
            _DefaultContext.DBType = DBType
            _DefaultContext.ConnectionString = ConnectionString
        End Sub

        Public Shared Function GetDBType() As UserDBProvider
            Return DefaultContext.DBType
        End Function

        Public Shared Function GetConnectionString() As String
            Return DefaultContext.ConnectionString
        End Function

        Public Shared ReadOnly Property DefaultContext() As DBContext
            Get
                If _DefaultContext Is Nothing Then _DefaultContext = New DBContext()
                Return _DefaultContext
            End Get
        End Property
    End Class

    Public Class SqlServerSections
        Inherits ConfigurationSection

        Private _Server As String

        <ConfigurationProperty("server", DefaultValue:="(local)")> _
        Public Property Server() As String
            Get
                Return Me("server")
            End Get
            Set(ByVal value As String)
                Me("server") = value
            End Set
        End Property

        <ConfigurationProperty("database", DefaultValue:="")> _
        Public Property Database() As String
            Get
                Return Me("database")
            End Get
            Set(ByVal value As String)
                Me("database") = value
            End Set
        End Property

        <ConfigurationProperty("username", DefaultValue:="sa")> _
        Public Property Username() As String
            Get
                Return Me("username")
            End Get
            Set(ByVal value As String)
                Me("username") = value
            End Set
        End Property

        <ConfigurationProperty("password", DefaultValue:="sa")> _
        Public Property Password() As String
            Get
                Return Me("password")
            End Get
            Set(ByVal value As String)
                Me("password") = value
            End Set
        End Property

    End Class

    Public Class AccessSections
        Inherits ConfigurationSection

        <ConfigurationProperty("filepath")> _
        Public Property FilePath() As String
            Get
                Return Me("filepath")
            End Get
            Set(ByVal value As String)
                Me("filepath") = value
            End Set
        End Property

        <ConfigurationProperty("password", DefaultValue:="")> _
        Public Property Password() As String
            Get
                Return Me("password")
            End Get
            Set(ByVal value As String)
                Me("password") = value
            End Set
        End Property
    End Class

    Public Class OracleSections
        Inherits ConfigurationSection
        <ConfigurationProperty("server")> _
        Public Property Server() As String
            Get
                Return Me("server")
            End Get
            Set(ByVal value As String)
                Me("server") = value
            End Set
        End Property

        <ConfigurationProperty("port", DefaultValue:="1521")> _
        Public Property Port() As String
            Get
                Return Me("port")
            End Get
            Set(ByVal value As String)
                Me("port") = value
            End Set
        End Property

        <ConfigurationProperty("service")> _
        Public Property Service() As String
            Get
                Return Me("service")
            End Get
            Set(ByVal value As String)
                Me("service") = value
            End Set
        End Property

        <ConfigurationProperty("username")> _
        Public Property Username() As String
            Get
                Return Me("username")
            End Get
            Set(ByVal value As String)
                Me("username") = value
            End Set
        End Property

        <ConfigurationProperty("password")> _
        Public Property Password() As String
            Get
                Return Me("password")
            End Get
            Set(ByVal value As String)
                Me("password") = value
            End Set
        End Property
    End Class

    Public Class MySqlSections
        Inherits ConfigurationSection

        <ConfigurationProperty("server")> _
        Public Property Server() As String
            Get
                Return Me("server")
            End Get
            Set(ByVal value As String)
                Me("server") = value
            End Set
        End Property

        <ConfigurationProperty("port", DefaultValue:="1521")> _
        Public Property Port() As String
            Get
                Return Me("port")
            End Get
            Set(ByVal value As String)
                Me("port") = value
            End Set
        End Property

        <ConfigurationProperty("service")> _
        Public Property Service() As String
            Get
                Return Me("service")
            End Get
            Set(ByVal value As String)
                Me("service") = value
            End Set
        End Property

        <ConfigurationProperty("username")> _
        Public Property Username() As String
            Get
                Return Me("username")
            End Get
            Set(ByVal value As String)
                Me("username") = value
            End Set
        End Property

        <ConfigurationProperty("password")> _
        Public Property Password() As String
            Get
                Return Me("password")
            End Get
            Set(ByVal value As String)
                Me("password") = value
            End Set
        End Property
    End Class

End Namespace
