

using System;
using System.IO.Ports;
using System.Threading;
using n_serial_data;

namespace n_serial
{
    public  enum SerialStatus
    {
        None,
        ReadStart,
        WriteStart,
        ReadEnd,
        WriteEnd,
        InWrite,
        WriteDone,
        InOpen,
        InClose,
        InWork,
    }

    public class Cserial
    {
        public  static bool _continue=false;
        public  SerialPort _serialPort = new SerialPort();

        public  Thread readThread ;
        public  Thread writeThread;
        public SynchronizationContext m_SyncContext = null;
        public SendOrPostCallback postCallback = null;

        public const int buffer_len = 0x100000;
        public const int str_count = 0x10;
        public  int port_count =0;
        public  byte[] read_buffer = new byte[buffer_len];
        public  byte[] write_buffer = new byte[buffer_len];
        public  string s_write_buf = "";
        public  string[] s_port = new string[str_count];
        public  int read_byte_count = 0;
        public  int write_byte_count = 0;
        public  ulong total_read_count = 0;
        public  ulong total_write_count = 0;
        public  string read_message;
        public  string write_message;

        public  bool debug=true;
        public  bool strart_write= false;
        public  int data_type=1;//0:string 1:Hexadecimal
        public  int index=0;//read index form read_buffer
        public  int deal_index=0;

        public  string com_s = "COM1";
        public  int baudrate = 115200;

        public SerialStatus read_status = SerialStatus.None;//0-end or not start .1:start
        public SerialStatus write_status = SerialStatus.None;//0-end or not start .1:start

        public Cdata cdata = new Cdata();
        public int read_sleep = 1; 
        public bool imitate_read =false;//true;

        public Cserial()
        {
            //this.imitate_read = true;//only for imitate test .
        }

        public  int  runme(string com_s, int baudrate)
        {
            if(this.debug)  Console.WriteLine("Cserial:runme()\n");

            try
            {
                setting(com_s, baudrate);
                _serialPort.Open();
              //  this.is_close = false;
            }
            catch(Exception e)
            {
                e.Data.ToString();
                return -1;
            }

            start();

            try
            {
                if(this.imitate_read)
                {
                    readThread = new Thread(this.ImitateRead);//test 
                }
                else
                {
                    readThread = new Thread(this.Read);
                }

               // readThread.IsBackground = true;
                writeThread = new Thread(this.Write);//
               // writeThread.IsBackground = true;
                readThread.Start();
                writeThread.Start();
            }
             catch (ThreadStateException e) {
                e.Data.ToString();
            }

            return 0;
        }

        public  int runme()
        {
            return this.runme(this.com_s, this.baudrate);
        }

        ~Cserial()
        {
            if (this.debug) Console.WriteLine("~Cserial()\n");
            this._serialPort.Close();
        }
/*
        bool is_close = false;
        public void close()
        {
            if (this.is_close) return;
            this.stop();
            this._serialPort.Close();
            this.is_close = true;
        }
*/
        public  int setting(string com_s="COM1",int baudrate= 38400) //should     public new int setting() 
        {
            if (debug) Console.WriteLine("Cserial:setting()\n");
            _serialPort.PortName = com_s;
            _serialPort.BaudRate = baudrate;
            _serialPort.Parity = System.IO.Ports.Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = System.IO.Ports.StopBits.One;
            _serialPort.Handshake = System.IO.Ports.Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            return 0;
        }

        public  int setting()
        {
            return this.setting(this.com_s, this.baudrate);
        }

        public  void scan()
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                if(debug) Console.WriteLine("   {0}", s);

                if (this.port_count < Cserial.str_count)
                {
                    this.s_port[this.port_count] = s;
                    this.port_count++;
                }
            }
        }

        public  void start()
        {
            Cserial._continue = true;
        }

        public  void stop(bool wait = true, int wait_count = 50)
        {
            this.strart_write = false;
            Cserial._continue = false;
            System.Threading.Thread.Sleep(1);
            if (this.read_status == SerialStatus.None) return;
            for (int n=0; wait&&n < wait_count && (this.read_status != SerialStatus.ReadEnd || this.write_status != SerialStatus.WriteEnd);n++)
            {
                System.Threading.Thread.Sleep(1);
            }
        }

        void display()
        {
            if(!this.debug) return;
            string s="";
           // Console.WriteLine("index="+this.index+"  deal="+this.deal_index);//test
            for (int i=this.deal_index; i <this.index; i++)
            {
                s += this.read_buffer[i].ToString("X2");
            }
            Console.WriteLine(s);
        }

        public bool adjust_buffer()
        {
                int i;
                if (this.index+Cdata.max_len>(buffer_len/2))//copy data to buf[0]->buf[i] and change index
                {
                        for (i=0;this.deal_index+i<this.index;i++) //Loop array for this.read_buffer
                        {
                            this.read_buffer[i] = this.read_buffer[this.deal_index + i];
                        }
                       this.deal_index = 0;
                       this.index = i;
                       return true;
                 }
                 return false;
        }

        public void reset_buffer()
        {
            this.index = 0;
            this.deal_index = 0;
            this.cdata.i_srecv = 0;
            this.cdata.i_deal = 0;
        }

        public  int read_deal()//should     public new int read_deal() 
        {
           // int i=0;
            byte cmd=(byte)Cmd.no_cmd;

            if (data_type == 0&&debug) Console.WriteLine(read_message);

            if (read_byte_count < 1) return 0;

            this.total_read_count += (ulong)this.read_byte_count;
            //this.display();//test
            if (this.index-this.deal_index < Cdata.min_len) return 0; //Data length is insufficient

            if (this.m_SyncContext != null && this.postCallback != null) //check and post main thread deal 
            {
                this.m_SyncContext.Post(this.postCallback, "read_deal");
            }

            while (true)
            {
                //0,1,2..min_len......index.........buf_len.
                while (this.deal_index <= this.index)
                {
                   // Console.WriteLine("read_deal():i=" +  ++ i);//test
                    cmd = cdata.what_recv(this.read_buffer, this.deal_index, this.index);
                    if (cmd != (byte)Cmd.no_cmd) break;
                    this.deal_index++;
                }
                //if (cmd == (byte)Cmd.no_cmd) return 0;//no command recv
                if (this.deal_index + Cdata.min_len > this.index) return 0;// start change //Data length is insufficient

                int len = cdata.get_length(this.read_buffer, this.deal_index, this.index,1);//type 1 recv length for scan recv length patch
                if (len == 0xFF)
                {
                    this.deal_index += Cdata.min_len; //ship deal get length fail data 
                    continue;
                }

                if (this.deal_index + Cdata.min_len+len> this.index) return 0; //Data length is insufficient
                
                //Console.WriteLine("index=" + this.index + "  deal=" + this.deal_index);//test
                this.deal_index = cdata.deal_data(this.read_buffer, this.deal_index, len+Cdata.min_len,true);//Data deal
                //Console.WriteLine("index=" + this.index + "  deal=" + this.deal_index);//test
                if(this.index+Cdata.max_len>(buffer_len/2)&&(this.deal_index+Cdata.min_len)>this.index) this.adjust_buffer();
                if(this.m_SyncContext!=null&&this.postCallback!=null) //check and post main thread deal 
                {
                    this.m_SyncContext.Post(this.postCallback, "read_deal");
                }
                //this.cdata.adjust_data_srecv();//for deal data adjust=false

                if (this.deal_index + Cdata.min_len < this.index) continue;// Check if there are next data package

                break; // end while 
             }//end  while (true)

            return 0;
        }

        public int check_and_post(string s)
        {
            if (this.m_SyncContext == null || this.postCallback == null) return -1; //check and post main thread deal 

            this.m_SyncContext.Post(this.postCallback, s);
            return 0;
        }

        public int check_and_post(byte [] b,int count )
        {
            string s = "";
            for (int i = 0; i < count; i++)
            {
                s += b[i].ToString("X2");
            }
            return this.check_and_post(s);
        }

        public void build_s_write_buf(byte[] b, int count,int start=0)
        {
            this.s_write_buf = "";
            for (int i = start; i < count; i++)
            {
                this.s_write_buf += b[i].ToString("X2");
            }
        }

        public  int write_deal() //should     public new int write_deal() 
        {
          //  if (data_type == 0)  write_byte_count = write_message.Length;
          if(cdata.s_len>0)
            {
                cdata.copy(cdata.s_buffer, this.write_buffer, cdata.s_len, Cdata.buf_len, Cserial.buffer_len);
                this.write_byte_count = cdata.s_len;
                this.total_read_count+= (ulong)this.write_byte_count;
                this.data_type = 1;//not string
                cdata.s_len = 0;
                this.strart_write = true;
                this.write_status = SerialStatus.InWrite;
                return 0;
            }

            return -1;
        }

        public void ImitateRead()
        {
            byte[] rd2 = { 0x78, 0x73, 0x63, 0x73, 0x01, 0x01, 0x2F, 0x00, 0x00, 0xD8, 0x6D, 0x12, 0x99, 0x50, 0xA0, 0x01, 0x02, 0x10, 0x06, 0x12, 0x09, 0x46, 0x4C, 0x59, 0x43, 0x4F, 0x20, 0x46, 0x48, 0x37, 0x30, 0x30, 0x31, 0x2F, 0x32, 0x2F, 0x33, 0x20, 0x08, 0x1B, 0x00, 0x6D, 0x12, 0x99, 0x50, 0xA0, 0x00, 0x78, 0x73, 0x63, 0x73, 0x01, 0x02, 0x2F, 0x00, 0x78, 0x73, 0x63, 0x73, 0x01, 0x01, 0x30, 0x00, 0x00, 0xD9, 0x6D, 0x12, 0x99, 0x50, 0xA0, 0x05, 0x02, 0x10, 0x06, 0x13, 0x09, 0x46, 0x46, 0x4C, 0x59, 0x43, 0x4F, 0x20, 0x46, 0x48, 0x37, 0x30, 0x30, 0x31, 0x2F, 0x32, 0x2F, 0x33, 0x20, 0x08, 0x1B, 0x00, 0x6D, 0x12, 0x99, 0x50, 0xA0, 0x00, 0x78, 0x73, 0x63, 0x73, 0x01, 0x02, 0x2F, 0x00 };

            while (Cserial._continue)
            {
                this.cdata.copy(rd2, this.read_buffer, rd2.Length, rd2.Length, Cserial.buffer_len, 0,this.index);
                this.read_byte_count = rd2.Length;
                this.index += rd2.Length;

                while(this.cdata.in_deal) System.Threading.Thread.Sleep(1);//wait and for other thread
                this.read_deal();
            }

        }

        public  void Read()
        {
            int read_count=0;
            if (debug) Console.WriteLine("Cserial:Read()\n");
            System.Threading.Thread.Sleep(1);//wait and for other
            //
            try
            {
                _serialPort.DiscardInBuffer();
            }catch(Exception e)
            {
                if (this.debug) Console.WriteLine(e.ToString());
            }

            this.read_status = SerialStatus.ReadStart;//start
            while (Cserial._continue)
            {
                try
                {
                    if (data_type == 0)
					{
                        read_message = _serialPort.ReadLine();
                        read_deal();
					}
                    else
                    {
                        try
                        {
                            read_count = this._serialPort.BytesToRead;
                            if (read_count < 1)
                            {
                                System.Threading.Thread.Sleep(this.read_sleep);//wait and for other
                                continue;
                            }
                        }
                        catch (InvalidOperationException e) { if (this.debug) Console.WriteLine(e.ToString()); break; }//InvalidOperationException

                        if (read_count > buffer_len - this.index)
                        {
                                if (this.index - this.deal_index > Cdata.min_len) read_deal();
                                if (buffer_len - this.index < buffer_len / 2) this.adjust_buffer();
                                read_count = buffer_len - this.index;
                        }

                        try//_serialPort.Read
                        {
                            read_byte_count = _serialPort.Read(this.read_buffer, this.index, read_count);
                            this.index += read_byte_count;
                            while (this.cdata.in_deal) System.Threading.Thread.Sleep(1);//wait and for other thread
                            read_deal();
                        }
                        catch (ArgumentNullException e) { if (this.debug) Console.WriteLine(e.ToString()); }
                        catch (InvalidOperationException e) { if (this.debug) Console.WriteLine(e.ToString()); break; }
                        catch (ArgumentOutOfRangeException e) { if (this.debug) Console.WriteLine(e.ToString()); }
                        catch (ArgumentException e) { if (this.debug) Console.WriteLine(e.ToString());}
                        catch (TimeoutException e) { if (this.debug) Console.WriteLine(e.ToString()); }
                        catch (Exception e) { if (this.debug) Console.WriteLine(e.ToString()); }
                    }//else

                }catch (Exception e) { if (this.debug) Console.WriteLine(e.ToString()); }
            }
            this.read_status = SerialStatus.ReadEnd;
        }

        public void wait_write_done()
        {
            if (this.debug) Console.WriteLine("wait_write_done()");
            int time_out = 100;
            System.Threading.Thread.Sleep(1);//wait and free cpu
            while (--time_out>0&&this.write_status != SerialStatus.WriteDone&&this.write_status != SerialStatus.WriteEnd)
            {
                System.Threading.Thread.Sleep(1);//wait and free cpu
            }
        }

        public  void Write()
        {
            if (debug) Console.WriteLine("Cserial:Write()\n");
            System.Threading.Thread.Sleep(1);//wait and for other
            this.write_status = SerialStatus.WriteStart;
            while (Cserial._continue)
            {

                write_deal();
                
                if (!strart_write)
                {
                    System.Threading.Thread.Sleep(1);//wait and free cpu
                    continue;//wait;
                }

                if(data_type ==0) 
                {
                    _serialPort.WriteLine(write_message);
                    write_message = "";
                }

                if (data_type != 0 && write_byte_count > 0)
                {
                    try
                    {
                        _serialPort.Write(write_buffer, 0, write_byte_count);
                        this.write_status = SerialStatus.WriteDone;
                        this.build_s_write_buf(write_buffer, write_byte_count);
                        this.check_and_post(this.s_write_buf);//post main thread
                        if (this.debug) Console.WriteLine(" Write()" + this.s_write_buf);
                    }
                    catch (ArgumentNullException)
                    {
                        if (this.debug) Console.WriteLine("catch(ArgumentNullException)");
                    }
                    catch (InvalidOperationException)
                    {
                        if (this.debug) Console.WriteLine("catch(InvalidOperationException)");
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        if (this.debug) Console.WriteLine("catch(ArgumentOutOfRangeException)");
                    }
                    catch (ArgumentException)
                    {
                        if (this.debug) Console.WriteLine("catch(ArgumentException)");
                    }
                    catch (TimeoutException)
                    {
                        if (this.debug) Console.WriteLine("catch(TimeoutException)");
                    }
                    catch (Exception e)
                    {
                        if (this.debug) Console.WriteLine("catch(Exception)"+e.ToString());
                    }
                                    
                    write_byte_count = 0;
                }
                strart_write = false;//write end
            }
            this.write_status = SerialStatus.WriteEnd;
        }

        // Display Port values and prompt user to enter a port.
        public  string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }
        // Display BaudRate values and prompt user to enter a value.
        public  int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }


    }//Cserial

}//n_serial