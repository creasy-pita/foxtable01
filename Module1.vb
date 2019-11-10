﻿Imports System.Data.SqlClient

Module Module1
    Public public_currentLayerSchemaDic As Dictionary(Of String, String) = New Dictionary(Of String, String)()
    Public public_GroupFieldDic As Dictionary(Of String, String) = New Dictionary(Of String, String)()
    Public public_currentDataBaseName As String
    Public public_currentDataBaseConnection As String ' 数据库连接名 可以通过此名称通过接口获取 数据库连接信息 
    Public public_currentLayerSchemaName As String ' 当前选中的图层表英文名
    Public public_currentGroupFieldName As String ' 当前选中的分组字段英文名
    Public public_currentGroupFieldChName As String ' 当前选中分组字段中文名 可以通过此名称通过接口获取字典列表作为具体分类
    Public public_currentTotalFieldEnName As String ' 当前选中统计计数字段英文名 分组后按此字段统计计数

    Public public_currentDatabaseSchema As GISQ.DataManager.Schema.DatabaseSchema ' 当前数据库配置模式信息
    'Public public_qxdmChName = "qxdm"  'qxdm 在列头显示的名称
    'Public public_qxmcChName = "qxmc" 'qxmc 在列头显示的名称
    'Public public_smcChName = "djmc" 'djmc 在列头显示的名称
    Public public_xzqbmFieldENName As String = "qxdm" '  行政区编码信息字段 固定为 qxdm
    Public public_xzqbmGroupExpression As String = "qxdm" '行政区编码 分组 表达式
    Public public_qxdmChName = "区县代码" 'qxdm 在列头显示的名称
    Public public_qxmcChName = "区县名称" 'qxmc 在列头显示的名称
    Public public_smcChName = "市级名称" 'djmc 在列头显示的名称

    Public ListDic As New List(Of GISQ.DataManager.Dictionary)
    Public ListFirst As New List(Of GISQ.DataManager.CodeName)
    Public ListSecond As New List(Of GISQ.DataManager.CodeName)
    Public ListThird As New List(Of GISQ.DataManager.CodeName)
    Public ListFourth As New List(Of GISQ.DataManager.Schema.DatasetSchema)
    Public ListFifth As New List(Of GISQ.DataManager.Schema.LayerSchema)
    Public ListSixth As New List(Of GISQ.DataManager.Schema.FieldSchema)
    Public ds As New GISQ.DataManager.Schema.DatabaseSchema
    Public bname As String '当前选中的图层表英文名
    Public fzzdname As String '当前选中分组字段英文名 可以通过此名称通过接口获取字典列表作为具体分类
    Public tjnrname As String '当前选中统计字段英文名
    Public dwzhbl As Double '当前选中数据单位和统计单位转化的比例  比如 数据单位 米 统计单位为 千米 则比例值 = 1/1000


    Sub Main()
        Dim vd As Double(,) = New Double(1, 2) {}



        Dim currentDataBaseName As String
        Dim currentDataSetSchemaName As String
        'currentDataBaseName = "现状_土地利用_土地调查" '第三级
        'currentDataSetSchemaName = "TDDC_TDDC" '第四级
        'public_currentDataBaseName = currentDataBaseName

        currentDataBaseName = "现状_土地利用_土地调查" '第三级
        Dim ds As GISQ.DataManager.Schema.DatabaseSchema = GISQ.DataManager.Schema.SchemaHelper.GetDatabaseSchemaByName(currentDataBaseName)


        Dim dbConnectionInfo As GISQ.Util.Data.DbConnectionInfo = GISQ.Framework.DbConnectHelper.GetDbConnection("xz_tdly")
        public_currentDataBaseConnection = dbConnectionInfo.GetConnectString()

        Dim pivotNames As String
        fzzdname = "地类编码" 'TBD 临时用 地类图斑

        '从字段中根据分组字段的 中文名称 获取动态列信息（包括 名称 和 值）
        ListDic = GISQ.DataManager.RuleConfigHelper.Dictionaries
        Dim currentGroupFieldRelationDic As GISQ.DataManager.Dictionary
        For Each dic As GISQ.DataManager.Dictionary In ListDic
            If dic.DictName = fzzdname Then
                currentGroupFieldRelationDic = dic
            End If
        Next

        For Each codeName As GISQ.DataManager.CodeName In currentGroupFieldRelationDic.CodeNames
            pivotNames &= "'" & codeName.Code & "'""" & codeName.Code & ""","
            'pivotNames &= "'" & codeName.Code & "'""" & codeName.Name & "_" & codeName.Code & ""","
        Next
        pivotNames = pivotNames.TrimEnd(","c)

    End Sub
    'Sub Main()
    '    Dim currentDataBaseName As String
    '    Dim currentDataSetSchemaName As String
    '    currentDataBaseName = "现状_土地利用_土地调查" '第三级
    '    currentDataSetSchemaName = "TDDC_TDDC" '第四级
    '    public_currentDataBaseName = currentDataBaseName
    '    'MsgBox(GISQ.DataManager.RuleConfigHelper.GetDictionariesFromLocal().Count.ToString())
    '    Dim ds As GISQ.DataManager.Schema.DatabaseSchema = GISQ.DataManager.Schema.SchemaHelper.GetDatabaseSchemaByName(currentDataBaseName)
    '    public_currentDataBaseConnection = ds.DefaultConnection
    '    Dim dsSchema As GISQ.DataManager.Schema.DatasetSchema
    '    For Each schema As GISQ.DataManager.Schema.DatasetSchema In ds.DatasetSchemas
    '        If schema.Name = currentDataSetSchemaName Then
    '            dsSchema = schema
    '        End If
    '    Next
    '    '获取图层列表
    '    Dim currentLayerSchemaDic As Dictionary(Of String, String) = New Dictionary(Of String, String)()

    '    Dim stasticLayerComboListString As String
    '    For Each layer As GISQ.DataManager.Schema.LayerSchema In dsSchema.LayerSchemas
    '        If Not currentLayerSchemaDic.ContainsKey(layer.AliasName) Then
    '            currentLayerSchemaDic.Add(layer.AliasName, layer.Name)
    '            stasticLayerComboListString += layer.AliasName + "|"
    '        End If
    '    Next
    '    public_currentLayerSchemaDic = currentLayerSchemaDic
    '    '挂接图层列表到 combobox中
    '    Dim layerSchemaCombobox As WinForm.ComboBox
    '    layerSchemaCombobox = Forms("主窗口").Controls("layerSchemaCombobox")
    '    layerSchemaCombobox.ComboList = stasticLayerComboListString


    'End Sub

    'Sub groupField()
    '    Dim cm As WinForm.ComboBox = e.Form.Controls("layerSchemaCombobox")
    '    MsgBox(cm.Text)

    '    Dim currentDataBaseName As String
    '    Dim currentDataSetSchemaName As String
    '    Dim currentLayerSchemaName As String
    '    currentDataBaseName = "现状_土地利用_土地调查" '第三级
    '    currentDataSetSchemaName = "TDDC_TDDC" '第四级
    '    currentLayerSchemaName = "DLTB" '第五级
    '    MsgBox(public_currentLayerSchemaDic.Count)
    '    currentLayerSchemaName = public_currentLayerSchemaDic(cm.Text)
    '    MsgBox(currentLayerSchemaName)
    '    Dim ds As GISQ.DataManager.Schema.DatabaseSchema = GISQ.DataManager.Schema.SchemaHelper.GetDatabaseSchemaByName("现状_土地利用_土地调查")
    '    Dim dsSchema As GISQ.DataManager.Schema.DatasetSchema
    '    For Each schema As GISQ.DataManager.Schema.DatasetSchema In ds.DatasetSchemas
    '        If schema.Name = currentDataSetSchemaName Then
    '            dsSchema = schema
    '        End If
    '    Next
    '    Dim groupFieldDic As Dictionary(Of String, String) = New Dictionary(Of String, String)()
    '    Dim groupFieldComboListString As String
    '    '取出 currentLayerSchema
    '    Dim currentLayerSchema As GISQ.DataManager.Schema.LayerSchema
    '    For Each layer As GISQ.DataManager.Schema.LayerSchema In dsSchema.LayerSchemas
    '        If layer.Name = currentLayerSchemaName Then
    '            currentLayerSchema = layer
    '        End If
    '    Next
    '    '获取图层中字段列表 作为分组字段
    '    Dim currentGroupFieldDic As Dictionary(Of String, String) = New Dictionary(Of String, String)()
    '    Dim currentGroupFieldComboListString As String
    '    For Each fieldSchema As GISQ.DataManager.Schema.FieldSchema In currentLayerSchema.FieldSchemas
    '        If fieldSchema.FieldType = GISQ.Util.Data.gisqFieldType.Double Or fieldSchema.FieldType = GISQ.Util.Data.gisqFieldType.Float Then 'TBD 分组字段 数据类型
    '            Continue For
    '        End If
    '        If Not currentGroupFieldDic.ContainsKey(fieldSchema.AliasName) Then
    '            currentGroupFieldDic.Add(fieldSchema.AliasName, fieldSchema.Name)
    '            currentGroupFieldComboListString += fieldSchema.AliasName + "|"
    '        End If
    '    Next
    '    public_GroupFieldDic = currentGroupFieldDic ' 复制到foxtable 全局变量中方便共享
    '    '挂接分组字段 列表到 combobox 中
    '    Dim groupFieldCmb As WinForm.ComboBox
    '    groupFieldCmb = Forms("主窗口").Controls("groupFieldCmb")
    '    groupFieldCmb.ComboList = currentGroupFieldComboListString

    'End Sub

    'Sub groupFieldChange()
    '    Dim cm As WinForm.ComboBox = e.Form.Controls("groupFieldCmb")
    '    public_currentGroupFieldName = public_GroupFieldDic(cm.Text)
    '    cm = e.Form.Controls("layerSchemaCombobox")
    '    public_currentLayerSchemaName = public_currentLayerSchemaDic(cm.Text)
    'End Sub

    'Sub doStastic()
    '    'Dim dbConnectionInfo As GISQ.Util.Data.DbConnectionInfo = GISQ.Framework.DbConnectHelper.GetDbConnection(public_currentDataBaseConnection)
    '    'Dim connectString As String = dbConnectionInfo.GetConnectString()
    '    'Data Source=192.168.0.113/orcl;User ID=gisq;Password=gisq



    '    'MsgBox(Builder.Tables("表A").Gettype().FullName) 'Foxtable.ADOTable
    '    'MsgBox(Tables("表A").Gettype().FullName) 'Foxtable.Table

    '    Dim cmd As New SqlCommand
    '    Dim dt As DataTable
    '    Dim pivotNames As String
    '    fzzdname = "地类编码" 'TBD 临时用 地类图斑

    '    '从字段中根据分组字段的 中文名称 获取动态列信息（包括 名称 和 值）
    '    ListDic = GISQ.DataManager.RuleConfigHelper.Dictionaries
    '    Dim currentGroupFieldRelationDic As GISQ.DataManager.Dictionary
    '    For Each dic As GISQ.DataManager.Dictionary In ListDic
    '        If dic.DictName = fzzdname Then
    '            currentGroupFieldRelationDic = dic
    '        End If
    '    Next

    '    For Each codeName As GISQ.DataManager.CodeName In currentGroupFieldRelationDic.CodeNames
    '        pivotNames &= "'" & codeName.Code & "'" & codeName.Name & "_" & codeName.Code & ","
    '    Next
    '    pivotNames = pivotNames.TrimEnd(","c)
    '    ' 根据选择的数据库类型 图层 分组 和 统计计数字段 组装 数据源连接串,sql 

    '    cmd.ConnectionName = "gisq113"
    '    'Dim sql As String = "select substr(zldwdm, 1, 6),substr(dlbm, 1, 2),sum(tbmj) as tbmj from DLTB where rownum<100 group by  substr(zldwdm, 1, 6),substr(dlbm, 1, 2)"
    '    'Dim sql As String = "select substr(zldwdm, 1, 6),substr(" & public_currentGroupFieldName & ", 1, 2),sum(tbmj) as tbmj from " & public_currentLayerSchemaName & " where rownum<100 group by  substr(zldwdm, 1, 6),substr(" & public_currentGroupFieldName & ", 1, 2)"

    '    Dim sql As String = "select * from (select substr(zldwdm, 1, 6),substr(dlbm, 1, 2) dlbm,sum(tbmj) tbmj from dltb where ROWNUM<100 group by substr(zldwdm, 1, 6) ,substr(dlbm, 1, 2))  pivot(sum(tbmj) for  dlbm in (" & pivotNames & "))"
    '    MsgBox(sql)
    '    'cmd.CommandText = sql
    '    'dt = cmd.ExecuteReader()

    '    ''动态列
    '    '删除原先 table列
    '    Dim Builder As New ADOXBuilder
    '    Builder.Open()
    '    For i As Integer = Tables("表A").Cols.Count - 1 To 0
    '        With Builder.Tables("表A")
    '            .Remove(Tables("表A").Cols)
    '        End With
    '    Next

    '    'Dim Names As String()
    '    'Names = New String() {"湿地_00", "耕地_01", "种植用地_02"}
    '    'Dim Builder As New ADOXBuilder
    '    'Builder.Open()
    '    'For i As Integer = 0 To Names.Length - 1
    '    '    With Builder.Tables("表A")
    '    '        .AddColumn(Names(i), ADOXType.Double)
    '    '    End With
    '    'Next
    '    'Builder.Close()
    '    'DataTables.Unload("表A")
    '    'DataTables.load("表A")
    '    'MainTable = Tables("表A")
    '    'MsgBox(dt.GetType().FullName) 'Foxtable.Table
    'End Sub
    'Sub doStastic()
    '    '根据选中的 第四层分类确定 获取数据库连接信息 并保存到 Connections中 
    '    If public_currentDataBaseName Is Nothing Then
    '        public_currentDataBaseName = "现状_土地利用_土地调查"
    '    End If

    '    If public_currentDatabaseSchema Is Nothing Then
    '        public_currentDatabaseSchema = GISQ.DataManager.Schema.SchemaHelper.GetDatabaseSchemaByName(public_currentDataBaseName)
    '    End If
    '    If public_currentDatabaseSchema Is Nothing Then
    '        MsgBox("通过配置名称【" & public_currentDataBaseName & "】获取的DatabaseSchema为空 请检查")
    '        Return
    '    End If
    '    'TBD public_currentDatabaseSchema public_currentDataBaseConnection 处理为空的场景
    '    public_currentDataBaseConnection = public_currentDatabaseSchema.DefaultConnection
    '    If public_currentDataBaseConnection Is Nothing Then
    '        MsgBox("通过 名称【" & public_currentDataBaseName & "】获取的DatabaseSchema 获取不到数据库连接名信息 请检查")
    '        Return
    '    End If
    '    Dim dbConnectionInfo As GISQ.Util.Data.DbConnectionInfo = GISQ.Framework.DbConnectHelper.GetDbConnection(public_currentDataBaseConnection)
    '    Dim connectString As String = dbConnectionInfo.GetConnectString()
    '    If connectString Is Nothing Then
    '        MsgBox("通过 名称【" & public_currentDataBaseName & "】获取的DatabaseSchema 获取不到数据库连接串信息 请检查")
    '        Return
    '    End If
    '    '并保存到 Connections中 
    '    If Not Connections.Contains(public_currentDataBaseConnection) Then
    '        Connections.Add(public_currentDataBaseConnection, connectString)
    '        MsgBox("Connections add " & public_currentDataBaseConnection & connectString)
    '    End If
    '    'Data Source=192.168.0.113/orcl;User ID=gisq;Password=gisq


    '    'MsgBox(Builder.Tables("表A").Gettype().FullName) 'Foxtable.ADOTable
    '    'MsgBox(Tables("表A").Gettype().FullName) 'Foxtable.Table
    '    Dim pivotNames As String

    '    Dim cm As WinForm.ComboBox = e.Form.Controls("groupFieldCmb") 'TBD 此处菜单形式的获取方式会不同
    '    public_currentGroupFieldChName = cm.Text
    '    public_currentGroupFieldName = public_GroupFieldDic(cm.Text)
    '    MsgBox(public_currentGroupFieldName)
    '    fzzdname = public_currentGroupFieldChName ' "地类编码" 'TBD 临时用 地类图斑
    '    '从字段中根据分组字段的 中文名称 获取动态列信息(包括 名称 和 值)
    '    ListDic = GISQ.DataManager.RuleConfigHelper.Dictionaries 'TBD ListDic 已经有值 不需要内容
    '    Dim currentGroupFieldRelationDic As GISQ.DataManager.Dictionary

    '    For Each dic As GISQ.DataManager.Dictionary In ListDic
    '        If dic.DictName = fzzdname Then
    '            currentGroupFieldRelationDic = dic
    '        End If
    '    Next
    '    If currentGroupFieldRelationDic Is Nothing Or currentGroupFieldRelationDic.CodeNames.Count = 0 Then
    '        MsgBox("分组字段【" & public_currentGroupFieldChName & "】没有获取到关联的数据字典项 不能进行统计 /r/n 分组字段需要获取到关联的数据字典项才能统计 请检查关联的数据字典项配置")
    '        Return
    '    End If
    '    If currentGroupFieldRelationDic.CodeNames.Count = 0 Then
    '        MsgBox("分组字段【" & public_currentGroupFieldChName & "】关联的数据字典项个数为0 不能进行统计 /r/n 分组字段需要有关联的数据字典项才能统计 请检查关联的数据字典项配置")
    '        Return
    '    End If
    '    For Each codeName As GISQ.DataManager.CodeName In currentGroupFieldRelationDic.CodeNames
    '        pivotNames &= "'" & codeName.Code & "' """ & codeName.Code & ""","
    '        'pivotNames &= "'" & codeName.Code & "'""" & codeName.Name & "_" & codeName.Code & ""","
    '    Next

    '    pivotNames = pivotNames.TrimEnd(","c)
    '    ' output.show(pivotNames)

    '    'TBD 需要去除一下预设 从用户选择中获取
    '    Dim groupFieldExpression As String = "substr(dlbm, 1, 2)"
    '    Dim xzqbmFieldENName As String = "zldwdm" ' 行政区编码信息字段
    '    Dim xzqbmGroupExpression As String = "substr(zldwdm, 1, 6)" '行政区编码 分组 表达式
    '    Dim totalFieldEnName As String = "tbmj" ' 分组计数字段
    '    public_currentTotalFieldEnName = totalFieldEnName

    '    ' 0 xzqbmGroupExpression  1 xzqbmFieldENName 2 groupFieldExpression 3 public_currentGroupFieldName
    '    ' 4 totalFieldEnName 5 public_currentLayerSchemaName 6 pivotNames

    '    Dim sql As String = "select * from (select {0} {1},{2} {3}, sum({4}) {4} from {5} where ROWNUM<10000 group by {0}, {2}) pivot (sum({4}) For {3} In ({6})) "
    '    sql = String.Format(sql, xzqbmGroupExpression, xzqbmFieldENName, groupFieldExpression, public_currentGroupFieldName, public_currentTotalFieldEnName, public_currentLayerSchemaName, pivotNames)
    '    MsgBox(sql)
    '    DataTables("表A").Fill(sql, public_currentDataBaseConnection, True)
    '    'DataTables("表A").Fill("select * from (select substr(zldwdm, 1, 6) zldwdm,substr(dlbm, 1, 2) dlbm,sum(tbmj) tbmj from dltb where ROWNUM<10000 group by substr(zldwdm, 1, 6) ,substr(dlbm, 1, 2))  pivot(sum(tbmj) for  dlbm in (" & pivotNames & "))", "gisq113", True)
    '    ' output.show("select * from (select substr(zldwdm, 1, 6) zldwdm,substr(dlbm, 1, 2) dlbm,sum(tbmj) tbmj from dltb where ROWNUM<10000 group by substr(zldwdm, 1, 6) ,substr(dlbm, 1, 2))  pivot(sum(tbmj) for  dlbm in (" & pivotNames  & "))")
    'End Sub

    Sub doStastic()
        '根据选中的 第四层分类确定 获取数据库连接信息 并保存到 Connections中 
        '需要用到全局变量有  
        '    Public ds As New GISQ.DataManager.Schema.DatabaseSchema ->  public_currentDatabaseSchema
        '    Public bname As String '表名称 -> public_currentLayerSchemaName
        '   Public fzzdname As String '分组字段名称   -》public_currentGroupFieldChName 
        '   public_currentGroupFieldChName 从 RibbonTabs("数据统计").Groups("功能组1").Items("工具栏5").Items("Combox10").Text

        public_currentDatabaseSchema = ds
        public_currentLayerSchemaName = bname
        public_currentGroupFieldName = fzzdname '比如 "地类编码"
        public_currentTotalFieldEnName = tjnrname '比如 "图斑面积:tbmj"

        If public_currentGroupFieldName Is Nothing Then
            MsgBox("分组字段名称为空 请检查")
            'Return
        End If
        If public_currentDatabaseSchema Is Nothing Then
            MsgBox("通过配置名称【" & public_currentDataBaseName & "】获取的DatabaseSchema为空 请检查")
            'Return
        End If

        'TBD public_currentDatabaseSchema public_currentDataBaseConnection 处理为空的场景
        public_currentDataBaseConnection = public_currentDatabaseSchema.DefaultConnection

        If public_currentDataBaseConnection Is Nothing Then
            MsgBox("通过 名称【" & public_currentDataBaseName & "】获取的DatabaseSchema 获取不到数据库连接名信息 请检查")
            'Return
        End If

        Dim dbConnectionInfo As GISQ.Util.Data.DbConnectionInfo = GISQ.Framework.DbConnectHelper.GetDbConnection(public_currentDataBaseConnection)
        If dbConnectionInfo Is Nothing Then
            MsgBox("通过传递 名称【" & public_currentDataBaseConnection & "】调用接口GISQ.Framework.DbConnectHelper.GetDbConnection 获取的 数据库连接信息为空 请检查DbConnectionInfo的配置")
            'Return
        End If
        Dim connectString As String = "Provider=MSDAORA.1;" & dbConnectionInfo.GetConnectString()

        If connectString Is Nothing Then
            MsgBox("通过 名称【" & public_currentDataBaseName & "】获取的DatabaseSchema 获取不到数据库连接串信息 请检查")
            'Return
        End If

        '并保存到 Connections中 
        If Not Connections.Contains(public_currentDataBaseConnection) Then
            Connections.Add(public_currentDataBaseConnection, connectString)
            'MsgBox("Connections add " & public_currentDataBaseConnection & connectString)
        End If


        public_currentGroupFieldChName = RibbonTabs("数据统计").Groups("功能组1").Items("工具栏5").Items("Combox10").Text
        If public_currentGroupFieldChName Is Nothing Then
            MsgBox("分组字段中文名称为空 请检查")
            'Return
        End If
        'MsgBox(public_currentGroupFieldName)
        '从字段中根据分组字段的 中文名称 获取动态列信息(包括 名称 和 值)
        ListDic = GISQ.DataManager.RuleConfigHelper.Dictionaries 'TBD ListDic 已经有值 不需要内容
        Dim currentGroupFieldRelationDic As GISQ.DataManager.Dictionary

        For Each dic As GISQ.DataManager.Dictionary In ListDic
            If dic.DictID = public_currentGroupFieldName Then
                currentGroupFieldRelationDic = dic
            End If
        Next

        If currentGroupFieldRelationDic Is Nothing OrElse currentGroupFieldRelationDic.CodeNames.Count = 0 Then
            MsgBox("分组字段【" & public_currentGroupFieldChName & "】没有获取到关联的数据字典项 不能进行统计" & vbCrLf & "分组字段需要获取到关联的数据字典项才能统计 请检查关联的数据字典项配置")
            'Return
        End If
        If currentGroupFieldRelationDic.CodeNames.Count = 0 Then
            MsgBox("分组字段【" & public_currentGroupFieldChName & "】关联的数据字典项个数为0 不能进行统计" & vbCrLf & "分组字段需要有关联的数据字典项才能统计 请检查关联的数据字典项配置")
            'Return
        End If

        Dim pivotNames As String '行转列的sql 语句段
        Dim foxtableTotalOnCols As String ' 比如 湿地_00,耕地_01,种植园用地_02
        '加入 Where判断 字典中重复的 Code 只加入一次
        Dim pivotNameList As List(Of String) = New List(Of String)()
        For Each codeName As GISQ.DataManager.CodeName In currentGroupFieldRelationDic.CodeNames
            If Not pivotNameList.Contains(codeName.Code) Then
                pivotNameList.Add(codeName.Code)
                'pivotNames &= "'" & codeName.Code & "'""" & codeName.Code & "_" & codeName.Code & ""","
                'foxtableTotalOnCols &= codeName.Code & "_" & codeName.Code & ","
                pivotNames &= "'" & codeName.Code & "'""" & codeName.Name & "_" & codeName.Code & ""","
                foxtableTotalOnCols &= codeName.Name & "_" & codeName.Code & ","
            End If
        Next

        pivotNames = pivotNames.TrimEnd(","c)
        foxtableTotalOnCols = foxtableTotalOnCols.TrimEnd(","c)
        ' output.show(pivotNames)

        Dim groupFieldExpression As String
        Dim xzqbmFieldENName As String = public_xzqbmFieldENName ' 行政区编码信息字段
        Dim xzqbmGroupExpression As String = public_xzqbmGroupExpression '行政区编码 分组 表达式
        Dim qxdmChName = public_qxdmChName
        Dim qxmcChName = public_qxmcChName
        Dim smcChName = public_smcChName
        Dim sjdmName = "djdm"
        Dim stasticCol = 4
        groupFieldExpression = public_currentGroupFieldName

        xzqbmGroupExpression = xzqbmFieldENName
        If xzqbmGroupExpression Is Nothing Then
            MsgBox("行政区分组表达式【" & xzqbmGroupExpression & "】为空 请检查")
            'Return
        End If
        If xzqbmFieldENName Is Nothing Then
            MsgBox("行政区字段【" & xzqbmFieldENName & "】为空 请检查")
            'Return
        End If
        If groupFieldExpression Is Nothing Then
            MsgBox("行政区分组表达式【" & groupFieldExpression & "】为空 请检查")
            'Return
        End If
        If xzqbmFieldENName Is Nothing Then
            MsgBox("行政区字段【" & xzqbmFieldENName & "】为空 请检查")
            'Return
        End If
        dwzhbl = 1
        If dwzhbl = 0 Then
            MsgBox("单位转换比例没有设置 请检查")
            'Return
        End If

        ' 0 xzqbmGroupExpression  1 xzqbmFieldENName 2 groupFieldExpression 3 public_currentGroupFieldName
        ' 4 totalFieldEnName 5 public_currentLayerSchemaName 6 pivotNames 7 dwzhbl 8 qxdmChName 9 qxmcChName 10 smcChName 11 sjdmName
        '组装sql 分组统计的sql语句
        Dim sql As String = "select substr(""{8}"",1,4) as ""{11}"",substr(""{8}"",1,4) as ""{10}"", ""{8}"" as ""{9}"",tb.* from (select {0} ""{8}"",{2} {3}, sum({4}) {4} from {5} group by {0}, {2}) pivot (sum({4}*{7}) For {3} In ({6})) tb  ORDER BY tb.""{8}"""
        'Dim sql As String = "select substr(qxdm,1,4) as djdm, qxdm as qxmc,tb.* from (select {0} {1},{2} {3}, sum({4}) {4} from {5} where ROWNUM<10000 group by {0}, {2}) pivot (sum({4}*{7}) For {3} In ({6})) tb "
        sql = String.Format(sql, xzqbmGroupExpression, xzqbmFieldENName, groupFieldExpression, public_currentGroupFieldName, public_currentTotalFieldEnName, public_currentLayerSchemaName, pivotNames, dwzhbl, qxdmChName, qxmcChName, smcChName, sjdmName)
        'output.show(sql)
        Dim stasticTableName As String = "表A"
        Dim t As Table = Tables(stasticTableName)
        MainTable = t
        t.Visible = False
        DataTables(stasticTableName).Fill(sql, public_currentDataBaseConnection, True)

        t.Cols(sjdmName).Visible = False
        t.Cols(smcChName).Visible = False

        For i As Integer = 0 To t.Rows.Count - 1
            't.Rows(i)(qxmcChName) = "合肥"
            t.Rows(i)(smcChName) = FoxtableXZQ.XZQClass.GetXZQNameByCode(t.Rows(i)(smcChName))
            t.Rows(i)(qxmcChName) = FoxtableXZQ.XZQClass.GetXZQNameByCode(t.Rows(i)(qxdmChName))
            For colIndex As Integer = stasticCol To DataTables(stasticTableName).DataCols.Count - 1
                t.Rows(i)(colIndex) = Round2(t.Rows(i)(colIndex), 2)
            Next
        Next

        MainTable = t
        t.GroupAboveData = True '指定分组行位于数据行之上
        t.SubtotalGroups.Clear() '清除原有的分组
        Dim g As SubtotalGroup '定义一个新的分组
        g = New SubtotalGroup
        g.GroupOn = smcChName '分组列为地级市代码
        g.TotalOn = foxtableTotalOnCols '对数量和金额进行统计
        t.SubtotalGroups.Add(g) '加入刚刚定义的分组

        g = New SubtotalGroup
        g.GroupOn = "*" '总计
        g.Caption = "安徽省"
        g.TotalOn = foxtableTotalOnCols '对数量和金额进行统计
        t.Sort = qxdmChName
        t.SubtotalGroups.Add(g) '加入刚刚定义的分组
        t.Subtotal(True) '生成汇总模式

        t.Visible = True


        'DataTables("表A").Fill("select * from (select substr(zldwdm, 1, 6) zldwdm,substr(dlbm, 1, 2) dlbm,sum(tbmj) tbmj from dltb where ROWNUM<10000 group by substr(zldwdm, 1, 6) ,substr(dlbm, 1, 2))  pivot(sum(tbmj) for  dlbm in (" & pivotNames & "))", "gisq113", True)
        ' output.show("select * from (select substr(zldwdm, 1, 6) zldwdm,substr(dlbm, 1, 2) dlbm,sum(tbmj) tbmj from dltb where ROWNUM<10000 group by substr(zldwdm, 1, 6) ,substr(dlbm, 1, 2))  pivot(sum(tbmj) for  dlbm in (" & pivotNames  & "))")
    End Sub

    Sub showChart()
        '从表A获取数值 判断是否已经有统计数据
        Dim stasticTableName As String = "表A"
        Dim t As Table = Tables(stasticTableName)
        If t Is Nothing Then
            MsgBox(stasticTableName & "为空，请检查")
            Return
        End If
        If t.Rows.Count = 0 Then
            MsgBox(stasticTableName & "数据为空，请先统计报表数据")
            Return
        End If
        '只有一行数据而且  00 位置为空时 表示没有数据
        If t.Rows.Count = 1 And t.Rows(0)(0) = "" Then
            MsgBox(stasticTableName & "数据为空,请先统计报表数据")
            Return
        End If


        Dim qxmcString As String
        Dim sjmcString As String
        Dim lbsString As String
        Dim qxmcStringList() As String '选中区县名称列表 
        Dim sjmcStringList() As String '选中地级市名称列表 如果区县名称列表不为空则使用区县名称列表画图 
        Dim xzqStringList() As String  'x维度  为行政区
        Dim xzqColName As String '行政区匹配时使用的列名 选中区县名称列表 使用public_qxmcChName 否则 public_smcChName

        ' 获取 区县
        Dim CheckedComboCol As WinForm.CheckedComboBox
        CheckedComboCol = Forms("图表展示").Controls("CheckedComboCol")
        output.show(CheckedComboCol.Text)
        lbsString = CheckedComboCol.Text
        Dim ComboShi As WinForm.ComboBox
        ComboShi = Forms("图表展示").Controls("ComboShi")
        output.show(ComboShi.Text)
        sjmcString = ComboShi.Text
        Dim ComboXian As WinForm.ComboBox
        ComboXian = Forms("图表展示").Controls("ComboXian")
        output.show(ComboXian.Text)
        qxmcString = ComboXian.Text

        If String.IsNullOrEmpty(qxmcString) And String.IsNullOrEmpty(sjmcString) Then
            MsgBox("请先选择行政区")
            Return
        End If
        If String.IsNullOrEmpty(lbsString) Then
            MsgBox("请先选择分组类别")
            Return
        End If

        sjmcStringList = {"合肥市", "阜阳市", "安庆市"}
        Dim lbStringList As String() = {"耕地_01", "园地_02", "林地_03"} 'Y维度为 分组的类别
        If Not String.IsNullOrEmpty(qxmcString) Then
            qxmcStringList = qxmcString.Split(","c)
        Else
            sjmcStringList = sjmcString.Split(","c)
        End If
        lbStringList = lbsString.Split(","c)

        If qxmcStringList IsNot Nothing Then
            xzqStringList = qxmcStringList
            xzqColName = public_qxmcChName
        Else
            xzqStringList = sjmcStringList
            xzqColName = public_smcChName
        End If

        Dim valueDeList As Double(,) = New Double(xzqStringList.Length, lbStringList.Length) {} '二维数组 记录x,y维度上的统计值
        Dim xzqDic As Dictionary(Of String, Short) = New Dictionary(Of String, Short)

        Dim i = 0
        ' 字典用于保存行政区名称,和对应的位置 后续用于通过名称找到二维数组的下标位置
        For Each xzq As String In xzqStringList
            xzqDic.Add(xzq, i)
            i = i + 1
        Next
        i = 0
        ' 字典用于保存类别名称,和对应的位置 后续用于通过名称找到二维数组的下标位置
        Dim lbDic As Dictionary(Of String, Short) = New Dictionary(Of String, Short)
        For Each lb As String In lbStringList
            lbDic.Add(lb, i)
            i = i + 1
        Next

        For rowIndex As Integer = 1 To t.Rows.Count
            Dim xzqmc = t.Rows(rowIndex)(xzqColName)

            If xzqDic.ContainsKey(xzqmc) Then
                ' 先固定获取 lbStringList(0) 列的值到  二维数组的对用行 列下标下  
                For Each lb As String In lbStringList
                    valueDeList(xzqDic(xzqmc), lbDic(lb)) = valueDeList(xzqDic(xzqmc), lbDic(lb)) + t.Rows(rowIndex)(lb)
                Next
            End If
        Next
        Dim chartType = Forms("图表展示").Controls("ComboBox1").Text '"饼状图"
        Dim Chart As WinForm.Chart '定义一个图表变量
        Dim Series As WinForm.ChartSeries '定义一个图系变量
        Chart = Forms("图表展示").Controls("Chart1") ' 引用窗口中的图表
        Chart.SeriesList.Clear() '清除图表原来的图系
        Chart.LegendVisible = True '显示图例

        Dim sum As Double = 0
        For colIndex As Integer = 0 To valueDeList.GetLength(1) - 1
            sum = sum + valueDeList(0, colIndex)
        Next

        Chart.AxisX.ClearValueLabel()
        If chartType = "饼状图" Then
            Chart.VisualEffect = True '加上这一行,让你的图表更漂亮
            Chart.ChartType = ChartTypeEnum.Pie '图表1类型改为Bar(条形)
            For Each lb As String In lbStringList
                Series = Chart.SeriesList.Add() '增加一个图系
                Series.Length = 1 '一个系列只能包括一个值
                Series.Text = lb & "(" & valueDeList(0, lbDic(lb)) & ")" '设置图系的标题
                Series.DataLabelText = Math.Round(valueDeList(0, lbDic(lb)) * 100 / sum, 2) & "%" '计算百分比
                Series.Y(0) = valueDeList(0, lbDic(lb)) '指定值

            Next
            Chart.LegendCompass = CompassEnum.East '图列显示在东方(右方)

        Else

            Chart.ChartType = ChartTypeEnum.Bar

            For xzqIndex As Integer = 0 To xzqStringList.Count - 1
                Chart.AxisX.SetValueLabel(xzqIndex, xzqStringList(xzqIndex)) 'x轴指定字符表示
            Next
            Chart.AxisY.Text = RibbonTabs("数据统计")("功能组2")("统计单位").Text 'x轴指定字符表示

            For Each lb As String In lbStringList
                Series = Chart.SeriesList.Add() '增加一个图系
                Series.Length = xzqStringList.Length
                Series.TooltipText = "X = {#XVAL}, Y = {#YVAL}"
                Series.Text = lb.Substring(0, lb.LastIndexOf("_")) '截取去除下划线及编码部分
                For xzqIndex As Integer = 0 To xzqStringList.Count - 1
                    Series.X(xzqIndex) = xzqIndex
                    Series.Y(xzqIndex) = valueDeList(xzqDic(xzqStringList(xzqIndex)), lbDic(lb))
                Next
            Next
            Chart.AxisX.AnnoWithLabels = True
        End If
    End Sub



    Sub doHistStastic()
        '根据选中的 第四层分类确定 获取数据库连接信息 并保存到 Connections中 
        '需要用到全局变量有  
        '    Public ds As New GISQ.DataManager.Schema.DatabaseSchema ->  public_currentDatabaseSchema
        '    Public bname As String '表名称 -> public_currentLayerSchemaName
        '   Public fzzdname As String '分组字段名称   -》public_currentGroupFieldChName 
        '   public_currentGroupFieldChName 从 RibbonTabs("数据统计").Groups("功能组1").Items("工具栏5").Items("Combox10").Text

        public_currentDatabaseSchema = ds
        public_currentLayerSchemaName = bname
        public_currentGroupFieldName = fzzdname '比如 "地类编码"
        public_currentTotalFieldEnName = tjnrname '比如 "图斑面积:tbmj"

        If public_currentGroupFieldName Is Nothing Then
            MsgBox("分组字段名称为空 请检查")
            'Return
        End If
        If public_currentDatabaseSchema Is Nothing Then
            MsgBox("通过配置名称【" & public_currentDataBaseName & "】获取的DatabaseSchema为空 请检查")
            'Return
        End If

        'TBD public_currentDatabaseSchema public_currentDataBaseConnection 处理为空的场景
        public_currentDataBaseConnection = public_currentDatabaseSchema.DefaultConnection

        If public_currentDataBaseConnection Is Nothing Then
            MsgBox("通过 名称【" & public_currentDataBaseName & "】获取的DatabaseSchema 获取不到数据库连接名信息 请检查")
            'Return
        End If

        Dim dbConnectionInfo As GISQ.Util.Data.DbConnectionInfo = GISQ.Framework.DbConnectHelper.GetDbConnection(public_currentDataBaseConnection)
        If dbConnectionInfo Is Nothing Then
            MsgBox("通过传递 名称【" & public_currentDataBaseConnection & "】调用接口GISQ.Framework.DbConnectHelper.GetDbConnection 获取的 数据库连接信息为空 请检查DbConnectionInfo的配置")
            'Return
        End If
        Dim connectString As String = "Provider=MSDAORA.1;" & dbConnectionInfo.GetConnectString()

        If connectString Is Nothing Then
            MsgBox("通过 名称【" & public_currentDataBaseName & "】获取的DatabaseSchema 获取不到数据库连接串信息 请检查")
            'Return
        End If

        '并保存到 Connections中 
        If Not Connections.Contains(public_currentDataBaseConnection) Then
            Connections.Add(public_currentDataBaseConnection, connectString)
            'MsgBox("Connections add " & public_currentDataBaseConnection & connectString)
        End If


        public_currentGroupFieldChName = RibbonTabs("数据统计").Groups("功能组1").Items("工具栏5").Items("Combox10").Text
        If public_currentGroupFieldChName Is Nothing Then
            MsgBox("分组字段中文名称为空 请检查")
            'Return
        End If
        'MsgBox(public_currentGroupFieldName)
        '从字段中根据分组字段的 中文名称 获取动态列信息(包括 名称 和 值)
        ListDic = GISQ.DataManager.RuleConfigHelper.Dictionaries 'TBD ListDic 已经有值 不需要内容
        Dim currentGroupFieldRelationDic As GISQ.DataManager.Dictionary

        For Each dic As GISQ.DataManager.Dictionary In ListDic
            If dic.DictID = public_currentGroupFieldName Then
                currentGroupFieldRelationDic = dic
            End If
        Next

        If currentGroupFieldRelationDic Is Nothing OrElse currentGroupFieldRelationDic.CodeNames.Count = 0 Then
            MsgBox("分组字段【" & public_currentGroupFieldChName & "】没有获取到关联的数据字典项 不能进行统计" & vbCrLf & "分组字段需要获取到关联的数据字典项才能统计 请检查关联的数据字典项配置")
            'Return
        End If
        If currentGroupFieldRelationDic.CodeNames.Count = 0 Then
            MsgBox("分组字段【" & public_currentGroupFieldChName & "】关联的数据字典项个数为0 不能进行统计" & vbCrLf & "分组字段需要有关联的数据字典项才能统计 请检查关联的数据字典项配置")
            'Return
        End If


        Dim groupFieldExpression As String
        Dim xzqbmFieldENName As String = public_xzqbmFieldENName ' 行政区编码信息字段
        Dim xzqbmGroupExpression As String = public_xzqbmGroupExpression '行政区编码 分组 表达式
        Dim qxdmChName = public_qxdmChName
        Dim qxmcChName = public_qxmcChName
        Dim smcChName = public_smcChName
        Dim sjdmName = "djdm"
        Dim stasticCol = 3
        groupFieldExpression = public_currentGroupFieldName

        xzqbmGroupExpression = xzqbmFieldENName
        If xzqbmGroupExpression Is Nothing Then
            MsgBox("行政区分组表达式【" & xzqbmGroupExpression & "】为空 请检查")
            'Return
        End If
        If xzqbmFieldENName Is Nothing Then
            MsgBox("行政区字段【" & xzqbmFieldENName & "】为空 请检查")
            'Return
        End If
        If groupFieldExpression Is Nothing Then
            MsgBox("行政区分组表达式【" & groupFieldExpression & "】为空 请检查")
            'Return
        End If
        If xzqbmFieldENName Is Nothing Then
            MsgBox("行政区字段【" & xzqbmFieldENName & "】为空 请检查")
            'Return
        End If
        If dwzhbl = 0 Then
            MsgBox("单位转换比例没有设置 请检查")
            'Return
        End If


        'datayearColName, public_currentGroupFieldChName, sjdmValue, GroupFieldAliasChName
        Dim datayearColName = "datayear"
        Dim sjdmValue = XzqhCode
        Dim GroupFieldAliasChName = "名称"
        Dim currentDataYearTbNameSuffix = "" 'TBD 当前年份时表的后缀 备注：有可能当前年份提前生成到历史表 比如 _2019
        '加载年份
        Dim dataYearDic As Dictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)
        Dim dataYearList As String()
        Dim year As Integer = Val(RibbonTabs("数据统计")("功能组3")("TextYear").Text)

        Dim now As Integer = Format(Date.Now, "yyyy")
        If year > now Or year < 1949 Then
            MsgBox("统计年份输入有误")
        Else
            Dim ssc As SqlSugar.SqlSugarClient
            ssc = FoxtableXZQ.XZQClass.GetSqlSugarClient(dbConnectionInfo)

            If year < now Then

                For i As Integer = year To now
                    Dim TableName As String = public_currentLayerSchemaName
                    If i < now Then
                        TableName = TableName & "_" & i
                    End If
                    dataYearDic.Add(i.ToString(), ssc.DbMaintenance.IsAnyTable(TableName, False))
                Next
            End If
        End If
        dataYearList = dataYearDic.Keys.ToArray()


        Dim pivotNames As String = "" '数据年份行转列的sql 语句段
        Dim datayearCols As String = ""
        '加入 Where判断 字典中重复的 Code 只加入一次
        For Each dataYear As String In dataYearList
            pivotNames &= dataYear & " """ & dataYear & ""","
            datayearCols &= """" & dataYear & ""","
        Next
        pivotNames = pivotNames.TrimEnd(","c)
        datayearCols = datayearCols.TrimEnd(","c)
        ' output.show(pivotNames)

        ' 0 xzqbmGroupExpression  1 xzqbmFieldENName 2 groupFieldExpression 3 public_currentGroupFieldName
        ' 4 totalFieldEnName 5 public_currentLayerSchemaName 6 pivotNames 7 sjdmName 8  datayearColName 
        ' 9 public_currentGroupFieldChName 10 sjdmValue 11 GroupFieldAliasChName 12 dwzhbl 13 datayearcols
        '组装sql 分组统计的sql语句
        Dim innerSql As String
        For Each dataYear As String In dataYearList
            If Not dataYearDic(dataYear) Then
                Continue For
            End If
            Dim dataYearTbNameSuffix = ""
            If DateTime.Now.Year.ToString() = dataYear Then
                dataYearTbNameSuffix = currentDataYearTbNameSuffix
            Else
                dataYearTbNameSuffix = "_" + dataYear
            End If

            If String.IsNullOrEmpty(sjdmValue) Then
                innerSql = innerSql & "select '" & dataYear & "' As {8}, {3} ""{9}"", sum({4}) {4},substr({1},1,4) ""{7}"" from {5}" & dataYearTbNameSuffix & " where 1=1 and rownum<10000  group by substr({1},1,4), {3} "
            Else
                innerSql = innerSql & "select '" & dataYear & "' As {8}, {3} ""{9}"", sum({4}) {4},substr({1},1,4) ""{7}"" from {5}" & dataYearTbNameSuffix & " where 1=1 and rownum<10000 And {1} like '{10}%' group by substr({1},1,4), {3} "
            End If
            innerSql = innerSql & " Union All "
        Next
        'TBD 去除 末尾的 Union All
        innerSql = innerSql.Substring(0, innerSql.LastIndexOf("Union All"))
        ' 组装完整sql 
        'select  tb."地类编码","地类编码" As "名称",TB."djdm","2017","2018","2019"
        Dim sql = "select tb.""{9}"", ""{9}"" As ""{11}"", tb.""{7}"",{13} from ("
        sql = sql & "Select uniontb.* from ("
        sql = sql & innerSql
        sql = sql & ") uniontb )  pivot (sum({4}*{12}) For {8} In ({6})) tb order by tb.""{9}"""

        'Dim sql As String = "select substr(qxdm,1,4) as djdm, qxdm as qxmc,tb.* from (select {0} {1},{2} {3}, sum({4}) {4} from {5} where ROWNUM<10000 group by {0}, {2}) pivot (sum({4}*{7}) For {3} In ({6})) tb "
        sql = String.Format(sql, xzqbmGroupExpression, xzqbmFieldENName, groupFieldExpression, public_currentGroupFieldName, public_currentTotalFieldEnName, public_currentLayerSchemaName, pivotNames, sjdmName, datayearColName, public_currentGroupFieldChName, sjdmValue, GroupFieldAliasChName, dwzhbl, datayearCols)
        'output.show(sql)
        Dim stasticTableName As String = "表A"
        Dim t As Table = Tables(stasticTableName)
        MainTable = t
        t.Visible = False
        DataTables(stasticTableName).Fill(sql, public_currentDataBaseConnection, True)

        Dim CodeAndNameDic As Dictionary(Of String, String) = New Dictionary(Of String, String)()
        For Each codeName As GISQ.DataManager.CodeName In currentGroupFieldRelationDic.CodeNames
            If CodeAndNameDic.ContainsKey(codeName.Code) Then
                Continue For
            End If
            CodeAndNameDic.Add(codeName.Code, codeName.Name)
        Next

        For i As Integer = 0 To t.Rows.Count - 1
            '地类名称 列 根据地类编码获取地类名称
            If CodeAndNameDic.ContainsKey(t.Rows(i)(public_currentGroupFieldChName)) Then
                t.Rows(i)(GroupFieldAliasChName) = CodeAndNameDic(t.Rows(i)(public_currentGroupFieldChName))
            End If
            For colIndex As Integer = stasticCol To DataTables(stasticTableName).DataCols.Count - 1
                t.Rows(i)(colIndex) = Round2(t.Rows(i)(colIndex), 2)
            Next
        Next

        MainTable = t

        t.Visible = True


        'DataTables("表A").Fill("select * from (select substr(zldwdm, 1, 6) zldwdm,substr(dlbm, 1, 2) dlbm,sum(tbmj) tbmj from dltb where ROWNUM<10000 group by substr(zldwdm, 1, 6) ,substr(dlbm, 1, 2))  pivot(sum(tbmj) for  dlbm in (" & pivotNames & "))", "gisq113", True)
        ' output.show("select * from (select substr(zldwdm, 1, 6) zldwdm,substr(dlbm, 1, 2) dlbm,sum(tbmj) tbmj from dltb where ROWNUM<10000 group by substr(zldwdm, 1, 6) ,substr(dlbm, 1, 2))  pivot(sum(tbmj) for  dlbm in (" & pivotNames  & "))")
    End Sub

    Sub showHistChart()
        '从表A获取数值 判断是否已经有统计数据
        Dim stasticTableName As String = "表A"
        Dim t As Table = Tables(stasticTableName)
        If t Is Nothing Then
            MsgBox(stasticTableName & "为空，请检查")
            Return
        End If
        If t.Rows.Count = 0 Then
            MsgBox(stasticTableName & "数据为空，请先统计报表数据")
            Return
        End If
        '只有一行数据而且  00 位置为空时 表示没有数据
        If t.Rows.Count = 1 And t.Rows(0)(0) = "" Then
            MsgBox(stasticTableName & "数据为空,请先统计报表数据")
            Return
        End If


        Dim groupLBString As String
        Dim datayearsString As String 'y 轴 选中年份列表名称列表 
        Dim groupLBStringList() As String 'x 轴 选中分组类别名称列表 
        Dim datayearStringList As String()  'Y维度为 分组的类别
        Dim groupFieldColName As String
        groupFieldColName = t.Cols(0).Name '比如 地类编码
        ' 获取 年份列表 和 分组类别 
        'Dim CheckedComboCol As WinForm.CheckedComboBox
        'CheckedComboCol = Forms("图表展示").Controls("CheckedComboCol")
        'output.show(CheckedComboCol.Text)
        'datayearsString = CheckedComboCol.Text

        'Dim ComboXian As WinForm.ComboBox
        'ComboXian = Forms("图表展示").Controls("ComboXian")
        'output.show(ComboXian.Text)
        'groupLBString = ComboXian.Text

        If String.IsNullOrEmpty(groupLBString) Then
            MsgBox("请先选择分组类别")
            Return
        End If
        If String.IsNullOrEmpty(datayearsString) Then
            MsgBox("请先选择年份列表")
            Return
        End If
        groupLBString = "021, 022"
        datayearsString = "2017,2018,2019"
        If Not String.IsNullOrEmpty(groupLBString) Then
            groupLBStringList = groupLBString.Split(","c)
        End If
        datayearStringList = datayearsString.Split(","c)


        Dim valueDeList As Double(,) = New Double(groupLBStringList.Length, datayearStringList.Length) {} '二维数组 记录x,y维度上的统计值
        Dim groupLBDic As Dictionary(Of String, Short) = New Dictionary(Of String, Short)

        Dim i = 0
        ' 字典用于保存行政区名称,和对应的位置 后续用于通过名称找到二维数组的下标位置
        For Each groupLB As String In groupLBStringList
            groupLBDic.Add(groupLB, i)
            i = i + 1
        Next
        i = 0
        ' 字典用于保存类别名称,和对应的位置 后续用于通过名称找到二维数组的下标位置
        Dim dataYearDic As Dictionary(Of String, Short) = New Dictionary(Of String, Short)
        For Each datayear As String In datayearStringList
            dataYearDic.Add(datayear, i)
            i = i + 1
        Next

        For rowIndex As Integer = 1 To t.Rows.Count
            Dim groupLB = t.Rows(rowIndex)(groupFieldColName)

            If groupLBDic.ContainsKey(groupLB) Then
                ' 先固定获取 lbStringList(0) 列的值到  二维数组的对用行 列下标下  
                For Each datayear As String In datayearStringList
                    valueDeList(groupLBDic(groupLB), dataYearDic(datayear)) = valueDeList(groupLBDic(groupLB), dataYearDic(datayear)) + t.Rows(rowIndex)(datayear)
                Next
            End If
        Next
        Dim chartType = Forms("图表展示").Controls("ComboBox1").Text '"饼状图"
        Dim Chart As WinForm.Chart '定义一个图表变量
        Dim Series As WinForm.ChartSeries '定义一个图系变量
        Chart = Forms("图表展示").Controls("Chart1") ' 引用窗口中的图表
        Chart.SeriesList.Clear() '清除图表原来的图系
        Chart.LegendVisible = True '显示图例

        Dim sum As Double = 0
        '遍历 二维 （即列向）
        For colIndex As Integer = 0 To valueDeList.GetLength(1) - 1
            sum = sum + valueDeList(0, colIndex)
        Next

        Chart.AxisX.ClearValueLabel()
        If chartType = "饼状图" Then
            Chart.VisualEffect = True '加上这一行,让你的图表更漂亮
            Chart.ChartType = ChartTypeEnum.Pie '图表1类型改为Bar(条形)
            For Each datayear As String In datayearStringList
                Series = Chart.SeriesList.Add() '增加一个图系
                Series.Length = 1 '一个系列只能包括一个值
                Series.Text = datayear & "(" & valueDeList(0, dataYearDic(datayear)) & ")" '设置图系的标题
                Series.DataLabelText = Math.Round(valueDeList(0, dataYearDic(datayear)) * 100 / sum, 2) & "%" '计算百分比
                Series.Y(0) = valueDeList(0, dataYearDic(datayear)) '指定值

            Next
            Chart.LegendCompass = CompassEnum.East '图列显示在东方(右方)

        Else

            Chart.ChartType = ChartTypeEnum.Bar

            For groupLB As Integer = 0 To groupLBStringList.Count - 1
                Chart.AxisX.SetValueLabel(groupLB, groupLBStringList(groupLB)) 'x轴指定字符表示
            Next
            Chart.AxisY.Text = RibbonTabs("数据统计")("功能组2")("统计单位").Text 'x轴指定字符表示

            For Each datayear As String In datayearStringList
                Series = Chart.SeriesList.Add() '增加一个图系
                Series.Length = groupLBStringList.Length
                Series.TooltipText = "X = {#XVAL}, Y = {#YVAL}"
                Series.Text = datayear
                For groupLBIndex As Integer = 0 To groupLBStringList.Count - 1
                    Series.X(groupLBIndex) = groupLBIndex
                    Series.Y(groupLBIndex) = valueDeList(groupLBDic(groupLBStringList(groupLBIndex)), dataYearDic(datayear))
                Next
            Next
            Chart.AxisX.AnnoWithLabels = True
        End If
    End Sub
    ''' <summary>
    ''' 根据图层表 和 行政区编码信息所在字段英文名获取 行政区信息的分组方式 比如 substr(zldwdm, 1, 6)
    ''' </summary>
    ''' <param name="layerSchemaName">图层表名称，不同图层表行政区编码信息所在字段会不同</param>
    ''' <param name="xzqbmFieldName">行政区编码信息所在字段英文名</param>
    ''' <returns></returns>
    Private Function getXzqbmGroupExpression(ByVal layerSchemaName As String, ByVal xzqbmFieldName As String) As String
        If layerSchemaName.ToLower = "dltb" Then
            Return "substr(zldwdm, 1, 6)"
        End If
        Return xzqbmFieldName
    End Function

    ''' <summary>
    ''' 根据图层表 和 分组所在字段英文名 获取 行政区信息的分组方式 比如 substr(zldwdm, 1, 6)
    ''' </summary>
    ''' <param name="layerSchemaName">图层表名称，不同图层表行政区编码信息所在字段会不同</param>
    ''' <param name="groupField">分组所在字段英文名</param>
    ''' <returns></returns>
    Private Function getGroupFieldExpression(ByVal layerSchemaName As String, ByVal groupField As String) As String
        If layerSchemaName.ToLower() = "dltb" And groupField.ToLower() = "dlbm" Then
            Return "substr(dlbm, 1, 2)"
        End If
        Return groupField
    End Function

End Module
