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
            this.serialEFT_POS = new System.IO.Ports.SerialPort(this.components);
            this.Debugging = new System.Windows.Forms.RichTextBox();
            this.serialRFID = new System.IO.Ports.SerialPort(this.components);
            this.serialBR = new System.IO.Ports.SerialPort(this.components);
            this.serialPRINTER = new System.IO.Ports.SerialPort(this.components);
            this.StartApplication = new System.Windows.Forms.Button();
            this.LanguageTimer = new System.Windows.Forms.Timer(this.components);
            this.GeneralTimer = new System.Windows.Forms.Timer(this.components);
            this.Init_System = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.Settings = new System.Windows.Forms.TabPage();
            this.label22 = new System.Windows.Forms.Label();
            this.Avail10Notes = new System.Windows.Forms.TextBox();
            this.ResetAllCoinsbtn = new System.Windows.Forms.Button();
            this.Paid200Coins = new System.Windows.Forms.TextBox();
            this.Paid100Coins = new System.Windows.Forms.TextBox();
            this.Paid50Coins = new System.Windows.Forms.TextBox();
            this.Paid20Coins = new System.Windows.Forms.TextBox();
            this.Paid10Coins = new System.Windows.Forms.TextBox();
            this.Paid05Coins = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.ResetAllNotesbtn = new System.Windows.Forms.Button();
            this.btnSmartEmpty = new System.Windows.Forms.Button();
            this.CloseBatchButton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.PrintCashValues = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.Paid50Notes = new System.Windows.Forms.TextBox();
            this.Paid20Notes = new System.Windows.Forms.TextBox();
            this.Paid05Notes = new System.Windows.Forms.TextBox();
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
            this.NoteRecycler = new System.Windows.Forms.TabPage();
            this.btnSetFloat = new System.Windows.Forms.Button();
            this.tbFloatCurrency = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbFloatAmount = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tbMinPayout = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tbLevelInfo = new System.Windows.Forms.TextBox();
            this.btnPayout = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.tbPayoutCurrency = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tbPayoutAmount = new System.Windows.Forms.TextBox();
            this.btnPayoutByDenom = new System.Windows.Forms.Button();
            this.btnEmpty = new System.Windows.Forms.Button();
            this.chkHold = new System.Windows.Forms.CheckBox();
            this.logTickBox = new System.Windows.Forms.CheckBox();
            this.resetValidatorBtn = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnHalt = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.StorageListBoxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stackNextNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emptyStoredNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRun = new System.Windows.Forms.Button();
            this.NVStatus = new System.Windows.Forms.Button();
            this.OpenNV = new System.Windows.Forms.Button();
            this.Printer = new System.Windows.Forms.TabPage();
            this.PRINTERStatus = new System.Windows.Forms.Button();
            this.OpenPRINTER = new System.Windows.Forms.Button();
            this.PrintTest = new System.Windows.Forms.Button();
            this.BarcodeReader = new System.Windows.Forms.TabPage();
            this.BRStatus = new System.Windows.Forms.Button();
            this.OpenBR = new System.Windows.Forms.Button();
            this.Server_Connect = new System.Windows.Forms.Button();
            this.MainConfig = new System.Windows.Forms.TabControl();
            this.RFID = new System.Windows.Forms.TabPage();
            this.RFIDStatus = new System.Windows.Forms.Button();
            this.OpenRFID = new System.Windows.Forms.Button();
            this.UX300 = new System.Windows.Forms.TabPage();
            this.UX300Status = new System.Windows.Forms.Button();
            this.OpenUX300 = new System.Windows.Forms.Button();
            this.quitBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emptyNoteFloatToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lostTimer = new System.Windows.Forms.Timer(this.components);
            this.Settings.SuspendLayout();
            this.Coins.SuspendLayout();
            this.NoteRecycler.SuspendLayout();
            this.StorageListBoxMenu.SuspendLayout();
            this.Printer.SuspendLayout();
            this.BarcodeReader.SuspendLayout();
            this.MainConfig.SuspendLayout();
            this.RFID.SuspendLayout();
            this.UX300.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialEFT_POS
            // 
            this.serialEFT_POS.ReceivedBytesThreshold = 6;
            this.serialEFT_POS.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.ReceiveEFT_POS);
            // 
            // Debugging
            // 
            this.Debugging.BackColor = System.Drawing.SystemColors.ControlText;
            this.Debugging.ForeColor = System.Drawing.SystemColors.Window;
            this.Debugging.Location = new System.Drawing.Point(17, 335);
            this.Debugging.Margin = new System.Windows.Forms.Padding(4);
            this.Debugging.Name = "Debugging";
            this.Debugging.ReadOnly = true;
            this.Debugging.Size = new System.Drawing.Size(508, 197);
            this.Debugging.TabIndex = 1;
            this.Debugging.Text = "System INIT\n";
            // 
            // serialRFID
            // 
            this.serialRFID.PortName = "COM6";
            this.serialRFID.ReadTimeout = 200;
            this.serialRFID.ReceivedBytesThreshold = 8;
            this.serialRFID.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.ReceiveRFID);
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
            this.GeneralTimer.Interval = 1000;
            this.GeneralTimer.Tick += new System.EventHandler(this.GeneralTimer_Tick);
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
            this.Settings.Controls.Add(this.label22);
            this.Settings.Controls.Add(this.Avail10Notes);
            this.Settings.Controls.Add(this.ResetAllCoinsbtn);
            this.Settings.Controls.Add(this.Paid200Coins);
            this.Settings.Controls.Add(this.Paid100Coins);
            this.Settings.Controls.Add(this.Paid50Coins);
            this.Settings.Controls.Add(this.Paid20Coins);
            this.Settings.Controls.Add(this.Paid10Coins);
            this.Settings.Controls.Add(this.Paid05Coins);
            this.Settings.Controls.Add(this.label20);
            this.Settings.Controls.Add(this.label21);
            this.Settings.Controls.Add(this.label25);
            this.Settings.Controls.Add(this.label26);
            this.Settings.Controls.Add(this.label27);
            this.Settings.Controls.Add(this.label28);
            this.Settings.Controls.Add(this.label19);
            this.Settings.Controls.Add(this.ResetAllNotesbtn);
            this.Settings.Controls.Add(this.btnSmartEmpty);
            this.Settings.Controls.Add(this.CloseBatchButton);
            this.Settings.Controls.Add(this.label13);
            this.Settings.Controls.Add(this.label15);
            this.Settings.Controls.Add(this.label14);
            this.Settings.Controls.Add(this.PrintCashValues);
            this.Settings.Controls.Add(this.label12);
            this.Settings.Controls.Add(this.Paid50Notes);
            this.Settings.Controls.Add(this.Paid20Notes);
            this.Settings.Controls.Add(this.Paid05Notes);
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
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(210, 79);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(51, 13);
            this.label22.TabIndex = 63;
            this.label22.Text = "10 Euro";
            // 
            // Avail10Notes
            // 
            this.Avail10Notes.Location = new System.Drawing.Point(135, 76);
            this.Avail10Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Avail10Notes.Name = "Avail10Notes";
            this.Avail10Notes.ReadOnly = true;
            this.Avail10Notes.Size = new System.Drawing.Size(59, 19);
            this.Avail10Notes.TabIndex = 62;
            this.Avail10Notes.Text = "0";
            this.Avail10Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ResetAllCoinsbtn
            // 
            this.ResetAllCoinsbtn.Location = new System.Drawing.Point(618, 204);
            this.ResetAllCoinsbtn.Name = "ResetAllCoinsbtn";
            this.ResetAllCoinsbtn.Size = new System.Drawing.Size(118, 23);
            this.ResetAllCoinsbtn.TabIndex = 61;
            this.ResetAllCoinsbtn.Text = "Reset all";
            this.ResetAllCoinsbtn.UseVisualStyleBackColor = true;
            // 
            // Paid200Coins
            // 
            this.Paid200Coins.Location = new System.Drawing.Point(618, 176);
            this.Paid200Coins.Margin = new System.Windows.Forms.Padding(4);
            this.Paid200Coins.Name = "Paid200Coins";
            this.Paid200Coins.Size = new System.Drawing.Size(61, 19);
            this.Paid200Coins.TabIndex = 54;
            this.Paid200Coins.Text = "0";
            this.Paid200Coins.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid100Coins
            // 
            this.Paid100Coins.Location = new System.Drawing.Point(618, 151);
            this.Paid100Coins.Margin = new System.Windows.Forms.Padding(4);
            this.Paid100Coins.Name = "Paid100Coins";
            this.Paid100Coins.Size = new System.Drawing.Size(61, 19);
            this.Paid100Coins.TabIndex = 53;
            this.Paid100Coins.Text = "0";
            this.Paid100Coins.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid50Coins
            // 
            this.Paid50Coins.Location = new System.Drawing.Point(618, 127);
            this.Paid50Coins.Margin = new System.Windows.Forms.Padding(4);
            this.Paid50Coins.Name = "Paid50Coins";
            this.Paid50Coins.Size = new System.Drawing.Size(61, 19);
            this.Paid50Coins.TabIndex = 52;
            this.Paid50Coins.Text = "0";
            this.Paid50Coins.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid20Coins
            // 
            this.Paid20Coins.Location = new System.Drawing.Point(618, 103);
            this.Paid20Coins.Margin = new System.Windows.Forms.Padding(4);
            this.Paid20Coins.Name = "Paid20Coins";
            this.Paid20Coins.Size = new System.Drawing.Size(61, 19);
            this.Paid20Coins.TabIndex = 51;
            this.Paid20Coins.Text = "0";
            this.Paid20Coins.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid10Coins
            // 
            this.Paid10Coins.Location = new System.Drawing.Point(618, 79);
            this.Paid10Coins.Margin = new System.Windows.Forms.Padding(4);
            this.Paid10Coins.Name = "Paid10Coins";
            this.Paid10Coins.Size = new System.Drawing.Size(61, 19);
            this.Paid10Coins.TabIndex = 50;
            this.Paid10Coins.Text = "0";
            this.Paid10Coins.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid05Coins
            // 
            this.Paid05Coins.Location = new System.Drawing.Point(618, 57);
            this.Paid05Coins.Margin = new System.Windows.Forms.Padding(4);
            this.Paid05Coins.Name = "Paid05Coins";
            this.Paid05Coins.Size = new System.Drawing.Size(61, 19);
            this.Paid05Coins.TabIndex = 49;
            this.Paid05Coins.Text = "0";
            this.Paid05Coins.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(691, 179);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(43, 13);
            this.label20.TabIndex = 60;
            this.label20.Text = "2.00 €";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(691, 154);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(43, 13);
            this.label21.TabIndex = 59;
            this.label21.Text = "1.00 €";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(691, 130);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(43, 13);
            this.label25.TabIndex = 58;
            this.label25.Text = "0.50 €";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(691, 106);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(43, 13);
            this.label26.TabIndex = 57;
            this.label26.Text = "0.20 €";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(691, 82);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(43, 13);
            this.label27.TabIndex = 56;
            this.label27.Text = "0.10 €";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(691, 60);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(43, 13);
            this.label28.TabIndex = 55;
            this.label28.Text = "0.05 €";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label19.Location = new System.Drawing.Point(615, 29);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(131, 16);
            this.label19.TabIndex = 37;
            this.label19.Text = "      Paid Coins      ";
            // 
            // ResetAllNotesbtn
            // 
            this.ResetAllNotesbtn.Location = new System.Drawing.Point(288, 169);
            this.ResetAllNotesbtn.Name = "ResetAllNotesbtn";
            this.ResetAllNotesbtn.Size = new System.Drawing.Size(118, 23);
            this.ResetAllNotesbtn.TabIndex = 36;
            this.ResetAllNotesbtn.Text = "Reset all";
            this.ResetAllNotesbtn.UseVisualStyleBackColor = true;
            // 
            // btnSmartEmpty
            // 
            this.btnSmartEmpty.Location = new System.Drawing.Point(133, 96);
            this.btnSmartEmpty.Margin = new System.Windows.Forms.Padding(4);
            this.btnSmartEmpty.Name = "btnSmartEmpty";
            this.btnSmartEmpty.Size = new System.Drawing.Size(121, 58);
            this.btnSmartEmpty.TabIndex = 32;
            this.btnSmartEmpty.Text = "Withdrawal \r\nStorage";
            this.btnSmartEmpty.UseVisualStyleBackColor = true;
            this.btnSmartEmpty.Click += new System.EventHandler(this.btnSmartEmpty_Click);
            // 
            // CloseBatchButton
            // 
            this.CloseBatchButton.Location = new System.Drawing.Point(133, 169);
            this.CloseBatchButton.Margin = new System.Windows.Forms.Padding(4);
            this.CloseBatchButton.Name = "CloseBatchButton";
            this.CloseBatchButton.Size = new System.Drawing.Size(121, 58);
            this.CloseBatchButton.TabIndex = 31;
            this.CloseBatchButton.Text = "Close Batch";
            this.CloseBatchButton.UseVisualStyleBackColor = true;
            this.CloseBatchButton.Click += new System.EventHandler(this.CloseBatchButton_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label13.Location = new System.Drawing.Point(132, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(143, 16);
            this.label13.TabIndex = 28;
            this.label13.Text = "    Available Notes  ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label15.Location = new System.Drawing.Point(448, 29);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(161, 16);
            this.label15.TabIndex = 30;
            this.label15.Text = "     Available Coins      ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label14.Location = new System.Drawing.Point(285, 29);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(157, 16);
            this.label14.TabIndex = 29;
            this.label14.Text = "        Paid Notes          ";
            // 
            // PrintCashValues
            // 
            this.PrintCashValues.Enabled = false;
            this.PrintCashValues.Location = new System.Drawing.Point(4, 29);
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
            this.label12.Location = new System.Drawing.Point(364, 138);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "50 Euro";
            this.label12.Visible = false;
            // 
            // Paid50Notes
            // 
            this.Paid50Notes.Location = new System.Drawing.Point(288, 135);
            this.Paid50Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Paid50Notes.Name = "Paid50Notes";
            this.Paid50Notes.Size = new System.Drawing.Size(59, 19);
            this.Paid50Notes.TabIndex = 25;
            this.Paid50Notes.Text = "0";
            this.Paid50Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Paid50Notes.Visible = false;
            // 
            // Paid20Notes
            // 
            this.Paid20Notes.Location = new System.Drawing.Point(288, 108);
            this.Paid20Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Paid20Notes.Name = "Paid20Notes";
            this.Paid20Notes.Size = new System.Drawing.Size(59, 19);
            this.Paid20Notes.TabIndex = 23;
            this.Paid20Notes.Text = "0";
            this.Paid20Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid05Notes
            // 
            this.Paid05Notes.Location = new System.Drawing.Point(288, 51);
            this.Paid05Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Paid05Notes.Name = "Paid05Notes";
            this.Paid05Notes.Size = new System.Drawing.Size(59, 19);
            this.Paid05Notes.TabIndex = 21;
            this.Paid05Notes.Text = "0";
            this.Paid05Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Paid10Notes
            // 
            this.Paid10Notes.Location = new System.Drawing.Point(288, 81);
            this.Paid10Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Paid10Notes.Name = "Paid10Notes";
            this.Paid10Notes.Size = new System.Drawing.Size(59, 19);
            this.Paid10Notes.TabIndex = 16;
            this.Paid10Notes.Text = "0";
            this.Paid10Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Avail05Notes
            // 
            this.Avail05Notes.Location = new System.Drawing.Point(135, 54);
            this.Avail05Notes.Margin = new System.Windows.Forms.Padding(4);
            this.Avail05Notes.Name = "Avail05Notes";
            this.Avail05Notes.ReadOnly = true;
            this.Avail05Notes.Size = new System.Drawing.Size(59, 19);
            this.Avail05Notes.TabIndex = 15;
            this.Avail05Notes.Text = "0";
            this.Avail05Notes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Coin200
            // 
            this.Coin200.Location = new System.Drawing.Point(467, 189);
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
            this.Coin100.Location = new System.Drawing.Point(467, 162);
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
            this.Coin50.Location = new System.Drawing.Point(467, 135);
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
            this.Coin20.Location = new System.Drawing.Point(467, 108);
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
            this.Coin10.Location = new System.Drawing.Point(467, 81);
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
            this.Coin5.Location = new System.Drawing.Point(467, 54);
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
            this.label11.Location = new System.Drawing.Point(364, 111);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "20 Euro";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(364, 54);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "5 Euro";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(364, 84);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "10 Euro";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(210, 57);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "5 Euro";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(546, 192);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "2.00 €";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(546, 165);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "1.00 €";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(546, 138);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "0.50 €";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(546, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "0.20 €";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(546, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "0.10 €";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(546, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "0.05 €";
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(4, 169);
            this.Save.Margin = new System.Windows.Forms.Padding(4);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(121, 58);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Refresh
            // 
            this.Refresh.Location = new System.Drawing.Point(4, 96);
            this.Refresh.Margin = new System.Windows.Forms.Padding(4);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(121, 58);
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
            this.Coins.UseVisualStyleBackColor = true;
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
            // NoteRecycler
            // 
            this.NoteRecycler.BackColor = System.Drawing.Color.Silver;
            this.NoteRecycler.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.NoteRecycler.Controls.Add(this.btnSetFloat);
            this.NoteRecycler.Controls.Add(this.tbFloatCurrency);
            this.NoteRecycler.Controls.Add(this.label9);
            this.NoteRecycler.Controls.Add(this.label16);
            this.NoteRecycler.Controls.Add(this.tbFloatAmount);
            this.NoteRecycler.Controls.Add(this.label17);
            this.NoteRecycler.Controls.Add(this.tbMinPayout);
            this.NoteRecycler.Controls.Add(this.label18);
            this.NoteRecycler.Controls.Add(this.tbLevelInfo);
            this.NoteRecycler.Controls.Add(this.btnPayout);
            this.NoteRecycler.Controls.Add(this.label23);
            this.NoteRecycler.Controls.Add(this.tbPayoutCurrency);
            this.NoteRecycler.Controls.Add(this.label24);
            this.NoteRecycler.Controls.Add(this.tbPayoutAmount);
            this.NoteRecycler.Controls.Add(this.btnPayoutByDenom);
            this.NoteRecycler.Controls.Add(this.btnEmpty);
            this.NoteRecycler.Controls.Add(this.chkHold);
            this.NoteRecycler.Controls.Add(this.logTickBox);
            this.NoteRecycler.Controls.Add(this.resetValidatorBtn);
            this.NoteRecycler.Controls.Add(this.btnReturn);
            this.NoteRecycler.Controls.Add(this.btnHalt);
            this.NoteRecycler.Controls.Add(this.textBox1);
            this.NoteRecycler.Controls.Add(this.btnRun);
            this.NoteRecycler.Controls.Add(this.NVStatus);
            this.NoteRecycler.Controls.Add(this.OpenNV);
            this.NoteRecycler.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.NoteRecycler.Location = new System.Drawing.Point(4, 22);
            this.NoteRecycler.Margin = new System.Windows.Forms.Padding(4);
            this.NoteRecycler.Name = "NoteRecycler";
            this.NoteRecycler.Padding = new System.Windows.Forms.Padding(4);
            this.NoteRecycler.Size = new System.Drawing.Size(751, 239);
            this.NoteRecycler.TabIndex = 3;
            this.NoteRecycler.Text = "NoteRecycler";
            this.NoteRecycler.UseVisualStyleBackColor = true;
            // 
            // btnSetFloat
            // 
            this.btnSetFloat.Location = new System.Drawing.Point(133, 129);
            this.btnSetFloat.Name = "btnSetFloat";
            this.btnSetFloat.Size = new System.Drawing.Size(199, 23);
            this.btnSetFloat.TabIndex = 98;
            this.btnSetFloat.Text = "Set Float";
            this.btnSetFloat.UseVisualStyleBackColor = true;
            this.btnSetFloat.Click += new System.EventHandler(this.btnSetFloat_Click);
            // 
            // tbFloatCurrency
            // 
            this.tbFloatCurrency.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbFloatCurrency.Location = new System.Drawing.Point(236, 110);
            this.tbFloatCurrency.Name = "tbFloatCurrency";
            this.tbFloatCurrency.Size = new System.Drawing.Size(101, 19);
            this.tbFloatCurrency.TabIndex = 96;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(139, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 97;
            this.label9.Text = "Float Currency:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(139, 91);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 13);
            this.label16.TabIndex = 95;
            this.label16.Text = "Float Amount:";
            // 
            // tbFloatAmount
            // 
            this.tbFloatAmount.Location = new System.Drawing.Point(236, 89);
            this.tbFloatAmount.Name = "tbFloatAmount";
            this.tbFloatAmount.Size = new System.Drawing.Size(103, 19);
            this.tbFloatAmount.TabIndex = 94;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(139, 71);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(74, 13);
            this.label17.TabIndex = 93;
            this.label17.Text = "Min Payout:";
            // 
            // tbMinPayout
            // 
            this.tbMinPayout.Location = new System.Drawing.Point(235, 68);
            this.tbMinPayout.Name = "tbMinPayout";
            this.tbMinPayout.Size = new System.Drawing.Size(102, 19);
            this.tbMinPayout.TabIndex = 92;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(343, 4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 91;
            this.label18.Text = "Level Info:";
            // 
            // tbLevelInfo
            // 
            this.tbLevelInfo.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbLevelInfo.Location = new System.Drawing.Point(343, 21);
            this.tbLevelInfo.Multiline = true;
            this.tbLevelInfo.Name = "tbLevelInfo";
            this.tbLevelInfo.ReadOnly = true;
            this.tbLevelInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLevelInfo.Size = new System.Drawing.Size(167, 93);
            this.tbLevelInfo.TabIndex = 90;
            this.tbLevelInfo.TextChanged += new System.EventHandler(this.tbLevelInfo_TextChanged);
            // 
            // btnPayout
            // 
            this.btnPayout.Location = new System.Drawing.Point(133, 43);
            this.btnPayout.Name = "btnPayout";
            this.btnPayout.Size = new System.Drawing.Size(191, 23);
            this.btnPayout.TabIndex = 89;
            this.btnPayout.Text = "Payout";
            this.btnPayout.UseVisualStyleBackColor = true;
            this.btnPayout.Click += new System.EventHandler(this.btnPayout_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(233, 4);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(104, 13);
            this.label23.TabIndex = 88;
            this.label23.Text = "Payout Currency:";
            // 
            // tbPayoutCurrency
            // 
            this.tbPayoutCurrency.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbPayoutCurrency.Location = new System.Drawing.Point(236, 21);
            this.tbPayoutCurrency.Name = "tbPayoutCurrency";
            this.tbPayoutCurrency.Size = new System.Drawing.Size(86, 19);
            this.tbPayoutCurrency.TabIndex = 87;
            this.tbPayoutCurrency.Text = "EUR";
            this.tbPayoutCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(130, 4);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(96, 13);
            this.label24.TabIndex = 86;
            this.label24.Text = "Payout Amount:";
            // 
            // tbPayoutAmount
            // 
            this.tbPayoutAmount.Location = new System.Drawing.Point(133, 21);
            this.tbPayoutAmount.Name = "tbPayoutAmount";
            this.tbPayoutAmount.Size = new System.Drawing.Size(80, 19);
            this.tbPayoutAmount.TabIndex = 85;
            this.tbPayoutAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnPayoutByDenom
            // 
            this.btnPayoutByDenom.Location = new System.Drawing.Point(133, 157);
            this.btnPayoutByDenom.Name = "btnPayoutByDenom";
            this.btnPayoutByDenom.Size = new System.Drawing.Size(199, 23);
            this.btnPayoutByDenom.TabIndex = 84;
            this.btnPayoutByDenom.Text = "Payout by Denomination";
            this.btnPayoutByDenom.UseVisualStyleBackColor = true;
            this.btnPayoutByDenom.Click += new System.EventHandler(this.btnPayoutByDenom_Click);
            // 
            // btnEmpty
            // 
            this.btnEmpty.Location = new System.Drawing.Point(133, 181);
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.Size = new System.Drawing.Size(199, 23);
            this.btnEmpty.TabIndex = 83;
            this.btnEmpty.Text = "Empty to Cashbox";
            this.btnEmpty.UseVisualStyleBackColor = true;
            this.btnEmpty.Click += new System.EventHandler(this.btnEmpty_Click);
            // 
            // chkHold
            // 
            this.chkHold.AutoSize = true;
            this.chkHold.Location = new System.Drawing.Point(14, 217);
            this.chkHold.Name = "chkHold";
            this.chkHold.Size = new System.Drawing.Size(111, 17);
            this.chkHold.TabIndex = 82;
            this.chkHold.Text = "Hold in Escrow";
            this.chkHold.UseVisualStyleBackColor = true;
            this.chkHold.CheckedChanged += new System.EventHandler(this.chkHold_CheckedChanged);
            // 
            // logTickBox
            // 
            this.logTickBox.AutoSize = true;
            this.logTickBox.Checked = true;
            this.logTickBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logTickBox.Location = new System.Drawing.Point(25, 194);
            this.logTickBox.Name = "logTickBox";
            this.logTickBox.Size = new System.Drawing.Size(90, 17);
            this.logTickBox.TabIndex = 81;
            this.logTickBox.Text = "Comms Log";
            this.logTickBox.UseVisualStyleBackColor = true;
            this.logTickBox.CheckedChanged += new System.EventHandler(this.logTickBox_CheckedChanged);
            // 
            // resetValidatorBtn
            // 
            this.resetValidatorBtn.Location = new System.Drawing.Point(133, 210);
            this.resetValidatorBtn.Name = "resetValidatorBtn";
            this.resetValidatorBtn.Size = new System.Drawing.Size(199, 23);
            this.resetValidatorBtn.TabIndex = 80;
            this.resetValidatorBtn.Text = "R&eset Validator";
            this.resetValidatorBtn.UseVisualStyleBackColor = true;
            this.resetValidatorBtn.Click += new System.EventHandler(this.resetValidatorBtn_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(7, 7);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(117, 32);
            this.btnReturn.TabIndex = 79;
            this.btnReturn.Text = "Return Note";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnHalt
            // 
            this.btnHalt.Location = new System.Drawing.Point(8, 156);
            this.btnHalt.Name = "btnHalt";
            this.btnHalt.Size = new System.Drawing.Size(117, 32);
            this.btnHalt.TabIndex = 78;
            this.btnHalt.Text = "&Halt";
            this.btnHalt.UseVisualStyleBackColor = true;
            this.btnHalt.Click += new System.EventHandler(this.btnHalt_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox1.ContextMenuStrip = this.StorageListBoxMenu;
            this.textBox1.Location = new System.Drawing.Point(343, 120);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(167, 113);
            this.textBox1.TabIndex = 77;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // StorageListBoxMenu
            // 
            this.StorageListBoxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.stackNextNoteToolStripMenuItem,
            this.emptyStoredNotesToolStripMenuItem});
            this.StorageListBoxMenu.Name = "contextMenuStrip1";
            this.StorageListBoxMenu.Size = new System.Drawing.Size(180, 70);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.testToolStripMenuItem.Text = "Payout Next Note";
            // 
            // stackNextNoteToolStripMenuItem
            // 
            this.stackNextNoteToolStripMenuItem.Name = "stackNextNoteToolStripMenuItem";
            this.stackNextNoteToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.stackNextNoteToolStripMenuItem.Text = "Stack Next Note";
            // 
            // emptyStoredNotesToolStripMenuItem
            // 
            this.emptyStoredNotesToolStripMenuItem.Name = "emptyStoredNotesToolStripMenuItem";
            this.emptyStoredNotesToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.emptyStoredNotesToolStripMenuItem.Text = "Empty Stored Notes";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(8, 118);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(117, 32);
            this.btnRun.TabIndex = 76;
            this.btnRun.Text = "&Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // NVStatus
            // 
            this.NVStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.NVStatus.Location = new System.Drawing.Point(7, 80);
            this.NVStatus.Margin = new System.Windows.Forms.Padding(4);
            this.NVStatus.Name = "NVStatus";
            this.NVStatus.Size = new System.Drawing.Size(117, 32);
            this.NVStatus.TabIndex = 75;
            this.NVStatus.Text = "Status";
            this.NVStatus.UseVisualStyleBackColor = true;
            this.NVStatus.Click += new System.EventHandler(this.NVStatus_Click);
            // 
            // OpenNV
            // 
            this.OpenNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.OpenNV.Location = new System.Drawing.Point(8, 42);
            this.OpenNV.Margin = new System.Windows.Forms.Padding(4);
            this.OpenNV.Name = "OpenNV";
            this.OpenNV.Size = new System.Drawing.Size(117, 32);
            this.OpenNV.TabIndex = 74;
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
            this.Printer.UseVisualStyleBackColor = true;
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
            // BarcodeReader
            // 
            this.BarcodeReader.BackColor = System.Drawing.Color.Silver;
            this.BarcodeReader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BarcodeReader.Controls.Add(this.BRStatus);
            this.BarcodeReader.Controls.Add(this.OpenBR);
            this.BarcodeReader.Controls.Add(this.Server_Connect);
            this.BarcodeReader.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.BarcodeReader.Location = new System.Drawing.Point(4, 22);
            this.BarcodeReader.Margin = new System.Windows.Forms.Padding(4);
            this.BarcodeReader.Name = "BarcodeReader";
            this.BarcodeReader.Padding = new System.Windows.Forms.Padding(4);
            this.BarcodeReader.Size = new System.Drawing.Size(751, 239);
            this.BarcodeReader.TabIndex = 0;
            this.BarcodeReader.Text = "BarcodeReader";
            this.BarcodeReader.UseVisualStyleBackColor = true;
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
            // Server_Connect
            // 
            this.Server_Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Server_Connect.Location = new System.Drawing.Point(370, 80);
            this.Server_Connect.Margin = new System.Windows.Forms.Padding(4);
            this.Server_Connect.Name = "Server_Connect";
            this.Server_Connect.Size = new System.Drawing.Size(137, 55);
            this.Server_Connect.TabIndex = 5;
            this.Server_Connect.Text = "Server Connect";
            this.Server_Connect.UseVisualStyleBackColor = true;
            this.Server_Connect.Click += new System.EventHandler(this.TCP_Connect_Click);
            // 
            // MainConfig
            // 
            this.MainConfig.Controls.Add(this.BarcodeReader);
            this.MainConfig.Controls.Add(this.RFID);
            this.MainConfig.Controls.Add(this.UX300);
            this.MainConfig.Controls.Add(this.Printer);
            this.MainConfig.Controls.Add(this.NoteRecycler);
            this.MainConfig.Controls.Add(this.Coins);
            this.MainConfig.Controls.Add(this.Settings);
            this.MainConfig.Enabled = false;
            this.MainConfig.HotTrack = true;
            this.MainConfig.Location = new System.Drawing.Point(13, 62);
            this.MainConfig.Margin = new System.Windows.Forms.Padding(4);
            this.MainConfig.Name = "MainConfig";
            this.MainConfig.SelectedIndex = 0;
            this.MainConfig.Size = new System.Drawing.Size(759, 265);
            this.MainConfig.TabIndex = 0;
            // 
            // RFID
            // 
            this.RFID.Controls.Add(this.RFIDStatus);
            this.RFID.Controls.Add(this.OpenRFID);
            this.RFID.Location = new System.Drawing.Point(4, 22);
            this.RFID.Name = "RFID";
            this.RFID.Size = new System.Drawing.Size(751, 239);
            this.RFID.TabIndex = 8;
            this.RFID.Text = "RFID";
            this.RFID.UseVisualStyleBackColor = true;
            // 
            // RFIDStatus
            // 
            this.RFIDStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.RFIDStatus.Location = new System.Drawing.Point(25, 132);
            this.RFIDStatus.Margin = new System.Windows.Forms.Padding(4);
            this.RFIDStatus.Name = "RFIDStatus";
            this.RFIDStatus.Size = new System.Drawing.Size(117, 60);
            this.RFIDStatus.TabIndex = 6;
            this.RFIDStatus.Text = "Status";
            this.RFIDStatus.UseVisualStyleBackColor = true;
            this.RFIDStatus.Click += new System.EventHandler(this.RFIDStatus_Click_1);
            // 
            // OpenRFID
            // 
            this.OpenRFID.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.OpenRFID.Location = new System.Drawing.Point(25, 46);
            this.OpenRFID.Margin = new System.Windows.Forms.Padding(4);
            this.OpenRFID.Name = "OpenRFID";
            this.OpenRFID.Size = new System.Drawing.Size(117, 59);
            this.OpenRFID.TabIndex = 1;
            this.OpenRFID.Text = "Open";
            this.OpenRFID.UseVisualStyleBackColor = true;
            this.OpenRFID.Click += new System.EventHandler(this.OpenRFID_Click_1);
            // 
            // UX300
            // 
            this.UX300.Controls.Add(this.UX300Status);
            this.UX300.Controls.Add(this.OpenUX300);
            this.UX300.Location = new System.Drawing.Point(4, 22);
            this.UX300.Name = "UX300";
            this.UX300.Size = new System.Drawing.Size(751, 239);
            this.UX300.TabIndex = 7;
            this.UX300.Text = "UX300";
            this.UX300.UseVisualStyleBackColor = true;
            // 
            // UX300Status
            // 
            this.UX300Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.UX300Status.Location = new System.Drawing.Point(25, 132);
            this.UX300Status.Margin = new System.Windows.Forms.Padding(4);
            this.UX300Status.Name = "UX300Status";
            this.UX300Status.Size = new System.Drawing.Size(117, 60);
            this.UX300Status.TabIndex = 6;
            this.UX300Status.Text = "Status";
            this.UX300Status.UseVisualStyleBackColor = true;
            this.UX300Status.Click += new System.EventHandler(this.UX300Status_Click);
            // 
            // OpenUX300
            // 
            this.OpenUX300.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.OpenUX300.Location = new System.Drawing.Point(25, 46);
            this.OpenUX300.Margin = new System.Windows.Forms.Padding(4);
            this.OpenUX300.Name = "OpenUX300";
            this.OpenUX300.Size = new System.Drawing.Size(117, 59);
            this.OpenUX300.TabIndex = 1;
            this.OpenUX300.Text = "Open";
            this.OpenUX300.UseVisualStyleBackColor = true;
            this.OpenUX300.Click += new System.EventHandler(this.OpenUX300_Click);
            // 
            // quitBtn
            // 
            this.quitBtn.BackColor = System.Drawing.Color.OrangeRed;
            this.quitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.quitBtn.Location = new System.Drawing.Point(676, 25);
            this.quitBtn.Margin = new System.Windows.Forms.Padding(4);
            this.quitBtn.Name = "quitBtn";
            this.quitBtn.Size = new System.Drawing.Size(80, 43);
            this.quitBtn.TabIndex = 8;
            this.quitBtn.Text = "Quit ";
            this.quitBtn.UseVisualStyleBackColor = false;
            this.quitBtn.Click += new System.EventHandler(this.quitBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyNoteFloatToolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // emptyNoteFloatToolStripMenuItem1
            // 
            this.emptyNoteFloatToolStripMenuItem1.Name = "emptyNoteFloatToolStripMenuItem1";
            this.emptyNoteFloatToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.emptyNoteFloatToolStripMenuItem1.Text = "Empty Note Float";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // lostTimer
            // 
            this.lostTimer.Interval = 20000;
            this.lostTimer.Tick += new System.EventHandler(this.lostTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(784, 568);
            this.ContextMenuStrip = this.StorageListBoxMenu;
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.quitBtn);
            this.Controls.Add(this.Init_System);
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
            this.NoteRecycler.ResumeLayout(false);
            this.NoteRecycler.PerformLayout();
            this.StorageListBoxMenu.ResumeLayout(false);
            this.Printer.ResumeLayout(false);
            this.BarcodeReader.ResumeLayout(false);
            this.MainConfig.ResumeLayout(false);
            this.RFID.ResumeLayout(false);
            this.UX300.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialEFT_POS;
        private System.Windows.Forms.RichTextBox Debugging;
        private System.IO.Ports.SerialPort serialRFID;
        private System.IO.Ports.SerialPort serialBR;
        private System.IO.Ports.SerialPort serialPRINTER;
        private System.Windows.Forms.Button StartApplication;
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
        private System.Windows.Forms.TabPage NoteRecycler;
        private System.Windows.Forms.TabPage Printer;
        private System.Windows.Forms.Button PRINTERStatus;
        private System.Windows.Forms.Button OpenPRINTER;
        private System.Windows.Forms.Button PrintTest;
        private System.Windows.Forms.TabPage BarcodeReader;
        private System.Windows.Forms.Button BRStatus;
        private System.Windows.Forms.Button OpenBR;
        private System.Windows.Forms.Button Server_Connect;
        private System.Windows.Forms.TabControl MainConfig;
        private System.Windows.Forms.Button quitBtn;
        private System.Windows.Forms.Timer LanguageTimer;
        private System.Windows.Forms.TabPage UX300;
        private System.Windows.Forms.Button UX300Status;
        private System.Windows.Forms.Button OpenUX300;
        private System.Windows.Forms.Timer GeneralTimer;
        private System.Windows.Forms.Button CloseBatchButton;
        private System.Windows.Forms.Button btnSmartEmpty;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emptyNoteFloatToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip StorageListBoxMenu;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stackNextNoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emptyStoredNotesToolStripMenuItem;
        private System.Windows.Forms.Timer lostTimer;
        private System.Windows.Forms.TabPage RFID;
        private System.Windows.Forms.Button RFIDStatus;
        private System.Windows.Forms.Button OpenRFID;
        private System.Windows.Forms.Button ResetAllCoinsbtn;
        private System.Windows.Forms.TextBox Paid200Coins;
        private System.Windows.Forms.TextBox Paid100Coins;
        private System.Windows.Forms.TextBox Paid50Coins;
        private System.Windows.Forms.TextBox Paid20Coins;
        private System.Windows.Forms.TextBox Paid10Coins;
        private System.Windows.Forms.TextBox Paid05Coins;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button ResetAllNotesbtn;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox Avail10Notes;
        private System.Windows.Forms.Button btnSetFloat;
        private System.Windows.Forms.TextBox tbFloatCurrency;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbFloatAmount;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbMinPayout;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tbLevelInfo;
        private System.Windows.Forms.Button btnPayout;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbPayoutCurrency;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tbPayoutAmount;
        private System.Windows.Forms.Button btnPayoutByDenom;
        private System.Windows.Forms.Button btnEmpty;
        private System.Windows.Forms.CheckBox chkHold;
        private System.Windows.Forms.CheckBox logTickBox;
        private System.Windows.Forms.Button resetValidatorBtn;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnHalt;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button NVStatus;
        private System.Windows.Forms.Button OpenNV;
    }
}

