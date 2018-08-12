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

namespace open_erp
{
    public partial class open_erp : Form
    {
        public static LanguageSel language = LanguageSel.NoneInit;
        n_company.company company;
        public bool set_mdi_child = false;
        public n_file.Cfile logfile = new Cfile(System.AppDomain.CurrentDomain.BaseDirectory + "open_erp.log");
        bool debug = true;
        public string s_ex;
        public int margin = 5;

        public open_erp()
        {
            InitializeComponent();
            //add code
            this.set_language(LanguageSel.English);
            if(this.set_mdi_child)
            {
                this.IsMdiContainer = true;
                this.company = new company(this);
                this.AdjustSize();
                this.company.Show();
            }
            else
            {
                this.company = new company();
            }
            this.AdjustSize();
        }

        private void AdjustSize()
        {
            this.navigateTabControl.Width = this.navigateSplitContainer.Width - this.margin;
            this.navigateTabControl.Height = this.navigateSplitContainer.Height - this.margin;
            this.workspaceTabControl.Width = this.workspaceSplitContainer.Panel1.Width - this.margin;
            this.workspaceTabControl.Height = this.workspaceSplitContainer.Panel1.Height - this.margin;
            this.messageTabControl.Width = this.workspaceSplitContainer.Panel2.Width - this.margin;
            this.messageTabControl.Height = this.workspaceSplitContainer.Panel2.Height - this.messageToolStrip.Height - this.margin;
            this.recordRichTextBox.Width = this.workspaceTabControl.Width;
            this.recordRichTextBox.Height = this.workspaceTabControl.Height;
            this.messageRichTextBox.Width = this.messageTabControl.Width-3;
            this.messageRichTextBox.Height = this.messageTabControl.Height-5;
            this.orgTreeView.Width = this.navigateTabControl.Width;
            this.orgTreeView.Height = this.navigateTabControl.Height;

            if (!this.set_mdi_child) return;
            this.company.Location = new System.Drawing.Point(this.Location.X, this.menuStrip.Size.Height + this.toolStrip.Size.Height);
            this.company.Size = new System.Drawing.Size(this.Size.Width-40, this.Size.Height - this.menuStrip.Size.Height - this.toolStrip.Size.Height-75);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(open_erp));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.companyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chineseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.navigateSplitContainer = new System.Windows.Forms.SplitContainer();
            this.workspaceSplitContainer = new System.Windows.Forms.SplitContainer();
            this.navigateTabControl = new System.Windows.Forms.TabControl();
            this.organizationTabPage = new System.Windows.Forms.TabPage();
            this.contactTabPage = new System.Windows.Forms.TabPage();
            this.workspaceTabControl = new System.Windows.Forms.TabControl();
            this.recordTabPage = new System.Windows.Forms.TabPage();
            this.workspaceTabPage = new System.Windows.Forms.TabPage();
            this.messageToolStrip = new System.Windows.Forms.ToolStrip();
            this.messageTabControl = new System.Windows.Forms.TabControl();
            this.inputTabPage = new System.Windows.Forms.TabPage();
            this.outputTabPage = new System.Windows.Forms.TabPage();
            this.recordRichTextBox = new System.Windows.Forms.RichTextBox();
            this.messageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.orgTreeView = new System.Windows.Forms.TreeView();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navigateSplitContainer)).BeginInit();
            this.navigateSplitContainer.Panel1.SuspendLayout();
            this.navigateSplitContainer.Panel2.SuspendLayout();
            this.navigateSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workspaceSplitContainer)).BeginInit();
            this.workspaceSplitContainer.Panel1.SuspendLayout();
            this.workspaceSplitContainer.Panel2.SuspendLayout();
            this.workspaceSplitContainer.SuspendLayout();
            this.navigateTabControl.SuspendLayout();
            this.organizationTabPage.SuspendLayout();
            this.workspaceTabControl.SuspendLayout();
            this.recordTabPage.SuspendLayout();
            this.messageTabControl.SuspendLayout();
            this.inputTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 464);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(866, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.groupToolStripMenuItem,
            this.settingToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(866, 25);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(42, 21);
            this.toolStripMenuItem1.Text = "Edit";
            // 
            // groupToolStripMenuItem
            // 
            this.groupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.companyToolStripMenuItem});
            this.groupToolStripMenuItem.Name = "groupToolStripMenuItem";
            this.groupToolStripMenuItem.Size = new System.Drawing.Size(57, 21);
            this.groupToolStripMenuItem.Text = "Group";
            // 
            // companyToolStripMenuItem
            // 
            this.companyToolStripMenuItem.Name = "companyToolStripMenuItem";
            this.companyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.companyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.companyToolStripMenuItem.Text = "Company";
            this.companyToolStripMenuItem.Click += new System.EventHandler(this.companyToolStripMenuItem_Click);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageToolStripMenuItem});
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.settingToolStripMenuItem.Text = "Setting";
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chineseToolStripMenuItem,
            this.englishToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.languageToolStripMenuItem.Text = "Language";
            // 
            // chineseToolStripMenuItem
            // 
            this.chineseToolStripMenuItem.Name = "chineseToolStripMenuItem";
            this.chineseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.chineseToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.chineseToolStripMenuItem.Text = "Chinese";
            this.chineseToolStripMenuItem.Click += new System.EventHandler(this.chineseToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStrip
            // 
            this.toolStrip.Location = new System.Drawing.Point(0, 25);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(866, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip";
            // 
            // navigateSplitContainer
            // 
            this.navigateSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigateSplitContainer.Location = new System.Drawing.Point(0, 50);
            this.navigateSplitContainer.Name = "navigateSplitContainer";
            // 
            // navigateSplitContainer.Panel1
            // 
            this.navigateSplitContainer.Panel1.Controls.Add(this.navigateTabControl);
            // 
            // navigateSplitContainer.Panel2
            // 
            this.navigateSplitContainer.Panel2.Controls.Add(this.workspaceSplitContainer);
            this.navigateSplitContainer.Size = new System.Drawing.Size(866, 414);
            this.navigateSplitContainer.SplitterDistance = 256;
            this.navigateSplitContainer.TabIndex = 3;
            // 
            // workspaceSplitContainer
            // 
            this.workspaceSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workspaceSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.workspaceSplitContainer.Name = "workspaceSplitContainer";
            this.workspaceSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // workspaceSplitContainer.Panel1
            // 
            this.workspaceSplitContainer.Panel1.Controls.Add(this.workspaceTabControl);
            this.workspaceSplitContainer.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel1_Paint);
            // 
            // workspaceSplitContainer.Panel2
            // 
            this.workspaceSplitContainer.Panel2.Controls.Add(this.messageTabControl);
            this.workspaceSplitContainer.Panel2.Controls.Add(this.messageToolStrip);
            this.workspaceSplitContainer.Size = new System.Drawing.Size(606, 414);
            this.workspaceSplitContainer.SplitterDistance = 246;
            this.workspaceSplitContainer.TabIndex = 0;
            // 
            // navigateTabControl
            // 
            this.navigateTabControl.Controls.Add(this.organizationTabPage);
            this.navigateTabControl.Controls.Add(this.contactTabPage);
            this.navigateTabControl.Location = new System.Drawing.Point(4, 4);
            this.navigateTabControl.Name = "navigateTabControl";
            this.navigateTabControl.SelectedIndex = 0;
            this.navigateTabControl.Size = new System.Drawing.Size(253, 410);
            this.navigateTabControl.TabIndex = 0;
            // 
            // organizationTabPage
            // 
            this.organizationTabPage.Controls.Add(this.orgTreeView);
            this.organizationTabPage.Location = new System.Drawing.Point(4, 22);
            this.organizationTabPage.Name = "organizationTabPage";
            this.organizationTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.organizationTabPage.Size = new System.Drawing.Size(261, 381);
            this.organizationTabPage.TabIndex = 0;
            this.organizationTabPage.Text = "Organization";
            this.organizationTabPage.UseVisualStyleBackColor = true;
            // 
            // contactTabPage
            // 
            this.contactTabPage.Location = new System.Drawing.Point(4, 22);
            this.contactTabPage.Name = "contactTabPage";
            this.contactTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.contactTabPage.Size = new System.Drawing.Size(245, 384);
            this.contactTabPage.TabIndex = 1;
            this.contactTabPage.Text = "Contact ";
            this.contactTabPage.UseVisualStyleBackColor = true;
            // 
            // workspaceTabControl
            // 
            this.workspaceTabControl.Controls.Add(this.recordTabPage);
            this.workspaceTabControl.Controls.Add(this.workspaceTabPage);
            this.workspaceTabControl.Location = new System.Drawing.Point(0, 4);
            this.workspaceTabControl.Name = "workspaceTabControl";
            this.workspaceTabControl.SelectedIndex = 0;
            this.workspaceTabControl.Size = new System.Drawing.Size(603, 239);
            this.workspaceTabControl.TabIndex = 0;
            // 
            // recordTabPage
            // 
            this.recordTabPage.Controls.Add(this.recordRichTextBox);
            this.recordTabPage.Location = new System.Drawing.Point(4, 22);
            this.recordTabPage.Name = "recordTabPage";
            this.recordTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.recordTabPage.Size = new System.Drawing.Size(595, 213);
            this.recordTabPage.TabIndex = 0;
            this.recordTabPage.Text = "Record";
            this.recordTabPage.UseVisualStyleBackColor = true;
            // 
            // workspaceTabPage
            // 
            this.workspaceTabPage.Location = new System.Drawing.Point(4, 22);
            this.workspaceTabPage.Name = "workspaceTabPage";
            this.workspaceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.workspaceTabPage.Size = new System.Drawing.Size(556, 257);
            this.workspaceTabPage.TabIndex = 1;
            this.workspaceTabPage.Text = "Workspace";
            this.workspaceTabPage.UseVisualStyleBackColor = true;
            // 
            // messageToolStrip
            // 
            this.messageToolStrip.Location = new System.Drawing.Point(0, 0);
            this.messageToolStrip.Name = "messageToolStrip";
            this.messageToolStrip.Size = new System.Drawing.Size(606, 25);
            this.messageToolStrip.TabIndex = 0;
            this.messageToolStrip.Text = "toolStrip1";
            // 
            // messageTabControl
            // 
            this.messageTabControl.Controls.Add(this.inputTabPage);
            this.messageTabControl.Controls.Add(this.outputTabPage);
            this.messageTabControl.Location = new System.Drawing.Point(0, 28);
            this.messageTabControl.Name = "messageTabControl";
            this.messageTabControl.SelectedIndex = 0;
            this.messageTabControl.Size = new System.Drawing.Size(603, 133);
            this.messageTabControl.TabIndex = 1;
            // 
            // inputTabPage
            // 
            this.inputTabPage.Controls.Add(this.messageRichTextBox);
            this.inputTabPage.Location = new System.Drawing.Point(4, 22);
            this.inputTabPage.Name = "inputTabPage";
            this.inputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.inputTabPage.Size = new System.Drawing.Size(595, 107);
            this.inputTabPage.TabIndex = 0;
            this.inputTabPage.Text = "Input";
            this.inputTabPage.UseVisualStyleBackColor = true;
            // 
            // outputTabPage
            // 
            this.outputTabPage.Location = new System.Drawing.Point(4, 22);
            this.outputTabPage.Name = "outputTabPage";
            this.outputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.outputTabPage.Size = new System.Drawing.Size(592, 107);
            this.outputTabPage.TabIndex = 1;
            this.outputTabPage.Text = "Output";
            this.outputTabPage.UseVisualStyleBackColor = true;
            // 
            // recordRichTextBox
            // 
            this.recordRichTextBox.Location = new System.Drawing.Point(-4, 0);
            this.recordRichTextBox.Name = "recordRichTextBox";
            this.recordRichTextBox.Size = new System.Drawing.Size(596, 217);
            this.recordRichTextBox.TabIndex = 0;
            this.recordRichTextBox.Text = "";
            this.recordRichTextBox.TextChanged += new System.EventHandler(this.recordRichTextBox_TextChanged);
            // 
            // messageRichTextBox
            // 
            this.messageRichTextBox.Location = new System.Drawing.Point(-7, 0);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.Size = new System.Drawing.Size(596, 104);
            this.messageRichTextBox.TabIndex = 0;
            this.messageRichTextBox.Text = "";
            // 
            // orgTreeView
            // 
            this.orgTreeView.Location = new System.Drawing.Point(0, 0);
            this.orgTreeView.Name = "orgTreeView";
            this.orgTreeView.Size = new System.Drawing.Size(261, 378);
            this.orgTreeView.TabIndex = 0;
            // 
            // open_erp
            // 
            this.ClientSize = new System.Drawing.Size(866, 486);
            this.Controls.Add(this.navigateSplitContainer);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "open_erp";
            this.Text = "open_erp";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.open_erp_Load);
            this.Resize += new System.EventHandler(this.open_erp_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.navigateSplitContainer.Panel1.ResumeLayout(false);
            this.navigateSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.navigateSplitContainer)).EndInit();
            this.navigateSplitContainer.ResumeLayout(false);
            this.workspaceSplitContainer.Panel1.ResumeLayout(false);
            this.workspaceSplitContainer.Panel2.ResumeLayout(false);
            this.workspaceSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workspaceSplitContainer)).EndInit();
            this.workspaceSplitContainer.ResumeLayout(false);
            this.navigateTabControl.ResumeLayout(false);
            this.organizationTabPage.ResumeLayout(false);
            this.workspaceTabControl.ResumeLayout(false);
            this.recordTabPage.ResumeLayout(false);
            this.messageTabControl.ResumeLayout(false);
            this.inputTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void set_language(LanguageSel sel= LanguageSel.English)
        {
            if (open_erp.language == sel) return;//init once
            open_erp.language = sel;

            Clanguage.sel_language(sel);
            this.fileToolStripMenuItem.Text = Clanguage.ls_file+ "(F)";
            this.openToolStripMenuItem.Text = Clanguage.ls_open;
            this.newToolStripMenuItem.Text = Clanguage.ls_new;
            this.exitToolStripMenuItem.Text = Clanguage.ls_exit;
            this.settingToolStripMenuItem.Text = Clanguage.ls_setting + "(S)";
            this.languageToolStripMenuItem.Text = Clanguage.ls_language;
            this.chineseToolStripMenuItem.Text = Clanguage.ls_chinese;
            this.englishToolStripMenuItem.Text = Clanguage.ls_english;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chineseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.set_language(LanguageSel.Chinese);
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.set_language(LanguageSel.English);
        }

        private void open_erp_Load(object sender, EventArgs e)
        {
            //this.companyToolStripMenuItem_Click(sender, e);
        }
        private void open_erp_Resize(object sender, EventArgs e)
        {
            this.AdjustSize();
        }

        private void companyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.company.ShowDialog();
            }
            catch(Exception ex)
            {
                if (this.debug) this.logfile.Append(ex.ToString());
            }
        }

        private void recordRichTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {
            this.AdjustSize();
        }
    }
}
