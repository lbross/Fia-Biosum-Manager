using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace FIA_Biosum_Manager
{
	/// <summary>
	/// Summary description for uc_scenario_ffe.
	/// </summary>
	public class uc_core_scenario_weighted_average : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.ComponentModel.IContainer components;
		//private int m_intFullHt=400;
		public System.Data.OleDb.OleDbDataAdapter m_OleDbDataAdapter;
		public System.Data.DataSet m_DataSet;
		public System.Data.OleDb.OleDbConnection m_OleDbConnectionMaster;
		public System.Data.OleDb.OleDbConnection m_OleDbConnectionScenario;
		public System.Data.OleDb.OleDbCommand m_OleDbCommand;
		public System.Data.DataRelation m_DataRelation;
		public System.Data.DataTable m_DataTable;
		public System.Data.DataRow m_DataRow;
		public int m_intError = 0;
		public string m_strError = "";
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrev;
		private string m_strCurrentIndexTypeAndClass;
		private string m_strOverallEffectiveExpression="";
		private FIA_Biosum_Manager.utils m_oUtils; 
		private bool _bTorchIndex=true;
		private bool _bCrownIndex=true;
		public System.Windows.Forms.Label lblTitle;
		private FIA_Biosum_Manager.frmCoreScenario _frmScenario=null;
		private FIA_Biosum_Manager.uc_scenario_fvs_prepost_variables_tiebreaker _uc_tiebreaker;

		
		private int m_intCurVar=-1;
		int m_intCurVariableDefinitionStepCount=1;
        string[] m_strUserNavigation = null;
        private System.Windows.Forms.GroupBox grpboxDetails;

		public bool m_bSave=false;
        //list view associated classes
        private ListViewEmbeddedControls.ListViewEx m_lvEx;



		const int COLUMN_CHECKBOX=0;
		const int COLUMN_OPTIMIZE_VARIABLE=1;
		const int COLUMN_FVS_VARIABLE=2;
		const int COLUMN_VALUESOURCE=3;
		const int COLUMN_MAXMIN=4;
		const int COLUMN_USEFILTER=5;
		const int COLUMN_FILTER_OPERATOR=6;
        const int COLUMN_FILTER_VALUE = 7;
        private FIA_Biosum_Manager.uc_scenario_fvs_prepost_variables_effective.Variables _oCurVar;
		public bool m_bFirstTime=true;
		private bool _bDisplayAuditMsg=true;
        private bool m_bIgnoreListViewItemCheck = false;
        private System.Windows.Forms.GroupBox grpboxSummary;
		private FIA_Biosum_Manager.ListViewAlternateBackgroundColors m_oLvRowColors=new ListViewAlternateBackgroundColors();
        private ValidateNumericValues m_oValidate = new ValidateNumericValues();
        private string m_strLastValue = "";
        private FIA_Biosum_Manager.frmMain m_frmMain;
        public int m_DialogHt;
        public Panel pnlDetails;
        private TextBox textBox17;
        private Label label7;
        private Button button1;
        private Button btnDetailsCancel;
        private GroupBox groupBox4;
        private ComboBox comboBox1;
        private GroupBox groupBox3;
        private ListBox listBox1;
        private GroupBox groupBox2;
        private ListBox lstFVSVariableTables;
        private Label LblSelectedVariable;
        private Label lblSelectedFVSVariable;
        private TextBox textBox18;
        private Label label8;
        public int m_DialogWd;
        private Panel pnlSummary;
        private Button btnProperties;
        private Button btnDelete;
        private Button btnNew;
        private ListView lstVariables;
        private Button btnCancelSummary;
        private ColumnHeader vName;
        private ColumnHeader vDescription;
        private Button BtnHelp;
        public ListView lstWeights;
        private ListViewAlternateBackgroundColors m_oLvAlternateColors = new FIA_Biosum_Manager.ListViewAlternateBackgroundColors();
        private ListViewColumnSorter lvwColumnSorter;
        private const int COL_YEAR = 0;
        private const int COL_SEQNUM = 1;
        private const int COL_WEIGHT = 2;


        public uc_core_scenario_weighted_average(FIA_Biosum_Manager.frmMain p_frmMain)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.m_oUtils = new utils();
            this.m_frmMain = p_frmMain;
			this.m_strCurrentIndexTypeAndClass="";

            this.grpboxDetails.Top = grpboxSummary.Top;
            this.grpboxDetails.Left = this.grpboxSummary.Left;
            this.grpboxDetails.Height = this.grpboxSummary.Height;
            this.grpboxDetails.Width = this.grpboxSummary.Width;
            this.grpboxDetails.Hide();

            //m_oValidate.RoundDecimalLength = 0;
            //m_oValidate.Money = false;
            //m_oValidate.NullsAllowed = false;
            //m_oValidate.TestForMaxMin = false;
            //m_oValidate.MinValue = -1000;
            //m_oValidate.TestForMin = true;

            m_oLvAlternateColors.ReferenceAlternateBackgroundColor = frmMain.g_oGridViewAlternateRowBackgroundColor;
            this.m_oLvAlternateColors.ReferenceBackgroundColor = frmMain.g_oGridViewRowBackgroundColor;
            this.m_oLvAlternateColors.ReferenceListView = this.lstVariables;
            this.m_oLvAlternateColors.ReferenceSelectedRowBackgroundColor = frmMain.g_oGridViewSelectedRowBackgroundColor;
            this.m_oLvAlternateColors.CustomFullRowSelect = true;
            if (frmMain.g_oGridViewFont != null) lstVariables.Font = frmMain.g_oGridViewFont;

			// TODO: Add any initialization after the InitializeComponent call
            this.m_DialogWd = this.Width + 25;
            this.m_DialogHt = this.pnlDetails.Height + 120;
            this.Height = m_DialogHt -40;

            //@ToDo: Plugging temporary data for screenshots
            this.m_oLvAlternateColors.InitializeRowCollection();
            lstVariables.Items.Add("Weight_Torch");
            lstVariables.Items[0].UseItemStyleForSubItems = false;
            lstVariables.Items[0].SubItems.Add("Weighted P-Torch severity");
            m_oLvAlternateColors.AddRow();
            m_oLvAlternateColors.AddColumns(0, lstVariables.Columns.Count);
            lstVariables.Items.Add("wHazard");
            lstVariables.Items[1].UseItemStyleForSubItems = false;
            lstVariables.Items[1].SubItems.Add("Weighted Hazard Score");
            m_oLvAlternateColors.AddRow();
            m_oLvAlternateColors.AddColumns(1, lstVariables.Columns.Count);
            this.m_oLvAlternateColors.ListView();

            this.loadvalues();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpboxSummary = new System.Windows.Forms.GroupBox();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.btnCancelSummary = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.lstVariables = new System.Windows.Forms.ListView();
            this.vName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpboxDetails = new System.Windows.Forms.GroupBox();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.BtnHelp = new System.Windows.Forms.Button();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDetailsCancel = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstFVSVariableTables = new System.Windows.Forms.ListBox();
            this.LblSelectedVariable = new System.Windows.Forms.Label();
            this.lblSelectedFVSVariable = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lstWeights = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.grpboxSummary.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            this.grpboxDetails.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.grpboxSummary);
            this.groupBox1.Controls.Add(this.grpboxDetails);
            this.groupBox1.Controls.Add(this.lblTitle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(872, 2000);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Leave += new System.EventHandler(this.groupBox1_Leave);
            // 
            // grpboxSummary
            // 
            this.grpboxSummary.BackColor = System.Drawing.SystemColors.Control;
            this.grpboxSummary.Controls.Add(this.pnlSummary);
            this.grpboxSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpboxSummary.ForeColor = System.Drawing.Color.Black;
            this.grpboxSummary.Location = new System.Drawing.Point(8, 48);
            this.grpboxSummary.Name = "grpboxSummary";
            this.grpboxSummary.Size = new System.Drawing.Size(856, 472);
            this.grpboxSummary.TabIndex = 35;
            this.grpboxSummary.TabStop = false;
            // 
            // pnlSummary
            // 
            this.pnlSummary.AutoScroll = true;
            this.pnlSummary.Controls.Add(this.btnCancelSummary);
            this.pnlSummary.Controls.Add(this.btnProperties);
            this.pnlSummary.Controls.Add(this.btnDelete);
            this.pnlSummary.Controls.Add(this.btnNew);
            this.pnlSummary.Controls.Add(this.lstVariables);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSummary.Location = new System.Drawing.Point(3, 18);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new System.Drawing.Size(850, 451);
            this.pnlSummary.TabIndex = 12;
            // 
            // btnCancelSummary
            // 
            this.btnCancelSummary.Location = new System.Drawing.Point(411, 360);
            this.btnCancelSummary.Name = "btnCancelSummary";
            this.btnCancelSummary.Size = new System.Drawing.Size(114, 32);
            this.btnCancelSummary.TabIndex = 13;
            this.btnCancelSummary.Text = "Cancel";
            this.btnCancelSummary.Click += new System.EventHandler(this.btnCancelSummary_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(291, 360);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(114, 32);
            this.btnProperties.TabIndex = 12;
            this.btnProperties.Text = "Properties";
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(221, 360);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(64, 32);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(151, 360);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(64, 32);
            this.btnNew.TabIndex = 4;
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lstVariables
            // 
            this.lstVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.vName,
            this.vDescription});
            this.lstVariables.GridLines = true;
            this.lstVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstVariables.HideSelection = false;
            this.lstVariables.Location = new System.Drawing.Point(18, 18);
            this.lstVariables.MultiSelect = false;
            this.lstVariables.Name = "lstVariables";
            this.lstVariables.Size = new System.Drawing.Size(640, 336);
            this.lstVariables.TabIndex = 2;
            this.lstVariables.UseCompatibleStateImageBehavior = false;
            this.lstVariables.View = System.Windows.Forms.View.Details;
            // 
            // vName
            // 
            this.vName.Text = "Variable Name";
            this.vName.Width = 200;
            // 
            // vDescription
            // 
            this.vDescription.Text = "Description";
            this.vDescription.Width = 400;
            // 
            // grpboxDetails
            // 
            this.grpboxDetails.BackColor = System.Drawing.SystemColors.Control;
            this.grpboxDetails.Controls.Add(this.pnlDetails);
            this.grpboxDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpboxDetails.ForeColor = System.Drawing.Color.Black;
            this.grpboxDetails.Location = new System.Drawing.Point(8, 536);
            this.grpboxDetails.Name = "grpboxDetails";
            this.grpboxDetails.Size = new System.Drawing.Size(856, 472);
            this.grpboxDetails.TabIndex = 32;
            this.grpboxDetails.TabStop = false;
            this.grpboxDetails.Text = "Weighted Average";
            this.grpboxDetails.Resize += new System.EventHandler(this.grpboxFVSVariablesPrePost_Resize);
            // 
            // pnlDetails
            // 
            this.pnlDetails.AutoScroll = true;
            this.pnlDetails.Controls.Add(this.lstWeights);
            this.pnlDetails.Controls.Add(this.BtnHelp);
            this.pnlDetails.Controls.Add(this.textBox18);
            this.pnlDetails.Controls.Add(this.label8);
            this.pnlDetails.Controls.Add(this.textBox17);
            this.pnlDetails.Controls.Add(this.label7);
            this.pnlDetails.Controls.Add(this.button1);
            this.pnlDetails.Controls.Add(this.btnDetailsCancel);
            this.pnlDetails.Controls.Add(this.groupBox4);
            this.pnlDetails.Controls.Add(this.groupBox3);
            this.pnlDetails.Controls.Add(this.groupBox2);
            this.pnlDetails.Controls.Add(this.LblSelectedVariable);
            this.pnlDetails.Controls.Add(this.lblSelectedFVSVariable);
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(3, 18);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(850, 451);
            this.pnlDetails.TabIndex = 70;
            // 
            // BtnHelp
            // 
            this.BtnHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.BtnHelp.Location = new System.Drawing.Point(565, 402);
            this.BtnHelp.Name = "BtnHelp";
            this.BtnHelp.Size = new System.Drawing.Size(64, 24);
            this.BtnHelp.TabIndex = 87;
            this.BtnHelp.Text = "Help";
            // 
            // textBox18
            // 
            this.textBox18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox18.Location = new System.Drawing.Point(231, 386);
            this.textBox18.Multiline = true;
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(259, 40);
            this.textBox18.TabIndex = 86;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(13, 389);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(212, 24);
            this.label8.TabIndex = 85;
            this.label8.Text = "Description:";
            // 
            // textBox17
            // 
            this.textBox17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox17.Location = new System.Drawing.Point(231, 357);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(259, 22);
            this.textBox17.TabIndex = 84;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(13, 360);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(212, 24);
            this.label7.TabIndex = 79;
            this.label7.Text = "Weighted average variable name:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(634, 402);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 24);
            this.button1.TabIndex = 77;
            this.button1.Text = "Calculate";
            // 
            // btnDetailsCancel
            // 
            this.btnDetailsCancel.Location = new System.Drawing.Point(716, 402);
            this.btnDetailsCancel.Name = "btnDetailsCancel";
            this.btnDetailsCancel.Size = new System.Drawing.Size(64, 24);
            this.btnDetailsCancel.TabIndex = 75;
            this.btnDetailsCancel.Text = "Cancel";
            this.btnDetailsCancel.Click += new System.EventHandler(this.btnDetailsCancel_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Location = new System.Drawing.Point(8, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(154, 48);
            this.groupBox4.TabIndex = 74;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Baseline RxPackage";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] {
            "005"});
            this.comboBox1.Location = new System.Drawing.Point(8, 18);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(72, 24);
            this.comboBox1.TabIndex = 77;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox1);
            this.groupBox3.Location = new System.Drawing.Point(440, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 133);
            this.groupBox3.TabIndex = 72;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "FVS Variable(s)";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Items.AddRange(new object[] {
            "Canopy_Density",
            "Canopy_Ht",
            "Crown_Index",
            "Fire_Type_Mod",
            "PTorch_Sev",
            "PTorch_Mod",
            "Torch_Index",
            ""});
            this.listBox1.Location = new System.Drawing.Point(6, 21);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(181, 100);
            this.listBox1.TabIndex = 70;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstFVSVariableTables);
            this.groupBox2.Location = new System.Drawing.Point(208, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 133);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "FVS Variable Table(s)";
            // 
            // lstFVSVariableTables
            // 
            this.lstFVSVariableTables.FormattingEnabled = true;
            this.lstFVSVariableTables.ItemHeight = 16;
            this.lstFVSVariableTables.Items.AddRange(new object[] {
            "FVS_BURNREPORT",
            "FVS_CARBON",
            "FVS_COMPUTE",
            "FVS_HRV_CARBON",
            "FVS_POTFIRE",
            "FVS_STRCLASS",
            "FVS_SUMMARY"});
            this.lstFVSVariableTables.Location = new System.Drawing.Point(6, 21);
            this.lstFVSVariableTables.Name = "lstFVSVariableTables";
            this.lstFVSVariableTables.Size = new System.Drawing.Size(181, 100);
            this.lstFVSVariableTables.TabIndex = 70;
            // 
            // LblSelectedVariable
            // 
            this.LblSelectedVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSelectedVariable.Location = new System.Drawing.Point(157, 145);
            this.LblSelectedVariable.Name = "LblSelectedVariable";
            this.LblSelectedVariable.Size = new System.Drawing.Size(302, 24);
            this.LblSelectedVariable.TabIndex = 69;
            this.LblSelectedVariable.Text = "FVS_POTFIRE.PTorch_Sev";
            // 
            // lblSelectedFVSVariable
            // 
            this.lblSelectedFVSVariable.Location = new System.Drawing.Point(11, 144);
            this.lblSelectedFVSVariable.Name = "lblSelectedFVSVariable";
            this.lblSelectedFVSVariable.Size = new System.Drawing.Size(151, 24);
            this.lblSelectedFVSVariable.TabIndex = 68;
            this.lblSelectedFVSVariable.Text = "Selected FVS Variable:";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Green;
            this.lblTitle.Location = new System.Drawing.Point(3, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(866, 32);
            this.lblTitle.TabIndex = 27;
            this.lblTitle.Text = "Calculated Variables";
            // 
            // lstWeights
            // 
            this.lstWeights.CheckBoxes = true;
            this.lstWeights.GridLines = true;
            this.lstWeights.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstWeights.HideSelection = false;
            this.lstWeights.Location = new System.Drawing.Point(17, 174);
            this.lstWeights.MultiSelect = false;
            this.lstWeights.Name = "lstWeights";
            this.lstWeights.Size = new System.Drawing.Size(350, 150);
            this.lstWeights.TabIndex = 88;
            this.lstWeights.UseCompatibleStateImageBehavior = false;
            this.lstWeights.View = System.Windows.Forms.View.Details;
            // 
            // uc_core_scenario_weighted_average
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "uc_core_scenario_weighted_average";
            this.Size = new System.Drawing.Size(872, 2000);
            this.groupBox1.ResumeLayout(false);
            this.grpboxSummary.ResumeLayout(false);
            this.pnlSummary.ResumeLayout(false);
            this.grpboxDetails.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        public void loadvalues_FromProperties()
        {


        }
        public void loadvalues()
		{
			
			this.m_intError=0;
			this.m_strError="";

            //
            //INSTANTIATE EXTENDED LISTVIEW OBJECT
            //
            m_lvEx = new ListViewEmbeddedControls.ListViewEx();
            m_lvEx.MouseUp += new System.Windows.Forms.MouseEventHandler(m_lvEx_MouseUp);
            m_lvEx.SelectedIndexChanged += new System.EventHandler(m_lvEx_SelectedIndexChanged);
            m_lvEx.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(m_lvEx_ColumnClick);
            this.pnlDetails.Controls.Add(m_lvEx);
            m_lvEx.Size = this.lstWeights.Size;
            m_lvEx.Location = this.lstWeights.Location;
            //m_lvEx.CheckBoxes = true;
            m_lvEx.AllowColumnReorder = false;
            m_lvEx.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            m_lvEx.FullRowSelect = false;
            m_lvEx.MultiSelect = false;
            m_lvEx.GridLines = true;
            this.m_lvEx.HideSelection = false;
            m_lvEx.View = System.Windows.Forms.View.Details;
            this.lstWeights.Hide();

            //
            //INITIALIZE LISTVIEW ALTERNATE ROW COLORS
            //
            System.Windows.Forms.ListViewItem entryListItem = null;
            this.m_oLvAlternateColors.InitializeRowCollection();
            this.m_oLvAlternateColors.ReferenceAlternateBackgroundColor = frmMain.g_oGridViewAlternateRowBackgroundColor;
            this.m_oLvAlternateColors.ReferenceAlternateForegroundColor = frmMain.g_oGridViewRowForegroundColor;
            this.m_oLvAlternateColors.ReferenceBackgroundColor = frmMain.g_oGridViewRowBackgroundColor;
            this.m_oLvAlternateColors.ReferenceForegroundColor = frmMain.g_oGridViewRowForegroundColor;
            this.m_oLvAlternateColors.ReferenceSelectedRowBackgroundColor = frmMain.g_oGridViewSelectedRowBackgroundColor;
            this.m_oLvAlternateColors.ReferenceListView = m_lvEx;
            this.m_oLvAlternateColors.CustomFullRowSelect = true;
            m_oLvAlternateColors.ColumnsToNotUpdate(COL_YEAR.ToString());
            m_oLvAlternateColors.ColumnsToNotUpdate(COL_SEQNUM.ToString());
            if (frmMain.g_oGridViewFont != null) m_lvEx.Font = frmMain.g_oGridViewFont;
            //
            //ASSIGN LISTVIEW COLUMN LABELS
            //
            m_lvEx.Show();
            this.m_lvEx.Clear();
            this.m_lvEx.Columns.Add("Year", 100, HorizontalAlignment.Right);
            this.m_lvEx.Columns.Add("Sequence Number", 125, HorizontalAlignment.Right);
            this.m_lvEx.Columns.Add("Weight", 100, HorizontalAlignment.Right);
            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.m_lvEx.ListViewItemSorter = lvwColumnSorter;
            //
            //TEMPORARILY ADD ITEMS
            //
            entryListItem = m_lvEx.Items.Add("1");
            entryListItem.UseItemStyleForSubItems = false;
            this.m_oLvAlternateColors.AddRow();
            this.m_oLvAlternateColors.AddColumns(m_lvEx.Items.Count - 1, m_lvEx.Columns.Count);
            entryListItem.SubItems.Add("2");
            entryListItem.SubItems.Add("0.025");

		}

		public int savevalues()
		{
			int x;
			ado_data_access oAdo = new ado_data_access();
			string strScenarioId = this.ReferenceCoreScenarioForm.uc_scenario1.txtScenarioId.Text.Trim().ToLower();
			string strScenarioMDB = 
				frmMain.g_oFrmMain.frmProject.uc_project1.txtRootDirectory.Text.Trim() + 
				"\\core\\db\\scenario_core_rule_definitions.mdb";
			oAdo.OpenConnection(oAdo.getMDBConnString(strScenarioMDB,"",""));
			if (oAdo.m_intError==0)
			{


			}
			
            return 1;
		}

		protected void SendKeyStrokes(System.Windows.Forms.TextBox p_oTextBox, string strKeyStrokes)
		{
			try 
			{
				p_oTextBox.Focus();
				System.Windows.Forms.SendKeys.Send(strKeyStrokes);
				p_oTextBox.Refresh();
			}
			catch  (Exception caught)
			{
               MessageBox.Show("SendKeyStrokes Method Failed With This Message:" + caught.Message);
			}

		}

		
		
		
			
		

		protected void NextButton(ref System.Windows.Forms.GroupBox p_oGb, ref System.Windows.Forms.Button p_oButton, string strButtonName)
		{
           p_oGb.Controls.Add(p_oButton);
		   p_oButton.Left = p_oGb.Width - p_oButton.Width - 5;
		   p_oButton.Top = p_oGb.Height - p_oButton.Height - 5;
		   p_oButton.Name = strButtonName;	
		}
		protected void PrevButton(ref System.Windows.Forms.GroupBox p_oGb, ref System.Windows.Forms.Button p_oButton, string strButtonName)
		{
			p_oGb.Controls.Add(p_oButton);
			p_oButton.Top = this.btnNext.Top;
			p_oButton.Height = this.btnNext.Height;
			p_oButton.Width = this.btnNext.Width;
			p_oButton.Left = this.btnNext.Left - p_oButton.Width;
			p_oButton.Name = strButtonName;	
		}
		
		

		
		public void main_resize()
		{
			
		}		

		private void btnFVSVariablesPrePostVariableValue_Click(object sender, System.EventArgs e)
		{
		
		}

		private void btnFVSVariablesPrePostVariableClearAll_Click(object sender, System.EventArgs e)
		{
			
		}

		private void btnFVSVariablesPrePostVariableNext_Click(object sender, System.EventArgs e)
		{
			
		}
		private void RollBack()
		{
			RollBack_variable1();
			RollBack_variable2();
			RollBack_variable3();
			RollBack_SqlBetter();
			RollBack_SqlWorse();
			RollBack_Overall();
		}
		private void RollBack_variable1()
		{
		}
		private void RollBack_variable2()
		{
		}
		private void RollBack_variable3()
		{
		}
		private void RollBack_SqlBetter()
		{
		}
		private void RollBack_SqlWorse()
		{
		}
		private void RollBack_Overall()
		{
		}

		private void grpboxFVSVariablesPrePostVariable_Resize(object sender, System.EventArgs e)
		{
			
		}

		private void grpboxFVSVariablesPrePostExpression_Resize(object sender, System.EventArgs e)
		{
		}

		private void grpboxFVSVariablesPrePostVariableValues_Resize(object sender, System.EventArgs e)
		{

		}

		private void btnFVSVariablesPrePostExpressionPrevious_Click(object sender, System.EventArgs e)
		{
			
			
			
		}

	


		private void Go()
		{

		}
		
		
		private void ShowGroupBox(string p_strName)
		{
			int x;
			//System.Windows.Forms.Control oControl;
			for (x=0;x<=groupBox1.Controls.Count-1;x++)
			{
				if (groupBox1.Controls[x].Name.Substring(0,3)=="grp")
				{
					if (p_strName.Trim().ToUpper() == 
						groupBox1.Controls[x].Name.Trim().ToUpper())
					{
						groupBox1.Controls[x].Show();
					}
					else
					{
						groupBox1.Controls[x].Hide();
					}
				}
			}
		}

		private void btnFVSVariablesPrePostValuesButtonsEdit_Click(object sender, System.EventArgs e)
		{

		}


		private void btnFVSVariablesPrePost2Overall_Click(object sender, System.EventArgs e)
		{
		}

		private void btnFVSVariablesPrePostValuesButtonsClear_Click(object sender, System.EventArgs e)
		{
		}
		

		

		private void grpboxFVSVariablesPrePost_Resize(object sender, System.EventArgs e)
		{
			   
		}

		

		private void lvFVSVariablesPrePostValues_DoubleClick(object sender, System.EventArgs e)
		{
		
		}

		private void pnlFVSVariablesPrePostExpression_Resize(object sender, System.EventArgs e)
		{
			
			
		}

		private void pnlFVSVariablesPrePostVariable_Resize(object sender, System.EventArgs e)
		{
			
			
		}

		private string ValidateNumeric(string p_strValue)
		{
            string strValue = p_strValue.Replace("$", "");
            strValue = strValue.Replace(",", "");
			try
			{
               double dbl = Convert.ToDouble(strValue);
			}
			catch 
			{
				return "0";
			}
			return strValue;
		}


		private void groupBox1_Leave(object sender, System.EventArgs e)
		{
			
		}
		private void EnableTabs(bool p_bEnable)
		{
            int x;
			ReferenceCoreScenarioForm.EnableTabPage(ReferenceCoreScenarioForm.tabControlScenario,"tbdesc,tbnotes,tbdatasources",p_bEnable);
			ReferenceCoreScenarioForm.EnableTabPage(ReferenceCoreScenarioForm.tabControlRules,"tbpsites,tbowners,tbcost,tbtreatmentintensity,tbfilterplots,tbrun",p_bEnable);
			ReferenceCoreScenarioForm.EnableTabPage(ReferenceCoreScenarioForm.tabControlFVSVariables,"tbeffective,tbtiebreaker",p_bEnable);
            for (x = 0; x <= ReferenceCoreScenarioForm.tlbScenario.Buttons.Count - 1; x++)
            {
                ReferenceCoreScenarioForm.tlbScenario.Buttons[x].Enabled = p_bEnable;
            }
            frmMain.g_oFrmMain.grpboxLeft.Enabled = p_bEnable;
            frmMain.g_oFrmMain.tlbMain.Enabled = p_bEnable;
            frmMain.g_oFrmMain.mnuMain.MenuItems[0].Enabled = p_bEnable;
            frmMain.g_oFrmMain.mnuMain.MenuItems[1].Enabled = p_bEnable;
            frmMain.g_oFrmMain.mnuMain.MenuItems[2].Enabled = p_bEnable;

		}

		private void btnOptimizationAudit_Click(object sender, System.EventArgs e)
		{
			this.DisplayAuditMessage=true;
			Audit();
		}
		public void Audit()
		{
			
			
			int x;
			this.m_intError=0;
			m_strError="";
			if (DisplayAuditMessage)
			{
				this.m_strError="Audit Results \r\n";
				this.m_strError=m_strError + "-------------\r\n\r\n";
			}

			
			if (DisplayAuditMessage)
			{
				if (m_intError==0) this.m_strError=m_strError + "Passed Audit";
				else m_strError = m_strError + "\r\n\r\n" + "Failed Audit";
				MessageBox.Show(m_strError,"FIA Biosum");
			}

		}
		
		public bool DisplayAuditMessage
		{
			get {return _bDisplayAuditMsg;}
			set {_bDisplayAuditMsg=value;}
		}
		public FIA_Biosum_Manager.frmCoreScenario ReferenceCoreScenarioForm
		{
			get {return _frmScenario;}
			set {_frmScenario=value;}
		}
		public FIA_Biosum_Manager.uc_scenario_fvs_prepost_variables_effective.Variables ReferenceFVSVariables
		{
			get {return this._oCurVar;}
			set {_oCurVar=value;}
		}
		public FIA_Biosum_Manager.uc_scenario_fvs_prepost_variables_tiebreaker ReferenceTieBreaker
		{
			get {return _uc_tiebreaker;}
			set {_uc_tiebreaker=value;}
		}

        private void btnCancelSummary_Click(object sender, EventArgs e)
        {

			this.ParentForm.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.grpboxSummary.Hide();
            this.grpboxDetails.Show();
        }

        private void btnDetailsCancel_Click(object sender, EventArgs e)
        {
            this.grpboxSummary.Show();
            this.grpboxDetails.Hide();
            //@ToDo: Add code to clear fields on details screen
        }


        private void m_lvEx_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int x;
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    int intRowHt = this.m_lvEx.Items[0].Bounds.Height;
                    double dblRow = (double)(e.Y / intRowHt);
                    this.m_lvEx.Items[m_lvEx.TopItem.Index + (int)dblRow - 1].Selected = true;
                    this.m_oLvAlternateColors.DelegateListViewItem(m_lvEx.Items[m_lvEx.TopItem.Index + (int)dblRow - 1]);


                }
            }
            catch
            {
            }
        }

        private void m_lvEx_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_lvEx.SelectedItems.Count > 0)
                m_oLvAlternateColors.DelegateListViewItem(m_lvEx.SelectedItems[0]);
        }
        private void m_lvEx_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            int x, y;

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.m_lvEx.Sort();
            //reinitialize the alternate row colors
            for (x = 0; x <= this.m_lvEx.Items.Count - 1; x++)
            {
                for (y = 0; y <= this.m_lvEx.Columns.Count - 1; y++)
                {
                    m_oLvAlternateColors.ListViewSubItem(this.m_lvEx.Items[x].Index, y, this.m_lvEx.Items[this.m_lvEx.Items[x].Index].SubItems[y], false);
                }
            }
        }
	}
}
