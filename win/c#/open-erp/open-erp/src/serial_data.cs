using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO.Ports;
using System.Threading;

namespace n_serial_data
{
    public enum Cmd
    {
        no_cmd=0,//0
    }

    public class Cdata
    {
        public static int buf_len=0x1000; 
        public static int data_len=0x10; //16bytes
        public static int s_count = 0x100;
        public static int min_len = 8; //data package min length
        public static int max_len = 0x40; //data package max length
        public byte[] s_buffer;//data send buffer 
        public byte[] r_buffer;//data recv buffer 
        public byte[] data;
        public int s_len; //s_len if>0 use for send if success should s_len =0
        public string[] s_recv;
        public int i_srecv;//current recv string index 
        public int i_deal;//index of deal 
        public bool in_deal = false;
        public bool debug = false;
        public Cmd current_deal_cmd = Cmd.no_cmd;

        public Cdata()
        {
            this.s_buffer = new byte[buf_len];// 4k bytes use send buffer
            this.r_buffer = new byte[buf_len];//use read buffer
            this.data=new byte[data_len];//16bytes
            this.s_recv = new string[s_count];

            this.s_len = 0;
            this.i_srecv = 0;
            this.i_deal = 0;
        }

        public byte get_length(byte[] buf, int start, int end, int len_offset=6,int type = 0)
        {
            if (start + len_offset > end) return (byte)0xFF;
            return buf[start + len_offset];
        }

        public void display(byte[]buf,int start,int len,string where="")
        {
            if(!this.debug) return;
            string s = where;
            for (int i = 0; i <len; i++)
            {
                s += buf[start + i].ToString("X2");
            }
            Console.WriteLine(s);
        }

        //name
        public bool is_print(byte b)
        {
            if (b >= 0x20 && b <= 0x7E) return true; //0x20 - 0x7e
            return false;
        }


        public int deal_cmd(byte[] buf ,int start, int len)
        {
            byte cmd = this.what_recv(buf, start, start+len);

            if (cmd == (byte)Cmd.no_cmd)
            {
                this.current_deal_cmd = Cmd.no_cmd;
                return -1;
            }
            return 0;
        }

        public void adjust_data_srecv()
        {
            if (this.i_srecv + 1 < Cdata.s_count) this.i_srecv++;//point next
            else this.i_srecv = 0;//form 0
        }

        public int deal_data(byte[] buf ,int start, int len,bool adjust=true)
        {
            int i;
            int end = start + len;

            this.s_recv[this.i_srecv] = "";
            for (i=start; i < end; i++)
            {
                this.s_recv[this.i_srecv]+=buf[i].ToString("X2");
            }
            if(this.debug) Console.WriteLine("deal_data()->"+this.s_recv[this.i_srecv]);//test

            this.deal_cmd(buf, start, len);//
            if(adjust) this.adjust_data_srecv();
            return start +len;
         }

        public byte what_recv() //return recv cmd 1-5
        {
            return (byte)Cmd.no_cmd;
        }

        public byte what_recv(byte[] data, int start, int end) //return recv cmd 1-5
        {
            return (byte)Cmd.no_cmd;
        }

        public byte what_cmd(byte[] data, int start, int end)  //package sub command
        {
            return (byte)0;
        }

        public byte what_cmd() //package sub command
        {
            return this.what_cmd(this.r_buffer,0,24);
        }

        public  int  copy(byte[] src,byte[] dir,int copy_len,int src_len,int dir_len,int src_offset=0,int dir_offset=0)
        {
            int i;
            for(i=0; i<copy_len&& src_offset+i < src_len&& dir_offset+i < dir_len;i++) dir[dir_offset+i] =src[src_offset+i];
            return i;
        }

        public void display(byte[] buf,int len )//test
        {
            if (!this.debug) return;
            string s = "";
            for(int i=0;i<len;i++) s += buf[i].ToString("X2");
            Console.WriteLine(s);
        }

    };
}
