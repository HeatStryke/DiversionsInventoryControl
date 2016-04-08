Imports Semantics3
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Xml
Imports System.Net.Sockets
Imports System.IO
Public Class Form1

    Private Sub txtUPC_LostFocus(sender As Object, e As EventArgs) Handles txtUPC.LostFocus
        If txtUPC.Text <> "" Then
            Dim products As Products = New Products("SEM3E2D4AE1C0962499936B01DA6F4FF64FE", "ZDNiZTE5ZjNjZTVjMWI3NjVjMWQwNDAzYTc2OTY3MzQ")
            ' Build the query
            products.products_field("upc", txtUPC.Text)
            Dim constructedJson As String = products.get_query_json("products")
            Console.Write(constructedJson)

            Dim apiResponse As JObject = products.get_products()
            If CInt(apiResponse("results_count")) > 0 Then
                Dim arrayProducts As JArray = apiResponse("results")
                txtName.Text = arrayProducts(0)("name")
                txtMSRP.Text = arrayProducts(0)("price")
                TxtSale.Text = txtMSRP.Text
            End If

            'Dim webclient As New System.Net.WebClient
            'Dim searchstring As String = "http://www.boardgamegeek.com/xmlapi/search?search=" & txtName.Text & "&exact=1"

            'Dim document As Xml.XmlDocument
            'document.Load(searchstring)
            ''document.NodeType
        End If
    End Sub



    Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click
        Dim UPC As Int64 = 0
        Dim count As Integer = 0
        UPC = Int64.Parse(txtUPC.Text)
        
        If txtcount.Text = "" Then
            count = 0
        Else
            count = CInt(txtcount.Text)

        End If
        Dim msrp As Decimal = 0
        If txtMSRP.Text <> "" Then
            msrp = CDec(txtMSRP.Text)
        End If

        Dim sale As Decimal = 0
        If TxtSale.Text <> "" Then
            sale = CDec(TxtSale.Text)
        End If


        DataTable1TableAdapter.InsertQuery(txtUPC.Text, txtName.Text, count, msrp, txtLocation.Text, CStr(chkdemo.Checked), sale)
        DataDataSet.DataTable1.AcceptChanges()
        DataTable1TableAdapter.Update(DataDataSet)
        DataTable1TableAdapter.Fill(DataDataSet.DataTable1)
        txtUPC.Text = ""
        txtName.Text = ""
        txtcount.Text = ""
        txtMSRP.Text = ""
        txtLocation.Text = ""
        chkdemo.Checked = False
        TxtSale.Text = ""
        txtUPC.Focus()

    End Sub

    Private Sub btnexport_Click(sender As Object, e As EventArgs) Handles btnexport.Click

        DataDataSet.DataTable1.Select("SELECT * FROM DataTable 1")
        Dim exportto As StreamWriter
        Dim path As String = System.IO.Path.GetDirectoryName( _
      System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)
        Dim exportstring As String = ""
        path = path & "/export.csv"
        exportto = New StreamWriter(path)
        exportto.WriteLine("Name,UPC,Number In Stock,MSRP,Location in Store,Demo?,Sale Price")
        For x = 0 To DataDataSet.DataTable1.Count
            With DataDataSet.DataTable1.Rows(x)
                exportstring = .Item("Name") & "," & .Item("UPC") & "," & .Item("Count") & "," & .Item("MSRP") & "," & .Item("Location") & "," & CStr(.Item("Demo")) & "," & .Item("Sale Price")
            End With
            exportto.WriteLine(exportstring)
        Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DataDataSet.DataTable1' table. You can move, or remove it, as needed.
        Me.DataTable1TableAdapter.Fill(Me.DataDataSet.DataTable1)
        'TODO: This line of code loads data into the 'DataDataSet.DataTable1' table. You can move, or remove it, as needed.
        Me.DataTable1TableAdapter.Fill(Me.DataDataSet.DataTable1)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub
End Class
