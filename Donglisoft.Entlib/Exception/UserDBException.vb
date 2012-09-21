
Imports System

Namespace Exceptions
    Public Class UserDBException
        Inherits Exception

        Public Enum ErrorCodes
            CommitTransError = -1000
            RollbackTransError = -1001
            PrimaryKeyConflict = -1002
            ForeignKeyConflict = -1003
            UniqueIndexConflict = -1004
        End Enum

        Public Sub New(ByVal ErrorCode As ErrorCodes)
            MyBase.New(GetMessage(ErrorCode))
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub

        Public Shared Function GetMessage(ByVal ErrorCode As ErrorCodes)
            Select Case ErrorCode
                Case ErrorCodes.CommitTransError
                    Return "提交的事务尚未创建，请先调用BeginTrans开始事务！"
                Case ErrorCodes.RollbackTransError
                    Return "取消的事务尚未创建，请先调用BeginTrans开始事务！"
                Case ErrorCodes.PrimaryKeyConflict
                    Return "主键冲突！"
                Case ErrorCodes.ForeignKeyConflict
                    Return "外键冲突！"
                Case ErrorCodes.UniqueIndexConflict
                    Return "唯一索引冲突！"
                Case Else
                    Return "未知的错误！"
            End Select
        End Function
    End Class
End Namespace
