Imports Syncfusion.Windows.Forms
Imports Syncfusion.Windows.Forms.Tools
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class FormEditStatusBayar
    Inherits Office2010Form
    Public rptNota, rptBPJS, rptLain As New ReportDocument
    Dim StatusRawat, JenisRawat, KdPenjamin, kdDokter, kdPoliklinik, kdTempatTidur, Stok, Generik, KdJenisObat, kdPabrik, kdKelompokObat, kdGolonganObat, NamaPenjamin, NamaDokter, kdTakaran, kdWaktu, kdKeterangan, JenisObat, memStok, kdSubUnit, Posting, nmPaket, kdRekening As String
    Public nmSubUnit, bilang, kdUnitDepo, kdApo As String

    Dim tglLahirPasien As DateTime
    Dim HargaBeli As Double

    Dim Bulan, Tahun As Integer
    Dim BDPenjualanResep, BDPenjualanResepKh, BDDataBarang, BDDataPasien, BDEtiket As New BindingSource
    Dim DSPenjualanResep, DSPenjualanResepKh, DSEtiket As New DataSet
    Dim DRWPenjualanResep, DRWPenjualanResepKh, DRWEtiket As DataRowView

    'Dim Trans As SqlTransaction
    Dim Trans As OleDb.OleDbTransaction

    Sub KosongkanHeader()
        DSPenjualanResep = Table.BuatTabelPenjualanResep("PenjualanResep")
        DSPenjualanResepKh = Table.BuatTabelPenjualanResepKh("PenjualanResepKh")
        gridDetailObat.BackgroundColor = Color.Azure
        DSPenjualanResep.Clear()
        gridDetailObat.DataSource = Nothing
        gridDetailObatKh.BackgroundColor = Color.Azure
        DSPenjualanResepKh.Clear()
        gridDetailObatKh.DataSource = Nothing
        gridStokKembali.DataSource = Nothing
        TglServer()
        DTPTanggalTrans.Value = TanggalServer
        DTPJamAwal.Value = TanggalServer
        DTPTanggalExp.Value = DateAdd("d", 30, DTPTanggalTrans.Value)
        txtNoResep.Clear()
        txtNoReg.Clear()
        txtJnsRawat.Clear()
        txtNoUrut.Clear()
        txtRM.Clear()
        txtSex.Clear()
        txtUmurBln.Clear()
        txtUmurThn.Clear()
        txtNamaPasien.Clear()
        txtAlamat.Clear()
        txtGrandTotal.Clear()
        txtGrandTotalBulat.Clear()
        txtGrandDijamin.Clear()
        txtGrandDijaminBulat.Clear()
        txtGrandIurBayar.Clear()
        txtGrandIurBayarBulat.Clear()
        txtGrandTotalPaket.Clear()
        txtGrandTotalPaketBulat.Clear()
        txtGrandTotalNonPaket.Clear()
        txtGrandTotalNonPaketBulat.Clear()
        txtQtyKh.Clear()
        cmbUnitAsal.Text = ""
        cmbPenjamin.Text = ""
        cmbDokter.Text = ""
        txtNota.Text = "-"
        btnSimpan.Enabled = False
        btnCetakNota.Enabled = False
        btnBaru.Enabled = False
        btnSimpanKh.Enabled = False
        btnCetakBPJS.Enabled = False
        btnCetakLain.Enabled = False
        btnInfoResepKh.Enabled = False
        btnCetakEtiketKh.Enabled = False
        btnBaruKh.Enabled = False
        btnHapusNotaKh.Enabled = False
        TabPktUmum.TabVisible = True
        TabPktKhusus.TabVisible = False
        btnUpdateDijamin.Enabled = False
        btnUpdateIurPasien.Enabled = False
        cmbPkt.SelectedIndex = 0
        DTPTanggalTrans.Focus()
    End Sub

    Sub KosongkanDetailPaketUmum()
        cmbRacikNon.Text = "N"
        lblNamaObat.Text = ""
        txtKodeObat.Clear()
        txtIdObat.Clear()
        txtDosis.Clear()
        txtSatDosis.Clear()
        txtHargaJual.Clear()
        txtJumlahJual.Clear()
        txtKdSatuan.Clear()
        txtSenPotBeli.Clear()
        txtJumlahHarga.Clear()
        txtDijamin.Clear()
        cmbDijamin.Text = ""
        txtIuranSisaBayar.Clear()
        txtJmlHari.IntegerValue = 0
        cmbEtiket.Text = "N"
        TabPktUmum.TabVisible = True
        TabPktKhusus.TabVisible = False
        cmbWaktu.SelectedIndex = 1
        cmbTakaran.SelectedIndex = 1
        cmbKeterangan.SelectedIndex = 1
        txtSigna1.Text = "0"
        txtSigna2.Text = "0"
        txtQty3.DecimalValue = 0
        txtJumlahObatEtiket.DecimalValue = 0
        txtJarakED.DecimalValue = 0
    End Sub

    Sub KosongkanDetailPaketKhusus()
        cmbRacikNonKh.Text = "N"
        lblNamaObatKh.Text = ""
        txtKodeObatKh.Clear()
        txtIdObatKh.Clear()
        txtDosisKh.Clear()
        txtSatDosisKh.Clear()
        txtHargaJualKh.Clear()
        txtDosisResepKh.Clear()
        txtJmlCapBPJSKh.Clear()
        txtJmlCapLainKh.Clear()
        txtJmlObatKh.Clear()
        txtPaketBPJSKh.Clear()
        txtSatPaketBPJSKh.Clear()
        txtPaketLainKh.Clear()
        txtSatPaketLainKh.Clear()
        txtTotalPaketBPJSKh.Clear()
        txtTotalPaketLainKh.Clear()
        txtJmlHariKh.IntegerValue = 0
        cmbEtiketKh.Text = "N"
        cmbWaktu.SelectedIndex = 1
        cmbTakaran.SelectedIndex = 1
        cmbKeterangan.SelectedIndex = 1
        txtSigna1.Text = "0"
        txtSigna2.Text = "0"
        txtQty3.DecimalValue = 0
        txtJumlahObatEtiket.DecimalValue = 0
        txtJarakED.DecimalValue = 0
    End Sub

    Sub AturGriddetailBarang()
        With gridDetailObat
            .Columns(0).HeaderText = "No"
            .Columns(0).ReadOnly = True
            .Columns(1).HeaderText = "R/N"
            .Columns(1).ReadOnly = True
            .Columns(2).HeaderText = "Nama Barang"
            .Columns(2).ReadOnly = True
            .Columns(3).HeaderText = "Harga"
            .Columns(3).ReadOnly = True
            .Columns(3).DefaultCellStyle.Format = "N2"
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(4).HeaderText = "Jumlah"
            .Columns(4).DefaultCellStyle.Format = "N2"
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(4).DefaultCellStyle.BackColor = Color.LightYellow
            .Columns(5).HeaderText = "Satuan"
            .Columns(5).ReadOnly = True
            .Columns(6).HeaderText = "Jumlah Harga"
            .Columns(6).ReadOnly = True
            .Columns(6).DefaultCellStyle.Format = "N2"
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(7).HeaderText = "Dijamin"
            .Columns(7).ReadOnly = True
            .Columns(7).DefaultCellStyle.Format = "N2"
            .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(8).HeaderText = "Iur Pasien"
            .Columns(8).ReadOnly = True
            .Columns(8).DefaultCellStyle.Format = "N2"
            .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(9).HeaderText = "Jml Hari"
            .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(9).ReadOnly = True
            .Columns(0).Width = 40
            .Columns(1).Width = 40
            .Columns(2).Width = 320
            .Columns(3).Width = 100
            .Columns(4).Width = 100
            .Columns(5).Width = 80
            .Columns(6).Width = 120
            .Columns(7).Width = 100
            .Columns(8).Width = 100
            .Columns(9).Width = 40
            .Columns(0).Visible = False
            .Columns(10).Visible = False
            .Columns(11).Visible = False
            .Columns(12).Visible = False
            .Columns(13).Visible = False
            .Columns(14).Visible = False
            .Columns(15).Visible = False
            .Columns(16).Visible = False
            .Columns(17).Visible = False
            .Columns(18).Visible = False
            .Columns(19).Visible = False
            .Columns(20).Visible = False
            .Columns(21).Visible = False
            .Columns(22).Visible = False
            .Columns(23).Visible = False
            .Columns(24).Visible = False
            .Columns(25).Visible = False
            .Columns(26).Visible = False
            .Columns(27).Visible = False
            .Columns(28).Visible = False
            .Columns(29).Visible = False
            .Columns(30).Visible = False
            .Columns(31).Visible = False
            .Columns(32).Visible = False
            .Columns(33).Visible = False
            .Columns(34).Visible = False
            .Columns(35).Visible = False
            .Columns(36).Visible = False
            .Columns(37).Visible = False
            .Columns(38).Visible = False
            .Columns(39).Visible = False
            .Columns(40).Visible = False
            .Columns(41).Visible = False
            .Columns(42).Visible = False
            .Columns(43).Visible = False
            .Columns(44).Visible = False
            .Columns(45).Visible = False
            .Columns(46).Visible = False
            .Columns(47).Visible = False
            .Columns(48).Visible = False
            .Columns(49).Visible = False
            .Columns(50).Visible = False
            .Columns(51).Visible = False
            .Columns(52).Visible = False
            .Columns(53).Visible = False
            .Columns(54).Visible = False
            .Columns(55).Visible = False
            .Columns(56).Visible = False
            .Columns(57).Visible = False
            .Columns(58).Visible = False
            .Columns(59).Visible = False
            .Columns(60).Visible = False
            .Columns(61).Visible = False
            .Columns(62).Visible = False
            .Columns(63).Visible = False
            .Columns(64).Visible = False
            .Columns(65).Visible = False
            .Columns(66).Visible = False
            .Columns(67).Visible = False
            .ReadOnly = True
            .BackgroundColor = Color.Azure
            .DefaultCellStyle.SelectionBackColor = Color.LightBlue
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .RowHeadersDefaultCellStyle.BackColor = Color.Black
        End With
    End Sub

    Sub cetakNota()
        rptNota = New ReportDocument
        Try
            Dim str As String = Application.StartupPath & "\Report\notaResep.rpt"
            rptNota.Load(str)
            FormCetak.CrystalReportViewer1.Refresh()
            rptNota.SetDatabaseLogon(dbUser, dbPassword)
            rptNota.SetParameterValue("tanggal", Format(DTPTanggalTrans.Value, "yyyy/MM/dd"))
            rptNota.SetParameterValue("notaresep", txtNoResep.Text)
            rptNota.SetParameterValue("alamat", txtAlamat.Text)
            rptNota.SetParameterValue("unit", nmSubUnit)
            rptNota.SetParameterValue("totalHarga", txtGrandTotalBulat.DecimalValue)
            rptNota.SetParameterValue("totalDijamin", txtGrandDijaminBulat.DecimalValue)
            rptNota.SetParameterValue("totalSisaBayar", txtGrandIurBayarBulat.DecimalValue)
            rptNota.SetParameterValue("terbilang", bilang)
            rptNota.SetParameterValue("nmdepo", pnmapo)
            rptNota.SetParameterValue("umur", txtUmurThn.Text)
            FormCetak.CrystalReportViewer1.ReportSource = rptNota
            FormCetak.CrystalReportViewer1.Show()
            FormCetak.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub cetakNotaBPJS()
        rptBPJS = New ReportDocument
        Try
            Dim str As String = Application.StartupPath & "\Report\notaResepBPJS.rpt"
            rptBPJS.Load(str)
            FormCetak.CrystalReportViewer1.Refresh()
            rptBPJS.SetDatabaseLogon(dbUser, dbPassword)
            rptBPJS.SetParameterValue("tanggal", Format(DTPTanggalTrans.Value, "yyyy/MM/dd"))
            rptBPJS.SetParameterValue("notaresep", txtNoResep.Text)
            rptBPJS.SetParameterValue("alamat", txtAlamat.Text)
            rptBPJS.SetParameterValue("unit", nmSubUnit)
            rptBPJS.SetParameterValue("totalPaketBulat", txtGrandTotalPaketBulat.DecimalValue)
            rptBPJS.SetParameterValue("terbilang", bilang)
            rptBPJS.SetParameterValue("nmdepo", pnmapo)
            rptBPJS.SetParameterValue("umur", txtUmurThn.Text)
            FormCetak.CrystalReportViewer1.ReportSource = rptBPJS
            FormCetak.CrystalReportViewer1.Show()
            FormCetak.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub cetakNotaLain()
        rptLain = New ReportDocument
        Try
            Dim str As String = Application.StartupPath & "\Report\notaResepLain.rpt"
            rptLain.Load(str)
            FormCetak.CrystalReportViewer1.Refresh()
            rptLain.SetDatabaseLogon(dbUser, dbPassword)
            rptLain.SetParameterValue("tanggal", Format(DTPTanggalTrans.Value, "yyyy/MM/dd"))
            rptLain.SetParameterValue("notaresep", txtNoResep.Text)
            rptLain.SetParameterValue("alamat", txtAlamat.Text)
            rptLain.SetParameterValue("unit", nmSubUnit)
            rptLain.SetParameterValue("totalNonPaketBulat", txtGrandTotalNonPaketBulat.DecimalValue)
            rptLain.SetParameterValue("terbilang", bilang)
            rptLain.SetParameterValue("nmdepo", pnmapo)
            rptLain.SetParameterValue("umur", txtUmurThn.Text)
            FormCetak.CrystalReportViewer1.ReportSource = rptLain
            FormCetak.CrystalReportViewer1.Show()
            FormCetak.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub ListDokter()
        CMD = New OleDb.OleDbCommand("select kd_pegawai,nama_pegawai,gelar_depan from pegawai where kd_jns_pegawai=1 order by nama_pegawai", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
        cmbDokter.Items.Clear()
        cmbDokter.Items.Add("")
        For i As Integer = 0 To DT.Rows.Count - 1
            cmbDokter.Items.Add(DT.Rows(i)("nama_pegawai") & "|" & DT.Rows(i)("kd_pegawai"))
        Next
        cmbDokter.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbDokter.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    End Sub

    Sub ListEtiketTakaran()
        CMD = New OleDb.OleDbCommand("select * from ap_etiket_takaran order by noid", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
        cmbTakaran.Items.Clear()
        cmbTakaran.Items.Add("")
        For i As Integer = 0 To DT.Rows.Count - 1
            cmbTakaran.Items.Add(DT.Rows(i)("takaran") & "|" & DT.Rows(i)("noid"))
        Next
        cmbTakaran.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbTakaran.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    End Sub

    Sub ListEtiketWaktu()
        CMD = New OleDb.OleDbCommand("select * from ap_etiket_waktu order by noid", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
        cmbWaktu.Items.Clear()
        cmbWaktu.Items.Add("")
        For i As Integer = 0 To DT.Rows.Count - 1
            cmbWaktu.Items.Add(DT.Rows(i)("waktu") & "|" & DT.Rows(i)("noid"))
        Next
        cmbWaktu.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbWaktu.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    End Sub

    Sub ListEtiketKeterangan()
        CMD = New OleDb.OleDbCommand("select * from ap_etiket_ketminum order by noid", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
        cmbKeterangan.Items.Clear()
        cmbKeterangan.Items.Add("")
        For i As Integer = 0 To DT.Rows.Count - 1
            cmbKeterangan.Items.Add(DT.Rows(i)("ketminum") & "|" & DT.Rows(i)("noid"))
        Next
        cmbKeterangan.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbKeterangan.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    End Sub

    Sub tampilPasien()
        Try
            DA = New OleDb.OleDbDataAdapter("SELECT ap_jualr1.kdbagian, ap_jualr1.stsrawat, ap_jualr1.tanggal, ap_jualr1.notaresep, ap_jualr1.no_rm, LTRIM(RTRIM(ap_jualr1.nama_pasien)) AS nama_pasien, LTRIM(RTRIM(ap_jualr1.nmdokter))  AS nmdokter, ap_jualr1.stsresep, ap_seting_apotek.kd_sub_unit FROM ap_jualr1 INNER JOIN ap_seting_apotek ON ap_jualr1.kdbagian = ap_seting_apotek.kdapo where ap_jualr1.stsrawat='" & StatusRawat & "' AND ap_jualr1.tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' order by ap_jualr1.tanggal,ap_jualr1.notaresep", CONN)
            DS = New DataSet
            DA.Fill(DS, "pasien")
            BDDataPasien.DataSource = DS
            BDDataPasien.DataMember = "pasien"
            With gridPasien
                .DataSource = Nothing
                .DataSource = BDDataPasien
                .Columns(1).HeaderText = "Unit Depo"
                .Columns(2).HeaderText = "Status Rawat"
                .Columns(3).HeaderText = "Tanggal Resep"
                .Columns(4).HeaderText = "Nota Resep"
                .Columns(5).HeaderText = "No RM"
                .Columns(6).HeaderText = "Nama Pasien"
                .Columns(7).HeaderText = "Nama Dokter"
                .Columns(8).HeaderText = "Status Resep"
                .Columns(0).Width = 30
                .Columns(1).Width = 40
                .Columns(2).Width = 45
                .Columns(3).Width = 75
                .Columns(4).Width = 100
                .Columns(5).Width = 60
                .Columns(6).Width = 130
                .Columns(7).Width = 130
                .Columns(8).Width = 75
                .Columns(9).Visible = False
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

    Sub cariNamaPenjamin()
        Dim cari As String = InStr(cmbPenjamin.Text, "|")
        If cari Then
            Dim ary As String() = Nothing
            ary = Strings.Split(cmbPenjamin.Text, "|", -1, CompareMethod.Binary)
            NamaPenjamin = (ary(0))
            'KdPenjamin = (ary(1))
        End If
    End Sub

    Sub cariDokter()
        Dim cari As String = InStr(cmbDokter.Text, "|")
        If cari Then
            Dim ary As String() = Nothing
            ary = Strings.Split(cmbDokter.Text, "|", -1, CompareMethod.Binary)
            NamaDokter = (ary(0))
            kdDokter = (ary(1))
        End If
    End Sub

    Sub carikdEtiketTakaran()
        Dim cari As String = InStr(cmbTakaran.Text, "|")
        If cari Then
            Dim ary As String() = Nothing
            ary = Strings.Split(cmbTakaran.Text, "|", -1, CompareMethod.Binary)
            kdTakaran = (ary(1))
        End If
    End Sub

    Sub carikdEtiketWaktu()
        Dim cari As String = InStr(cmbWaktu.Text, "|")
        If cari Then
            Dim ary As String() = Nothing
            ary = Strings.Split(cmbWaktu.Text, "|", -1, CompareMethod.Binary)
            kdWaktu = (ary(1))
        End If
    End Sub

    Sub carikdEtiketKeterangan()
        Dim cari As String = InStr(cmbKeterangan.Text, "|")
        If cari Then
            Dim ary As String() = Nothing
            ary = Strings.Split(cmbKeterangan.Text, "|", -1, CompareMethod.Binary)
            kdKeterangan = (ary(1))
        End If
    End Sub

    Sub cariSubUnitAsal()
        Dim cari As String = InStr(cmbUnitAsal.Text, "|")
        If cari Then
            Dim ary As String() = Nothing
            ary = Strings.Split(cmbUnitAsal.Text, "|", -1, CompareMethod.Binary)
            nmSubUnit = (ary(0))
            kdSubUnit = (ary(1))
        End If
    End Sub

    Sub addBarang()
        cariNamaPenjamin()
        cariDokter()
        carikdEtiketTakaran()
        carikdEtiketWaktu()
        carikdEtiketKeterangan()

        BDPenjualanResep.DataSource = DSPenjualanResep
        BDPenjualanResep.DataMember = "PenjualanResep"

        BDPenjualanResep.AddNew()
        DRWPenjualanResep = BDPenjualanResep.Current
        DRWPenjualanResep("stsrawat") = StatusRawat
        DRWPenjualanResep("kdkasir") = Trim(FormLogin.LabelKode.Text)
        DRWPenjualanResep("nmkasir") = Trim(FormLogin.LabelNama.Text)
        DRWPenjualanResep("tanggal") = DTPTanggalTrans.Value
        DRWPenjualanResep("notaresep") = Trim(txtNoResep.Text)
        DRWPenjualanResep("no_reg") = Trim(txtNoReg.Text)
        DRWPenjualanResep("no_rm") = Trim(txtRM.Text)
        DRWPenjualanResep("nmpasien") = Trim(txtNamaPasien.Text)
        DRWPenjualanResep("umurthn") = txtUmurThn.Text
        DRWPenjualanResep("umurbln") = txtUmurBln.Text
        DRWPenjualanResep("kd_penjamin") = KdPenjamin
        DRWPenjualanResep("nm_penjamin") = NamaPenjamin
        DRWPenjualanResep("kddokter") = kdDokter
        DRWPenjualanResep("nmdokter") = NamaDokter
        DRWPenjualanResep("nonota") = Trim(txtNota.Text)
        DRWPenjualanResep("urut") = 1
        DRWPenjualanResep("kd_barang") = Trim(txtKodeObat.Text)
        DRWPenjualanResep("idx_barang") = Trim(txtIdObat.Text)
        DRWPenjualanResep("nama_barang") = Trim(lblNamaObat.Text)
        DRWPenjualanResep("kd_jns_obat") = KdJenisObat
        DRWPenjualanResep("kd_gol_obat") = kdGolonganObat
        DRWPenjualanResep("kd_kel_obat") = kdKelompokObat
        DRWPenjualanResep("kdpabrik") = kdPabrik
        DRWPenjualanResep("generik") = Generik
        DRWPenjualanResep("formularium") = "FORMULARIUM"
        DRWPenjualanResep("racik") = Trim(cmbRacikNon.Text)
        DRWPenjualanResep("harga") = txtHargaJual.DecimalValue
        DRWPenjualanResep("jmlp") = txtJumlahJual.DecimalValue
        DRWPenjualanResep("totalp") = txtJumlahHarga.DecimalValue
        DRWPenjualanResep("jmln") = 0
        DRWPenjualanResep("totaln") = 0
        DRWPenjualanResep("jml") = txtJumlahJual.DecimalValue
        DRWPenjualanResep("nmsatuan") = Trim(txtKdSatuan.Text)
        DRWPenjualanResep("totalharga") = txtJumlahHarga.DecimalValue
        DRWPenjualanResep("senpot") = 0
        DRWPenjualanResep("potongan") = 0
        DRWPenjualanResep("jmlnet") = txtJumlahHarga.DecimalValue
        DRWPenjualanResep("dijamin") = txtDijamin.DecimalValue
        DRWPenjualanResep("sisabayar") = txtIuranSisaBayar.DecimalValue
        DRWPenjualanResep("hrgbeli") = HargaBeli
        DRWPenjualanResep("jamawal") = Format(DTPJamAwal.Value, "HH:mm:ss").ToString
        DRWPenjualanResep("kdbagian") = pkdapo
        DRWPenjualanResep("stsresep") = "PKTUMUM"
        DRWPenjualanResep("rek_p") = kdRekening
        DRWPenjualanResep("stsetiket") = cmbEtiket.Text
        DRWPenjualanResep("qty1") = txtSigna1.Text
        DRWPenjualanResep("qty2") = txtSigna2.Text
        DRWPenjualanResep("qty3") = txtQty3.DecimalValue
        DRWPenjualanResep("jmlhari") = 0
        DRWPenjualanResep("takaran") = kdTakaran
        DRWPenjualanResep("waktu") = kdWaktu
        DRWPenjualanResep("ketminum") = kdKeterangan
        DRWPenjualanResep("posting") = "1"
        DRWPenjualanResep("diserahkan") = "B"
        DRWPenjualanResep("jns_obat") = JenisObat
        DRWPenjualanResep("jmljatah") = txtJmlHari.IntegerValue
        DRWPenjualanResep("tglakhir") = DTPTglAkhir.Value
        DRWPenjualanResep("jml_awal") = 0
        DRWPenjualanResep("tgl_exp") = DTPTanggalExp.Value
        DRWPenjualanResep("nmobat_etiket") = txtNamaObatEtiket.Text
        DRWPenjualanResep("jmlobat_etiket") = txtJumlahObatEtiket.DecimalValue

        BDPenjualanResep.EndEdit()

        gridDetailObat.DataSource = Nothing
        gridDetailObat.DataSource = BDPenjualanResep

        TotalHarga()
        TotalDijamin()
        TotalIurBayar()
    End Sub

    Sub addBarangKh()
        cariNamaPenjamin()
        cariDokter()
        carikdEtiketTakaran()
        carikdEtiketWaktu()
        carikdEtiketKeterangan()

        BDPenjualanResepKh.DataSource = DSPenjualanResepKh
        BDPenjualanResepKh.DataMember = "PenjualanResepKh"

        BDPenjualanResepKh.AddNew()
        DRWPenjualanResepKh = BDPenjualanResepKh.Current
        DRWPenjualanResepKh("stsrawat") = StatusRawat
        DRWPenjualanResepKh("kdkasir") = Trim(FormLogin.LabelKode.Text)
        DRWPenjualanResepKh("nmkasir") = Trim(FormLogin.LabelNama.Text)
        DRWPenjualanResepKh("tanggal") = DTPTanggalTrans.Value
        DRWPenjualanResepKh("notaresep") = Trim(txtNoResep.Text)
        DRWPenjualanResepKh("no_reg") = Trim(txtNoReg.Text)
        DRWPenjualanResepKh("no_rm") = Trim(txtRM.Text)
        DRWPenjualanResepKh("nmpasien") = Trim(txtNamaPasien.Text)
        DRWPenjualanResepKh("umurthn") = txtUmurThn.Text
        DRWPenjualanResepKh("umurbln") = txtUmurBln.Text
        DRWPenjualanResepKh("kd_penjamin") = KdPenjamin
        DRWPenjualanResepKh("nm_penjamin") = NamaPenjamin
        DRWPenjualanResepKh("kddokter") = kdDokter
        DRWPenjualanResepKh("nmdokter") = NamaDokter
        DRWPenjualanResepKh("nonota") = Trim(txtNota.Text)
        DRWPenjualanResepKh("urut") = 1
        DRWPenjualanResepKh("kd_barang") = Trim(txtKodeObatKh.Text)
        DRWPenjualanResepKh("idx_barang") = Trim(txtIdObatKh.Text)
        DRWPenjualanResepKh("nama_barang") = Trim(lblNamaObatKh.Text)
        DRWPenjualanResepKh("kd_jns_obat") = KdJenisObat
        DRWPenjualanResepKh("kd_gol_obat") = kdGolonganObat
        DRWPenjualanResepKh("kd_kel_obat") = kdKelompokObat
        DRWPenjualanResepKh("kdpabrik") = kdPabrik
        DRWPenjualanResepKh("generik") = Generik
        DRWPenjualanResepKh("formularium") = "FORMULARIUM"
        DRWPenjualanResepKh("racik") = Trim(cmbRacikNonKh.Text)
        DRWPenjualanResepKh("harga") = txtHargaJualKh.DecimalValue
        DRWPenjualanResepKh("jmlp") = txtPaketBPJSKh.DecimalValue
        DRWPenjualanResepKh("totalp") = txtTotalPaketBPJSKh.DecimalValue
        DRWPenjualanResepKh("jmln") = txtPaketLainKh.DecimalValue
        DRWPenjualanResepKh("totaln") = txtTotalPaketLainKh.DecimalValue
        DRWPenjualanResepKh("jml") = txtPaketBPJSKh.DecimalValue + txtPaketLainKh.DecimalValue
        DRWPenjualanResepKh("nmsatuan") = Trim(txtSatPaketBPJSKh.Text)
        DRWPenjualanResepKh("totalharga") = txtTotalPaketBPJSKh.DecimalValue + txtTotalPaketLainKh.DecimalValue
        DRWPenjualanResepKh("senpot") = 0
        DRWPenjualanResepKh("potongan") = 0
        DRWPenjualanResepKh("jmlnet") = txtPaketBPJSKh.DecimalValue
        DRWPenjualanResepKh("dijamin") = txtTotalPaketBPJSKh.DecimalValue
        DRWPenjualanResepKh("sisabayar") = 0
        DRWPenjualanResepKh("hrgbeli") = HargaBeli
        DRWPenjualanResepKh("jamawal") = Format(DTPJamAwal.Value, "HH:mm:ss")
        DRWPenjualanResepKh("kdbagian") = pkdapo
        DRWPenjualanResepKh("stsresep") = "PKTKHUSUS"
        DRWPenjualanResepKh("rek_p") = kdRekening
        DRWPenjualanResepKh("stsetiket") = cmbEtiketKh.Text
        DRWPenjualanResepKh("qty1") = txtSigna1.Text
        DRWPenjualanResepKh("qty2") = txtSigna2.Text
        DRWPenjualanResepKh("qty3") = txtQty3.DecimalValue
        DRWPenjualanResepKh("jmlhari") = 0
        DRWPenjualanResepKh("takaran") = kdTakaran
        DRWPenjualanResepKh("waktu") = kdWaktu
        DRWPenjualanResepKh("ketminum") = kdKeterangan
        DRWPenjualanResepKh("posting") = "1"
        DRWPenjualanResepKh("diserahkan") = "B"
        DRWPenjualanResepKh("jns_obat") = JenisObat
        DRWPenjualanResepKh("jmljatah") = txtJmlHariKh.IntegerValue
        DRWPenjualanResepKh("tglakhir") = DTPTglAkhirKh.Value
        DRWPenjualanResepKh("jml_awal") = 0
        DRWPenjualanResepKh("tgl_exp") = DTPTanggalExp.Value
        DRWPenjualanResepKh("nmobat_etiket") = txtNamaObatEtiket.Text
        DRWPenjualanResepKh("jmlobat_etiket") = txtJumlahObatEtiket.DecimalValue

        BDPenjualanResepKh.EndEdit()

        gridDetailObatKh.DataSource = Nothing
        gridDetailObatKh.DataSource = BDPenjualanResepKh

        TotalPaket()
        TotalNonPaket()
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
            DA = New OleDb.OleDbDataAdapter("select idx_barang, kd_barang, LTRIM(RTRIM(nama_barang)) as nama_barang," & Stok & ", LTRIM(RTRIM(kd_satuan_kecil)) as kd_satuan_kecil, LTRIM(RTRIM(keterangan)) as keterangan from Barang_Farmasi WHERE stsaktif ='1' order by kd_barang", CONN)
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

    Sub tampilPaket()
        CMD = New OleDb.OleDbCommand("SELECT * FROM ap_jualr1 WHERE notaresep='" & Trim(txtNoResep.Text) & "'", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)

        If DT.Rows.Count > 0 Then
            txtNoReg.Text = Trim(DT.Rows(0).Item("no_reg"))
            txtRM.Text = Trim(DT.Rows(0).Item("no_rm"))
            StatusRawat = Trim(DT.Rows(0).Item("stsrawat"))
            txtNamaPasien.Text = Trim(DT.Rows(0).Item("nama_pasien"))
            kdSubUnit = Trim(DT.Rows(0).Item("kd_sub_unit_asal"))
            nmSubUnit = Trim(DT.Rows(0).Item("nama_sub_unit"))
            KdPenjamin = Trim(DT.Rows(0).Item("kd_penjamin"))
            NamaPenjamin = Trim(DT.Rows(0).Item("nm_penjamin"))
            kdDokter = Trim(DT.Rows(0).Item("kddokter"))
            NamaDokter = Trim(DT.Rows(0).Item("nmdokter"))
            Posting = Trim(DT.Rows(0).Item("posting"))
            If NamaPenjamin = "-" Then
                cmbPenjamin.Text = "-|UMUM"
            Else
                cmbPenjamin.Text = NamaPenjamin + "|" + KdPenjamin
            End If
            cmbUnitAsal.Text = nmSubUnit + "|" + kdSubUnit
            cmbDokter.Text = NamaDokter + "|" + kdDokter
            txtJnsRawat.Text = JenisRawat
        End If

        CMD = New OleDb.OleDbCommand("SELECT Pasien.no_RM, Pasien.alamat, Pasien.RT, Pasien.RW, Kelurahan.nama_kelurahan, Kecamatan.nama_kecamatan,Kabupaten.nama_kabupaten, Propinsi.nama_propinsi, pasien.nama_pasien, case pasien.jns_kel when '0' then 'P' else 'L' end as jns_kel, pasien.tgl_lahir FROM Pasien INNER JOIN Kelurahan ON Pasien.kd_kelurahan = Kelurahan.kd_kelurahan INNER JOIN Kecamatan ON Kelurahan.kd_kecamatan = Kecamatan.kd_kecamatan INNER JOIN Kabupaten ON Kecamatan.kd_kabupaten = Kabupaten.kd_kabupaten INNER JOIN Propinsi ON Kabupaten.kd_propinsi = Propinsi.kd_propinsi where Pasien.no_RM='" & txtRM.Text & "'", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
        txtAlamat.Text = DT.Rows(0).Item("alamat") + " RT " + DT.Rows(0).Item("rt") + " RW " + DT.Rows(0).Item("rw") + " Kel : " + DT.Rows(0).Item("nama_kelurahan") + " Kec : " + DT.Rows(0).Item("nama_kecamatan") + " Kab : " + DT.Rows(0).Item("nama_kabupaten") + " Prov : " + DT.Rows(0).Item("nama_propinsi")
        tglLahirPasien = DT.Rows(0).Item("tgl_lahir")
        txtSex.Text = DT.Rows(0).Item("jns_kel")
        TglServer()
        'txtUmurThn.Text = DateDiff(DateInterval.Year, tglLahirPasien, TanggalServer)
        'txtUmurBln.Text = DateDiff(DateInterval.Month, tglLahirPasien, TanggalServer) Mod 12
        txtUmurThn.Text = TanggalServer.Year - tglLahirPasien.Year
        txtUmurBln.Text = TanggalServer.Month - tglLahirPasien.Month
        If Val(txtUmurBln.Text) < 0 Then
            txtUmurThn.Text = Val(txtUmurThn.Text) - 1
            txtUmurBln.Text = 12 + Val(txtUmurBln.Text)
        End If

        If cmbPkt.Text = "Paket Umum" Then
            tampilDetailObatPaketUmum()
        Else
            tampilDetailObatPaketKhusus()
        End If

    End Sub

    Sub AturGriddetailBarangKh()
        With gridDetailObatKh
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "R/N"
            .Columns(2).HeaderText = "Nama Barang"
            .Columns(3).HeaderText = "Harga"
            .Columns(3).DefaultCellStyle.Format = "N2"
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(4).HeaderText = "Jumlah Paket BPJS"
            .Columns(4).DefaultCellStyle.Format = "N2"
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(5).HeaderText = "Total Paket BPJS"
            .Columns(5).DefaultCellStyle.Format = "N2"
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(6).HeaderText = "Jumlah Paket Lain"
            .Columns(6).DefaultCellStyle.Format = "N2"
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(7).HeaderText = "Total Paket Lain"
            .Columns(7).DefaultCellStyle.Format = "N2"
            .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(8).HeaderText = "Jumlah Obat"
            .Columns(8).DefaultCellStyle.Format = "N2"
            .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(9).HeaderText = "Satuan"
            .Columns(10).HeaderText = "Jml Hari"
            .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(0).Width = 40
            .Columns(1).Width = 40
            .Columns(2).Width = 320
            .Columns(3).Width = 100
            .Columns(4).Width = 70
            .Columns(5).Width = 80
            .Columns(6).Width = 70
            .Columns(7).Width = 100
            .Columns(8).Width = 70
            .Columns(9).Width = 100
            .Columns(10).Width = 40
            .Columns(0).Visible = False
            .Columns(11).Visible = False
            .Columns(12).Visible = False
            .Columns(13).Visible = False
            .Columns(14).Visible = False
            .Columns(15).Visible = False
            .Columns(16).Visible = False
            .Columns(17).Visible = False
            .Columns(18).Visible = False
            .Columns(19).Visible = False
            .Columns(20).Visible = False
            .Columns(21).Visible = False
            .Columns(22).Visible = False
            .Columns(23).Visible = False
            .Columns(24).Visible = False
            .Columns(25).Visible = False
            .Columns(26).Visible = False
            .Columns(27).Visible = False
            .Columns(28).Visible = False
            .Columns(29).Visible = False
            .Columns(30).Visible = False
            .Columns(31).Visible = False
            .Columns(32).Visible = False
            .Columns(33).Visible = False
            .Columns(34).Visible = False
            .Columns(35).Visible = False
            .Columns(36).Visible = False
            .Columns(37).Visible = False
            .Columns(38).Visible = False
            .Columns(39).Visible = False
            .Columns(40).Visible = False
            .Columns(41).Visible = False
            .Columns(42).Visible = False
            .Columns(43).Visible = False
            .Columns(44).Visible = False
            .Columns(45).Visible = False
            .Columns(46).Visible = False
            .Columns(47).Visible = False
            .Columns(48).Visible = False
            .Columns(49).Visible = False
            .Columns(50).Visible = False
            .Columns(51).Visible = False
            .Columns(52).Visible = False
            .Columns(53).Visible = False
            .Columns(54).Visible = False
            .Columns(55).Visible = False
            .Columns(56).Visible = False
            .Columns(57).Visible = False
            .Columns(58).Visible = False
            .Columns(59).Visible = False
            .Columns(60).Visible = False
            .BackgroundColor = Color.Azure
            .DefaultCellStyle.SelectionBackColor = Color.LightBlue
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .RowHeadersDefaultCellStyle.BackColor = Color.Black
            .ReadOnly = True
        End With
    End Sub

    Sub tampilDetailObatPaketKhusus()
        Try
            DA = New OleDb.OleDbDataAdapter("select urut, racik, nama_barang, harga, jmlpaket as jmlp, totalpaket as totalp, jmlnonpaket as jmln, totalnonpaket as totaln, jml,  nmsatuan, jmljatah, stsrawat, kdkasir, nmkasir, tanggal, notaresep, no_reg, no_rm, nmpasien, umurthn, umurbln, kd_penjamin, nm_penjamin, kddokter, nmdokter, nonota,  kd_barang, idx_barang,  kd_jns_obat, kd_gol_obat, kd_kel_obat, kdpabrik, generik, formularium,totalharga, senpot, potongan, jmlnet, dijamin, sisabayar, hrgbeli, jamawal, kdbagian, stsresep, stsetiket, 'signa1' as qty1, 'signa2' as qty2, qty3, jmlhari, EtiketTakaran as takaran, EtiketWaktu as waktu, EtiketKetminum as ketminum, rek_p,posting, diserahkan, jns_obat, tglakhir, (jmlpaket + jmlnonpaket) as jml_awal, CAST(REPLACE('2012-08-17', '-', '') AS DATETIME) as tgl_exp, nama_barang as nmobat_etiket, 0 as jmlobat_etiket from ap_jualr2 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "'  and notaresep='" & Trim(txtNoResep.Text) & "'  order by urut", CONN)
            DSPenjualanResepKh = New DataSet

            DA.Fill(DSPenjualanResepKh, "PenjualanResepKh")
            BDPenjualanResepKh.DataSource = DSPenjualanResepKh
            BDPenjualanResepKh.DataMember = "PenjualanResepKh"

            DS = New DataSet '''''''''''''''' Bantu Tambahan
            DA.Fill(DS) ''''''''''''''''''''' Bantu Tambahan
            With gridStokKembali
                .DataSource = Nothing
                .DataSource = DS.Tables(0)
            End With

            DA = New OleDb.OleDbDataAdapter("select * from ap_etiketNew where tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'", CONN)
            DSEtiket = New DataSet
            DA.Fill(DSEtiket, "Etiket")
            BDEtiket.DataSource = DSEtiket
            BDEtiket.DataMember = "Etiket"

            If BDEtiket.Count > 0 Then
                BDEtiket.MoveFirst()
                For i = 1 To BDEtiket.Count
                    DRWEtiket = BDEtiket.Current
                    BDPenjualanResepKh.Filter = "kd_barang = '" & Trim(DRWEtiket.Item("kd_barang").ToString) & "' AND urut = '" & Trim(DRWEtiket.Item("urut")) & "'"
                    DRWPenjualanResepKh = BDPenjualanResepKh.Current
                    DRWPenjualanResepKh("qty1") = DRWEtiket.Item("signa1")
                    DRWPenjualanResepKh("qty2") = DRWEtiket.Item("signa2")
                    DRWPenjualanResepKh("takaran") = DRWEtiket.Item("kd_takaran")
                    DRWPenjualanResepKh("waktu") = DRWEtiket.Item("kd_waktu")
                    DRWPenjualanResepKh("ketminum") = DRWEtiket.Item("kd_ketminum")
                    DRWPenjualanResepKh("tgl_exp") = DRWEtiket.Item("tgl_exp")
                    DRWPenjualanResepKh("nmobat_etiket") = DRWEtiket.Item("nama_barang")
                    DRWPenjualanResepKh("jmlobat_etiket") = DRWEtiket.Item("jml_obat")
                    BDPenjualanResepKh.EndEdit()
                    BDEtiket.MoveNext()
                Next
            End If
            BDPenjualanResepKh.RemoveFilter()
            With gridDetailObatKh
                .DataSource = Nothing
                .DataSource = BDPenjualanResepKh
            End With

            AturGriddetailBarangKh()
            TotalPaket()
            TotalNonPaket()
            txtQtyKh.DecimalValue = gridDetailObatKh.Rows.Count() - 1

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Sub tampilDetailObatPaketUmum()
        Try
            DA = New OleDb.OleDbDataAdapter("select urut, racik, nama_barang, harga, jml, nmsatuan, totalharga, dijamin, sisabayar, jmljatah, stsrawat, kdkasir, nmkasir, tanggal, notaresep, no_reg, no_rm, nmpasien, umurthn, umurbln, kd_penjamin, nm_penjamin, kddokter, nmdokter, nonota,  kd_barang, idx_barang,  kd_jns_obat, kd_gol_obat, kd_kel_obat, kdpabrik, generik, formularium, jmlpaket as jmlp, totalpaket as totalp, jmlnonpaket as jmln, totalnonpaket as totaln, senpot, potongan, jmlnet, dijamin, sisabayar, hrgbeli, jamawal, kdbagian, stsresep, stsetiket, 'signa1' as qty1, 'signa2' as qty2, qty3, jmlhari, EtiketTakaran as takaran, EtiketWaktu as waktu, EtiketKetminum as ketminum, rek_p, posting, diserahkan, jns_obat, tglakhir, jml as jml_awal, CAST(REPLACE('2012-08-17', '-', '') AS DATETIME) as tgl_exp, nama_barang as nmobat_etiket, 0 as jmlobat_etiket, '' as model_etiket, '' as nmobat_etiket_infus, 0 as jmlobat_etiket_infus, '' as obat_infus, '' as tetes_infus from ap_jualr2 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "' order by urut", CONN)
            DSPenjualanResep = New DataSet

            DA.Fill(DSPenjualanResep, "PenjualanResep")
            BDPenjualanResep.DataSource = DSPenjualanResep
            BDPenjualanResep.DataMember = "PenjualanResep"

            DS = New DataSet '''''''''''''''' Bantu Tambahan
            DA.Fill(DS) ''''''''''''''''''''' Bantu Tambahan
            With gridStokKembali
                .DataSource = Nothing
                .DataSource = DS.Tables(0)
            End With

            DA = New OleDb.OleDbDataAdapter("select * from ap_etiketNew where tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'", CONN)
            DSEtiket = New DataSet
            DA.Fill(DSEtiket, "Etiket")
            BDEtiket.DataSource = DSEtiket
            BDEtiket.DataMember = "Etiket"

            If BDEtiket.Count > 0 Then
                BDEtiket.MoveFirst()
                For i = 1 To BDEtiket.Count
                    DRWEtiket = BDEtiket.Current
                    BDPenjualanResep.Filter = "kd_barang = '" & Trim(DRWEtiket.Item("kd_barang").ToString) & "' AND urut = '" & Trim(DRWEtiket.Item("urut")) & "'"
                    DRWPenjualanResep = BDPenjualanResep.Current
                    DRWPenjualanResep("qty1") = DRWEtiket.Item("signa1")
                    DRWPenjualanResep("qty2") = DRWEtiket.Item("signa2")
                    DRWPenjualanResep("takaran") = DRWEtiket.Item("kd_takaran")
                    DRWPenjualanResep("waktu") = DRWEtiket.Item("kd_waktu")
                    DRWPenjualanResep("ketminum") = DRWEtiket.Item("kd_ketminum")
                    DRWPenjualanResep("tgl_exp") = DRWEtiket.Item("tgl_exp")
                    DRWPenjualanResep("nmobat_etiket") = DRWEtiket.Item("nama_barang")
                    DRWPenjualanResep("jmlobat_etiket") = DRWEtiket.Item("jml_obat")
                    DRWPenjualanResep("model_etiket") = DRWEtiket.Item("model")
                    DRWPenjualanResep("nmobat_etiket_infus") = DRWEtiket.Item("nama_barang")
                    DRWPenjualanResep("jmlobat_etiket_infus") = DRWEtiket.Item("jml_obat")
                    DRWPenjualanResep("obat_infus") = DRWEtiket.Item("obat")
                    DRWPenjualanResep("tetes_infus") = DRWEtiket.Item("tetes")
                    BDPenjualanResep.EndEdit()
                    BDEtiket.MoveNext()
                Next
            End If
            BDPenjualanResep.RemoveFilter()
            With gridDetailObat
                .DataSource = Nothing
                .DataSource = BDPenjualanResep
            End With

            AturGriddetailBarang()
            TotalHarga()
            TotalDijamin()
            TotalIurBayar()
            txtQty.DecimalValue = gridDetailObat.Rows.Count() - 1

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Sub TotalHarga()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To gridDetailObat.RowCount - 1
            HitungTotal = HitungTotal + gridDetailObat.Rows(baris).Cells("totalharga").Value
        Next
        txtGrandTotal.DecimalValue = HitungTotal
        txtGrandTotalBulat.DecimalValue = buletin(txtGrandTotal.DecimalValue, 100)
    End Sub

    Sub TotalDijamin()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To gridDetailObat.RowCount - 1
            HitungTotal = HitungTotal + gridDetailObat.Rows(baris).Cells("dijamin").Value
        Next
        txtGrandDijamin.DecimalValue = HitungTotal
        txtGrandDijaminBulat.DecimalValue = buletin(txtGrandDijamin.DecimalValue, 100)
    End Sub

    Sub TotalIurBayar()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To gridDetailObat.RowCount - 1
            HitungTotal = HitungTotal + gridDetailObat.Rows(baris).Cells("sisabayar").Value
        Next
        txtGrandIurBayar.DecimalValue = HitungTotal
        txtGrandIurBayarBulat.DecimalValue = buletin(txtGrandIurBayar.DecimalValue, 100)
    End Sub

    Sub TotalPaket()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To gridDetailObatKh.RowCount - 1
            HitungTotal = HitungTotal + gridDetailObatKh.Rows(baris).Cells("totalp").Value
        Next
        txtGrandTotalPaket.DecimalValue = HitungTotal
        txtGrandTotalPaketBulat.DecimalValue = buletin(txtGrandTotalPaket.DecimalValue, 100)
    End Sub

    Sub TotalNonPaket()
        Dim HitungTotal As Decimal = 0
        For baris As Integer = 0 To gridDetailObatKh.RowCount - 1
            HitungTotal = HitungTotal + gridDetailObatKh.Rows(baris).Cells("totaln").Value
        Next
        txtGrandTotalNonPaket.DecimalValue = HitungTotal
        txtGrandTotalNonPaketBulat.DecimalValue = buletin(txtGrandTotalNonPaket.DecimalValue, 100)
    End Sub

    Sub HargaJual()
        'txtHargaJual.DecimalValue = (txtHargaJual.DecimalValue + (txtHargaJual.DecimalValue * Val(ppn) / 100)) + (txtHargaJual.DecimalValue * Val(laba) / 100)
    End Sub

    Sub HargaJualKh()
        'txtHargaJualKh.DecimalValue = (txtHargaJualKh.DecimalValue + (txtHargaJualKh.DecimalValue * Val(ppn) / 100)) + (txtHargaJualKh.DecimalValue * Val(laba) / 100)
    End Sub

    Sub cekJangkaPemberianObatBPJS(ByVal KodeObat As String)
        CMD = New OleDb.OleDbCommand("SELECT top(1) no_rm, kd_barang, tglakhir FROM ap_jualr2_bpjs WHERE no_rm='" & Trim(txtRM.Text) & "' AND kd_barang='" & KodeObat & "' AND kdbagian='" & pkdapo & "' order by tglakhir desc", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
    End Sub

    Sub NoUrut()
        If BDPenjualanResep.Count > 0 Then
            txtNoUrut.Text = Val(txtNoUrut.Text) + 1
        Else
            txtNoUrut.Text = "1"
        End If
    End Sub

    Sub detailObat(ByVal KodeObat As String)
        CMD = New OleDb.OleDbCommand("SELECT * FROM Barang_Farmasi WHERE kd_barang='" & KodeObat & "'", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
        If DT.Rows.Count > 0 Then
            If cmbPkt.Text = "Paket Umum" Then
                txtIdObat.Text = Trim(DT.Rows(0).Item("idx_barang"))
                lblNamaObat.Text = Trim(DT.Rows(0).Item("nama_barang"))
                HargaBeli = DT.Rows(0).Item("harga_jual")
                txtHargaJual.DecimalValue = DT.Rows(0).Item("harga_jual")
                txtKdSatuan.Text = Trim(DT.Rows(0).Item("kd_satuan_kecil"))
                txtDosis.DecimalValue = DT.Rows(0).Item("dosis")
                txtSatDosis.Text = Trim(DT.Rows(0).Item("satdosis"))
                HargaJual()
                If cmbPenjamin.Text = "-|UMUM" Then
                    cmbDijamin.Text = "N"
                Else
                    cmbDijamin.Text = "Y"
                End If
                If cmbRacikNon.Text = "R" Then
                    txtDosisResep.Focus()
                Else
                    cmbDijamin.Focus()
                End If
            ElseIf cmbPkt.Text = "Paket Khusus" Then
                txtIdObatKh.Text = Trim(DT.Rows(0).Item("idx_barang"))
                lblNamaObatKh.Text = Trim(DT.Rows(0).Item("nama_barang"))
                HargaBeli = DT.Rows(0).Item("harga_jual")
                txtHargaJualKh.DecimalValue = DT.Rows(0).Item("harga_jual")
                txtSatPaketBPJSKh.Text = Trim(DT.Rows(0).Item("kd_satuan_kecil"))
                txtSatPaketLainKh.Text = Trim(DT.Rows(0).Item("kd_satuan_kecil"))
                txtDosisKh.DecimalValue = DT.Rows(0).Item("dosis")
                txtSatDosisKh.Text = Trim(DT.Rows(0).Item("satdosis"))
                HargaJualKh()
                If cmbRacikNonKh.Text = "N" Then
                    txtPaketBPJSKh.Focus()
                Else
                    txtDosisResepKh.Focus()
                End If
            End If

            Generik = Trim(DT.Rows(0).Item("generik"))
            KdJenisObat = Trim(DT.Rows(0).Item("kd_jns_obat"))
            kdPabrik = Trim(DT.Rows(0).Item("kdpabrik"))
            kdKelompokObat = Trim(DT.Rows(0).Item("kd_kel_obat"))
            kdGolonganObat = Trim(DT.Rows(0).Item("kd_gol_obat"))
            txtSenPotBeli.DecimalValue = DT.Rows(0).Item("senpotbeli")
        End If
        CMD = New OleDb.OleDbCommand("SELECT * FROM jenis_obat WHERE kd_jns_obat='" & Trim(KdJenisObat) & "'", CONN)
        DA = New OleDb.OleDbDataAdapter(CMD)
        DT = New DataTable
        DA.Fill(DT)
        If DT.Rows.Count > 0 Then
            JenisObat = Trim(DT.Rows(0).Item("jns_obat"))
            kdRekening = Trim(DT.Rows(0).Item("rek_p"))
        End If
    End Sub

    Private Sub txtNoResep_Click(sender As Object, e As EventArgs) Handles txtNoResep.Click
        DTPBantu.Value = DateAdd("m", 1, DTPTanggalTrans.Value)
        Bulan = Month(DTPBantu.Value)
        Tahun = Year(DTPBantu.Value)
        'cekTutupStok()
        'If DR.HasRows Then
        '    DTPTanggalTrans.Focus()
        '    MsgBox("Tidak bisa melakukan transaksi!!! " & vbCrLf & "Bulan dan tahun tersebut sudah tutup stok", vbInformation, "Informasi")
        '    Exit Sub
        'Else
        tampilPasien()
        PanelPasien.Visible = True
        txtCariPasien.Clear()
        txtCariPasien.Focus()
        'End If
    End Sub

    Private Sub txtNoResep_GotFocus(sender As Object, e As EventArgs) Handles txtNoResep.GotFocus
        DTPBantu.Value = DateAdd("m", 1, DTPTanggalTrans.Value)
        Bulan = Month(DTPBantu.Value)
        Tahun = Year(DTPBantu.Value)
        'cekTutupStok()
        'If DR.HasRows Then
        '    DTPTanggalTrans.Focus()
        '    MsgBox("Tidak bisa melakukan transaksi!!! " & vbCrLf & "Bulan dan tahun tersebut sudah tutup stok", vbInformation, "Informasi")
        '    Exit Sub
        'Else
        tampilPasien()
        PanelPasien.Visible = True
        txtCariPasien.Clear()
        txtCariPasien.Focus()
        'End If
    End Sub

    Private Sub FormEditPenjualanResep_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F12 Then
            If cmbPkt.Text = "Paket Umum" Then
                btnSimpan.PerformClick()
            ElseIf cmbPkt.Text = "Paket Khusus" Then
                btnSimpanKh.PerformClick()
            End If
        ElseIf e.KeyCode = Keys.F1 Then
            btnCetakNota.PerformClick()
        ElseIf e.KeyCode = Keys.F10 Then
            If cmbPkt.Text = "Paket Umum" Then
                btnBaru.PerformClick()
            ElseIf cmbPkt.Text = "Paket Khusus" Then
                btnBaruKh.PerformClick()
            End If
        ElseIf e.KeyCode = Keys.F2 Then
            btnCetakBPJS.PerformClick()
        ElseIf e.KeyCode = Keys.F3 Then
            btnCetakLain.PerformClick()
        End If
    End Sub

    Private Sub FormEditPenjualanResep_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setApo()
        Me.KeyPreview = True
        FormPemanggil = "FormEditPenjualanResep"
        cmbJenisRawat.SelectedIndex = 0
        ListDokter()
        ListEtiketTakaran()
        ListEtiketWaktu()
        ListEtiketKeterangan()
        KosongkanHeader()
        KosongkanDetailPaketUmum()
        KosongkanDetailPaketKhusus()
        cmbJenisRawat.Focus()
    End Sub

    Private Sub FormEditPenjualanResep_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelPasien.Top = txtNoResep.Top + 61
        PanelPasien.Left = txtNoResep.Left
        PanelObat.Top = txtKodeObat.Top + 218
        PanelObat.Left = txtKodeObat.Left + 4
    End Sub

    Private Sub btnEx_Click(sender As Object, e As EventArgs) Handles btnEx.Click
        PanelPasien.Visible = False
    End Sub

    Private Sub cmbJenisRawat_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbJenisRawat.KeyPress
        If e.KeyChar = Chr(13) Then
            DTPTanggalTrans.Focus()
        End If
    End Sub

    Private Sub cmbJenisRawat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbJenisRawat.SelectedIndexChanged
        If cmbJenisRawat.SelectedIndex = 0 Then
            StatusRawat = "RJ"
            JenisRawat = "1"
            cmbPkt.SelectedIndex = 0
            cmbPkt.Enabled = True
        ElseIf cmbJenisRawat.SelectedIndex = 1 Then
            StatusRawat = "RI"
            JenisRawat = "2"
            cmbPkt.SelectedIndex = 0
            cmbPkt.Enabled = False
        ElseIf cmbJenisRawat.SelectedIndex = 2 Then
            StatusRawat = "RD"
            JenisRawat = "3"
            cmbPkt.SelectedIndex = 0
            cmbPkt.Enabled = True
        Else
            MsgBox("Coba Lagi")
        End If
        DTPTanggalTrans.Focus()
    End Sub

    Private Sub txtCariPasien_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCariPasien.KeyDown
        If e.KeyCode = Keys.Down Then
            gridPasien.Focus()
        End If
    End Sub

    Private Sub txtCariPasien_TextChanged(sender As Object, e As EventArgs) Handles txtCariPasien.TextChanged
        If rRm.Checked = True Then
            BDDataPasien.Filter = "no_RM like '%" & txtCariPasien.Text & "%'"
        Else
            BDDataPasien.Filter = "nama_pasien like '%" & txtCariPasien.Text & "%'"
        End If
    End Sub

    Private Sub DTPTanggalTrans_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DTPTanggalTrans.KeyPress
        If e.KeyChar = Chr(13) Then
            txtNoResep.Focus()
        End If
    End Sub

    Private Sub gridPasien_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridPasien.CellContentClick
        Dim Paket As String
        If e.ColumnIndex = 0 Then
            If Not IsDBNull(gridPasien.Rows(e.RowIndex).Cells(1).Value) Then
                txtNoResep.Text = Trim(gridPasien.Rows(e.RowIndex).Cells(4).Value)
                kdUnitDepo = Trim(gridPasien.Rows(e.RowIndex).Cells("kd_sub_unit").Value)
                kdApo = Trim(gridPasien.Rows(e.RowIndex).Cells("kdbagian").Value)
                Paket = Trim(gridPasien.Rows(e.RowIndex).Cells(8).Value)
                If Paket = "PKTUMUM" Then
                    cmbPkt.SelectedIndex = 0
                Else
                    cmbPkt.SelectedIndex = 1
                    'MsgBox("Tidak bisa membuka Paket Khusus", vbInformation, "Informasi")
                    'Exit Sub
                End If
                PanelPasien.Visible = False
                tampilPaket()
                If cmbPenjamin.Text = "-|UMUM" Then
                    cmbDijamin.Text = "N"
                Else
                    cmbDijamin.Text = "Y"
                End If
            End If
            btnInfoResepKh.Enabled = True
            btnBaru.Enabled = True
            btnSimpan.Enabled = True
            btnBaruKh.Enabled = True
            btnSimpanKh.Enabled = True
            btnCetakBPJS.Enabled = True
            btnCetakLain.Enabled = True
            btnCetakEtiketKh.Enabled = True
            btnCetakNota.Enabled = True
            btnHapusNotaKh.Enabled = True
            btnUpdateDijamin.Enabled = True
            btnUpdateIurPasien.Enabled = True
        End If
    End Sub

    Private Sub cmbPkt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPkt.SelectedIndexChanged
        If cmbPkt.SelectedIndex = 0 Then
            TabPktUmum.TabVisible = True
            TabPktKhusus.TabVisible = False
            cmbRacikNon.Focus()
        ElseIf cmbPkt.SelectedIndex = 1 Then
            TabPktUmum.TabVisible = False
            TabPktKhusus.TabVisible = True
            cmbRacikNonKh.Focus()
        Else
            TabPktUmum.TabVisible = False
            TabPktKhusus.TabVisible = False
        End If
    End Sub

    Private Sub btnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        Dispose()
    End Sub

    Private Sub btnInfoResep_Click(sender As Object, e As EventArgs)
        FormPemanggil = "FormEditPenjualanResep"
        If txtNoReg.Text = "" Then
            MsgBox("Pilih pasien terlebih dahulu")
            txtNoResep.Focus()
        Else
            FormInfoResepObat.ShowDialog()
        End If
    End Sub

    Private Sub gridPasien_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridPasien.KeyPress
        Dim Paket As String
        If e.KeyChar = Chr(13) Then
            Dim i = gridPasien.CurrentRow.Index - 1
            If Not IsDBNull(gridPasien.Rows(i).Cells(1).Value) Then
                txtNoResep.Text = Trim(gridPasien.Rows(i).Cells(4).Value)
                kdUnitDepo = Trim(gridPasien.Rows(i).Cells("kd_sub_unit").Value)
                kdApo = Trim(gridPasien.Rows(i).Cells("kdbagian").Value)
                Paket = Trim(gridPasien.Rows(i).Cells(8).Value)
                If Paket = "PKTUMUM" Then
                    cmbPkt.SelectedIndex = 0
                Else
                    MsgBox("Tidak bisa membuka Paket Khusus", vbInformation, "Informasi")
                    Exit Sub
                End If
                PanelPasien.Visible = False
                tampilPaket()
            End If
            btnInfoResepKh.Enabled = True
            btnBaru.Enabled = True
            btnSimpan.Enabled = True
            btnBaruKh.Enabled = True
            btnSimpanKh.Enabled = True
            btnCetakBPJS.Enabled = True
            btnCetakLain.Enabled = True
            btnCetakEtiketKh.Enabled = True
            btnCetakNota.Enabled = True
            btnHapusNotaKh.Enabled = True
            btnUpdateDijamin.Enabled = True
            btnUpdateIurPasien.Enabled = True
        End If
    End Sub

    Private Sub gridDetailObat_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles gridDetailObat.CellEndEdit
        'gridDetailObat.Rows(e.RowIndex).Cells("totalharga").Value = gridDetailObat.Rows(e.RowIndex).Cells("harga").Value * gridDetailObat.Rows(e.RowIndex).Cells("jml").Value
        ''jmlobat_etiket
        'gridDetailObat.Rows(e.RowIndex).Cells("jmlobat_etiket").Value = gridDetailObat.Rows(e.RowIndex).Cells("jml").Value
        'If cmbPenjamin.Text = "-|UMUM" Then
        '    gridDetailObat.Rows(e.RowIndex).Cells(8).Value = gridDetailObat.Rows(e.RowIndex).Cells(6).Value
        'Else
        '    gridDetailObat.Rows(e.RowIndex).Cells(7).Value = gridDetailObat.Rows(e.RowIndex).Cells(6).Value
        'End If
        'TotalHarga()
        'TotalDijamin()
        'TotalIurBayar()
    End Sub

    Private Sub gridDetailObat_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles gridDetailObat.CellFormatting
        'gridDetailObat.Rows(e.RowIndex).HeaderCell.Value = CStr(e.RowIndex + 1)
    End Sub

    Private Sub txtKodeObat_Click(sender As Object, e As EventArgs) Handles txtKodeObat.Click
        tampilBarang()
        PanelObat.Visible = True
        txtCariObat.Clear()
        txtCariObat.Focus()
    End Sub

    Private Sub txtKodeObat_GotFocus(sender As Object, e As EventArgs) Handles txtKodeObat.GotFocus
        tampilBarang()
        PanelObat.Visible = True
        txtCariObat.Clear()
        txtCariObat.Focus()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PanelObat.Visible = False
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtNoReg.Text = "" Then
            MsgBox("Pilih pasien terlebih dahulu")
            Exit Sub
        End If
        If txtKodeObat.Text = "" Then
            MsgBox("Obat belum dipilih")
            Exit Sub
        End If
        If txtJumlahJual.DecimalValue <= 0 Then
            MsgBox("Jumlah belum diisi")
            txtJumlahJual.Focus()
        Else
            'For barisGrid As Integer = 0 To gridDetailObat.RowCount - 1
            '    If Trim(txtKodeObat.Text) = gridDetailObat.Rows(barisGrid).Cells("kd_barang").Value Then
            '        MsgBox("Obat ini sudah dientry")
            '        KosongkanDetailPaketUmum()
            '        txtKodeObat.Focus()
            '        Exit Sub
            '    End If
            'Next
            addBarang()
            AturGriddetailBarang()
            NoUrut()
            KosongkanDetailPaketUmum()
            btnSimpan.Enabled = True
            txtQty.DecimalValue = gridDetailObat.Rows.Count() - 1
            cmbRacikNon.Focus()
        End If
    End Sub

    Private Sub gridBarang_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridBarang.CellContentClick
        If e.ColumnIndex = 0 Then
            If Not IsDBNull(gridBarang.Rows(e.RowIndex).Cells(1).Value) Then
                If cmbPkt.Text = "Paket Umum" Then
                    txtKodeObat.Text = gridBarang.Rows(e.RowIndex).Cells(2).Value
                    PanelObat.Visible = False
                    detailObat(Trim(txtKodeObat.Text))
                ElseIf cmbPkt.Text = "Paket Khusus" Then
                    txtKodeObatKh.Text = gridBarang.Rows(e.RowIndex).Cells(2).Value
                    PanelObat.Visible = False
                    detailObat(Trim(txtKodeObatKh.Text))
                End If
            End If
        End If
    End Sub

    Private Sub gridBarang_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridBarang.KeyPress
        If e.KeyChar = Chr(13) Then
            Dim i = gridBarang.CurrentRow.Index - 1
            If Not IsDBNull(gridBarang.Rows(i).Cells(1).Value) Then
                If cmbPkt.Text = "Paket Umum" Then
                    txtKodeObat.Text = gridBarang.Rows(i).Cells(2).Value
                    PanelObat.Visible = False
                    detailObat(Trim(txtKodeObat.Text))
                ElseIf cmbPkt.Text = "Paket Khusus" Then
                    txtKodeObatKh.Text = gridBarang.Rows(i).Cells(2).Value
                    PanelObat.Visible = False
                    detailObat(Trim(txtKodeObatKh.Text))
                End If
            End If
        End If
    End Sub

    Private Sub cmbDijamin_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbDijamin.KeyDown
        If e.KeyCode = Keys.Left Then
            txtKodeObat.Focus()
        End If
    End Sub


    Private Sub cmbDijamin_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbDijamin.KeyPress
        If e.KeyChar = Chr(13) Then
            If cmbDijamin.Text = "Y" Or cmbDijamin.Text = "y" Or cmbDijamin.Text = "N" Or cmbDijamin.Text = "n" Then
                SendKeys.Send("{TAB}")
            Else
                MsgBox("Pilih yang benar", vbCritical, "Kesalahan")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbDijamin_LostFocus(sender As Object, e As EventArgs) Handles cmbDijamin.LostFocus
        cmbDijamin.Text = (cmbDijamin.Text.ToUpper)
    End Sub

    Private Sub cmbDijamin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDijamin.SelectedIndexChanged
        If cmbDijamin.Text = "Y" Then
            txtDijamin.Enabled = False
        Else
            txtDijamin.Enabled = True
        End If
    End Sub

    Private Sub txtJumlahJual_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJumlahJual.KeyDown
        If e.KeyCode = Keys.Up Then
            cmbDijamin.Focus()
        End If
    End Sub

    Private Sub txtJumlahJual_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJumlahJual.KeyPress
        If e.KeyChar = Chr(13) Then
            If cmbDijamin.Text = "Y" Then
                SendKeys.Send("{TAB}")
            Else
                txtDijamin.Focus()
            End If
        End If
    End Sub

    Private Sub txtJumlahJual_TextChanged(sender As Object, e As EventArgs) Handles txtJumlahJual.TextChanged
        txtJumlahHarga.DecimalValue = txtHargaJual.DecimalValue * txtJumlahJual.DecimalValue
        If cmbDijamin.Text = "N" Then
            txtDijamin.DecimalValue = 0
            txtIuranSisaBayar.DecimalValue = txtJumlahHarga.DecimalValue
        ElseIf cmbDijamin.Text = "Y" Then
            txtIuranSisaBayar.DecimalValue = 0
            txtDijamin.DecimalValue = txtJumlahHarga.DecimalValue
        End If
    End Sub

    Private Sub txtJmlHari_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJmlHari.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmbEtiket.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            txtJumlahJual.Focus()
        End If
    End Sub

    Private Sub txtJmlHari_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJmlHari.KeyPress

    End Sub

    Private Sub txtJmlHari_TextChanged(sender As Object, e As EventArgs) Handles txtJmlHari.TextChanged
        TglServer()
        DTPTglAkhir.Value = DateAdd("d", Val(txtJmlHari.Text), DTPTanggalTrans.Value)
    End Sub

    Private Sub cmbEtiket_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbEtiket.KeyDown
        If e.KeyCode = Keys.Left Then
            txtJmlHari.Focus()
        End If
    End Sub

    Private Sub cmbEtiket_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbEtiket.KeyPress
        If e.KeyChar = Chr(13) Then
            If cmbEtiket.Text = "Y" Or cmbEtiket.Text = "y" Or cmbEtiket.Text = "N" Or cmbEtiket.Text = "n" Then
                If cmbEtiket.Text = "N" Then
                    PanelEtiket.Visible = False
                    SendKeys.Send("{TAB}")
                Else
                    PanelEtiket.Visible = True
                    txtNamaObatEtiket.Text = lblNamaObat.Text
                    txtJumlahObatEtiket.DecimalValue = txtJumlahJual.DecimalValue
                    txtNamaObatEtiket.Focus()
                End If
            Else
                MsgBox("Pilih yang benar", vbCritical, "Kesalahan")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbEtiket_LostFocus(sender As Object, e As EventArgs) Handles cmbEtiket.LostFocus
        cmbEtiket.Text = (cmbEtiket.Text.ToUpper)
        nmPaket = "PKTUMUM"
        If cmbEtiket.Text = "Y" Then
            PanelEtiket.Visible = True
            txtNamaObatEtiket.Focus()
        Else
            PanelEtiket.Visible = False
        End If
    End Sub

    Private Sub cmbEtiket_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEtiket.SelectedIndexChanged
        If cmbEtiket.Text = "Y" Then
            PanelEtiket.Visible = True
            txtNamaObatEtiket.Text = lblNamaObat.Text
            txtJumlahObatEtiket.DecimalValue = txtJumlahJual.DecimalValue
            txtNamaObatEtiket.Focus()
        Else
            PanelEtiket.Visible = False
        End If
    End Sub

    Private Sub cmbRacikNon_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbRacikNon.KeyPress
        If e.KeyChar = Chr(13) Then
            If cmbRacikNon.Text = "R" Or cmbRacikNon.Text = "r" Or cmbRacikNon.Text = "N" Or cmbRacikNon.Text = "n" Then
                SendKeys.Send("{TAB}")
            Else
                MsgBox("Pilih yang benar", vbCritical, "Kesalahan")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbRacikNon_LostFocus(sender As Object, e As EventArgs) Handles cmbRacikNon.LostFocus
        cmbRacikNon.Text = (cmbRacikNon.Text.ToUpper)
    End Sub

    Private Sub cmbRacikNon_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbRacikNon.SelectedIndexChanged
        If cmbRacikNon.Text = "R" Then
            txtDosisResep.Enabled = True
            txtJmlBungkus.Enabled = True
        Else
            txtDosisResep.Enabled = False
            txtJmlBungkus.Enabled = False
        End If
    End Sub

    Private Sub txtDosisResep_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDosisResep.KeyDown
        If e.KeyCode = Keys.Up Then
            txtKodeObat.Focus()
        End If
    End Sub

    Private Sub txtDosisResep_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDosisResep.KeyPress
        If e.KeyChar = Chr(13) Then
            txtJmlBungkus.Focus()
        End If
    End Sub

    Private Sub txtDosisResep_TextChanged(sender As Object, e As EventArgs) Handles txtDosisResep.TextChanged

    End Sub

    Private Sub txtJmlBungkus_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJmlBungkus.KeyDown
        If e.KeyCode = Keys.Up Then
            txtDosisResep.Focus()
        End If
    End Sub

    Private Sub txtJmlBungkus_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJmlBungkus.KeyPress
        If e.KeyChar = Chr(13) Then
            cmbDijamin.Focus()
        End If
    End Sub

    Private Sub txtJmlBungkus_TextChanged(sender As Object, e As EventArgs) Handles txtJmlBungkus.TextChanged

    End Sub

    Private Sub txtHapusBaris_Click(sender As Object, e As EventArgs) Handles txtHapusBaris.Click
        If MessageBox.Show("Yakin Akan Dihapus?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Try
                If gridDetailObat.CurrentRow.Index <> gridDetailObat.NewRowIndex Then
                    gridDetailObat.Rows.RemoveAt(gridDetailObat.CurrentRow.Index)
                End If
                txtQty.DecimalValue = gridDetailObat.Rows.Count() - 1
                TotalHarga()
                TotalDijamin()
                TotalIurBayar()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        If Posting = "2" Then
            MsgBox("Transaksi tidak bisa diedit, sudah diposting oleh kasir", vbInformation, "Informasi")
            Exit Sub
        End If
        cariDokter()
        cariSubUnitAsal()
        If pkdapo = "001" Then
            memStok = "stok001"
        ElseIf pkdapo = "002" Then
            memStok = "stok002"
        ElseIf pkdapo = "003" Then
            memStok = "stok003"
        ElseIf pkdapo = "004" Then
            memStok = "stok004"
        ElseIf pkdapo = "005" Then
            memStok = "stok005"
        ElseIf pkdapo = "006" Then
            memStok = "stok006"
        Else
            MsgBox("Setting apotik belum benar, silahkan hubungi administrator")
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Cek Stok
        'If CekKunciStokPenjualan = "Y" Then
        '    For i = 0 To gridDetailObat.RowCount - 2
        '        konek()
        '        CMD = New OleDb.OleDbCommand("select idx_barang,kd_barang,nama_barang, " & memStok & " as stok, kd_satuan_kecil from barang_farmasi where idx_barang='" & gridDetailObat.Rows(i).Cells("idx_barang").Value & "'", CONN)
        '        DR = CMD.ExecuteReader
        '        DR.Read()
        '        If DR.HasRows Then
        '            If (DR.Item("stok") + gridDetailObat.Rows(i).Cells("jml_awal").Value) < gridDetailObat.Rows(i).Cells("jml").Value Then
        '                MsgBox("Stok " + Trim(DR.Item("nama_barang")) + " hanya " + (DR.Item("stok") + gridDetailObat.Rows(i).Cells("jml_awal").Value).ToString + " masukan ulang jumlah barang", vbInformation, "Informasi")
        '                Exit Sub
        '            End If
        '        End If
        '    Next
        'End If

        If MessageBox.Show("Data Penjualan sudah benar ...?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim sqlEditPenjualanObat As String = ""
            TglServer()
            DTPJamAkhir.Value = TanggalServer
            Trans = CONN.BeginTransaction(IsolationLevel.ReadCommitted)
            CMD.Connection = CONN
            CMD.Transaction = Trans
            Try
                ''''''''''''''''''''''''''''''''''''''HAPUS TRANS APOTEK'''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr1
                sqlEditPenjualanObat = "Delete from ap_jualr1 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr2
                sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "Delete from ap_jualr2 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr2_bpjs
                sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "Delete from ap_jualr2_bpjs WHERE tglresep='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_etiketNew
                sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "Delete from ap_etiketNew WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                ''''''''''''''''''''''''''''''''''''''HAPUS TRANS KASIR'''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus resep_jual
                sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "Delete from resep_jual WHERE no_nota='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus resep_jual_detail
                sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "Delete from resep_jual_detail WHERE no_nota='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''Stok Kembali Asal'''''''''''''''''''''''''''''''''''''''''''''
                If psts_stok = "1" Then
                    For i = 0 To gridStokKembali.RowCount - 2
                        'konek()
                        sqlEditPenjualanObat = sqlEditPenjualanObat + vbCrLf + "UPDATE barang_farmasi SET " & memStok & "=" & memStok & "+" & Num_En_US(gridStokKembali.Rows(i).Cells("jml").Value) & " WHERE kd_barang='" & Trim(gridStokKembali.Rows(i).Cells("kd_barang").Value) & "'"
                        'CMD.ExecuteNonQuery()
                    Next
                End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''TRANS KE APOTEK''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Simpan ap_jualr1
                sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "insert into ap_jualr1 (stsrawat, kdkasir, nmkasir, tanggal, notaresep, no_reg, no_rm, nama_pasien, kd_penjamin, nm_penjamin, kddokter, nmdokter, kdbagian, stsresep, totalpaket, totalpaket_bulat, totalnonpaket, totalnonpaket_bulat, totaldijamin, totaldijamin_bulat, totalselisih_bayar, totalselisih_bayar_bulat, kd_sub_unit, kd_sub_unit_asal, nama_sub_unit, jam, rsp_pulang, posting, diserahkan) values ('" & StatusRawat & "', '" & Trim(FormLogin.LabelKode.Text) & "', '" & Trim(FormLogin.LabelNama.Text) & "', '" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtNoReg.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(txtNamaPasien.Text) & "', '" & Trim(KdPenjamin) & "', '" & Trim(NamaPenjamin) & "', '" & Trim(kdDokter) & "', '" & Trim(NamaDokter) & "', '" & kdApo & "', 'PKTUMUM', '" & Num_En_US(txtGrandTotal.DecimalValue) & "', '" & Num_En_US(txtGrandTotalBulat.DecimalValue) & "', '0', '0', '" & Num_En_US(txtGrandDijamin.DecimalValue) & "', '" & Num_En_US(txtGrandDijaminBulat.DecimalValue) & "', '" & Num_En_US(txtGrandIurBayar.DecimalValue) & "', '" & Num_En_US(txtGrandIurBayarBulat.DecimalValue) & "', '" & kdSubUnit & "', '" & kdSubUnit & "', '" & nmSubUnit & "', '" & Format(DTPJamAkhir.Value, "HH:mm:ss") & "', 'N', '1', 'B')"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Simpan ap_jualr2
                For i = 0 To gridDetailObat.RowCount - 2
                    sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "INSERT INTO ap_jualr2(stsrawat, kdkasir, nmkasir, tanggal, notaresep, no_reg, no_rm, nmpasien, umurthn, umurbln, kd_penjamin, nm_penjamin, kddokter, nmdokter, nonota, urut, kd_barang, idx_barang, nama_barang, kd_jns_obat, kd_gol_obat, kd_kel_obat, kdpabrik, generik, formularium, racik, harga, jmlpaket, totalpaket, jmlnonpaket, totalnonpaket, jml, nmsatuan, totalharga, senpot, potongan, jmlnet, dijamin, sisabayar, hrgbeli, jamawal, kdbagian, stsresep, rek_p, stsetiket, jmlhari, posting, diserahkan, jam, rsp_pulang, jns_obat, jmljatah, tglakhir)  VALUES ('" & StatusRawat & "', '" & Trim(FormLogin.LabelKode.Text) & "', '" & Trim(FormLogin.LabelNama.Text) & "', '" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtNoReg.Text) & "', '" & Trim(txtRM.Text) & "','" & Trim(txtNamaPasien.Text) & "','" & Trim(txtUmurThn.Text) & "', '" & Trim(txtUmurBln.Text) & "', '" & KdPenjamin & "', '" & NamaPenjamin & "', '" & kdDokter & "', '" & NamaDokter & "', '" & Trim(txtNota.Text) & "', " & i + 1 & ", '" & Trim(gridDetailObat.Rows(i).Cells("kd_barang").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("idx_barang").Value) & "', '" & Rep(Trim(gridDetailObat.Rows(i).Cells("nama_barang").Value)) & "','" & Trim(gridDetailObat.Rows(i).Cells("kd_jns_obat").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("kd_gol_obat").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("kd_kel_obat").Value) & "','" & Trim(gridDetailObat.Rows(i).Cells("kdpabrik").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("generik").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("formularium").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("racik").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("harga").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("jml").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("totalharga").Value) & "','" & Num_En_US(gridDetailObat.Rows(i).Cells("jmln").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("totaln").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("jml").Value) & "',  '" & Trim(gridDetailObat.Rows(i).Cells("nmsatuan").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("totalharga").Value) & "', '" & gridDetailObat.Rows(i).Cells("senpot").Value & "','" & Num_En_US(gridDetailObat.Rows(i).Cells("potongan").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("totalharga").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("dijamin").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("sisabayar").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("hrgbeli").Value) & "', '" & Format(DTPJamAwal.Value, "HH:mm:ss") & "', '" & kdApo & "', '" & Trim(gridDetailObat.Rows(i).Cells("stsresep").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("rek_p").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("stsetiket").Value) & "', '" & gridDetailObat.Rows(i).Cells("jmlhari").Value & "', '" & Trim(gridDetailObat.Rows(i).Cells("posting").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("diserahkan").Value) & "', '" & Format(DTPJamAkhir.Value, "HH:mm:ss") & "', 'N', '" & Trim(gridDetailObat.Rows(i).Cells("jns_obat").Value) & "', '" & gridDetailObat.Rows(i).Cells("jmljatah").Value & "', '" & Format(gridDetailObat.Rows(i).Cells("tglakhir").Value, "yyyy/MM/dd") & "')"
                    'CMD.ExecuteNonQuery()
                Next
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Cek Jatah Paket
                For i = 0 To gridDetailObat.RowCount - 2
                    If gridDetailObat.Rows(i).Cells("jmljatah").Value > 0 Then
                        sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "INSERT INTO ap_jualr2_bpjs(stsrawat,tglresep,notaresep,no_rm,kd_penjamin,kd_barang,nama_barang,jmlpaket,jmlnonpaket,jmljatah,tglakhir,kdbagian) VALUES ('" & StatusRawat & "', '" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtRM.Text) & "', '" & KdPenjamin & "', '" & Trim(gridDetailObat.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObat.Rows(i).Cells("nama_barang").Value)) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("jml").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("jmln").Value) & "', '" & gridDetailObat.Rows(i).Cells("jmljatah").Value & "', '" & Format(gridDetailObat.Rows(i).Cells("tglakhir").Value, "yyyy/MM/dd") & "',  '" & kdApo & "')"
                        'CMD.ExecuteNonQuery()
                    End If
                Next
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Simpan etiketNew
                For i = 0 To gridDetailObat.RowCount - 2
                    'Dim a = gridDetailObat.CurrentRow.Index - 1
                    If gridDetailObat.Rows(i).Cells("stsetiket").Value = "Y" And gridDetailObat.Rows(i).Cells("model_etiket").Value = "1" Then
                        sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "insert into ap_etiketNew(tanggal, notaresep, no_rm, kd_barang, nama_barang, kd_takaran, kd_waktu, kd_ketminum, signa1, signa2, tgl_exp, jml_obat, urut, model) values ('" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(gridDetailObat.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObat.Rows(i).Cells("nmobat_etiket").Value)) & "', '" & gridDetailObat.Rows(i).Cells("takaran").Value & "', '" & gridDetailObat.Rows(i).Cells("waktu").Value & "', '" & gridDetailObat.Rows(i).Cells("ketminum").Value & "','" & Trim(gridDetailObat.Rows(i).Cells("qty1").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("qty2").Value) & "',  '" & Format(gridDetailObat.Rows(i).Cells("tgl_exp").Value, "yyyy/MM/dd") & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("jmlobat_etiket").Value) & "', " & i + 1 & ",'1')"
                    ElseIf gridDetailObat.Rows(i).Cells("stsetiket").Value = "Y" And gridDetailObat.Rows(i).Cells("model_etiket").Value = "2" Then
                        sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "insert into ap_etiketNew(tanggal, notaresep, no_rm, kd_barang, nama_barang, jml_obat, urut, obat, tetes, model) values ('" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(gridDetailObat.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObat.Rows(i).Cells("nmobat_etiket_infus").Value)) & "',  '" & Num_En_US(gridDetailObat.Rows(i).Cells("jmlobat_etiket_infus").Value) & "', " & i + 1 & ", '" & Rep(Trim(gridDetailObat.Rows(i).Cells("obat_infus").Value)) & "', '" & Rep(Trim(gridDetailObat.Rows(i).Cells("tetes_infus").Value)) & "','2')"
                    End If
                Next
                'For i = 0 To gridDetailObat.RowCount - 2
                '    'Dim a = gridDetailObat.CurrentRow.Index - 1
                '    If gridDetailObat.Rows(i).Cells("stsetiket").Value = "Y" Then
                '        sqlEditPenjualanObat = sqlEditPenjualanObat + vbCrLf + "insert into ap_etiketNew(tanggal, notaresep, no_rm, kd_barang, nama_barang, kd_takaran, kd_waktu, kd_ketminum, signa1, signa2, tgl_exp, jml_obat, urut) values ('" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(gridDetailObat.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObat.Rows(i).Cells("nmobat_etiket").Value)) & "', '" & gridDetailObat.Rows(i).Cells("takaran").Value & "', '" & gridDetailObat.Rows(i).Cells("waktu").Value & "', '" & gridDetailObat.Rows(i).Cells("ketminum").Value & "','" & Trim(gridDetailObat.Rows(i).Cells("qty1").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("qty2").Value) & "',  '" & Format(gridDetailObat.Rows(i).Cells("tgl_exp").Value, "yyyy/MM/dd") & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("jmlobat_etiket").Value) & "', " & i + 1 & ")"
                '    End If
                'Next
                ''CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''TRANS KE KASIR''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Simpan resep_jual
                sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "insert into resep_jual(no_nota, no_rm, no_reg, jenis_rawat, tgl_jual, waktu, kd_dokter, kd_sub_unit, status_bayar, kd_kelompok_pelanggan, metode_bayar, total, user_id, user_nama, waktu_in, waktu_out, kd_sub_unit_asal, total_bulat, total_non_paket, total_non_paket_bulat, total_tunai, total_tunai_bulat, total_piutang, total_piutang_bulat) values ('" & Trim(txtNoResep.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(txtNoReg.Text) & "',  '" & StatusRawat & "', '" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Format(DTPJamAkhir.Value, "HH:mm:ss").ToString & "', '" & kdDokter & "', '" & kdUnitDepo & "', 'BELUM', '0', 'KREDIT', '" & Num_En_US(txtGrandTotal.DecimalValue) & "', '" & Trim(FormLogin.LabelKode.Text) & "', '" & Trim(FormLogin.LabelNama.Text) & "', '" & Format(DTPJamAkhir.Value, "HH:mm:ss") & "', '-', '" & kdSubUnit & "', '" & Num_En_US(txtGrandTotalBulat.DecimalValue) & "', '0', '0','" & Num_En_US(txtGrandIurBayar.DecimalValue) & "', '" & Num_En_US(txtGrandIurBayarBulat.DecimalValue) & "', '" & Num_En_US(txtGrandDijamin.DecimalValue) & "', '" & Num_En_US(txtGrandDijaminBulat.DecimalValue) & "')"
                'CMD.ExecuteNonQuery()
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Simpan resep_jual_detail
                For i = 0 To gridDetailObat.RowCount - 2
                    sqlEditPenjualanObat = sqlEditPenjualanObat & vbCrLf & "INSERT INTO resep_jual_detail(no_nota, idx_barang, kd_satuan_kecil, hpp, harga, jumlah, biaya_jaminan, discount_persen, discount_rupiah, tunai, piutang, tagihan, nr, urutan, kd_sub_unit_asal, status_verifikasi, status_jurnal, rek_p, kd_barang, nama_barang, status_paket) values ('" & Trim(txtNoResep.Text) & "', '" & Trim(gridDetailObat.Rows(i).Cells("idx_barang").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("nmsatuan").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("hrgbeli").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("harga").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("jml").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("dijamin").Value) & "', '0', '0', '" & Num_En_US((gridDetailObat.Rows(i).Cells("totalharga").Value) - (gridDetailObat.Rows(i).Cells("dijamin").Value)) & "',  '" & Num_En_US(gridDetailObat.Rows(i).Cells("dijamin").Value) & "', '" & Num_En_US(gridDetailObat.Rows(i).Cells("totalharga").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("racik").Value) & "', " & i + 1 & ", '" & kdSubUnit & "', '0', '0', '" & Trim(gridDetailObat.Rows(i).Cells("rek_p").Value) & "', '" & Trim(gridDetailObat.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObat.Rows(i).Cells("nama_barang").Value)) & "', '0')"
                    'CMD.ExecuteNonQuery()
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Update Stok
                'If  psts_stok = "1" Then
                '    For i = 0 To gridDetailObat.RowCount - 2
                '        'konek()
                '        sqlEditPenjualanObat = sqlEditPenjualanObat + vbCrLf + "UPDATE barang_farmasi SET " & memStok & "=" & memStok & "-" & Num_En_US(gridDetailObat.Rows(i).Cells("jml").Value) & " WHERE kd_barang='" & Trim(gridDetailObat.Rows(i).Cells("kd_barang").Value) & "'"
                '        'CMD.ExecuteNonQuery()
                '    Next
                'End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                CMD.CommandText = sqlEditPenjualanObat
                CMD.ExecuteNonQuery()
                Trans.Commit()
                MsgBox("Transaksi penjualan berhasil diedit", vbInformation, "Informasi")
                btnSimpan.Enabled = False
                btnCetakNota.Enabled = True
                btnCetakNota.Focus()
            Catch ex As Exception
                MsgBox(" Commit Exception Type: {0}" & ex.GetType.ToString, vbCritical, "Kesalahan")
                MsgBox(" Message: {0}" & ex.Message, vbCritical, "Kesalahan")
                Try
                    Trans.Rollback()
                Catch ex2 As Exception
                    MsgBox(" Rollback Exception Type: {0}" & ex.GetType.ToString, vbCritical, "Kesalahan")
                    MsgBox(" Message: {0}" & ex.Message, vbCritical, "Kesalahan")
                End Try
            End Try
        End If
    End Sub

    Private Sub txtCariObat_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCariObat.KeyDown
        If e.KeyCode = Keys.Down Then
            gridBarang.Focus()
        End If
    End Sub

    Private Sub txtCariObat_TextChanged(sender As Object, e As EventArgs) Handles txtCariObat.TextChanged
        BDDataBarang.Filter = "nama_barang like '%" & txtCariObat.Text & "%'"
    End Sub

    Private Sub btnBaru_Click(sender As Object, e As EventArgs) Handles btnBaru.Click
        KosongkanHeader()
        KosongkanDetailPaketUmum()
    End Sub

    Private Sub btnKeluarKh_Click(sender As Object, e As EventArgs) Handles btnKeluarKh.Click
        Dispose()
    End Sub

    Private Sub gridDetailObatKh_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridDetailObatKh.CellContentClick

    End Sub

    Private Sub gridDetailObatKh_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles gridDetailObatKh.CellFormatting
        gridDetailObatKh.Rows(e.RowIndex).HeaderCell.Value = CStr(e.RowIndex + 1)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MessageBox.Show("Yakin Akan Dihapus?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Try
                If gridDetailObatKh.CurrentRow.Index <> gridDetailObatKh.NewRowIndex Then
                    gridDetailObatKh.Rows.RemoveAt(gridDetailObatKh.CurrentRow.Index)
                End If
                txtQtyKh.DecimalValue = gridDetailObatKh.Rows.Count() - 1
                TotalPaket()
                TotalNonPaket()
            Catch ex As Exception
                If gridDetailObatKh.CurrentRow.Index <> gridDetailObatKh.NewRowIndex Then
                    gridDetailObatKh.Rows.RemoveAt(gridDetailObatKh.CurrentRow.Index)
                End If
                txtQtyKh.DecimalValue = gridDetailObatKh.Rows.Count() - 1
                TotalPaket()
                TotalNonPaket()
            End Try
        End If
    End Sub

    Private Sub btnInfoResepKh_Click(sender As Object, e As EventArgs) Handles btnInfoResepKh.Click
        FormPemanggil = "FormEditPenjualanResep"
        If txtNoReg.Text = "" Then
            MsgBox("Pilih pasien terlebih dahulu")
            txtNoReg.Focus()
        Else
            FormInfoResepObat.ShowDialog()
        End If
    End Sub

    Private Sub cmbRacikNonKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbRacikNonKh.KeyPress
        If e.KeyChar = Chr(13) Then
            If cmbRacikNonKh.Text = "R" Or cmbRacikNonKh.Text = "r" Or cmbRacikNonKh.Text = "N" Or cmbRacikNonKh.Text = "n" Then
                txtKodeObatKh.Focus()
            Else
                MsgBox("Pilih yang benar", vbCritical, "Kesalahan")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbRacikNonKh_LostFocus(sender As Object, e As EventArgs) Handles cmbRacikNonKh.LostFocus
        cmbRacikNonKh.Text = (cmbRacikNonKh.Text.ToUpper)
    End Sub

    Private Sub cmbRacikNonKh_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbRacikNonKh.SelectedIndexChanged
        If cmbRacikNonKh.Text = "R" Then
            txtDosisResepKh.Enabled = True
            txtJmlCapBPJSKh.Enabled = True
            txtJmlCapLainKh.Enabled = True
            txtJmlObatKh.Enabled = True
        Else
            txtDosisResepKh.Enabled = False
            txtJmlCapBPJSKh.Enabled = False
            txtJmlCapLainKh.Enabled = False
            txtJmlObatKh.Enabled = False
        End If
    End Sub

    Private Sub txtKodeObatKh_Click(sender As Object, e As EventArgs) Handles txtKodeObatKh.Click
        tampilBarang()
        PanelObat.Visible = True
        txtCariObat.Clear()
        txtCariObat.Focus()
    End Sub

    Private Sub txtKodeObatKh_GotFocus(sender As Object, e As EventArgs) Handles txtKodeObatKh.GotFocus
        tampilBarang()
        PanelObat.Visible = True
        txtCariObat.Clear()
        txtCariObat.Focus()
    End Sub

    Private Sub txtDosisResepKh_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDosisResepKh.KeyDown
        If e.KeyCode = Keys.Up Then
            txtKodeObatKh.Focus()
        End If
    End Sub

    Private Sub txtDosisResepKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDosisResepKh.KeyPress
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtJmlCapBPJSKh_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJmlCapBPJSKh.KeyDown
        If e.KeyCode = Keys.Up Then
            txtDosisResepKh.Focus()
        End If
    End Sub

    Private Sub txtJmlCapBPJSKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJmlCapBPJSKh.KeyPress
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtJmlCapLainKh_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJmlCapLainKh.KeyDown
        If e.KeyCode = Keys.Up Then
            txtJmlCapBPJSKh.Focus()
        End If
    End Sub

    Private Sub txtJmlCapLainKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJmlCapLainKh.KeyPress
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtJmlObatKh_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJmlObatKh.KeyDown
        If e.KeyCode = Keys.Up Then
            txtJmlCapLainKh.Focus()
        End If
    End Sub

    Private Sub txtJmlObatKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJmlObatKh.KeyPress
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtPaketBPJSKh_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPaketBPJSKh.KeyDown
        If e.KeyCode = Keys.Up Then
            txtKodeObatKh.Focus()
        End If
    End Sub

    Private Sub txtPaketBPJSKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPaketBPJSKh.KeyPress
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtPaketBPJSKh_TextChanged(sender As Object, e As EventArgs) Handles txtPaketBPJSKh.TextChanged
        txtTotalPaketBPJSKh.DecimalValue = txtPaketBPJSKh.DecimalValue * txtHargaJualKh.DecimalValue
    End Sub

    Private Sub txtPaketLainKh_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPaketLainKh.KeyDown
        If e.KeyCode = Keys.Up Then
            txtPaketBPJSKh.Focus()
        End If
    End Sub

    Private Sub txtPaketLainKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPaketLainKh.KeyPress
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtPaketLainKh_TextChanged(sender As Object, e As EventArgs) Handles txtPaketLainKh.TextChanged
        txtTotalPaketLainKh.DecimalValue = txtPaketLainKh.DecimalValue * txtHargaJualKh.DecimalValue
    End Sub

    Private Sub txtJmlHariKh_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJmlHariKh.KeyDown
        If e.KeyCode = Keys.Up Then
            txtPaketLainKh.Focus()
        End If
    End Sub

    Private Sub txtJmlHariKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJmlHariKh.KeyPress
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtJmlHariKh_TextChanged(sender As Object, e As EventArgs) Handles txtJmlHariKh.TextChanged
        TglServer()
        DTPTglAkhirKh.Value = DateAdd("d", Val(txtJmlHariKh.Text), DTPTanggalTrans.Value)
    End Sub

    Private Sub cmbEtiketKh_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbEtiketKh.KeyPress
        If e.KeyChar = Chr(13) Then
            If cmbEtiketKh.Text = "Y" Or cmbEtiketKh.Text = "y" Or cmbEtiketKh.Text = "N" Or cmbEtiketKh.Text = "n" Then
                If cmbEtiketKh.Text = "N" Then
                    PanelEtiket.Visible = False
                    SendKeys.Send("{TAB}")
                Else
                    PanelEtiket.Visible = True
                    txtNamaObatEtiket.Text = lblNamaObatKh.Text
                    txtJumlahObatEtiket.DecimalValue = txtPaketBPJSKh.DecimalValue + txtPaketLainKh.DecimalValue
                    txtNamaObatEtiket.Focus()
                End If
            Else
                MsgBox("Pilih yang benar", vbCritical, "Kesalahan")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbEtiketKh_LostFocus(sender As Object, e As EventArgs) Handles cmbEtiketKh.LostFocus
        cmbEtiketKh.Text = (cmbEtiketKh.Text.ToUpper)
        nmPaket = "PKTKHUSUS"
    End Sub

    Private Sub cmbEtiketKh_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEtiketKh.SelectedIndexChanged
        If cmbEtiketKh.Text = "N" Then
            PanelEtiket.Visible = False
        Else
            PanelEtiket.Visible = True
            txtNamaObatEtiket.Text = lblNamaObatKh.Text
            txtJumlahObatEtiket.DecimalValue = txtPaketBPJSKh.DecimalValue + txtPaketLainKh.DecimalValue
            txtNamaObatEtiket.Focus()
        End If
    End Sub

    Private Sub txtQty1_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtQty2_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = Chr(13) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbTakaran_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbTakaran.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmbWaktu.Focus()
        End If
        If e.KeyCode = Keys.Left Then
            txtSigna1.Focus()
        End If
    End Sub

    Private Sub cmbWaktu_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbWaktu.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmbKeterangan.Focus()
        End If
        If e.KeyCode = Keys.Left Then
            cmbTakaran.Focus()
        End If
    End Sub

    Private Sub cmbKeterangan_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbKeterangan.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtJarakED.Focus()
        End If
        If e.KeyCode = Keys.Left Then
            cmbWaktu.Focus()
        End If
    End Sub

    Private Sub btnAddKh_Click(sender As Object, e As EventArgs) Handles btnAddKh.Click
        If txtNoReg.Text = "" Then
            MsgBox("Pilih pasien terlebih dahulu")
            Exit Sub
        End If
        If txtKodeObatKh.Text = "" Then
            MsgBox("Obat belum dipilih")
            Exit Sub
        End If
        'For barisGrid As Integer = 0 To gridDetailObatKh.RowCount - 1
        '    If Trim(txtKodeObatKh.Text) = gridDetailObatKh.Rows(barisGrid).Cells("kd_barang").Value Then
        '        MsgBox("Obat ini sudah dientry")
        '        KosongkanDetailPaketKhusus()
        '        txtKodeObatKh.Focus()
        '        Exit Sub
        '    End If
        'Next
        addBarangKh()
        AturGriddetailBarangKh()
        NoUrut()
        KosongkanDetailPaketKhusus()
        txtQtyKh.DecimalValue = gridDetailObatKh.Rows.Count() - 1
        cmbRacikNonKh.Focus()
        btnSimpanKh.Enabled = True
    End Sub

    Private Sub btnBaruKh_Click(sender As Object, e As EventArgs) Handles btnBaruKh.Click
        KosongkanHeader()
        KosongkanDetailPaketKhusus()
        DTPTanggalTrans.Focus()
    End Sub

    Private Sub btnSimpanKh_Click(sender As Object, e As EventArgs) Handles btnSimpanKh.Click
        If Posting = "2" Then
            MsgBox("Transaksi tidak bisa diedit, sudah diposting oleh kasir", vbInformation, "Informasi")
            Exit Sub
        End If
        cariSubUnitAsal()
        If pkdapo = "001" Then
            memStok = "stok001"
        ElseIf pkdapo = "002" Then
            memStok = "stok002"
        ElseIf pkdapo = "003" Then
            memStok = "stok003"
        ElseIf pkdapo = "004" Then
            memStok = "stok004"
        ElseIf pkdapo = "005" Then
            memStok = "stok005"
        ElseIf pkdapo = "006" Then
            memStok = "stok006"
        ElseIf pkdapo = "007" Then
            memStok = "stok007"
        Else
            MsgBox("Setting apotik belum benar, silahkan hubungi administrator")
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Cek Stok
        If CekKunciStokPenjualan = "Y" Then
            For i = 0 To gridDetailObatKh.RowCount - 2
                CMD = New OleDb.OleDbCommand("select idx_barang,kd_barang,nama_barang, " & memStok & " as stok, kd_satuan_kecil from Barang_Farmasi where kd_barang='" & gridDetailObatKh.Rows(i).Cells("kd_barang").Value & "'", CONN)
                DA = New OleDb.OleDbDataAdapter(CMD)
                DT = New DataTable
                DA.Fill(DT)
                If DT.Rows.Count > 0 Then
                    If (DT.Rows(0).Item("stok") + gridDetailObatKh.Rows(i).Cells("jml_awal").Value) < gridDetailObatKh.Rows(i).Cells("jml").Value Then
                        MsgBox("Stok " + Trim(DT.Rows(0).Item("nama_barang")) + " hanya " + (DT.Rows(0).Item("stok") + gridDetailObatKh.Rows(i).Cells("jml_awal").Value).ToString + " masukan ulang jumlah barang", vbInformation, "Informasi")
                        Exit Sub
                    End If
                End If
            Next
        End If
        If MessageBox.Show("Data Penjualan sudah benar ...?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim sqlEditPenjualanObatKh As String = ""
            TglServer()
            DTPJamAkhir.Value = TanggalServer
            Trans = CONN.BeginTransaction(IsolationLevel.ReadCommitted)
            CMD.Connection = CONN
            CMD.Transaction = Trans
            Try
                ''''''''''''''''''''''''''''''''''''''HAPUS TRANS APOTEK''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr1
                sqlEditPenjualanObatKh = "Delete from ap_jualr1 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr2
                sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "Delete from ap_jualr2 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr2_bpjs
                sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "Delete from ap_jualr2_bpjs WHERE tglresep='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_etiketNew
                sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "Delete from ap_etiketNew WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''HAPUS TRANS KASIR'''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus resep_jual
                sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "Delete from resep_jual WHERE no_nota='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus resep_jual_detail
                sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "Delete from resep_jual_detail WHERE no_nota='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''Stok Kembali Asal'''''''''''''''''''''''''''''''''''''''''''''
                'If psts_stok = "1" Then
                '    For i = 0 To gridStokKembali.RowCount - 2
                '        sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "UPDATE Barang_Farmasi SET " & memStok & "=(" & memStok & "+" & Num_En_US(gridStokKembali.Rows(i).Cells("jml").Value) & ") WHERE kd_barang='" & Trim(gridStokKembali.Rows(i).Cells("kd_barang").Value) & "'"
                '        'CMD.ExecuteNonQuery()
                '    Next
                'End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''TRANS KE APOTEK'''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 'Simpan ap_jualr1
                sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "insert into ap_jualr1 (stsrawat,kdkasir,nmkasir,tanggal,notaresep,no_reg,no_rm,nama_pasien,kd_penjamin,nm_penjamin,kddokter,nmdokter,kdbagian,stsresep,totalpaket,totalpaket_bulat,totalnonpaket,totalnonpaket_bulat,totaldijamin,totaldijamin_bulat,totalselisih_bayar,totalselisih_bayar_bulat,kd_sub_unit,kd_sub_unit_asal,nama_sub_unit,jam,rsp_pulang,posting,diserahkan) values ('" & StatusRawat & "', '" & Trim(FormLogin.LabelKode.Text) & "', '" & Trim(FormLogin.LabelNama.Text) & "', '" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtNoReg.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(txtNamaPasien.Text) & "', '" & KdPenjamin & "', '" & NamaPenjamin & "', '" & kdDokter & "', '" & NamaDokter & "', '" & pkdapo & "', 'PKTKHUSUS', '" & Num_En_US(txtGrandTotalPaket.DecimalValue) & "', '" & Num_En_US(txtGrandTotalPaketBulat.DecimalValue) & "', '" & Num_En_US(txtGrandTotalNonPaket.DecimalValue) & "', '" & Num_En_US(txtGrandTotalNonPaketBulat.DecimalValue) & "', '" & Num_En_US(txtGrandTotalPaket.DecimalValue) & "', '" & Num_En_US(txtGrandTotalPaketBulat.DecimalValue) & "', '0', '0', '" & kdSubUnit & "', '" & kdSubUnit & "', '" & nmSubUnit & "', '" & Format(DTPJamAkhir.Value, "HH:mm:ss") & "', 'N', '1', 'B')"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 'Simpan ap_jualr2
                For i = 0 To gridDetailObatKh.RowCount - 2

                    sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "INSERT INTO ap_jualr2(stsrawat,kdkasir,nmkasir,tanggal,notaresep,no_reg,no_rm,nmpasien,umurthn,umurbln,kd_penjamin,nm_penjamin,kddokter,nmdokter,nonota,urut,kd_barang,idx_barang,nama_barang,kd_jns_obat,kd_gol_obat,kd_kel_obat,kdpabrik,generik,formularium,racik,harga,jmlpaket,totalpaket,jmlnonpaket,totalnonpaket,jml,nmsatuan,totalharga,senpot,potongan,jmlnet,dijamin,sisabayar,hrgbeli,jamawal,kdbagian,stsresep,rek_p,stsetiket,jmlhari,posting,diserahkan,jam,rsp_pulang,jns_obat,jmljatah,tglakhir) VALUES ('" & StatusRawat & "', '" & Trim(FormLogin.LabelKode.Text) & "', '" & Trim(FormLogin.LabelNama.Text) & "','" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtNoReg.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(txtNamaPasien.Text) & "','" & Trim(txtUmurThn.Text) & "', '" & Trim(txtUmurBln.Text) & "', '" & KdPenjamin & "', '" & NamaPenjamin & "', '" & kdDokter & "', '" & NamaDokter & "', '" & Trim(txtNota.Text) & "', " & i + 1 & ", '" & Trim(gridDetailObatKh.Rows(i).Cells("kd_barang").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("idx_barang").Value) & "', '" & Rep(Trim(gridDetailObatKh.Rows(i).Cells("nama_barang").Value)) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("kd_jns_obat").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("kd_gol_obat").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("kd_kel_obat").Value) & "','" & Trim(gridDetailObatKh.Rows(i).Cells("kdpabrik").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("generik").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("formularium").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("racik").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("harga").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jmlp").Value) & "','" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totalp").Value) & "','" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jmln").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totaln").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jml").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("nmsatuan").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totalharga").Value) & "', '" & gridDetailObatKh.Rows(i).Cells("senpot").Value & "','" & Num_En_US(gridDetailObatKh.Rows(i).Cells("potongan").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jmlnet").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("dijamin").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("sisabayar").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("hrgbeli").Value) & "', '" & Format(DTPJamAwal.Value, "HH:mm:ss") & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("kdbagian").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("stsresep").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("rek_p").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("stsetiket").Value) & "', '" & gridDetailObatKh.Rows(i).Cells("jmlhari").Value & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("posting").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("diserahkan").Value) & "', '" & Format(DTPJamAkhir.Value, "HH:mm:ss") & "', 'N', '" & Trim(gridDetailObatKh.Rows(i).Cells("jns_obat").Value) & "', '" & gridDetailObatKh.Rows(i).Cells("jmljatah").Value & "', '" & Format(gridDetailObatKh.Rows(i).Cells("tglakhir").Value, "yyyy/MM/dd") & "')"
                    'CMD.ExecuteNonQuery()
                Next
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Jatah Paket
                For i = 0 To gridDetailObatKh.RowCount - 2
                    If gridDetailObatKh.Rows(i).Cells("jmljatah").Value > 0 Then
                        sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "INSERT INTO ap_jualr2_bpjs(stsrawat,tglresep,notaresep,no_rm,kd_penjamin,kd_barang,nama_barang,jmlpaket,jmlnonpaket,jmljatah,tglakhir,kdbagian) VALUES ('" & StatusRawat & "', '" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtRM.Text) & "', '" & KdPenjamin & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObatKh.Rows(i).Cells("nama_barang").Value)) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jmlp").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jmln").Value) & "', '" & gridDetailObatKh.Rows(i).Cells("jmljatah").Value & "', '" & Format(gridDetailObatKh.Rows(i).Cells("tglakhir").Value, "yyyy/MM/dd") & "',  '" & Trim(gridDetailObatKh.Rows(i).Cells("kdbagian").Value) & "')"
                        'CMD.ExecuteNonQuery()
                    End If
                Next
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Simpan etiketNew
                For i = 0 To gridDetailObatKh.RowCount - 2
                    If gridDetailObatKh.Rows(i).Cells("stsetiket").Value = "Y" Then
                        sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "insert into ap_etiketNew(tanggal, notaresep, no_rm, kd_barang, nama_barang, kd_takaran, kd_waktu, kd_ketminum, signa1, signa2, tgl_exp, jml_obat, urut) values ('" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Trim(txtNoResep.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObatKh.Rows(i).Cells("nmobat_etiket").Value)) & "', '" & gridDetailObatKh.Rows(i).Cells("takaran").Value & "', '" & gridDetailObatKh.Rows(i).Cells("waktu").Value & "', '" & gridDetailObatKh.Rows(i).Cells("ketminum").Value & "','" & Trim(gridDetailObatKh.Rows(i).Cells("qty1").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("qty2").Value) & "', '" & Format(gridDetailObatKh.Rows(i).Cells("tgl_exp").Value, "yyyy/MM/dd") & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jmlobat_etiket").Value) & "', " & i + 1 & ")"
                    End If
                Next
                ''''''''''''''''''''''''''''''''''''''TRANS KE KASIR'''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 'Simpan resep_jual
                sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "insert into resep_jual(no_nota, no_rm, no_reg, jenis_rawat, tgl_jual, waktu, kd_dokter, kd_sub_unit, status_bayar, kd_kelompok_pelanggan, metode_bayar, total, user_id, user_nama, waktu_in, waktu_out, kd_sub_unit_asal, total_bulat, total_non_paket, total_non_paket_bulat, total_tunai, total_tunai_bulat, total_piutang, total_piutang_bulat) values ('" & Trim(txtNoResep.Text) & "', '" & Trim(txtRM.Text) & "', '" & Trim(txtNoReg.Text) & "',  '" & StatusRawat & "', '" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "', '" & Format(DTPJamAkhir.Value, "HH:mm:ss").ToString & "', '" & kdDokter & "', '" & pkdsubunit & "', 'BELUM', '0', 'KREDIT', '" & Num_En_US(txtGrandTotalPaket.DecimalValue) & "', '" & Trim(FormLogin.LabelKode.Text) & "', '" & Trim(FormLogin.LabelNama.Text) & "', '" & Format(DTPJamAkhir.Value, "HH:mm:ss") & "', '-', '" & kdSubUnit & "', '" & Num_En_US(txtGrandTotalPaketBulat.DecimalValue) & "', '" & Num_En_US(txtGrandTotalNonPaket.DecimalValue) & "', '" & Num_En_US(txtGrandTotalNonPaketBulat.DecimalValue) & "','0', '0', '" & Num_En_US(txtGrandTotalPaket.DecimalValue) & "', '" & Num_En_US(txtGrandTotalPaketBulat.DecimalValue) & "')"
                'CMD.ExecuteNonQuery()
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 'Simpan resep_jual_detail
                For i = 0 To gridDetailObatKh.RowCount - 2
                    If gridDetailObatKh.Rows(i).Cells("jmlp").Value > 0 Then
                        sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "INSERT INTO resep_jual_detail(no_nota, idx_barang, kd_satuan_kecil, hpp, harga, jumlah, biaya_jaminan, discount_persen, discount_rupiah, tunai, piutang, tagihan, nr, urutan, kd_sub_unit_asal, status_verifikasi, status_jurnal, rek_p, kd_barang, nama_barang, status_paket) values ('" & Trim(txtNoResep.Text) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("idx_barang").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("nmsatuan").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("hrgbeli").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("harga").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jmlp").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totalp").Value) & "', '0', '0', '0',  '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totalp").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totalp").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("racik").Value) & "', " & i + 1 & ", '" & kdSubUnit & "', '0', '0', '" & Trim(gridDetailObatKh.Rows(i).Cells("rek_p").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObatKh.Rows(i).Cells("nama_barang").Value)) & "', '0')"
                        'CMD.ExecuteNonQuery()
                    End If
                Next
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For i = 0 To gridDetailObatKh.RowCount - 2
                    If gridDetailObatKh.Rows(i).Cells("totaln").Value > 0 Then
                        sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "INSERT INTO resep_jual_detail(no_nota, idx_barang, kd_satuan_kecil, hpp, harga, jumlah, biaya_jaminan, discount_persen, discount_rupiah, tunai, piutang, tagihan, nr, urutan, kd_sub_unit_asal, status_verifikasi, status_jurnal, rek_p, kd_barang, nama_barang, status_paket) values ('" & Trim(txtNoResep.Text) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("idx_barang").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("nmsatuan").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("hrgbeli").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("harga").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jmln").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totaln").Value) & "', '0', '0', '0',  '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totaln").Value) & "', '" & Num_En_US(gridDetailObatKh.Rows(i).Cells("totaln").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("racik").Value) & "', " & i + 1 & ", '" & kdSubUnit & "', '0', '0', '" & Trim(gridDetailObatKh.Rows(i).Cells("rek_p").Value) & "', '" & Trim(gridDetailObatKh.Rows(i).Cells("kd_barang").Value) & "', '" & Rep(Trim(gridDetailObatKh.Rows(i).Cells("nama_barang").Value)) & "', '1')"
                        'CMD.ExecuteNonQuery()
                    End If
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Update Stok
                'If psts_stok = "1" Then
                '    For i = 0 To gridDetailObatKh.RowCount - 2
                '        sqlEditPenjualanObatKh = sqlEditPenjualanObatKh & vbCrLf & "UPDATE Barang_Farmasi SET " & memStok & "=(" & memStok & "-" & Num_En_US(gridDetailObatKh.Rows(i).Cells("jml").Value) & ") WHERE kd_barang='" & Trim(gridDetailObatKh.Rows(i).Cells("kd_barang").Value) & "'"
                '        'CMD.ExecuteNonQuery()
                '    Next
                'End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                CMD.CommandText = sqlEditPenjualanObatKh
                CMD.ExecuteNonQuery()
                Trans.Commit()
                MsgBox("Transaksi penjualan berhasil diedit", vbInformation, "Informasi")
                btnSimpanKh.Enabled = False
                btnCetakBPJS.Enabled = True
                btnCetakLain.Enabled = True
                btnCetakEtiketKh.Enabled = True
            Catch ex As Exception
                MsgBox(" Commit Exception Type: {0}" & ex.GetType.ToString, vbCritical, "Kesalahan")
                MsgBox(" Message: {0}" & ex.Message, vbCritical, "Kesalahan")
                Try
                    Trans.Rollback()
                Catch ex2 As Exception
                    MsgBox(" Rollback Exception Type: {0}" & ex.GetType.ToString, vbCritical, "Kesalahan")
                    MsgBox(" Message: {0}" & ex.Message, vbCritical, "Kesalahan")
                End Try
            End Try
        End If
    End Sub

    Private Sub btnHapusNotaKh_Click(sender As Object, e As EventArgs) Handles btnHapusNotaKh.Click
        If MessageBox.Show("Yakin transaksi ini akan dihapus?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            If Posting = "2" Then
                MsgBox("Transaksi tidak bisa diedit, sudah diposting oleh kasir", vbInformation, "Informasi")
                Exit Sub
            End If
            cariSubUnitAsal()

            If pkdapo = "001" Then
                memStok = "stok001"
            ElseIf pkdapo = "002" Then
                memStok = "stok002"
            ElseIf pkdapo = "003" Then
                memStok = "stok003"
            ElseIf pkdapo = "004" Then
                memStok = "stok004"
            ElseIf pkdapo = "005" Then
                memStok = "stok005"
            ElseIf pkdapo = "006" Then
                memStok = "stok006"
            ElseIf pkdapo = "007" Then
                memStok = "stok007"
            Else
                MsgBox("Setting apotik belum benar, silahkan hubungi administrator")
            End If

            Dim sqlHapusPenjualanObatKh As String = ""
            Trans = CONN.BeginTransaction(IsolationLevel.ReadCommitted)
            CMD.Connection = CONN
            CMD.Transaction = Trans
            Try
                ''''''''''''''''''''''''''''''''''''''HAPUS TRANS APOTEK''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr1
                sqlHapusPenjualanObatKh = "Delete from ap_jualr1 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr2
                sqlHapusPenjualanObatKh = sqlHapusPenjualanObatKh & vbCrLf & "Delete from ap_jualr2 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr2_bpjs
                sqlHapusPenjualanObatKh = sqlHapusPenjualanObatKh & vbCrLf & "Delete from ap_jualr2_bpjs WHERE tglresep='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus etiket
                sqlHapusPenjualanObatKh = sqlHapusPenjualanObatKh & vbCrLf & "Delete from ap_etiketNew WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''HAPUS TRANS KASIR'''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus resep_jual
                sqlHapusPenjualanObatKh = sqlHapusPenjualanObatKh & vbCrLf & "Delete from resep_jual WHERE no_nota='" & Trim(txtNoResep.Text) & "'"
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus resep_jual_detail
                sqlHapusPenjualanObatKh = sqlHapusPenjualanObatKh & vbCrLf & "Delete from resep_jual_detail WHERE no_nota='" & Trim(txtNoResep.Text) & "'"
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''Stok Kembali Asal'''''''''''''''''''''''''''''''''''''''''''''
                If psts_stok = "1" Then
                    For i = 0 To gridStokKembali.RowCount - 2
                        sqlHapusPenjualanObatKh = sqlHapusPenjualanObatKh & vbCrLf & "UPDATE Barang_Farmasi SET " & memStok & "=(" & memStok & "+" & Num_En_US(gridStokKembali.Rows(i).Cells("jml").Value) & ") WHERE kd_barang='" & Trim(gridStokKembali.Rows(i).Cells("kd_barang").Value) & "'"
                        'CMD.ExecuteNonQuery()
                    Next
                End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                CMD.CommandText = sqlHapusPenjualanObatKh
                CMD.ExecuteNonQuery()
                Trans.Commit()
                MsgBox("Transaksi penjualan berhasil dihapus", vbInformation, "Informasi")
                KosongkanHeader()
                KosongkanDetailPaketKhusus()
                KosongkanDetailPaketUmum()
            Catch ex As Exception
                MsgBox(" Commit Exception Type: {0}" & ex.GetType.ToString, vbCritical, "Kesalahan")
                MsgBox(" Message: {0}" & ex.Message, vbCritical, "Kesalahan")
                Try
                    Trans.Rollback()
                Catch ex2 As Exception
                    MsgBox(" Rollback Exception Type: {0}" & ex.GetType.ToString, vbCritical, "Kesalahan")
                    MsgBox(" Message: {0}" & ex.Message, vbCritical, "Kesalahan")
                End Try
            End Try
        End If
    End Sub

    Private Sub btnCetakNota_Click(sender As Object, e As EventArgs) Handles btnCetakNota.Click
        FormPemanggil = "FormEditStatusBayar"
        bilang = Terbilang(txtGrandTotalBulat.DecimalValue)
        cetakNota()
        btnCetakNota.Enabled = False
    End Sub

    Private Sub btnCetakBPJS_Click(sender As Object, e As EventArgs) Handles btnCetakBPJS.Click
        FormPemanggil = "FormEditStatusBayar_BPJS"
        bilang = Terbilang(txtGrandTotalPaketBulat.DecimalValue)
        cetakNotaBPJS()
        btnCetakBPJS.Enabled = False
    End Sub

    Private Sub btnCetakLain_Click(sender As Object, e As EventArgs) Handles btnCetakLain.Click
        FormPemanggil = "FormEditStatusBayar_Lain"
        bilang = Terbilang(txtGrandTotalNonPaketBulat.DecimalValue)
        cetakNotaLain()
        btnCetakLain.Enabled = False
    End Sub

    Private Sub btnCetakEtiket_Click(sender As Object, e As EventArgs)
        If MessageBox.Show("Apakah akan cetak etiket ...?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            For i = 0 To gridDetailObat.RowCount - 2
                If gridDetailObat.Rows(i).Cells("stsetiket").Value = "Y" Then
                    Dim rpt As New ReportDocument
                    Try
                        Dim str As String = Application.StartupPath & "\Report\etiket.rpt"
                        rpt.Load(str)
                        'FormCetak.CrystalReportViewer1.Refresh()
                        rpt.SetDatabaseLogon(dbUser, dbPassword)
                        rpt.SetParameterValue("tanggal", Format(DTPTanggalTrans.Value, "yyyy/MM/dd"))
                        rpt.SetParameterValue("notaresep", Trim(txtNoResep.Text))
                        rpt.SetParameterValue("kdbarang", Trim(gridDetailObat.Rows(i).Cells("kd_barang").Value))
                        rpt.SetParameterValue("urut", (i + 1))
                        rpt.SetParameterValue("nmPasien", Trim(txtNamaPasien.Text))
                        rpt.SetParameterValue("bulan", Trim(txtUmurBln.Text))
                        rpt.SetParameterValue("tahun", Trim(txtUmurThn.Text))
                        rpt.SetParameterValue("user", Trim(FormLogin.LabelNama.Text))
                        rpt.PrintToPrinter(1, False, 0, 0)
                        rpt.Close()
                        rpt.Dispose()
                        'FormCetak.CrystalReportViewer1.ReportSource = rpt
                        'FormCetak.CrystalReportViewer1.Show()
                        'FormCetak.ShowDialog()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                End If
            Next
        End If
    End Sub

    Private Sub btnCetakEtiketKh_Click(sender As Object, e As EventArgs) Handles btnCetakEtiketKh.Click
        If MessageBox.Show("Apakah akan cetak etiket ...?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            For i = 0 To gridDetailObatKh.RowCount - 2
                If gridDetailObatKh.Rows(i).Cells("stsetiket").Value = "Y" Then
                    Dim rpt As New ReportDocument
                    Try
                        Dim str As String = Application.StartupPath & "\Report\etiket.rpt"
                        rpt.Load(str)
                        'FormCetak.CrystalReportViewer1.Refresh()
                        rpt.SetDatabaseLogon(dbUser, dbPassword)
                        rpt.SetParameterValue("tanggal", Format(DTPTanggalTrans.Value, "yyyy/MM/dd"))
                        rpt.SetParameterValue("notaresep", Trim(txtNoResep.Text))
                        rpt.SetParameterValue("kdbarang", Trim(gridDetailObatKh.Rows(i).Cells("kd_barang").Value))
                        rpt.SetParameterValue("urut", (i + 1))
                        rpt.SetParameterValue("nmPasien", Trim(txtNamaPasien.Text))
                        rpt.SetParameterValue("bulan", Trim(txtUmurBln.Text))
                        rpt.SetParameterValue("tahun", Trim(txtUmurThn.Text))
                        rpt.SetParameterValue("user", Trim(FormLogin.LabelNama.Text))
                        rpt.PrintToPrinter(1, False, 0, 0)
                        rpt.Close()
                        rpt.Dispose()
                        'FormCetak.CrystalReportViewer1.ReportSource = rpt
                        'FormCetak.CrystalReportViewer1.Show()
                        'FormCetak.ShowDialog()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                End If
            Next
            btnCetakEtiketKh.Enabled = False
        End If
    End Sub

    Private Sub btnUpdateDijamin_Click(sender As Object, e As EventArgs) Handles btnUpdateDijamin.Click
        For i = 0 To gridDetailObat.RowCount - 2
            gridDetailObat.Rows(i).Cells("dijamin").Value = gridDetailObat.Rows(i).Cells("jmlnet").Value
            gridDetailObat.Rows(i).Cells("sisabayar").Value = 0
        Next
        cmbDijamin.Text = "Y"
        TotalHarga()
        TotalDijamin()
        TotalIurBayar()
    End Sub

    Private Sub btnUpdateIurPasien_Click(sender As Object, e As EventArgs) Handles btnUpdateIurPasien.Click
        For i = 0 To gridDetailObat.RowCount - 2
            gridDetailObat.Rows(i).Cells("dijamin").Value = 0
            gridDetailObat.Rows(i).Cells("sisabayar").Value = gridDetailObat.Rows(i).Cells("jmlnet").Value
        Next
        cmbDijamin.Text = "N"
        TotalHarga()
        TotalDijamin()
        TotalIurBayar()
    End Sub

    Private Sub btnHapusNota_Click(sender As Object, e As EventArgs)
        If MessageBox.Show("Yakin transaksi ini akan dihapus?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            If Posting = "2" Then
                MsgBox("Transaksi tidak bisa diedit, sudah diposting oleh kasir", vbInformation, "Informasi")
                Exit Sub
            End If
            cariSubUnitAsal()
            If pkdapo = "001" Then
                memStok = "stok001"
            ElseIf pkdapo = "002" Then
                memStok = "stok002"
            ElseIf pkdapo = "003" Then
                memStok = "stok003"
            ElseIf pkdapo = "004" Then
                memStok = "stok004"
            ElseIf pkdapo = "005" Then
                memStok = "stok005"
            ElseIf pkdapo = "006" Then
                memStok = "stok006"
            ElseIf pkdapo = "007" Then
                memStok = "stok007"
            Else
                MsgBox("Setting apotik belum benar, silahkan hubungi administrator")
            End If

            Dim sqlHapusPenjualanObat As String = ""
            Trans = CONN.BeginTransaction(IsolationLevel.ReadCommitted)
            CMD.Connection = CONN
            CMD.Transaction = Trans
            Try
                ''''''''''''''''''''''''''''''''''''''HAPUS TRANS APOTEK'''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr1
                sqlHapusPenjualanObat = "Delete from ap_jualr1 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr2
                sqlHapusPenjualanObat = sqlHapusPenjualanObat & vbCrLf & "Delete from ap_jualr2 WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus ap_jualr2_bpjs
                sqlHapusPenjualanObat = sqlHapusPenjualanObat & vbCrLf & "Delete from ap_jualr2_bpjs WHERE tglresep='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus etiket
                sqlHapusPenjualanObat = sqlHapusPenjualanObat & vbCrLf & "Delete from ap_etiketNew WHERE tanggal='" & Format(DTPTanggalTrans.Value, "yyyy/MM/dd") & "' and notaresep='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                ''''''''''''''''''''''''''''''''''''''HAPUS TRANS KASIR'''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus resep_jual
                sqlHapusPenjualanObat = sqlHapusPenjualanObat & vbCrLf & "Delete from resep_jual WHERE no_nota='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Hapus resep_jual_detail
                sqlHapusPenjualanObat = sqlHapusPenjualanObat & vbCrLf & "Delete from resep_jual_detail WHERE no_nota='" & Trim(txtNoResep.Text) & "'"
                'CMD.ExecuteNonQuery()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''Stok Kembali Asal'''''''''''''''''''''''''''''''''''''''''''''
                If psts_stok = "1" Then
                    For i = 0 To gridStokKembali.RowCount - 2
                        sqlHapusPenjualanObat = sqlHapusPenjualanObat & vbCrLf & "UPDATE Barang_Farmasi SET " & memStok & "=(" & memStok & "+" & Num_En_US(gridStokKembali.Rows(i).Cells("jml").Value) & ") WHERE kd_barang='" & Trim(gridStokKembali.Rows(i).Cells("kd_barang").Value) & "'"
                        'CMD.ExecuteNonQuery()
                    Next
                End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                CMD.CommandText = sqlHapusPenjualanObat
                CMD.ExecuteNonQuery()
                Trans.Commit()
                MsgBox("Transaksi penjualan berhasil dihapus", vbInformation, "Informasi")
                KosongkanHeader()
                KosongkanDetailPaketKhusus()
                KosongkanDetailPaketUmum()
            Catch ex As Exception
                MsgBox(" Commit Exception Type: {0}" & ex.GetType.ToString, vbCritical, "Kesalahan")
                MsgBox(" Message: {0}" & ex.Message, vbCritical, "Kesalahan")
                Try
                    Trans.Rollback()
                Catch ex2 As Exception
                    MsgBox(" Rollback Exception Type: {0}" & ex.GetType.ToString, vbCritical, "Kesalahan")
                    MsgBox(" Message: {0}" & ex.Message, vbCritical, "Kesalahan")
                End Try
            End Try
        End If
    End Sub

    Private Sub DTPTanggalTrans_ValueChanged(sender As Object, e As EventArgs) Handles DTPTanggalTrans.ValueChanged
        DTPTanggalExp.Value = DateAdd("d", 30, DTPTanggalTrans.Value)
    End Sub

    Private Sub txtNamaObatEtiket_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNamaObatEtiket.KeyDown
        If e.KeyCode = Keys.Up Then
            cmbEtiket.Focus()
        End If
    End Sub

    Private Sub txtNamaObatEtiket_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNamaObatEtiket.KeyPress
        If e.KeyChar = Chr(13) Then
            txtJumlahObatEtiket.Focus()
        End If
    End Sub

    Private Sub txtSigna1_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSigna1.KeyDown
        If e.KeyCode = Keys.Up Then
            txtJumlahObatEtiket.Focus()
        End If
    End Sub

    Private Sub txtSigna1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSigna1.KeyPress
        If e.KeyChar = Chr(13) Then
            txtSigna2.Focus()
        End If
    End Sub

    Private Sub txtSigna2_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSigna2.KeyDown
        If e.KeyCode = Keys.Up Then
            txtSigna1.Focus()
        End If
    End Sub

    Private Sub txtSigna2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSigna2.KeyPress
        If e.KeyChar = Chr(13) Then
            cmbTakaran.Focus()
        End If
    End Sub

    Private Sub txtJarakED_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJarakED.KeyDown
        If e.KeyCode = Keys.Up Then
            cmbKeterangan.Focus()
        End If
    End Sub

    Private Sub txtJarakED_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJarakED.KeyPress
        If e.KeyChar = Chr(13) Then
            PanelEtiket.Visible = False
            If nmPaket = "PKTUMUM" Then
                btnAdd.Focus()
            Else
                btnAddKh.Focus()
            End If
        End If
    End Sub

    Private Sub txtJarakED_TextChanged(sender As Object, e As EventArgs) Handles txtJarakED.TextChanged
        DTPTanggalExp.Value = DateAdd("d", Val(txtJarakED.DecimalValue), DTPTanggalTrans.Value)
    End Sub

    Private Sub txtNamaObatEtiket_TextChanged(sender As Object, e As EventArgs) Handles txtNamaObatEtiket.TextChanged

    End Sub

    Private Sub txtJumlahObatEtiket_KeyDown(sender As Object, e As KeyEventArgs) Handles txtJumlahObatEtiket.KeyDown
        If e.KeyCode = Keys.Up Then
            txtNamaObatEtiket.Focus()
        End If
    End Sub

    Private Sub txtJumlahObatEtiket_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtJumlahObatEtiket.KeyPress
        If e.KeyChar = Chr(13) Then
            txtSigna1.Focus()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        FormCetakEtiketPerBarang.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        FormCetakEtiketPerBarang.ShowDialog()
    End Sub


    Private Sub txtJumlahObatEtiket_TextChanged(sender As Object, e As EventArgs) Handles txtJumlahObatEtiket.TextChanged

    End Sub

    Private Sub txtSigna1_TextChanged(sender As Object, e As EventArgs) Handles txtSigna1.TextChanged

    End Sub

    Private Sub txtSigna2_TextChanged(sender As Object, e As EventArgs) Handles txtSigna2.TextChanged

    End Sub

    Private Sub txtDosisResepKh_TextChanged(sender As Object, e As EventArgs) Handles txtDosisResepKh.TextChanged

    End Sub

    Private Sub txtJmlCapBPJSKh_TextChanged(sender As Object, e As EventArgs) Handles txtJmlCapBPJSKh.TextChanged

    End Sub

    Private Sub txtJmlCapLainKh_TextChanged(sender As Object, e As EventArgs) Handles txtJmlCapLainKh.TextChanged

    End Sub

    Private Sub txtJmlObatKh_TextChanged(sender As Object, e As EventArgs) Handles txtJmlObatKh.TextChanged

    End Sub

    Private Sub txtDijamin_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDijamin.KeyPress
        If e.KeyChar = Chr(13) Then
            txtJmlHari.Focus()
        End If
    End Sub

    Private Sub txtDijamin_TextChanged(sender As Object, e As EventArgs) Handles txtDijamin.TextChanged

    End Sub

    Private Sub txtNoResep_TextChanged(sender As Object, e As EventArgs) Handles txtNoResep.TextChanged

    End Sub

    Private Sub cmbTakaran_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTakaran.SelectedIndexChanged

    End Sub

    Private Sub cmbWaktu_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbWaktu.SelectedIndexChanged

    End Sub

    Private Sub cmbKeterangan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbKeterangan.SelectedIndexChanged

    End Sub
End Class