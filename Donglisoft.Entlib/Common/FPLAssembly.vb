Imports System
Imports System.IO
Imports System.CodeDom
Imports System.CodeDom.Compiler
Imports System.Globalization
Imports System.Reflection

Public Class FPLAssembly
    ''' <summary>
    ''' 动态编译类库
    ''' </summary>
    ''' <param name="sourceName">要编译的源文件</param>
    ''' <param name="DLLPath">编译生成的类库路径</param>
    ''' <param name="ReturnDLLName">返回生成的类库名称</param>
    ''' <param name="RequireAssemblies">编译所需要加载的装配件</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CompileDll(ByVal sourceName As String, ByVal DLLPath As String, ByRef ReturnDLLName As String, _
                                      Optional ByVal RequireAssemblies() As String = Nothing) As Boolean
        Dim sourceFile As FileInfo = New FileInfo(sourceName)
        Dim provider As CodeDomProvider = Nothing
        Dim compileOk As Boolean = False

        ' 根据原文件的扩展名选择code provider 
        If sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) = ".CS" Then

            provider = New Microsoft.CSharp.CSharpCodeProvider()

        ElseIf sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) = ".VB" Then

            provider = New Microsoft.VisualBasic.VBCodeProvider()

        Else
            Console.WriteLine("原文件必须包含 .cs 或 .vb 扩展名")
        End If

        If Not provider Is Nothing Then
            ' 构造DLL文件的全路径 

            Dim dllName As String = String.Format("{0}\{1}.dll", _
            DLLPath, _
            sourceFile.Name.Replace(".", "_"))

            ReturnDLLName = dllName

            Dim cp As CompilerParameters = New CompilerParameters()

            ' 设置编译控制参数 
            cp.GenerateExecutable = False '生成DLL，如果是True则生成exe文件 
            cp.OutputAssembly = dllName
            cp.GenerateInMemory = False
            cp.TreatWarningsAsErrors = False

            If Not RequireAssemblies Is Nothing Then
                Dim i As Integer
                For i = RequireAssemblies.GetLowerBound(0) To RequireAssemblies.GetUpperBound(0)
                    cp.ReferencedAssemblies.Add(RequireAssemblies(i))
                Next
            End If

            ' 调用编译方法将原代码文件编译成DLL 
            Dim cr As CompilerResults = provider.CompileAssemblyFromFile(cp, _
            sourceName)

            If cr.Errors.Count > 0 Then
                ' 显示编译错误 
                Console.WriteLine("编译错误 {0} 编译成 {1}", _
                sourceName, cr.PathToAssembly)
                Dim ce As CompilerError
                For Each ce In cr.Errors
                    Console.WriteLine(" {0}", ce.ToString())
                    Console.WriteLine()
                Next ce
            Else
                ' 显示编译成功的消息 
                Console.WriteLine("原文件 {0} 编译成 {1} 成功完成.", _
                sourceName, cr.PathToAssembly)
            End If

            ' 返回编译结果 
            If cr.Errors.Count > 0 Then
                compileOk = False
            Else
                compileOk = True
            End If
        End If
        Return compileOk

    End Function
    ''' <summary>
    ''' 从动态链接库中创建实体类型
    ''' </summary>
    ''' <param name="DllName">加载的动态链接库物理路径</param>
    ''' <param name="ClassName">创建的类名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateObjFromDll(ByVal DllName As String, ByVal ClassName As String) As DataObject
        Dim a As Assembly = Assembly.LoadFrom(DllName) '加载DLL 
        If a Is Nothing Then Return Nothing
        Dim myType As System.Type = a.GetType(ClassName) '获得MyClasses的Type
        If myType Is Nothing Then Return Nothing
        Dim obj As Object = Activator.CreateInstance(myType) '获得MyClasses的实例
        Dim entity As DataObject = CType(obj, DataObject)
        Return entity
    End Function
    ''' <summary>
    ''' 从项目装配件中创建实体类型
    ''' </summary>
    ''' <param name="EntityAssemblyName">项目中引用的装配件名称</param>
    ''' <param name="ClassName">创建的类名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateObjFromAssembly(ByVal EntityAssemblyName As String, ByVal ClassName As String) As DataObject
        Dim a As Assembly = Assembly.Load(EntityAssemblyName)
        If a Is Nothing Then Return Nothing
        Dim myType As System.Type = a.GetType(EntityAssemblyName & "." & ClassName)
        If myType Is Nothing Then Return Nothing
        Dim obj As Object = Activator.CreateInstance(myType) '获得MyClasses的实例
        Dim entity As DataObject = CType(obj, DataObject)
        Return entity
    End Function
    ''' <summary>
    ''' 利用反射计算字符串表达式的值
    ''' </summary>
    ''' <param name="Expression"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Eval(ByVal Expression As String) As Object
        Dim ret As Object = Nothing
        Dim CodeBuilder As New Text.StringBuilder
        Dim comp As CodeDom.Compiler.CodeDomProvider = New Microsoft.VisualBasic.VBCodeProvider
        Dim cp As New CodeDom.Compiler.CompilerParameters
        Dim mi As Reflection.MethodInfo

        CodeBuilder.Append("Imports   System" & vbCrLf)
        CodeBuilder.Append("Imports   System.Math" & vbCrLf)
        CodeBuilder.Append("Imports   Microsoft.VisualBasic" & vbCrLf)
        CodeBuilder.Append(vbCrLf)
        CodeBuilder.Append("Public   Module   Mode" & vbCrLf)
        CodeBuilder.Append("       Public   Function   Func()   As   Object" & vbCrLf)
        CodeBuilder.Append("                 Return   " & Expression & vbCrLf)
        CodeBuilder.Append("       End   Function" & vbCrLf)
        CodeBuilder.Append("End   Module" & vbCrLf)

        cp.ReferencedAssemblies.Add("System.dll")
        cp.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll")
        cp.GenerateExecutable = False
        cp.GenerateInMemory = True

        Dim code As String = CodeBuilder.ToString
        Dim cr As CodeDom.Compiler.CompilerResults = comp.CompileAssemblyFromSource(cp, code)
        Dim asm As Reflection.Assembly
        Dim t As Type
        If cr.Errors.HasErrors = False Then
            asm = cr.CompiledAssembly
            t = asm.GetType("Mode")
            mi = t.GetMethod("Func", Reflection.BindingFlags.Static Or Reflection.BindingFlags.Public)
            ret = mi.Invoke(Nothing, New Object() {})
        End If

        Return ret
    End Function
    ''' <summary>
    ''' 获取指定类型的成员和属性集合
    ''' </summary>
    ''' <param name="MyType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFields(ByVal MyType As Type) As PersistFieldCollection
        Dim i, j As Integer
        Dim pfc As New PersistFieldCollection()
        Dim MyMemberInfo() As MemberInfo
        MyMemberInfo = MyType.GetMembers(BindingFlags.Public Or BindingFlags.Instance)
        For i = MyMemberInfo.GetLowerBound(0) To MyMemberInfo.GetUpperBound(0)
            If MyMemberInfo(i).MemberType = MemberTypes.Field Or MyMemberInfo(i).MemberType = MemberTypes.Property Then
                Dim pf As New PersistField(MyMemberInfo(i).Name)
                pf.MapTo = pf.Name        '缺省与Name同名
                pf.IsPrimary = False      '缺省非主键
                pf.IsAllowNull = False    '缺省不充许空值
                pf.IsIdentity = False     '缺省非自增
                pf.Size = 0               '缺省为0，表示不判断Size
                '收集成员定制属性信息
                Dim obj() As Object = MyMemberInfo(i).GetCustomAttributes(True)
                For j = obj.GetLowerBound(0) To obj.GetUpperBound(0)
                    Dim myObj As Object = obj(j)
                    If TypeOf (myObj) Is MapToAttribute Then
                        pf.MapTo = CType(myObj, MapToAttribute).MapTo
                    ElseIf TypeOf (myObj) Is PrimaryKeyAttribute Then
                        pf.IsPrimary = CType(myObj, PrimaryKeyAttribute).IsPrimaryKey
                    ElseIf TypeOf (myObj) Is SizeAttribute Then
                        pf.Size = CType(myObj, SizeAttribute).Size
                    ElseIf TypeOf (myObj) Is AllowNullAttribute Then
                        pf.IsAllowNull = CType(myObj, AllowNullAttribute).IsAllowNull
                    ElseIf TypeOf (myObj) Is IdentityAttribute Then
                        pf.IsIdentity = CType(myObj, IdentityAttribute).IsIdentity
                    ElseIf TypeOf (myObj) Is LockFieldAttribute Then
                        pf.IsLockField = True
                    ElseIf TypeOf (myObj) Is OrderFieldAttribute Then
                        pf.IsOrderField = True
                    ElseIf TypeOf (myObj) Is NameFieldAttribute Then
                        pf.IsNameField = True
                    ElseIf TypeOf (myObj) Is TitleAttribute Then
                        pf.Title = CType(myObj, TitleAttribute).Title
                    ElseIf TypeOf (myObj) Is NonPersistedAttribute Then
                        pf.IsNonPersisted = True
                    End If
                Next

                pfc.Add(pf)
            End If
        Next
        Return pfc
    End Function
    ''' <summary>
    ''' 获取指定装配件中的所有公共类型
    ''' </summary>
    ''' <param name="AssemblyName">装配件名称</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetTypesFromAssembly(ByVal AssemblyName As String) As Type()
        Dim a As Assembly = Assembly.Load(AssemblyName)
        If a Is Nothing Then Return Nothing
        Return a.GetExportedTypes()
    End Function
    ''' <summary>
    ''' 获取指定装配件中指定名称的类型
    ''' </summary>
    ''' <param name="AssemblyName">装配件名称</param>
    ''' <param name="TypeName">类型名称</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetTypeFromAssembly(ByVal AssemblyName As String, ByVal TypeName As String) As Type
        Dim a As Assembly = Assembly.Load(AssemblyName)
        If a Is Nothing Then Return Nothing
        Return a.GetType(AssemblyName & "." & TypeName)
    End Function

End Class
