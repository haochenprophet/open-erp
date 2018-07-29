using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace n_company
{
    public partial class company : Form
    {
        public bool is_mdi_child = false;
        public company()
        {
            InitializeComponent();
        }

        public company(Form parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
            this.is_mdi_child = true;
        }

        private void company_Load(object sender, EventArgs e)
        {
        }
    }
}
