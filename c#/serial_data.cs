using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO.Ports;
using System.Threading;

using n_mysql;

namespace n_feike
{

    public enum Cmd
    {
        no_cmd=0,//0
        scan=1,//1
        connect=2,//2
        disconnect=3,//3
        package=4,//4 point -> PackageCmd
        status =5,//5
        in_reset = 0xFC,
        out_reset =0xFD,
        sel_mac=0xFE,
    }

    public enum PackageCmd
    {
        Time=0xC1,
        Weight=0xD1,
        Stable=0xD2,
        History=0xD3,
        Update=0xC3,
        UpdateEnd=0xC4,
        Mode=0xC2,
        SetTestResult = 0xF1,
        Function, //note note set(=) value , and point->FunctionCmd
    }

    public enum FunctionCmd
    {
        QueryMac=0x00,
        QueryRssi = 0x01,
        QueryStatus = 0x02,
        Reset = 0x03,
        Disconnect = 0x04,
        Sleep = 0x05,
        QueryName = 0x06,
        ModifyName=0x07,
        QueryBaudrate=0x08,
        ModifyBaudrate=0x09,
        QueryFindStatus=0x0A,
        ModifyFindStatus = 0x0B,
        QueryPID=0x0C,//Query product identification code
        ModifyPID = 0x0D,
        QueryBroadcast=0x0E,  //Query  broadcast content
        ModifyBroadcast=0x0F,
        QueryBroadcastCycle=0x10,//Query the broadcast cycle
        ModifyBroadcastCycle=0x11,
        QueryPower=0x12,//Query the transmit power
        ModifyPower=0x13,
        QueryFwVersion=0x14,//Query the firmware version
        QueryBroadcastTime=0x15,//Query the broadcast time
        ModifyBroadcastTime = 0x16,//Query the broadcast time
        QueryServiceUUID=0x17,
        QueryServiceWirteUUID=0x18,
        QueryServiceNotifyUUID=0x19,
    }

    public enum DeviceType
    {
        no_type,
        fh7001,//FLYCO_FH7001_2_3;
        fh7005,//FLYCO_FH7005_6_8; 
    }


    public enum FactoryType
    {
        TELINK = 0x0211,
        AMICCOM = 0x00C0,
        CYPRESS = 0x0131,
    }

    public enum WeightType
    {
        kilogram,
        catty,
        pound,
    }
    public enum ResultType
    {
        NotTest = -1,
        Pass,
        Warning,
        Fail,
    }

    public class Ctestitem
    {
        public int row = -1;
        public bool select;
        public string id;
        public string item;
        public string min;
        public string max;
        public string mask;
        public string must;
        public string value;
        public string status;

        public double d_min=0;
        public double d_max=0;
        public double d_value=0;
        public UInt64 u64_value=0;

       public Ctestitem()
        {
            this.select = false;
            this.id = "";
            this.item = "";
            this.min = "";
            this.max = "";
            this.mask = "";
            this.must = "";
            this.value = "";
            this.status = "";
        }

        public string record(ResultType rt )
        {
            string s="";
            if (rt == ResultType.NotTest) s = "NA:";
            if (rt == ResultType.Fail) s = "Fail:";
            if (rt == ResultType.Pass) s = "Pass:";
            if (rt == ResultType.Warning) s = "Warning:";
            s +=/* this.item +*/ "(min=" + this.min + ",max=" + this.max + ",mask=" + this.mask + ",must=" + this.must + ",value=" + this.value + ")";
            return s;
        }

        public bool convert()
        {
            try
            {
                this.d_min = Convert.ToDouble(this.min);
            }
            catch
            {
                return false;
            }

            try
            {
                this.d_max = Convert.ToDouble(this.max);
            }
            catch
            {
                return false;
            }

            try
            { 
                this.d_value = Convert.ToDouble(this.value);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

    public class Cmac
    {
        public int id;
        public byte[] mac=new byte[Cdata.mac_len];//
        public string s_mac = "";//xx xx xx xx xx xx
        public string mac_s=""; //xx:xx:xx:xx:xx:xx
        public UInt64 u64_mac=0;// mac 64 bit
        public int rssi = int.MinValue;
        public string name="";
        public string version = "";
        public bool linked=false;
        public bool locked=false;
        public bool ditto;
        public UInt16 factory_type = 0;
        public DeviceType type;

        public Cmac()
        {
            this.init();
        }
        public void init()
        {
            this.id = 0;
            this.mac= new byte[Cdata.mac_len];//
            this.s_mac = "";
            this.mac_s = "" ;
            this.u64_mac = 0;
            this.rssi = int.MinValue;
            this.name = "";
            this.linked = false;
            this.ditto = false;
            this.locked = false;
            this.type = DeviceType.no_type;
        }

        public void set_mac()
        {
            for (int i = 0; i < Cdata.mac_len; i++)
            {
                this.mac[i] = (byte)(this.u64_mac >> ((Cdata.mac_len - 1 - i) * 8) & 0xFF);
            }
        }

        public void set_mac(UInt64 mac)
        {
            this.u64_mac = mac;
            this.set_mac();
        }

        public void mac_to_s(string s)
        {
            s = "";
            for (int i = 0; i < Cdata.mac_len; i++)
            {
                s += this.mac[i].ToString("X2");
                if (i < Cdata.mac_len - 1) s += ":";
            }
        }

        public void mac_to_s()
        {
            this.mac_s = "";
            for (int i = 0; i < Cdata.mac_len; i++)
            {
                this.mac_s += this.mac[i].ToString("X2");
                if (i < Cdata.mac_len - 1) this.mac_s += ":";
            }
        }

        public void set_u64_mac(byte[] buf, int len = Cdata.mac_len)
        {
            this.u64_mac = 0;
            for (int i = 0; i < len; i++)
            {
                this.u64_mac <<= 8;
                this.u64_mac |= ((UInt64)buf[i] & 0xFF);
            }
        }

        public void set_u64_mac()
        {
            this.set_u64_mac(this.mac, Cdata.mac_len);
        }

        public void display(bool debug =true,int i=0,string where="")
        {
            if (debug) Console.WriteLine(where+"[" +i+"]:"+"Cmac:s_mac=" + this.s_mac+",mac_s="+this.mac_s+",u64_mac="+this.u64_mac+",rssi="+this.rssi+",linked="+this.linked+ ",factory_type="+ this.factory_type+",name="+this.name);
        }
    }

    public class Cdata
    {
        public const int buf_len=0x1000; 
        public const int data_len=0x10; //16bytes
        public const int s_count = 0x100;
        public const int min_len=8; //data package min length
        public const int max_len = 0x40; //data package max length
        public const int mac_len = 0x6; //data package max length
        public const int mac_offset = 10;  //head[0-7]:78,73,63,73,01,01,01,00; rssi[8-9]:00,C1; mac[10-15]:6D,12,99,50,A0,00
        public const int rssi_offset = 8;
        public const int data_len_offset = 6;
        public byte[] s_buffer;//data send buffer 
        public byte[] r_buffer;//data recv buffer 
        public byte[] data;
        public int s_len; //s_len if>0 use for send if success should s_len =0
        public string[] s_recv;
        public int i_srecv;//current recv string index 
        public int i_deal;//index of deal 
        public bool in_deal = false;
        //CMD
        public byte[] send_sign = { 0x58, 0x53, 0x43, 0x53 };
        public byte[] recv_sign = { 0x78, 0x73, 0x63, 0x73 };
        public const int sign_len = 4;
        
        // CONNECT
        public string s_conn = "";
        public const int conn_offset = Cdata.min_len;
        public bool isset_connect=false;
        //DISCONNECT
        public bool is_wait_disconn = false;
        public bool isset_disconnect=false;

        //MAC
        public Cmac [] mac; // mac buf
        public bool isset_mac = false;
        public int mac_sel;//mac select
        public string s_c_mac = "";//record current read mac
        public string s_mac_sel = "";
        public int i_mac;//mac_index 
        public int mac_count = 0;
        //RSSI
        public byte rssi;
        public int rc_rssi; //record current rssi
        public string s_rssi;
        public bool isset_rssi=false;
        public int rssi_min_limit = int.MinValue; //range check for connect
        public int rssi_max_limit = int.MaxValue;
        //weight
        public const int weigt_offset= Cdata.min_len+0x7;//high  weight byte of package,low byte = weigt_offset+1
        public bool convert_kilo = false;
        public int weight;
        public float f_weight;
        public float kilo_weight;
        public int weight_type;
        public int weight_stable;
        public bool isset_weigt = false;
        //resistance
        public const int resistance_offset = Cdata.min_len + 0x9;//high  weight byte of package,low byte = weigt_offset+1
        public int resistance;
        public bool isset_resistance=false;
        //voltage
        public const int voltage_offset_7001 = Cdata.min_len + 12;//voltage offset 
        public const int voltage_offset_7005 = Cdata.min_len + 6;//voltage offset
        public int voltage;
        public bool isset_voltage;
        public float f_voltage;
        //stable
        public bool isset_stable = false;
        public bool is_test = false;
        //set test result
        public byte result = 0xFF;
        public bool isset_test_result=false;
        //version
        public bool isset_version = false;
        public string s_version;//string version
        //name 
        public bool isset_name = false;
        public string s_name;//string name
        public const int name_offset = Cdata.min_len + 13;//8+8+5
        public const int ble_ad_offset = Cdata.min_len+8;//buf[16]=AD0-length
        //debug
        public bool debug = true;

        //Function Command
        public Cmd current_cmd = Cmd.no_cmd;//for test
        public Cmd current_deal_cmd = Cmd.no_cmd;//for test
        public const int func_buf_len = 0x100;
        public const int func_sign_offset = 8;//from byte 8
        public const int func_sign_len = 5;//=sizeof func_send_sign
        public const int func_cmd_offset = (Cdata.func_sign_offset + Cdata.func_sign_len);//byte[13]
        public const int func_len_offset = (Cdata.func_sign_offset + Cdata.func_sign_len+1);//byte[14]
        public const int func_data_offset = (Cdata.func_sign_offset + Cdata.func_sign_len + 2);//byte[15]
        public const int func_checksum_base= (Cdata.func_sign_offset + Cdata.func_sign_len + 2);//byte[15]
        public const int func_version_offset = (func_data_offset);//for get_version

        public byte[] func_send_sign = { 0x46, 0x4C, 0x59, 0x43, 0x4F };
        public byte[] func_recv_sign = { 0x66, 0x65, 0x69, 0x6B, 0x65 };
        //heartbeat
        public int heartbeat=0;
        //factory_type
        public bool auto_ble_chip = true;
        public UInt16 factory_type = 0;
        public bool in_clear = false;
        public string s_chip = "";

        public Cdata()
        {
            this.s_buffer = new byte[buf_len];// 4k bytes use send buffer
            this.r_buffer = new byte[buf_len];//use read buffer
            this.data=new byte[data_len];//16bytes
            this.s_recv = new string[s_count];

            this.mac = new Cmac[s_count];
            for (int i = 0; i < s_count; i++)
            {
                this.mac[i] = new Cmac();
            }
            this.clear_mac();
              
            this.s_len = 0;
            this.i_srecv = 0;
            this.i_deal = 0;
           //if(this.factory_type==0) this.factory_type = (UInt16)FactoryType.CYPRESS;//for test 
        }

        public void clear_mac()
        {
            if (this.debug) Console.WriteLine("clear_mac()");
            this.in_clear = true;
            this.i_mac = 0;
            
            this.mac_count = 0;
            for (int i = 0; i < s_count; i++)
            {
                this.mac[i].linked = false;
            }
            this.in_clear = false;
        }

        public void clear_isset()
        {
            if (this.debug) Console.WriteLine("clear_isset()");
            this.isset_connect=false;
            this.isset_disconnect=false;
            this.isset_mac = false;
            this.isset_resistance = false;
            this.isset_voltage = false;
            this.isset_weigt = false;
            this.isset_rssi = false;
            this.isset_version = false;
            this.isset_stable = false;
            this.isset_name = false;
            this.isset_test_result = false;
            this.result = 0xFF;
            this.current_deal_cmd = Cmd.no_cmd;
            this.current_cmd = Cmd.no_cmd;
            //this.is_test = false;
            //this.clear_mac();
        }

        public bool is_seting(Cmd cmd)
        {
            if (cmd==Cmd.scan&&(this.isset_name && this.isset_mac && this.isset_rssi )) return true;
            if (cmd == Cmd.connect && ( this.isset_connect||this.isset_stable || this.isset_weigt || this.isset_version || this.isset_resistance || this.isset_voltage||this.isset_version)) return true;
            if (cmd == Cmd.disconnect && (this.isset_disconnect)) return true;
            return false;
        }

        public void set_send_sign()
        {
            for (int i=0; i <Cdata.sign_len;i++)
                this.s_buffer[i] = this.send_sign[i];
        }

        public  void set_recv_sign()
        {
            for (int i = 0; i < Cdata.sign_len; i++)
                this.s_buffer[i] = this.recv_sign[i];
        }


        public void set_func_send_sign()
        {
            this.set_send_sign();
            for (int i =0; i < Cdata.func_sign_len; i++) this.s_buffer[Cdata.func_sign_offset+i] = this.func_send_sign[i];
        }

        public void set_func_recv_sign()
        {
            this.set_recv_sign();
            for (int i = 0; i < Cdata.func_sign_len; i++)  this.s_buffer[Cdata.func_sign_offset+i] = this.func_recv_sign[i];
        }

        public bool set_func_cmd(FunctionCmd cmd,byte[] data=null, int data_len=0,int type=0) //type:0 send, 1 or other: recv type 
        {
            int i, len, chksum_offset;
            if (data_len > 0 && data == null) return false;

            if (type == 0) this.set_func_send_sign();
            else this.set_func_recv_sign();

            this.s_buffer[Cdata.func_cmd_offset]=(byte)cmd;
            this.s_buffer[Cdata.func_len_offset] = (byte)data_len;

            for (i=0;i< data_len;i++)
            {
                this.s_buffer[Cdata.func_data_offset+i] = (byte)data[i];
            }

            chksum_offset = func_checksum_base + data_len;
            for (this.s_buffer[chksum_offset]=0, i= Cdata.func_cmd_offset; i < chksum_offset;i++)
            {
                this.s_buffer[chksum_offset] += this.s_buffer[i];
            }

            this.s_buffer[4] = (byte)0x01;//feikeid =1
            this.s_buffer[5] = (byte)Cmd.package;//data package cmd

            len = (data_len + 8);//8=5*sign+1*cmd+1*len+1*checksum
            this.s_buffer[6] =(byte)(len);
            for (this.s_buffer[7] =0, i = 0; i < len; i++) //checksum
            {
                this.s_buffer[7] += this.s_buffer[8 + i];
            }
            this.s_len = len+8;//0-7header, 
            this.display(this.s_buffer, this.s_len);
            return true;
        }

        public void set_scan(byte type= (byte)DeviceType.fh7001)
        {
            if (this.debug) Console.WriteLine("set_scan()");
            this.set_send_sign();
            this.s_buffer[4] = (byte)0x0;//id =xx
            this.s_buffer[5] =(byte) Cmd.scan;//scan cmd
            this.s_buffer[6] = (byte)0x1;//data len
            this.s_buffer[7] = (byte)0x0;//xx
            this.s_buffer[8] = (byte)type;//xx
            this.s_buffer[7] +=type;
            this.s_len = 9;
            this.current_cmd = Cmd.scan;
        }

        public bool set_mac_sel(string s_mac, int len = Cdata.mac_len)
        {
            int n;
            for (n = 0; n < this.mac_count&& n < Cdata.s_count; n++)
            {
                if (String.Compare(s_mac, this.mac[n].s_mac) == 0)
                {
                    this.mac_sel = n;
                    return true;
                }
            }
            return false;
        }

        public bool set_mac_sel(byte[] mac, int len = Cdata.mac_len)
        {
            int n, i;
            for(n=0;n<this.mac_count && n < Cdata.s_count; n++)
            {
                for(i=0;i<Cdata.mac_len;i++)
                {
                    if (mac[i] != this.mac[n].mac[i]) break;
                }
                if (i < Cdata.mac_len) continue;
                this.mac_sel = n;
                return true;
            }
            return false;
        }
        public string s_verify_name = "FLYCO FH70";
        public bool set_mac_sel()
        {
            bool ret = false;
            int n, max_rssi=int.MinValue;
            for (n = 0; n < this.mac_count&&n<Cdata.s_count; n++)
            {
                if (this.debug) this.mac[n].display(true, n, "set_mac_sel()");
                if (this.mac[n].linked||this.mac[n].locked) continue; //skip linked locked
                if (this.mac[n].rssi < this.rssi_min_limit || this.mac[n].rssi > this.rssi_max_limit) continue;
                if(this.mac[n].name.Length< this.s_verify_name.Length||this.mac[n].name.IndexOf(this.s_verify_name)==-1) continue; //s_verify_name
                if (this.mac[n].rssi > max_rssi)
                {
                    this.mac[n].locked = true;
                    max_rssi = this.mac[n].rssi;
                    this.mac_sel = n;
                    ret = true;
                }
            }
            if(this.debug) Console.WriteLine("set_mac_sel()->this.mac_sel="+ this.mac_sel);
            this.current_cmd = Cmd.sel_mac;
            return ret;
        }

        public bool set_conn(byte[] mac,UInt16 factory_type,int len= Cdata.mac_len)//connect mac len =6
        {
            byte checksum = (byte)0;
            this.set_send_sign();
            this.s_buffer[4] = (byte)0x01;//feikeid =1
            this.s_buffer[5] = (byte)Cmd.connect;//connect cmd
            this.s_buffer[6] = (byte)len;//0x6;//mac len

            int i,index= 8 + Cdata.mac_len - 1;//revert
            for (i = 0; i < len; i++)
            {
                this.s_buffer[index - i] = mac[i];//revert
                checksum += mac[i];
            }

            this.s_buffer[8 + i] = (byte)((factory_type>>8) & 0xFF);
            this.s_buffer[9 + i] = (byte)(factory_type & 0xFF);

            checksum += this.s_buffer[8 + i];
            checksum += this.s_buffer[9 + i];

            this.s_buffer[7] = checksum;
            this.s_len = 8 + len+2;//2:factory type
            if (this.debug) this.display(this.s_buffer, this.s_len);
            this.current_cmd = Cmd.connect;
            return true;
        }

        public bool set_conn()//connect mac len =6
        {
            if (debug) this.mac[this.mac_sel].display(true,this.mac_sel, "set_conn()");
            this.set_conn(this.mac[this.mac_sel].mac, this.mac[this.mac_sel].factory_type);
            return true;
        }

        public void set_disconn()//disconnect
        {
            if (this.debug) Console.WriteLine("set_disconn()");
            this.set_send_sign();
            this.s_buffer[4] = (byte)0x01;//feikeid =1
            this.s_buffer[5] = (byte)Cmd.disconnect;//disconnect cmd
            this.s_buffer[6] = (byte)0;//0x6;//mac len
            this.s_buffer[7] = (byte)0;
            this.s_len = 8;
            this.current_cmd = Cmd.disconnect;
        }

        public void set_status()//status
        {
            this.set_send_sign();
            this.s_buffer[4] = (byte)0x01;//feikeid =1
            this.s_buffer[5] = (byte)Cmd.status;//status cmd
            this.s_buffer[6] = (byte)0;//0x6;//mac len
            this.s_buffer[7] = (byte)0;
            this.s_len = 8;
        }

        public void set_weight()
        {
            this.data[0]=(byte)0x5A;//app->mcu
            this.data[1]=(byte)0xD2; //weight cmd
              
            for(int i=2;i<14;i++)
            {
                this.data[i]=(byte)0;//
                this.data[14]^=this.data[i];
            }
            this.data[15]=(byte)0xAA; //end
            this.set_package(this.data, data_len);
        }

        public void set_history(byte index)
        {
            this.data[0] = (byte)0x5A;//app->mcu
            this.data[1] = (byte)0xD3;//history cmd
            this.data[2] = index;//number of history

            this.data[14] ^= this.data[2];
            for (int i = 3; i < 14; i++)
            {
                this.data[i] = (byte)0;//
                this.data[14] ^= this.data[i];
            }
            this.data[15] = (byte)0xAA; //end
            this.set_package(this.data, data_len);
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

        public void display_mac(string where="")
        {
            int i ;
            for(i=0;i<this.mac_count&&i<Cdata.s_count;i++)
            {
                this.mac[i].display(this.debug,i,where);
            }
        }

        public void set_get_rssi()
        {
            byte[] data = {0x46,0x4C,0x59,0x43,0x4F,0x01,0x00,0x01 };
            this.set_package(data, data.Length, 0,false);
        }

        public void set_stable()
        {
            byte[] data = { 0x5A, 0xD2, 0x0, 0x0 ,0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xAA, };
            this.set_package(data,16,0);
        }

        public void set_result()
        {
            this.set_result(this.result);
        }
        //0x5A	0xf1	0x00	0x00	0x00	0x00	0x00/0x01	0x00	0x00	0x00	0x00	0x00	0x00	0x00	0x00-0xFF	0xAA
        public void set_result(byte result)
        {
            byte[] data = {0x5A,0xF1,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xAA};
            data[6] = result;
            this.set_package(data, data.Length, 0);
        }

        public void set_package(byte[] data,int len=data_len,int type=0,bool is_xor=true) //feike data package is 0x 10
        {
            byte checksum = (byte)0;
            byte xor_byte = (byte)0;

            if(type==0) this.set_send_sign();//type 0 send
            else this.set_recv_sign();//type else recv

            this.s_buffer[4] = (byte)0x01;//feikeid =1
            this.s_buffer[5] = (byte)Cmd.package;//data package cmd
            this.s_buffer[6] = (byte)len;//0x6;//mac len

            for (int i = 0; i < len; i++)
            {
                this.s_buffer[8 + i] = data[i];
                checksum += data[i];
                if(i!=0&&i!=14&&i< len - 1) xor_byte ^= data[i];
            }
            if(is_xor) this.s_buffer[8 + len-2] = xor_byte;//set xor byte
            
            this.s_buffer[7] = checksum;
            this.s_len = 8 + len;
            display(this.s_buffer, 0, this.s_len);
        }

        public void set_cmd(byte cmd)
        {
            this.s_buffer[5]=cmd;
        }

        public byte get_id()
        {
            return this.s_buffer[4];
        }

        public byte get_cmd()
        {
            return this.s_buffer[5];
        }

        public byte get_length()
        {
            return this.s_buffer[6];
        }

        public byte get_length(byte cmd,int type=0)
        {

            if(cmd==(byte)Cmd.scan)
            {
                if (type == 0) return 0;//send length
                else return 8;//recv data length //2byte rssi = 6bytes mac 
            }
            return 0xFF;
        }

        public byte get_length(byte[] buf, int start, int end,int type=0)
        {
            if (start + 6 > end) return (byte)0xFF;
/*
            if (buf[start + 6] == 1)//this is patch
            {
                byte len = this.get_length(this.what_recv(buf, start, end), type);
                if (len != 0xFF) return len;
            }
*/
            return buf[start+6];
        }

        public byte get_checksum()
        {
            return this.s_buffer[7];
        }

        public int get_recv(byte[] buf,int len)
        {
            return 0;
        }
        //name
        public bool is_print(byte b)
        {
            if (b >= 0x20 && b <= 0x7E) return true; //0x20 - 0x7e
            return false;
        }

        public int get_ad_x(byte[] buf, int start, int len,byte adn)//adn:ad number: MAC:0x1B ,name:0x09, return :index ad-len of adn
        {
            if (Cdata.ble_ad_offset >= len) return -1;

            int i;
            for(i=Cdata.ble_ad_offset; i+1<len;)
            {
                if (buf[start+i + 1] == adn) return start+i;
                i += buf[start+i] +1;//point next ad-len
            }

            return 0;
        }

       //78736373-01012F00-00D7-6D:12:99:50:A0:00:-02.01.06.12.09-46.4C.59.43.4F.20.46.48.37.30.30.31.2F.32.2F.33.20-081B006D129950A000 
       // header-----cmd---------rssi---mac--------------------------------------[F--L--Y--C-O-    -F--H--7--0---0--1---/--2--/---3--   -]
       public int set_ble_chip(int len)
        {
            if (this.auto_ble_chip == false)
            {
                this.mac[i_mac].factory_type = this.factory_type;
            } 
            else
            {
                if (len == 0x11) this.mac[i_mac].factory_type = (UInt16)FactoryType.AMICCOM;
                if (len == 0x13) this.mac[i_mac].factory_type = (UInt16)FactoryType.TELINK;
                if (len == 0x12) this.mac[i_mac].factory_type = (UInt16)FactoryType.CYPRESS;
                this.factory_type = this.mac[i_mac].factory_type;
            }

            if (this.factory_type == (UInt16)FactoryType.AMICCOM) this.s_chip= "AMICCOM";
            if (this.factory_type == (UInt16)FactoryType.TELINK) this.s_chip= "TELINK";
            if (this.factory_type == (UInt16)FactoryType.CYPRESS) this.s_chip= "CYPRESS";

            return 0;
        }
        public string s_7001_name = "FH7001";
        public string s_7005_name = "FH7005";
        public int get_name(byte[] buf, int start, int len)
        {
            //this.isset_name = false;

            int i = this.get_ad_x(buf, start, len, 0x09);//name ad=09
            if (i <= 0) return -1;
            if (i + buf[i] > start+len) return -1;

            this.s_name = "";
            len = buf[i];
            this.set_ble_chip(len);//set ble chip
            int n;
            for (n=0, i+=2;n<len - 1;n++)
            {
                this.s_name += (char)buf[i+n];
            }
            this.mac[i_mac].name = this.s_name;

            this.mac[i_mac].type = DeviceType.no_type;
            if (this.s_name.IndexOf(this.s_7001_name) > 0) this.mac[i_mac].type = DeviceType.fh7001;
            if (this.s_name.IndexOf(this.s_7005_name) > 0) this.mac[i_mac].type = DeviceType.fh7005;

            if (this.mac[i_mac].name.Length >0) this.isset_name = true;
            if (this.debug) Console.WriteLine("get_name():" + this.s_name);
            return 0;
        }
        //version
        public int get_version(byte[] buf, int start, int len)
        {
            if (Cdata.func_version_offset >= len) return -1;
            int i,n= buf[start + Cdata.func_len_offset];
            for (this.s_version = "",i=0;i<n;i++)
            {
                this.s_version += buf[start + Cdata.func_version_offset + i].ToString();
                if (i + 1 < n) this.s_version += ".";
            }
            if (debug) Console.WriteLine("get_version():" + this.s_version);

            if (this.s_version.Length > 0)
            {
                this.mac[i_mac].version = this.s_version;
                this.isset_version = true;
            }
            return 0;
        }

        public int get_func_rssi(byte[] buf, int start, int len)
        {
            this.heartbeat = 0;
            if (debug) Console.WriteLine("get_func_rssi():this.heartbeat=" + this.heartbeat);
            return 0;
        }

        //voltage
        public int get_voltage(byte[] buf,int start,int len)
        {
            if(this.mac[this.mac_sel].type==DeviceType.fh7005)
            {
                if (Cdata.voltage_offset_7005 >= len) return -1;
                this.voltage = buf[start + Cdata.voltage_offset_7005];
                this.f_voltage = (this.voltage & 0x3F);
                this.f_voltage/=10; //bit 0-5 is voltage 
                this.isset_voltage = true;
            }

            if (this.mac[this.mac_sel].type == DeviceType.fh7001)
            {
                if (Cdata.voltage_offset_7001 >= len) return -1;
                this.voltage = buf[start + Cdata.voltage_offset_7001];
                this.f_voltage = this.voltage;
                    this.f_voltage/= 10; //bit 0-5 is voltage 
                this.isset_voltage = true;
            }

            return 0;
        }

        public int get_resistance(byte[] buf, int start, int len,int version=20)//20=2.0 18 =1.8
        {
            if (Cdata.resistance_offset + 2 >=len) return -1;
            // this.isset_resistance=false;

           if (version==18)
            {
                this.resistance = buf[start + Cdata.resistance_offset];
                this.resistance <<= 16;
                this.resistance |= ((buf[start + Cdata.resistance_offset + 1] & 0xFF) << 8);
                this.resistance |= (buf[start + Cdata.resistance_offset + 2] & 0xFF);
            }
            if (version == 20)
            {
                this.resistance = 0;
                this.resistance |= ((buf[start + Cdata.resistance_offset] & 0xFF) << 8);
                this.resistance |= (buf[start + Cdata.resistance_offset + 1] & 0xFF);
            }
            this.isset_resistance = true;
            return 0;
        }
        //head[0-7]:78,73,63,73,01,04,10,0E,   weight[7]:A5,D1,56,1F,6C,9A,00,[7]:80,3E,00,00,00,5B,00,8B,AA
        public int get_weight(byte[] buf, int start, int len)
        {
            if (Cdata.weigt_offset + 1 >=len) return -1;
           // this.isset_weigt=false;
            this.weight_type = (byte)(0xC0);
            this.weight_type &= buf[start+Cdata.weigt_offset];
            this.weight_type >>= 6;

            this.weight = buf[start+Cdata.weigt_offset]&0x3F;//high byte
            this.weight <<= 8;
            this.weight |= buf[start+Cdata.weigt_offset + 1];//low byte
            this.f_weight = (float)this.weight / 10;

            this.kilo_weight = this.f_weight;
            if (this.convert_kilo)
            {
                if (this.weight_type == (int)WeightType.catty) this.kilo_weight = this.f_weight / 2;
                if (this.weight_type == (int)WeightType.pound) this.kilo_weight = (this.f_weight * (float)453.59237) / 1000;
                this.f_weight = this.kilo_weight;
            }
            this.isset_weigt = true;
            return 0;
        }
        //head[0-7]:78,73,63,73,01,01,01,00; rssi[8-9]:00,C1; mac[10-15]:6D,12,99,50,A0,00
        public int get_rssi(byte[] buf, int start, int len)
        {
            this.rssi = buf[start + Cdata.rssi_offset + 1];//byte=9 rssi
            this.s_rssi = ""  + (- (0x7F-(this.rssi&0x7F)));
            if (debug) Console.WriteLine("get_rssi():"+this.s_rssi);
            this.mac[i_mac].rssi = -(0x7F-(this.rssi & 0x7F));
            this.rc_rssi = this.mac[i_mac].rssi;//record current rssi 
            this.isset_rssi = true;
            return 0;
        }

        public bool set_i_mac()
        {
            if (this.mac[this.i_mac].locked == false) return true;
            do
            {
                if (this.i_mac + 1 < Cdata.s_count) this.i_mac++;//point next
                else this.i_mac = 0;//form 0
                if (this.mac[this.i_mac].locked == false) break;
            } while (true);
            return true;
        }

        public int get_mac(byte[] buf, int start,int len)
        {
            if(Cdata.mac_offset+Cdata.mac_len>len) return -1;
            int i;
            while (this.in_clear) ;//wait in_clear
            this.set_i_mac();
            this.mac[this.i_mac].s_mac = "";//clear string

            int index = start + Cdata.mac_offset + Cdata.mac_len - 1;//revert
            for (i=0; i < Cdata.mac_len; i++)
            {
                this.mac[this.i_mac].s_mac += buf[index - i].ToString("X2");
            }
            this.s_c_mac = this.mac[this.i_mac].s_mac;//record current mac
            if (this.debug) Console.WriteLine("mac="+this.mac[this.i_mac].s_mac);

            this.mac[this.i_mac].ditto = false;
            if ( String.Compare(this.mac[this.i_mac].s_mac, this.s_mac_sel) == 0) //check ditto
            {
                this.mac[this.i_mac].ditto = true;
            }
            //store mac []
            for (i = 0; i < Cdata.mac_len && (Cdata.mac_offset) + i < len && i < Cdata.mac_len; i++)
            {
                //if (this.debug) Console.WriteLine("get_mac()[" +i+"]:"+ buf[start + Cdata.mac_offset + i]);
                this.mac[this.i_mac].mac[i] = buf[index - i];//revert
            }
            if (this.debug) this.display(this.mac[this.i_mac].mac, 0, Cdata.mac_len, "get_mac()");
            this.mac[this.i_mac].set_u64_mac();//set u64_mac value
            this.mac[this.i_mac].mac_to_s();
            this.mac[this.i_mac].linked = false;

            if (this.debug) Console.WriteLine("get_mac()i_mac:" + this.mac[this.i_mac].mac_s);

            if (this.i_mac + 1 < Cdata.s_count) this.i_mac++;//point next
            else this.i_mac = 0;//form 0

            this.mac_count++;
            this.isset_mac = true;
            return 0;
        }

        public int get_conn(byte[] buf, int start, int len)
        {
           try { 
                this.s_conn = "";
                for (int i = 0; i < buf[start + Cdata.data_len_offset]; i++)
                {
                    this.s_conn += buf[start + Cdata.conn_offset + i].ToString("X2");
                }
                if (this.debug) Console.WriteLine("get_conn():" + this.s_conn);
           }
           catch (Exception e)
           {
                if (this.debug) Console.WriteLine("get_conn():"+ e.ToString());
           }
           return 0;
        }

        public int deal_func_cmd(byte[] buf, int start, int len)
        {
            byte cmd=this.what_func_cmd(buf, start, len);
            if (cmd == 0) return -1;//error

            if(cmd==(byte)FunctionCmd.QueryFwVersion)
            {
                this.get_version(buf, start, len);
            }

            if (cmd == (byte)FunctionCmd.QueryRssi)
            {
                this.get_func_rssi(buf, start, len);
            }
            return 0;
        }

        public int deal_cmd(byte[] buf ,int start, int len)
        {
            byte p_cmd; //PackageCmd 
            byte cmd = this.what_recv(buf, start, start+len);

            if (cmd == (byte)Cmd.no_cmd)
            {
                this.current_deal_cmd = Cmd.no_cmd;
                return -1;
            }
            if(cmd==(byte)Cmd.scan)//get mac and rssi
            {
                if (this.is_wait_disconn)
                {
                    this.clear_isset();
                    this.clear_mac();
                    this.isset_disconnect = true;//send disconn
                    this.is_wait_disconn = false;
                    this.is_test = false;
                }

                // if (Cdata.mac_offset + Cdata.mac_len > len) return -1;

                this.get_rssi(buf, start, len);
                this.get_name(buf, start, len);
                this.get_mac(buf, start, len);

                if(this.debug) this.display_mac("deal_cmd().Cmd.scan");//test
                // System.Threading.Thread.Sleep(1); //sleep test 
                this.current_deal_cmd = Cmd.scan;
            }

            if(cmd==(byte)Cmd.connect)
            {
                this.get_conn(buf, start, len);
                this.isset_connect=true;
                this.current_deal_cmd = Cmd.connect;
            }

            if(cmd==(byte)Cmd.disconnect)
            {
                this.clear_isset();
                this.clear_mac();
                this.isset_disconnect = true;
                this.is_test = false;
                //this.set_disconn();
                //System.Threading.Thread.Sleep(1); //sleep for to test thread
                this.current_deal_cmd = Cmd.disconnect;
            }

            if (cmd == (byte)Cmd.package)
            {
                p_cmd = this.what_cmd(buf,start, start + len);
                if(p_cmd==(byte)PackageCmd.Function)
                {
                    this.deal_func_cmd(buf, start, len);
                }

                if (this.isset_version == false&&(p_cmd == (byte)PackageCmd.Stable || p_cmd == (byte)PackageCmd.Weight))
                {
                    this.set_func_cmd(FunctionCmd.QueryFwVersion);
                    System.Threading.Thread.Sleep(0);  //wait get version,do not delete the sleep
                }

                if(p_cmd==(byte)PackageCmd.Weight)
                {
                    this.get_weight(buf, start, len);
                }

                if(p_cmd==(byte)PackageCmd.Stable)
                {
                    if (this.debug) Console.WriteLine("if(p_cmd==(byte)PackageCmd.Stable)");
                    this.get_weight(buf, start, len);
                    this.get_resistance(buf, start, len);
                    this.get_voltage(buf, start, len);

                    if (this.isset_version)
                    {
                        this.set_stable();
                        System.Threading.Thread.Sleep(0);  //wait 
                        this.isset_stable = true;
                    }
                    this.current_deal_cmd = Cmd.package;
                }

                if (p_cmd == (byte)PackageCmd.SetTestResult)
                {
                    this.isset_test_result = true;
                }
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
            if(this.r_buffer[0]==(byte)0x78&&this.r_buffer[1]==(byte)0x73&&this.r_buffer[2]==(byte)0x63&&this.r_buffer[3]==(byte)0x73)
            {
                return this.r_buffer[5];
            }
            return (byte)Cmd.no_cmd;
        }

        public byte what_recv(byte[] data, int start, int end) //return recv cmd 1-5
        {
            if (start + 5 >= end) return (byte)Cmd.no_cmd;
            if (data[start + 0] == (byte)0x78 && data[start + 1] == (byte)0x73 && data[start + 2] == (byte)0x63 && data[start + 3] == (byte)0x73)
            {
                return data[start+5];
            }
            return (byte)Cmd.no_cmd;
        }

        public byte what_cmd(byte[] data, int start, int end)  //package sub command
        {
            byte cmd = this.what_recv(data, start, end);
            if(cmd != (byte)Cmd.package) return (byte)0;

            // check function cmd
            int i;
            for(i=0;i<Cdata.func_sign_len;i++)
            {
                if (data[start + Cdata.func_sign_offset + i] == this.func_recv_sign[i]) continue;
                break;
            }

            if(i>=Cdata.func_sign_len)
            {
                return (byte)PackageCmd.Function;//return function cmd
            }
            //check package other cmd 
            if ( data[start+8] == (byte)0xA5 && data[start + 23] == (byte)0xAA)
            {
                return data[start + 9];
            }
            return (byte)0;
        }

        public byte what_cmd() //package sub command
        {
            return this.what_cmd(this.r_buffer,0,24);
        }

        public byte what_func_cmd(byte[] buf, int start, int len)
        {
            if (Cdata.func_cmd_offset >= len) return (byte)0;
            return buf[start + Cdata.func_cmd_offset];
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

    public class Cfeike
    {

    }

    public class CfeikeDB : Cmysql
    {
        public string export_file;
        public string time_format;

        public CfeikeDB()
        {
            this.database = "bluetooth";
            this.build_conn();
            this.secure_file_priv = "C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/";
            this.export_file = "";
            this.time_format = "yyyy_MM_dd_HH_mm_ss_ffff";
        }

        public override int deal_rdr()
        {
            this.rdr_count = 0;
            while (this.rdr.Read())
            {
                this.rdr_count++;
                try
                {
                    if (this.debug) Console.WriteLine(this.rdr[0] + " -- " + this.rdr[1] + "--" + this.rdr[2]);
                }
                catch (Exception e) { if (this.debug) Console.WriteLine(e.ToString()); break; }

            }
            this.close();
            return this.rdr_count;
        }

        public int is_exist_mac(UInt64 mac)//-1 do nothing ,0 :not exist ;>0 mac count 
        {
            this.sql = "SELECT mac from `bluetooth`.`mac` where mac = '" + mac + "';";
            if (this.command(this.conn_str, sql, ExeType.Select, false))
            {
                return this.deal_rdr();
            }
            else return -1; //do nothing
        }

        public int is_empty(string db_tab)//db_tab="bluetooth.mac"
        {
            this.sql = "SELECT * FROM " + db_tab +";";
            if (this.debug) Console.WriteLine(this.sql);
            if (this.transaction(this.conn_str, sql, ExeType.Select, true)) return this.rdr_count;
            return -1;
        }

        public bool get_last_record(string db_tab,string id)//db_tab="bluetooth.mac" ,id "idmac"
        {
            this.sql = "SELECT "+ id+" FROM " +db_tab+" ORDER BY " +id+ " DESC LIMIT 0,1;";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Select,false);//not deal
        }

        public bool get_first_record(string db_tab, string id)//db_tab="bluetooth.mac" ,id "idmac"
        {
            this.sql = "SELECT "+ id +" FROM " + db_tab + " ORDER BY " + id + " ASC LIMIT 0,1;";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Select, false);//not deal
        }

        public bool alter_auto_inc(string db_tab,int inc_value)
        {
            this.sql = "ALTER TABLE "+db_tab+" AUTO_INCREMENT = " +inc_value+" ;";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, this.sql, ExeType.Script, true);
        }

        public bool delete_worker(string idworker)
        {
            this.sql = " DELETE FROM `bluetooth`.`worker` WHERE `idworker`='"+ idworker + "';";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        public bool insert_worker(string name, string status, int idstatus)
        {
            this.sql = " INSERT INTO `bluetooth`.`worker` (`name`, `status`, `idstatus`) VALUES('" + name + "', '" + status + "', '" + idstatus + "');";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }
        

        public bool insert_order(string idstatus, string devicetype, string order_number, string product_name, string amount, string deliverytime, string starttime,string who,string unitprice, string total,string status)
        {
            this.sql = " INSERT INTO `bluetooth`.`order` ( `idstatus`, `devicetype`, `number`, `name`, `quantity`, `deliverytime`, `starttime`, `who`, `unitprice`, `total`, `status`) VALUES('"+ idstatus+"', '"+devicetype+"', '"+order_number+"', '"+product_name+"', '"+ amount+"', '"+ deliverytime+"', '"+ starttime+"', '"+who+"', '"+ unitprice+"', '"+total+"',  '"+status+"');";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        public bool insert_order(int idstatus, int devicetype, string order_number, string product_name, int amount, string deliverytime, string starttime, string who, double unitprice, double total, string status)
        {
            return this.insert_order(idstatus.ToString(), devicetype.ToString(), order_number, product_name, amount.ToString(), deliverytime, starttime, who, unitprice.ToString(), total.ToString(), status);
        }

        public bool delete_line(string number)
        {
            this.sql = " DELETE FROM `bluetooth`.`line` WHERE `number`='" + number + "';";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        public bool insert_line(string number, string name,string who, string what,string when,string start,string end,string where,string capacity,string status, string contact,string level)
        {
            string guid = Guid.NewGuid().ToString();

            this.sql = "INSERT INTO `bluetooth`.`line` (`guid`, `number`, `name`, `who`, `what`, `time`, `start`, `end`, `where`, `capacity`, `status`, `contact`, `level`) VALUES('"+ guid+"', '"+ number+"', '"+ name+"', '"+who+"', '"+what+"', '"+ when + "', '"+start+"', '"+end+"', '"+where+"', '"+capacity+"', '"+status+"', '"+contact+"', '"+ level + "');";

            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        public bool insert_test(string where,string who,string what,string how)
        {
            string guid = Guid.NewGuid().ToString();
            this.sql = "INSERT INTO `bluetooth`.`test` (`guid`,`where`, `who`, `what`, `how`) VALUES('" + guid +"','"+ where+"','"+ who+"','"+ what+"','"+ how+"');";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        public bool insert_fail(string order, string type, UInt64 mac, string s_mac, string chip, string r_mac, string weight, string resistance, string voltage, string rssi, string version, string name, string line, string who_worker, string result)
        {
            this.sql = "INSERT INTO `bluetooth`.`fail` (`order_from`, `type`, `mac`, `s_mac`, `chip`,`r_mac`,`weight`, `resistance`, `voltage`, `rssi`, `version`, `name`, `line`, `who`,  `result`)VALUES ( '" + order + "', '" + type + "', '" + mac + "', '" + s_mac + "', '" + chip + "', '" + r_mac + "', '" + weight + "', '" + resistance + "', '" + voltage + "', '" + rssi + "', '" + version + "', '" + name + "', '" + line + "', '" + who_worker + "', '" + result + "');";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        public bool insert_product(string order, string type, UInt64 mac, string s_mac, string chip, string r_mac, string weight,string resistance,string voltage,string rssi,string version,string name,string line,string who_worker,string result)
        {
            this.sql = "INSERT INTO `bluetooth`.`product` (`order_from`, `type`, `mac`, `s_mac`, `chip`, `r_mac`, `weight`, `resistance`, `voltage`, `rssi`, `version`, `name`, `line`, `who`,  `result`)VALUES ( '" + order+"', '"+type+"', '"+mac+"', '"+s_mac+"', '" + chip + "', '" + r_mac + "', '" + weight+"', '"+resistance+"', '"+voltage+"', '"+rssi+"', '"+version+"', '"+name+"', '"+line+"', '"+who_worker+"', '"+result+"');";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        //INSERT INTO  `bluetooth`.`mac` (`mac`) VALUES (5),(6),(7),(8);
        public bool insert_mac(DataTable d, UInt64 min, UInt64 max, int fromBase=16) //mac (min,max)
        {
            try
            {
                if (this.debug) Console.WriteLine("insert_mac()");
                if (d == null) return false;
                if (this.debug) Console.WriteLine("Columns" + d.Columns.Count);
                if (this.debug) Console.WriteLine("Rows:" + d.Rows.Count);

                UInt64 mac;
                int count = 0, insert_count = 0;
                string sql = "INSERT INTO  `bluetooth`.`mac` (`mac`) VALUES ";
                string s;
                foreach (DataRow dr in d.Rows)
                {
                    count++;
                    if (dr[0].ToString().Length < 1) continue; //check length
                    s = dr[0].ToString();
                    s=s.Replace(":", "");
                    mac = Convert.ToUInt64(s, fromBase);
                    if ( mac >= min&& mac <= max&&this.is_exist(" `bluetooth`.`mac` ","mac", mac.ToString()) >0) continue;
                    insert_count++;
                    sql += "(" + mac + ")";
                    if (count < d.Rows.Count) sql += ",";
                }
                sql += ";";
                if (this.debug) Console.WriteLine("insert_count="+ insert_count+","+sql);
                if (insert_count < 1) return true;
                return this.transaction(this.conn_str, sql, ExeType.Insert);
            }
            catch (Exception e)
            {
                if (this.debug) Console.WriteLine(e.ToString());
                return false;
            }
        }

        public int insert_mac(DataTable d)
        {
            if (this.debug) Console.WriteLine("insert_mac()");
            if (d == null) return -1;
            if (this.debug) Console.WriteLine("Columns"+d.Columns.Count);
            if (this.debug) Console.WriteLine("Rows:"+d.Rows.Count);

            int false_count = 0;
            foreach (DataRow dr in d.Rows)
            {
                if (dr[0].ToString().Length < 1) continue; //check length
                if (this.insert_mac(UInt64.Parse(dr[0].ToString()))) continue;
               false_count++;//add false
              if (this.debug) Console.WriteLine("error insert_mac:"+dr[0]);//show fail
            }

            return false_count;
        }

        public bool insert_mac(UInt64 mac)
        {
            this.sql = "INSERT INTO `bluetooth`.`mac` (`mac`) VALUES( '" + mac + "');";
            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        public bool export_mac(string outfile,bool add_secure_file_priv=true)
        {
            if (add_secure_file_priv) this.export_file = this.secure_file_priv + outfile;
            else this.export_file = outfile;

            this.sql = "SELECT* FROM bluetooth.mac into outfile '"+ this.export_file + "';";
            return this.transaction(this.conn_str, sql, ExeType.Select, true);
        }

        public bool export_mac()
        {
            string outfile="MAC_" + DateTime.Now.ToString(this.time_format) + ".csv";
            return this.export_mac(outfile, true);
        }

        public bool load_mac(string infile)
        {
            string file =infile.Replace("\\","/");
            this.sql= "LOAD DATA INFILE '"+file+"' INTO TABLE bluetooth.mac;";
            return this.transaction(this.conn_str, sql, ExeType.Script, true);
        }

        public bool insert_customer(string who,string type,string country,string planet,string area,string where,string latitudeLongitude,string contact,string mail,string url,string start,string end,string what,string introduction,int level,string group,int credit,string remark,string status,string pionerr,string file,string logo,string image,string audio, string vedio)
        {
            this.sql = "INSERT INTO `bluetooth`.`customer` ( `who`, `type`, `country`, `planet`, `area`, `where`, `l_l`, `contact`, `mail`, `url`, `start`, `end`, `what`, `introduction`, `level`, `group`, `credit`, `remark`, `status`, `pioneer`, `file`, `logo`, `image`, `audio`, `vedio`) VALUES( '"+who+"', '"+type+"', '"+country+"', '"+planet+"', '"+area+"', '"+where+"', '"+latitudeLongitude+"', '"+contact+"', '"+mail+"', '"+url+"', '"+ start+"', '"+ end+"', '"+what+"', '"+introduction+"', '"+ level+"', '"+group+"', '"+ credit +" ', '"+remark+"', '"+status+"', '"+pionerr+"', '"+file+"', '"+logo+"', '"+image+"', '"+audio+"', '"+vedio+"');";

            if (this.debug) Console.WriteLine(this.sql);
            return this.transaction(this.conn_str, sql, ExeType.Insert, true);
        }

        public bool set_test_item(string item, int idtestitem)
        {
            this.sql="UPDATE `bluetooth`.`testitem` SET `item`= '"+item+"' WHERE `idtestitem`= '"+ idtestitem + "';";
            return this.transaction(this.conn_str, this.sql, ExeType.Script, true);
        }


    }//  class CfeikeDB

   public class Ctest: CfeikeDB
    {
        public ResultType result;
        public int pass_count = 0;
        public int fail_count = 0;
        public bool exist_fail = true;
        //record test 
        public string r_mac;
        public string r_rssi;
        public string r_name;
        public string r_version;
        public string r_weight;
        public string r_voltage;
        public string r_resistance;

        public Ctest()
        {
            this.result = ResultType.NotTest;
        }

        public ResultType range_test(Ctestitem it)
        {
            if (!it.convert())
            {
                if (it.select) return ResultType.Fail;
                return ResultType.NotTest;
            }
            if (it.d_value >= it.d_min && it.d_value <= it.d_max) return ResultType.Pass;
            else return ResultType.Fail;
        }

        public void init_database_server(string server_name)
        {
            if (server_name.Length < 1) return;
            this.server = server_name;
            this.build_conn(); //set connect
        }

        public ResultType must_test(Ctestitem it)
        {
            if (it.must.Length < 1)
            {
                if (it.select) return ResultType.Fail;
                return ResultType.NotTest;
            }
            
            if (String.Compare(it.must,it.value)==0) return ResultType.Pass;
            else return ResultType.Fail;
        }

        public ResultType exec_test(Ctestitem it)
        {

            return ResultType.Pass;
        }

        public ResultType chk_all_db(Ctestitem it)
        {

            return ResultType.Pass;
        }

        public ResultType chk_mac_range(Ctestitem it)
        {
            if(it.min.Length<1||it.max.Length<1) return ResultType.NotTest;
            UInt64 min, max, value;
            try
            {
                min = Convert.ToUInt64(it.min, 16);
                max = Convert.ToUInt64(it.max, 16);
                value = Convert.ToUInt64(it.value, 16);
                if (value >= min && value <= max) return ResultType.Pass;
                else return ResultType.Fail;
            }
            catch(Exception ex)
            {
                if (this.debug) Console.WriteLine(ex.ToString());
                if (it.select) return ResultType.Fail;
                return ResultType.NotTest;
            }
        }

        public ResultType chk_mac(Ctestitem it)
        {
            ResultType rt=ResultType.NotTest;
            do//run once
            {
                if (!it.select) {rt= ResultType.NotTest; break;}
                rt = this.chk_mac_range(it);
                if (rt == ResultType.Fail) break;//Fail exit test 
                int count = this.is_exist_mac(it.u64_value);
                if (count <0)// -1 do nothing
                {
                     rt = ResultType.Fail;  break;
                }

                if(count >0)//exist mac 
                {
                    if (this.exist_fail) {rt = ResultType.Fail; break;}
                    rt= ResultType.Pass; break;
                }
                
                if (count == 0)///not exist 
                {
                    if (this.exist_fail) {rt = ResultType.Pass; break;}
                    rt= ResultType.Fail; break;
                } 

            } while (false);
            this.r_mac = it.record(rt);
            return rt;
        }

        public ResultType chk_rssi(Ctestitem it)
        {
            ResultType rt = this.range_test(it);
            this.r_rssi = it.record(rt);
            return rt;
        }

        public ResultType chk_name(Ctestitem it)
        {
            ResultType rt = this.must_test(it);
            this.r_name = it.record(rt);
            return rt;
        }
        
        public ResultType chk_version(Ctestitem it)
        {
            ResultType rt = this.must_test(it);
            this.r_version= it.record(rt);
            return rt;
        }

        public ResultType chk_weight(Ctestitem it)
        {
            ResultType rt = this.range_test(it);
            this.r_weight = it.record(rt);
            return rt;
        }

        public ResultType chk_voltage(Ctestitem it)
        {
            ResultType rt = this.range_test(it);
            this.r_voltage = it.record(rt);
            return rt;
        }

        public ResultType chk_resistance(Ctestitem it)
        {
            ResultType rt = this.range_test(it);
            this.r_resistance= it.record(rt);
            return rt;
        }

    }

}
