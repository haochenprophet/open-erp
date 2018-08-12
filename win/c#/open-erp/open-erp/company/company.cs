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
        bool debug = true;
        public int margin= 5;
        public bool is_mdi_child = false;
        public n_file.Cfile logfile = new Cfile(System.AppDomain.CurrentDomain.BaseDirectory + "open_erp_company.log");
        public company()
        {
            InitializeComponent();
            this.AdjustSize();
        }

        public company(Form parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
            this.is_mdi_child = true;
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
    }
}
