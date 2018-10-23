namespace n_language
{
    public enum LanguageSel
    {
        NoneInit,
        English,
        Chinese,
    }

    //ls_xxx : Language String xxxx         
    //sel: select

    public class Clanguage
    {
        public static LanguageSel lang= LanguageSel.NoneInit;
		//company
		public static string ls_legal_affairs;//LegalAffairs
		public static string ls_environment_safety;//environment
		public static string ls_require;//Require
		public static string ls_training;//Training
		public static string ls_communicate;//Communicate
		public static string ls_service;//Service
		public static string ls_task;//Task
		public static string ls_question;//Question
		public static string ls_business;
		public static string ls_develop;
		public static string ls_project;
		public static string ls_materials;
		public static string ls_fiscal;
		public static string ls_personnel;
		public static string ls_stock;
		public static string ls_group;
		public static string ls_edit;
		public static string ls_organization;
		public static string ls_record;
		public static string ls_workspace;
		public static string ls_company;
		//Public 
		public static string ls_colon;
        public static string ls_serial_port;
        public static string ls_error;
        public static string ls_please;
        public static string ls_check;
        public static string ls_clear;
        public static string ls_status;
        public static string ls_owner;
        public static string ls_setting;
        public static string ls_test;
        public static string ls_test_setting;
        public static string ls_output;
        public static string ls_input;
        public static string ls_message;
        public static string ls_if_or_not;
        public static string ls_success;
        public static string ls_no;
        public static string ls_content;
        public static string ls_prompt;
        public static string ls_text;
        public static string ls_time;
        public static string ls_log;
        public static string ls_open;
		public static string ls_database;
		//Language
		public static string ls_english;
        public static string ls_chinese;
        //Menu Bar string
        public static string ls_file;
        public static string ls_option;
        public static string ls_view;
        public static string ls_step;
        public static string ls_start;
        public static string ls_stop;
        public static string ls_help;
        public static string ls_language;
        public static string ls_import;
        public static string ls_import_fail;
        public static string ls_import_sucess;
        public static string ls_export;
        public static string ls_export_fail;
        public static string ls_export_sucess;
        public static string ls_exit;
        public static string ls_order;
        public static string ls_rescan_serial_port;
        public static string ls_connect;
        public static string ls_stable;
        public static string ls_disconnect;
        public static string ls_has_stop;
        public static string ls_modify;
        public static string ls_auto_test;
        public static string ls_fail_log;
        public static string ls_fail_export;
        public static string ls_product;
        //StripButton
        public static string ls_scan;
        public static string ls_open_com;
        public static string ls_close_com;
        //From
        public static string ls_bluetooth_test;
        public static string ls_verion_h;//help
        //serial
        public static string ls_com_port;
        public static string ls_baud_rate;
        public static string ls_data_bits;
        public static string ls_parity;
        public static string ls_stop_bits;
        public static string ls_product_line;
        public static string ls_p_line;
        public static string ls_order_no;
        public static string ls_worker_no;
        public static string ls_server_name;
        public static string ls_device_type;
        public static string ls_mac_select;
        //Ctest
        public static string ls_not_test;
        public static string ls_pass;
        public static string ls_fail;
        public static string ls_result;
        public static string ls_warning;
        //Other
        public static string ls_error_no_serial_port;
        public static string ls_error_no_serial_start;
        public static string ls_com_is_open;
        //datagridview
        public static string ls_select;
        public static string ls_id;
        public static string ls_number;
        public static string ls_item;
        public static string ls_min;
        public static string ls_max;
        public static string ls_mask;
        public static string ls_must;
        public static string ls_value;
        public static string ls_datagridview_status;
        public static string ls_remark;

        //test item
        public static string ls_name;
        public static string ls_p_name;
        public static string ls_version;
        public static string ls_mac;
        public static string ls_rssi;
        public static string ls_voltage;
        public static string ls_resistance;
        public static string ls_resistor;
        public static string ls_weight;
        public static string ls_total;
        public static string ls_pass_count;
        public static string ls_fail_count;
        public static string ls_waring_count;
        public static string ls_nottest_count;
        //export list
        public static string ls_exportlist;
        public static string ls_importlist;
        public static string ls_save_setting;
        public static string ls_load_setting;
        public static string ls_test_list;
        public static string ls_new;
        public static string ls_signal;
        public static string ls_worker;
        public static string ls_operator;
        public static string ls_wait_stable;
        public static string ls_disconn_time;
        //order
        public static string ls_entry_order;
        public static string ls_inquiry_order;
        public static string ls_order_status;
        public static string ls_product_name;
        public static string ls_quantity;
        public static string ls_unit_price;
        public static string ls_total_amount;
        public static string ls_order_owner;
        public static string ls_start_time;
        public static string ls_delivery_time;
        //
        public static string ls_feike;
        public static string ls_complete;
        public static string ls_inquiry;
        public static string ls_condition;
        public static string ls_increase;
        public static string ls_entry_worker;
        public static string ls_inquiry_worker;
        public static string ls_delete_worker;
        public static string ls_delete_id;
        public static string ls_employee;
        public static string ls_login;
        public static string ls_register;
        public static string ls_user_name;
        public static string ls_password;
        public static string ls_check_ditto;
        public static string ls_q_check_ditto;
        public static string ls_q_ditto_test;
        public static string ls_q_restart_again;
        public static string ls_ble_chip;
        public static string ls_auto_acq;
        public static string ls_i_db_not_conn;//i:informaotion
        public static string ls_db_conn_success;
        public static string ls_db_conn_fail;
        //customer
        public static string ls_customer;
        public static string ls_customer_name;
        public static string ls_customer_type;
        public static string ls_websit;
        public static string ls_pioneer;
        public static string ls_country;
        public static string ls_country_area;
        public static string ls_introduction;
        public static string ls_trade_product;
        public static string ls_coordinate;
        public static string ls_work_group_name;
        public static string ls_mail;
        public static string ls_contact;
        public static string ls_credit_points;
        public static string ls_end_time;
        public static string ls_file_path;
        public static string ls_address;
        public static string ls_level;
        public static string ls_enter_customer;
        public static string ls_inquiry_customer;
        public static string ls_debug;
        public static string ls_debug_mode;
        public static string ls_del_db;
        //product_line
        public static string ls_product_line_number;
        public static string ls_product_line_name;
        public static string ls_product_line_who;
        public static string ls_product_line_level;
        public static string ls_product_line_what;
        public static string ls_product_line_where;
        public static string ls_product_line_status;
        public static string ls_product_line_capacity;
        public static string ls_product_line_contact;
        public static string ls_product_line_create;
        public static string ls_product_line_start;
        public static string ls_product_line_end;
        public static string ls_product_line_entry;
        public static string ls_product_line_inquire;
        public static string ls_product_line_delete;
        public static string ls_product_line_modify;
        public static string ls_insert_product_line;
        public static string ls_product_capacity;
        public static string ls_automatic_acquisition;
        //fun
        public Clanguage(LanguageSel sel= LanguageSel.English)
        {
            Clanguage.sel_language(sel);
            Clanguage.lang = sel;
        }

        public static void sel_language(LanguageSel sel = LanguageSel.English)
        {
            Clanguage.ls_english = "English";
            Clanguage.ls_chinese = "简体中文";
            Clanguage.ls_colon = ":";
            if (sel == LanguageSel.English) Clanguage.sel_english();
            if (sel == LanguageSel.Chinese) Clanguage.sel_chinese();
            Clanguage.lang = sel;
        }

        public static void sel_english()
        {
            if (Clanguage.lang == LanguageSel.English) return;
			Clanguage.ls_stock = "Stock";
			Clanguage.ls_group = "Group";
			Clanguage.ls_edit = "Edit";
			Clanguage.ls_organization = "Organization";
			Clanguage.ls_record = "Record";
			Clanguage.ls_workspace = "Workspace";
			Clanguage.ls_company = "Company";
			Clanguage.ls_personnel = "Personnel";
			Clanguage.ls_fiscal = "Fiscal";
			Clanguage.ls_materials = "Materials";
			Clanguage.ls_project = "Project";
			Clanguage.ls_develop = "Develop";
			Clanguage.ls_business = "Business";
			Clanguage.ls_legal_affairs = "LegalAffairs";
			Clanguage.ls_environment_safety = "environment";
			Clanguage.ls_require = "Require";
			Clanguage.ls_training = "Training";
			Clanguage.ls_communicate = "Communicate";
			Clanguage.ls_service = "Service";
			Clanguage.ls_task = "Task";
			Clanguage.ls_question = "Question";
			Clanguage.ls_error = "Error";
            Clanguage.ls_please = "Please";
            Clanguage.ls_check = "Check";
            Clanguage.ls_clear = "Clear";
            Clanguage.ls_status = "Status";
            Clanguage.ls_owner = "Owner";
            Clanguage.ls_file = "File";
            Clanguage.ls_option = "Option";
            Clanguage.ls_view = "View";
            Clanguage.ls_step="Step";
            Clanguage.ls_start="Start";
            Clanguage.ls_stop="Stop";
            Clanguage.ls_help="Help";
            Clanguage.ls_language="Language";
            Clanguage.ls_if_or_not = "If or Not ";
            Clanguage.ls_success = "Success";
            Clanguage.ls_no = "No";
            Clanguage.ls_text = "Text";
            Clanguage.ls_log = "Log";
            Clanguage.ls_open = "Open";
			Clanguage.ls_database = "DataBase";
            Clanguage.ls_content = "Content";
            Clanguage.ls_prompt = "Prompt";
            Clanguage.ls_import="Import";
            Clanguage.ls_import_fail = "Import Fail ";
            Clanguage.ls_import_sucess = "Import Success ";
            Clanguage.ls_export="Export";
            Clanguage.ls_export_fail="Export Fail ";
            Clanguage.ls_export_sucess= "Export Success ";
            Clanguage.ls_exit="Exit";
            Clanguage.ls_order="Order";
            Clanguage.ls_rescan_serial_port="RescanSerialPort";
            Clanguage.ls_connect="Connect";
            Clanguage.ls_disconnect="Disconnect";
            Clanguage.ls_bluetooth_test = "BluetoothTest";
            Clanguage.ls_verion_h = "Version";
            Clanguage.ls_serial_port = "SerialPort";
            Clanguage.ls_fail_log = "FailLog";
            Clanguage.ls_product = "Product";
            Clanguage.ls_scan = "Scan";
            Clanguage.ls_stable = "Stable";
            Clanguage.ls_open_com = "Open COM";
            Clanguage.ls_close_com = "Close COM";
            Clanguage.ls_setting = "Setting";
            Clanguage.ls_test = "Test";
            Clanguage.ls_time = "Time";
            Clanguage.ls_modify = "Modify";
            Clanguage.ls_auto_test = "AutoTest";
            Clanguage.ls_test_setting = "TestSetting";
            Clanguage.ls_output = "Output";
            Clanguage.ls_input = "Input";
            Clanguage.ls_com_port="ComPort";
            Clanguage.ls_baud_rate="BaudRate";
            Clanguage.ls_data_bits="DataBits";
            Clanguage.ls_parity="Parity";
            Clanguage.ls_stop_bits="StopBits";
            Clanguage.ls_product_line="ProductLine";
            Clanguage.ls_p_line = "Line";
            Clanguage.ls_order_no = "OrderNO.";
            Clanguage.ls_worker_no = "WorkerNO.";
            Clanguage.ls_server_name = "ServerName";
            Clanguage.ls_device_type = "DeviceType";
            Clanguage.ls_mac_select = "MAC Select";
            Clanguage.ls_error_no_serial_port = "Error:No serial port found!";
            Clanguage.ls_error_no_serial_start="Error:No serial port start!";
            Clanguage.ls_com_is_open = "COM is opening";
            Clanguage.ls_not_test = "Result:[NotTest]";
            Clanguage.ls_fail = "Result:[Fail]";
            Clanguage.ls_pass = "Result:[Pass]";
            Clanguage.ls_fail_export = "Fail";
            Clanguage.ls_result = "Result";
            Clanguage.ls_warning = "Warning";
            Clanguage.ls_has_stop = "Has stopped";
            Clanguage.ls_message = "Message";
            Clanguage.ls_select="select";
            Clanguage.ls_id="id";
            Clanguage.ls_number = "Number";
            Clanguage.ls_item="item";
            Clanguage.ls_min="min";
            Clanguage.ls_max="max";
            Clanguage.ls_mask="mask";
            Clanguage.ls_must = "must";
            Clanguage.ls_value="value";
            Clanguage.ls_datagridview_status="status";
            //test item
            Clanguage.ls_name= "Name";
            Clanguage.ls_p_name = "Name"; //iditem 1
            Clanguage.ls_version= "Version";
            Clanguage.ls_mac= "MAC";
            Clanguage.ls_rssi= "RSSI";
            Clanguage.ls_voltage= "Voltage";
            Clanguage.ls_resistance= "Resistance";
            Clanguage.ls_resistor = "Resistor";
            Clanguage.ls_weight= "Weight";
            Clanguage.ls_total = "Total";
            Clanguage.ls_pass_count="PassCount";
            Clanguage.ls_fail_count="FailCount";
            Clanguage.ls_waring_count="WarningCount";
            Clanguage.ls_nottest_count="NotTestCount";
            //export list
            Clanguage.ls_exportlist = "ExportList";
            Clanguage.ls_importlist = "InportList";
            Clanguage.ls_save_setting = "SaveSetting";
            Clanguage.ls_load_setting = "LoadSetting";
            Clanguage.ls_test_list = "TestingList";
            Clanguage.ls_new = "New";
            Clanguage.ls_signal = "Signal";
            Clanguage.ls_worker = "Worker";
            Clanguage.ls_operator = "Operator";
            Clanguage.ls_wait_stable = "WaitStable";
            Clanguage.ls_disconn_time = "DisconnTime";
            Clanguage.ls_entry_order = "EntryOrder";
            Clanguage.ls_inquiry_order = "InquiryOrder";
            Clanguage.ls_order_status = "OrderStatus";
            Clanguage.ls_product_name = "ProductName";
            Clanguage.ls_quantity = "Quantity";
            Clanguage.ls_unit_price = "UnitPrice";
            Clanguage.ls_total_amount = "TotalAmount";
            Clanguage.ls_order_owner = "OrderOwner";
            Clanguage.ls_start_time = "StartTime";
            Clanguage.ls_delivery_time = "DeliveryTime";
            Clanguage.ls_feike = "FLYCO";
            Clanguage.ls_complete = "Complete";
            Clanguage.ls_inquiry = "Inquiry";
            Clanguage.ls_condition = "Condition";
            Clanguage.ls_increase = "Increase";
            Clanguage.ls_entry_worker = "EntryWorker";
            Clanguage.ls_inquiry_worker = "InquiryWorker";
            Clanguage.ls_delete_worker = "DeleteWorker";
            Clanguage.ls_delete_id = "DeleteID";
            Clanguage.ls_employee = "Employee";
            Clanguage.ls_login = "Login";
            Clanguage.ls_register = "Register";
            Clanguage.ls_user_name = "UserName";
            Clanguage.ls_password = "Password";
            Clanguage.ls_remark = "Remark";
            Clanguage.ls_check_ditto = "CheckDitto";
            Clanguage.ls_q_check_ditto = "Do the same for two consecutive addresses, please confirm whether the test?";
            Clanguage.ls_q_ditto_test = "Repeat the same for two consecutive addresses, please confirm whether to repeat the test?";
            Clanguage.ls_q_restart_again = "Replace the serial port Please start again.";
            Clanguage.ls_ble_chip = "BLE Chip";
            Clanguage.ls_auto_acq = "Auto-Acquisition";
            Clanguage.ls_i_db_not_conn = "The database is not connected.";
            Clanguage.ls_db_conn_success = "The database connection is successful.";
            Clanguage.ls_db_conn_fail = "Database connection failed.";
            Clanguage.ls_customer = "Customer";
            Clanguage.ls_customer_name = "CustomerName";
            Clanguage.ls_customer_type = "CustomerType";
            Clanguage.ls_websit = "Website";
            Clanguage.ls_pioneer = "Pioneer";
            Clanguage.ls_country = "Country";
            Clanguage.ls_country_area = "CountryArea";
            Clanguage.ls_trade_product = "TradeProduct";
            Clanguage.ls_introduction = "Introduction";
            Clanguage.ls_work_group_name = "WorkingGroup";
            Clanguage.ls_mail = "e-mail";
            Clanguage.ls_contact = "Contact";
            Clanguage.ls_coordinate = "Coordinate";
            Clanguage.ls_credit_points = "CreditPoints";
            Clanguage.ls_end_time = "EndTime";
            Clanguage.ls_file_path = "FilePath";
            Clanguage.ls_address = "Address";
            Clanguage.ls_level = "Level";
            Clanguage.ls_enter_customer = "EnterCustomer";
            Clanguage.ls_inquiry_customer = "InquiryCustomer";
            Clanguage.ls_debug = "Debug";
            Clanguage.ls_debug_mode = "DebugMode";
            Clanguage.ls_del_db = "DeleteDatabase";
            //product line
            Clanguage.ls_product_line_number= "ProductLineNumber";
            Clanguage.ls_product_line_name = "ProductLineName";
            Clanguage.ls_product_line_who = "ProductLineWho";
            Clanguage.ls_product_line_level = "ProductLineLevel";
            Clanguage.ls_product_line_what = "ProductLineWhat";
            Clanguage.ls_product_line_where = "ProductLineWhere";
            Clanguage.ls_product_line_status = "ProductLineStatus";
            Clanguage.ls_product_line_capacity = "ProductLineCapacity";
            Clanguage.ls_product_line_contact = "ProductLineContact";
            Clanguage.ls_product_line_create = "ProductLineCreate";
            Clanguage.ls_product_line_start = "ProductLineStart";
            Clanguage.ls_product_line_end = "ProductLineEnd";
            Clanguage.ls_product_line_entry = "ProductLineEntry";
            Clanguage.ls_product_line_inquire = "ProductLineInquiry";
            Clanguage.ls_product_line_delete = "ProductLineDelete";
            Clanguage.ls_product_line_modify = "ProductLineModify";
            Clanguage.ls_insert_product_line = "Insert Product Line ";
            Clanguage.ls_product_capacity = "ProductCapacity";
            Clanguage.ls_automatic_acquisition = "Automatic acquisition";
        }

        public static void sel_chinese()
        {
            if (Clanguage.lang == LanguageSel.Chinese) return;
			Clanguage.ls_stock = "股份";
			Clanguage.ls_group = "集团";
			Clanguage.ls_edit = "编辑";
			Clanguage.ls_organization = "组织架构";
			Clanguage.ls_record = "记录";
			Clanguage.ls_workspace = "工作区";
			Clanguage.ls_company = "公司";
			Clanguage.ls_personnel = "人力";
			Clanguage.ls_fiscal = "财务";
			Clanguage.ls_materials = "物料";
			Clanguage.ls_develop = "研发";
			Clanguage.ls_project = "项目";
			Clanguage.ls_business = "商务";
			Clanguage.ls_legal_affairs = "法务";
			Clanguage.ls_environment_safety = "环保";
			Clanguage.ls_require = "需求";
			Clanguage.ls_training = "培训";
			Clanguage.ls_communicate = "通信";
			Clanguage.ls_service = "服务";
			Clanguage.ls_task = "任务";
			Clanguage.ls_question = "问题";
			Clanguage.ls_error = "错误";
            Clanguage.ls_please = "请";
            Clanguage.ls_check = "检查";
            Clanguage.ls_clear = "清除";
            Clanguage.ls_status = "状态";
            Clanguage.ls_owner = "所有者";
            Clanguage.ls_file = "文件";
            Clanguage.ls_option = "选项";
            Clanguage.ls_view = "视图";
            Clanguage.ls_step = "单步";
            Clanguage.ls_start = "开始";
            Clanguage.ls_stop = "停止";
            Clanguage.ls_help = "帮助";
            Clanguage.ls_language = "语言";
            Clanguage.ls_if_or_not = "是否 ";
            Clanguage.ls_success = "成功";
            Clanguage.ls_no = "没有";
            Clanguage.ls_text = "文本";
            Clanguage.ls_log = "记录";
            Clanguage.ls_open = "打开配置";
			Clanguage.ls_database = "数据库";
			Clanguage.ls_content = "内容";
            Clanguage.ls_prompt = "提示";
            Clanguage.ls_import="导入";
            Clanguage.ls_import_fail = "导入失败 ";
            Clanguage.ls_import_sucess = "导入成功 ";
            Clanguage.ls_export="导出";
            Clanguage.ls_export_fail = "导出失败 ";
            Clanguage.ls_export_sucess = "导出成功 ";
            Clanguage.ls_exit="退出";
            Clanguage.ls_order="订单";
            Clanguage.ls_rescan_serial_port="重新扫描串口";
            Clanguage.ls_connect="连接";
            Clanguage.ls_disconnect="断开";
            Clanguage.ls_bluetooth_test = "蓝牙测试";
            Clanguage.ls_verion_h = "版本";
            Clanguage.ls_serial_port = "串口";
            Clanguage.ls_scan = "扫描";
            Clanguage.ls_stable = "稳定";
            Clanguage.ls_open_com = "打开串口";
            Clanguage.ls_close_com = "关闭串口";
            Clanguage.ls_fail_log = "错误记录";
            Clanguage.ls_product = "产品";
            Clanguage.ls_setting = "设置";
            Clanguage.ls_test = "测试";
            Clanguage.ls_time = "时间";
            Clanguage.ls_modify = "修改";
            Clanguage.ls_auto_test = "自动测试";
            Clanguage.ls_test_setting = "测试设置";
            Clanguage.ls_output = "输出";
            Clanguage.ls_input = "输入";
            Clanguage.ls_com_port="串口号";
            Clanguage.ls_baud_rate="波特率";
            Clanguage.ls_data_bits="数据位";
            Clanguage.ls_parity = "奇偶校验";
            Clanguage.ls_stop_bits = "停止位";
            Clanguage.ls_product_line = "产线";
            Clanguage.ls_p_line = "产线";
            Clanguage.ls_order_no = "订单号";
            Clanguage.ls_worker_no = "工号";
            Clanguage.ls_server_name = "服务器";
            Clanguage.ls_device_type = "设备类型";
            Clanguage.ls_mac_select = "MAC选中";
            Clanguage.ls_error_no_serial_port = "错误:未找到串口!";
            Clanguage.ls_error_no_serial_start="错误:没有串口被开启!";
            Clanguage.ls_com_is_open = "串口已开启";
            Clanguage.ls_not_test = "结果:[未测试]";
            Clanguage.ls_fail = "结果:[失败]";
            Clanguage.ls_fail_export = "失败项";
            Clanguage.ls_pass = "结果:[通过]";
            Clanguage.ls_result = "结果";
            Clanguage.ls_warning = "警告";
            Clanguage.ls_has_stop = "已经停止";
            Clanguage.ls_message = "消息";
            Clanguage.ls_select = "选测";
            Clanguage.ls_id = "序号";
            Clanguage.ls_number = "编号";
            Clanguage.ls_item = "测项";
            Clanguage.ls_min = "最小值";
            Clanguage.ls_max = "最大值";
            Clanguage.ls_mask = "掩值";
            Clanguage.ls_must = "定值";
            Clanguage.ls_value = "测试值";
            Clanguage.ls_datagridview_status = "状态";
            //test item
            Clanguage.ls_name = "名称"; //iditem 1
            Clanguage.ls_p_name = "姓名"; //iditem 1
            Clanguage.ls_version = "版本号";//iditem 2
            Clanguage.ls_mac = "MAC";//iditem 3
            Clanguage.ls_rssi = "信号强度";//iditem 4
            Clanguage.ls_voltage = "电压";//iditem 5
            Clanguage.ls_resistance = "电阻";//iditem 6
            Clanguage.ls_resistor = "电阻";
            Clanguage.ls_weight = "重量";//iditem 7
            Clanguage.ls_total = "合计";
            Clanguage.ls_pass_count = "通过";
            Clanguage.ls_fail_count = "失败";
            Clanguage.ls_waring_count = "警告";
            Clanguage.ls_nottest_count = "未测试";
            //export list
            Clanguage.ls_exportlist = "导出清单";
            Clanguage.ls_importlist = "导入清单";
            Clanguage.ls_save_setting = "保存设置";
            Clanguage.ls_load_setting = "加载设置";
            Clanguage.ls_test_list = "测试列表";
            Clanguage.ls_new = "新建";
            Clanguage.ls_signal = "信号";
            Clanguage.ls_worker = "工人";
            Clanguage.ls_operator = "操作员";
            Clanguage.ls_wait_stable = "等待稳定";
            Clanguage.ls_disconn_time = "断开时间";
            Clanguage.ls_entry_order = "录入订单";
            Clanguage.ls_inquiry_order = "查询订单";
            Clanguage.ls_order_status = "订单状态";
            Clanguage.ls_product_name = "产品名称";
            Clanguage.ls_quantity = "数量";
            Clanguage.ls_unit_price = "单价";
            Clanguage.ls_total_amount = "总金额";
            Clanguage.ls_order_owner = "跟单员";
            Clanguage.ls_start_time = "签单时间";
            Clanguage.ls_delivery_time = "交期时间";
            Clanguage.ls_feike = "飞科";
            Clanguage.ls_complete = "完成";
            Clanguage.ls_inquiry = "查询";
            Clanguage.ls_condition = "条件";
            Clanguage.ls_increase = "新增";
            Clanguage.ls_entry_worker = "录入员工";
            Clanguage.ls_inquiry_worker = "查询员工";
            Clanguage.ls_delete_worker="删除员工";
            Clanguage.ls_delete_id="删除序号";
            Clanguage.ls_employee = "员工";
            Clanguage.ls_login = "登陆";
            Clanguage.ls_register = "注册";
            Clanguage.ls_user_name = "用户名";
            Clanguage.ls_password = "密码";
            Clanguage.ls_remark = "备注";
            Clanguage.ls_check_ditto = "重复地址确认";
            Clanguage.ls_q_ditto_test ="连续两次地址相同，请确认是否是重复测试 ?";
            Clanguage.ls_q_restart_again = "更换串口请重新开始.";
            Clanguage.ls_ble_chip = "BLE 芯片";
            Clanguage.ls_auto_acq = "自动获取";
            Clanguage.ls_i_db_not_conn = "数据库未连接";
            Clanguage.ls_db_conn_success = "数据库连接成功";
            Clanguage.ls_db_conn_fail = "数据库连接失败";
            Clanguage.ls_customer = "客户";
            Clanguage.ls_customer_name = "客户名";
            Clanguage.ls_customer_type = "客户类型";
            Clanguage.ls_websit = "网址";
            Clanguage.ls_pioneer = "开拓者";
            Clanguage.ls_country = "国家";
            Clanguage.ls_country_area = "地区";
            Clanguage.ls_trade_product = "交易品";
            Clanguage.ls_introduction = "简介";
            Clanguage.ls_work_group_name = "群组名";
            Clanguage.ls_mail = "e-mail";
            Clanguage.ls_contact = "联系人";
            Clanguage.ls_coordinate = "坐标";
            Clanguage.ls_credit_points = "信用分";
            Clanguage.ls_end_time = "完结时间";
            Clanguage.ls_file_path = "资料路径";
            Clanguage.ls_address = "地址";
            Clanguage.ls_level = "等级";
            Clanguage.ls_enter_customer = "录入客户";
            Clanguage.ls_inquiry_customer = "查询客户";
            Clanguage.ls_debug = "调试";
            Clanguage.ls_debug_mode = "调试模式";
            Clanguage.ls_del_db = "删除数据库";
            //product line
            Clanguage.ls_product_line_number = "产线号";
            Clanguage.ls_product_line_name = "产线名";
            Clanguage.ls_product_line_who = "产线所有人";
            Clanguage.ls_product_line_level = "产线级别";
            Clanguage.ls_product_line_what = "产线产品";
            Clanguage.ls_product_line_where = "产线地点";
            Clanguage.ls_product_line_status = "产线状态";
            Clanguage.ls_product_line_capacity = "产线产能";
            Clanguage.ls_product_line_contact = "联系方式";
            Clanguage.ls_product_line_create = "产线创建时间";
            Clanguage.ls_product_line_start = "产线投产时间";
            Clanguage.ls_product_line_end = "产线停产时间";
            Clanguage.ls_product_line_entry = "录入产线";
            Clanguage.ls_product_line_inquire = "查询产线";
            Clanguage.ls_product_line_delete = "删除产线";
            Clanguage.ls_product_line_modify = "修改产线";
            Clanguage.ls_insert_product_line = "插入产线";
            Clanguage.ls_product_capacity = "产能";
            Clanguage.ls_automatic_acquisition = "自动获取";
        }


}//end  Clanguage
    
}//end n_language