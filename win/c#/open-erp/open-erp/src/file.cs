using System;
using System.IO;
using System.Text;

namespace n_file
{
    public class Cfile
    {
        public bool debug = false;
        public string s_ex;
        public string file = System.AppDomain.CurrentDomain.BaseDirectory + "Cfile.txt";
        public FileStream fs = null;

        public Cfile(string file_name = null)
        {
            this.create(file_name);
        }

        ~Cfile()
        {
            this.close();
        }

        public void close()
        {
            if (this.fs != null)
            {
                this.fs.Close();
                this.fs = null;
            }
        }

        public bool create(string file_name = null)
        {
            if (file_name != null && file_name.Length > 0) this.file = file_name;
            try
            {
                if (File.Exists(this.file)) File.Delete(this.file);        // Delete the file if it exists.
                this.fs = File.Create(this.file);
                return true;
            }
            catch(Exception e)
            {
                this.fs = null;
                if (this.debug) Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool open(FileMode fm=FileMode.Open,FileAccess fa=FileAccess.ReadWrite)
        {
            try
            {
                this.fs = File.Open(this.file,FileMode.Open,FileAccess.ReadWrite);
                return true;
            }
            catch(Exception e)
            {
                if(this.debug) this.s_ex= e.ToString();
                this.fs = null;
                return false;
            }
        }

        public bool AddText(string value)
        {
            if (this.fs == null) return false;
            try
            {
                byte[] info = new UTF8Encoding(true).GetBytes(value);
                this.fs.Write(info, 0, info.Length);
                this.fs.Flush();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }//class Cfile
}