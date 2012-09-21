
Imports System
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class Encryption
    Public Enum SymmProvEnum
        DES
        RC2
        Rijndael
    End Enum

    Private mobjCryptoService As SymmetricAlgorithm

    Public Sub New(ByVal NetSelected As SymmProvEnum)
        Select Case NetSelected
            Case SymmProvEnum.DES
                mobjCryptoService = New DESCryptoServiceProvider()
                Exit Select
            Case SymmProvEnum.RC2
                mobjCryptoService = New RC2CryptoServiceProvider()
                Exit Select
            Case SymmProvEnum.Rijndael
                mobjCryptoService = New RijndaelManaged()
                Exit Select
        End Select
    End Sub

    ''' <remarks>
    ''' 使用自定义SymmetricAlgorithm类的构造器.
    ''' </remarks>
    Public Sub New(ByVal ServiceProvider As SymmetricAlgorithm)
        mobjCryptoService = ServiceProvider
    End Sub

    ''' <remarks>
    ''' Depending on the legal key size limitations of 
    ''' a specific CryptoService provider and length of 
    ''' the private key provided, padding the secret key 
    ''' with space character to meet the legal size of the algorithm.
    ''' </remarks>
    Private Function GetLegalKey(ByVal Key As String) As Byte()
        Dim sTemp As String
        If mobjCryptoService.LegalKeySizes.Length > 0 Then
            Dim lessSize As Integer = 0, moreSize As Integer = mobjCryptoService.LegalKeySizes(0).MinSize
            ' key sizes are in bits
            While Key.Length * 8 > moreSize
                lessSize = moreSize
                moreSize += mobjCryptoService.LegalKeySizes(0).SkipSize
            End While
            sTemp = Key.PadRight(moreSize \ 8, " "c)
        Else
            sTemp = Key
        End If

        ' convert the secret key to byte array
        Return ASCIIEncoding.ASCII.GetBytes(sTemp)
    End Function

    Public Function Encrypting(ByVal Source As String, ByVal Key As String) As String
        Dim bytIn As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(Source)
        ' create a MemoryStream so that the process can be done without I/O files
        Dim ms As New System.IO.MemoryStream()

        Dim bytKey As Byte() = GetLegalKey(Key)

        ' set the private key
        mobjCryptoService.Key = bytKey
        mobjCryptoService.IV = bytKey

        ' create an Encryptor from the Provider Service instance
        Dim encrypto As ICryptoTransform = mobjCryptoService.CreateEncryptor()

        ' create Crypto Stream that transforms a stream using the encryption
        Dim cs As New CryptoStream(ms, encrypto, CryptoStreamMode.Write)

        ' write out encrypted content into MemoryStream
        cs.Write(bytIn, 0, bytIn.Length)
        cs.FlushFinalBlock()

        ' get the output and trim the '\0' bytes
        Dim bytOut As Byte() = ms.GetBuffer()
        Dim i As Integer = 0
        For i = 0 To bytOut.Length - 1
            If bytOut(i) = 0 Then
                Exit For
            End If
        Next

        ' convert into Base64 so that the result can be used in xml
        Return System.Convert.ToBase64String(bytOut, 0, i)
    End Function

    Public Function Decrypting(ByVal Source As String, ByVal Key As String) As String
        ' convert from Base64 to binary
        Dim bytIn As Byte() = System.Convert.FromBase64String(Source)
        ' create a MemoryStream with the input
        Dim ms As New System.IO.MemoryStream(bytIn, 0, bytIn.Length)

        Dim bytKey As Byte() = GetLegalKey(Key)

        ' set the private key
        mobjCryptoService.Key = bytKey
        mobjCryptoService.IV = bytKey

        ' create a Decryptor from the Provider Service instance
        Dim encrypto As ICryptoTransform = mobjCryptoService.CreateDecryptor()

        ' create Crypto Stream that transforms a stream using the decryption
        Dim cs As New CryptoStream(ms, encrypto, CryptoStreamMode.Read)

        ' read out the result from the Crypto Stream
        Dim sr As New System.IO.StreamReader(cs)
        Return sr.ReadToEnd()
    End Function
End Class
