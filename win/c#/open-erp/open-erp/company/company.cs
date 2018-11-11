using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//user namespace
using n_serial_data;
using n_serial;
using n_mysql;
using n_language;
using n_autosize;
using n_nopi;
using n_file;
using n_company;

namespace n_company
{

	public partial class company : Form
    {
		public LanguageSel language = LanguageSel.NoneInit;
		bool debug = true;
        public int margin= 5;
        public bool is_mdi_child = false;
        public n_file.Cfile logfile = new Cfile(System.AppDomain.CurrentDomain.BaseDirectory + "open_erp_company.log");

        public company()
        {
            InitializeComponent();
			this.set_language();
            this.AdjustSize();
        }

        public company(Form parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
            this.is_mdi_child = true;
        }

		public void set_language(LanguageSel sel = LanguageSel.English)
		{
			if (this.language == sel) return;//init once
			this.language = sel;
            this.Text = Clanguage.ls_company;
			this.fileToolStripMenuItem.Text = Clanguage.ls_file;//File
			this.databaseToolStripMenuItem.Text = Clanguage.ls_database;//DataBase
			this.stockToolStripMenuItem.Text = Clanguage.ls_stock;//Stock
			this.personnelToolStripMenuItem.Text = Clanguage.ls_personnel;//Personnel
			this.fiscalToolStripMenuItem.Text = Clanguage.ls_fiscal;//Fiscal
			this.materialsToolStripMenuItem.Text = Clanguage.ls_materials;//Materials
			this.projectToolStripMenuItem.Text = Clanguage.ls_project;//Project
			this.developToolStripMenuItem.Text = Clanguage.ls_develop;//Develop
			this.businessToolStripMenuItem.Text = Clanguage.ls_business;//Business
			this.legalAffairsToolStripMenuItem.Text = Clanguage.ls_legal_affairs;//LegalAffairs
			this.qualityToolStripMenuItem.Text = Clanguage.ls_quantity;//Quality
			this.envSafetyToolStripMenuItem.Text = Clanguage.ls_environment_safety;//environment
			this.requireToolStripMenuItem.Text = Clanguage.ls_require;//Require
			this.productToolStripMenuItem.Text = Clanguage.ls_product;//Product
			this.trainingToolStripMenuItem.Text = Clanguage.ls_training;//Training
			this.communicateToolStripMenuItem.Text = Clanguage.ls_communicate;//Communicate
			this.serviceToolStripMenuItem.Text = Clanguage.ls_service;//Service
			this.taskToolStripMenuItem.Text = Clanguage.ls_task;//Task
			this.questionToolStripMenuItem.Text = Clanguage.ls_question;//Question
			this.helpToolStripMenuItem.Text = Clanguage.ls_help;//Help
			this.organizationTabPage.Text = Clanguage.ls_organization;//Organization
			this.contactTabPage.Text = Clanguage.ls_contact;//Contact
			this.recordTabPage.Text = Clanguage.ls_record;//Record
			this.workspaceTabPage.Text = Clanguage.ls_workspace;//Workspace
			this.inputTabPage.Text = Clanguage.ls_input;//Input
			this.outputTabPage.Text = Clanguage.ls_output;//Output
		}

		private void AdjustSize()
        {
            this.navigateTabControl.Width = this.navigateSplitContainer.Width - this.margin;
            this.navigateTabControl.Height = this.navigateSplitContainer.Height - this.margin;
            this.workspaceTabControl.Width=this.workspaceSplitContainer.Panel1.Width- this.margin;
            this.workspaceTabControl.Height=this.workspaceSplitContainer.Panel1.Height- this.margin;
            this.messageTabControl.Width = this.workspaceSplitContainer.Panel2.Width - this.margin;
            this.messageTabControl.Height = this.workspaceSplitContainer.Panel2.Height - this.messageToolStrip.Height - this.margin;
            this.recordRichTextBox.Width = this.workspaceTabControl.Width;
            this.recordRichTextBox.Height = this.workspaceTabControl.Height;
            this.messageRichTextBox.Width = this.messageTabControl.Width;
            this.messageRichTextBox.Height = this.messageTabControl.Height - this.margin;
            this.orgTreeView.Width = this.navigateTabControl.Width;
            this.orgTreeView.Height = this.navigateTabControl.Height;
        }

        private void company_Load(object sender, EventArgs e)
        {
        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void messageRichTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void workspaceSplitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void workspaceSplitContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {
            this.AdjustSize();
        }

        private void fileToolStripButton_Click(object sender, EventArgs e)
        {
            this.recordRichTextBox.Text += this.messageRichTextBox.Text+"\n";
            this.messageRichTextBox.Text = "";
            try
            {
                this.recordRichTextBox.SelectionStart = this.recordRichTextBox.Text.Length; //Set the current caret position at the end
                this.recordRichTextBox.ScrollToCaret(); //Now scroll it automatically
            }
            catch(Exception ex)
            {
                if (this.debug) this.logfile.Append(ex.ToString());
            }

        }

		private void fileToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}
	}
}
