
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace n_autosize
{
    public class AutoSizeForm
    {
        public bool debug = false;
        public struct controlRect
        {
            public int left;
            public int top;
            public int width;
            public int height;
        }
        public List<controlRect> oldControlRects = new List<controlRect>();
        private int ctrlIndex = 0;
        private int columnWidth = 0;
        DataGridView dgv;
        public void InitOldControlRects(Control form)
        {
            controlRect ctR;
            ctR.left = form.Left;
            ctR.top = form.Top;
            ctR.width = form.Width;
            ctR.height = form.Height;
            oldControlRects.Add(ctR);
            AddControls(form);
        }
        private void AddControls(Control Control)
        {
            foreach (Control ctrl in Control.Controls)
            {
                controlRect ctR;
                ctR.left = ctrl.Left;
                ctR.top = ctrl.Top;
                ctR.width = ctrl.Width;
                ctR.height = ctrl.Height;
                oldControlRects.Add(ctR);
                if (ctrl.Controls.Count > 0)
                    AddControls(ctrl);
            }
            if (Control is DataGridView)
            {
                dgv = Control as DataGridView;
                foreach (DataGridViewColumn dgColum in dgv.Columns)
                {
                    controlRect ctR;
                    ctR.left = 0;
                    ctR.top = 0;
                    ctR.height = 0;
                    ctR.width = dgColum.Width;
                    oldControlRects.Add(ctR);
                }
            }

        }
        public void AutoSize(Control form)
        {
            controlRect ctR;
            ctR.left = 0;
            ctR.top = 0;
            ctR.width = form.PreferredSize.Width;
            ctR.height = form.PreferredSize.Height;
            float widthScale = (float)form.Width / (float)oldControlRects[0].width;
            float heightScale = (float)form.Height / (float)oldControlRects[0].height;
            ctrlIndex = 1;
            try
            {
                AutoSizeControls(widthScale, heightScale, form);
            }
            catch (Exception e)
            {
                if (this.debug) Console.WriteLine(e.ToString());
            }
        }
        private void AutoSizeControls(float widthSacle, float heightScale, Control Control)
        {
            foreach (Control ctrl in Control.Controls)
            {
                ctrl.Left = (int)(oldControlRects[ctrlIndex].left * widthSacle);
                ctrl.Top = (int)(oldControlRects[ctrlIndex].top * heightScale);
                ctrl.Height = (int)(oldControlRects[ctrlIndex].height * heightScale);
                ctrl.Width = (int)(oldControlRects[ctrlIndex].width * widthSacle);
                ctrlIndex++;
                if (ctrl.Controls.Count > 0)
                    AutoSizeControls(widthSacle, heightScale, ctrl);
                //列等宽版本
                /*    Cursor.Current = Cursors.WaitCursor;
                    int columnWidth = 0;
                    for(int i = 0; i < dgv.ColumnCount; i++)
                    {
                        dgv.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                        columnWidth += dgv.Columns[i].Width;
                    }
                    if (columnWidth > ctrl.Size.Width)
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
                    if (columnWidth < ctrl.Size.Width)
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                  Cursor.Current = Cursors.Default;*/
                //列等比版本
                if (ctrl is DataGridView)
                {
                    dgv = ctrl as DataGridView;
                    columnWidth = 0;
                    foreach (DataGridViewColumn dgColum in dgv.Columns)
                    {
                        dgColum.Width = (int)(oldControlRects[ctrlIndex].width * widthSacle);
                        columnWidth += dgColum.Width;
                        ctrlIndex++;

                    }
                    
                   if (columnWidth!= ctrl.Size.Width)
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    //     dgv.Columns[dgv.Columns.Count - 1].Width += ctrl.Size.Width - columnWidth;
                    //   if (columnWidth > ctrl.Size.Width)
                    //      dgv.Columns[dgv.Columns.Count - 1].Width -= ctrl.Size.Width - columnWidth;
                }
            }
        }

    }
}//end namespace n_autosize