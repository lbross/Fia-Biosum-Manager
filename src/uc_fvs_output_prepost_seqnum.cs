﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FIA_Biosum_Manager
{
    public partial class uc_fvs_output_prepost_seqnum : UserControl
    {
        bool m_bSave = false;
        bool _bExit = false;
        int m_intCurIndex = -1;
        int m_intCurSeqNumItemIndex = -1;
        int m_intCurEditSeqNumItemIndex = -1;
        const int COL_STATUS = 0;
        const int COL_ID=1;
        const int COL_TABLENAME = 2;
        const int COL_TYPE = 3;
        const int COL_PACKAGELIST = 4;
        private FIA_Biosum_Manager.frmDialog _frmDialog = null;
        private FIA_Biosum_Manager.FVSPrePostSeqNumItem_Collection m_oCurFVSPrepostSeqNumItem_Collection = new FVSPrePostSeqNumItem_Collection();
        private FIA_Biosum_Manager.FVSPrePostSeqNumItem_Collection m_oSavFVSPrepostSeqNumItem_Collection = new FVSPrePostSeqNumItem_Collection();
        private FIA_Biosum_Manager.RxPackageItem_Collection m_oRxPackageItem_Collection = null;
        private ado_data_access m_oAdo = new ado_data_access();
        private RxTools m_oRxTools = new RxTools();
        private ComboBox m_cmbTableCustom;

        private ComboBox m_cmbFVSStrClassPre1;
        private ComboBox m_cmbFVSStrClassPre2;
        private ComboBox m_cmbFVSStrClassPre3;
        private ComboBox m_cmbFVSStrClassPre4;
        private ComboBox m_cmbFVSStrClassPost1;
        private ComboBox m_cmbFVSStrClassPost2;
        private ComboBox m_cmbFVSStrClassPost3;
        private ComboBox m_cmbFVSStrClassPost4;




       
        private static string m_strColumnList = "PREPOST_SEQNUM_ID,TableName,Type," +
                                    "RxCycle1_PRE_SeqNum," +
                                    "RxCycle1_POST_SeqNum," +
                                    "RxCycle2_PRE_SeqNum," +
                                    "RxCycle2_POST_SeqNum," +
                                    "RxCycle3_PRE_SeqNum," +
                                    "RxCycle3_POST_SeqNum," +
                                    "RxCycle4_PRE_SeqNum," +
                                    "RxCycle4_POST_SeqNum," +
                                    "RxCycle1_PRE_BASEYR_YN," +
                                    "RxCycle2_PRE_BASEYR_YN," +
                                    "RxCycle3_PRE_BASEYR_YN," +
                                    "RxCycle4_PRE_BASEYR_YN," + 
                                    "RxCycle1_PRE_BEFORECUT_YN," + 
                                    "RxCycle1_POST_BEFORECUT_YN," +
                                    "RxCycle2_PRE_BEFORECUT_YN," +
                                    "RxCycle2_POST_BEFORECUT_YN," +
                                    "RxCycle3_PRE_BEFORECUT_YN," +
                                    "RxCycle3_POST_BEFORECUT_YN," +
                                    "RxCycle4_PRE_BEFORECUT_YN," +
                                    "RxCycle4_POST_BEFORECUT_YN," + 
                                    "USE_SUMMARY_TABLE_SEQNUM_YN";
        
        public uc_fvs_output_prepost_seqnum()
        {
            int x;
            InitializeComponent();

            //add combo control to groupbox
            m_cmbTableCustom = new ComboBox();
            groupBox1.Controls.Add(m_cmbTableCustom);
            m_cmbTableCustom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            m_cmbTableCustom.ForeColor = lblCurTable.ForeColor;
            m_cmbTableCustom.BackColor = groupBox1.BackColor;
            m_cmbTableCustom.Location = lblCurTable.Location;
            m_cmbTableCustom.Width = lblType.Left - lblCurTable.Left;
            for (x = 0; x <= Tables.FVS.g_strFVSOutTablesArray.Length - 1; x++)
            {
                if (Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() != "FVS_TREELIST" &&
                    Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() != "FVS_ATRTLIST" &&
                    Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() != "FVS_CASES" &&
                    Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() != "FVS_MORTALITY" &&
                    Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() != "FVS_SNAGDET")
                        m_cmbTableCustom.Items.Add(Tables.FVS.g_strFVSOutTablesArray[x]);
            }
            m_cmbTableCustom.Text = "<Select or Enter Table Name>"; 
            m_cmbTableCustom.Hide();
            m_cmbTableCustom.SelectedIndexChanged += new System.EventHandler(m_cmbTableCustom_SelectedIndexChanged);

            lvFVSTables.Columns[COL_STATUS].TextAlign = HorizontalAlignment.Center;

            //add FVS_STRCLASS combo boxes
            m_cmbFVSStrClassPre1 = new ComboBox();
            groupBox1.Controls.Add(m_cmbFVSStrClassPre1);
            m_cmbFVSStrClassPre1.Location = chkPRE1BaseYear.Location;
            m_cmbFVSStrClassPre1.Width = (int)this.CreateGraphics().MeasureString("0 = After Tree Removal******", m_cmbFVSStrClassPre1.Font).Width;
            m_cmbFVSStrClassPre1.Items.Add("0 = Before Tree Removal");
            m_cmbFVSStrClassPre1.Items.Add("1 = After Tree Removal");
            m_cmbFVSStrClassPre1.Text = "0 = Before Tree Removal"; 
            m_cmbFVSStrClassPre1.Hide();
           

            m_cmbFVSStrClassPre2 = new ComboBox();
            groupBox1.Controls.Add(m_cmbFVSStrClassPre2);
            m_cmbFVSStrClassPre2.Location = chkPRE2BaseYear.Location;
            m_cmbFVSStrClassPre2.Width = m_cmbFVSStrClassPre1.Width;
            m_cmbFVSStrClassPre2.Items.Add("0 = Before Tree Removal");
            m_cmbFVSStrClassPre2.Items.Add("1 = After Tree Removal");
            m_cmbFVSStrClassPre2.Text = "0 = Before Tree Removal";
            m_cmbFVSStrClassPre2.Hide();
           

            m_cmbFVSStrClassPre3 = new ComboBox();
            groupBox1.Controls.Add(m_cmbFVSStrClassPre3);
            m_cmbFVSStrClassPre3.Location = chkPRE3BaseYear.Location;
            m_cmbFVSStrClassPre3.Width = m_cmbFVSStrClassPre1.Width;
            m_cmbFVSStrClassPre3.Items.Add("0 = Before Tree Removal");
            m_cmbFVSStrClassPre3.Items.Add("1 = After Tree Removal");
            m_cmbFVSStrClassPre3.Text = "0 = Before Tree Removal";
            m_cmbFVSStrClassPre3.Hide();
           
            m_cmbFVSStrClassPre4 = new ComboBox();
            groupBox1.Controls.Add(m_cmbFVSStrClassPre4);
            m_cmbFVSStrClassPre4.Location = chkPRE4BaseYear.Location;
            m_cmbFVSStrClassPre4.Width = m_cmbFVSStrClassPre1.Width;
            m_cmbFVSStrClassPre4.Items.Add("0 = Before Tree Removal");
            m_cmbFVSStrClassPre4.Items.Add("1 = After Tree Removal");
            m_cmbFVSStrClassPre4.Text = "0 = Before Tree Removal";
            m_cmbFVSStrClassPre4.Hide();
           

            m_cmbFVSStrClassPost1 = new ComboBox();
            groupBox1.Controls.Add(m_cmbFVSStrClassPost1);
            m_cmbFVSStrClassPost1.Location = cmbPOST1.Location;
            m_cmbFVSStrClassPost1.Left = m_cmbFVSStrClassPost1.Left + cmbPOST1.Width + 2;
            m_cmbFVSStrClassPost1.Width = m_cmbFVSStrClassPre1.Width;
            m_cmbFVSStrClassPost1.Items.Add("0 = Before Tree Removal");
            m_cmbFVSStrClassPost1.Items.Add("1 = After Tree Removal");
            m_cmbFVSStrClassPost1.Text = "0 = Before Tree Removal"; 
            m_cmbFVSStrClassPost1.Hide();
            

            m_cmbFVSStrClassPost2 = new ComboBox();
            groupBox1.Controls.Add(m_cmbFVSStrClassPost2);
            m_cmbFVSStrClassPost2.Location = m_cmbFVSStrClassPost1.Location;
            m_cmbFVSStrClassPost2.Top = cmbPOST2.Top;
            m_cmbFVSStrClassPost2.Width = m_cmbFVSStrClassPre1.Width;
            m_cmbFVSStrClassPost2.Items.Add("0 = Before Tree Removal");
            m_cmbFVSStrClassPost2.Items.Add("1 = After Tree Removal");
            m_cmbFVSStrClassPost2.Text = "0 = Before Tree Removal";
            m_cmbFVSStrClassPost2.Hide();
            

            m_cmbFVSStrClassPost3 = new ComboBox();
            groupBox1.Controls.Add(m_cmbFVSStrClassPost3);
            m_cmbFVSStrClassPost3.Location = m_cmbFVSStrClassPost1.Location;
            m_cmbFVSStrClassPost3.Top = cmbPOST3.Top;
            m_cmbFVSStrClassPost3.Width = m_cmbFVSStrClassPre1.Width;
            m_cmbFVSStrClassPost3.Items.Add("0 = Before Tree Removal");
            m_cmbFVSStrClassPost3.Items.Add("1 = After Tree Removal");
            m_cmbFVSStrClassPost3.Text = "0 = Before Tree Removal";
            m_cmbFVSStrClassPost3.Hide();
            

            m_cmbFVSStrClassPost4 = new ComboBox();
            groupBox1.Controls.Add(m_cmbFVSStrClassPost4);
            m_cmbFVSStrClassPost4.Location = m_cmbFVSStrClassPost1.Location;
            m_cmbFVSStrClassPost4.Top = cmbPOST4.Top;
            m_cmbFVSStrClassPost4.Width = m_cmbFVSStrClassPre1.Width;
            m_cmbFVSStrClassPost4.Items.Add("0 = Before Tree Removal");
            m_cmbFVSStrClassPost4.Items.Add("1 = After Tree Removal");
            m_cmbFVSStrClassPost4.Text = "0 = Before Tree Removal";
  
            m_cmbFVSStrClassPost4.Hide();
            

            loadvalues();
            
            
        }
        public frmDialog ReferenceDialog
        {
            get { return _frmDialog; }
            set { _frmDialog = value; }
        }
        public bool Exit
        {
            get {return _bExit;}
            set {_bExit=value;}
        }
        public void loadvalues()
        {
            lvFVSTables.Items.Clear();
            lblCurId.Text = "NA";
            lblCurTable.Text = "NA";
            lblCurType.Text= "NA";
            
           
            
            int x,y;

            for (x = m_oCurFVSPrepostSeqNumItem_Collection.Count - 1; x >= 0; x--)
            {
                m_oCurFVSPrepostSeqNumItem_Collection.Remove(x);
            }
            for (x = m_oSavFVSPrepostSeqNumItem_Collection.Count - 1; x >= 0; x--)
            {
                m_oSavFVSPrepostSeqNumItem_Collection.Remove(x);
            }

            EnableEdit(false);
            cmbPRE1.Items.Add("Not Used");
            cmbPRE2.Items.Add("Not Used");
            cmbPRE3.Items.Add("Not Used");
            cmbPRE4.Items.Add("Not Used");
            cmbPOST1.Items.Add("Not Used");
            cmbPOST2.Items.Add("Not Used");
            cmbPOST3.Items.Add("Not Used");
            cmbPOST4.Items.Add("Not Used");
            for (x = 1; x <= 50;x++ )
            {
                cmbPRE1.Items.Add(x.ToString().Trim());
                cmbPRE2.Items.Add(x.ToString().Trim());
                cmbPRE3.Items.Add(x.ToString().Trim());
                cmbPRE4.Items.Add(x.ToString().Trim());
                cmbPOST1.Items.Add(x.ToString().Trim());
                cmbPOST2.Items.Add(x.ToString().Trim());
                cmbPOST3.Items.Add(x.ToString().Trim());
                cmbPOST4.Items.Add(x.ToString().Trim());
            }
           
            m_oAdo.OpenConnection(m_oAdo.getMDBConnString(frmMain.g_oFrmMain.frmProject.uc_project1.txtRootDirectory.Text.Trim() + "\\" + Tables.FVS.DefaultFVSPrePostSeqNumTableDbFile, "", ""));

            if (m_oAdo.m_intError == 0)
            {

                InitializePrePostSeqNumTables(m_oAdo, m_oAdo.m_OleDbConnection);
                m_oRxTools.LoadFVSOutputPrePostRxCycleSeqNum(m_oAdo, m_oAdo.m_OleDbConnection, m_oCurFVSPrepostSeqNumItem_Collection);
                m_oCurFVSPrepostSeqNumItem_Collection.CopyProperties(m_oSavFVSPrepostSeqNumItem_Collection, m_oCurFVSPrepostSeqNumItem_Collection);
                m_oAdo.CloseConnection(m_oAdo.m_OleDbConnection);

                for (x = 0; x <= m_oCurFVSPrepostSeqNumItem_Collection.Count - 1; x++)
                {
                    AddListViewItemFromProperties(x);
                    

                }
                lvFVSTables.Columns[COL_TABLENAME].Width = -1;
                lvFVSTables.Columns[COL_TYPE].Width = -1;

            }
            
            
           
        }
        public static void InitializePrePostSeqNumTables(ado_data_access p_oAdo, string p_strDbFile)
        {
            p_oAdo.OpenConnection(p_oAdo.getMDBConnString(p_strDbFile, "", ""));
            if (p_oAdo.m_intError == 0)
                InitializePrePostSeqNumTables(p_oAdo, p_oAdo.m_OleDbConnection);
            p_oAdo.CloseConnection(p_oAdo.m_OleDbConnection);
        }

        public static void InitializePrePostSeqNumTables(ado_data_access p_oAdo, System.Data.OleDb.OleDbConnection p_oOleDbConnection)
        {
            int intRowCount = 0;

            string strValueList = "";
            int x;


            if (!p_oAdo.TableExist(p_oAdo.m_OleDbConnection, Tables.FVS.DefaultFVSPrePostSeqNumTable))
            {
                frmMain.g_oTables.m_oFvs.CreateFVSOutputPrePostSeqNumTable(p_oAdo, p_oAdo.m_OleDbConnection, Tables.FVS.DefaultFVSPrePostSeqNumTable);
            }
            if ((int)p_oAdo.getRecordCount(p_oAdo.m_OleDbConnection,"SELECT * FROM " + Tables.FVS.DefaultFVSPrePostSeqNumTable,Tables.FVS.DefaultFVSPrePostSeqNumTable) == 0)
            {


                for (x = 0; x <= Tables.FVS.g_strFVSOutTablesArray.Length - 1; x++)
                {
                    if (Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() == "FVS_SUMMARY" ||
                        Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() == "FVS_POTFIRE" ||
                        Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() == "FVS_CUTLIST" ||
                        Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() == "FVS_STRCLASS")
                    {
                        strValueList = Convert.ToString(intRowCount + 1).Trim() + ",'" + Tables.FVS.g_strFVSOutTablesArray[x].Trim() + "','D',";



                        if (Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper()=="FVS_SUMMARY")
                        {
                            strValueList = strValueList + "2,3,5,6,8,9,11,12,'N','N','N','N','Y','N','Y','N','Y','N','Y','N','Y'";
                        }
                        else if (Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper()=="FVS_POTFIRE")
                        {
                            strValueList = strValueList + "1,2,4,5,7,8,10,11,'N','N','N','N','Y','N','Y','N','Y','N','Y','N','N'";
                        }
                        else if (Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() == "FVS_CUTLIST")
                        {
                            strValueList = strValueList + "1,null,3,null,5,null,7,null,'N','N','N','N','Y','N','Y','N','Y','N','Y','N','Y'";
                        }
                        if (Tables.FVS.g_strFVSOutTablesArray[x].Trim().ToUpper() == "FVS_STRCLASS")
                        {
                            strValueList = strValueList + "2,3,5,6,8,9,11,12,'N','N','N','N','Y','Y','Y','Y','Y','Y','Y','Y','Y'";
                        }
                        p_oAdo.m_strSQL = "INSERT INTO " + Tables.FVS.DefaultFVSPrePostSeqNumTable + " " +
                                          "(" + m_strColumnList + ") VALUES " +
                                          "(" + strValueList + ")";
                        p_oAdo.SqlNonQuery(p_oAdo.m_OleDbConnection, p_oAdo.m_strSQL);

                        intRowCount++;
                    }

                }

            }
            if (!p_oAdo.TableExist(p_oAdo.m_OleDbConnection, Tables.FVS.DefaultFVSPrePostSeqNumRxPackageAssgnTable))
            {
                frmMain.g_oTables.m_oFvs.CreateFVSOutputPrePostSeqNumRxPackageAssgnTable(p_oAdo, p_oAdo.m_OleDbConnection, Tables.FVS.DefaultFVSPrePostSeqNumRxPackageAssgnTable);

            }
        }
        private void AddListViewItemFromProperties(int x)
        {
            int y;
            lvFVSTables.Items.Add(" ");
            lvFVSTables.Items[lvFVSTables.Items.Count - 1].UseItemStyleForSubItems = false;
            lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_STATUS].ForeColor = Color.White;
            for (y = 1; y <= lvFVSTables.Columns.Count - 1; y++)
            {
                lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems.Add(" ");
            }
            lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_ID].Text =
                m_oCurFVSPrepostSeqNumItem_Collection.Item(x).PrePostSeqNumId.ToString().Trim();
            lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_TABLENAME].Text =
                m_oCurFVSPrepostSeqNumItem_Collection.Item(x).TableName;
            if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).Type == "D")
            {
                lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_TYPE].Text =
                "DEFAULT";
                lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_PACKAGELIST].Text = "NA";
            }
            else
            {
                lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_TYPE].Text =
                "CUSTOM";
                lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_PACKAGELIST].Text =
                    m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxPackageList;
            }
        }
        private void EnableEdit(bool p_bEnable)
        {
             lvFVSTables.Enabled = !p_bEnable;
             //
             //TREE TREATMENT OUTPUT 
             //
             if (lvFVSTables.Enabled == false && lvFVSTables.SelectedItems.Count > 0 &&
                 (lvFVSTables.SelectedItems[0].SubItems[COL_TABLENAME].Text.Trim() == "FVS_CUTLIST" ||
                  lvFVSTables.SelectedItems[0].SubItems[COL_TABLENAME].Text.Trim() == "FVS_ATRTLIST" ||
                  lvFVSTables.SelectedItems[0].SubItems[COL_TABLENAME].Text.Trim() == "FVS_MORTALITY" ||
                  lvFVSTables.SelectedItems[0].SubItems[COL_TABLENAME].Text.Trim() == "FVS_SNAGDET"))
             {
                 cmbPOST1.Enabled = false;
                 cmbPOST2.Enabled = false;
                 cmbPOST3.Enabled = false;
                 cmbPOST4.Enabled = false;
                 cmbPOST1.Text = "Not Used";
                 cmbPOST2.Text = "Not Used";
                 cmbPOST3.Text = "Not Used";
                 cmbPOST4.Text = "Not Used";

             }
             else
             {
                 cmbPOST1.Enabled = p_bEnable;
                 cmbPOST2.Enabled = p_bEnable;
                 cmbPOST3.Enabled = p_bEnable;
                 cmbPOST4.Enabled = p_bEnable;
                
             }
             //
             //RADIO BUTTON SEQNUM OPTIONS
             //
             if (lvFVSTables.SelectedItems.Count > 0)
                 SetSeqNumConfigObjectsEnabled(lblCurTable.Text.Trim().ToUpper());
             else
                 SetSeqNumConfigObjectsEnabled("");

             cmbPRE1.Enabled = p_bEnable;
             cmbPRE2.Enabled = p_bEnable;
             cmbPRE3.Enabled = p_bEnable;
             cmbPRE4.Enabled = p_bEnable;
             btnCancel.Enabled = p_bEnable;
             btnDone.Enabled = p_bEnable;
             cmbDefault.Enabled = p_bEnable;
             btnDefault.Enabled = p_bEnable;
             
           

            if (lvFVSTables.Enabled)
            {
                cmbDefault.Text = "";
                if (lvFVSTables.SelectedItems.Count > 0)
                {
                    if (lvFVSTables.SelectedItems[0].SubItems[COL_TYPE].Text.Trim() == "DEFAULT")
                        btnRemove.Enabled = false;
                    else
                        btnRemove.Enabled = true;

                    btnEdit.Enabled = true;
                    btnNewCustom.Enabled = true;
                    btnSeqNum.Enabled = true;
                    btnSave.Enabled = m_bSave;
                   
                }
                else
                {
                    btnEdit.Enabled = false;
                    btnNewCustom.Enabled = true;
                    btnSave.Enabled = m_bSave;
                    btnRemove.Enabled = false;
                    btnSeqNum.Enabled = false;
                }
                chkPRE1BaseYear.Enabled = false;
                chkPRE2BaseYear.Enabled = false;
                chkPRE3BaseYear.Enabled = false;
                chkPRE4BaseYear.Enabled = false;
                btnAssignPackage.Enabled = false;
                txtPackages.Enabled = false;
                m_cmbFVSStrClassPre1.Enabled = false;
                m_cmbFVSStrClassPre2.Enabled = false;
                m_cmbFVSStrClassPre3.Enabled = false;
                m_cmbFVSStrClassPre4.Enabled = false;
                m_cmbFVSStrClassPost1.Enabled = false;
                m_cmbFVSStrClassPost2.Enabled = false;
                m_cmbFVSStrClassPost3.Enabled = false;
                m_cmbFVSStrClassPost4.Enabled = false;
                
                if (m_cmbTableCustom.Visible)
                {
                    m_cmbTableCustom.Hide();
                    lblCurTable.Show();

                }
            }
            else
            {
                btnRemove.Enabled = false;
                btnSave.Enabled = false;
                btnNewCustom.Enabled = false;
                btnEdit.Enabled = false;
                btnSeqNum.Enabled = false;
                
                if (lblCurType.Text.Trim() == "CUSTOM")
                {
                    btnAssignPackage.Enabled = true;
                    txtPackages.Enabled = true;
                }
                else
                {
                    btnAssignPackage.Enabled = false;
                    txtPackages.Enabled = false;
                }
                chkPRE1BaseYear.Enabled = chkPRE1BaseYear.Visible;
                chkPRE2BaseYear.Enabled = chkPRE2BaseYear.Visible;
                chkPRE3BaseYear.Enabled = chkPRE3BaseYear.Visible;
                chkPRE4BaseYear.Enabled = chkPRE4BaseYear.Visible;
                m_cmbFVSStrClassPre1.Enabled = m_cmbFVSStrClassPre1.Visible;
                m_cmbFVSStrClassPre2.Enabled = m_cmbFVSStrClassPre2.Visible;
                m_cmbFVSStrClassPre3.Enabled = m_cmbFVSStrClassPre1.Visible;
                m_cmbFVSStrClassPre4.Enabled = m_cmbFVSStrClassPre2.Visible;
                m_cmbFVSStrClassPost1.Enabled = m_cmbFVSStrClassPost1.Visible;
                m_cmbFVSStrClassPost2.Enabled = m_cmbFVSStrClassPost2.Visible;
                m_cmbFVSStrClassPost3.Enabled = m_cmbFVSStrClassPost1.Visible;
                m_cmbFVSStrClassPost4.Enabled = m_cmbFVSStrClassPost2.Visible;



                
            }
            
        }

        private void lvFVSTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            
           

            ListViewSelectedIndexChanged();

        }
        private void ListViewSelectedIndexChanged()
        {
            int x;
            if (lvFVSTables.SelectedItems.Count > 0)
            {
                if (lvFVSTables.SelectedItems[0].SubItems[COL_TYPE].Text.Trim() == "DEFAULT")
                    btnRemove.Enabled = false;
                else
                {
                    btnRemove.Enabled = true;
                    if (lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].BackColor==Color.Red)
                    {
                        btnRemove.Text = "Undelete Custom";
                       
                    }
                    else
                    {
                        btnRemove.Text = "Delete Custom";
                    }
                }
                if (btnRemove.Text == "Undelete Custom")
                {
                    btnEdit.Enabled = false;
                    btnSeqNum.Enabled = false;
                }
                else
                {
                    btnEdit.Enabled = true;
                    btnSeqNum.Enabled = true;
                }
                btnNewCustom.Enabled = true;
                btnSave.Enabled = m_bSave;
                UpdateDesc();

            }
            else
            {
                btnEdit.Enabled = false;
                btnNewCustom.Enabled = true;
                btnSave.Enabled = m_bSave;
                btnRemove.Enabled = false;
                btnSeqNum.Enabled = false;
                return;
            }
            for (x = 0; x <= m_oCurFVSPrepostSeqNumItem_Collection.Count - 1; x++)
            {
                if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).PrePostSeqNumId.ToString().Trim() ==
                    lvFVSTables.SelectedItems[0].SubItems[COL_ID].Text.Trim())
                {
                    m_intCurSeqNumItemIndex = x;
                    lblCurId.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).PrePostSeqNumId.ToString();
                    lblCurTable.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).TableName;
                    lblCurType.Text = lvFVSTables.SelectedItems[0].SubItems[COL_TYPE].Text;
                    txtPackages.Text = lvFVSTables.SelectedItems[0].SubItems[COL_PACKAGELIST].Text;
                    cmbPRE1.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle1PreSeqNum;
                    cmbPRE2.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle2PreSeqNum;
                    cmbPRE3.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle3PreSeqNum;
                    cmbPRE4.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle4PreSeqNum;
                    cmbPOST1.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle1PostSeqNum;
                    cmbPOST2.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle2PostSeqNum;
                    cmbPOST3.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle3PostSeqNum;
                    cmbPOST4.Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle4PostSeqNum;

                    rdoSummaryTableSeqNumTemplate.Checked = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).UseSummaryTableSeqNumYN=="Y" ? true : false;
                    rdoCustomTableSeqNumTemplate.Checked = !rdoSummaryTableSeqNumTemplate.Checked;
                    rdoCustomTableSeqNumTemplate.Text = rdoSummaryTableSeqNumTemplate.Text.Replace("FVS_SUMMARY", lblCurTable.Text.Trim());
                    if (lblCurTable.Text.Trim().ToUpper().IndexOf("POTFIRE", 0) >= 0)
                    {
                        m_cmbFVSStrClassPost1.Hide();
                        m_cmbFVSStrClassPost2.Hide();
                        m_cmbFVSStrClassPost3.Hide();
                        m_cmbFVSStrClassPost4.Hide();
                        m_cmbFVSStrClassPre1.Hide();
                        m_cmbFVSStrClassPre2.Hide();
                        m_cmbFVSStrClassPre3.Hide();
                        m_cmbFVSStrClassPre4.Hide();

                        chkPRE1BaseYear.Enabled = false;
                        chkPRE2BaseYear.Enabled = false;
                        chkPRE3BaseYear.Enabled = false;
                        chkPRE4BaseYear.Enabled = false;

                        if (cmbPRE1.Text.Trim() == "1") chkPRE1BaseYear.Show();
                        if (cmbPRE2.Text.Trim() == "1") chkPRE2BaseYear.Show();
                        if (cmbPRE3.Text.Trim() == "1") chkPRE3BaseYear.Show();
                        if (cmbPRE4.Text.Trim() == "1") chkPRE4BaseYear.Show();

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle1PreSeqNumBaseYearYN == "Y")
                            chkPRE1BaseYear.Checked = true;
                        else
                            chkPRE1BaseYear.Checked = false;

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle2PreSeqNumBaseYearYN == "Y")
                            chkPRE2BaseYear.Checked = true;
                        else
                            chkPRE2BaseYear.Checked = false;

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle3PreSeqNumBaseYearYN == "Y")
                            chkPRE3BaseYear.Checked = true;
                        else
                            chkPRE3BaseYear.Checked = false;

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle4PreSeqNumBaseYearYN == "Y")
                            chkPRE4BaseYear.Checked = true;
                        else
                            chkPRE4BaseYear.Checked = false;


                    }
                    else if (lblCurTable.Text.Trim().ToUpper().IndexOf("STRCLASS", 0) >= 0)
                    {
                        chkPRE1BaseYear.Hide();
                        chkPRE2BaseYear.Hide();
                        chkPRE3BaseYear.Hide();
                        chkPRE4BaseYear.Hide();

                        m_cmbFVSStrClassPre1.Enabled = false;
                        m_cmbFVSStrClassPre2.Enabled = false;
                        m_cmbFVSStrClassPre3.Enabled = false;
                        m_cmbFVSStrClassPre4.Enabled = false;
                        m_cmbFVSStrClassPost1.Enabled = false;
                        m_cmbFVSStrClassPost2.Enabled = false;
                        m_cmbFVSStrClassPost3.Enabled = false;
                        m_cmbFVSStrClassPost4.Enabled = false;

                        m_cmbFVSStrClassPre1.Show();
                        m_cmbFVSStrClassPre2.Show();
                        m_cmbFVSStrClassPre3.Show();
                        m_cmbFVSStrClassPre4.Show();
                        m_cmbFVSStrClassPost1.Show();
                        m_cmbFVSStrClassPost2.Show();
                        m_cmbFVSStrClassPost3.Show();
                        m_cmbFVSStrClassPost4.Show();



                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle1PreStrClassBeforeTreeRemovalYN == "Y")
                            m_cmbFVSStrClassPre1.Text = "0 = Before Tree Removal";
                        else
                            m_cmbFVSStrClassPre1.Text = "1 = After Tree Removal";

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle2PreStrClassBeforeTreeRemovalYN == "Y")
                            m_cmbFVSStrClassPre2.Text = "0 = Before Tree Removal";
                        else
                            m_cmbFVSStrClassPre2.Text = "1 = After Tree Removal";

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle3PreStrClassBeforeTreeRemovalYN == "Y")
                            m_cmbFVSStrClassPre3.Text = "0 = Before Tree Removal";
                        else
                            m_cmbFVSStrClassPre3.Text = "1 = After Tree Removal";

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle4PreStrClassBeforeTreeRemovalYN == "Y")
                            m_cmbFVSStrClassPre4.Text = "0 = Before Tree Removal";
                        else
                            m_cmbFVSStrClassPre4.Text = "1 = After Tree Removal";

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle1PostStrClassBeforeTreeRemovalYN == "Y")
                            m_cmbFVSStrClassPost1.Text = "0 = Before Tree Removal";
                        else
                            m_cmbFVSStrClassPost1.Text = "1 = After Tree Removal";

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle2PostStrClassBeforeTreeRemovalYN == "Y")
                            m_cmbFVSStrClassPost2.Text = "0 = Before Tree Removal";
                        else
                            m_cmbFVSStrClassPost2.Text = "1 = After Tree Removal";

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle3PostStrClassBeforeTreeRemovalYN == "Y")
                            m_cmbFVSStrClassPost3.Text = "0 = Before Tree Removal";
                        else
                            m_cmbFVSStrClassPost3.Text = "1 = After Tree Removal";

                        if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxCycle4PostStrClassBeforeTreeRemovalYN == "Y")
                            m_cmbFVSStrClassPost4.Text = "0 = Before Tree Removal";
                        else
                            m_cmbFVSStrClassPost4.Text = "1 = After Tree Removal";
                    }
                    else
                    {
                        chkPRE1BaseYear.Hide();
                        chkPRE2BaseYear.Hide();
                        chkPRE3BaseYear.Hide();
                        chkPRE4BaseYear.Hide();
                        m_cmbFVSStrClassPost1.Hide();
                        m_cmbFVSStrClassPost2.Hide();
                        m_cmbFVSStrClassPost3.Hide();
                        m_cmbFVSStrClassPost4.Hide();
                        m_cmbFVSStrClassPre1.Hide();
                        m_cmbFVSStrClassPre2.Hide();
                        m_cmbFVSStrClassPre3.Hide();
                        m_cmbFVSStrClassPre4.Hide();
                    }

                }
            }
            m_intCurIndex = lvFVSTables.SelectedItems[0].Index;

        }
        private void UpdateDesc()
        {
            switch (lvFVSTables.SelectedItems[0].SubItems[COL_TABLENAME].Text.Trim().ToUpper())
            {
                
                case  "FVS_SUMMARY":
                    if (lvFVSTables.SelectedItems[0].SubItems[COL_TYPE].Text.Trim()=="DEFAULT")
                        txtDesc.Text = "Define DEFAULT PRE-POST sequence number pattern of cycles 1,2,3 and 4 that will be used for all FVS Output tables except FVS_POTFIRE and FVS_CUTLIST tables";
                    else
                        txtDesc.Text = "Assigned RxPackages will use this defined CUSTOM PRE-POST sequence number pattern of cycles 1,2,3 and 4 for all FVS Output tables except FVS_POTFIRE and FVS_CUTLIST tables.";
                    break;
                case "FVS_CUTLIST":
                     if (lvFVSTables.SelectedItems[0].SubItems[COL_TYPE].Text.Trim()=="DEFAULT")
                        txtDesc.Text = "Define DEFAULT FVS_SUMMARY Sequence number pattern for harvested trees for cycles 1,2,3 and 4";
                    else
                        txtDesc.Text = "Assigned RxPackages will use this defined CUSTOM FVS_SUMMARY sequence number pattern for harvested trees for cycles 1,2,3 and 4.";
                    break;
                default: 
                    if (lvFVSTables.SelectedItems[0].SubItems[COL_TYPE].Text.Trim() == "DEFAULT")
                        txtDesc.Text = "Define DEFAULT PRE-POST sequence number pattern of cycles 1,2,3 and 4 for this table only.";
                    else
                        txtDesc.Text = "Assigned RxPackages will use this defined CUSTOM PRE-POST sequence number pattern of cycles 1,2,3 and 4 for this table only.";

                    break;

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvFVSTables.SelectedItems.Count == 0) return;

            EnableEdit(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (m_cmbTableCustom.Visible)
            {
                if (lvFVSTables.SelectedItems.Count == 0)
                {
                    lblCurId.Text = "NA"; lblCurTable.Text = "NA"; lblCurType.Text = "NA";
                }
                else
                {
                    ListViewSelectedIndexChanged();
                }
            }
            EnableEdit(false);
            lvFVSTables.Focus();
            if (lvFVSTables.SelectedItems.Count > 0)
                lvFVSTables.Items[m_intCurIndex].Selected = true;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            AssignDefault(cmbDefault.Text.Trim().ToUpper());
        }
        private void AssignDefault(string p_strOption)
        {
            string strTable = "";
            if (m_cmbTableCustom.Visible) strTable = m_cmbTableCustom.Text.Trim().ToUpper();
            else strTable = lblCurTable.Text.Trim().ToUpper();
            if (p_strOption == "OPTION 1")
            {
                if (strTable.IndexOf("POTFIRE", 0) >= 0)
                {
                    cmbPRE1.Text = "1"; cmbPOST1.Text = "2";
                    cmbPRE2.Text = "4"; cmbPOST2.Text = "5";
                    cmbPRE3.Text = "7"; cmbPOST3.Text = "8";
                    cmbPRE4.Text = "10"; cmbPOST4.Text = "11";

                    chkPRE1BaseYear.Checked = false;
                    chkPRE2BaseYear.Checked = false;
                    chkPRE3BaseYear.Checked = false;
                    chkPRE4BaseYear.Checked = false;
                    chkPRE1BaseYear.Show();
                    chkPRE2BaseYear.Hide();
                    chkPRE3BaseYear.Hide();
                    chkPRE4BaseYear.Hide();
                    
                    
                    
                    
                }
                else if (strTable.Trim().ToUpper() == "FVS_CUTLIST")
                {
                    cmbPRE1.Text = "1"; cmbPOST1.Text = "Not Used";
                    cmbPRE2.Text = "3"; cmbPOST2.Text = "Not Used";
                    cmbPRE3.Text = "5"; cmbPOST3.Text = "Not Used";
                    cmbPRE4.Text = "7"; cmbPOST4.Text = "Not Used";
                    chkPRE1BaseYear.Checked = false;
                    chkPRE2BaseYear.Checked = false;
                    chkPRE3BaseYear.Checked = false;
                    chkPRE4BaseYear.Checked = false;
                    chkPRE1BaseYear.Hide();
                    chkPRE2BaseYear.Hide();
                    chkPRE3BaseYear.Hide();
                    chkPRE4BaseYear.Hide();
                }
                else if (strTable.Trim().ToUpper() == "FVS_STRCLASS")
                {
                    cmbPRE1.Text = "1"; cmbPOST1.Text = "2";
                    cmbPRE2.Text = "4"; cmbPOST2.Text = "5";
                    cmbPRE3.Text = "7"; cmbPOST3.Text = "8";
                    cmbPRE4.Text = "10"; cmbPOST4.Text = "11";
                    m_cmbFVSStrClassPre1.SelectedIndex = 0;
                    m_cmbFVSStrClassPost1.SelectedIndex = 0;
                    m_cmbFVSStrClassPre2.SelectedIndex = 0;
                    m_cmbFVSStrClassPost2.SelectedIndex = 0;
                    m_cmbFVSStrClassPre3.SelectedIndex = 0;
                    m_cmbFVSStrClassPost3.SelectedIndex = 0;
                    m_cmbFVSStrClassPre4.SelectedIndex = 0;
                    m_cmbFVSStrClassPost4.SelectedIndex = 0;

                }
                else
                {
                    cmbPRE1.Text = "2"; cmbPOST1.Text = "3";
                    cmbPRE2.Text = "5"; cmbPOST2.Text = "6";
                    cmbPRE3.Text = "8"; cmbPOST3.Text = "9";
                    cmbPRE4.Text = "11"; cmbPOST4.Text = "12";

                    chkPRE1BaseYear.Checked = false;
                    chkPRE2BaseYear.Checked = false;
                    chkPRE3BaseYear.Checked = false;
                    chkPRE4BaseYear.Checked = false;
                    chkPRE1BaseYear.Hide();
                    chkPRE2BaseYear.Hide();
                    chkPRE3BaseYear.Hide();
                    chkPRE4BaseYear.Hide();
                }
            }
            else if (p_strOption == "OPTION 2")
            {
                if (strTable.IndexOf("POTFIRE", 0) >= 0)
                {
                    cmbPRE1.Text = "1"; cmbPOST1.Text = "1";
                    cmbPRE2.Text = "Not Used"; cmbPOST2.Text = "3";
                    cmbPRE3.Text = "Not Used"; cmbPOST3.Text = "5";
                    cmbPRE4.Text = "Not Used"; cmbPOST4.Text = "7";

                    chkPRE1BaseYear.Checked = true;
                    chkPRE2BaseYear.Checked = false;
                    chkPRE3BaseYear.Checked = false;
                    chkPRE4BaseYear.Checked = false;
                    chkPRE1BaseYear.Show();
                    chkPRE2BaseYear.Hide();
                    chkPRE3BaseYear.Hide();
                    chkPRE4BaseYear.Hide();




                }
                else if (strTable.Trim().ToUpper() == "FVS_CUTLIST")
                {
                    cmbPRE1.Text = "2"; cmbPOST1.Text = "Not Used";
                    cmbPRE2.Text = "4"; cmbPOST2.Text = "Not Used";
                    cmbPRE3.Text = "6"; cmbPOST3.Text = "Not Used";
                    cmbPRE4.Text = "8"; cmbPOST4.Text = "Not Used";
                    chkPRE1BaseYear.Checked = false;
                    chkPRE2BaseYear.Checked = false;
                    chkPRE3BaseYear.Checked = false;
                    chkPRE4BaseYear.Checked = false;
                    chkPRE1BaseYear.Hide();
                    chkPRE2BaseYear.Hide();
                    chkPRE3BaseYear.Hide();
                    chkPRE4BaseYear.Hide();
                }
                else if (strTable.Trim().ToUpper() == "FVS_STRCLASS")
                {
                    cmbPRE1.Text = "1"; cmbPOST1.Text = "2";
                    cmbPRE2.Text = "3"; cmbPOST2.Text = "4";
                    cmbPRE3.Text = "5"; cmbPOST3.Text = "6";
                    cmbPRE4.Text = "7"; cmbPOST4.Text = "8";
                    m_cmbFVSStrClassPre1.SelectedIndex = 0;
                    m_cmbFVSStrClassPost1.SelectedIndex = 0;
                    m_cmbFVSStrClassPre2.SelectedIndex = 0;
                    m_cmbFVSStrClassPost2.SelectedIndex = 0;
                    m_cmbFVSStrClassPre3.SelectedIndex = 0;
                    m_cmbFVSStrClassPost3.SelectedIndex = 0;
                    m_cmbFVSStrClassPre4.SelectedIndex = 0;
                    m_cmbFVSStrClassPost4.SelectedIndex = 0;

                }
                else
                {
                    cmbPRE1.Text = "1"; cmbPOST1.Text = "2";
                    cmbPRE2.Text = "3"; cmbPOST2.Text = "4";
                    cmbPRE3.Text = "5"; cmbPOST3.Text = "6";
                    cmbPRE4.Text = "7"; cmbPOST4.Text = "8";

                    chkPRE1BaseYear.Checked = false;
                    chkPRE2BaseYear.Checked = false;
                    chkPRE3BaseYear.Checked = false;
                    chkPRE4BaseYear.Checked = false;
                    chkPRE1BaseYear.Hide();
                    chkPRE2BaseYear.Hide();
                    chkPRE3BaseYear.Hide();
                    chkPRE4BaseYear.Hide();
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            int x;
            if (m_cmbTableCustom.Visible == false)
            {
                for (x = 0; x <= m_oCurFVSPrepostSeqNumItem_Collection.Count - 1; x++)
                {
                    if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).PrePostSeqNumId.ToString().Trim() ==
                        lvFVSTables.SelectedItems[0].SubItems[COL_ID].Text.Trim())
                    {
                        SaveEditValuesToProperties(x);

                        m_oCurFVSPrepostSeqNumItem_Collection.Item(x).Modified = true;
                        m_bSave = true;
                        EnableEdit(false);
                        lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].BackColor = Color.Green;
                        lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].Text = "m";
                        lvFVSTables.SelectedItems[0].SubItems[COL_PACKAGELIST].Text = m_oCurFVSPrepostSeqNumItem_Collection.Item(x).RxPackageList;
                        lvFVSTables.Focus();
                        lvFVSTables.Items[m_intCurIndex].Selected = true;



                        break;
                    }

                }
            }
            else
            {

                FVSPrePostSeqNumItem oItem = new FVSPrePostSeqNumItem();
                oItem.PrePostSeqNumId = Convert.ToInt32(lblCurId.Text.Trim());
                oItem.TableName = m_cmbTableCustom.Text.Trim();
                oItem.Type = "C";
                oItem.Add = true;
                m_oCurFVSPrepostSeqNumItem_Collection.Add(oItem);
                SaveEditValuesToProperties(m_oCurFVSPrepostSeqNumItem_Collection.Count - 1);
                AddListViewItemFromProperties(m_oCurFVSPrepostSeqNumItem_Collection.Count - 1);

                m_cmbTableCustom.Hide();
                lblCurTable.Show();
                m_bSave = true;
                EnableEdit(false);
               
                m_intCurIndex = lvFVSTables.Items.Count - 1;
                lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_STATUS].BackColor = Color.Blue;
                lvFVSTables.Items[lvFVSTables.Items.Count - 1].Text = "+";
                lvFVSTables.Focus();
                lvFVSTables.Items[lvFVSTables.Items.Count - 1].Selected = true;

                lvFVSTables.SelectedItems[0].EnsureVisible();
                
                

            }
        }
        private void SaveEditValuesToProperties(FVSPrePostSeqNumItem p_oItem)
        {
            if (p_oItem.TableName.Trim().Length == 0)
            {
                if (m_cmbTableCustom.Visible)
                {
                    p_oItem.TableName = m_cmbTableCustom.Text.Trim().ToUpper();
                }
                else
                {
                    p_oItem.TableName = lblCurTable.Text.Trim().ToUpper();
                }
            }
            if (cmbPRE1.Text.Trim().Length > 0)
                p_oItem.RxCycle1PreSeqNum = cmbPRE1.Text.Trim();
            else
                p_oItem.RxCycle1PreSeqNum = "Not Used";

            if (cmbPRE2.Text.Trim().Length > 0)
                p_oItem.RxCycle2PreSeqNum = cmbPRE2.Text.Trim();
            else
                p_oItem.RxCycle2PreSeqNum = "Not Used";

            if (cmbPRE3.Text.Trim().Length > 0)
                p_oItem.RxCycle3PreSeqNum = cmbPRE3.Text.Trim();
            else
                p_oItem.RxCycle3PreSeqNum = "Not Used";

            if (cmbPRE4.Text.Trim().Length > 0)
                p_oItem.RxCycle4PreSeqNum = cmbPRE4.Text.Trim();
            else
                p_oItem.RxCycle4PreSeqNum = "Not Used";

            if (cmbPOST1.Text.Trim().Length > 0)
                p_oItem.RxCycle1PostSeqNum = cmbPOST1.Text.Trim();
            else
                p_oItem.RxCycle1PostSeqNum = "Not Used";

            if (cmbPOST2.Text.Trim().Length > 0)
                p_oItem.RxCycle2PostSeqNum = cmbPOST2.Text.Trim();
            else
                p_oItem.RxCycle2PostSeqNum = "Not Used";

            if (cmbPOST3.Text.Trim().Length > 0)
                p_oItem.RxCycle3PostSeqNum = cmbPOST3.Text.Trim();
            else
                p_oItem.RxCycle3PostSeqNum = "Not Used";

            if (cmbPOST4.Text.Trim().Length > 0)
                p_oItem.RxCycle4PostSeqNum = cmbPOST4.Text.Trim();
            else
                p_oItem.RxCycle4PostSeqNum = "Not Used";

            if (chkPRE1BaseYear.Checked)
                p_oItem.RxCycle1PreSeqNumBaseYearYN = "Y";
            else
                p_oItem.RxCycle1PreSeqNumBaseYearYN = "N";

            if (chkPRE2BaseYear.Checked)
                p_oItem.RxCycle2PreSeqNumBaseYearYN = "Y";
            else
                p_oItem.RxCycle2PreSeqNumBaseYearYN = "N";

            if (chkPRE3BaseYear.Checked)
                p_oItem.RxCycle3PreSeqNumBaseYearYN = "Y";
            else
                p_oItem.RxCycle3PreSeqNumBaseYearYN = "N";

            if (chkPRE4BaseYear.Checked)
                p_oItem.RxCycle4PreSeqNumBaseYearYN = "Y";
            else
                p_oItem.RxCycle4PreSeqNumBaseYearYN = "N";

            if (m_cmbFVSStrClassPre1.Text.Trim().ToUpper() == "1 = AFTER TREE REMOVAL")
            {
                p_oItem.RxCycle1PreStrClassBeforeTreeRemovalYN = "N";
            }
            else
            {
                p_oItem.RxCycle1PreStrClassBeforeTreeRemovalYN = "Y";
            }
            if (m_cmbFVSStrClassPre2.Text.Trim().ToUpper() == "1 = AFTER TREE REMOVAL")
            {
                p_oItem.RxCycle2PreStrClassBeforeTreeRemovalYN = "N";
            }
            else
            {
                p_oItem.RxCycle2PreStrClassBeforeTreeRemovalYN = "Y";
            }
            if (m_cmbFVSStrClassPre3.Text.Trim().ToUpper() == "1 = AFTER TREE REMOVAL")
            {
                p_oItem.RxCycle3PreStrClassBeforeTreeRemovalYN = "N";
            }
            else
            {
                p_oItem.RxCycle3PreStrClassBeforeTreeRemovalYN = "Y";
            }
            if (m_cmbFVSStrClassPre4.Text.Trim().ToUpper() == "1 = AFTER TREE REMOVAL")
            {
                p_oItem.RxCycle4PreStrClassBeforeTreeRemovalYN = "N";
            }
            else
            {
                p_oItem.RxCycle4PreStrClassBeforeTreeRemovalYN = "Y";
            }
            if (m_cmbFVSStrClassPost1.Text.Trim().ToUpper() == "1 = AFTER TREE REMOVAL")
            {
                p_oItem.RxCycle1PostStrClassBeforeTreeRemovalYN = "N";
            }
            else
            {
                p_oItem.RxCycle1PostStrClassBeforeTreeRemovalYN = "Y";
            }
            if (m_cmbFVSStrClassPost2.Text.Trim().ToUpper() == "1 = AFTER TREE REMOVAL")
            {
                p_oItem.RxCycle2PostStrClassBeforeTreeRemovalYN = "N";
            }
            else
            {
                p_oItem.RxCycle2PostStrClassBeforeTreeRemovalYN = "Y";
            }
            if (m_cmbFVSStrClassPost3.Text.Trim().ToUpper() == "1 = AFTER TREE REMOVAL")
            {
                p_oItem.RxCycle3PostStrClassBeforeTreeRemovalYN = "N";
            }
            else
            {
                p_oItem.RxCycle3PostStrClassBeforeTreeRemovalYN = "Y";
            }
            if (m_cmbFVSStrClassPost4.Text.Trim().ToUpper() == "1 = AFTER TREE REMOVAL")
            {
                p_oItem.RxCycle4PostStrClassBeforeTreeRemovalYN = "N";
            }
            else
            {
                p_oItem.RxCycle4PostStrClassBeforeTreeRemovalYN = "Y";
            }
            p_oItem.UseSummaryTableSeqNumYN = rdoSummaryTableSeqNumTemplate.Checked ? "Y" : "N";
            p_oItem.RxPackageList = txtPackages.Text;
        }

        private void SaveEditValuesToProperties(int x)
        {
            SaveEditValuesToProperties(m_oCurFVSPrepostSeqNumItem_Collection.Item(x));

        }
        private void savevalues()
        {
            int x, y, z;
            string strValues = "";
            bool bDelete = false;
            m_oAdo.OpenConnection(m_oAdo.getMDBConnString(frmMain.g_oFrmMain.frmProject.uc_project1.txtRootDirectory.Text.Trim() + "\\" + Tables.FVS.DefaultFVSPrePostSeqNumTableDbFile, "", ""));
            if (m_oAdo.m_intError == 0)
            {

                for (x = 0; x <= lvFVSTables.Items.Count - 1; x++)
                {
                    //
                    //DELETE
                    //
                    if (lvFVSTables.Items[x].SubItems[COL_STATUS].BackColor == Color.Red)
                    {
                        for (y = 0; y <= m_oCurFVSPrepostSeqNumItem_Collection.Count - 1; y++)
                        {
                            if (m_oCurFVSPrepostSeqNumItem_Collection.Item(y).PrePostSeqNumId.ToString().Trim() ==
                               lvFVSTables.Items[x].SubItems[COL_ID].Text.Trim())
                            {
                                m_oAdo.m_strSQL = "DELETE FROM " + Tables.FVS.DefaultFVSPrePostSeqNumTable + " " +
                                                 "WHERE PREPOST_SEQNUM_ID=" +
                                                 m_oCurFVSPrepostSeqNumItem_Collection.Item(y).PrePostSeqNumId.ToString().Trim();
                                m_oAdo.SqlNonQuery(m_oAdo.m_OleDbConnection, m_oAdo.m_strSQL);
                                if (m_oAdo.m_intError == 0)
                                {

                                    bDelete = true;

                                }
                                m_oAdo.m_strSQL = "DELETE FROM " + Tables.FVS.DefaultFVSPrePostSeqNumRxPackageAssgnTable + " " +
                                                 "WHERE PREPOST_SEQNUM_ID=" +
                                                 m_oCurFVSPrepostSeqNumItem_Collection.Item(y).PrePostSeqNumId.ToString().Trim();
                                m_oAdo.SqlNonQuery(m_oAdo.m_OleDbConnection, m_oAdo.m_strSQL);


                            }
                        }
                    }
                    //
                    //INSERT
                    //
                    else if (lvFVSTables.Items[x].SubItems[COL_STATUS].BackColor == Color.Blue)
                    {
                        for (y = 0; y <= m_oCurFVSPrepostSeqNumItem_Collection.Count - 1; y++)
                        {
                            if (m_oCurFVSPrepostSeqNumItem_Collection.Item(y).PrePostSeqNumId.ToString().Trim() ==
                               lvFVSTables.Items[x].SubItems[COL_ID].Text.Trim())
                            {
                                strValues = "";
                                FVSPrePostSeqNumItem oItem = m_oCurFVSPrepostSeqNumItem_Collection.Item(y);
                                if (m_oCurFVSPrepostSeqNumItem_Collection.Item(y).Add)
                                {
                                    //INSERT
                                    strValues = oItem.PrePostSeqNumId.ToString().Trim() + ",";
                                    strValues = strValues + "'" + oItem.TableName + "',";
                                    strValues = strValues + "'" + oItem.Type + "',";
                                    if (oItem.RxCycle1PreSeqNum.Trim().Length > 0 &&
                                        oItem.RxCycle1PreSeqNum.Trim().ToUpper() != "NOT USED")
                                    {
                                        strValues = strValues + oItem.RxCycle1PreSeqNum.Trim() + ",";
                                    }
                                    else
                                    {
                                        strValues = strValues + "null,";
                                    }
                                    if (oItem.RxCycle1PostSeqNum.Trim().Length > 0 &&
                                        oItem.RxCycle1PostSeqNum.Trim().ToUpper() != "NOT USED")
                                    {
                                        strValues = strValues + oItem.RxCycle1PostSeqNum.Trim() + ",";
                                    }
                                    else
                                    {
                                        strValues = strValues + "null,";
                                    }
                                    if (oItem.RxCycle2PreSeqNum.Trim().Length > 0 &&
                                       oItem.RxCycle2PreSeqNum.Trim().ToUpper() != "NOT USED")
                                    {
                                        strValues = strValues + oItem.RxCycle2PreSeqNum.Trim() + ",";
                                    }
                                    else
                                    {
                                        strValues = strValues + "null,";
                                    }
                                    if (oItem.RxCycle2PostSeqNum.Trim().Length > 0 &&
                                        oItem.RxCycle2PostSeqNum.Trim().ToUpper() != "NOT USED")
                                    {
                                        strValues = strValues + oItem.RxCycle2PostSeqNum.Trim() + ",";
                                    }
                                    else
                                    {
                                        strValues = strValues + "null,";
                                    }
                                    if (oItem.RxCycle3PreSeqNum.Trim().Length > 0 &&
                                       oItem.RxCycle3PreSeqNum.Trim().ToUpper() != "NOT USED")
                                    {
                                        strValues = strValues + oItem.RxCycle3PreSeqNum.Trim() + ",";
                                    }
                                    else
                                    {
                                        strValues = strValues + "null,";
                                    }
                                    if (oItem.RxCycle3PostSeqNum.Trim().Length > 0 &&
                                        oItem.RxCycle3PostSeqNum.Trim().ToUpper() != "NOT USED")
                                    {
                                        strValues = strValues + oItem.RxCycle3PostSeqNum.Trim() + ",";
                                    }
                                    else
                                    {
                                        strValues = strValues + "null,";
                                    }
                                    if (oItem.RxCycle4PreSeqNum.Trim().Length > 0 &&
                                       oItem.RxCycle4PreSeqNum.Trim().ToUpper() != "NOT USED")
                                    {
                                        strValues = strValues + oItem.RxCycle4PreSeqNum.Trim() + ",";
                                    }
                                    else
                                    {
                                        strValues = strValues + "null,";
                                    }
                                    if (oItem.RxCycle4PostSeqNum.Trim().Length > 0 &&
                                        oItem.RxCycle4PostSeqNum.Trim().ToUpper() != "NOT USED")
                                    {
                                        strValues = strValues + oItem.RxCycle4PostSeqNum.Trim() + ",";
                                    }
                                    else
                                    {
                                        strValues = strValues + "null,";
                                    }
                                    strValues = strValues + "'" + oItem.RxCycle1PreSeqNumBaseYearYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle2PreSeqNumBaseYearYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle3PreSeqNumBaseYearYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle4PreSeqNumBaseYearYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle1PreStrClassBeforeTreeRemovalYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle1PostStrClassBeforeTreeRemovalYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle2PreStrClassBeforeTreeRemovalYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle2PostStrClassBeforeTreeRemovalYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle3PreStrClassBeforeTreeRemovalYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle3PostStrClassBeforeTreeRemovalYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle4PreStrClassBeforeTreeRemovalYN + "',";
                                    strValues = strValues + "'" + oItem.RxCycle4PostStrClassBeforeTreeRemovalYN + "',";

                                    strValues = strValues + "'" + oItem.UseSummaryTableSeqNumYN + "'";

                                    m_oAdo.m_strSQL = "INSERT INTO " + Tables.FVS.DefaultFVSPrePostSeqNumTable + " " +
                                                      "(" + m_strColumnList + ") VALUES " +
                                                      "(" + strValues + ")";
                                    m_oAdo.SqlNonQuery(m_oAdo.m_OleDbConnection, m_oAdo.m_strSQL);
                                    if (m_oAdo.m_intError == 0)
                                    {
                                        //if custom and rxpackages assigned
                                        if (oItem.Type == "C" && oItem.RxPackageList.Trim().Length > 0)
                                        {
                                            string[] strArray = frmMain.g_oUtils.ConvertListToArray(oItem.RxPackageList, ",");
                                            for (z = 0; z <= strArray.Length - 1; z++)
                                            {
                                                m_oAdo.m_strSQL = "INSERT INTO " + Tables.FVS.DefaultFVSPrePostSeqNumRxPackageAssgnTable + " " +
                                                      "(RXPACKAGE,PREPOST_SEQNUM_ID) VALUES " +
                                                      "('" + strArray[z].Trim() + "'," + oItem.PrePostSeqNumId.ToString().Trim() + ")";
                                                m_oAdo.SqlNonQuery(m_oAdo.m_OleDbConnection, m_oAdo.m_strSQL);
                                                if (m_oAdo.m_intError != 0)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (m_oAdo.m_intError == 0)
                                    {
                                        lvFVSTables.Items[x].SubItems[COL_STATUS].BackColor = Color.White;
                                        lvFVSTables.Items[x].SubItems[COL_STATUS].Text = "";
                                        m_oCurFVSPrepostSeqNumItem_Collection.Item(y).Add = false;
                                    }
                                    break;
                                }
                            }
                        }

                    }
                    //
                    //UPDATE
                    //
                    else if (lvFVSTables.Items[x].SubItems[COL_STATUS].BackColor == Color.Green)
                    {
                        for (y = 0; y <= m_oCurFVSPrepostSeqNumItem_Collection.Count - 1; y++)
                        {
                            if (m_oCurFVSPrepostSeqNumItem_Collection.Item(y).PrePostSeqNumId.ToString().Trim() ==
                               lvFVSTables.Items[x].SubItems[COL_ID].Text)
                            {
                                strValues = "";
                                FVSPrePostSeqNumItem oItem = m_oCurFVSPrepostSeqNumItem_Collection.Item(y);


                                //UPDATE
                                if (oItem.RxCycle1PreSeqNum.Trim().Length > 0 &&
                                    oItem.RxCycle1PreSeqNum.Trim().ToUpper() != "NOT USED")
                                {
                                    strValues = strValues + "RXCYCLE1_PRE_SEQNUM=" + oItem.RxCycle1PreSeqNum.Trim() + ",";
                                }
                                else
                                {
                                    strValues = strValues + "RXCYCLE1_PRE_SEQNUM=null,";
                                }
                                if (oItem.RxCycle2PreSeqNum.Trim().Length > 0 &&
                                   oItem.RxCycle2PreSeqNum.Trim().ToUpper() != "NOT USED")
                                {
                                    strValues = strValues + "RXCYCLE2_PRE_SEQNUM=" + oItem.RxCycle2PreSeqNum.Trim() + ",";
                                }
                                else
                                {
                                    strValues = strValues + "RXCYCLE2_PRE_SEQNUM=null,";
                                }
                                if (oItem.RxCycle3PreSeqNum.Trim().Length > 0 &&
                                  oItem.RxCycle3PreSeqNum.Trim().ToUpper() != "NOT USED")
                                {
                                    strValues = strValues + "RXCYCLE3_PRE_SEQNUM=" + oItem.RxCycle3PreSeqNum.Trim() + ",";
                                }
                                else
                                {
                                    strValues = strValues + "RXCYCLE3_PRE_SEQNUM=null,";
                                }
                                if (oItem.RxCycle4PreSeqNum.Trim().Length > 0 &&
                                  oItem.RxCycle4PreSeqNum.Trim().ToUpper() != "NOT USED")
                                {
                                    strValues = strValues + "RXCYCLE4_PRE_SEQNUM=" + oItem.RxCycle4PreSeqNum.Trim() + ",";
                                }
                                else
                                {
                                    strValues = strValues + "RXCYCLE4_PRE_SEQNUM=null,";
                                }
                                if (oItem.RxCycle1PostSeqNum.Trim().Length > 0 &&
                                    oItem.RxCycle1PostSeqNum.Trim().ToUpper() != "NOT USED")
                                {
                                    strValues = strValues + "RXCYCLE1_POST_SEQNUM=" + oItem.RxCycle1PostSeqNum.Trim() + ",";
                                }
                                else
                                {
                                    strValues = strValues + "RXCYCLE1_POST_SEQNUM=null,";
                                }
                                if (oItem.RxCycle2PostSeqNum.Trim().Length > 0 &&
                                   oItem.RxCycle2PostSeqNum.Trim().ToUpper() != "NOT USED")
                                {
                                    strValues = strValues + "RXCYCLE2_POST_SEQNUM=" + oItem.RxCycle2PostSeqNum.Trim() + ",";
                                }
                                else
                                {
                                    strValues = strValues + "RXCYCLE2_POST_SEQNUM=null,";
                                }
                                if (oItem.RxCycle3PostSeqNum.Trim().Length > 0 &&
                                  oItem.RxCycle3PostSeqNum.Trim().ToUpper() != "NOT USED")
                                {
                                    strValues = strValues + "RXCYCLE3_POST_SEQNUM=" + oItem.RxCycle3PostSeqNum.Trim() + ",";
                                }
                                else
                                {
                                    strValues = strValues + "RXCYCLE3_POST_SEQNUM=null,";
                                }
                                if (oItem.RxCycle4PostSeqNum.Trim().Length > 0 &&
                                    oItem.RxCycle4PostSeqNum.Trim().ToUpper() != "NOT USED")
                                {
                                    strValues = strValues + "RXCYCLE4_POST_SEQNUM=" + oItem.RxCycle4PostSeqNum.Trim() + ",";
                                }
                                else
                                {
                                    strValues = strValues + "RXCYCLE4_POST_SEQNUM=null,";
                                }
                                strValues = strValues + "RXCYCLE1_PRE_BASEYR_YN='" + oItem.RxCycle1PreSeqNumBaseYearYN + "',";
                                strValues = strValues + "RXCYCLE2_PRE_BASEYR_YN='" + oItem.RxCycle2PreSeqNumBaseYearYN + "',";
                                strValues = strValues + "RXCYCLE3_PRE_BASEYR_YN='" + oItem.RxCycle3PreSeqNumBaseYearYN + "',";
                                strValues = strValues + "RXCYCLE4_PRE_BASEYR_YN='" + oItem.RxCycle4PreSeqNumBaseYearYN + "',";
                                strValues = strValues + "RXCYCLE1_PRE_BEFORECUT_YN='" + oItem.RxCycle1PreStrClassBeforeTreeRemovalYN + "',";
                                strValues = strValues + "RXCYCLE1_POST_BEFORECUT_YN='" + oItem.RxCycle1PostStrClassBeforeTreeRemovalYN + "',";
                                strValues = strValues + "RXCYCLE2_PRE_BEFORECUT_YN='" + oItem.RxCycle2PreStrClassBeforeTreeRemovalYN + "',";
                                strValues = strValues + "RXCYCLE2_POST_BEFORECUT_YN='" + oItem.RxCycle2PostStrClassBeforeTreeRemovalYN + "',";
                                strValues = strValues + "RXCYCLE3_PRE_BEFORECUT_YN='" + oItem.RxCycle3PreStrClassBeforeTreeRemovalYN + "',";
                                strValues = strValues + "RXCYCLE3_POST_BEFORECUT_YN='" + oItem.RxCycle3PostStrClassBeforeTreeRemovalYN + "',";
                                strValues = strValues + "RXCYCLE4_PRE_BEFORECUT_YN='" + oItem.RxCycle4PreStrClassBeforeTreeRemovalYN + "',";
                                strValues = strValues + "RXCYCLE4_POST_BEFORECUT_YN='" + oItem.RxCycle4PostStrClassBeforeTreeRemovalYN + "',";
                                strValues = strValues + "USE_SUMMARY_TABLE_SEQNUM_YN='" + oItem.UseSummaryTableSeqNumYN + "'";

                                m_oAdo.m_strSQL = "UPDATE " + Tables.FVS.DefaultFVSPrePostSeqNumTable + " " +
                                                  "SET " + strValues + " " +
                                                  "WHERE PREPOST_SEQNUM_ID=" + oItem.PrePostSeqNumId.ToString().Trim();

                                m_oAdo.SqlNonQuery(m_oAdo.m_OleDbConnection, m_oAdo.m_strSQL);
                                if (m_oAdo.m_intError == 0)
                                {
                                    //if custom 
                                    if (oItem.Type == "C")
                                    {
                                        //first delete all records for id
                                        m_oAdo.m_strSQL = "DELETE FROM " + Tables.FVS.DefaultFVSPrePostSeqNumRxPackageAssgnTable + " " +
                                                          "WHERE PREPOST_SEQNUM_ID=" + oItem.PrePostSeqNumId.ToString().Trim();
                                        m_oAdo.SqlNonQuery(m_oAdo.m_OleDbConnection, m_oAdo.m_strSQL);
                                        if (m_oAdo.m_intError == 0)
                                        {
                                            string[] strArray = frmMain.g_oUtils.ConvertListToArray(oItem.RxPackageList, ",");
                                            for (z = 0; z <= strArray.Length - 1; z++)
                                            {
                                                m_oAdo.m_strSQL = "INSERT INTO " + Tables.FVS.DefaultFVSPrePostSeqNumRxPackageAssgnTable + " " +
                                                      "(RXPACKAGE,PREPOST_SEQNUM_ID) VALUES " +
                                                      "('" + strArray[z].Trim() + "'," + oItem.PrePostSeqNumId.ToString().Trim() + ")";
                                                m_oAdo.SqlNonQuery(m_oAdo.m_OleDbConnection, m_oAdo.m_strSQL);
                                                if (m_oAdo.m_intError != 0)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }


                                if (m_oAdo.m_intError == 0)
                                    lvFVSTables.Items[x].SubItems[COL_STATUS].BackColor = Color.White;
                                lvFVSTables.Items[x].SubItems[COL_STATUS].Text = "";
                                lvFVSTables.Items[x].SubItems[COL_ID].Text = oItem.PrePostSeqNumId.ToString().Trim();
                                break;

                            }
                        }
                    }
                }
                m_oAdo.CloseConnection(m_oAdo.m_OleDbConnection);
                if (m_oAdo.m_intError == 0)
                {
                    if (bDelete) loadvalues();
                    MessageBox.Show("Saved", "FIA Biosum");
                    m_bSave = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Done, but with errors", "FIA Biosum");
                }
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            savevalues();
        }

        private void btnNewCustom_Click(object sender, EventArgs e)
        {
            //
            //get the next unique id available
            //
            string strId = lvFVSTables.Items[lvFVSTables.Items.Count - 1].SubItems[COL_ID].Text.Trim();
            strId = Convert.ToString(Convert.ToInt32(strId) + 1);
            lblCurId.Text = strId;
            lblCurTable.Hide();
            m_cmbTableCustom.Text = "<Select or Enter Table Name>";
            m_cmbTableCustom.Show();
            lblCurType.Text = "CUSTOM";
            EnableEdit(true);
            chkPRE1BaseYear.Checked = false; chkPRE1BaseYear.Hide();
            chkPRE2BaseYear.Checked = false; chkPRE2BaseYear.Hide();
            chkPRE3BaseYear.Checked = false; chkPRE3BaseYear.Hide();
            chkPRE4BaseYear.Checked = false; chkPRE4BaseYear.Hide();
            btnAssignPackage.Enabled = true;
            txtDesc.Text = "";
            this.txtPackages.Text = "";

        }

        private void cmbPRE1_SelectedIndexChanged(object sender, EventArgs e)
        {

            SetPotFireBaseYearVisible(cmbPRE1, chkPRE1BaseYear);
        }

        private void cmbPRE2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPotFireBaseYearVisible(cmbPRE2, chkPRE2BaseYear);
        }

        private void cmbPRE3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPotFireBaseYearVisible(cmbPRE3, chkPRE3BaseYear);
        }

        private void cmbPRE4_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPotFireBaseYearVisible(cmbPRE4, chkPRE4BaseYear);
        }
        private void m_cmbTableCustom_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                SetPotFireBaseYearVisible(cmbPRE1, chkPRE1BaseYear);
                SetPotFireBaseYearVisible(cmbPRE2, chkPRE2BaseYear);
                SetPotFireBaseYearVisible(cmbPRE3, chkPRE3BaseYear);
                SetPotFireBaseYearVisible(cmbPRE4, chkPRE4BaseYear);
                SetStrClassVisible();
                SetHarvestTreeInputOptions();
                SetSeqNumConfigObjectsEnabled(m_cmbTableCustom.Text.Trim().ToUpper());
                
            
        }
        private void SetStrClassVisible()
        {
            string strTable = "";
            if (m_cmbTableCustom.Visible) strTable = m_cmbTableCustom.Text.Trim().ToUpper();
            else strTable = lblCurTable.Text.Trim().ToUpper();
            if (strTable.IndexOf("FVS_STRCLASS", 0) >= 0)
            {
                m_cmbFVSStrClassPre1.Show();
                m_cmbFVSStrClassPre2.Show();
                m_cmbFVSStrClassPre3.Show();
                m_cmbFVSStrClassPre4.Show();
                m_cmbFVSStrClassPost1.Show();
                m_cmbFVSStrClassPost2.Show();
                m_cmbFVSStrClassPost3.Show();
                m_cmbFVSStrClassPost4.Show();
            }
            else
            {
                m_cmbFVSStrClassPre1.Hide();
                m_cmbFVSStrClassPre2.Hide();
                m_cmbFVSStrClassPre3.Hide();
                m_cmbFVSStrClassPre4.Hide();
                m_cmbFVSStrClassPost1.Hide();
                m_cmbFVSStrClassPost2.Hide();
                m_cmbFVSStrClassPost3.Hide();
                m_cmbFVSStrClassPost4.Hide();
            }
        }
        private void SetSeqNumConfigObjectsEnabled(string p_strTable)
        {
            if (lvFVSTables.Enabled)
            {
                btnSeqNumEdit.Enabled = false;
                rdoCustomTableSeqNumTemplate.Enabled = false;
                rdoSummaryTableSeqNumTemplate.Enabled = false;
            }
            else
            {
                if (p_strTable.Trim().Length > 0)
                {
                    if (p_strTable == "FVS_CUTLIST" ||
                        p_strTable == "FVS_ATRTLIST" ||
                        p_strTable == "FVS_MORTALITY" ||
                        p_strTable == "FVS_SNAGDET" ||
                        p_strTable == "FVS_SUMMARY")
                    {
                        //always use the FVS_SUMMARY
                        rdoCustomTableSeqNumTemplate.Enabled = false;
                        rdoSummaryTableSeqNumTemplate.Enabled = false;
                        rdoSummaryTableSeqNumTemplate.Checked = true;
                    }
                    else if (p_strTable == "FVS_POTFIRE" || p_strTable == "FVS_POTFIRE_EAST")
                    {
                        rdoCustomTableSeqNumTemplate.Enabled = true;
                        rdoSummaryTableSeqNumTemplate.Enabled = false;
                    }
                    else
                    {
                        //option to use FVS Outputs table seqnum or use FVS_SUMMARY
                        rdoCustomTableSeqNumTemplate.Enabled = true;
                        rdoSummaryTableSeqNumTemplate.Enabled = true;
                    }
                    rdoCustomTableSeqNumTemplate.Text = rdoSummaryTableSeqNumTemplate.Text.Replace("FVS_SUMMARY", p_strTable);
                    btnSeqNumEdit.Enabled = true;
                }
                else
                {
                    btnSeqNumEdit.Enabled = false;
                    rdoSummaryTableSeqNumTemplate.Enabled = false;
                    rdoCustomTableSeqNumTemplate.Enabled = false;
                }
            }
        }
        private void SetPotFireBaseYearVisible(ComboBox p_oCombo, CheckBox p_oCheckBox)
        {
            if (p_oCombo.Text.Trim() == "1")
            {
                string strTable = "";
                if (m_cmbTableCustom.Visible) strTable = m_cmbTableCustom.Text.Trim().ToUpper();
                else strTable = lblCurTable.Text.Trim().ToUpper();
                if (strTable.IndexOf("POTFIRE", 0) >= 0)
                {
                    p_oCheckBox.Enabled = true;
                    p_oCheckBox.Show();
                }
                else
                {
                    if (p_oCheckBox.Checked) p_oCheckBox.Checked = false;
                    if (p_oCheckBox.Visible) p_oCheckBox.Hide();
                }
            }
            else
            {
                if (p_oCheckBox.Checked) p_oCheckBox.Checked = false;
                if (p_oCheckBox.Visible) p_oCheckBox.Hide();
            }
        }
        private void SetHarvestTreeInputOptions()
        {
            if (m_cmbTableCustom.Text.Trim().ToUpper() == "FVS_CUTLIST")
            {
                cmbPOST1.Text = "Not Used";
                cmbPOST2.Text = "Not Used";
                cmbPOST3.Text = "Not Used";
                cmbPOST4.Text = "Not Used";

                cmbPOST1.Enabled = false;
                cmbPOST2.Enabled = false;
                cmbPOST3.Enabled = false;
                cmbPOST4.Enabled = false;
            }
            else
            {
                cmbPOST1.Text = "";
                cmbPOST2.Text = "";
                cmbPOST3.Text = "";
                cmbPOST4.Text = "";

                cmbPOST1.Enabled = true;
                cmbPOST2.Enabled = true;
                cmbPOST3.Enabled = true;
                cmbPOST4.Enabled = true;
            }
        }

        private void cmbPRE1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbPRE2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbPRE3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbPRE4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbPOST1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbPOST2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbPOST3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbPOST4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbDefault_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtPackages_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int x;
            for (x = 0; x <= m_oCurFVSPrepostSeqNumItem_Collection.Count - 1; x++)
            {
                if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).PrePostSeqNumId.ToString().Trim() ==
                    lvFVSTables.SelectedItems[0].SubItems[COL_ID].Text.Trim())
                {
                    break;
                }
            }
            if (btnRemove.Text == "Undelete Custom")
            {
                //remove deletion mark
                if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).Add)
                {
                    lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].BackColor = Color.Blue;
                    lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].Text = "+";
                }
                else if (m_oCurFVSPrepostSeqNumItem_Collection.Item(x).Modified)
                {
                    lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].BackColor = Color.Green;
                    lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].Text = "m";
                }
                else
                {
                    lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].BackColor = Color.White;
                    lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].Text = "";
                }
                m_oCurFVSPrepostSeqNumItem_Collection.Item(x).Delete = false;
                btnRemove.Text = "Delete Custom";
                btnEdit.Enabled = true;
                btnSeqNum.Enabled = true;
            }
            else
            {
                //mark for deletion
                lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].BackColor = Color.Red;
                lvFVSTables.SelectedItems[0].SubItems[COL_STATUS].Text = "x";
                m_oCurFVSPrepostSeqNumItem_Collection.Item(x).Delete = true;
                btnRemove.Text = "Undelete Custom";
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnSeqNum.Enabled = false;
                

            }
               
                

        }

        private void btnAssignPackage_Click(object sender, EventArgs e)
        {
            int x;
            string strList = "";
            if (m_oRxPackageItem_Collection == null)
            {
               m_oRxPackageItem_Collection = new RxPackageItem_Collection();
               Queries oQueries = new Queries();
               oQueries.m_oFvs.LoadDatasource = true;
               oQueries.LoadDatasources(true);
               ado_data_access oAdo = new ado_data_access();
               oAdo.OpenConnection(oAdo.getMDBConnString(oQueries.m_strTempDbFile, "", ""));
               this.m_oRxTools.LoadAllRxPackageItemsFromTableIntoRxPackageCollection(oAdo,
                                                                                      oAdo.m_OleDbConnection,
                                                                                      oQueries,
                                                                                      m_oRxPackageItem_Collection);
               oAdo.CloseConnection(oAdo.m_OleDbConnection);
            }

            frmDialog frmTemp = new frmDialog();
            frmTemp.Text = "FVS Output";
            frmTemp.uc_previous_expressions1.Visible = true;
            if (txtPackages.Text.Trim().Length > 0 && txtPackages.Text.Trim() != "NA")
                frmTemp.uc_previous_expressions1.LoadRxPackageItemCollection(m_oRxPackageItem_Collection,frmMain.g_oUtils.ConvertListToArray(txtPackages.Text.Trim(),","));
            else
                frmTemp.uc_previous_expressions1.LoadRxPackageItemCollection(m_oRxPackageItem_Collection, null);
            DialogResult result = frmTemp.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.txtPackages.Text = "";
                for (x = 0; x <= frmTemp.uc_previous_expressions1.listView1.CheckedItems.Count - 1; x++)
                {
                    strList = strList + frmTemp.uc_previous_expressions1.listView1.CheckedItems[x].SubItems[1].Text.Trim() + ",";

                }
                if (strList.Trim().Length > 0) strList = strList.Substring(0, strList.Length - 1);
                txtPackages.Text = strList;

            }

            

            
        }

        private void btnSeqNum_Click(object sender, EventArgs e)
        {
            
            int x;
            if (lvFVSTables.SelectedItems.Count == 0) return;

            ViewSeqNumAssignments(lvFVSTables.SelectedItems[0].SubItems[COL_TABLENAME].Text.Trim().ToUpper(),m_oCurFVSPrepostSeqNumItem_Collection.Item(m_intCurSeqNumItemIndex));

           
        }
        private void ViewSeqNumAssignments(string p_strTable,FVSPrePostSeqNumItem p_oItem)
        {
            int x;
            string strPotFireBaseYearFile = "";
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Title = "Open FVS Output Access File";
            OpenFileDialog1.Filter = "MS Access Database File (*.MDB,*.MDE,*.ACCDB) |*.mdb;*.mde;*.accdb";

            DialogResult result = OpenFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (OpenFileDialog1.FileName.Trim().Length > 0)
                {
                    
                    dao_data_access oDao = new dao_data_access();
                    if (oDao.TableExists(OpenFileDialog1.FileName.Trim(), p_strTable) == true)
                    {
                        frmMain.g_oFrmMain.ActivateStandByAnimation(
                            this.ParentForm.WindowState,
                            this.ParentForm.Left,
                            this.ParentForm.Height,
                            this.ParentForm.Width,
                            this.ParentForm.Top);
                        string strTempFile = frmMain.g_oUtils.getRandomFile(frmMain.g_oEnv.strTempDir, "accdb");
                        oDao.CreateMDB(strTempFile);
                        oDao.CreateTableLink(strTempFile, p_strTable, OpenFileDialog1.FileName.Trim(), p_strTable);
                        if (p_strTable.Trim().ToUpper() != "FVS_SUMMARY") oDao.CreateTableLink(strTempFile, "FVS_SUMMARY", OpenFileDialog1.FileName.Trim(), "FVS_SUMMARY");
                        //check base year file and table exist
                        if (p_strTable.Trim().ToUpper() == "FVS_POTFIRE")
                        {
                            strPotFireBaseYearFile = frmMain.g_oUtils.getFileName(OpenFileDialog1.FileName.Trim());
                            strPotFireBaseYearFile = strPotFireBaseYearFile.Substring(0, 9) + "_POTFIRE_BaseYr.MDB";
                            strPotFireBaseYearFile = frmMain.g_oUtils.getDirectory(OpenFileDialog1.FileName.Trim()) + "\\" + strPotFireBaseYearFile;
                            if (System.IO.File.Exists(strPotFireBaseYearFile))
                            {
                                if (oDao.TableExists(strPotFireBaseYearFile,"FVS_POTFIRE"))
                                {
                                  oDao.CreateTableLink(strTempFile, p_strTable + "_BaseYear", strPotFireBaseYearFile, p_strTable);
                                }
                                else
                                {
                                    strPotFireBaseYearFile="";
                                }
                            }
                            else
                            {
                                strPotFireBaseYearFile="";
                            }
                            
                        }
                        oDao.m_DaoWorkspace.Close();
                        oDao = null;
                        //make sure selected table exists
                        ado_data_access oAdo = new ado_data_access();
                        oAdo.OpenConnection(oAdo.getMDBConnString(strTempFile.Trim(), "", ""));
                        if (oAdo.m_intError == 0)
                        {

                            frmGridView frmGridView1 = new frmGridView();

                            if (p_strTable.Trim().ToUpper().IndexOf("FVS_POTFIRE", 0) < 0)
                            {
                                m_oRxTools.CreateFVSPrePostSeqNumTables(oAdo, oAdo.m_OleDbConnection, p_oItem, p_strTable, p_strTable, false, false, "");
                                if (oAdo.TableExist(oAdo.m_OleDbConnection, "temp_table1"))
                                {
                                    oAdo.m_strSQL = "DROP TABLE temp_table1";
                                    oAdo.SqlNonQuery(oAdo.m_OleDbConnection, oAdo.m_strSQL);
                                }
                            }

                            if (p_strTable.Trim().ToUpper() == "FVS_STRCLASS")
                            {
                                //assign a sequence number to each row in the FVS Output table for removal code = 0 and group by standid,year
                                oAdo.m_strSQL = Queries.FVS.FVSOutputTable_PrePostStrClassSQL("", "FVS_STRCLASS", true, "0");



                                frmMain.g_sbpInfo.Text = "Loading " + p_strTable.Trim() + "...Stand by";
                                frmGridView1.LoadDataSet(
                                    oAdo.m_OleDbConnection,
                                    oAdo.m_OleDbConnection.ConnectionString,
                                    oAdo.m_strSQL, p_strTable.Trim() + "_RemovalCodeEq0");

                                //assign a sequence number to each row in the FVS Output table for removal code = 1 and group by standid,year
                                oAdo.m_strSQL = Queries.FVS.FVSOutputTable_PrePostStrClassSQL("", "FVS_STRCLASS", true, "1");


                                frmGridView1.LoadDataSet(
                                    oAdo.m_OleDbConnection,
                                    oAdo.m_OleDbConnection.ConnectionString,
                                    oAdo.m_strSQL, p_strTable.Trim() + "_RemovalCodeEq1");


                                //assign a sequence number to each row in the FVS_SUMMARY table and group by standid,year
                                string[] strSQL = Queries.FVS.FVSOutputTable_AssignSummarySeqNumSQL("temp_table1", "temp_table2", "FVS_SUMMARY", p_strTable.Trim());
                                for (x = 0; x <= strSQL.Length - 1; x++)
                                {
                                    oAdo.SqlNonQuery(oAdo.m_OleDbConnection, strSQL[x]);
                                }
                                oAdo.m_strSQL = "SELECT * FROM temp_table1";
                                frmGridView1.LoadDataSet(
                                   oAdo.m_OleDbConnection,
                                   oAdo.m_OleDbConnection.ConnectionString,
                                   oAdo.m_strSQL, "FVS_SUMMARY_with_" + p_strTable.Trim() + "_RowCounts");

                                //view the PRE-POST records that will be retrieved based on sequential number assignment filters
                                oAdo.m_strSQL = Queries.FVS.FVSOutputTable_StrClassPrePostSeqNumByCycle("FVS_STRCLASS", p_strTable + "_PREPOST_SEQNUM_MATRIX", p_oItem);
                                frmGridView1.LoadDataSet(
                                 oAdo.m_OleDbConnection,
                                 oAdo.m_OleDbConnection.ConnectionString,
                                 oAdo.m_strSQL, p_strTable + "_CURRENT_PREPOST_SEQNUM_ASSIGNMENTS");

                                frmGridView1.TileGridViews();

                            }
                            else if (p_strTable.Trim().ToUpper() == "FVS_POTFIRE")
                            {
                                string[] strSQL = null;


                                frmMain.g_sbpInfo.Text = "Loading " + p_strTable.Trim() + "...Stand by";

                                //assign a sequence number to each row in the FVS Output table and group by standid,year
                                oAdo.m_strSQL = Queries.FVS.FVSOutputTable_PrePostGenericSQL("", p_strTable.Trim(), true);



                                frmGridView1.LoadDataSet(
                                    oAdo.m_OleDbConnection,
                                    oAdo.m_OleDbConnection.ConnectionString,
                                    oAdo.m_strSQL, p_strTable.Trim() + "_BaseYearNotIncluded");





                                if (strPotFireBaseYearFile.Trim().Length > 0)
                                {
                                    if (oAdo.TableExist(oAdo.m_OleDbConnection, "FVS_POTFIRE_BASEYEAR_WORK_TABLE"))
                                        oAdo.SqlNonQuery(oAdo.m_OleDbConnection, "DROP TABLE FVS_POTFIRE_BASEYEAR_WORK_TABLE");

                                    string[] strSQLArray = null;
                                    if (oAdo.ColumnExist(oAdo.m_OleDbConnection, "FVS_POTFIRE", "ID"))
                                    {

                                        strSQLArray = Queries.FVS.FVSOutputTable_PrePostPotFireBaseYearIDColumnSQL
                                            ("FVS_POTFIRE_BASEYEAR", "FVS_POTFIRE", "FVS_POTFIRE_BASEYEAR_WORK_TABLE");
                                    }
                                    else
                                    {
                                        strSQLArray = Queries.FVS.FVSOutputTable_PrePostPotFireBaseYearSQL
                                           ("FVS_POTFIRE_BASEYEAR", "FVS_POTFIRE", "FVS_POTFIRE_BASEYEAR_WORK_TABLE");
                                    }

                                    for (x = 0; x <= strSQLArray.Length - 1; x++)
                                    {
                                        oAdo.SqlNonQuery(oAdo.m_OleDbConnection, strSQLArray[x]);
                                    }

                                    //assign a sequence number to each row in the FVS Output table and group by standid,year
                                    oAdo.m_strSQL = Queries.FVS.FVSOutputTable_PrePostGenericSQL("", "FVS_POTFIRE_BASEYEAR_WORK_TABLE", true);

                                    frmGridView1.LoadDataSet(
                                        oAdo.m_OleDbConnection,
                                        oAdo.m_OleDbConnection.ConnectionString,
                                        oAdo.m_strSQL, p_strTable.Trim() + "_BaseYearIncluded");


                                    //assign a sequence number to each row in the FVS_SUMMARY table and group by standid,year
                                    strSQL = Queries.FVS.FVSOutputTable_AssignSummarySeqNumSQL("temp_table_baseyearincluded", "temp_table2", "FVS_SUMMARY", "FVS_POTFIRE_BASEYEAR_WORK_TABLE");
                                    for (x = 0; x <= strSQL.Length - 1; x++)
                                    {
                                        oAdo.SqlNonQuery(oAdo.m_OleDbConnection, strSQL[x]);
                                    }
                                    oAdo.m_strSQL = "SELECT * FROM temp_table_baseyearincluded";
                                    frmGridView1.LoadDataSet(
                                       oAdo.m_OleDbConnection,
                                       oAdo.m_OleDbConnection.ConnectionString,
                                       oAdo.m_strSQL, "FVS_SUMMARY_with_" + p_strTable.Trim() + "_BaseYearIncluded_RowCounts");
                                }



                                //assign a sequence number to each row in the FVS_SUMMARY table and group by standid,year
                                strSQL = Queries.FVS.FVSOutputTable_AssignSummarySeqNumSQL("temp_table1", "temp_table2", "FVS_SUMMARY", p_strTable.Trim());
                                for (x = 0; x <= strSQL.Length - 1; x++)
                                {
                                    oAdo.SqlNonQuery(oAdo.m_OleDbConnection, strSQL[x]);
                                }
                                oAdo.m_strSQL = "SELECT * FROM temp_table1";
                                frmGridView1.LoadDataSet(
                                   oAdo.m_OleDbConnection,
                                   oAdo.m_OleDbConnection.ConnectionString,
                                   oAdo.m_strSQL, "FVS_SUMMARY_with_" + p_strTable.Trim() + "_BaseYearNotIncluded_RowCounts");


                                if (p_oItem.RxCycle1PreSeqNumBaseYearYN == "Y" ||
                                    p_oItem.RxCycle2PreSeqNumBaseYearYN == "Y" ||
                                    p_oItem.RxCycle3PreSeqNumBaseYearYN == "Y" ||
                                    p_oItem.RxCycle4PreSeqNumBaseYearYN == "Y")
                                {
                                    m_oRxTools.CreateFVSPrePostSeqNumTables(oAdo, oAdo.m_OleDbConnection, p_oItem, "FVS_POTFIRE_BASEYEAR_WORK_TABLE", "FVS_POTFIRE_BASEYEAR_WORK_TABLE", false, false, "");
                                    if (oAdo.TableExist(oAdo.m_OleDbConnection, "temp_table1"))
                                    {
                                        oAdo.m_strSQL = "DROP TABLE temp_table1";
                                        oAdo.SqlNonQuery(oAdo.m_OleDbConnection, oAdo.m_strSQL);
                                    }

                                    //view the PRE-POST records that will be retrieved based on sequential number assignment filters
                                    oAdo.m_strSQL = Queries.FVS.FVSOutputTable_GenericPrePostSeqNumByCycle("FVS_POTFIRE_BASEYEAR_WORK_TABLE", "FVS_POTFIRE_BASEYEAR_WORK_TABLE_PREPOST_SEQNUM_MATRIX");

                                    frmGridView1.LoadDataSet(
                                      oAdo.m_OleDbConnection,
                                      oAdo.m_OleDbConnection.ConnectionString,
                                      oAdo.m_strSQL, p_strTable + "_CURRENT_PREPOST_SEQNUM_ASSIGN_BaseYearIncluded");
                                }
                                else
                                {

                                    m_oRxTools.CreateFVSPrePostSeqNumTables(oAdo, oAdo.m_OleDbConnection, p_oItem, p_strTable, p_strTable, false, false, "");
                                    if (oAdo.TableExist(oAdo.m_OleDbConnection, "temp_table1"))
                                    {
                                        oAdo.m_strSQL = "DROP TABLE temp_table1";
                                        oAdo.SqlNonQuery(oAdo.m_OleDbConnection, oAdo.m_strSQL);
                                    }

                                    //view the PRE-POST records that will be retrieved based on sequential number assignment filters
                                    oAdo.m_strSQL = Queries.FVS.FVSOutputTable_GenericPrePostSeqNumByCycle(p_strTable, p_strTable + "_PREPOST_SEQNUM_MATRIX");

                                    frmGridView1.LoadDataSet(
                                      oAdo.m_OleDbConnection,
                                      oAdo.m_OleDbConnection.ConnectionString,
                                      oAdo.m_strSQL, p_strTable + "_CURRENT_PREPOST_SEQNUM_ASSIGN_BaseYearNotIncluded");
                                }


                                frmGridView1.TileGridViews();
                            }
                            else if (p_strTable.Trim().ToUpper() == "FVS_CUTLIST")
                            {
                                frmMain.g_sbpInfo.Text = "Loading " + p_strTable.Trim() + "...Stand by";
                                string[] strSQL = Queries.FVS.FVSOutputTable_TreeHarvestSeqNumSQL("temp_table1", "temp_table2", "FVS_SUMMARY", "FVS_CUTLIST");
                                for (x = 0; x <= strSQL.Length - 1; x++)
                                {
                                    oAdo.SqlNonQuery(oAdo.m_OleDbConnection, strSQL[x]);
                                }
                                oAdo.m_strSQL = "SELECT * FROM temp_table1";
                                frmGridView1.LoadDataSet(
                                   oAdo.m_OleDbConnection,
                                   oAdo.m_OleDbConnection.ConnectionString,
                                   oAdo.m_strSQL, "FVS_SUMMARY_with_FVS_CUTLIST_RowCounts");

                                //view the PRE records that will be retrieved based on sequential number assignment filters
                                oAdo.m_strSQL = Queries.FVS.FVSOutputTable_GenericPrePostSeqNumByCycle(p_strTable, p_strTable + "_PREPOST_SEQNUM_MATRIX");

                                frmGridView1.LoadDataSet(
                                  oAdo.m_OleDbConnection,
                                  oAdo.m_OleDbConnection.ConnectionString,
                                  oAdo.m_strSQL, p_strTable + "_CURRENT_PREPOST_SEQNUM_ASSIGNMENTS");

                                frmGridView1.TileGridViews();

                            }
                            else
                            {
                                oAdo.m_strSQL = Queries.FVS.FVSOutputTable_PrePostGenericSQL("", p_strTable.Trim(), true);


                                frmMain.g_sbpInfo.Text = "Loading " + p_strTable.Trim() + "...Stand by";
                                frmGridView1.LoadDataSet(
                                    oAdo.m_OleDbConnection,
                                    oAdo.m_OleDbConnection.ConnectionString,
                                    oAdo.m_strSQL, p_strTable.Trim());

                                string[] strSQL = Queries.FVS.FVSOutputTable_AssignSummarySeqNumSQL("temp_table1", "temp_table2", "FVS_SUMMARY", p_strTable.Trim());
                                for (x = 0; x <= strSQL.Length - 1; x++)
                                {
                                    oAdo.SqlNonQuery(oAdo.m_OleDbConnection, strSQL[x]);
                                }
                                oAdo.m_strSQL = "SELECT * FROM temp_table1";
                                frmGridView1.LoadDataSet(
                                   oAdo.m_OleDbConnection,
                                   oAdo.m_OleDbConnection.ConnectionString,
                                   oAdo.m_strSQL, "FVS_SUMMARY_with_" + p_strTable.Trim() + "_RowCounts");

                                //view the PRE-POST records that will be retrieved based on sequential number assignment filters
                                oAdo.m_strSQL = Queries.FVS.FVSOutputTable_GenericPrePostSeqNumByCycle(p_strTable, p_strTable + "_PREPOST_SEQNUM_MATRIX");

                                frmGridView1.LoadDataSet(
                                  oAdo.m_OleDbConnection,
                                  oAdo.m_OleDbConnection.ConnectionString,
                                  oAdo.m_strSQL, p_strTable + "_CURRENT_PREPOST_SEQNUM_ASSIGNMENTS");

                                frmGridView1.TileGridViews();
                            }

                            frmMain.g_sbpInfo.Text = "Ready";
                            frmMain.g_oFrmMain.DeactivateStandByAnimation();
                            frmGridView1.ShowDialog();
                            if (oAdo.TableExist(oAdo.m_OleDbConnection, "temp_table1"))
                            {
                                oAdo.m_strSQL = "DROP TABLE temp_table1";
                                oAdo.SqlNonQuery(oAdo.m_OleDbConnection, oAdo.m_strSQL);
                            }
                        }
                        else frmMain.g_oFrmMain.DeactivateStandByAnimation();
                        oAdo.CloseConnection(oAdo.m_OleDbConnection);
                    }
                    else
                    {
                        MessageBox.Show(p_strTable.Trim() + " not found", "FIA Biosum", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            OpenFileDialog1 = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        public void CloseForm()
        {
            if ((btnSave.Enabled || m_bSave) && !btnAssignPackage.Enabled)
            {
                DialogResult result = MessageBox.Show("Save Changes? (Y/N)", "FIA Biosum", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case (DialogResult.Yes):
                        savevalues();
                        ReferenceDialog.Close();
                        break;
                    case (DialogResult.Cancel):
                        break;
                    default:
                        Exit = true;
                        ReferenceDialog.Close();
                        break;
                }
            }
            else if (cmbPOST1.Enabled)
            {
                DialogResult result = MessageBox.Show("Cancel custom edit? (Y/N)", "FIA Biosum", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case (DialogResult.Yes):
                        Exit = true;
                        
                        ReferenceDialog.Close();
                        break;
                    case (DialogResult.Cancel):
                        break;
                    default:
                       
                        break;
                }
            }
            else
            {

                Exit = true;
                ReferenceDialog.Close();
            }
        
        }

        private void btnSeqNumEdit_Click(object sender, EventArgs e)
        {
            FVSPrePostSeqNumItem oItem;
            if (m_cmbTableCustom.Visible)
            {
                if (this.m_cmbTableCustom.Text.Trim().ToUpper().IndexOf("FVS_", 0) == 0)
                {
                    oItem = new FVSPrePostSeqNumItem();
                    SaveEditValuesToProperties(oItem);
                    ViewSeqNumAssignments(m_cmbTableCustom.Text.Trim().ToUpper(), oItem);
                    oItem = null;
                }
            }
            else
            {
                oItem = new FVSPrePostSeqNumItem();
                SaveEditValuesToProperties(oItem);
                ViewSeqNumAssignments(lblCurTable.Text.Trim().ToUpper(), oItem);
                oItem = null;
            }
           
        }
    }
    /*********************************************************************************************************
	 **FVS Output PREPOST SeqNum Definition Item                          
	 *********************************************************************************************************/
    public class FVSPrePostSeqNumItem
    {
        public FVSPrePostSeqNumRxPackageAssgnItem_Collection  m_FVSPrePostSeqNumRxPackageAssgnItem_Collection1 = null;
        public FVSPrePostSeqNumRxPackageAssgnItem_Collection _FVSPrePostSeqNumRxPackageAssgnItem_Collection1 = null;

        private int _intPrePostSeqNumId = -1;
        [CategoryAttribute("General"), DescriptionAttribute("Unique Identifier")]
        public int PrePostSeqNumId
        {
            get { return _intPrePostSeqNumId; }
            set { _intPrePostSeqNumId = value; }
        }
        private string _strTableName = "";
        [CategoryAttribute("General"), DescriptionAttribute("Description")]
        public string TableName
        {
            get { return _strTableName; }
            set { _strTableName = value; }

        }
        private string _strType = "D";
        [CategoryAttribute("General"), DescriptionAttribute("Type")]
        public string Type
        {
            get { return _strType; }
            set { _strType = value; }

        }
        private string _strRxCycle1PreSeqNum = "";
        public string RxCycle1PreSeqNum
        {
            get { return _strRxCycle1PreSeqNum; }
            set { _strRxCycle1PreSeqNum = value; }
        }
        private string _strRxCycle1PostSeqNum = "";
        public string RxCycle1PostSeqNum
        {
            get { return _strRxCycle1PostSeqNum; }
            set { _strRxCycle1PostSeqNum = value; }
        }
        private string _strRxCycle2PreSeqNum = "";
        public string RxCycle2PreSeqNum
        {
            get { return _strRxCycle2PreSeqNum; }
            set { _strRxCycle2PreSeqNum = value; }
        }
        private string _strRxCycle2PostSeqNum = "";
        public string RxCycle2PostSeqNum
        {
            get { return _strRxCycle2PostSeqNum; }
            set { _strRxCycle2PostSeqNum = value; }
        }
        private string _strRxCycle3PreSeqNum = "";
        public string RxCycle3PreSeqNum
        {
            get { return _strRxCycle3PreSeqNum; }
            set { _strRxCycle3PreSeqNum = value; }
        }
        private string _strRxCycle3PostSeqNum = "";
        public string RxCycle3PostSeqNum
        {
            get { return _strRxCycle3PostSeqNum; }
            set { _strRxCycle3PostSeqNum = value; }
        }
        private string _strRxCycle4PreSeqNum = "";
        public string RxCycle4PreSeqNum
        {
            get { return _strRxCycle4PreSeqNum; }
            set { _strRxCycle4PreSeqNum = value; }
        }
        private string _strRxCycle4PostSeqNum = "";
        public string RxCycle4PostSeqNum
        {
            get { return _strRxCycle4PostSeqNum; }
            set { _strRxCycle4PostSeqNum = value; }
        }
        //
        //Use POTFIRE base year flag
        //
        //Y=Use BASEYEAR data; N=Do not use BASEYEAR data
        private string _strRxCycle1PreSeqNumBaseYearYN = "N";
        [CategoryAttribute("General"), DescriptionAttribute("Base Year")]
        public string RxCycle1PreSeqNumBaseYearYN
        {
            get { return _strRxCycle1PreSeqNumBaseYearYN; }
            set { _strRxCycle1PreSeqNumBaseYearYN = value; }

        }
        private string _strRxCycle2PreSeqNumBaseYearYN = "N";
        [CategoryAttribute("General"), DescriptionAttribute("Base Year")]
        public string RxCycle2PreSeqNumBaseYearYN
        {
            get { return _strRxCycle2PreSeqNumBaseYearYN; }
            set { _strRxCycle2PreSeqNumBaseYearYN = value; }
        }
        private string _strRxCycle3PreSeqNumBaseYearYN = "N";
        [CategoryAttribute("General"), DescriptionAttribute("Base Year")]
        public string RxCycle3PreSeqNumBaseYearYN
        {
            get { return _strRxCycle3PreSeqNumBaseYearYN; }
            set { _strRxCycle3PreSeqNumBaseYearYN = value; }
        }
        private string _strRxCycle4PreSeqNumBaseYearYN = "N";
        [CategoryAttribute("General"), DescriptionAttribute("Base Year")]
        public string RxCycle4PreSeqNumBaseYearYN
        {
            get { return _strRxCycle4PreSeqNumBaseYearYN; }
            set { _strRxCycle4PreSeqNumBaseYearYN = value; }

        }
        //
        //Use Before Tree Removal flag
        //
        //Y=Use Before Tree Removal Data; N=Use After Tree Removal Data
        private string _strRxCycle1PreStrClassBeforeTreeRemovalYN = "N";
        public string RxCycle1PreStrClassBeforeTreeRemovalYN
        {
            get { return _strRxCycle1PreStrClassBeforeTreeRemovalYN; }
            set { _strRxCycle1PreStrClassBeforeTreeRemovalYN = value; }

        }
        private string _strRxCycle2PreStrClassBeforeTreeRemovalYN = "N";
        public string RxCycle2PreStrClassBeforeTreeRemovalYN
        {
            get { return _strRxCycle2PreStrClassBeforeTreeRemovalYN; }
            set { _strRxCycle2PreStrClassBeforeTreeRemovalYN = value; }
        }
        private string _strRxCycle3PreStrClassBeforeTreeRemovalYN = "N";
        public string RxCycle3PreStrClassBeforeTreeRemovalYN
        {
            get { return _strRxCycle3PreStrClassBeforeTreeRemovalYN; }
            set { _strRxCycle3PreStrClassBeforeTreeRemovalYN = value; }
        }
        private string _strRxCycle4PreStrClassBeforeTreeRemovalYN = "N";
        public string RxCycle4PreStrClassBeforeTreeRemovalYN
        {
            get { return _strRxCycle4PreStrClassBeforeTreeRemovalYN; }
            set { _strRxCycle4PreStrClassBeforeTreeRemovalYN = value; }
        }
        private string _strRxCycle1PostStrClassBeforeTreeRemovalYN = "N";
        public string RxCycle1PostStrClassBeforeTreeRemovalYN
        {
            get { return _strRxCycle1PostStrClassBeforeTreeRemovalYN; }
            set { _strRxCycle1PostStrClassBeforeTreeRemovalYN = value; }

        }
        private string _strRxCycle2PostStrClassBeforeTreeRemovalYN = "N";
        public string RxCycle2PostStrClassBeforeTreeRemovalYN
        {
            get { return _strRxCycle2PostStrClassBeforeTreeRemovalYN; }
            set { _strRxCycle2PostStrClassBeforeTreeRemovalYN = value; }
        }
        private string _strRxCycle3PostStrClassBeforeTreeRemovalYN = "N";
        public string RxCycle3PostStrClassBeforeTreeRemovalYN
        {
            get { return _strRxCycle3PostStrClassBeforeTreeRemovalYN; }
            set { _strRxCycle3PostStrClassBeforeTreeRemovalYN = value; }
        }
        private string _strRxCycle4PostStrClassBeforeTreeRemovalYN = "N";
        public string RxCycle4PostStrClassBeforeTreeRemovalYN
        {
            get { return _strRxCycle4PostStrClassBeforeTreeRemovalYN; }
            set { _strRxCycle4PostStrClassBeforeTreeRemovalYN = value; }

        }

        private string _strUseSummaryTableSeqNumYN="Y";
        public string UseSummaryTableSeqNumYN
        {
            get { return _strUseSummaryTableSeqNumYN; }
            set { _strUseSummaryTableSeqNumYN = value; }
        }

        private string _strRxPackageList = "";
        [CategoryAttribute("General"), DescriptionAttribute("RX Package List")]
        public string RxPackageList
        {
            get { return _strRxPackageList; }
            set { _strRxPackageList = value; }

        }
        private bool _bMultipleRecordsForASingleStandYearCombination = false;
        public bool MultipleRecordsForASingleStandYearCombination
        {
            get { return _bMultipleRecordsForASingleStandYearCombination; }
            set { _bMultipleRecordsForASingleStandYearCombination = value; }
        }
        bool _bDelete = false;
        public bool Delete
        {
            get { return _bDelete; }
            set { _bDelete = value; }
        }
        bool _bAdd = false;
        public bool Add
        {
            get { return _bAdd; }
            set { _bAdd = value; }
        }
        private bool _bModified = false;
        public bool Modified
        {
            get { return _bModified; }
            set { _bModified = value; }
        }
        
        public FVSPrePostSeqNumRxPackageAssgnItem_Collection ReferenceFVSPrePostSeqNumRxPackageAssgnCollection
        {
            get { return _FVSPrePostSeqNumRxPackageAssgnItem_Collection1; }
            set { _FVSPrePostSeqNumRxPackageAssgnItem_Collection1 = value; }
        }

        public void CopyProperties(FIA_Biosum_Manager.FVSPrePostSeqNumItem p_ItemSource, FIA_Biosum_Manager.FVSPrePostSeqNumItem p_ItemDestination)
        {
            int x;

            p_ItemDestination.TableName = "";
            p_ItemDestination.PrePostSeqNumId = -1;
            p_ItemDestination.Type = "";
            p_ItemDestination.RxPackageList = "";

            p_ItemDestination.RxCycle1PreSeqNum = "";
            p_ItemDestination.RxCycle1PreSeqNumBaseYearYN = "N";
            p_ItemDestination.RxCycle1PostSeqNum = "";
            p_ItemDestination.RxCycle1PreStrClassBeforeTreeRemovalYN = "Y";
            p_ItemDestination.RxCycle1PostStrClassBeforeTreeRemovalYN = "N";

            p_ItemDestination.RxCycle2PreSeqNum = "";
            p_ItemDestination.RxCycle2PreSeqNumBaseYearYN = "N";
            p_ItemDestination.RxCycle2PostSeqNum = "";
            p_ItemDestination.RxCycle2PreStrClassBeforeTreeRemovalYN = "Y";
            p_ItemDestination.RxCycle2PostStrClassBeforeTreeRemovalYN = "N";


            p_ItemDestination.RxCycle3PreSeqNum = "";
            p_ItemDestination.RxCycle3PreSeqNumBaseYearYN = "N";
            p_ItemDestination.RxCycle3PostSeqNum = "";
            p_ItemDestination.RxCycle3PreStrClassBeforeTreeRemovalYN = "Y";
            p_ItemDestination.RxCycle3PostStrClassBeforeTreeRemovalYN = "N";


            p_ItemDestination.RxCycle4PreSeqNum = "";
            p_ItemDestination.RxCycle4PreSeqNumBaseYearYN = "N";
            p_ItemDestination.RxCycle4PostSeqNum = "";
            p_ItemDestination.RxCycle4PreStrClassBeforeTreeRemovalYN = "Y";
            p_ItemDestination.RxCycle4PostStrClassBeforeTreeRemovalYN = "N";


            p_ItemDestination.UseSummaryTableSeqNumYN = "Y";


            p_ItemDestination.PrePostSeqNumId = p_ItemSource.PrePostSeqNumId;
            p_ItemDestination.TableName = p_ItemSource.TableName;
            p_ItemDestination.Type = p_ItemSource.Type;
            p_ItemDestination.RxCycle1PreSeqNum = p_ItemSource.RxCycle1PreSeqNum;
            p_ItemDestination.RxCycle1PreSeqNumBaseYearYN = p_ItemSource.RxCycle1PreSeqNumBaseYearYN;
            p_ItemDestination.RxCycle1PostSeqNum = p_ItemSource.RxCycle1PostSeqNum;
            p_ItemDestination.RxPackageList = p_ItemSource.RxPackageList;

            p_ItemDestination.RxCycle2PreSeqNum = p_ItemSource.RxCycle2PreSeqNum;
            p_ItemDestination.RxCycle2PreSeqNumBaseYearYN = p_ItemSource.RxCycle2PreSeqNumBaseYearYN;
            p_ItemDestination.RxCycle2PostSeqNum = p_ItemSource.RxCycle2PostSeqNum;

            
            p_ItemDestination.RxCycle3PreSeqNum = p_ItemSource.RxCycle3PreSeqNum;
            p_ItemDestination.RxCycle3PreSeqNumBaseYearYN = p_ItemSource.RxCycle3PreSeqNumBaseYearYN;
            p_ItemDestination.RxCycle3PostSeqNum = p_ItemSource.RxCycle3PostSeqNum;

           
            p_ItemDestination.RxCycle4PreSeqNum = p_ItemSource.RxCycle4PreSeqNum;
            p_ItemDestination.RxCycle4PreSeqNumBaseYearYN = p_ItemSource.RxCycle4PreSeqNumBaseYearYN;
            p_ItemDestination.RxCycle4PostSeqNum = p_ItemSource.RxCycle4PostSeqNum;

            p_ItemDestination.RxCycle1PreStrClassBeforeTreeRemovalYN = p_ItemSource.RxCycle1PreStrClassBeforeTreeRemovalYN;
            p_ItemDestination.RxCycle1PostStrClassBeforeTreeRemovalYN = p_ItemSource.RxCycle1PostStrClassBeforeTreeRemovalYN;
            p_ItemDestination.RxCycle2PreStrClassBeforeTreeRemovalYN = p_ItemSource.RxCycle2PreStrClassBeforeTreeRemovalYN;
            p_ItemDestination.RxCycle2PostStrClassBeforeTreeRemovalYN = p_ItemSource.RxCycle2PostStrClassBeforeTreeRemovalYN;
            p_ItemDestination.RxCycle3PreStrClassBeforeTreeRemovalYN = p_ItemSource.RxCycle3PreStrClassBeforeTreeRemovalYN;
            p_ItemDestination.RxCycle3PostStrClassBeforeTreeRemovalYN = p_ItemSource.RxCycle3PostStrClassBeforeTreeRemovalYN;
            p_ItemDestination.RxCycle4PreStrClassBeforeTreeRemovalYN = p_ItemSource.RxCycle4PreStrClassBeforeTreeRemovalYN;
            p_ItemDestination.RxCycle4PostStrClassBeforeTreeRemovalYN = p_ItemSource.RxCycle4PostStrClassBeforeTreeRemovalYN;



            p_ItemDestination.UseSummaryTableSeqNumYN = p_ItemSource.UseSummaryTableSeqNumYN;


            p_ItemDestination.Delete = p_ItemSource.Delete;
            p_ItemDestination.Add = p_ItemSource.Add;
            p_ItemDestination.Modified = p_ItemSource.Modified;

            //remove any existing destination fvs command collection items 
            //since we are copying all the source to the destination
            if (p_ItemDestination.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection != null)
            {
                for (x = p_ItemDestination.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection.Count - 1; x >= 0; x--)
                {
                    if (p_ItemDestination.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection.Item(x).PrePostSeqNumId ==
                        p_ItemDestination.PrePostSeqNumId)
                            p_ItemDestination.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection.Remove(x);
                }
            }
            //load up the fvs commands specific to the package that are not members of a treatment
            if (p_ItemSource.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection != null)
            {
                p_ItemDestination.m_FVSPrePostSeqNumRxPackageAssgnItem_Collection1 = new FVSPrePostSeqNumRxPackageAssgnItem_Collection();
                for (x = 0; x <= p_ItemSource.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection.Count - 1; x++)
                {
                    if (p_ItemSource.PrePostSeqNumId == p_ItemSource.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection.Item(x).PrePostSeqNumId)
                    {
                        FIA_Biosum_Manager.FVSPrePostSeqNumRxPackageAssgnItem oItem = new FVSPrePostSeqNumRxPackageAssgnItem();
                        oItem.PrePostSeqNumId = p_ItemSource.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection.Item(x).PrePostSeqNumId;
                        oItem.RxPackageId = p_ItemSource.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection.Item(x).RxPackageId;
                        p_ItemDestination.m_FVSPrePostSeqNumRxPackageAssgnItem_Collection1.Add(oItem);

                    }
                    p_ItemDestination.ReferenceFVSPrePostSeqNumRxPackageAssgnCollection = p_ItemDestination.m_FVSPrePostSeqNumRxPackageAssgnItem_Collection1;

                }
            }
        }
    }
    public class FVSPrePostSeqNumItem_Collection : System.Collections.CollectionBase
    {
        public FVSPrePostSeqNumItem_Collection()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Add(FIA_Biosum_Manager.FVSPrePostSeqNumItem p_oItem)
        {
            // vérify if object is not already in
            if (this.List.Contains(p_oItem))
                throw new InvalidOperationException();

            // adding it
            this.List.Add(p_oItem);

            // return collection
            //return this;
        }
        public void Remove(int index)
        {
            // Check to see if there is a widget at the supplied index.
            if (index > Count - 1 || index < 0)
            // If no widget exists, a messagebox is shown and the operation 
            // is canColumned.
            {
                System.Windows.Forms.MessageBox.Show("Index not valid!");
            }
            else
            {
                List.RemoveAt(index);
            }
        }
        public FIA_Biosum_Manager.FVSPrePostSeqNumItem Item(int Index)
        {
            // The appropriate item is retrieved from the List object and
            // explicitly cast to the Widget type, then returned to the 
            // caller.
            return (FIA_Biosum_Manager.FVSPrePostSeqNumItem)List[Index];
        }
        public void CopyProperties(FVSPrePostSeqNumItem_Collection p_oDest, FVSPrePostSeqNumItem_Collection p_oSource)
        {
            p_oDest.Clear();
            for (int x = 0; x <= p_oSource.Count - 1; x++)
            {
               FVSPrePostSeqNumItem oItem = new FVSPrePostSeqNumItem();
               p_oSource.Item(x).CopyProperties(p_oSource.Item(x), oItem);
               p_oDest.Add(oItem);
            }
        }
       
    }
    public class FVSPrePostSeqNumRxPackageAssgnItem
    {
        private string _strRxPackageId = "";
        [CategoryAttribute("General"), DescriptionAttribute("RX Package Indentifier")]
        public string RxPackageId
        {
            get { return _strRxPackageId; }
            set { _strRxPackageId = value; }
        }

        private int _intPrePostSeqNumId = -1;
        [CategoryAttribute("General"), DescriptionAttribute("Unique Identifier")]
        public int PrePostSeqNumId
        {
            get { return _intPrePostSeqNumId; }
            set { _intPrePostSeqNumId = value; }
        }
        private bool _bDelete = false;
        public bool Delete
        {
            get { return _bDelete; }
            set { _bDelete = value; }
        }
        public void CopyProperties(FVSPrePostSeqNumRxPackageAssgnItem p_oSource, FVSPrePostSeqNumRxPackageAssgnItem p_oDest)
        {
            p_oSource.RxPackageId = "";
            p_oSource.PrePostSeqNumId = -1;

            p_oDest.PrePostSeqNumId = p_oSource.PrePostSeqNumId;
            p_oDest.RxPackageId = p_oSource.RxPackageId;
            p_oDest.Delete = p_oSource.Delete;
        }
    }
    public class FVSPrePostSeqNumRxPackageAssgnItem_Collection : System.Collections.CollectionBase
    {
        public FVSPrePostSeqNumRxPackageAssgnItem_Collection()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Add(FIA_Biosum_Manager.FVSPrePostSeqNumRxPackageAssgnItem m_FVSPrePostSeqNumRxPackageAssgnItem)
        {
            // vérify if object is not already in
            if (this.List.Contains(m_FVSPrePostSeqNumRxPackageAssgnItem))
                throw new InvalidOperationException();

            // adding it
            this.List.Add(m_FVSPrePostSeqNumRxPackageAssgnItem);

            // return collection
            //return this;
        }
        public void Remove(int index)
        {
            // Check to see if there is a widget at the supplied index.
            if (index > Count - 1 || index < 0)
            // If no widget exists, a messagebox is shown and the operation 
            // is canColumned.
            {
                System.Windows.Forms.MessageBox.Show("Index not valid!");
            }
            else
            {
                List.RemoveAt(index);
            }
        }
        public FIA_Biosum_Manager.FVSPrePostSeqNumRxPackageAssgnItem Item(int Index)
        {
            // The appropriate item is retrieved from the List object and
            // explicitly cast to the Widget type, then returned to the 
            // caller.
            return (FIA_Biosum_Manager.FVSPrePostSeqNumRxPackageAssgnItem)List[Index];
        }
    }
}
