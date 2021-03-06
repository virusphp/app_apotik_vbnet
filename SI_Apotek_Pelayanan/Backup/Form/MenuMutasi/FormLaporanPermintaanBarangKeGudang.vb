Imports Syncfusion.Windows.Forms
Imports Syncfusion.Windows.Forms.Tools
Imports System.Data.SqlClient
Imports Syncfusion.XlsIO
Imports CrystalDecisions.CrystalReports.Engine

Public Class FormLaporanPermintaanBarangKeGudang
    Inherits Office2010Form
    Public rptPerTanggal, rptPerUnit, rptPerBarang, rptPerUnitTanggal As New ReportDocument
    Dim BDPerTanggal, BDDataBarang, BDPerBarang, BDPerUnit, BDPerUnitTanggal As New BindingSource
    Dim kdBagian, nmBagian, Stok As String

    Sub Kosongkan()
        TglServer()
        DTPTanggalAkhirTab1.Value = TanggalServer
        DTPTanggalAwalTab1.Value = TanggalServer
        DTPTahunTab2.Value = TanggalServer
        DTPBulanTab2.Value = TanggalServer
        DTPTanggalAwalTab3.Value = TanggalServer
        DTPTanggalAkhirTab3.Value = TanggalServer
        DTPTanggalAwalTab4.Value = TanggalServer
        DTPTanggalAkhirTab4.Value = TanggalServer
        cmbBagianTab2.Text = ""
        GridTab1.DataSource = Nothing
        GridTab2.DataSource = Nothing
        gridTab3.DataSource = Nothing
        gridTab4.DataSource = Nothing
        GridTab1.BackgroundColor = Color.Azure
        GridTab2.BackgroundColor = Color.Azure
        gridTab3.BackgroundColor = Color.Azure
        gridTab4.BackgroundColor = Color.Azure
        txtKodeBarangTab3.Clear()
        txtNamaBarangTab3.Clear()
        txtGrandTotalTab1.DecimalValue = 0
        txtGrandTotalTab2.DecimalValue = 0
        txtGrandTotalTab3.DecimalValue = 0
        txtGrandTotalTab4.DecimalValue = 0
        cmbBagianTab4.Text = ""
    End Sub

    Sub tampilPerTanggal()
        Try
            DA = New OleDb.OleDbDataAdapter("select tanggal,nota,RTRIM(LTRIM(nmkasir)) as nmkasir,RTRIM(LTRIM(nmbagian)) as nmbagian,idbarang,kdbarang,RTRIM(LTRIM(nmbarang)) as nmbarang, jml,RTRIM(LTRIM(nmsatuan)) as nmsatuan,harga,jmlharga,tglexp,RTRIM(LTRIM(nobatch)) as nobatch,posting from ap_mintabrg where tanggal >= '" & Format(DTPTanggalAwalTab1.Value, "yyyy/MM/dd") & "' and tanggal <= '" & Format(DTPTanggalAkhirTab1.Value, "yyyy/MM/dd") & "' order by posting Desc,tanggal,nota,nomer", CONN)
            DS = New DataSet
            DA.Fill(DS, "permintaanGudangPerTanggal")
            BDPerTanggal.DataSource = DS
            BDPerTanggal.DataMember = "permintaanGudangPerTanggal"
            With GridTab1
                .DataSource = Nothing
                .DataSource = BDPerTanggal
                .Columns(0).HeaderText = "Tanggal Permintaan"
                .Columns(1).HeaderText = "Nota Permintaan"
                .Columns(2).HeaderText = "Petugas"
                .Columns(3).HeaderText = "Dari Unit"
                .Columns(4).HeaderText = "ID Barang"
                .Columns(5).HeaderText = "Kode Barang"
                .Columns(6).HeaderText = "Nama Barang"
                .Columns(7).HeaderText = "Jumlah Yang Diminta"
                .Columns(7).DefaultCellStyle.Format = "N2"
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(8).HeaderText = "Satuan"
                .Columns(9).HeaderText = "Harga"
                .Columns(9).DefaultCellStyle.Format = "N2"
                .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(10).HeaderText = "Jumlah Harga"
                .Columns(10).DefaultCellStyle.Format = "N2"
                .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(11).HeaderText = "Tanggal Exp"
                .Columns(12).HeaderText = "No Batch"
                .Columns(13).HeaderText = "Posting"
                .Columns(0).Width = 75
                .Columns(1).Width = 85
                .Columns(2).Width = 100
                .Columns(3).Width = 120
                .Columns(4).Width = 80
                .Columns(5).Width = 80
                .Columns(6).Width = 150
                .Columns(7).Width = 50
                .Columns(8).Width = 75
                .Columns(9).Width = 90
                .Columns(10).Width = 120
                .Columns(11).Width = 75
                .Columns(12).Width = 75
                .Columns(13).Width = 50
                .BackgroundColor = Color.Azure
                .DefaultCellStyle.SelectionBackColor = Color.LightBlue
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .RowHeadersDefaultCellStyle.BackColor = Color.Black
                .ReadOnly = True
                For i As Integer = 0 To .RowCount - 1
                    If Trim(.Rows(i).Cells("posting").Value) = "2" Then
                        .Rows(i).Cells("nmbarang").Style.BackColor = Color.Aquamarine
                    End If
                Next
            End With
            MsgBox("Data sudah ditampilkan", vbInformation, "Informasi")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub tampilPerUnit()
        Try
            DA = New OleDb.OleDbDataAdapter("select tanggal, nota, RTRIM(LTRIM(nmkasir)) as nmkasir, RTRIM(LTRIM(nmbagian)) as nmbagian, idbarang, kdbarang, RTRIM(LTRIM(nmbarang)) as nmbarang, jml, RTRIM(LTRIM(nmsatuan)) as nmsatuan, harga, jmlharga, tglexp, RTRIM(LTRIM(nobatch)) as nobatch, posting from ap_mintabrg where YEAR(tanggal) = '" & Year(DTPTahunTab2.Value) & "' and MONTH(tanggal) = '" & Month(DTPBulanTab2.Value) & "' and kdbagian='" & Trim(kdBagian) & "' order by posting Desc,nota,nmbarang", CONN)
            DS = New DataSet
            DA.Fill(DS, "permintaanGudangPerUnit")
            BDPerUnit.DataSource = DS
            BDPerUnit.DataMember = "permintaanGudangPerUnit"
            With GridTab2
                .DataSource = Nothing
                .DataSource = BDPerUnit
                .Columns(0).HeaderText = "Tanggal Permintaan"
                .Columns(1).HeaderText = "Nota Permintaan"
                .Columns(2).HeaderText = "Petugas"
                .Columns(3).HeaderText = "Dari Unit"
                .Columns(4).HeaderText = "ID Barang"
                .Columns(5).HeaderText = "Kode Barang"
                .Columns(6).HeaderText = "Nama Barang"
                .Columns(7).HeaderText = "Jumlah Yang Diminta"
                .Columns(7).DefaultCellStyle.Format = "N2"
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(8).HeaderText = "Satuan"
                .Columns(9).HeaderText = "Harga"
                .Columns(9).DefaultCellStyle.Format = "N2"
                .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(10).HeaderText = "Jumlah Harga"
                .Columns(10).DefaultCellStyle.Format = "N2"
                .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(11).HeaderText = "Tanggal Exp"
                .Columns(12).HeaderText = "No Batch"
                .Columns(13).HeaderText = "Posting"
                .Columns(0).Width = 75
                .Columns(1).Width = 85
                .Columns(2).Width = 100
                .Columns(3).Width = 120
                .Columns(4).Width = 80
                .Columns(5).Width = 80
                .Columns(6).Width = 150
                .Columns(7).Width = 50
                .Columns(8).Width = 75
                .Columns(9).Width = 90
                .Columns(10).Width = 120
                .Columns(11).Width = 75
                .Columns(12).Width = 75
                .Columns(13).Width = 50
                .BackgroundColor = Color.Azure
                .DefaultCellStyle.SelectionBackColor = Color.LightBlue
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .RowHeadersDefaultCellStyle.BackColor = Color.Black
                .ReadOnly = True
                For i As Integer = 0 To .RowCount - 1
                    If Trim(.Rows(i).Cells("posting").Value) = "2" Then
                        .Rows(i).Cells("nmbarang").Style.BackColor = Color.Aquamarine
                    End If
                Next
            End With
            MsgBox("Data sudah ditampilkan", vbInformation, "Informasi")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub tampilPerBarang()
        Try
            DA = New OleDb.OleDbDataAdapter("select tanggal, nota, LTRIM(RTRIM(nmkasir)) as nmkasir, RTRIM(LTRIM(nmbagian)) as nmbagian, idbarang, kdbarang, RTRIM(LTRIM(nmbarang)) as nmbarang, jml, RTRIM(LTRIM(nmsatuan)) as nmsatuan, harga, jmlharga, tglexp, RTRIM(LTRIM(nobatch)) as nobatch, posting from ap_mintabrg where tanggal >= '" & Format(DTPTanggalAwalTab3.Value, "yyyy/MM/dd") & "' and tanggal <= '" & Format(DTPTanggalAkhirTab3.Value, "yyyy/MM/dd") & "' and kdbarang='" & Trim(txtKodeBarangTab3.Text) & "' order by posting Desc,tanggal,nota,nomer", CONN)
            DS = New DataSet
            DA.Fill(DS, "permintaanGudangPerBarang")
            BDPerBarang.DataSource = DS
            BDPerBarang.DataMember = "permintaanGudangPerBarang"
            With gridTab3
                .DataSource = Nothing
                .DataSource = BDPerBarang
                .Columns(0).HeaderText = "Tanggal Permintaan"
                .Columns(1).HeaderText = "Nota Permintaan"
                .Columns(2).HeaderText = "Petugas"
                .Columns(3).HeaderText = "Dari Unit"
                .Columns(4).HeaderText = "ID Barang"
                .Columns(5).HeaderText = "Kode Barang"
                .Columns(6).HeaderText = "Nama Barang"
                .Columns(7).HeaderText = "Jumlah Yang Diminta"
                .Columns(7).DefaultCellStyle.Format = "N2"
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(8).HeaderText = "Satuan"
                .Columns(9).HeaderText = "Harga"
                .Columns(9).DefaultCellStyle.Format = "N2"
                .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(10).HeaderText = "Jumlah Harga"
                .Columns(10).DefaultCellStyle.Format = "N2"
                .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(11).HeaderText = "Tanggal Exp"
                .Columns(12).HeaderText = "No Batch"
                .Columns(13).HeaderText = "Posting"
                .Columns(0).Width = 75
                .Columns(1).Width = 85
                .Columns(2).Width = 100
                .Columns(3).Width = 120
                .Columns(4).Width = 80
                .Columns(5).Width = 80
                .Columns(6).Width = 150
                .Columns(7).Width = 50
                .Columns(8).Width = 75
                .Columns(9).Width = 90
                .Columns(10).Width = 120
                .Columns(11).Width = 75
                .Columns(12).Width = 75
                .Columns(13).Width = 50
                .BackgroundColor = Color.Azure
                .DefaultCellStyle.SelectionBackColor = Color.LightBlue
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .RowHeadersDefaultCellStyle.BackColor = Color.Black
                .ReadOnly = True
                For i As Integer = 0 To .RowCount - 1
                    If Trim(.Rows(i).Cells("posting").Value) = "2" Then
                        .Rows(i).Cells("nmbarang").Style.BackColor = Color.Aquamarine
                    End If
                Next
            End With
            MsgBox("Data sudah ditampilkan", vbInformation, "Informasi")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub tampilPerUnitTanggal()
        Try
            DA = New OleDb.OleDbDataAdapter("select tanggal,nota,RTRIM(LTRIM(nmkasir)) as nmkasir,RTRIM(LTRIM(nmbagian)) as nmbagian,idbarang,kdbarang,RTRIM(LTRIM(nmbarang)) as nmbarang, jml,RTRIM(LTRIM(nmsatuan)) as nmsatuan,harga,jmlharga,tglexp,RTRIM(LTRIM(nobatch)) as nobatch,posting from ap_mintabrg where tanggal >= '" & Format(DTPTanggalAwalTab4.Value, "yyyy/MM/dd") & "' and tanggal <= '" & Format(DTPTanggalAkhirTab4.Value, "yyyy/MM/dd") & "' and kdbagian='" & Trim(kdBagian) & "'order by posting Desc,tanggal,nota,nomer", CONN)
            DS = New DataSet
            DA.Fill(DS, "permintaanGudangPerUnitTanggal")
            BDPerUnitTanggal.DataSource = DS
            BDPerUnitTanggal.DataMember = "permintaanGudangPerUnitTanggal"
            With gridTab4
                .DataSource = Nothing
                .DataSource = BDPerUnitTanggal
                .Columns(0).HeaderText = "Tanggal Permintaan"
                .Columns(1).HeaderText = "Nota Permintaan"
                .Columns(2).HeaderText = "Petugas"
                .Columns(3).HeaderText = "Dari Unit"
                .Columns(4).HeaderText = "ID Barang"
                .Columns(5).HeaderText = "Kode Barang"
                .Columns(6).HeaderText = "Nama Barang"
                .Columns(7).HeaderText = "Jumlah Yang Diminta"
                .Columns(7).DefaultCellStyle.Format = "N2"
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(8).HeaderText = "Satuan"
                .Columns(9).HeaderText = "Harga"
                .Columns(9).DefaultCellStyle.Format = "N2"
                .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(10).HeaderText = "Jumlah Harga"
                .Columns(10).DefaultCellStyle.Format = "N2"
                .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(11).HeaderText = "Tanggal Exp"
                .Columns(12).HeaderText = "No Batch"
                .Columns(13).HeaderText = "Posting"
                .Columns(0).Width = 75
                .Columns(1).Width = 85
                .Columns(2).Width = 100
                .Columns(3).Width = 120
                .Columns(4).Width = 80
                .Columns(5).Width = 80
                .Columns(6).Width = 150
                .Columns(7).Width = 50
                .Columns(8).Width = 75
                .Columns(9).Width = 90
                .Columns(10).Width = 120
                .Columns(11).Width = 75
                .Columns(12).Width = 75
                .Columns(13).Width = 50
                .BackgroundColor = Color.Azure
                .DefaultCellStyle.SelectionBackColor = Color.LightBlue
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .RowHeadersDefaultCellStyle.BackColor = Color.Black
                .ReadOnly = True
                For i As Integer = 0 To .RowCount - 1
                    If Trim(.Rows(i).Cells("posting").Value) = "2" Then
                        .Rows(i).Cells("nmbarang").Style.BackColor = Color.Aquamarine
                    End If
                Next
            End With
            MsgBox("Data sudah ditampilkan", vbInformation, "Informasi")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub TotalHargaTab1()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To GridTab1.RowCount - 1
            HitungTotal = HitungTotal + GridTab1.Rows(baris).Cells("jmlharga").Value
        Next
        txtGrandTotalTab1.DecimalValue = HitungTotal
    End Sub

    Sub TotalHargaTab2()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To GridTab2.RowCount - 1
            HitungTotal = HitungTotal + GridTab2.Rows(baris).Cells("jmlharga").Value
        Next
        txtGrandTotalTab2.DecimalValue = HitungTotal
    End Sub

    Sub TotalHargaTab3()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To gridTab3.RowCount - 1
            HitungTotal = HitungTotal + gridTab3.Rows(baris).Cells("jmlharga").Value
        Next
        txtGrandTotalTab3.DecimalValue = HitungTotal
    End Sub

    Sub TotalHargaTab4()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To gridTab4.RowCount - 1
            HitungTotal = HitungTotal + gridTab4.Rows(baris).Cells("jmlharga").Value
        Next
        txtGrandTotalTab4.DecimalValue = HitungTotal
    End Sub

    Sub ListBagian()
        CMD = New OleDb.OleDbCommand("select kdbagian, nmbagian from ap_bagian order by kdbagian", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
        cmbBagianTab2.Items.Clear()
        cmbBagianTab2.Items.Add("")
        cmbBagianTab4.Items.Clear()
        cmbBagianTab4.Items.Add("")
        For i As Integer = 0 To DT.Rows.Count - 1
            cmbBagianTab2.Items.Add(DT.Rows(i)("nmbagian") & "|" & DT.Rows(i)("kdbagian"))
            cmbBagianTab4.Items.Add(DT.Rows(i)("nmbagian") & "|" & DT.Rows(i)("kdbagian"))
        Next
        cmbBagianTab2.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbBagianTab2.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbBagianTab4.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbBagianTab4.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    End Sub

    Sub cariBagianTab2()
        Dim cari As String = InStr(cmbBagianTab2.Text, "|")
        If cari Then
            Dim ary As String() = Nothing
            ary = Strings.Split(cmbBagianTab2.Text, "|", -1, CompareMethod.Binary)
            nmBagian = (ary(0))
            kdBagian = (ary(1))
        End If
    End Sub

    Sub cariBagianTab4()
        Dim cari As String = InStr(cmbBagianTab4.Text, "|")
        If cari Then
            Dim ary As String() = Nothing
            ary = Strings.Split(cmbBagianTab4.Text, "|", -1, CompareMethod.Binary)
            nmBagian = (ary(0))
            kdBagian = (ary(1))
        End If
    End Sub

    Sub tampilBarang()
        If pkdapo = "001" Then
            Stok = "stok001"
        ElseIf pkdapo = "002" Then
            Stok = "stok002"
        ElseIf pkdapo = "003" Then
            Stok = "stok003"
        ElseIf pkdapo = "004" Then
            Stok = "stok004"
        ElseIf pkdapo = "005" Then
            Stok = "stok005"
        ElseIf pkdapo = "006" Then
            Stok = "stok006"
        ElseIf pkdapo = "007" Then
            Stok = "stok007"
        End If
        Try
            DA = New OleDb.OleDbDataAdapter("select idx_barang, kd_barang, LTRIM(RTRIM(nama_barang)) as nama_barang," & Stok & ", LTRIM(RTRIM(kd_satuan_kecil)) as kd_satuan_kecil, LTRIM(RTRIM(keterangan)) as keterangan from barang_farmasi WHERE stsaktif ='1' order by kd_barang", CONN)
            DS = New DataSet
            DA.Fill(DS, "obat")
            BDDataBarang.DataSource = DS
            BDDataBarang.DataMember = "obat"

            With gridBarang
                .DataSource = Nothing
                .DataSource = BDDataBarang
                .Columns(1).HeaderText = "ID Barang"
                .Columns(2).HeaderText = "Kode Barang"
                .Columns(3).HeaderText = "Nama Barang"
                .Columns(4).HeaderText = "Stok"
                .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(5).HeaderText = "Satuan"
                .Columns(6).HeaderText = "Keterangan"
                .Columns(0).Width = 30
                .Columns(1).Width = 50
                .Columns(2).Width = 75
                .Columns(3).Width = 190
                .Columns(4).Width = 40
                .Columns(5).Width = 50
                .Columns(6).Width = 120
                .BackgroundColor = Color.Azure
                .DefaultCellStyle.SelectionBackColor = Color.LightBlue
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .RowHeadersDefaultCellStyle.BackColor = Color.Black
                .ReadOnly = True
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub cetakPerTanggal()
        rptPerTanggal = New ReportDocument
        Try
            Dim str As String = Application.StartupPath & "\Report\LaporanPermintaanKeGudangPerTanggal.rpt"
            rptPerTanggal.Load(str)
            FormCetak.CrystalReportViewer1.Refresh()
            rptPerTanggal.SetDatabaseLogon(dbUser, dbPassword)
            rptPerTanggal.SetParameterValue("tglAwal", Format(DTPTanggalAwalTab1.Value, "yyyy/MM/dd"))
            rptPerTanggal.SetParameterValue("tglAkhir", Format(DTPTanggalAkhirTab1.Value, "yyyy/MM/dd"))
            rptPerTanggal.SetParameterValue("tanggalAwal", DTPTanggalAwalTab1.Text)
            rptPerTanggal.SetParameterValue("tanggalAkhir", DTPTanggalAkhirTab1.Text)
            FormCetak.CrystalReportViewer1.ReportSource = rptPerTanggal
            FormCetak.CrystalReportViewer1.Show()
            FormCetak.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub cetakPerUnit()
        rptPerUnit = New ReportDocument
        Try
            Dim str As String = Application.StartupPath & "\Report\LaporanPermintaanKeGudangPerUnit.rpt"
            rptPerUnit.Load(str)
            FormCetak.CrystalReportViewer1.Refresh()
            rptPerUnit.SetDatabaseLogon(dbUser, dbPassword)
            rptPerUnit.SetParameterValue("Bulan", Month(DTPBulanTab2.Value))
            rptPerUnit.SetParameterValue("Tahun", Year(DTPTahunTab2.Value))
            rptPerUnit.SetParameterValue("NBulan", DTPBulanTab2.Text)
            rptPerUnit.SetParameterValue("kdBagian", kdBagian)
            FormCetak.CrystalReportViewer1.ReportSource = rptPerUnit
            FormCetak.CrystalReportViewer1.Show()
            FormCetak.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub cetakPerBarang()
        rptPerBarang = New ReportDocument
        Try
            Dim str As String = Application.StartupPath & "\Report\LaporanPermintaanKeGudangPerBarang.rpt"
            rptPerBarang.Load(str)
            FormCetak.CrystalReportViewer1.Refresh()
            rptPerBarang.SetDatabaseLogon(dbUser, dbPassword)
            rptPerBarang.SetParameterValue("tglAwal", Format(DTPTanggalAwalTab3.Value, "yyyy/MM/dd"))
            rptPerBarang.SetParameterValue("tglAkhir", Format(DTPTanggalAkhirTab3.Value, "yyyy/MM/dd"))
            rptPerBarang.SetParameterValue("tanggalAwal", DTPTanggalAwalTab3.Text)
            rptPerBarang.SetParameterValue("tanggalAkhir", DTPTanggalAkhirTab3.Text)
            rptPerBarang.SetParameterValue("kdBarang", txtKodeBarangTab3.Text)
            FormCetak.CrystalReportViewer1.ReportSource = rptPerBarang
            FormCetak.CrystalReportViewer1.Show()
            FormCetak.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub cetakPerUnitTanggal()
        rptPerUnitTanggal = New ReportDocument
        Try
            Dim str As String = Application.StartupPath & "\Report\LaporanPermintaanKeGudangPerTanggalUnit.rpt"
            rptPerUnitTanggal.Load(str)
            FormCetak.CrystalReportViewer1.Refresh()
            rptPerUnitTanggal.SetDatabaseLogon(dbUser, dbPassword)
            rptPerUnitTanggal.SetParameterValue("tglAwal", Format(DTPTanggalAwalTab4.Value, "yyyy/MM/dd"))
            rptPerUnitTanggal.SetParameterValue("tglAkhir", Format(DTPTanggalAkhirTab4.Value, "yyyy/MM/dd"))
            rptPerUnitTanggal.SetParameterValue("tanggalAwal", DTPTanggalAwalTab4.Text)
            rptPerUnitTanggal.SetParameterValue("tanggalAkhir", DTPTanggalAkhirTab4.Text)
            rptPerUnitTanggal.SetParameterValue("kdBagian", Trim(kdBagian))
            FormCetak.CrystalReportViewer1.ReportSource = rptPerUnitTanggal
            FormCetak.CrystalReportViewer1.Show()
            FormCetak.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FormLaporanPermintaanBarangKeGudang_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dispose()
    End Sub

    Private Sub FormLaporanPermintaanBarangKeGudang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setApo()
        Kosongkan()
        ListBagian()
    End Sub

    Private Sub btnProsesTab1_Click(sender As Object, e As EventArgs) Handles btnProsesTab1.Click
        tampilPerTanggal()
        TotalHargaTab1()
    End Sub

    Private Sub btnBaruTab1_Click(sender As Object, e As EventArgs) Handles btnBaruTab1.Click
        Kosongkan()
        TotalHargaTab1()
    End Sub

    Private Sub btnExcelTab1_Click(sender As Object, e As EventArgs) Handles btnExcelTab1.Click
        If MessageBox.Show("Apakah data akan di eksport ke excel?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            'GridTab1.DataSource = DS.Tables("permintaanGudangPerTanggal")
            Try
                Dim dtXls As DataTable = CType(DS.Tables("permintaanGudangPerTanggal"), DataTable)
                Dim excelEngine As New ExcelEngine
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2007
                Dim workbook As IWorkbook = excelEngine.Excel.Workbooks.Open(Application.StartupPath & "\Report\LaporanPermintaanKeGudangPerTanggalXLSIO.xlsx")
                Dim sheet As IWorksheet = workbook.Worksheets(0)
                sheet.Range("B7").Text = DTPTanggalAwalTab1.Text
                sheet.Range("B8").Text = DTPTanggalAkhirTab1.Text
                Dim marker As ITemplateMarkersProcessor = workbook.CreateTemplateMarkersProcessor
                marker.AddVariable("Data", dtXls)
                marker.ApplyMarkers()
                workbook.Version = ExcelVersion.Excel2007
                workbook.SaveAs("Laporan Permintaan Ke Gudang Per Tanggal.xlsx")
                workbook.Close()
                excelEngine.Dispose()
                System.Diagnostics.Process.Start("Laporan Permintaan Ke Gudang Per Tanggal.xlsx")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnProsesTab2_Click(sender As Object, e As EventArgs) Handles btnProsesTab2.Click
        If cmbBagianTab2.Text = "" Then
            MsgBox("Bagian/ Unit belum dipilih")
            cmbBagianTab2.Focus()
            Exit Sub
        End If
        cariBagianTab2()
        tampilPerUnit()
        TotalHargaTab2()
    End Sub

    Private Sub btnBaruTab2_Click(sender As Object, e As EventArgs) Handles btnBaruTab2.Click
        Kosongkan()
        TotalHargaTab2()
    End Sub

    Private Sub btnExcelTab2_Click(sender As Object, e As EventArgs) Handles btnExcelTab2.Click
        If MessageBox.Show("Apakah data akan di eksport ke excel?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            'GridTab1.DataSource = DS.Tables("permintaanGudangPerTanggal")
            Try
                Dim dtXls As DataTable = CType(DS.Tables("permintaanGudangPerUnit"), DataTable)
                Dim excelEngine As New ExcelEngine
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2007
                Dim workbook As IWorkbook = excelEngine.Excel.Workbooks.Open(Application.StartupPath & "\Report\LaporanPermintaanKeGudangPerUnitXLSIO.xlsx")
                Dim sheet As IWorksheet = workbook.Worksheets(0)
                sheet.Range("B7").Text = DTPBulanTab2.Text
                sheet.Range("B8").Text = DTPTahunTab2.Text
                Dim marker As ITemplateMarkersProcessor = workbook.CreateTemplateMarkersProcessor
                marker.AddVariable("Data", dtXls)
                marker.ApplyMarkers()
                workbook.Version = ExcelVersion.Excel2007
                workbook.SaveAs("Laporan Permintaan Ke Gudang Per Unit.xlsx")
                workbook.Close()
                excelEngine.Dispose()
                System.Diagnostics.Process.Start("Laporan Permintaan Ke Gudang Per Unit.xlsx")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PanelObat.Visible = False
    End Sub

    Private Sub FormLaporanPermintaanBarangKeGudang_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelObat.Top = txtKodeBarangTab3.Top + 50
        PanelObat.Left = txtKodeBarangTab3.Left + 1
    End Sub

    Private Sub txtKodeBarangTab3_Click(sender As Object, e As EventArgs) Handles txtKodeBarangTab3.Click
        PanelObat.Visible = True
        tampilBarang()
        txtCariObat.Clear()
        txtCariObat.Focus()
    End Sub

    Private Sub txtKodeBarangTab3_GotFocus(sender As Object, e As EventArgs) Handles txtKodeBarangTab3.GotFocus
        PanelObat.Visible = True
        tampilBarang()
        txtCariObat.Clear()
        txtCariObat.Focus()
    End Sub

    Private Sub txtCariObat_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCariObat.KeyDown
        If e.KeyCode = Keys.Down Then
            gridBarang.Focus()
        End If
    End Sub

    Private Sub txtCariObat_TextChanged(sender As Object, e As EventArgs) Handles txtCariObat.TextChanged
        BDDataBarang.Filter = "nama_barang like '%" & txtCariObat.Text & "%'"
    End Sub

    Private Sub gridBarang_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridBarang.CellContentClick
        If e.ColumnIndex = 0 Then
            If Not IsDBNull(gridBarang.Rows(e.RowIndex).Cells(1).Value) Then
                txtKodeBarangTab3.Text = gridBarang.Rows(e.RowIndex).Cells(2).Value
                txtNamaBarangTab3.Text = gridBarang.Rows(e.RowIndex).Cells(3).Value
                PanelObat.Visible = False
                btnProsesTab3.Focus()
            End If
        End If
    End Sub

    Private Sub btnProsesTab3_Click(sender As Object, e As EventArgs) Handles btnProsesTab3.Click
        If txtKodeBarangTab3.Text = "" Then
            MsgBox("Barang belum dipilih")
            txtKodeBarangTab3.Focus()
            Exit Sub
        End If
        tampilPerBarang()
        TotalHargaTab3()
    End Sub

    Private Sub gridBarang_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridBarang.KeyPress
        If e.KeyChar = Chr(13) Then
            Dim i = gridBarang.CurrentRow.Index - 1
            If Not IsDBNull(gridBarang.Rows(i).Cells(1).Value) Then
                txtKodeBarangTab3.Text = gridBarang.Rows(i).Cells(2).Value
                txtNamaBarangTab3.Text = gridBarang.Rows(i).Cells(3).Value
                PanelObat.Visible = False
                btnProsesTab3.Focus()
            End If
        End If
    End Sub

    Private Sub btnBaruTab3_Click(sender As Object, e As EventArgs) Handles btnBaruTab3.Click
        Kosongkan()
    End Sub

    Private Sub btnExcelTab3_Click(sender As Object, e As EventArgs) Handles btnExcelTab3.Click
        If MessageBox.Show("Apakah data akan di eksport ke excel?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            'GridTab1.DataSource = DS.Tables("permintaanGudangPerTanggal")
            Try
                Dim dtXls As DataTable = CType(DS.Tables("permintaanGudangPerBarang"), DataTable)
                Dim excelEngine As New ExcelEngine
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2007
                Dim workbook As IWorkbook = excelEngine.Excel.Workbooks.Open(Application.StartupPath & "\Report\LaporanPermintaanKeGudangPerBarangXLSIO.xlsx")
                Dim sheet As IWorksheet = workbook.Worksheets(0)
                sheet.Range("B7").Text = DTPTanggalAwalTab3.Text
                sheet.Range("B8").Text = DTPTanggalAkhirTab3.Text
                Dim marker As ITemplateMarkersProcessor = workbook.CreateTemplateMarkersProcessor
                marker.AddVariable("Data", dtXls)
                marker.ApplyMarkers()
                workbook.Version = ExcelVersion.Excel2007
                workbook.SaveAs("Laporan Permintaan Ke Gudang Per Barang.xlsx")
                workbook.Close()
                excelEngine.Dispose()
                System.Diagnostics.Process.Start("Laporan Permintaan Ke Gudang Per Barang.xlsx")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnSimpanTab4_Click(sender As Object, e As EventArgs) Handles btnSimpanTab4.Click
        If cmbBagianTab4.Text = "" Then
            MsgBox("Bagian/ Unit belum dipilih")
            cmbBagianTab4.Focus()
            Exit Sub
        End If
        cariBagianTab4()
        tampilPerUnitTanggal()
        TotalHargaTab4()
    End Sub

    Private Sub btnBaruTab4_Click(sender As Object, e As EventArgs) Handles btnBaruTab4.Click
        Kosongkan()
    End Sub

    Private Sub btnExcelTab4_Click(sender As Object, e As EventArgs) Handles btnExcelTab4.Click
        If MessageBox.Show("Apakah data akan di eksport ke excel?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            'GridTab1.DataSource = DS.Tables("permintaanGudangPerTanggal")
            Try
                Dim dtXls As DataTable = CType(DS.Tables("permintaanGudangPerUnitTanggal"), DataTable)
                Dim excelEngine As New ExcelEngine
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2007
                Dim workbook As IWorkbook = excelEngine.Excel.Workbooks.Open(Application.StartupPath & "\Report\LaporanPermintaanKeGudangPerUnitTanggalXLSIO.xlsx")
                Dim sheet As IWorksheet = workbook.Worksheets(0)
                sheet.Range("B7").Text = DTPTanggalAwalTab4.Text
                sheet.Range("B8").Text = DTPTanggalAkhirTab4.Text
                Dim marker As ITemplateMarkersProcessor = workbook.CreateTemplateMarkersProcessor
                marker.AddVariable("Data", dtXls)
                marker.ApplyMarkers()
                workbook.Version = ExcelVersion.Excel2007
                workbook.SaveAs("Laporan Permintaan Ke Gudang Per Unit Tanggal.xlsx")
                workbook.Close()
                excelEngine.Dispose()
                System.Diagnostics.Process.Start("Laporan Permintaan Ke Gudang Per Unit Tanggal.xlsx")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnPreviewTab1_Click(sender As Object, e As EventArgs) Handles btnPreviewTab1.Click
        FormPemanggil = "FormLaporanPermintaanBarangKeGudang_PerTanggal"
        cetakPerTanggal()
    End Sub

    Private Sub btnPreviewTab2_Click(sender As Object, e As EventArgs) Handles btnPreviewTab2.Click
        FormPemanggil = "FormLaporanPermintaanBarangKeGudang_PerUnit"
        cetakPerUnit()
    End Sub

    Private Sub btnPreviewTab3_Click(sender As Object, e As EventArgs) Handles btnPreviewTab3.Click
        FormPemanggil = "FormLaporanPermintaanBarangKeGudang_PerBarang"
        cetakPerBarang()
    End Sub

    Private Sub btnPreviewTab4_Click(sender As Object, e As EventArgs) Handles btnPreviewTab4.Click
        FormPemanggil = "FormLaporanPermintaanBarangKeGudang_PerTanggalUnit"
        cetakPerUnitTanggal()
    End Sub

    Private Sub txtKodeBarangTab3_TextChanged(sender As Object, e As EventArgs) Handles txtKodeBarangTab3.TextChanged

    End Sub
End Class