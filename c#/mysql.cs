
using System;
using System.IO;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

//sql refer http://www.w3school.com.cn/sql/sql_orderby.asp
//GRANT ALL PRIVILEGES ON *.* TO 'myuser'@'%' IDENTIFIED BY 'mypassword' WITH GRANT OPTION; 

namespace n_mysql
{
    public delegate void Callback(object state);
    public interface Ideal
    {
          int deal_rdr();
    }

   public enum ExeType
    {
        Select,
        Insert,
        Script,
    }

    public enum MysqlStatus
    {
        None,
        NewConn,
        NewCmd,
        OpenConn,
        Select,
        Insert,
        Script,
        Success,
        Close,
        NewConnExecption,
        NewCmdExecption,
        OpenConnExecption,
        SelectExecption,
        InsertExecption,
        ScriptExecption,
        CloseExecption,
    }

    public class Cmysql: Ideal
    {
        public  string conn_str;
        public string sql;
        public  MySqlConnection conn;
        public  MySqlDataReader rdr=null;
        public int rdr_count=0;
        public  bool con_display = true;
        public bool is_exec = false;
        public bool is_conn = false;
        public bool debug =true;
        public MysqlStatus status = MysqlStatus.None;
        public string ex_s;//exception string 
        public string secure_file_priv;//Windows OS : "C:/ProgramData/MySQL/MySQL Server 8.0/Uploads";
        public int script_error_count;
        //conn
        public string server= "localhost";
        public string user = "test";
        public string database="test";
        public string port = "3306";
        public string password = "test@123";
        public string charset = "utf8";

        public Callback callback =null;

        public string build_conn(string server, string user, string database, string port, string password)
        {
            string conn_s = "server="+server+";user="+user+";database="+database+";port="+port+";password="+password+";"+ "Charset = "+ charset+"; " ;
            return conn_s;
        }

        public string build_conn()
        {
            this.conn_str=this.build_conn(this.server,this.user,this.database,this.port,this.password);
            return this.conn_str;
        }

        public Cmysql()
        {
            this.build_conn();
        }

        public Cmysql(string  conn_str)
        {
            this.conn_str = conn_str;
            this.secure_file_priv = "";
            this.ex_s = "";
        }
   
         ~Cmysql()
        {
            this.close();
        }

        virtual public int deal_rdr()
        {
            this.rdr_count = 0;
            while (this.con_display && this.rdr.Read())
            {
                this.rdr_count++;
                if (this.debug)
                {
                    for (int i = 0; i < this.rdr.FieldCount; i++)  Console.WriteLine(this.rdr[i]+"\t");
                }
            }
            this.close();
            return this.rdr_count;
        }

        public bool test_conn()
        {
            return this.test_conn(this.conn_str);
        }

        public bool test_conn(string conn_s)
        {
            bool ret = false;
            MySqlConnection conn = new MySqlConnection(conn_s);
            try
            {
                if(this.debug) Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // Perform database operations
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                if (this.debug) Console.WriteLine(ex.ToString());
            }
            conn.Close();
            if (this.debug) Console.WriteLine("Done.");
            return ret;
        }

        public bool command(string sql, ExeType exe_type, bool deal = false)
        {
            return this.command(this.conn_str,sql, exe_type, deal);
        }

        public void post_text(object state)
        {
            if (this.callback != null) this.callback(state);
        }
        public bool command(string conn_str, string sql, ExeType exe_type,bool deal =false)
        {
            //if (conn_str.Length > 0) this.conn_str = conn_str;//rm save
            this.is_exec = false;
            if (this.debug) this.post_text(conn_str);
            this.post_text(sql);
            //New
            if (!this.is_conn)
            {
                try
                {
                    this.status = MysqlStatus.NewConn;
                    this.conn = new MySqlConnection(this.conn_str);
                    this.is_conn = false;
                }
                catch (Exception ex)
                {
                    this.status = MysqlStatus.NewConnExecption;
                    if (this.debug) Console.WriteLine(ex.ToString());
                    else this.ex_s = ex.ToString();
                    this.post_text(ex.ToString());
                    return false;
                }
            }

            // Open
            if (!this.is_conn)
            {
                     try
                    {
                        if(this.debug) Console.WriteLine("Connecting to MySQL...");
                        this.status = MysqlStatus.OpenConn;
                        this.conn.Open();
                        this.is_conn = true;
                    }
                    catch (Exception ex)
                    {
                        this.status = MysqlStatus.OpenConnExecption;
                        if (this.debug) Console.WriteLine(ex.ToString());
                        else this.ex_s = ex.ToString();
                        this.post_text(ex.ToString());
                        return false;
                    }
            }

            //check is_conn
            if (!this.is_conn) return false;
            //new cmd
            MySqlCommand cmd;
            try
            {
                this.status = MysqlStatus.NewCmd;
                cmd = new MySqlCommand(sql, this.conn);
             }
             catch (Exception ex)
            {
                      this.status = MysqlStatus.NewCmdExecption;
                      if (this.debug) Console.WriteLine(ex.ToString());
                      else this.ex_s = ex.ToString();
                      this.post_text(ex.ToString());
                      return false;
             }

            //Select
            if (exe_type == ExeType.Select)
            {
                    try
                    { 
                           this.status = MysqlStatus.Select;
                           this.rdr = cmd.ExecuteReader();
                            if (deal) deal_rdr();
                    }
                    catch (Exception ex)
                    {
                        this.status = MysqlStatus.SelectExecption;
                        if (this.debug) Console.WriteLine(ex.ToString());
                        else this.ex_s = ex.ToString();
                        this.post_text(ex.ToString());
                        return false;
                    }
            }

            //insert
            if (exe_type == ExeType.Insert)
            {
                    try
                    {
                              this.status = MysqlStatus.Insert;
                              cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        this.status = MysqlStatus.InsertExecption;
                        if(this.debug) Console.WriteLine(ex.ToString());
                        else this.ex_s = ex.ToString();
                        this.post_text(ex.ToString());
                        return false;
                    }
            }

            //Script
            if (exe_type == ExeType.Script)
            {
                this.script_error_count = 0;
                    try
                    { 
                        MySqlScript script = new MySqlScript(this.conn, sql);

                        script.Error += new MySqlScriptErrorEventHandler(this.script_Error);
                        script.ScriptCompleted += new EventHandler(this.script_ScriptCompleted);
                        script.StatementExecuted += new MySqlStatementExecutedEventHandler(this.script_StatementExecuted);

                        int count = script.Execute();
                        if (debug) Console.WriteLine("Executed " + count + " statement(s).");
                        if (debug) Console.WriteLine("Delimiter: " + script.Delimiter);
                  }
                catch (Exception ex)
                {
                    this.script_error_count++;
                    this.status = MysqlStatus.ScriptExecption;
                    if(debug) Console.WriteLine(ex.ToString());
                    else this.ex_s = ex.ToString();
                    this.post_text(ex.ToString());
                    return false;
                }
                if (this.script_error_count > 0) return false;
            }


            this.is_exec = true;
            return true;
        }

        public void script_StatementExecuted(object sender, MySqlScriptEventArgs args)
        {
            if (debug) Console.WriteLine("script_StatementExecuted");
        }

        public void script_ScriptCompleted(object sender, EventArgs e)
        {
            /// EventArgs e will be EventArgs.Empty for this method
            if (debug) Console.WriteLine("script_ScriptCompleted!");
        }

        public void script_Error(Object sender, MySqlScriptErrorEventArgs args)
        {
            if (debug) Console.WriteLine("script_Error: " + args.Exception.ToString());
            this.script_error_count++;
            this.ex_s = args.Exception.ToString();
            this.post_text(this.ex_s);
        }

        public bool transaction(string conn_str, string sql, ExeType exe_type,bool deal=false)
        {
            string s_task="begin ; "+sql+" commit ; ";
            return this.command(conn_str,s_task, exe_type,deal);
        }

        public int is_exist(string db_tab, string col, string value)//db_tab="bluetooth.mac"
        {
            this.sql = "SELECT " + col + " FROM " + db_tab + " WHERE " + col + " = " + value + ";";
            if (this.debug) Console.WriteLine(this.sql);
            if (this.transaction(this.conn_str, sql, ExeType.Select, true)) return this.rdr_count;
            return -1;
        }

        public bool source(string sql_file)//db_tab="bluetooth.mac"
        {
            try
            {
                StreamReader sr = new StreamReader(sql_file);
                string s_sql = sr.ReadToEnd();
                sr.Close();
                return this.transaction(this.conn_str, s_sql, ExeType.Script);
            }
            catch(Exception e)
            {
                this.post_text(e.ToString());
                return false;
            }
        }

        public void close()
        {
            if(this.debug) this.post_text("Cmysql.close()");
            if(is_exec)
            {
                try
                {
                    if(this.rdr!=null) this.rdr.Close();
                }
                catch (Exception ex)
                {
                    if (this.debug) Console.WriteLine(ex.ToString());
                }
                
                this.is_exec = false;
            }

            if (this.is_conn)
            {
                try
                {
                    this.conn.Close();
                }
                catch (Exception ex)
                {
                    if (this.debug) Console.WriteLine(ex.ToString());
                }

                this.is_conn = false;
            }
        }

    }//end public class Cmysql

}//end namespace n_mysql