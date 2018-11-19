Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Net.Mail
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Web

Public Class Commonfunction
    Function DateFormat(ByVal strDate As String, ByVal strFormat As String) As String
        '--convert string to date
        Dim dtDate As Date = CDate(strDate)

        Return dtDate.ToString(strFormat)
    End Function
    Public Function DateDisplay(ByVal dtSelected As Date) As String
        Return dtSelected.ToString("dddd dd-MM-yyyy")

    End Function

    Function ExecuteSqlTransaction() As String
        Dim strRet As String = "0"
        Dim strconn As String = ConfigurationManager.AppSettings("ConnectionString")

        Using connection As New SqlConnection(strconn)
            connection.Open()

            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction

            ' Start a local transaction
            transaction = connection.BeginTransaction("TxnStart")

            ' Must assign both transaction object and connection 
            ' to Command object for a pending local transaction.
            command.Connection = connection
            command.Transaction = transaction
            command.CommandTimeout = 300    '5minit. timeout in second

            Try
                command.CommandText = "Insert into Region (RegionID, RegionDescription) VALUES (100, 'Description')"
                command.ExecuteNonQuery()

                command.CommandText = "Insert into Region (RegionID, RegionDescription) VALUES (101, 'Description')"
                command.ExecuteNonQuery()

                ' Attempt to commit the transaction.
                transaction.Commit()
                '--Console.WriteLine("Both records are written to database.")

            Catch ex As Exception
                'Console.WriteLine("Commit Exception Type: {0}", ex.GetType())
                'Console.WriteLine("  Message: {0}", ex.Message)
                strRet = "Error Message:" & ex.Message

                ' Attempt to roll back the transaction. 
                Try
                    transaction.Rollback()

                Catch ex2 As Exception
                    ' This catch block will handle any errors that may have occurred 
                    ' on the server that would cause the rollback to fail, such as 
                    ' a closed connection.
                    'Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType())
                    'Console.WriteLine("  Message: {0}", ex2.Message)

                    strRet = "Rollback Message:" & ex2.Message

                End Try
            End Try
        End Using

        '--0 means success
        Return strRet

    End Function

    '--FIX UKM2 Mod and Index
    Public Sub UKM2DONE_Mod(ByVal strStudentID As String, ByVal strExamYear As String)
        Dim strMod1, strMod2, strMod3, strMod4, strMod5, strMod6, strMod7, strMod8, strMod9, strMod10, strMod11, strMod12, strMod13, strMod14, strMod15 As String
        Dim strSQL As String = ""
        Dim strRet As String = ""

        ''-mod1
        strSQL = "SELECT SUM(isnull(Q001,0)+isnull(Q002,0)+isnull(Q003,0)+isnull(Q004,0)+isnull(Q005,0)+isnull(Q006,0)+isnull(Q007,0)+isnull(Q008,0)+isnull(Q009,0)+isnull(Q010,0)+isnull(Q011,0)+isnull(Q012,0)+isnull(Q013,0)+isnull(Q014,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod1 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q015,0)+isnull(Q016,0)+isnull(Q017,0)+isnull(Q018,0)+isnull(Q019,0)+isnull(Q020,0)+isnull(Q021,0)+isnull(Q022,0)+isnull(Q023,0)+isnull(Q024,0)+isnull(Q025,0)+isnull(Q026,0)+isnull(Q027,0)+isnull(Q028,0)+isnull(Q029,0)+isnull(Q030,0)+isnull(Q031,0)+isnull(Q032,0)+isnull(Q033,0)+isnull(Q034,0)+isnull(Q035,0)+isnull(Q036,0)+isnull(Q037,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod2 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q038,0)+isnull(Q039,0)+isnull(Q040,0)+isnull(Q041,0)+isnull(Q042,0)+isnull(Q043,0)+isnull(Q044,0)+isnull(Q045,0)+isnull(Q046,0)+isnull(Q047,0)+isnull(Q048,0)+isnull(Q049,0)+isnull(Q050,0)+isnull(Q051,0)+isnull(Q052,0)+isnull(Q053,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod3 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q054,0)+isnull(Q055,0)+isnull(Q056,0)+isnull(Q057,0)+isnull(Q058,0)+isnull(Q059,0)+isnull(Q060,0)+isnull(Q061,0)+isnull(Q062,0)+isnull(Q063,0)+isnull(Q064,0)+isnull(Q065,0)+isnull(Q066,0)+isnull(Q067,0)+isnull(Q068,0)+isnull(Q069,0)+isnull(Q070,0)+isnull(Q071,0)+isnull(Q072,0)+isnull(Q073,0)+isnull(Q074,0)+isnull(Q075,0)+isnull(Q076,0)+isnull(Q077,0)+isnull(Q078,0)+isnull(Q079,0)+isnull(Q080,0)+isnull(Q081,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod4 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q082,0)+isnull(Q083,0)+isnull(Q084,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod5 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q085,0)+isnull(Q086,0)+isnull(Q087,0)+isnull(Q088,0)+isnull(Q089,0)+isnull(Q090,0)+isnull(Q091,0)+isnull(Q092,0)+isnull(Q093,0)+isnull(Q094,0)+isnull(Q095,0)+isnull(Q096,0)+isnull(Q097,0)+isnull(Q098,0)+isnull(Q099,0)+isnull(Q100,0)+isnull(Q101,0)+isnull(Q102,0)+isnull(Q103,0)+isnull(Q104,0)+isnull(Q105,0)+isnull(Q106,0)+isnull(Q107,0)+isnull(Q108,0)+isnull(Q109,0)+isnull(Q110,0)+isnull(Q111,0)+isnull(Q112,0)+isnull(Q113,0)+isnull(Q114,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod6 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q115,0)+isnull(Q116,0)+isnull(Q117,0)+isnull(Q118,0)+isnull(Q119,0)+isnull(Q120,0)+isnull(Q121,0)+isnull(Q122,0)+isnull(Q123,0)+isnull(Q124,0)+isnull(Q125,0)+isnull(Q126,0)+isnull(Q127,0)+isnull(Q128,0)+isnull(Q129,0)+isnull(Q130,0)+isnull(Q131,0)+isnull(Q132,0)+isnull(Q133,0)+isnull(Q134,0)+isnull(Q135,0)+isnull(Q136,0)+isnull(Q137,0)+isnull(Q138,0)+isnull(Q139,0)+isnull(Q140,0)+isnull(Q141,0)+isnull(Q142,0)+isnull(Q143,0)+isnull(Q144,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod7 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q145,0)+isnull(Q146,0)+isnull(Q147,0)+isnull(Q148,0)+isnull(Q149,0)+isnull(Q150,0)+isnull(Q151,0)+isnull(Q152,0)+isnull(Q153,0)+isnull(Q154,0)+isnull(Q155,0)+isnull(Q156,0)+isnull(Q157,0)+isnull(Q158,0)+isnull(Q159,0)+isnull(Q160,0)+isnull(Q161,0)+isnull(Q162,0)+isnull(Q163,0)+isnull(Q164,0)+isnull(Q165,0)+isnull(Q166,0)+isnull(Q167,0)+isnull(Q168,0)+isnull(Q169,0)+isnull(Q170,0)+isnull(Q171,0)+isnull(Q172,0)+isnull(Q173,0)+isnull(Q174,0)+isnull(Q175,0)+isnull(Q176,0)+isnull(Q177,0)+isnull(Q178,0)+isnull(Q179,0)+isnull(Q180,0)+isnull(Q181,0)+isnull(Q182,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod8 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q183,0)+isnull(Q184,0)+isnull(Q185,0)+isnull(Q186,0)+isnull(Q187,0)+isnull(Q188,0)+isnull(Q189,0)+isnull(Q190,0)+isnull(Q191,0)+isnull(Q192,0)+isnull(Q193,0)+isnull(Q194,0)+isnull(Q195,0)+isnull(Q196,0)+isnull(Q197,0)+isnull(Q198,0)+isnull(Q199,0)+isnull(Q200,0)+isnull(Q201,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod9 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q202,0)+isnull(Q203,0)+isnull(Q204,0)+isnull(Q205,0)+isnull(Q206,0)+isnull(Q207,0)+isnull(Q208,0)+isnull(Q209,0)+isnull(Q210,0)+isnull(Q211,0)+isnull(Q212,0)+isnull(Q213,0)+isnull(Q214,0)+isnull(Q215,0)+isnull(Q216,0)+isnull(Q217,0)+isnull(Q218,0)+isnull(Q219,0)+isnull(Q220,0)+isnull(Q221,0)+isnull(Q222,0)+isnull(Q223,0)+isnull(Q224,0)+isnull(Q225,0)+isnull(Q226,0)+isnull(Q227,0)+isnull(Q228,0)+isnull(Q229,0)+isnull(Q230,0)+isnull(Q231,0)+isnull(Q232,0)+isnull(Q233,0)+isnull(Q234,0)+isnull(Q235,0)+isnull(Q236,0)+isnull(Q237,0)+isnull(Q238,0)+isnull(Q239,0)+isnull(Q240,0)+isnull(Q241,0)+isnull(Q242,0)+isnull(Q243,0)+isnull(Q244,0)+isnull(Q245,0)+isnull(Q246,0)+isnull(Q247,0)+isnull(Q248,0)+isnull(Q249,0)+isnull(Q250,0)+isnull(Q251,0)+isnull(Q252,0)+isnull(Q253,0)+isnull(Q254,0)+isnull(Q255,0)+isnull(Q256,0)+isnull(Q257,0)+isnull(Q258,0)+isnull(Q259,0)+isnull(Q260,0)+isnull(Q261,0)+isnull(Q262,0)+isnull(Q263,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod10 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q264,0)+isnull(Q265,0)+isnull(Q266,0)+isnull(Q267,0)+isnull(Q268,0)+isnull(Q269,0)+isnull(Q270,0)+isnull(Q271,0)+isnull(Q272,0)+isnull(Q273,0)+isnull(Q274,0)+isnull(Q275,0)+isnull(Q276,0)+isnull(Q277,0)+isnull(Q278,0)+isnull(Q279,0)+isnull(Q280,0)+isnull(Q281,0)+isnull(Q282,0)+isnull(Q283,0)+isnull(Q284,0)+isnull(Q285,0)+isnull(Q286,0)+isnull(Q287,0)+isnull(Q288,0)+isnull(Q289,0)+isnull(Q290,0)+isnull(Q291,0)+isnull(Q292,0)+isnull(Q293,0)+isnull(Q294,0)+isnull(Q295,0)+isnull(Q296,0)+isnull(Q297,0)+isnull(Q298,0)+isnull(Q299,0)+isnull(Q300,0)+isnull(Q301,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod11 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q302,0)+isnull(Q303,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod12 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q304,0)+isnull(Q305,0)+isnull(Q306,0)+isnull(Q307,0)+isnull(Q308,0)+isnull(Q309,0)+isnull(Q310,0)+isnull(Q311,0)+isnull(Q312,0)+isnull(Q313,0)+isnull(Q314,0)+isnull(Q315,0)+isnull(Q316,0)+isnull(Q317,0)+isnull(Q318,0)+isnull(Q319,0)+isnull(Q320,0)+isnull(Q321,0)+isnull(Q322,0)+isnull(Q323,0)+isnull(Q324,0)+isnull(Q325,0)+isnull(Q326,0)+isnull(Q327,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod13 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q328,0)+isnull(Q329,0)+isnull(Q330,0)+isnull(Q331,0)+isnull(Q332,0)+isnull(Q333,0)+isnull(Q334,0)+isnull(Q335,0)+isnull(Q336,0)+isnull(Q337,0)+isnull(Q338,0)+isnull(Q339,0)+isnull(Q340,0)+isnull(Q341,0)+isnull(Q342,0)+isnull(Q343,0)+isnull(Q344,0)+isnull(Q345,0)+isnull(Q346,0)+isnull(Q347,0)+isnull(Q348,0)+isnull(Q349,0)+isnull(Q350,0)+isnull(Q351,0)+isnull(Q352,0)+isnull(Q353,0)+isnull(Q354,0)+isnull(Q355,0)+isnull(Q356,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod14 = getFieldValue(strSQL)

        strSQL = "SELECT SUM(isnull(Q357,0)+isnull(Q358,0)+isnull(Q359,0)+isnull(Q360,0)+isnull(Q361,0)+isnull(Q362,0)+isnull(Q363,0)+isnull(Q364,0)+isnull(Q365,0)+isnull(Q366,0)+isnull(Q367,0)+isnull(Q368,0)+isnull(Q369,0)+isnull(Q370,0)+isnull(Q371,0)+isnull(Q372,0)+isnull(Q373,0)+isnull(Q374,0)+isnull(Q375,0)+isnull(Q376,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strMod15 = getFieldValue(strSQL)

        strSQL = "UPDATE UKM2 WITH (UPDLOCK) SET Mod01=" & strMod1 & ",Mod02=" & strMod2 & ",Mod03=" & strMod3 & ",Mod04=" & strMod4 & ",Mod05=" & strMod5 & ",Mod06=" & strMod6 & ",Mod07=" & strMod7 & ",Mod08=" & strMod8 & ",Mod09=" & strMod9 & ",Mod10=" & strMod10 & ",Mod11=" & strMod11 & ",Mod12=" & strMod12 & ",Mod13=" & strMod13 & ",Mod14=" & strMod14 & ",Mod15=" & strMod15 & " WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strRet = ExecuteSQL(strSQL)

    End Sub

    Public Sub UKM2DONE_Index(ByVal strStudentID As String, ByVal strExamYear As String)
        Dim strVCI, strPRI, strWMI, strPSI As String
        Dim strTotalScore As String
        Dim strSQL As String = ""
        Dim strRet As String = ""

        'VCI (Verbal Completion Index)	2+6+9+13+15  (verbal)
        strSQL = "SELECT SUM(isnull(Mod02,0)+isnull(Mod06,0)+isnull(Mod09,0)+isnull(Mod13,0)+isnull(Mod15,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strVCI = getFieldValue(strSQL)

        'PRI (Perseptual Reasoning Index)	1+4+8+12 (science+math)
        strSQL = "SELECT SUM(isnull(Mod01,0)+isnull(Mod04,0)+isnull(Mod08,0)+isnull(Mod12,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strPRI = getFieldValue(strSQL)

        'WMI(Working Memory Index)	3+7+14 (sokongan VCI/PRI)
        strSQL = "SELECT SUM(isnull(Mod03,0)+isnull(Mod07,0)+isnull(Mod14,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strWMI = getFieldValue(strSQL)

        'PSI(Processing Speed Index)	5+10+11 (sokongan VCI/PRI)
        strSQL = "SELECT SUM(isnull(Mod05,0)+isnull(Mod10,0)+isnull(Mod11,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strPSI = getFieldValue(strSQL)

        ''OK
        strSQL = "UPDATE UKM2 set VCI=" & strVCI & ",PRI=" & strPRI & ",WMI=" & strWMI & ",PSI=" & strPSI & " WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strRet = ExecuteSQL(strSQL)

        'get strTotalScore. NOK
        strSQL = "SELECT SUM(isnull(VCI,0)+isnull(PRI,0)+isnull(WMI,0)+isnull(PSI,0)) as SumA FROM UKM2 WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strTotalScore = getFieldValue(strSQL)

        Dim dblTotalPercentage As Double
        dblTotalPercentage = (CInt(strTotalScore) / 692) * 100
        Dim strTotalPercentage As String = DoConvertD(dblTotalPercentage, 4)

        'update TotalPercentage
        strSQL = "UPDATE UKM2 WITH (UPDLOCK) SET FullMark=692,TotalPercentage=" & strTotalPercentage & ",TotalScore=" & strTotalScore & " WHERE StudentID='" & strStudentID & "' AND ExamYear='" & strExamYear & "'"
        strRet = ExecuteSQL(strSQL)

    End Sub


    ''sqlinjection
    Function CheckSqlInjection(ByVal userValue As String) As Boolean
        ' Throw an exception if a blacklisted word is detected.
        Dim blackList As [String]() = {"alter", "begin", "cast", "create", "cursor", "declare", _
         "delete", "drop", "exec", "execute", "fetch", "insert", _
         "kill", "open", "select", "sys", "sysobjects", "syscolumns", _
         "table", "update", "<script", "</script", "--", "/*", _
         "*/", "@@", "@"}
        For i As Integer = 0 To blackList.Length - 1
            If userValue.ToLower().IndexOf(blackList(i)) <> -1 Then
                Return True
            End If
        Next

        Return False
    End Function

    '--security
    Public Sub LoginTrail(ByVal strLoginID As String, ByVal strLogTime As String, ByVal strUserHostAddress As String, ByVal strUserHostName As String, ByVal strUserBrowser As String, ByVal strActivity As String, ByVal strAuditDetail As String)
        Dim strSQL As String
        Dim strRet As String

        Try
            strSQL = "INSERT INTO security_login_trail (LoginID,LogTime,UserHostAddress,UserHostName,UserBrowser,Activity,AuditDetail) VALUES ('" & strLoginID & "','" & strLogTime & "','" & strUserHostAddress & "','" & strUserHostName & "','" & strUserBrowser & "','" & strActivity & "','" & strAuditDetail & "')"
            strRet = ExecuteSQL(strSQL)
            If Not strRet = "0" Then
                'lblMsg.Text = strRet
            End If
        Catch ex As Exception

        End Try

    End Sub

    Public Function TransactionLog(ByVal SQLAction As String, ByVal strSQLStatement As String, ByVal strIPAddress As String) As String
        Dim strSQL As String
        Dim strRet As String

        Try
            strSQL = "INSERT INTO TransactionLog (SQLAction,SQLStatement,IPAddress,DateCreated) VALUES ('" & SQLAction & "','" & strSQLStatement & "','" & strIPAddress & "','" & Now.ToString & "')"
            strRet = ExecuteSQL(strSQL)
            Return strRet
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function


    '--
    Sub WriteLogFile(ByVal strPath As String, ByVal strError As String)
        Dim File As System.IO.StreamWriter
        Dim strReturn As String = ""
        Dim rowscreated As Integer = 0
        Dim sqlinsert As String = ""

        Try
            '--open append
            File = New System.IO.StreamWriter(strPath, True)

            File.WriteLine(strError)

            File.Close()
            File = Nothing
        Catch ae As SqlException

        Finally

        End Try

    End Sub


    '---
    Function DoConvertC(ByVal Str As String, ByVal DecPlc As Integer) As String
        Return String.Format("{0:c" & DecPlc & "}", CDec(Str))

    End Function

    '--decimal places for number
    Function DoConvertN(ByVal Str As String, ByVal DecPlc As Integer) As String
        Return String.Format("{0:n" & DecPlc & "}", CDec(Str))

    End Function

    Function DoConvertD(ByVal Str As String, ByVal DecPlc As Double) As String
        Return String.Format("{0:n" & DecPlc & "}", CDbl(Str))

    End Function

    ''padleft
    Function DoPadZeroLeft(ByVal strValue As String, ByVal nCount As Integer) As String
        Return strValue.PadLeft(nCount, "0")

    End Function

    Function RemoveComa(ByVal strString As String)
        RemoveComa = strString.Replace(",", ".")

    End Function

    Function IsTextValidated(ByVal strTextEntry As String) As Boolean
        Dim objNotWholePattern As New Regex("[^0-9]")
        Return (Not objNotWholePattern.IsMatch(strTextEntry))

    End Function

    Function isNumeric(ByVal strTextEntry As String) As Boolean
        Dim objNotWholePattern As New Regex("[^0-9]")
        Return (Not objNotWholePattern.IsMatch(strTextEntry))

    End Function

    Function IsCurrency(ByVal value As String) As Boolean
        Dim dummy As Decimal
        Return ([Decimal].TryParse(value, NumberStyles.Currency, CultureInfo.CurrentCulture, dummy))

    End Function

    Function gettxnref()
        Return Now.ToString("yyyyMMdd") & Now.Minute & Now.Second & Now.Millisecond

    End Function

    Function ValidatePhone(ByVal num As String) As Boolean
        'create our regular exp<b></b>ression pattern
        Dim pattern As String = "^[0][18]\d{1}[\-][1-9]\d{2}\d{4}$"
        'create our regular exp<b></b>ression object
        Dim check As New Regex(pattern)
        Dim valid As Boolean = False
        'Make sure a phone number was provided
        If Not String.IsNullOrEmpty(num) Then
            valid = check.IsMatch(num)
        Else
            valid = False
        End If
        Return valid
    End Function
    Function isEmail(ByVal inputEmail As String) As Boolean

        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Match = Regex.Match(inputEmail, pattern)
        If emailAddressMatch.Success Then
            isEmail = True
        Else
            isEmail = False
        End If

    End Function

    Function isMyKad(ByVal inputMyKad As String) As Boolean

        Dim pattern As String = "^\d\d(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])-[0-9]{2}-[0-9]{4}$"
        Dim MyKadNumberMatch As Match = Regex.Match(inputMyKad, pattern)
        If MyKadNumberMatch.Success Then
            isMyKad = True
        Else
            isMyKad = False
        End If

    End Function

    Function isMyKad2(ByVal inputMyKad As String) As Boolean

        Dim pattern As String = "^\d\d(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[0-9]{2}[0-9]{4}$"
        Dim MyKadNumberMatch As Match = Regex.Match(inputMyKad, pattern)
        If MyKadNumberMatch.Success Then
            isMyKad2 = True
        Else
            isMyKad2 = False
        End If

    End Function
    Function FormatDateDMY(ByVal strDate As Date) As String
        Dim ddate As Date
        ddate = strDate
        Dim dd, mm, yy As String
        dd = Day(ddate).ToString
        If dd.Length = 1 Then
            dd = "0" & dd
        End If
        mm = Month(ddate).ToString
        If mm.Length = 1 Then
            mm = "0" & mm
        End If
        yy = Year(ddate).ToString

        FormatDateDMY = dd & "/" & mm & "/" & yy
    End Function

    'yyyy'-'MM'-'dd'T'HH': 'mm': 'ss.fffffff'Z' —For UCT values
    'yyyy'-'MM'-'dd'T'HH': 'mm': 'ss.fffffff'zzz' —For local values
    'yyyy'-'MM'-'dd'T'HH': 'mm': 'ss.fffffff' —For abstract time values
    Function getNow() As String
        Return Now.ToString("yyyyMMdd HH:mm:ss.fff")

    End Function

    Function getToday() As String
        Return Now.ToString("yyyyMMdd")

    End Function

    Function getTodayFormated() As String
        Return Now.ToString("yyyy-MM-dd")

    End Function


    Function setTrailZero(ByVal strNumber As String) As String
        Select Case strNumber.Length
            Case 1
                strNumber = "Q00" & strNumber
            Case 2
                strNumber = "Q0" & strNumber
            Case Else
                strNumber = "Q" & strNumber
        End Select

        Return strNumber
    End Function


    Function getRandom() As String
        Dim strTemp As String = Now.Year & Now.Month & Now.Day & Now.Second & Now.Millisecond

        Return strTemp
    End Function

    Function getGUID() As String
        Return System.Guid.NewGuid.ToString()

    End Function

    Function getRandomQuestion() As String
        '--comment this since Dr Siti said many error on the question set 2009
        'Dim aRand As New Random
        'Dim nRand As Integer = aRand.Next(1, 3)

        'Select Case nRand
        '    Case 1
        '        Return "2009"
        '    Case 2
        '        Return "2010"
        '    Case Else
        '        Return "2010"
        'End Select
        Return "2010"

    End Function

    Function FixSingleQuotes(ByVal strValue As String) As String
        '--fix complete sql injection
        Dim intLevel As Integer = 2

        Try
            If Not IsDBNull(strValue) Then
                If intLevel > 0 Then
                    strValue = Replace(strValue, "'", "''") ' Most important one! This line alone can prevent most injection attacks
                    strValue = Replace(strValue, "--", "")
                    strValue = Replace(strValue, "[", "(")
                    strValue = Replace(strValue, "%", "[%]")
                End If

                ''If intLevel > 1 Then
                ''    Dim myArray As Array
                ''    myArray = Split("xp_ ;update ;insert ;select ;drop ;alter ;create ;rename ;delete ;replace ", ";")
                ''    Dim i, i2, intLenghtLeft As Integer
                ''    For i = LBound(myArray) To UBound(myArray)
                ''        Dim rx As New Regex(myArray(i), RegexOptions.Compiled Or RegexOptions.IgnoreCase)
                ''        Dim matches As MatchCollection = rx.Matches(strValue)
                ''        i2 = 0
                ''        For Each match As Match In matches
                ''            Dim groups As GroupCollection = match.Groups
                ''            intLenghtLeft = groups.Item(0).Index + Len(myArray(i)) + i2
                ''            strValue = Left(strValue, intLenghtLeft - 1) & "&nbsp;" & Right(strValue, Len(strValue) - intLenghtLeft)
                ''            i2 += 5
                ''        Next
                ''    Next
                ''End If

                'strValue = replace(strValue, ";", ";&nbsp;")
                'strValue = replace(strValue, "_", "[_]")

                Return strValue
            Else
                Return strValue
            End If
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Function CToString(ByVal strString As String)
        Dim strTemp As String
        strTemp = strString
        CToString = strTemp

    End Function

    Function ReplaceComa(ByVal strString As String)
        Dim intIndex
        Dim strTemp As String = ""
        intIndex = InStr(strString, ",")
        If intIndex > 0 Then
            strTemp = strString
            strTemp.Replace(",", ".")
        End If
        ReplaceComa = strTemp

    End Function

    Function FixComa(ByVal strString As String)
        Dim intIndex
        Dim strTemp As String
        intIndex = InStr(strString, ",")
        If intIndex > 0 Then
            strTemp = """" & strString & """"
        Else
            strTemp = strString
        End If
        FixComa = strTemp

    End Function


    Function ChkTime(ByVal strICnumber As String, ByVal strTestID As String) As Boolean
        '--comment for launching purpose only


        Return True

    End Function

    Function StartTimer(ByVal strSQL As String)
        Dim dtStartDate As Date = Now

        '--update into database starttime
        Return True

    End Function

    Function EndTimer(ByVal strSQL As String)
        Dim dtEndDate As Date = Now

        '--update into database endtime

        Return True
    End Function

    Function ComputeTime()

        Return True
    End Function

    Function LogEventDB(ByVal myEvent As String, ByVal FileID As String, _
    ByVal FileName As String, ByVal FolderName As String, ByVal FolDir As String, ByVal History As String, _
    ByVal UserID As String, ByVal LoginID As String) As String

        Dim strSQL As String
        strSQL = "INSERT INTO mLog (myEvent,FileID,FileName,FolderName,FolDir,History,UserID,LoginID) VALUES ('" & myEvent & "'," & FileID & ",'" & FileName.Replace("'", "") & "','" & FolderName.Replace("'", "") & "','" & FolDir.Replace("'", "") & "','" & History.Replace("'", "") & "'," & UserID & ",'" & LoginID & "')"
        LogEventDB = ExecuteSQL(strSQL)

    End Function




    Function strClean(ByVal strtoclean As String) As String
        '--special '
        strtoclean = strtoclean.Replace("'", "-")

        Dim outputStr As String
        Dim rgPattern = "[(?*"",\\<>&#~%{}+@:\/!;]+$^():~`"
        Dim objRegExp As New Regex(rgPattern)

        outputStr = objRegExp.Replace(strtoclean, "")

        Return outputStr
    End Function

    Function filterFilename(ByVal strFilename As String) As String
        '--Replace invalid file name characters \ /:*?"<>|
        strFilename = strFilename.Replace("'", "")
        strFilename = strFilename.Replace(":", "")
        strFilename = strFilename.Replace("*", "")
        strFilename = strFilename.Replace("?", "")
        strFilename = strFilename.Replace("<", "")
        strFilename = strFilename.Replace(">", "")
        strFilename = strFilename.Replace("|", "")
        strFilename = strFilename.Replace("/", "")
        strFilename = strFilename.Replace("\\", "")
        strFilename = strFilename.Replace("\", "")

        Return strFilename

    End Function

    Sub sendmail(ByVal mailfrom As String, ByVal mailto As String, ByVal mailsubject As String, ByVal mailbody As String)

        'create the mail message
        Dim mail As New MailMessage()
        '--Dim MyAttachment As Attachment = New Attachment(strFileAttach_mykad)

        'set the addresses
        mail.From = New MailAddress(mailfrom)
        mail.To.Add(mailto)

        'set the content
        mail.Subject = mailsubject
        '--mail.Attachments.Add(MyAttachment)
        mail.Body = mailbody
        'mail.IsBodyHtml = True

        'send the message
        Dim smtp As New SmtpClient("mail.onlineapp.com.my", 587)
        smtp.Credentials = New NetworkCredential("mykadpro@onlineapp.com.my", "p@ssw0rd1")
        smtp.Send(mail)

    End Sub

    Sub sendmailHTML(ByVal mailfrom As String, ByVal mailto As String, ByVal mailsubject As String, ByVal mailbody As String)
        Dim message As New MailMessage(mailfrom, mailto)
        Dim SmtpClient As New SmtpClient("mail.onlineapp.com.my", 587)
        message.Subject = mailsubject
        message.Body = mailbody
        message.IsBodyHtml = True
        SmtpClient.Credentials = New NetworkCredential("mykadpro@onlineapp.com.my", "p@ssw0rd1")
        SmtpClient.Send(message)

    End Sub

    '--sum columns 
    Public Function SumColumn(ByVal mySQL As String) As Integer
        Dim strconn As String = ConfigurationManager.AppSettings("connectionString")
        Dim objConn As SqlConnection = New SqlConnection(strconn)
        Dim sqlDA As New SqlDataAdapter(mySQL, objConn)
        Dim ds As DataSet = New DataSet

        Dim nCol As Integer = 0
        Dim strTemp As String = ""
        Dim nTemp As Integer = 0

        Try
            sqlDA.Fill(ds, "AnyTable")
            If ds.Tables(0).Rows.Count > 0 Then
                While nCol < ds.Tables(0).Columns.Count
                    If Not IsDBNull(ds.Tables(0).Rows(0).Item(nCol)) Then
                        strTemp = ds.Tables(0).Rows(0).Item(nCol)
                    Else
                        strTemp = "0"
                    End If

                    nTemp += CInt(strTemp)
                    nCol += 1
                End While
            End If
        Catch ex As Exception
            Return 0
        Finally
            ds.Dispose()
            sqlDA.Dispose()
            objConn.Dispose()
        End Try

        Return nTemp
    End Function

    Public Function ExecuteSQL(ByVal strSQL As String) As String
        ExecuteSQL = "0"

        If strSQL.Length = 0 Then
            ExecuteSQL = "*System error (Contact system admin): No query string pass."
            Exit Function
        End If
        'If isBlockText(strSQL) = True Then
        '    ExecuteSQL = "*Security alert (Contact system admin): IP address and SQL command logged."
        '    Exit Function
        'End If

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim cmdSQL As New SqlCommand(strSQL, objConn)

        Try
            cmdSQL.Connection.Open()
            cmdSQL.ExecuteNonQuery()
            cmdSQL.Connection.Close()

            Return "0"
        Catch ex As SqlException
            ExecuteSQL = "Error." & Err.Description & "." & strSQL
            'do not exposed it to end user. hacker might used the info
        Finally
            If Not (objConn Is Nothing) Then
                objConn.Close()
            End If

            ''--detach the SqlParameters from the command object, so they can be used again
            'cmdSQL.Parameters.Clear()
            'objConn.Dispose()
        End Try

    End Function

    Public Function isBlockText(ByVal strValue As String) As Boolean
        Dim myArray As Array
        myArray = Split("xp_;drop;alter;create;rename;delete;replace", ";")

        Dim myValue As Array
        myValue = Split(strValue, " ")

        Dim i As Integer
        For i = LBound(myArray) To UBound(myArray)
            Dim n As Integer
            For n = LBound(myValue) To UBound(myValue)
                If String.Compare(myArray(i), myValue(n), True) = 0 Then
                    Return True
                End If
            Next
        Next

        Return False
    End Function

    Public Function isExist(ByVal strSQL As String) As Boolean
        If strSQL.Length = 0 Then
            Return False
        End If
        ''If isBlockText(strSQL) = True Then
        ''    Return False
        ''End If

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")
            If ds.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        Finally
            objConn.Dispose()
        End Try

    End Function

    Public Function isAnswered(ByVal strQ As String, ByVal strKey As String) As Boolean
        Dim strSQL As String = "SELECT ICnumber FROM ukm1_respondent_mark WHERE ICnumber='" & strKey & "' AND TestID='2010' AND NOT(" & strQ & " IS NULL)"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")
            If ds.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        Finally
            objConn.Dispose()
        End Try

    End Function

    Public Function getCount(ByVal strSQL As String) As Integer
        If strSQL.Length = 0 Then
            Return 0
        End If

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")
            getCount = ds.Tables(0).Rows.Count
        Catch ex As Exception
            Return 0
        Finally
            objConn.Dispose()
        End Try

    End Function

    Public Function getRowValue(ByVal strSQL As String) As String
        Dim strValue As String = ""

        If strSQL.Length = 0 Then
            Return ""
        End If

        Dim strconn As String = ConfigurationManager.AppSettings("connectionString")
        Dim objConn As SqlConnection = New SqlConnection(strconn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
        Dim strRowValue As String = ""
        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    If Not IsDBNull(ds.Tables(0).Rows(i).Item(0).ToString) Then
                        strRowValue += ds.Tables(0).Rows(i).Item(0).ToString & ","
                    End If
                Next
            End If

            '--remove last coma
            If strRowValue.Length > 0 Then
                strValue = strRowValue.Substring(0, strRowValue.Length - 1)
            End If

            Return strValue

        Catch ex As Exception
            Return "*err:" & ex.Message
        Finally
            objConn.Dispose()
        End Try

    End Function


    Public Function getFieldValue(ByVal strSQL As String) As String
        If strSQL.Length = 0 Then
            Return ""
        End If
        'If isBlockText(strSQL) = True Then
        '    getFieldValue = "*Security alert (Contact system admin): IP address and SQL command logged."
        '    Exit Function
        'End If

        Dim strconn As String = ConfigurationManager.AppSettings("connectionString")
        Dim objConn As SqlConnection = New SqlConnection(strconn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
        Dim strFieldValue As String = ""
        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            If ds.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(ds.Tables(0).Rows(0).Item(0).ToString) Then
                    strFieldValue = ds.Tables(0).Rows(0).Item(0).ToString
                Else
                    Return ""
                End If
            End If

        Catch ex As Exception
            Return "*System error (Contact system admin): " & ex.Message
        Finally
            objConn.Dispose()
        End Try

        Return strFieldValue
    End Function

    Public Function getFieldValueInt(ByVal strSQL As String) As String
        If strSQL.Length = 0 Then
            Return "0"
        End If
        'If isBlockText(strSQL) = True Then
        '    getFieldValue = "*Security alert (Contact system admin): IP address and SQL command logged."
        '    Exit Function
        'End If

        Dim strconn As String = ConfigurationManager.AppSettings("connectionString")
        Dim objConn As SqlConnection = New SqlConnection(strconn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
        Dim strFieldValue As String = ""
        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            If ds.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(ds.Tables(0).Rows(0).Item(0).ToString) Then
                    strFieldValue = ds.Tables(0).Rows(0).Item(0).ToString
                Else
                    Return "0"
                End If
            End If

        Catch ex As Exception
            Return "0"
        Finally
            objConn.Dispose()
        End Try

        Return strFieldValue
    End Function

    Public Function getFieldValueEx(ByVal strSQL As String) As String
        If strSQL.Length = 0 Then
            Return "0"
        End If
        'If isBlockText(strSQL) = True Then
        '    getFieldValueEx = "*Security alert (Contact system admin): IP address and SQL command logged."
        '    Exit Function
        'End If

        Dim strconn As String = ConfigurationManager.AppSettings("connectionString")
        Dim objConn As SqlConnection = New SqlConnection(strconn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
        Dim strFieldValue As String = ""
        Dim i As Integer
        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If Not IsDBNull(ds.Tables(0).Rows(0).Item(0).ToString) Then
                        strFieldValue += ds.Tables(0).Rows(0).Item(i).ToString & "|"
                    Else
                        Return ""
                    End If
                Next
            End If

        Catch ex As Exception
            Return "*System error (Contact system admin): "  '--+ ex.Message
        Finally
            objConn.Dispose()
        End Try

        Return strFieldValue
    End Function

    Public Function ppcs_activity_insert(ByVal strcreatedby As String, ByVal strusertype As String, ByVal stractivitydesc As String) As String
        Dim strSQL As String
        Dim strcreatedate As String = Now.ToString("ddd dd-MM-yyyy HH:mm:ss")
        strSQL = "SELECT Fullname FROM ppcs_users WHERE myGUID='" & strcreatedby & "'"
        Dim strFullname As String = getFieldValue(strSQL)

        strSQL = "INSERT INTO ppcs_activity (createdby,usertype,activitydesc,createdate) VALUES ('" & strFullname & "','" & strusertype & "','" & stractivitydesc & "','" & strcreatedate & "')"
        ppcs_activity_insert = ExecuteSQL(strSQL)

    End Function

    ''--strip special char and space
    Public Function StringStrip(ByVal strStrip As String)
        Dim strorigFileName As String
        Dim intCounter As Integer
        Dim arrSpecialChar() As String = {".", ",", "<", ">", ":", "?", """", "/", "{", "[", "}", "]", "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "=", "|", " ", "\"}
        strorigFileName = strStrip
        intCounter = 0
        Dim i As Integer
        For i = 0 To arrSpecialChar.Length - 1
            Do Until intCounter = 29
                strStrip = Replace(strorigFileName, arrSpecialChar(i), "")
                intCounter = intCounter + 1
                strorigFileName = strStrip
            Loop
            intCounter = 0
        Next
        Return strorigFileName

    End Function

    Public Function Bulk_Transfer(ByVal strSQLSource As String, ByVal strDestinationTableName As String) As String
        Dim strconn As String = ConfigurationManager.AppSettings("connectionString")

        ' Create source connection
        Dim source As New SqlConnection(strconn)
        ' Create destination connection
        Dim destination As New SqlConnection(strconn)

        Try
            ' Select data from source table
            Dim cmd As New SqlCommand(strSQLSource, source)

            ''open connection
            source.Open()
            destination.Open()

            ' Execute reader
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            ' Create SqlBulkCopy
            Dim bulkData As New SqlBulkCopy(destination)
            ' Set destination table name
            bulkData.DestinationTableName = strDestinationTableName
            ' Write data
            bulkData.WriteToServer(reader)
            ' Close objects
            bulkData.Close()
            destination.Close()
            source.Close()

            Return "0"
        Catch ex As Exception

            Return ex.Message
        End Try


    End Function

    Private Function ConvertDataViewToString(ByVal srcDataView As DataView, ByVal strPath As String, Optional ByVal Delimiter As String = Nothing, Optional ByVal Separator As String = ",") As String
        Dim File As System.IO.StreamWriter
        File = New System.IO.StreamWriter(strPath)

        Dim ResultBuilder As StringBuilder
        ResultBuilder = New StringBuilder()
        ResultBuilder.Length = 0

        Dim aCol As DataColumn
        For Each aCol In srcDataView.Table.Columns
            If Not Delimiter Is Nothing AndAlso (Delimiter.Trim.Length > 0) Then
                ResultBuilder.Append(Delimiter)
            End If
            ResultBuilder.Append(aCol.ColumnName)
            If Not Delimiter Is Nothing AndAlso (Delimiter.Trim.Length > 0) Then
                ResultBuilder.Append(Delimiter)
            End If
            ResultBuilder.Append(Separator)
        Next
        If ResultBuilder.Length > Separator.Trim.Length Then
            ResultBuilder.Length = ResultBuilder.Length - Separator.Trim.Length
        End If
        ResultBuilder.Append(Environment.NewLine)

        Dim aRow As DataRowView
        For Each aRow In srcDataView
            For Each aCol In srcDataView.Table.Columns
                If Not Delimiter Is Nothing AndAlso (Delimiter.Trim.Length > 0) Then
                    ResultBuilder.Append(Delimiter)
                End If
                ResultBuilder.Append(aRow(aCol.ColumnName))
                If Not Delimiter Is Nothing AndAlso (Delimiter.Trim.Length > 0) Then
                    ResultBuilder.Append(Delimiter)
                End If
                ResultBuilder.Append(Separator)
            Next aCol
            ResultBuilder.Length = ResultBuilder.Length - 1
            ResultBuilder.Append(vbNewLine)
        Next aRow

        If Not ResultBuilder Is Nothing Then
            '--Return ResultBuilder.ToString()
            File.WriteLine(ResultBuilder.ToString())
            File.Close()
            File = Nothing

            Return "OK"
        Else
            '--Return String.Empty
            Return "NOK"
        End If

    End Function

    Function WriteInExportedFile(ByVal strPath As String, ByVal tableColumns As DataColumnCollection, ByVal tableRows As DataRowCollection) As String
        Dim File As System.IO.StreamWriter

        Dim strReturn As String = ""
        Dim rowscreated As Integer = 0
        Dim sqlinsert As String = ""

        Try
            File = New System.IO.StreamWriter(strPath)

            'Loop through columns of table to generate first row of CSV file
            Dim ctrColumn As Integer = 0
            Dim dc As DataColumn
            For Each dc In tableColumns
                If (ctrColumn < tableColumns.Count - 1) Then
                    sqlinsert += dc.ColumnName.ToString() + ","
                Else
                    sqlinsert += dc.ColumnName.ToString()
                End If

                ctrColumn = ctrColumn + 1
            Next
            File.WriteLine(sqlinsert)

            Dim row As DataRow
            For Each row In tableRows
                sqlinsert = ""
                Dim sqlvalues As String = ""
                Dim rowItems() As Object = row.ItemArray

                ctrColumn = 0
                Dim dcol As DataColumn
                For Each dcol In tableColumns
                    If (ctrColumn < tableColumns.Count - 1) Then
                        sqlvalues += """" + rowItems(ctrColumn).ToString().Replace(" ''", "'") + """" + ","
                    Else
                        sqlvalues += """" + rowItems(ctrColumn).ToString().Replace(" ''", "'") + """"
                    End If

                    ctrColumn = ctrColumn + 1
                Next

                sqlinsert = sqlinsert + sqlvalues
                File.WriteLine(sqlinsert)

                rowscreated = rowscreated + 1
            Next
            strReturn = "Records Exported Successfully!<br>"
            strReturn += rowscreated.ToString()
            strReturn += " rows created in CSV file "

            Dim intFileNameLength = InStr(1, StrReverse(strPath), "\")
            Dim strFilename As String = Mid(strPath, (Len(strPath) - intFileNameLength) + 2)
            strReturn += "<a target=_blank href='../cert_pdf/" + strFilename + "'>" + strFilename + "</a>"
            File.Close()
            File = Nothing
        Catch ae As SqlException
            strReturn = "Error at Record Number: "
            strReturn += rowscreated.ToString()
            strReturn += "<br>Message: " + ae.Message.ToString() + "<br>"
            strReturn += "Error importing. Please try again"
        Finally

        End Try

        Return strReturn
    End Function

    Function ExportDataXLS(ByVal dt As DataTable, ByVal strFilePath As String) As Boolean
        Try
            ' Create the CSV file to which grid data will be exported.
            Dim sw As New StreamWriter(strFilePath, False)
            ' First we will write the headers.
            'DataTable dt = m_dsProducts.Tables[0];
            Dim iColCount As Integer = dt.Columns.Count
            For i As Integer = 0 To iColCount - 1
                sw.Write(dt.Columns(i))
                If i < iColCount - 1 Then
                    sw.Write(",")
                End If
            Next
            sw.Write(sw.NewLine)

            ' Now write all the rows.
            For Each dr As DataRow In dt.Rows
                For i As Integer = 0 To iColCount - 1
                    If Not Convert.IsDBNull(dr(i)) Then
                        sw.Write(dr(i).ToString())
                    End If
                    If i < iColCount - 1 Then
                        sw.Write(",")
                    End If
                Next

                sw.Write(sw.NewLine)
            Next
            sw.Close()
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub drHasRows(ByVal connection As SqlConnection, ByVal strSQL As String)
        Using connection
            Dim command As SqlCommand = New SqlCommand(strSQL, connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                Do While reader.Read()
                    Console.WriteLine(reader.GetInt32(0) _
                      & vbTab & reader.GetString(1))
                Loop
            Else
                Console.WriteLine("No rows found.")
            End If

            reader.Close()
        End Using
    End Sub

    Private Sub drRetrieveMultipleResults(ByVal connection As SqlConnection)
        Using connection
            Dim command As SqlCommand = New SqlCommand( _
                      "SELECT CategoryID, CategoryName FROM Categories;" & _
                      "SELECT EmployeeID, LastName FROM Employees", connection)

            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()

            Do While reader.HasRows
                Console.WriteLine(vbTab & reader.GetName(0) _
                  & vbTab & reader.GetName(1))

                Do While reader.Read()
                    Console.WriteLine(vbTab & reader.GetInt32(0) _
                      & vbTab & reader.GetString(1))
                Loop

                reader.NextResult()
            Loop
        End Using
    End Sub


    Private Sub drGetSchemaInfo(ByVal connection As SqlConnection)
        Using connection
            Dim command As SqlCommand = New SqlCommand( _
              "SELECT CategoryID, CategoryName FROM Categories;", _
              connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader()
            Dim schemaTable As DataTable = reader.GetSchemaTable()

            Dim row As DataRow
            Dim column As DataColumn

            For Each row In schemaTable.Rows
                For Each column In schemaTable.Columns
                    Console.WriteLine(String.Format("{0} = {1}", _
                      column.ColumnName, row(column)))
                Next
                Console.WriteLine()
            Next
            reader.Close()
        End Using
    End Sub

    Private Sub drRead()
        Dim strDisplay As String = ""

        Dim sConnection As String = "server=(local);uid=sa;pwd=PassWord;database=DatabaseName"
        Using Con As New SqlConnection(sConnection)
            Con.Open()
            Using Com As New SqlCommand("Select * From tablename", Con)
                Using RDR = Com.ExecuteReader()
                    If RDR.HasRows Then
                        Do While RDR.Read
                            strDisplay = RDR.Item("Name").ToString()
                        Loop
                    End If
                End Using
            End Using
            Con.Close()
        End Using
    End Sub

End Class

