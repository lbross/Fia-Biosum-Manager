using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;

namespace FIA_Biosum_Manager
{
	/// <summary>
	/// Summary description for uc_scenario_tree_groupings.
	/// </summary>
	public class uc_scenario_tree_groupings : System.Windows.Forms.UserControl
    {
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Label lblTitle;
		private FIA_Biosum_Manager.frmProcessorScenario _frmProcessorScenario=null;
		private string _strScenarioType="processor";
        private Button BtnTreeDiameterGroups;
        private FIA_Biosum_Manager.frmDialog m_frmTreeDiamGroups;     //processor tree diameter form
        private Button BtnTreeSpeciesGroups;       
        private FIA_Biosum_Manager.frmDialog m_frmTreeSpeciesGroups;     //processor tree species groups form
        private bool _bCopyScenarioTreeDiamGroups = false;
        private bool _bCopyScenarioTreeSpeciesGroups = false;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public uc_scenario_tree_groupings()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
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
            this.BtnTreeDiameterGroups = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.BtnTreeSpeciesGroups = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnTreeSpeciesGroups);
            this.groupBox1.Controls.Add(this.BtnTreeDiameterGroups);
            this.groupBox1.Controls.Add(this.lblTitle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(664, 424);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // BtnTreeDiameterGroups
            // 
            this.BtnTreeDiameterGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTreeDiameterGroups.Location = new System.Drawing.Point(185, 99);
            this.BtnTreeDiameterGroups.Name = "BtnTreeDiameterGroups";
            this.BtnTreeDiameterGroups.Size = new System.Drawing.Size(198, 33);
            this.BtnTreeDiameterGroups.TabIndex = 27;
            this.BtnTreeDiameterGroups.Text = "TREE DIAMETER GROUPS";
            this.BtnTreeDiameterGroups.UseVisualStyleBackColor = true;
            this.BtnTreeDiameterGroups.Click += new System.EventHandler(this.BtnTreeDiameterGroups_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Green;
            this.lblTitle.Location = new System.Drawing.Point(3, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(658, 32);
            this.lblTitle.TabIndex = 26;
            this.lblTitle.Text = "Tree Groupings";
            // 
            // BtnTreeSpeciesGroups
            // 
            this.BtnTreeSpeciesGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTreeSpeciesGroups.Location = new System.Drawing.Point(185, 160);
            this.BtnTreeSpeciesGroups.Name = "BtnTreeSpeciesGroups";
            this.BtnTreeSpeciesGroups.Size = new System.Drawing.Size(198, 33);
            this.BtnTreeSpeciesGroups.TabIndex = 28;
            this.BtnTreeSpeciesGroups.Text = "TREE SPECIES GROUPS";
            this.BtnTreeSpeciesGroups.UseVisualStyleBackColor = true;
            this.BtnTreeSpeciesGroups.Click += new System.EventHandler(this.BtnTreeSpeciesGroups_Click);
            // 
            // uc_scenario_tree_groupings
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "uc_scenario_tree_groupings";
            this.Size = new System.Drawing.Size(664, 424);
            this.Resize += new System.EventHandler(this.uc_scenario_tree_groupings_Resize);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void uc_scenario_tree_groupings_Resize(object sender, System.EventArgs e)
		{
			
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Visible=false;
			if (this.ScenarioType.Trim().ToUpper() == "CORE") ((frmCoreScenario)this.ParentForm).Height = 0 ; //((frmScenario)this.ParentForm).grpboxMenu.Height * 2;
			else this.ReferenceProcessorScenarioForm.Height=0;
		}

		public FIA_Biosum_Manager.frmProcessorScenario ReferenceProcessorScenarioForm
		{
			get {return _frmProcessorScenario;}
			set {_frmProcessorScenario=value;}
		}
		public string ScenarioType
		{
			get {return _strScenarioType;}
			set {_strScenarioType=value;}
		}
        public bool CopyScenarioTreeDiamGroups
        {
            get { return _bCopyScenarioTreeDiamGroups; }
            set { _bCopyScenarioTreeDiamGroups = value; }
        }
        public bool CopyScenarioTreeSpeciesGroups
        {
            get { return _bCopyScenarioTreeSpeciesGroups; }
            set { _bCopyScenarioTreeSpeciesGroups = value; }
        }
        public FIA_Biosum_Manager.uc_tree_diam_groups_list uc_tree_diam_groups_list1
        {
            get 
            { 
                return m_frmTreeDiamGroups.uc_tree_diam_groups_list1;  
            }
        }
        public FIA_Biosum_Manager.uc_tree_spc_groups uc_tree_spc_groups1
        {
            get
            {
                return m_frmTreeSpeciesGroups.uc_tree_spc_groups1;
            }
        }

        private void BtnTreeDiameterGroups_Click(object sender, EventArgs e)
        {

            frmMain.g_oFrmMain.ActivateStandByAnimation(
                frmMain.g_oFrmMain.WindowState,
                frmMain.g_oFrmMain.Left,
                frmMain.g_oFrmMain.Height,
                frmMain.g_oFrmMain.Width,
                frmMain.g_oFrmMain.Top);

            // Initialize Tree Diameter form
            this.m_frmTreeDiamGroups = new frmDialog(_frmProcessorScenario, frmMain.g_oFrmMain);
            this.m_frmTreeDiamGroups.MaximizeBox = false;
            this.m_frmTreeDiamGroups.BackColor = System.Drawing.SystemColors.Control;
            this.m_frmTreeDiamGroups.Text = "Processor: Tree Diameter Groups";
            // @ToDo: Not sure if we need this
            //this.m_frmTreeDiam.MdiParent = ofrmProcessorScenario;
            this.m_frmTreeDiamGroups.Initialize_Plot_Tree_Diam_User_Control();

            this.m_frmTreeDiamGroups.Height = 0;
            this.m_frmTreeDiamGroups.Width = 0;    
            
            if (this.m_frmTreeDiamGroups.uc_tree_diam_groups_list1.Top + this.m_frmTreeDiamGroups.uc_tree_diam_groups_list1.Height > this.m_frmTreeDiamGroups.ClientSize.Height + 2)
                {
                    for (int x = 1; ; x++)
                    {
                        this.m_frmTreeDiamGroups.Height = x;
                        if (this.m_frmTreeDiamGroups.uc_tree_diam_groups_list1.Top +
                            this.m_frmTreeDiamGroups.uc_tree_diam_groups_list1.Height <
                            this.m_frmTreeDiamGroups.ClientSize.Height)
                        {
                            break;
                        }
                    }

                }
                if (this.m_frmTreeDiamGroups.uc_tree_diam_groups_list1.Left + this.m_frmTreeDiamGroups.uc_tree_diam_groups_list1.Width > this.m_frmTreeDiamGroups.ClientSize.Width + 2)
                {
                    for (int x = 1; ; x++)
                    {
                        this.m_frmTreeDiamGroups.Width = x;
                        if (this.m_frmTreeDiamGroups.uc_tree_diam_groups_list1.Left +
                            this.m_frmTreeDiamGroups.uc_tree_diam_groups_list1.Width <
                            this.m_frmTreeDiamGroups.ClientSize.Width)
                        {
                            break;
                        }
                    }

                }

            m_frmTreeDiamGroups.uc_tree_diam_groups_list1.loadvalues();

            frmMain.g_oFrmMain.DeactivateStandByAnimation();
                
            this.m_frmTreeDiamGroups.Show();
            this.m_frmTreeDiamGroups.Left = 0;
            this.m_frmTreeDiamGroups.Top = 0;
            this.m_frmTreeDiamGroups.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        private void BtnTreeSpeciesGroups_Click(object sender, EventArgs e)
        {

            frmMain.g_oFrmMain.ActivateStandByAnimation(
                frmMain.g_oFrmMain.WindowState,
                frmMain.g_oFrmMain.Left,
                frmMain.g_oFrmMain.Height,
                frmMain.g_oFrmMain.Width,
                frmMain.g_oFrmMain.Top);

            // Initialize Tree Species Group form
            this.m_frmTreeSpeciesGroups = new frmDialog(_frmProcessorScenario, frmMain.g_oFrmMain);
            this.m_frmTreeSpeciesGroups.MaximizeBox = false;
            this.m_frmTreeSpeciesGroups.BackColor = System.Drawing.SystemColors.Control;
            this.m_frmTreeSpeciesGroups.Text = "Processor: Tree Species Groups";
            this.m_frmTreeSpeciesGroups.Initialize_Processor_Tree_Species_Groups();
            this.m_frmTreeSpeciesGroups.Height = 0;
            this.m_frmTreeSpeciesGroups.Width = 0;
            if (this.m_frmTreeSpeciesGroups.uc_tree_spc_groups1.Top + m_frmTreeSpeciesGroups.uc_tree_spc_groups1.Height > this.m_frmTreeSpeciesGroups.ClientSize.Height + 2)
            {
                for (int x = 1; ; x++)
                {
                    this.m_frmTreeSpeciesGroups.Height = x;
                    if (m_frmTreeSpeciesGroups.uc_tree_spc_groups1.Top +
                        m_frmTreeSpeciesGroups.uc_tree_spc_groups1.Height <
                        this.m_frmTreeSpeciesGroups.ClientSize.Height)
                    {
                        break;
                    }
                }

            }
            if (m_frmTreeSpeciesGroups.uc_tree_spc_groups1.Left + m_frmTreeSpeciesGroups.uc_tree_spc_groups1.Width > this.m_frmTreeSpeciesGroups.ClientSize.Width + 2)
            {
                for (int x = 1; ; x++)
                {
                    this.m_frmTreeSpeciesGroups.Width = x;
                    if (m_frmTreeSpeciesGroups.uc_tree_spc_groups1.Left +
                        m_frmTreeSpeciesGroups.uc_tree_spc_groups1.Width <
                        this.m_frmTreeSpeciesGroups.ClientSize.Width)
                    {
                        break;
                    }
                }

            }

            if (_bCopyScenarioTreeSpeciesGroups == false)
            {
                m_frmTreeSpeciesGroups.uc_tree_spc_groups1.loadvalues();
            }
            else
            {
                //@ToDo: Implement this method
                //m_frmTreeSpeciesGroups.uc_tree_spc_groups1.loadvalues_FromProperties();
            }

            frmMain.g_oFrmMain.DeactivateStandByAnimation();
            this.m_frmTreeSpeciesGroups.Left = 0;
            this.m_frmTreeSpeciesGroups.Top = 0;
            this.m_frmTreeSpeciesGroups.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.m_frmTreeSpeciesGroups.Show();
        }
        }
}
