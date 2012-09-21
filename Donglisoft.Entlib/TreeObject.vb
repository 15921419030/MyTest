
Public Class TreeNodeObject(Of T As {New, DataObject})
    Implements System.Web.UI.IHierarchyData

    Private _ChildNodes As TreeNodes(Of T)
    Private _NodeIDName As String
    Private _NodeTextName As String
    Private _ParentIDName As String
    Private _SortName As String
    Private _DepthName As String
    Private _LockName As String
    Private _LockValue As Integer

    Private _ParentNode As TreeNodeObject(Of T)
    'Protected Friend _PathName As String

    Private _Item As T

    Public Sub New(ByVal NodeID As String, ByVal NodeIDName As String, _
                   ByVal NodeTextName As String, ByVal ParentIDName As String, _
                   ByVal SortName As String, Optional ByVal DepthName As String = "Depth", _
                   Optional ByVal LockName As String = "IsLock", Optional ByVal LockValue As Integer = 0)
        _NodeIDName = NodeIDName
        _NodeTextName = NodeTextName
        _ParentIDName = ParentIDName
        _SortName = SortName
        _LockName = LockName
        _LockValue = LockValue
        '
        _Item = New T
        _Item.SetMemberValue(_NodeIDName, NodeID)
        _Item.Reload()
        '创建子节点容器
        _ChildNodes = New TreeNodes(Of T)()
    End Sub

    Public Sub New(ByVal NodeItem As T, ByVal NodeIDName As String, _
                   ByVal NodeTextName As String, ByVal ParentIDName As String, _
                   ByVal SortName As String, Optional ByVal DepthName As String = "Depth", _
                   Optional ByVal LockName As String = "", Optional ByVal LockValue As Integer = 0)
        _NodeIDName = NodeIDName
        _NodeTextName = NodeTextName
        _ParentIDName = ParentIDName
        _SortName = SortName
        _LockName = LockName
        _LockValue = LockValue
        _DepthName = DepthName
        '
        _Item = NodeItem
        '创建子节点容器
        _ChildNodes = New TreeNodes(Of T)()
    End Sub

    Public Function GetChildren() As System.Web.UI.IHierarchicalEnumerable Implements System.Web.UI.IHierarchyData.GetChildren
        Dim childs As New DataObjectCollection(Of T)
        childs.Where(F(_ParentIDName) = NodeID And F(_NodeIDName) <> NodeID And F(_LockName) = _LockValue)
        childs.Order(_SortName)
        childs.AsCollection()
        _ChildNodes.Clear()
        For Each e As T In childs
            'Debug.WriteLine(e.GetMemberValue(_NodeIDName))
            _ChildNodes.Add(New TreeNodeObject(Of T)(e, _NodeIDName, _NodeTextName, _ParentIDName, _SortName, _DepthName, _LockName, _LockValue))
        Next
        Return _ChildNodes
    End Function

    Public Function GetParent() As System.Web.UI.IHierarchyData Implements System.Web.UI.IHierarchyData.GetParent
        Dim _ParentID As String = _Item.GetMemberValue(_ParentIDName)
        If _ParentID Is Nothing Or _ParentID = "" Then Return Nothing
        Dim e As New T
        e.SetMemberValue(_NodeIDName, _ParentID)
        e.Reload()
        If Not e.IsPersisted Then Return Nothing
        Dim p As New TreeNodeObject(Of T)(e, _NodeIDName, _NodeTextName, _ParentIDName, _SortName)
        Return p
    End Function

    Public ReadOnly Property NodeID() As String
        Get
            Return _Item.GetMemberValue(_NodeIDName)
        End Get
    End Property

    Public ReadOnly Property NodeText() As String
        Get
            Return _Item.GetMemberValue(_NodeTextName)
        End Get
    End Property

    Public ReadOnly Property ParentID() As String
        Get
            Return _Item.GetMemberValue(_ParentIDName)
        End Get
    End Property

    Public ReadOnly Property Sort() As Object
        Get
            Return _Item.GetMemberValue(_SortName)
        End Get
    End Property

    Public ReadOnly Property HasChildren() As Boolean Implements System.Web.UI.IHierarchyData.HasChildren
        Get
            GetChildren()
            If _ChildNodes Is Nothing Then
                Return False
            Else
                Return _ChildNodes.Count > 0
            End If

        End Get
    End Property

    Public ReadOnly Property Item() As Object Implements System.Web.UI.IHierarchyData.Item
        Get
            Return _Item
        End Get
    End Property

    Public ReadOnly Property Path() As String Implements System.Web.UI.IHierarchyData.Path
        Get
            If _ParentNode Is Nothing Then
                Return NodeID
            Else
                Return _ParentNode.Path & "," & NodeID
            End If
        End Get
    End Property

    Public ReadOnly Property Type() As String Implements System.Web.UI.IHierarchyData.Type
        Get
            Return Me.GetType().Name
        End Get
    End Property

    Public ReadOnly Property Depth() As Integer
        Get
            Return _Item.GetMemberValue(_DepthName)
        End Get
    End Property

    Public ReadOnly Property IsLeaf() As Boolean
        Get
            Return Not HasChildren()
        End Get
    End Property

    Public ReadOnly Property ParentNode() As TreeNodeObject(Of T)
        Get
            Return _ParentNode
        End Get
    End Property

    'Public Sub AddChild(ByVal item As TreeNodeObject(Of T))
    '    _ChildNodes.Add(item)
    '    item._ParentNode = Me
    'End Sub

    'Public Sub RemoveChild(ByVal item As TreeNodeObject(Of T))
    '    _ChildNodes.Remove(item)
    'End Sub
End Class

Public Class TreeNodes(Of T As {New, DataObject})
    Inherits List(Of TreeNodeObject(Of T))
    Implements System.Web.UI.IHierarchicalEnumerable

    Private _NodeIDName As String
    Private _NodeTextName As String
    Private _ParentIDName As String
    Private _SortName As String
    Private _DepthName As String
    Private _LockName As String

    ''' <summary>
    ''' 从数据库中创建TreeNodes
    ''' </summary>
    ''' <param name="RootNodeID">根节点ID，如果是检索全部，则根节点设置为Nothing或""</param>
    ''' <param name="NodeIDName">节点ID对应的字段名称</param>
    ''' <param name="NodeTextName">节点文本对应的字段名称</param>
    ''' <param name="ParentIDName">父节点对应的字段名称</param>
    ''' <param name="SortName">排序字段对应的字段名称</param>
    ''' <param name="DepthName">节点深度对应的字段名称</param>
    ''' <param name="LockName">锁定字段对应的字段名称</param>
    ''' <param name="LockValue">锁定查询值，0代表显示未锁定数据，1代表显示锁定数据</param>
    ''' <param name="Condition">查询条件：为Fpl.CriteriaOperator类型</param>
    ''' <returns>返回创建的TreeNodes</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateTreeNodes(ByVal RootNodeID As String, ByVal NodeIDName As String, _
                               ByVal NodeTextName As String, _
                               ByVal ParentIDName As String, _
                               Optional ByVal SortName As String = "Sort", _
                               Optional ByVal DepthName As String = "Depth", _
                               Optional ByVal LockName As String = "IsLock", _
                               Optional ByVal LockValue As Integer = 0, _
                               Optional ByVal Condition As CriteriaOperator = CType(Nothing, CriteriaOperator), _
                               Optional ByVal Context As Settings.DBContext = Nothing) As TreeNodes(Of T)

        If Context Is Nothing Then Context = Settings.DBContext.DefaultContext
        Dim root As New DataObjectCollection(Of T)(Context)

        Dim strWhere As CriteriaOperator = F("")

        If LockValue = 0 Then
            If RootNodeID Is Nothing OrElse RootNodeID.Trim() = "" Then
                strWhere = strWhere And F(DepthName) = 0 And F(LockName) = LockValue
            Else
                strWhere = strWhere And F(NodeIDName) = RootNodeID And F(LockName) = LockValue
            End If
        Else
            strWhere = strWhere And F(LockName) = LockValue
        End If

        If Not Condition Is Nothing Then
            strWhere = strWhere And Condition
        End If

        root.Where(strWhere)

        root.Order(SortName)
        root.AsCollection()
        Dim trees As New TreeNodes(Of T)
        '
        trees._NodeIDName = NodeIDName
        trees._NodeTextName = NodeTextName
        trees._ParentIDName = ParentIDName
        trees._SortName = SortName
        trees._DepthName = DepthName
        trees._LockName = LockName
        '
        For Each e As T In root
            trees.Add(New TreeNodeObject(Of T)(e, NodeIDName, NodeTextName, ParentIDName, SortName, DepthName, LockName, LockValue))
        Next
        Return trees
    End Function

    Private _SearchName As String

    Private Function FindByNodeID(ByVal Node As TreeNodeObject(Of T)) As Boolean
        If Node.NodeID = _SearchName Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function Compare(ByVal x As TreeNodeObject(Of T), ByVal y As TreeNodeObject(Of T)) As Integer
        If x.Depth < y.Depth Then
            Return -1
        ElseIf x.Depth = y.Depth Then
            If x.Sort > y.Sort Then
                Return 1
            ElseIf x.Sort = y.Sort Then
                Return 0
            Else
                Return -1
            End If
        Else
            Return 1
        End If
    End Function
    ''' <summary>
    ''' 根据节点ID查找节点，不查找子节点
    ''' </summary>
    ''' <param name="NodeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetItem(ByVal NodeID As String) As TreeNodeObject(Of T)
        _SearchName = NodeID
        Return Find(AddressOf FindByNodeID)
    End Function
    ''' <summary>
    ''' 根据节点ID查找节点，查找子节点
    ''' </summary>
    ''' <param name="NodeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNode(ByVal NodeID As String) As TreeNodeObject(Of T)
        Dim tno As TreeNodeObject(Of T) = GetItem(NodeID)
        If tno Is Nothing Then
            Dim i As Integer
            For i = 0 To Count - 1
                tno = CType(Item(i).GetChildren(), TreeNodes(Of T)).GetNode(NodeID)
                If Not tno Is Nothing Then
                    Exit For
                End If
            Next
        End If
        Return tno
    End Function

    Public Overloads Function Remove(ByVal NodeID As String) As Boolean
        Return Remove(GetItem(NodeID))
    End Function

    Public Sub SortByDepth()
        Sort(AddressOf Compare)
    End Sub
    ''' <summary>
    ''' 同级节点上移
    ''' </summary>
    ''' <param name="NodeID">要移动的节点ID</param>
    ''' <remarks></remarks>
    Public Sub MoveUp(ByVal NodeID As String)

        Dim snode As TreeNodeObject(Of T) = GetNode(NodeID)
        Dim pnode As TreeNodeObject(Of T) = GetNode(snode.ParentID)
        Dim mytrees As TreeNodes(Of T)
        If (pnode Is Nothing) Or (pnode.NodeID = snode.NodeID) Then
            mytrees = Me
        Else
            mytrees = pnode.GetChildren()
        End If
        If mytrees Is Nothing Then Return
        '
        Dim st As T = snode.Item
        If Not st.IsPersisted Then Return
        Dim si As Integer = mytrees.IndexOf(mytrees.GetItem(NodeID))
        If si > 0 And mytrees.Count > 1 Then
            Dim di As Integer = si - 1
            Dim dnode As TreeNodeObject(Of T) = mytrees.Item(di)
            Dim dt As T = dnode.Item
            Dim ssort As Integer = snode.Sort
            Dim dsort As Integer = dnode.Sort
            Dim trans As New UserDBTransaction()
            trans.BeginTrans()
            trans.Add(st)
            trans.Add(dt)
            Dim bHasError As Boolean = False
            dt.SetMemberValue(_SortName, ssort)
            bHasError = bHasError Or Not dt.Save()
            st.SetMemberValue(_SortName, dsort)
            bHasError = bHasError Or Not st.Save()
            If bHasError Then
                trans.RollbackTrans()
                dt.SetMemberValue(_SortName, dsort)
                st.SetMemberValue(_SortName, ssort)
            Else
                Try
                    trans.CommitTrans()
                Catch ex As Exception
                    trans.RollbackTrans()
                    dt.SetMemberValue(_SortName, dsort)
                    st.SetMemberValue(_SortName, ssort)
                End Try

            End If
        End If
    End Sub
    ''' <summary>
    ''' 同级节点下移
    ''' </summary>
    ''' <param name="NodeID">要移动的节点ID</param>
    ''' <remarks></remarks>
    Public Sub MoveDown(ByVal NodeID As String)
        '获取当前节点和父节点
        Dim snode As TreeNodeObject(Of T) = GetNode(NodeID)
        Dim pnode As TreeNodeObject(Of T) = GetNode(snode.ParentID)
        Dim mytrees As TreeNodes(Of T)
        If (pnode Is Nothing) Or (pnode.NodeID = snode.NodeID) Then
            mytrees = Me
        Else
            mytrees = pnode.GetChildren()
        End If
        If mytrees Is Nothing Then Return
        '
        Dim st As T = snode.Item
        If Not st.IsPersisted Then Return
        Dim si As Integer = mytrees.IndexOf(mytrees.GetItem(NodeID))
        If si < mytrees.Count - 1 And mytrees.Count > 1 Then
            Dim di As Integer = si + 1
            Dim dnode As TreeNodeObject(Of T) = mytrees.Item(di)
            Dim dt As T = dnode.Item
            Dim ssort As Integer = snode.Sort
            Dim dsort As Integer = dnode.Sort
            Dim trans As New UserDBTransaction()
            trans.BeginTrans()
            trans.Add(st)
            trans.Add(dt)
            Dim bHasError As Boolean = False
            dt.SetMemberValue(_SortName, ssort)
            bHasError = bHasError Or Not dt.Save()
            st.SetMemberValue(_SortName, dsort)
            bHasError = bHasError Or Not st.Save()
            If bHasError Then
                trans.RollbackTrans()
                dt.SetMemberValue(_SortName, dsort)
                st.SetMemberValue(_SortName, ssort)
            Else
                Try
                    trans.CommitTrans()
                Catch ex As Exception
                    trans.RollbackTrans()
                    dt.SetMemberValue(_SortName, dsort)
                    st.SetMemberValue(_SortName, ssort)
                End Try

            End If
        End If
    End Sub

    Public Function GetHierarchyData(ByVal enumeratedItem As Object) As System.Web.UI.IHierarchyData Implements System.Web.UI.IHierarchicalEnumerable.GetHierarchyData
        Return CType(enumeratedItem, System.Web.UI.IHierarchyData)
    End Function
    ''' <summary>
    ''' 获取根节点数据，并以实体集合返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCollection() As DataObjectCollection(Of T)
        Dim treedata As New DataObjectCollection(Of T)
        For i As Integer = 0 To Count - 1
            treedata.Add(Item(i).Item)
        Next
        Return treedata
    End Function
    ''' <summary>
    ''' 获取根节点数据并以Json字符串格式返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetJson() As String
        Dim sJson As New Text.StringBuilder()
        sJson.Append("{")
        sJson.Append("[")
        For i As Integer = 0 To Count - 1
            sJson.Append("{")
            sJson.Append("NodeID:'")
            sJson.Append(Ajax.ToJsonString(Item(i).NodeID))
            sJson.Append("',NodeText:'")
            sJson.Append(Ajax.ToJsonString(Item(i).NodeText))
            sJson.Append("',Path:'")
            sJson.Append(Ajax.ToJsonString(Item(i).Path))
            sJson.Append("',Sort:'")
            sJson.Append(Item(i).Sort.ToString())
            sJson.Append("',Depth:'")
            sJson.Append(Item(i).Depth.ToString())
            sJson.Append("',Item:'")
            sJson.Append(CType(Item(i).Item, T).ToJson())
            sJson.Append("'")
            sJson.Append("}")
            If i < Count - 1 Then sJson.Append(",")
        Next
        sJson.Append("]}")
        Return sJson.ToString()
    End Function
    ''' <summary>
    ''' 获取根节点及儿子节点数据，并以实体集合返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMeAndChildCollection() As DataObjectCollection(Of T)
        Dim treedata As New DataObjectCollection(Of T)
        For i As Integer = 0 To Count - 1
            treedata.Add(Item(i).Item)
            Dim child As TreeNodes(Of T) = Item(i).GetChildren()
            For j As Integer = 0 To child.Count - 1
                treedata.Add(child.Item(j).Item)
            Next
        Next
        Return treedata
    End Function
    ''' <summary>
    ''' 获取树的所有节点ID的值以,号分隔
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMeAndAllChildNodeIDS() As String
        Dim sJson As New Text.StringBuilder()
        For i As Integer = 0 To Count - 1
            sJson.Append(Item(i).NodeID)
            sJson.Append(",")
            Dim child As TreeNodes(Of T) = Item(i).GetChildren()
            If child.Count > 0 Then
                sJson.Append(child.GetMeAndAllChildNodeIDS())
            End If
        Next
        Dim str As String = sJson.ToString()
        If str.EndsWith(",") Then str = str.Remove(str.Length - 1, 1)
        Return str
    End Function

    ''' <summary>
    ''' 获取根节点及儿子节点数据，并以Json字符串格式返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMeAndChildJson() As String
        Dim sJson As New Text.StringBuilder()
        sJson.Append("{")
        sJson.Append("[")
        For i As Integer = 0 To Count - 1
            sJson.Append("{")
            sJson.Append("NodeID:'")
            sJson.Append(Ajax.ToJsonString(Item(i).NodeID))
            sJson.Append("',NodeText:'")
            sJson.Append(Ajax.ToJsonString(Item(i).NodeText))
            sJson.Append("',Path:'")
            sJson.Append(Ajax.ToJsonString(Item(i).Path))
            sJson.Append("',Sort:'")
            sJson.Append(Item(i).Sort.ToString())
            sJson.Append("',Depth:'")
            sJson.Append(Item(i).Depth.ToString())
            sJson.Append("'")
            Dim child As TreeNodes(Of T) = Item(i).GetChildren()

            For j As Integer = 0 To child.Count - 1
                sJson.Append(",Children:{[")
                sJson.Append("{")
                sJson.Append("NodeID:'")
                sJson.Append(Ajax.ToJsonString(child(j).NodeID))
                sJson.Append("',NodeText:'")
                sJson.Append(Ajax.ToJsonString(child(j).NodeText))
                sJson.Append("',Path:'")
                sJson.Append(Ajax.ToJsonString(child(j).Path))
                sJson.Append("',Sort:'")
                sJson.Append(child(j).Sort.ToString())
                sJson.Append("',Depth:'")
                sJson.Append(child(j).Depth.ToString())
                sJson.Append("'")
                sJson.Append("]}")
            Next

            sJson.Append("}")
        Next
        sJson.Append("]")
        Return sJson.ToString()
    End Function
    ''' <summary>
    ''' 获取树的全部节点数据，并以实体集合返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllCollection() As DataObjectCollection(Of T)
        Dim treedata As New DataObjectCollection(Of T)
        For i As Integer = 0 To Count - 1
            treedata.Add(Item(i).Item)
            Dim child As TreeNodes(Of T) = Item(i).GetChildren()
            If child.Count > 0 Then
                treedata.AddRange(child.GetAllCollection())
            End If
        Next
        Return treedata
    End Function
    ''' <summary>
    ''' 获取树的全部节点数据，并以Json字符串格式返回，返回结果为树结构
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllJson() As String
        Dim sJson As New Text.StringBuilder()
        sJson.Append("[")
        For i As Integer = 0 To Count - 1
            sJson.Append("{root:{")
            sJson.Append("NodeID:'")
            sJson.Append(Ajax.ToJsonString(Item(i).NodeID))
            sJson.Append("',NodeText:'")
            sJson.Append(Ajax.ToJsonString(Item(i).NodeText))
            sJson.Append("',Path:'")
            sJson.Append(Ajax.ToJsonString(Item(i).Path))
            sJson.Append("',Sort:'")
            sJson.Append(Item(i).Sort.ToString())
            sJson.Append("',Depth:'")
            sJson.Append(Item(i).Depth.ToString())
            sJson.Append("',Item:")
            sJson.Append(CType(Item(i).Item, T).ToJson())
            sJson.Append("}")
            Dim child As TreeNodes(Of T) = Item(i).GetChildren()
            If child.Count > 0 Then
                sJson.Append(",Children:")
                sJson.Append(child.GetAllJson())
            End If
            sJson.Append("}")
            If i < Count - 1 Then sJson.Append(",")
        Next
        sJson.Append("]")
        Return sJson.ToString()

    End Function
    ''' <summary>
    ''' 获取树的所有节点并按对应深度增加空格形成树状排版的数据，以实体集合返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllTreeCollection(Optional ByVal Decode As Boolean = False) As DataObjectCollection(Of T)
        Dim data As DataObjectCollection(Of T) = GetAllCollection()
        Dim i, j As Integer
        For i = 0 To data.Count - 1
            Dim dCount As Integer = data.Item(i).GetMemberValue(_DepthName) - data.Item(0).GetMemberValue(_DepthName)
            Dim sText As New Text.StringBuilder()
            For j = 1 To dCount
                If Decode Then
                    sText.Append(Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"))
                Else
                    sText.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
                End If
            Next
            sText.Append(data.Item(i).GetMemberValue(_NodeTextName))
            data.Item(i).SetMemberValue(_NodeTextName, sText.ToString())
        Next
        Return data
    End Function
    ''' <summary>
    ''' 获取根节点和儿子节点并按对应深度增加空格形成树状排版的数据，以实体集合返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMeAndChildTreeCollection() As DataObjectCollection(Of T)
        Dim data As DataObjectCollection(Of T) = GetMeAndChildCollection()
        Dim i, j As Integer
        For i = 0 To data.Count - 1
            Dim dCount As Integer = data.Item(i).GetMemberValue(_DepthName) - data.Item(0).GetMemberValue(_DepthName)
            Dim sText As New Text.StringBuilder()
            For j = 1 To dCount
                sText.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
            Next
            sText.Append(data.Item(i).GetMemberValue(_NodeTextName))
            data.Item(i).SetMemberValue(_NodeTextName, sText.ToString())
        Next
        Return data
    End Function

End Class

