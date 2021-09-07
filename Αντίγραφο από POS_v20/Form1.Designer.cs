namespace POS_v20
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPortND = new System.IO.Ports.SerialPort(this.components);
            this.Debugging = new System.Windows.Forms.RichTextBox();
            this.serialPortRFID = new System.IO.Ports.SerialPort(this.components);
            this.serialBR = new System.IO.Ports.SerialPort(this.components);
            this.serialPRINTER = new System.IO.Ports.SerialPort(this.components);
            this.serialNV = new System.IO.Ports.SerialPort(this.components);
            this.StartApplication = new System.Windows.Forms.Button();
            this.LanguageTimer = new System.Windows.Forms.Timer(this.components);
            this.GeneralTimer = new System.Windows.Forms.Timer(this.components);
            this.test2 = new System.Windows.Forms.Button();
            this.Init_System = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.Settings = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.PrintCashValues = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.Paid50Notes = new System.Windows.Forms.TextBox();
            this.Paid20Notes = new System.Windows.Forms.TextBox();
            this.Paid05Notes = new System.Windows.Forms.TextBox();
            this.Avail10Notes = new System.Windows.Forms.TextBox();
            this.Paid10Notes = new System.Windows.Forms.TextBox();
            this.Avail05Notes = new System.Windows.Forms.TextBox();
            this.Coin200 = new System.Windows.Forms.TextBox();
            this.Coin100 = new System.Windows.Forms.TextBox();
            this.Coin50 = new System.Windows.Forms.TextBox();
            this.Coin20 = new System.Windows.Forms.TextBox();
            this.Coin10 = new System.Windows.Forms.TextBox();
            this.Coin5 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Save = new System.Windows.Forms.Button();
            this.Refresh = new System.Windows.Forms.Button();
            this.Coins = new System.Windows.Forms.TabPage();
            this.CoinsTest = new System.Windows.Forms.Button();
            this.CoinStatus = new System.Windows.Forms.Button();
            this.OpenCoins = new System.Windows.Forms.Button();
            this.NoteValidator = new System.Windows.Forms.TabPage();
            this.NVStatus = new System.Windows.Forms.Button();
            this.OpenNV = new System.Windows.Forms.Button();
            this.Printer = new System.Windows.Forms.TabPage();
            this.PRINTERStatus = new System.Windows.Forms.Button();
            this.OpenPRINTER = new System.Windows.Forms.Button();
            this.PrintTest = new System.Windows.Forms.Button();
            this.NoteDispenser = new System.Windows.Forms.TabPage();
            this.NotesTest = new System.Windows.Forms.Button();
            this.NDStatus = new System.Windows.Forms.Button();
            this.OpenND = new System.Windows.Forms.Button();
            this.RFID = new System.Windows.Forms.TabPage();
            this.RFIDStatus = new System.Windows.Forms.Button();
            this.OpenRFID = new System.Windows.Forms.Button();
            this.BarcodeReader = new System.Windows.Forms.TabPage();
            this.BRStatus = new System.Windows.Forms.Button();
            this.OpenBR = new System.Windows.Forms.Button();
            this.TCP_Connect = new System.Windows.Forms.Button();
            this.MainConfig = new System.Windows.Forms.TabControl();
            this.quitBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Settings.SuspendLayout();
            this.Coins.SuspendLayout();
            this.NoteValidator.SuspendLayout();
            this.Printer.SuspendLayout();
            this.NoteDispenser.SuspendLayout();
            this.RFID.SuspendLayout();
            this.BarcodeReader.SuspendLayout();
            this.MainConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPortND
            // 
            this.serialPortND.PortName = "COM5";
            this.serialPortND.ReceivedBytesThreshold = 6;
            this.serialPortND.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.ReceiveND);
            // 
            // Debugging
            // 
            this.Debugging.BackColor = System.Drawing.SystemColors.ControlText;
            this.Debugging.ForeColor = System.Drawing.SystemColors.Window;
            this.Debugging.Location = new System.Drawing.Point(14, 315);
            this.Debugging.Margin = new System.Windows.Forms.Padding(4);
            this.Debugging.Name = "Debugging";
            this.Debugging.ReadOnly = true;
            this.Debugging.Size = new System.Drawing.Size(511, 217);
            this.Debugging.TabIndex = 1;
            this.Debugging.Text = "System INIT\n";
            // 
            // serialPortRFID
            // 
            this.serialPortRFID.PortName = "COM6";
            this.serialPortRFID.ReadTimeout = 200;
            this.serialPortRFID.ReceivedBytesThreshold = 8;
            this.serialPortRFID.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.ReceiveRFID);
            // 
            // serialBR
            // 
            this.serialBR.PortName = "COM4";
            this.serialBR.ReadTimeout = 500;
            this.serialBR.WriteTimeout = 500;
            this.serialBR.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.ReceiveBR);
            // 
            // serialPRINTER
            // 
            this.serialPRINTER.Handshake = System.IO.Ports.Handshake.XOnXOff;
            this.serialPRINTER.RtsEnable = true;
            this.serialPRINTER.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.ReceivePRINTER);
            // 
            // serialNV
            // 
            this.serialNV.PortName = "COM3";
            this.serialNV.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.ReceiveNV);
            // 
            // StartApplication
            // 
            this.StartApplication.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.StartApplication.Enabled = false;
            this.StartApplication.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.StartApplication.Location = new System.Drawing.Point(577, 444);
            this.StartApplication.Margin = new System.Windows.Forms.Padding(4);
            this.StartApplication.Name = "StartApplication";
            this.StartApplication.Size = new System.Drawing.Size(169, 88);
            this.StartApplication.TabIndex = 4;
            this.StartApplication.Text = "Start Application";
            this.StartApplication.UseVisualStyleBackColor = false;
            this.StartApplication.Click += new System.EventHandler(this.StartApplication_Click);
            // 
            // LanguageTimer
            // 
            this.LanguageTimer.Interval = 1000;
            this.LanguageTimer.Tick += new System.EventHandler(this.Language_Tick);
            // 
            // GeneralTimer
            // 
            this.GeneralTimer.Interval = 500;
            this.GeneralTimer.Tick += new System.EventHandler(this.GeneralTimer_Tick);
            // 
            // test2
            // 
            this.test2.Location = new System.Drawing.Point(739, 539);
            this.test2.Margin = new System.Windows.Forms.Padding(4);
            this.test2.Name = "test2";
            this.test2.Size = new System.Drawing.Size(56, 30);
            this.test2.TabIndex = 6;
            this.test2.Text = "test2";
            this.test2.UseVisualStyleBackColor = true;
            this.test2.Visible = false;
            this.test2.Click += new System.EventHandler(this.test2_Click);
            // 
            // Init_System
            // 
            this.Init_System.BackColor = System.Drawing.Color.Yellow;
            this.Init_System.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Init_System.Location = new System.Drawing.Point(593, 346);
            this.Init_System.Margin = new System.Windows.Forms.Padding(4);
            this.Init_System.Name = "Init_System";
            this.Init_System.Size = new System.Drawing.Size(137, 71);
            this.Init_System.TabIndex = 7;
            this.Init_System.Text = "Init\r\nSystem";
            this.Init_System.UseVisualStyleBackColor = false;
            this.Init_System.Click += new System.EventHandler(this.Init_System_Click);
            // 
            // Settings
            // 
            this.Settings.BackColor = System.Drawing.Color.Silver;
            this.Settings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Settings.Controls.Add(this.label13);
            this.Settings.Controls.Add(this.label15);
            this.Settings.Controls.Add(this.label14);
            this.Settings.Controls.Add(this.PrintCashValues);
            this.Settings.Controls.Add(this.label12);
            this.Settings.Controls.Add(this.Paid50Notes);
            this.Settings.Controls.Add(this.Paid20Notes);
            this.Settings.Controls.Add(this.Paid05Notes);
            this.Settings.Controls.Add(this.Avail10Notes);
            this.Settings.Controls.Add(this.Paid10Notes);
            this.Settings.Controls.Add(this.Avail05Notes);
            this.Settings.Controls.Add(this.Coin200);
            this.Settings.Controls.Add(this.Coin100);
            this.Settings.Controls.Add(this.Coin50);
            this.Settings.Controls.Add(this.Coin20);
            this.Settings.Controls.Add(this.Coin10);
            this.Settings.Controls.Add(this.Coin5);
            this.Settings.Controls.Add(this.label11);
            this.Settings.Controls.Add(this.label10);
            this.Settings.Controls.Add(this.label9);
            this.Settings.Controls.Add(this.label8);
            this.Settings.Controls.Add(this.label7);
            this.Settings.Controls.Add(this.label6);
            this.Settings.Controls.Add(this.label5);
            this.Settings.Controls.Add(this.label4);
            this.Settings.Controls.Add(this.label3);
            this.Settings.Controls.Add(this.label2);
            this.Settings.Controls.Add(this.label1);
            this.Settings.Controls.Add(this.Save);
            this.Settings.Controls.Add(this.Refresh);
            this.Settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Settings.Location = new System.Drawing.Point(4, 22);
            this.Settings.Margin = new System.Windows.Forms.Padding(4);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(751, 239);
            this.Settings.TabIndex = 6;
            this.Settings.Text = "Settings";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label13.Location = new System.Drawing.Point(208, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(143, 16);
            this.label13.TabIndex = 28;
            this.label13.Text = "    Available Notes  ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label15.Location = new System.Drawing.Point(548, 29);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(177, 16);
            this.label15.TabIndex = 30;
            this.label15.Text = "       Available Coins        ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label14.Location = new System.Drawing.Point(354, 29);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(173, 16);
            this.label14.TabIndex = 29;
            this.label14.Text = "          Paid Notes            ";
            // 
            // PrintCashValues
            // 
            this.PrintCashValues.Enabled = false;
            this.PrintCashValues.Location = new System.Drawing.Point(29, 52);
            this.PrintCashValues.Margin = new System.Windows.Forms.Padding(4);
            this.PrintCashValues.Name = "PrintCashValues";
            this.PrintCashValues.Size = new System.Drawing.Size(121, 58);
            this.PrintCashValues.TabIndex = 27;
            this.PrintCashValues.Text = "Print Cash Values Report";
            this.PrintCashValues.UseVisualStyleBackColor = true;
            this.PrintCashValues.Click += new System.EventHandler(this.button1_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(466, 138);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "50 Euro";
            // 
            // Paid50Notes
            // 
            this.Paid50Notes.Location = new System.Drawing.Point(399, 135);
            this.Paid50Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Paid50Notes.Name = "Paid50Notes";
            this.Paid50Notes.Size = new System.Drawing.Size(59, 19);
            this.Paid50Notes.TabIndex = 25;
            this.Paid50Notes.Text = "0";
            this.Paid50Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid20Notes
            // 
            this.Paid20Notes.Location = new System.Drawing.Point(399, 108);
            this.Paid20Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Paid20Notes.Name = "Paid20Notes";
            this.Paid20Notes.Size = new System.Drawing.Size(59, 19);
            this.Paid20Notes.TabIndex = 23;
            this.Paid20Notes.Text = "0";
            this.Paid20Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid05Notes
            // 
            this.Paid05Notes.Location = new System.Drawing.Point(399, 54);
            this.Paid05Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Paid05Notes.Name = "Paid05Notes";
            this.Paid05Notes.Size = new System.Drawing.Size(59, 19);
            this.Paid05Notes.TabIndex = 21;
            this.Paid05Notes.Text = "0";
            this.Paid05Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Avail10Notes
            // 
            this.Avail10Notes.Location = new System.Drawing.Point(232, 81);
            this.Avail10Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Avail10Notes.Name = "Avail10Notes";
            this.Avail10Notes.ReadOnly = true;
            this.Avail10Notes.Size = new System.Drawing.Size(59, 19);
            this.Avail10Notes.TabIndex = 19;
            this.Avail10Notes.Text = "0";
            this.Avail10Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid10Notes
            // 
            this.Paid10Notes.Location = new System.Drawing.Point(399, 81);
            this.Paid10Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Paid10Notes.Name = "Paid10Notes";
            this.Paid10Notes.Size = new System.Drawing.Size(59, 19);
            this.Paid10Notes.TabIndex = 16;
            this.Paid10Notes.Text = "0";
            this.Paid10Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Avail05Notes
            // 
            this.Avail05Notes.Location = new System.Drawing.Point(232, 54);
            this.Avail05Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Avail05Notes.Name = "Avail05Notes";
            this.Avail05Notes.Size = new System.Drawing.Size(59, 19);
            this.Avail05Notes.TabIndex = 15;
            this.Avail05Notes.Text = "0";
            this.Avail05Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Coin200
            // 
            this.Coin200.Location = new System.Drawing.Point(579, 189);
            this.Coin200.Margin = new System.Windows.Forms.Padding(4);
            this.Coin200.Name = "Coin200";
            this.Coin200.ReadOnly = true;
            this.Coin200.Size = new System.Drawing.Size(61, 19);
            this.Coin200.TabIndex = 7;
            this.Coin200.Text = "0";
            this.Coin200.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Coin100
            // 
            this.Coin100.Location = new System.Drawing.Point(579, 162);
            this.Coin100.Margin = new System.Windows.Forms.Padding(4);
            this.Coin100.Name = "Coin100";
            this.Coin100.ReadOnly = true;
            this.Coin100.Size = new System.Drawing.Size(61, 19);
            this.Coin100.TabIndex = 6;
            this.Coin100.Text = "0";
            this.Coin100.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Coin50
            // 
            this.Coin50.Location = new System.Drawing.Point(579, 135);
            this.Coin50.Margin = new System.Windows.Forms.Padding(4);
            this.Coin50.Name = "Coin50";
            this.Coin50.ReadOnly = true;
            this.Coin50.Size = new System.Drawing.Size(61, 19);
            this.Coin50.TabIndex = 5;
            this.Coin50.Text = "0";
            this.Coin50.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Coin20
            // 
            this.Coin20.Location = new System.Drawing.Point(579, 108);
            this.Coin20.Margin = new System.Windows.Forms.Padding(4);
            this.Coin20.Name = "Coin20";
            this.Coin20.ReadOnly = true;
            this.Coin20.Size = new System.Drawing.Size(61, 19);
            this.Coin20.TabIndex = 4;
            this.Coin20.Text = "0";
            this.Coin20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Coin10
            // 
            this.Coin10.Location = new System.Drawing.Point(579, 81);
            this.Coin10.Margin = new System.Windows.Forms.Padding(4);
            this.Coin10.Name = "Coin10";
            this.Coin10.ReadOnly = true;
            this.Coin10.Size = new System.Drawing.Size(61, 19);
            this.Coin10.TabIndex = 3;
            this.Coin10.Text = "0";
            this.Coin10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Coin5
            // 
            this.Coin5.Location = new System.Drawing.Point(579, 54);
            this.Coin5.Margin = new System.Windows.Forms.Padding(4);
            this.Coin5.Name = "Coin5";
            this.Coin5.ReadOnly = true;
            this.Coin5.Size = new System.Drawing.Size(61, 19);
            this.Coin5.TabIndex = 2;
            this.Coin5.Text = "0";
            this.Coin5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(466, 111);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "20 Euro";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(473, 57);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "5 Euro";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(292, 84);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "10 Euro";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(466, 84);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "10 Euro";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(299, 57);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "5 Euro";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(658, 192);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "2.00 €";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(658, 165);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "1.00 €";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(658, 138);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "0.50 €";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(658, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "0.20 €";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(658, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "0.10 €";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(658, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "0.05 €";
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(158, 159);
            this.Save.Margin = new System.Windows.Forms.Padding(4);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(121, 44);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Refresh
            // 
            this.Refresh.Location = new System.Drawing.Point(29, 159);
            this.Refresh.Margin = new System.Windows.Forms.Padding(4);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(121, 44);
            this.Refresh.TabIndex = 0;
            this.Refresh.Text = "Refresh";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // Coins
            // 
            this.Coins.BackColor = System.Drawing.Color.Silver;
            this.Coins.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Coins.Controls.Add(this.CoinsTest);
            this.Coins.Controls.Add(this.CoinStatus);
            this.Coins.Controls.Add(this.OpenCoins);
            this.Coins.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Coins.Location = new System.Drawing.Point(4, 22);
            this.Coins.Margin = new System.Windows.Forms.Padding(4);
            this.Coins.Name = "Coins";
            this.Coins.Size = new System.Drawing.Size(751, 239);
            this.Coins.TabIndex = 5;
            this.Coins.Text = "Coins";
            // 
            // CoinsTest
            // 
            this.CoinsTest.Enabled = false;
            this.CoinsTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.CoinsTest.Location = new System.Drawing.Point(370, 80);
            this.CoinsTest.Margin = new System.Windows.Forms.Padding(4);
            this.CoinsTest.Name = "CoinsTest";
            this.CoinsTest.Size = new System.Drawing.Size(168, 47);
            this.CoinsTest.TabIndex = 8;
            this.CoinsTest.Text = "Return Coins Test";
            this.CoinsTest.UseVisualStyleBackColor = true;
            this.CoinsTest.Visible = false;
            this.CoinsTest.Click += new System.EventHandler(this.CoinsTest_Click);
            // 
            // CoinStatus
            // 
            this.CoinStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.CoinStatus.Location = new System.Drawing.Point(25, 133);
            this.CoinStatus.Margin = new System.Windows.Forms.Padding(4);
            this.CoinStatus.Name = "CoinStatus";
            this.CoinStatus.Size = new System.Drawing.Size(117, 60);
            this.CoinStatus.TabIndex = 9;
            this.CoinStatus.Text = "Status";
            this.CoinStatus.UseVisualStyleBackColor = true;
            this.CoinStatus.Click += new System.EventHandler(this.CoinStatus_Click);
            // 
            // OpenCoins
            // 
            this.OpenCoins.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.OpenCoins.Location = new System.Drawing.Point(25, 46);
            this.OpenCoins.Margin = new System.Windows.Forms.Padding(4);
            this.OpenCoins.Name = "OpenCoins";
            this.OpenCoins.Size = new System.Drawing.Size(117, 59);
            this.OpenCoins.TabIndex = 8;
            this.OpenCoins.Text = "Open";
            this.OpenCoins.UseVisualStyleBackColor = true;
            this.OpenCoins.Click += new System.EventHandler(this.OpenCoins_Click);
            // 
            // NoteValidator
            // 
            this.NoteValidator.BackColor = System.Drawing.Color.Silver;
            this.NoteValidator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.NoteValidator.Controls.Add(this.NVStatus);
            this.NoteValidator.Controls.Add(this.OpenNV);
            this.NoteValidator.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.NoteValidator.Location = new System.Drawing.Point(4, 22);
            this.NoteValidator.Margin = new System.Windows.Forms.Padding(4);
            this.NoteValidator.Name = "NoteValidator";
            this.NoteValidator.Padding = new System.Windows.Forms.Padding(4);
            this.NoteValidator.Size = new System.Drawing.Size(751, 239);
            this.NoteValidator.TabIndex = 3;
            this.NoteValidator.Text = "NoteValidator";
            // 
            // NVStatus
            // 
            this.NVStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.NVStatus.Location = new System.Drawing.Point(25, 133);
            this.NVStatus.Margin = new System.Windows.Forms.Padding(4);
            this.NVStatus.Name = "NVStatus";
            this.NVStatus.Size = new System.Drawing.Size(117, 60);
            this.NVStatus.TabIndex = 7;
            this.NVStatus.Text = "Status";
            this.NVStatus.UseVisualStyleBackColor = true;
            this.NVStatus.Click += new System.EventHandler(this.NVStatus_Click);
            // 
            // OpenNV
            // 
            this.OpenNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.OpenNV.Location = new System.Drawing.Point(25, 46);
            this.OpenNV.Margin = new System.Windows.Forms.Padding(4);
            this.OpenNV.Name = "OpenNV";
            this.OpenNV.Size = new System.Drawing.Size(117, 59);
            this.OpenNV.TabIndex = 6;
            this.OpenNV.Text = "Open";
            this.OpenNV.UseVisualStyleBackColor = true;
            this.OpenNV.Click += new System.EventHandler(this.OpenNV_Click);
            // 
            // Printer
            // 
            this.Printer.BackColor = System.Drawing.Color.Silver;
            this.Printer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Printer.Controls.Add(this.PRINTERStatus);
            this.Printer.Controls.Add(this.OpenPRINTER);
            this.Printer.Controls.Add(this.PrintTest);
            this.Printer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Printer.Location = new System.Drawing.Point(4, 22);
            this.Printer.Margin = new System.Windows.Forms.Padding(4);
            this.Printer.Name = "Printer";
            this.Printer.Size = new System.Drawing.Size(751, 239);
            this.Printer.TabIndex = 4;
            this.Printer.Text = "Printer";
            // 
            // PRINTERStatus
            // 
            this.PRINTERStatus.Location = new System.Drawing.Point(25, 132);
            this.PRINTERStatus.Margin = new System.Windows.Forms.Padding(4);
            this.PRINTERStatus.Name = "PRINTERStatus";
            this.PRINTERStatus.Size = new System.Drawing.Size(117, 60);
            this.PRINTERStatus.TabIndex = 4;
            this.PRINTERStatus.Text = "Status";
            this.PRINTERStatus.UseVisualStyleBackColor = true;
            this.PRINTERStatus.Click += new System.EventHandler(this.PRINTERStatus_Click);
            // 
            // OpenPRINTER
            // 
            this.OpenPRINTER.Location = new System.Drawing.Point(25, 46);
            this.OpenPRINTER.Margin = new System.Windows.Forms.Padding(4);
            this.OpenPRINTER.Name = "OpenPRINTER";
            this.OpenPRINTER.Size = new System.Drawing.Size(117, 59);
            this.OpenPRINTER.TabIndex = 3;
            this.OpenPRINTER.Text = "Open";
            this.OpenPRINTER.UseVisualStyleBackColor = true;
            this.OpenPRINTER.Click += new System.EventHandler(this.OpenPRINTER_Click);
            // 
            // PrintTest
            // 
            this.PrintTest.Enabled = false;
            this.PrintTest.Location = new System.Drawing.Point(370, 80);
            this.PrintTest.Margin = new System.Windows.Forms.Padding(4);
            this.PrintTest.Name = "PrintTest";
            this.PrintTest.Size = new System.Drawing.Size(177, 50);
            this.PrintTest.TabIndex = 2;
            this.PrintTest.Text = "Print Receipt Test";
            this.PrintTest.UseVisualStyleBackColor = true;
            this.PrintTest.Click += new System.EventHandler(this.test_Click);
            // 
            // NoteDispenser
            // 
            this.NoteDispenser.BackColor = System.Drawing.Color.Silver;
            this.NoteDispenser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.NoteDispenser.Controls.Add(this.NotesTest);
            this.NoteDispenser.Controls.Add(this.NDStatus);
            this.NoteDispenser.Controls.Add(this.OpenND);
            this.NoteDispenser.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.NoteDispenser.Location = new System.Drawing.Point(4, 22);
            this.NoteDispenser.Margin = new System.Windows.Forms.Padding(4);
            this.NoteDispenser.Name = "NoteDispenser";
            this.NoteDispenser.Padding = new System.Windows.Forms.Padding(4);
            this.NoteDispenser.Size = new System.Drawing.Size(751, 239);
            this.NoteDispenser.TabIndex = 2;
            this.NoteDispenser.Text = "NoteDispenser";
            // 
            // NotesTest
            // 
            this.NotesTest.Enabled = false;
            this.NotesTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.NotesTest.Location = new System.Drawing.Point(370, 80);
            this.NotesTest.Margin = new System.Windows.Forms.Padding(4);
            this.NotesTest.Name = "NotesTest";
            this.NotesTest.Size = new System.Drawing.Size(185, 40);
            this.NotesTest.TabIndex = 9;
            this.NotesTest.Text = "Return Notes Test";
            this.NotesTest.UseVisualStyleBackColor = true;
            this.NotesTest.Visible = false;
            this.NotesTest.Click += new System.EventHandler(this.NotesTest_Click);
            // 
            // NDStatus
            // 
            this.NDStatus.Location = new System.Drawing.Point(25, 132);
            this.NDStatus.Margin = new System.Windows.Forms.Padding(4);
            this.NDStatus.Name = "NDStatus";
            this.NDStatus.Size = new System.Drawing.Size(117, 60);
            this.NDStatus.TabIndex = 2;
            this.NDStatus.Text = "Status";
            this.NDStatus.UseVisualStyleBackColor = true;
            this.NDStatus.Click += new System.EventHandler(this.NDStatus_Click);
            // 
            // OpenND
            // 
            this.OpenND.Location = new System.Drawing.Point(25, 46);
            this.OpenND.Margin = new System.Windows.Forms.Padding(4);
            this.OpenND.Name = "OpenND";
            this.OpenND.Size = new System.Drawing.Size(117, 59);
            this.OpenND.TabIndex = 0;
            this.OpenND.Text = "Open";
            this.OpenND.UseVisualStyleBackColor = true;
            this.OpenND.Click += new System.EventHandler(this.OpenND_Click);
            // 
            // RFID
            // 
            this.RFID.BackColor = System.Drawing.Color.Silver;
            this.RFID.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RFID.Controls.Add(this.RFIDStatus);
            this.RFID.Controls.Add(this.OpenRFID);
            this.RFID.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.RFID.Location = new System.Drawing.Point(4, 22);
            this.RFID.Margin = new System.Windows.Forms.Padding(4);
            this.RFID.Name = "RFID";
            this.RFID.Padding = new System.Windows.Forms.Padding(4);
            this.RFID.Size = new System.Drawing.Size(751, 239);
            this.RFID.TabIndex = 1;
            this.RFID.Text = "RFID";
            // 
            // RFIDStatus
            // 
            this.RFIDStatus.Location = new System.Drawing.Point(25, 132);
            this.RFIDStatus.Margin = new System.Windows.Forms.Padding(4);
            this.RFIDStatus.Name = "RFIDStatus";
            this.RFIDStatus.Size = new System.Drawing.Size(117, 60);
            this.RFIDStatus.TabIndex = 4;
            this.RFIDStatus.Text = "Status";
            this.RFIDStatus.UseVisualStyleBackColor = true;
            this.RFIDStatus.Click += new System.EventHandler(this.RFIDStatus_Click);
            // 
            // OpenRFID
            // 
            this.OpenRFID.Location = new System.Drawing.Point(25, 46);
            this.OpenRFID.Margin = new System.Windows.Forms.Padding(4);
            this.OpenRFID.Name = "OpenRFID";
            this.OpenRFID.Size = new System.Drawing.Size(117, 59);
            this.OpenRFID.TabIndex = 3;
            this.OpenRFID.Text = "Open";
            this.OpenRFID.UseVisualStyleBackColor = true;
            this.OpenRFID.Click += new System.EventHandler(this.OpenRFID_Click);
            // 
            // BarcodeReader
            // 
            this.BarcodeReader.BackColor = System.Drawing.Color.Silver;
            this.BarcodeReader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BarcodeReader.Controls.Add(this.BRStatus);
            this.BarcodeReader.Controls.Add(this.OpenBR);
            this.BarcodeReader.Controls.Add(this.TCP_Connect);
            this.BarcodeReader.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.BarcodeReader.Location = new System.Drawing.Point(4, 22);
            this.BarcodeReader.Margin = new System.Windows.Forms.Padding(4);
            this.BarcodeReader.Name = "BarcodeReader";
            this.BarcodeReader.Padding = new System.Windows.Forms.Padding(4);
            this.BarcodeReader.Size = new System.Drawing.Size(751, 239);
            this.BarcodeReader.TabIndex = 0;
            this.BarcodeReader.Text = "BarcodeReader";
            // 
            // BRStatus
            // 
            this.BRStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.BRStatus.Location = new System.Drawing.Point(25, 132);
            this.BRStatus.Margin = new System.Windows.Forms.Padding(4);
            this.BRStatus.Name = "BRStatus";
            this.BRStatus.Size = new System.Drawing.Size(117, 60);
            this.BRStatus.TabIndex = 5;
            this.BRStatus.Text = "Status";
            this.BRStatus.UseVisualStyleBackColor = true;
            this.BRStatus.Click += new System.EventHandler(this.BRStatus_Click);
            // 
            // OpenBR
            // 
            this.OpenBR.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.OpenBR.Location = new System.Drawing.Point(25, 46);
            this.OpenBR.Margin = new System.Windows.Forms.Padding(4);
            this.OpenBR.Name = "OpenBR";
            this.OpenBR.Size = new System.Drawing.Size(117, 59);
            this.OpenBR.TabIndex = 0;
            this.OpenBR.Text = "Open";
            this.OpenBR.UseVisualStyleBackColor = true;
            this.OpenBR.Click += new System.EventHandler(this.OpenBR_Click);
            // 
            // TCP_Connect
            // 
            this.TCP_Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.TCP_Connect.Location = new System.Drawing.Point(370, 80);
            this.TCP_Connect.Margin = new System.Windows.Forms.Padding(4);
            this.TCP_Connect.Name = "TCP_Connect";
            this.TCP_Connect.Size = new System.Drawing.Size(137, 55);
            this.TCP_Connect.TabIndex = 5;
            this.TCP_Connect.Text = "TCP Connect";
            this.TCP_Connect.UseVisualStyleBackColor = true;
            this.TCP_Connect.Click += new System.EventHandler(this.TCP_Connect_Click);
            // 
            // MainConfig
            // 
            this.MainConfig.Controls.Add(this.BarcodeReader);
            this.MainConfig.Controls.Add(this.RFID);
            this.MainConfig.Controls.Add(this.NoteDispenser);
            this.MainConfig.Controls.Add(this.Printer);
            this.MainConfig.Controls.Add(this.NoteValidator);
            this.MainConfig.Controls.Add(this.Coins);
            this.MainConfig.Controls.Add(this.Settings);
            this.MainConfig.Enabled = false;
            this.MainConfig.HotTrack = true;
            this.MainConfig.Location = new System.Drawing.Point(13, 39);
            this.MainConfig.Margin = new System.Windows.Forms.Padding(4);
            this.MainConfig.Name = "MainConfig";
            this.MainConfig.SelectedIndex = 0;
            this.MainConfig.Size = new System.Drawing.Size(759, 265);
            this.MainConfig.TabIndex = 0;
            // 
            // quitBtn
            // 
            this.quitBtn.BackColor = System.Drawing.Color.OrangeRed;
            this.quitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.quitBtn.Location = new System.Drawing.Point(676, 11);
            this.quitBtn.Margin = new System.Windows.Forms.Padding(4);
            this.quitBtn.Name = "quitBtn";
            this.quitBtn.Size = new System.Drawing.Size(80, 43);
            this.quitBtn.TabIndex = 8;
            this.quitBtn.Text = "Quit ";
            this.quitBtn.UseVisualStyleBackColor = false;
            this.quitBtn.Click += new System.EventHandler(this.quitBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(663, 539);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 30);
            this.button1.TabIndex = 9;
            this.button1.Text = "Test3";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(784, 568);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.quitBtn);
            this.Controls.Add(this.Init_System);
            this.Controls.Add(this.test2);
            this.Controls.Add(this.StartApplication);
            this.Controls.Add(this.Debugging);
            this.Controls.Add(this.MainConfig);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "POS Terminal";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Closing);
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.Coins.ResumeLayout(false);
            this.NoteValidator.ResumeLayout(false);
            this.Printer.ResumeLayout(false);
            this.NoteDispenser.ResumeLayout(false);
            this.RFID.ResumeLayout(false);
            this.BarcodeReader.ResumeLayout(false);
            this.MainConfig.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPortND;
        private System.Windows.Forms.RichTextBox Debugging;
        private System.IO.Ports.SerialPort serialPortRFID;
        private System.IO.Ports.SerialPort serialBR;
        private System.IO.Ports.SerialPort serialPRINTER;
        private System.IO.Ports.SerialPort serialNV;
        private System.Windows.Forms.Button StartApplication;
        private System.Windows.Forms.Timer GeneralTimer;
        private System.Windows.Forms.Button test2;
        private System.Windows.Forms.Button Init_System;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button PrintCashValues;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox Paid50Notes;
        private System.Windows.Forms.TextBox Paid20Notes;
        private System.Windows.Forms.TextBox Paid05Notes;
        private System.Windows.Forms.TextBox Avail10Notes;
        private System.Windows.Forms.TextBox Paid10Notes;
        private System.Windows.Forms.TextBox Avail05Notes;
        private System.Windows.Forms.TextBox Coin200;
        private System.Windows.Forms.TextBox Coin100;
        private System.Windows.Forms.TextBox Coin50;
        private System.Windows.Forms.TextBox Coin20;
        private System.Windows.Forms.TextBox Coin10;
        private System.Windows.Forms.TextBox Coin5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.TabPage Coins;
        private System.Windows.Forms.Button CoinsTest;
        private System.Windows.Forms.Button CoinStatus;
        private System.Windows.Forms.Button OpenCoins;
        private System.Windows.Forms.TabPage NoteValidator;
        private System.Windows.Forms.Button NVStatus;
        private System.Windows.Forms.Button OpenNV;
        private System.Windows.Forms.TabPage Printer;
        private System.Windows.Forms.Button PRINTERStatus;
        private System.Windows.Forms.Button OpenPRINTER;
        private System.Windows.Forms.Button PrintTest;
        private System.Windows.Forms.TabPage NoteDispenser;
        private System.Windows.Forms.Button NotesTest;
        private System.Windows.Forms.Button NDStatus;
        private System.Windows.Forms.Button OpenND;
        private System.Windows.Forms.TabPage RFID;
        private System.Windows.Forms.Button RFIDStatus;
        private System.Windows.Forms.Button OpenRFID;
        private System.Windows.Forms.TabPage BarcodeReader;
        private System.Windows.Forms.Button BRStatus;
        private System.Windows.Forms.Button OpenBR;
        private System.Windows.Forms.Button TCP_Connect;
        private System.Windows.Forms.TabControl MainConfig;
        private System.Windows.Forms.Button quitBtn;
        private System.Windows.Forms.Timer LanguageTimer;
        private System.Windows.Forms.Button button1;
    }
}

