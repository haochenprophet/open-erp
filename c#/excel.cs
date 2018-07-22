using System;
using System.Text;
using System.IO;
using System.Data;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;


/*
//refer: http://npoi.codeplex.com/
//refer :https://msdn.microsoft.com/zh-tw/ee818993.aspx
*/

namespace n_nopi
{
    public class Cexcel
    {
        public bool debug = true;
        private string fileName = null;
        private IWorkbook workbook = null;
        public FileStream fs = null;
        public string s_ex = "";
        public string time_format;
        public string default_create_file;
        public string default_open_file;
        //System.AppDomain.CurrentDomain.BaseDirectory + this.default_save_file

        public Cexcel()
        {
            this.time_format = "yyyy_MM_dd_HH_mm_ss_ffff";
            this.default_create_file = System.AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString(this.time_format) + ".xls";
            this.default_open_file = this.default_create_file;
        }

        ~Cexcel()
        {
            try
            {
                if (this.fs != null) fs.Close();
            }
            catch (Exception e)
            {
                if (this.debug) Console.WriteLine(e.ToString());
            }
        }

        public bool create(string[] sheet = null)
        {
            return this.create(this.default_create_file, sheet);
        }

        public bool create(string fileName, string[] sheet = null)
        {
            if (fileName.Length < 1) return false;

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            hssfworkbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            hssfworkbook.SummaryInformation = si;

            //here, we must insert at least one sheet to the workbook. otherwise, Excel will say 'data lost in file'
            //So we insert three sheet just like what Excel does
            if (sheet == null) hssfworkbook.CreateSheet("Sheet1");
            else
            {
                foreach (string s in sheet)
                {
                    if (s.Length > 0) hssfworkbook.CreateSheet(s);
                }
            }

            //Write the stream data of workbook to the root directory
            if (fileName.IndexOf(".xls") <= 0) fileName += ".xls";
            FileStream file = new FileStream(fileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return true;
        }

        public void display(DataTable d)
        {
            if (d == null || !this.debug) return;

            Console.WriteLine("Columns=" + d.Columns.Count);
            int n = -1;
            foreach (DataRow dr in d.Rows)
            {
                Console.WriteLine(dr.ToString() + "[" + ++n + "]");
                foreach (DataColumn dc in d.Columns)
                {
                    Console.WriteLine(dr[dc]);
                }
            }
        }

        public bool write_to_file(DataTable d, string file, int col = -1, int auto_inc = -1)
        {
            if (d == null || file.Length < 1) return false;
            try
            {

                FileStream fs = new FileStream(file, FileMode.Create);
                byte[] data;
                foreach (DataRow dr in d.Rows)
                {
                    if (col >= 0 && col < d.Columns.Count)
                    {
                        if (this.debug) Console.WriteLine(dr[col]);

                        if (auto_inc >= 0) data = Encoding.Default.GetBytes(++auto_inc + "\t" + dr[col].ToString() + "\n");
                        else data = Encoding.Default.GetBytes(dr[col].ToString() + "\n");

                        fs.Write(data, 0, data.Length);
                        continue;
                    }

                    foreach (DataColumn dc in d.Columns)
                    {
                        if (this.debug) Console.WriteLine(dr[dc]);

                        if (auto_inc >= 0) data = Encoding.Default.GetBytes(++auto_inc + "\t" + dr[dc].ToString() + "\t");
                        else data = Encoding.Default.GetBytes(dr[dc].ToString() + "\t");

                        fs.Write(data, 0, data.Length);
                    }

                    data = Encoding.Default.GetBytes("\n");
                    fs.Write(data, 0, data.Length);
                }

                fs.Flush();
                fs.Close();
                return true;

            }
            catch (Exception e)
            {
                if (this.debug) Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool open(string file = null)
        {
            try
            {
                if (file != null) this.fileName = file;
                this.fs = new FileStream(this.fileName, FileMode.Open, FileAccess.Read);

                if (this.fileName.IndexOf(".xlsx") > 0) // xlsx
                {
                    this.workbook = new XSSFWorkbook(this.fs);
                    return true;
                }

                if (this.fileName.IndexOf(".xls") > 0) // xls
                {
                    this.workbook = new HSSFWorkbook(this.fs);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                this.s_ex = e.ToString();
                if (this.debug) Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool write(ISheet sheet, int row, int col, string value)
        {
            if (sheet == null) return false;
            if (row < 1) row = 1;
            if (col < 1) col = 1;
            if (this.debug) Console.WriteLine("row=" + row + ",col=" + col + ",value=" + value);
            try
            {
                for (int i = sheet.LastRowNum; i < row; i++)
                {
                    IRow arow = sheet.CreateRow(i); //add row
                }

                IRow irow = sheet.GetRow(row - 1);
                if (irow == null) return false;
                for (int j = irow.Cells.Count; j < col; j++)//check col is exist
                {
                    irow.CreateCell(j).SetCellValue("");
                }
                irow.Cells[col - 1].SetCellValue(value);
                return true;
            }
            catch (Exception e)
            {
                this.s_ex = e.ToString();
                if (this.debug) Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool write(string file, int row, int col, string value, string sheetName = null)
        {
            if (!this.open(file)) return false;

            try
            {
                bool ret = false;
                ISheet sheet = null;
                if (sheetName != null) sheet = this.workbook.GetSheet(sheetName);
                if (sheet == null) sheet = this.workbook.GetSheetAt(0);
                if (sheet == null) { this.fs.Close(); return false; }
                ret = this.write(sheet, row, col, value);
                if (ret) this.write_back();
                return ret;
            }
            catch (Exception e)
            {
                this.s_ex = e.ToString();
                if (this.debug) Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool write_back()
        {
            try
            {
                //Write the stream data of workbook to the root directory
                FileStream file = new FileStream(this.fileName, FileMode.Create);
                this.workbook.Write(file);
                file.Close();
                return true;
            }
            catch (Exception e)
            {
                this.s_ex = e.ToString();
                if (this.debug) Console.WriteLine(e.ToString());
                return false;
            }
        }

        public DataTable read(string file = null, string sheetName = null, bool isFirstRowColumn = false, int startRow = 0)
        {
            int i, n, cellCount;
            ISheet sheet = null;
            IRow irow = null, max_col_row=null;
            DataTable data = new DataTable();

            if (!this.open(file)) return null;

            try
            {
                if (sheetName != null)
                {
                    sheet = this.workbook.GetSheet(sheetName);
                    if (sheet == null) //if not find sheetName get first sheet
                    {
                        sheet = this.workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = this.workbook.GetSheetAt(0);
                }

                if (sheet == null) { this.fs.Close(); return null; }

                for (n = 0, cellCount = 0; n <= sheet.LastRowNum; n++)// get max col 
                {
                    irow = sheet.GetRow(n);
                    if (irow == null) continue;
                    if (irow.LastCellNum > cellCount)
                    {
                        max_col_row = irow;
                        cellCount = irow.LastCellNum;
                    }
                }

                if (max_col_row == null) return null;
                if (cellCount < 1) cellCount = 1;//default to 1

                for (i = max_col_row.FirstCellNum, n = 0; i < cellCount; ++i)
                {
                    ICell cell = max_col_row.GetCell(i);
                    if (cell != null)
                    {
                        string cellValue = cell.ToString();
                        if (cellValue != null)
                        {
                            DataColumn column = new DataColumn("col" + n);
                            data.Columns.Add(column);
                            n++;
                        }
                    }
                }

                if (isFirstRowColumn) //2 
                {
                    startRow = sheet.FirstRowNum + 1;
                }
                else//1
                {
                    startRow = sheet.FirstRowNum;
                }

                //last row count 
                int rowCount = sheet.LastRowNum;
                for (i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //empty row  data default is null

                    DataRow dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null) dataRow[j] = row.Cells[j].ToString();
                    }

                    data.Rows.Add(dataRow);
                }

                return data;
            }
            catch (Exception e)
            {
                this.s_ex = e.ToString();
                if (this.debug) Console.WriteLine("Exception: " + e.ToString());
                this.fs.Close();
                return null;
            }
        }


    }//end Cexcel
}//n_nopi