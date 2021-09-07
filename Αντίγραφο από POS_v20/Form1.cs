using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

using Ini;

//using PC;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;//USED for Dll 
using NRI.PaymenentManager;
using System.Globalization;

//Printer
using System.Drawing.Printing;



namespace POS_v20
{
    public partial class Form1 : Form
    {
        static Form1 instance = null;
        string IniFilePath = "C:/POS/POS.ini"; 
        IniFile ini;

        string NV_Data = "";
        string BC_Data = "";
        string RF_Card = "";
        string BR_Card = "";

        int SM = 1;
        
        Form2 secondForm = new Form2();
        public string UserCode;
        public string UserCoupon;
        int GeneralCounter = 120;

        //int NDres = 0;
        int DNote = 0;
        int Total_Coins = 0;
        int Total_Notes = 0;
        int RFID_Lenght = 0;
        int BC_Lenght = 0;
        int CPN_Length = 0;

        Boolean UserHasCpn = false;

        //TCP
        IAsyncResult m_result;
        public Socket m_clientSocket;
        public AsyncCallback m_pfnCallBack;
        string TCP_Data = "";
        int Payment = 0;
        int ReturnMoney = 0;
        int InitalCost = 0;
        string FileName = "C:/POS/log/POS_Log.txt";

        string type = "";
        string state = "";
        string value = "";
        string tstart = "";
        string tend = "";
        string plate = "";
        string ticket = "";
        string receipt = "";
        string freemin = "";
        bool p = false;

        static System.IO.StreamWriter WRfile;

        // PaymentManager
        #region PaymentManager handling

        protected PaymentManagerWrapper pm = new PaymentManagerWrapper();
        protected int wndHandle;
        protected const int WM_USER = 0x0400;
        protected const int WM_PAYMENTMESSAGE = WM_USER + 0x21;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAYMENTMESSAGE)
                processPMMessage(m.WParam.ToInt32(), m.LParam.ToInt32());
            base.WndProc(ref m);
        }

        protected void processPMMessage(int wParam, int lParam)
        {
            switch (wParam){
                case 0x00:										// Coin accepted
                    Display("Coins Ready " + "W" + wParam.ToString() + "L" + lParam.ToString()+"\n");
                    break;
                case 0x01:
                    Display("Coins Unknown" + "W" + wParam.ToString() + "L" + lParam.ToString()+"\n");
                    if (lParam == 4)
                        Display("NO MONEY LEFT\n");
                        Display("Tubes EMPTY:" + "W" + wParam.ToString() + "L" + lParam.ToString()+"\n");
                    break;
                case 0x11://W17										// Coin accepted
                    Display("Coins Accepted" + "W" + wParam.ToString() + "L " + lParam.ToString()+"\n");
                    switch (lParam){
                        case 5:
                            Payment = Payment + 5;
                            GeneralCounter = 120;
                            break;
                        case 10:
                            Payment = Payment + 10;
                            GeneralCounter = 120;
                            break;
                        case 20:
                            Payment = Payment + 20;
                            GeneralCounter = 120;
                            break;
                        case 50:
                            Payment = Payment + 50;
                            GeneralCounter = 120;
                            break;
                        case 100:
                            Payment = Payment + 100;
                            GeneralCounter = 120;
                            break;
                        case 200:
                            Payment = Payment + 200;
                            GeneralCounter = 120;
                            break;
                    }
                    break;
                case 33:
                    Display("Coins Manual Eject:" + "W" + wParam.ToString() + "L" + lParam.ToString()+"\n");
                    break;
                case 0x31:										// Reject button pressed
                    Display("Coins Reject Button Pressed\n");//btnReject_Click(this, new EventArgs());
                    break;
                // ... Other events ...

                case 65:
                    //Display("\nW" + wParam.ToString() + "L" + lParam.ToString());
                    break;
                default:
                    Display("Coins Unknown" + "W" + wParam.ToString() + "L" + lParam.ToString()+ "\n");
                    break;
            }
            switch (lParam)
            {
                case 1: Display("Coins Device Ready\n");
                    this.Invoke((MethodInvoker)delegate
                    {
                        CoinStatus.Enabled = false;
                        CoinStatus.BackColor = Color.YellowGreen;
                        this.Coins.Text = "Coins_OK";
                        int tab = this.MainConfig.SelectedIndex;
                        this.MainConfig.SelectedIndex = tab + 1;
                        this.MainConfig.TabPages[6].Parent.Focus();                        
                    });
                    break;
                case 8: Display("No Configuration file\n");
                    break;
            }
        }

        #endregion

        public Form1()
        {
            InitializeComponent();
            this.Text = "POS Configuration";
            if (instance == null)
            {
                instance = this;
            }
            FileName = "C:/POS/log/" + DateTime.Now.ToString("ddMMyy") + "_POS_Log.txt";
            WRfile = new System.IO.StreamWriter(FileName, true, System.Text.Encoding.UTF8, 100);
            WRfile.AutoFlush = true;
            
            //Open Config File
            if (System.IO.File.Exists(IniFilePath) == true){
                ini = new IniFile(IniFilePath);
            }else{
                MessageBox.Show("Ini NOT Found\nFix Error and restart application");
                return;
            }            

            //Buttons Status
            NDStatus.Enabled = false;
            RFIDStatus.Enabled = false;
            BRStatus.Enabled = false;
            PRINTERStatus.Enabled = false;
            NVStatus.Enabled = false;
            CoinStatus.Enabled = false;

            secondForm.pictureBox05.Visible = false;            
            secondForm.pictureBox10.Visible = false;
            secondForm.pictureBox20.Visible = false;
            secondForm.btnYes.Visible = false;
            secondForm.btnNo.Visible = false;
            
            secondForm.Messages2.Visible = false;

            secondForm.Vprogress.Maximum = 120;

            BC_Lenght   = Convert.ToInt16(ini.IniReadValue("Params", "BC_Length"));
            CPN_Length = Convert.ToInt16(ini.IniReadValue("Params", "CPN_Length"));
            RFID_Lenght = Convert.ToInt16(ini.IniReadValue("Params", "RFID_Lenght"));

            this.WindowState = FormWindowState.Maximized;

        }

        /*~Form1()
        {
            DW.Abort();
        }*/
/**************************************************************************************/
/**************************************************************************************/
// TEST 2
    private void test2_Click(object sender, EventArgs e)
    {

        /*secondForm.Show();

        secondForm.Text = "PAY MACHINE";
        secondForm.Refresh();
        //secondForm.WindowState = FormWindowState.Maximized;
        //secondForm.FormBorderStyle = FormBorderStyle.None;
        Thread.Sleep(1);
        LanguageTimer.Interval = 1000;
        LanguageTimer.Start();
        GeneralTimer.Interval = 500;//STATE MACHINE
        GeneralTimer.Start();
        StartApplication.Enabled = false;
        secondForm.Messages2.Visible = false;*/
        
        //value= "4500";
        //tstart = "28-04-2014 14:22:40";
        //tend = "28-04-2014 14:22:40";
        //plate = "ZMX1978";
        //receipt = "3";
        //ticket = "007563";
        //InitalCost = 4500;
        //Payment = 500;
        //ReturnMoney = 0;
        
        //PrintReceipt_OK("@" + value + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@");
        //Thread.Sleep(2000);
        //PrintReceipt_INCOMPLETE("@" + value + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@");
        //SM = 6;
        RF_Card = "8172635445362718";
    }

//Test 3
    private void button1_Click_1(object sender, EventArgs e)
    {
        Payment = Payment + 1200;
    }
/**************************************************************************************/
/**************************************************************************************/
//TCP Handling    
    #region TCP handling
    private void TCP_Connect_Click(object sender, EventArgs e)
    {

        string IpAddress = ini.IniReadValue("Params", "ServerIP");
        string IpPort = ini.IniReadValue("Params", "ServerPort");

        // Create the socket instance
        m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Set the remote IP address
        IPAddress ip = IPAddress.Parse(IpAddress);
        int iPortNo = System.Convert.ToInt16(IpPort);
        // Create the end point 
        IPEndPoint ipEnd = new IPEndPoint(ip, iPortNo);

        // Connect to the remote host
        try
        {
            m_clientSocket.Connect(ipEnd);
            if (m_clientSocket.Connected)
            {
                WaitForData();
                this.TCP_Connect.Enabled = false;
            }
        }
        catch
        {
            MessageBox.Show("Could not connect to server\n Check TCP settings");
        }
    }
    /************************/
    public void WaitForData()
    {
        try
        {
            if (m_pfnCallBack == null)
                m_pfnCallBack = new AsyncCallback(OnDataReceived);

            SocketPacket theSocPkt = new SocketPacket();
            theSocPkt.thisSocket = m_clientSocket;
            // Start listening to the data asynchronously
            m_result = m_clientSocket.BeginReceive(theSocPkt.dataBuffer,
                                                    0, theSocPkt.dataBuffer.Length,
                                                    SocketFlags.None,
                                                    m_pfnCallBack,
                                                    theSocPkt);
            TCP_Connect.BackColor = Color.YellowGreen;
        }
        catch (SocketException se)
        {
            Display("Exception Wait For Data" + se.Message+"\n");
        }
    }
    /************************/
    public class SocketPacket
    {
        public System.Net.Sockets.Socket thisSocket;
        public byte[] dataBuffer = new byte[100];
    }
    /************************/
    public void OnDataReceived(IAsyncResult asyn)
    {
        //byte[] byData;

        try
        {
            SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
            int iRx = theSockId.thisSocket.EndReceive(asyn);
            char[] chars = new char[iRx + 1];
            System.Text.Decoder d = System.Text.Encoding.Default.GetDecoder();

            int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
            System.String szData = new System.String(chars);
            Display("TCP_RCV:" + szData+"\n");//richTextRxMessage.Text = richTextRxMessage.Text + szData;

            if (szData.IndexOf("#TIC") != -1)
            {
                TCP_Data = szData.TrimEnd('\0');
            }
            if (szData.IndexOf("#CARD") != -1)
            {
                TCP_Data = szData.TrimEnd('\0');
            }
            if (szData.IndexOf("#RCP") != -1)
            {
                TCP_Data = szData.TrimEnd('\0');
            }
            if (szData.IndexOf("#CPN") != -1)
            {
                TCP_Data = szData.TrimEnd('\0');
            }            
            if (szData.IndexOf("\0") == 0)
            {
                TCP_Send("ERROR: Problem with connection");
                m_clientSocket.Close();
                SM = 11; 
                Display("Problem with connection\n");
                return;
            }
            //byData = System.Text.Encoding.ASCII.GetBytes(szData);
            //m_clientSocket.Send(byData);
            WaitForData();
        }
        catch (ObjectDisposedException)
        {
            System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
        }
        catch (SocketException se)
        {
            Display("Exception On Data Received "+se.Message+"\n");
            TCP_Connect.BackColor = Color.Tomato;
        }
    }
    #endregion

/**************************************************************************************/
// TCP SEND FUNCTION
    public int TCP_Send(string send)
    {

        byte[] byData;
        byData = System.Text.Encoding.ASCII.GetBytes(send);

        if (send.Length == 0)
        {
            Display("Empty TCP data to send\n");
            return 0;
        }
        else if (!m_clientSocket.Connected)
        {
            Display("No Connection with server\n");
            SM = 11;
            return 0;
        }
        else
        {
            int count = m_clientSocket.Send(byData);
            Display("TCP_SEND:" + send+"\n"); Thread.Sleep(300);
            return count;
        }

    }

/**************************************************************************************/
/**************************************************************************************/
//OPEN BARCODE PORT

    private void OpenBR_Click(object sender, EventArgs e)
    {
        string Port = ini.IniReadValue("SerialBR", "COM");

        Display("LOAD SETTINGS SerialBR COM Port: " + Port + "\n");
        serialBR.PortName = Port;
        serialBR.BaudRate = 9600;
        serialBR.DataBits = 8;
        serialBR.Parity = System.IO.Ports.Parity.None;
        serialBR.StopBits = System.IO.Ports.StopBits.One;
        serialBR.Handshake = Handshake.None;
        serialBR.ReadTimeout = 300;

        try
        {
            serialBR.Open();
            OpenBR.BackColor = Color.GreenYellow;
            BRStatus.Enabled = true;
            OpenBR.Enabled = false;
            this.Refresh();
        }
        catch
        {
            MessageBox.Show("SerialBR Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
            Display("SerialBR Error \n\nProblem While Opening Serial Port: " + Port + " \n");
            OpenBR.BackColor = Color.Tomato;
            this.Refresh();
        }
        serialBR.Close();
    }
/**************************************************************************************/
//BARCODE STATUS

    private void BRStatus_Click(object sender, EventArgs e)
    {
        byte[] health = { 0x68 };
        byte[] reset = { 0x52 };

        if (!serialBR.IsOpen)
            serialBR.Open();

        else
            Thread.Sleep(100);
        try
        {
            serialBR.Write(reset, 0, reset.Length);
            Display("Wait BRStatus\n");
            Thread.Sleep(500);            
            Thread.Sleep(1000);//1000 //500
            serialBR.Write(health, 0, health.Length);
        }
        catch
        {
            MessageBox.Show("SerialBR Error \n\nProblem Writing Status \nCheck Ini Settings!");
            Display("SerialBR Error: Problem Writing Status\n");            
        }
    }
/**************************************************************************************/
//RECEIVE HANDLER BARCODE READER

    private void ReceiveBR(object sender, SerialDataReceivedEventArgs e)
    {
        byte[] buffer = new byte[21];
        string data = "";
        try
        {
            int size = serialBR.Read(buffer, 0, 20);
            data = System.Text.Encoding.UTF8.GetString(buffer);
            data = data.Trim('\0', '\n', ' ');
            if (data.IndexOf('*') != -1) BC_Data = ""; //|| BC_Data.Length >= 7
            BC_Data = BC_Data + data;               
            Display("BC_Data SET: " + BC_Data + "\n");            
        }
        catch
        {
            data = "---";
            Display("Barcode DATA ERROR: --- \n");
        }

        if (BC_Data.IndexOf("OK") > 0)
        {
            BC_Data = "";
            this.Invoke((MethodInvoker)delegate//used to call this function from other function 
            {
                BRStatus.Enabled = false;
                BRStatus.BackColor = Color.YellowGreen;
                Display("Barcode Reader READY!\n");
                this.BarcodeReader.Text = "BarcodeReader_OK";
                this.Refresh();
                int tab = this.MainConfig.SelectedIndex;
                this.MainConfig.SelectedIndex = tab + 1;
                this.MainConfig.TabPages[6].Parent.Focus();
                this.Refresh();                
            });
        }

        if (BC_Data.IndexOf('\r') != -1)//BC_Data.Length >= 7
        {
            BC_Data = BC_Data.Trim('*');
            data = BC_Data;
            data = data.Trim('\r', '\0', '\n', ' ');
            this.Invoke((MethodInvoker)delegate
            { Display("BarCode_" + data.Length.ToString() + ": " + data + "\n"); });
            if (BR_Card == "") { BR_Card = data; }//.Substring(0, 6); }
            //while (BR_Card.Length < 8)
            //{
            //   BR_Card = "0" + BR_Card;
            //}    
            data = "";
        }

        if (data.IndexOf('\r') != -1) BC_Data = "";
        data = "";
        //Thread.Sleep(10);
    }

/**************************************************************************************/
/**************************************************************************************/
//OPEN RFID PORT

    private void OpenRFID_Click(object sender, EventArgs e)
    {
        string Port = ini.IniReadValue("SerialRFID", "COM");

        Display("LOAD SETTINGS SerialRFID COM Port: " + Port + "\n");
        
        serialPortRFID.PortName = Port;
        serialPortRFID.BaudRate = 9600;
        serialPortRFID.DataBits = 8;
        serialPortRFID.Parity = System.IO.Ports.Parity.None;
        serialPortRFID.StopBits = System.IO.Ports.StopBits.One;
        serialPortRFID.Handshake = Handshake.None;

        try
        {
            serialPortRFID.Open();
            OpenRFID.BackColor = Color.GreenYellow;
            OpenRFID.Enabled = false;
            RFIDStatus.Enabled = true;
            this.Refresh();
        }
        catch
        {
            MessageBox.Show("SerialRFID Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
            Display("SerialRFID Error \n\nProblem While Opening Serial Port: " + Port + " \n");
            OpenRFID.BackColor = Color.Tomato;
            this.Refresh();
        }
        serialPortRFID.Close();
    }

/**************************************************************************************/   
//RFID STATUS

    private void RFIDStatus_Click(object sender, EventArgs e)
    {
        byte[] reset = { 0x78 };//'x'

        if (!serialPortRFID.IsOpen)
            serialPortRFID.Open();
        else
            Thread.Sleep(1);

        Thread.Sleep(100);
        serialPortRFID.Write(reset, 0, reset.Length);
        Thread.Sleep(1000);
        serialPortRFID.Write(reset, 0, reset.Length);
    }

/**************************************************************************************/
//RECEIVE HANDLER FOR RFID

    private void ReceiveRFID(object sender, SerialDataReceivedEventArgs e)
    {
        string data = serialPortRFID.ReadLine();
        data = data.TrimEnd('\r');
        data = data.TrimEnd('\n');
        Display("Got RFID data: " + data +"\n");

        switch (data)
        {
            case "S":
                this.Invoke((MethodInvoker)delegate
                {
                    Display("RFID Automatic reading NOT enabled\n");
                    this.RFIDStatus.BackColor = Color.Tomato;
                    this.Refresh();
                });
                break;
            case "MultiISO 1.0":
                this.Invoke((MethodInvoker)delegate
                {
                    Display("RFID MultiISO 1.0 READY\n");
                    RFIDStatus.BackColor = Color.YellowGreen;
                    Thread.Sleep(1000);
                    int tab = this.MainConfig.SelectedIndex;
                    this.RFID.Text = "RFID_OK";
                    this.MainConfig.SelectedIndex = tab + 1;
                    this.MainConfig.TabPages[6].Parent.Focus();
                    RFIDStatus.Enabled = false;
                    this.Refresh();
                });
                break;
            case "MultiISO 1.2.5":
                this.Invoke((MethodInvoker)delegate
                {
                    Display("RFID MultiISO 1.2.5 READY\n");
                    RFIDStatus.BackColor = Color.YellowGreen;
                    Thread.Sleep(1000);
                    int tab = this.MainConfig.SelectedIndex;
                    this.RFID.Text = "RFID_OK";
                    this.MainConfig.SelectedIndex = tab + 1;
                    this.MainConfig.TabPages[6].Parent.Focus();
                    RFIDStatus.Enabled = false;
                    this.Refresh();                    
                });
                break;
            default:
                if (data.Length >= 16)
                {
                    data = data.Substring(0, 16);
                    this.Invoke((MethodInvoker)delegate
                    { Display("RFID Card: " + data + "\n"); });
                    if (RF_Card=="") { RF_Card = data; }
                    //return;
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        Display("RFID GOT UNKNOWN DATA:" + data + "\n");                        
                    });
                }
                data = "";
                break;
        }
    }

/**************************************************************************************/
/**************************************************************************************/
//OPEN NOTE DISPENSER PORT.

    private void OpenND_Click(object sender, EventArgs e)
    {
        string Port = ini.IniReadValue("SerialND","COM");

        Display("LOAD SETTINGS SerialND COM Port: " + Port + "\n");
        serialPortND.PortName = Port;
        serialPortND.BaudRate = 9600;
        serialPortND.DataBits = 8;
        serialPortND.Parity = System.IO.Ports.Parity.Even; 
        serialPortND.StopBits = System.IO.Ports.StopBits.One;
        serialPortND.Handshake = Handshake.None;
        serialPortND.ReadTimeout = 500;
        try
        {
            serialPortND.Open();
            OpenND.BackColor = Color.GreenYellow;
            OpenND.Enabled = false;
            NDStatus.Enabled = true;
            NotesTest.Enabled = true;
            this.Refresh();
        }
        catch
        {
            MessageBox.Show("SerialND Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
            Display("SerialND Error \n\nProblem While Opening Serial Port: " + Port + " \n");
            OpenND.BackColor = Color.Tomato;
            this.Refresh();
        } 
        serialPortND.Close();
    }

/**************************************************************************************/
//NOTE DISPENSER STATUS

    private void NDStatus_Click(object sender, EventArgs e)
    {
        byte[] status = {0x01,0x10,0x00,0x11,0x00,0x22 };
        byte[] reset  = {0x01,0x10,0x00,0x12,0x00,0x23 };
        
        if (!serialPortND.IsOpen)
            serialPortND.Open();
        else
            Thread.Sleep(1);

        Thread.Sleep(500);
        serialPortND.Write(reset, 0, reset.Length);
        Display("ND Wait\n");
        Thread.Sleep(500);         
        Thread.Sleep(500);
        serialPortND.Write(status, 0, status.Length);
    }

/**************************************************************************************/
//RECEIVE HANDLER FOR NOTE DISPENSER        

    private void ReceiveND(object sender, SerialDataReceivedEventArgs e)
    {
        byte[] buffer = new byte[256];

        int size = serialPortND.Read(buffer, 0, 7);
        String text = System.Text.Encoding.UTF8.GetString(buffer);        
        Display("ND_" + size.ToString() + " : " + text + "\n");
        text = text.TrimEnd('\0');
        if (size!=6)
            Display("ND ERROR: In Packet Length\n");

        switch (buffer[3])
        {
            case 0x00: Display("ND Status FINE\n");
                this.Invoke((MethodInvoker)delegate//used to call this function fron other function 
                {
                    this.NDStatus.Enabled = false;
                    this.NDStatus.BackColor = Color.YellowGreen;
                    this.NoteDispenser.Text = "NoteDispensser_OK";
                    int tab = this.MainConfig.SelectedIndex;
                    this.MainConfig.SelectedIndex = tab + 1;
                    this.MainConfig.TabPages[6].Parent.Focus();                    
                });
                Display("Note Dispenser READY\n");
                break;
            case 0x01: Display("ND Empty ReFILL NoteDispenser\n");
                break;
            case 0x02: Display("ND Low Stock\n");
                this.Invoke((MethodInvoker)delegate//used to call this function fron other function 
                { NDStatus.BackColor = Color.Tomato; });
                break;
            case 0x03: Display("ND Jam\n");
                this.Invoke((MethodInvoker)delegate//used to call this function fron other function 
                { NDStatus.BackColor = Color.Tomato; });
                break;
            case 0x06: Display("ND Sensor Error!!\n");
                this.Invoke((MethodInvoker)delegate//used to call this function fron other function 
                { NDStatus.BackColor = Color.Tomato; });
                break;
            case 0x08: Display("ND Motor Error!!\n");
                this.Invoke((MethodInvoker)delegate//used to call this function fron other function 
                { NDStatus.BackColor = Color.Tomato; });
                break;
            case 0xAA: Display("ND Succesful Pay\n");
                //NDres = 1;
                DNote++;
                //ReturnMoney = ReturnMoney + 500;
                break;
            case 0x0B: Display("ND Bad CRC\n");
                this.Invoke((MethodInvoker)delegate//used to call this function fron other function 
                { NDStatus.BackColor = Color.Tomato; });
                break;
            case 0xBB: Display("ND Payout FAIL\n");
                this.Invoke((MethodInvoker)delegate//used to call this function fron other function 
                {
                    this.NDStatus.Enabled = true;
                    this.NDStatus.BackColor = Color.Tomato;
                });
                break;
            default: Display("ND UNKNOWN Read Error: " + Convert.ToInt16(buffer[3])+"\n");
                break;

        }
    }

/**************************************************************************************/
    private void NotesTest_Click(object sender, EventArgs e)
    {
        byte[] EnAll = { 184 };
        byte[] NDOneBill = { 0x01, 0x10, 0x00, 0x10, 0x01, 0x22 };

        if (!serialPortND.IsOpen)
        {
            serialPortND.Open(); Thread.Sleep(100);
        }
        if (serialPortND.IsOpen) { serialPortND.Write(NDOneBill, 0, NDOneBill.Length); Thread.Sleep(100); }//1000

        if (!serialNV.IsOpen)
        {
            serialNV.Open(); Thread.Sleep(100);
        }
        if (serialNV.IsOpen) { serialNV.Write(EnAll, 0, EnAll.Length); Thread.Sleep(100); }
    }

/**************************************************************************************/
/**************************************************************************************/
    //OPEN PRINTER PORT
    private void OpenPRINTER_Click(object sender, EventArgs e)
    {
        string Port = ini.IniReadValue("SerialPRINTER", "COM");

        Display("LOAD SETTINGS SerialPrinter COM Port: " + Port + "\n");
        serialPRINTER.PortName = Port;
        serialPRINTER.BaudRate = 9600;
        serialPRINTER.DataBits = 8;
        serialPRINTER.Parity = System.IO.Ports.Parity.None;
        serialPRINTER.StopBits = System.IO.Ports.StopBits.One;
        serialPRINTER.Handshake = Handshake.XOnXOff;

        try
        {
            serialPRINTER.Open();
            OpenPRINTER.BackColor = Color.GreenYellow;
            OpenPRINTER.Enabled = false;
            PRINTERStatus.Enabled = true;
            PrintCashValues.Enabled = true;
            PrintTest.Enabled = true;
            this.Refresh();
        }
        catch
        {
            MessageBox.Show("SerialPrinter Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
            Display("SerialPrinter Error \n\nProblem While Opening Serial Port: " + Port + " \n");
            OpenPRINTER.BackColor = Color.Tomato;
            this.Refresh();
        }
        serialPRINTER.Close();
    }
    /**************************************************************************************/
    //PRINTER STATUS
    private void PRINTERStatus_Click(object sender, EventArgs e)
    {
        byte[] reset = { 0x1b, 0x40 };
        byte[] cut = { 0x1c, 0xc0, 0x34 }; // <- cut and move back -- //byte[] cut = { 0x1b, 0x69 };
        byte[] Bold = { 0x1b, 0x45, 3 };//2-No Bold
        byte[] Pos = { 0x1D, 0x4c, 20, 0 };//Gia na mhn tupwnei sthn akrh tou xartiou
        byte[] Status = { 0x10, 0x04, 0x04 };   //PMU = byte[] Status = { 0x10, 0x04, 0x04 };
        byte[] Large = { 0x1d, 0x21, 0x01 };
        byte[] Normal = { 0x1d, 0x21, 0x00 };
        byte[] single = { 0x00 };
        byte[] Euro = { 0x1b, 0x74, 0xff, 0x24 }; //byte[] Euro = { 0x1b, 0x74, 0xff, 0x24 };
        byte[] font_reset = { 0x1b, 0x74, 0x00 }; 
        
        try
        {
            if (!serialPRINTER.IsOpen)
                serialPRINTER.Open();
            else
                Thread.Sleep(1);

            serialPRINTER.Write(reset, 0, reset.Length); Thread.Sleep(100);
            serialPRINTER.Write(Bold, 0, Bold.Length); Thread.Sleep(100);
            serialPRINTER.Write(Pos, 0, Pos.Length); Thread.Sleep(100);
            Display("\nStatus");
            serialPRINTER.Write(Status, 0, Status.Length); Thread.Sleep(500);

            //TEST PRINTER
            serialPRINTER.Write(Large, 0, Large.Length);
            //serialPRINTER.WriteLine("DATA & CONTROL");
            //serialPRINTER.WriteLine("SYSTEMS");
            serialPRINTER.Write(Normal, 0, Normal.Length);
            serialPRINTER.WriteLine("\n   DATA & CONTROL\n      SYSTEMS\n\n");
            serialPRINTER.WriteLine(" PRINTER TEST: OK!");
            //serialPRINTER.WriteLine("@ KRHTIKA AKINHTA");
            /*serialPRINTER.WriteLine("!@#$%^&*()_+");
            serialPRINTER.WriteLine("01234567890123456789012345\n");
            serialPRINTER.WriteLine("ΕΛΛΗΝΙΚΑ\n");
            for (single[0] = 0x00; single[0] < 0xFF; single[0]++)
            {
                serialPRINTER.Write(single, 0, single.Length);
            }*/
            serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
            serialPRINTER.WriteLine("\n       - - -\n       - - -\n\n EOF.-\n"); Thread.Sleep(100);
            serialPRINTER.Write(cut, 0, cut.Length);
        }
        catch (Exception ex)
        {
            MessageBox.Show("SerialPrinter Error \n\nProblem While Writing Data");
            Display("SerialPrinter Error \n\nProblem While Writing Data \n");
            OpenPRINTER.BackColor = Color.Tomato;
        }
    }
    
    /**************************************************************************************/
    //RECEIVE HANDLER FOR PRINTER
    private void ReceivePRINTER(object sender, SerialDataReceivedEventArgs e)
    {
        byte[] buffer = new byte[100];

        int size = serialPRINTER.Read(buffer, 0, 60);
        //String data = System.Text.Encoding.UTF8.GetString(buffer);
        string data = BitConverter.ToString(buffer);
        data = data.Replace("-", "");
        data = data.Substring(0, 2 * size);
        //data = data.TrimEnd('\r', '\0', '\n');

        if (size > 0)
            this.Invoke((MethodInvoker)delegate
            { Display("Got Printer Message_" + size.ToString() + ": " + data + "\n"); });

        if (size >= 1 && ((buffer[0]) == 0x7E))
        {
            this.Invoke((MethodInvoker)delegate
            {
                Display("Printer ERROR: Paper End\nReplace paper\n"); SM = 11;
                TCP_Send("ERROR: Paper End Replace paper");
                secondForm.richTextBox1.ForeColor = Color.DarkRed;
                secondForm.richTextBox1.Text = "Warning: *** Printer OUT OF ORDER, Replace Paper! ***";
                PRINTERStatus.BackColor = Color.Tomato;
            });
            Thread.Sleep(1);
        }
        if (size >= 1 && (buffer[0] == 0x1E))
        {
            this.Invoke((MethodInvoker)delegate
            {
                Display("Printer Status OK\n");
                PRINTERStatus.BackColor = Color.YellowGreen;
                this.Printer.Text = "Printer_OK";
                PRINTERStatus.Enabled = false;
                int tab = this.MainConfig.SelectedIndex;
                if (tab < 5) this.MainConfig.SelectedIndex = tab + 1;
                this.MainConfig.TabPages[6].Parent.Focus();
                /*Display("\nPrinter MESSAGE: Near Paper End!!!\n\n");
                PRINTERStatus.BackColor = Color.Tomato;
                this.Printer.Text = "Printer_Near_Paper_End";
                secondForm.richTextBox1.ForeColor = Color.DarkOrange;
                secondForm.richTextBox1.Text = "Warning: *** Printer Near Paper END ***";
                int tab = this.MainConfig.SelectedIndex;
                if (tab < 5) this.MainConfig.SelectedIndex = tab + 1;
                this.MainConfig.TabPages[6].Parent.Focus();*/
            });
            Thread.Sleep(1);
        }
        if (size >= 1 && (buffer[0] == 0x12))
        {
            this.Invoke((MethodInvoker)delegate
            {
                Display("Printer Status OK\n");
                PRINTERStatus.BackColor = Color.YellowGreen;
                this.Printer.Text = "Printer_OK";
                PRINTERStatus.Enabled = false;
                int tab = this.MainConfig.SelectedIndex;
                if (tab < 5) this.MainConfig.SelectedIndex = tab + 1;
                this.MainConfig.TabPages[6].Parent.Focus();
            });
            Thread.Sleep(1);
        }
        data = "";
    }

/**************************************************************************************/
// PrintTEST Receipt 

    private void test_Click(object sender, EventArgs e)
    {
        //int i = 0;
        byte[] cut = { 0x1c, 0xc0, 0x34 }; // <- cut and move back -- //byte[] cut = { 0x1b, 0x69 };
        byte[] reset = { 0x1b, 0x40 };
        byte[] Bold = { 0x1b, 0x45, 3 };//2-No Bold
        byte[] Pos = { 0x1D, 0x4c, 20, 0 };//Gia na mhn tupwnei sthn akrh tou xartiou
        byte[] VeryLarge = { 0x1d, 0x21, 0x22 };
        byte[] Large = { 0x1d, 0x21, 0x01 };
        byte[] Normal = { 0x1d, 0x21, 0x00 };
        //byte[] AbsPos = { 0x1b, 0x5c, 100,100 };
        byte[] Status = { 0x10, 0x04, 0x04 };  //PMU = byte[] Status = { 0x10, 0x04, 0x04 };
        //byte[] AbsPos = { 0x1d, 0x50, 10,10 };//DEN KANEI KATI
        //byte[] AbsPos = { 0x1b, 0x24, 0, 0 };
        byte[] AbsPos = { 0x1d, 0x24, 1, 0 };//{ 0x1b, 0x5c, 100, 100 };
        byte[] Euro = { 0x1b, 0x74, 0xff, 0x24 };
        byte[] font_reset = { 0x1b, 0x74, 0x00 };

        //string GreekTest = "ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ";
        //OpenPRINTER_Click(this,e);

        if (!serialPRINTER.IsOpen)
            serialPRINTER.Open();
        else
            Thread.Sleep(1);

        serialPRINTER.Write(reset, 0, reset.Length); Thread.Sleep(100);
        serialPRINTER.Write(AbsPos, 0, AbsPos.Length); Thread.Sleep(100);
        //serialPRINTER.Write(Bold, 0, Bold.Length); Thread.Sleep(100);
        serialPRINTER.Write(Pos, 0, Pos.Length); Thread.Sleep(500);
        serialPRINTER.Write(Status, 0, Status.Length); Thread.Sleep(500);   // 1500
        //serialPRINTER.Write(AbsPos, 0, AbsPos.Length); Thread.Sleep(100);        
        serialPRINTER.Write(Large, 0, Large.Length);
        GR("ΤΣΟΥΦΛΙΔΟΥ ΕΛΕΝΗ\n");
        //GR("ΚΥΔΩΝ ΔΗΜ.ΚΗ ΑΕ\n");
        //GR("ΔΗΜ.ΚΗ Α.Ε");
        //serialPRINTER.WriteLine("\n");
        serialPRINTER.Write(Bold, 0, Bold.Length); Thread.Sleep(100);
        serialPRINTER.Write(Normal, 0, Normal.Length); Thread.Sleep(100);
        GR("ΕΚΜΕΤΑΛΛΕΥΣΗ ΧΩΡΟΥ ΣΤΑΘΜΕΥΣΗΣ\n");
        GR("ΥΠΟΚ/ΜΑ: PARKING ΤΕΛΩΝΕΙΟΥ\n");
        //GR("ΠΕΡΙΔΟΥ & ΥΨΗΛΑΝΤΩΝ 73135\n");
        GR("Τ.Κ. 65500\n");
        //GR("ΥΠΗΡΕΣΙΕΣ ΣΤΑΘΜΕΥΣΗΣ\n");
        //GR("    ΑΥΤΟΚΙΝΗΤΩΝ    "); serialPRINTER.WriteLine(" ");

        GR("ΑΦΜ: 142212131, ΔΟΥ: ΚΑΒΑΛΑΣ\n");
        //GR("ΔΟΥ: ΧΑΝΙΩΝ\n");
        GR("ΤΗΛ: 6932 383139\n");


        GR("ΑΠΟΔΕΙΞΗ ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ\n");
        //GR("ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ"); 
        GR("--------------------");
        GR("\nΑΡΙΘΜΟΣ ΑΠΟΔΕΙΞΗΣ:\n");
        GR("\n--------------------");
        GR("\nAΞΙΑ ΕΙΣΙΤΗΡΙΟΥ:  "); 
        GR("\nΕΙΣΟΔΟΣ: "); 
        GR("\nΕΞΟΔΟΣ :  ");
        GR("\n--------------------");
        GR("\nΣΥΝΟΛΟ ΜΕ Φ.Π.Α: %");
        GR("\n\nΜΕΤΡΗΤΑ "); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
        GR("\nΠΟΣΟ ΧΩΡΙΣ Φ.Π.Α"); 
        GR("\nΦ.Π.Α");
        GR("\n--------------------");
        GR("\nΓΙΑ ΤΟΝ ΠΕΛΑΤΗ"); 
        GR("\nΑΘΕΩΡΗΤΑ \nΒΑΣΕΙ ΤΗΣ ΠΟΛ. 1083/2003\n\n");
        GR("********************"); serialPRINTER.WriteLine(" ");
        GR("*   ΕΥΧΑΡΙΣΤΟΥΜΕ   *"); serialPRINTER.WriteLine(" ");
        GR("********************"); serialPRINTER.WriteLine(" ");
        serialPRINTER.WriteLine("\n"); Thread.Sleep(100);
        serialPRINTER.Write(cut, 0, cut.Length);

    }

/**************************************************************************************/
/**************************************************************************************/
//OPEN NV PORT

    private void OpenNV_Click(object sender, EventArgs e)
    {
        string Port = ini.IniReadValue("SerialNV", "COM");

        Display("LOAD SETTINGS Note Validator COM Port: " + Port + "\n");
        serialNV.PortName = Port;
        serialNV.BaudRate = 9600;//300;    //was 300 for NV9, now 9600 for NV9 USB
        serialNV.DataBits = 8;
        serialNV.Parity = System.IO.Ports.Parity.None;
        serialNV.StopBits = System.IO.Ports.StopBits.Two;
        serialNV.Handshake = Handshake.XOnXOff;
        //serialNV.ReadTimeout = 300;

        try
        {
            serialNV.Open();
            OpenNV.BackColor = Color.GreenYellow;
            OpenNV.Enabled = false;
            NVStatus.Enabled = true;            
            this.Refresh();
        }
        catch
        {
            MessageBox.Show("NoteValidator Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
            Display("NoteValidator Error \n\nProblem While Opening Serial Port: " + Port + " \n");
            OpenNV.BackColor = Color.Tomato;
            this.Refresh();
        }
        serialNV.Close();
    }

/**************************************************************************************/
//NV STATUS

    private void NVStatus_Click(object sender, EventArgs e)
    {
        byte[] status = { 0xB6 };
        byte[] Inh05 = { 131 };
        byte[] Inh10 = { 132 };
        byte[] Inh20 = { 133 };
        byte[] Inh50 = { 134 };
        byte[] UnInh05 = { 151 };
        byte[] UnInh10 = { 152 };
        byte[] UnInh20 = { 153 };
        byte[] UnInh50 = { 154 };
        byte[] EnAll = { 184 };
        byte[] DisAll = { 185 };
        byte[] EnEscrow = { 170 };
        byte[] DisEscrow = { 171 };
        byte[] AccEscrow = { 172 };
        byte[] RejEscrow = { 173 };
        byte[] DisEscrowTimeout = { 190 };
         

        if (!serialNV.IsOpen)
        {
            serialNV.Open(); Thread.Sleep(100);
        }
        else
            Thread.Sleep(1);
        //Disable NOTES
        serialNV.Write(Inh05, 0, Inh05.Length); Thread.Sleep(100);
        serialNV.Write(Inh10, 0, Inh10.Length); Thread.Sleep(100);
        serialNV.Write(Inh20, 0, Inh20.Length); Thread.Sleep(100);
        serialNV.Write(Inh50, 0, Inh50.Length); Thread.Sleep(100);
        string NVdata = ini.IniReadValue("SerialNV", "NOTES");

        //Enable NOTES
        if (NVdata.IndexOf("05") != -1)
            serialNV.Write(UnInh05, 0, UnInh05.Length); Thread.Sleep(100);
        if (NVdata.IndexOf("10") != -1)
            serialNV.Write(UnInh10, 0, UnInh10.Length); Thread.Sleep(100);
        if (NVdata.IndexOf("20") != -1)
            serialNV.Write(UnInh20, 0, UnInh20.Length); Thread.Sleep(100);
        if (NVdata.IndexOf("50") != -1)
            serialNV.Write(UnInh50, 0, UnInh50.Length); Thread.Sleep(100);

        serialNV.Write(DisAll, 0, DisAll.Length); Thread.Sleep(100);
        serialNV.Write(DisEscrow, 0, DisEscrow.Length); Thread.Sleep(100);
        //serialNV.Write(DisEscrowTimeout, 0, DisEscrowTimeout.Length); Thread.Sleep(100);
        serialNV.Write(status, 0, status.Length); Thread.Sleep(100);
        NV_Data = "";

    }

/**************************************************************************************/
//RECEIVE HANDLER FOR NV

    private void ReceiveNV(object sender, SerialDataReceivedEventArgs e)
    {
        byte[] buffer = new byte[12];

        int size = serialNV.Read(buffer, 0, 8);
        string data = BitConverter.ToString(buffer);
        data = data.Replace("-", "");
        data = data.Substring(0, 2 * size);
        //for (int i = 0; i <= size; i++)
        //{
            //Display("NoteValidator, RAW buffer1: " + buffer[0] + "\n");
            //Display("NoteValidator, RAW data: " + data + "\n");
        //}
        if (data.IndexOf("B6") >= 0)
        {
            this.Invoke((MethodInvoker)delegate
            {
                NVStatus.Enabled = false;
                NVStatus.BackColor = Color.YellowGreen;
                this.NoteValidator.Text = "NoteValidator_OK";
                int tab = this.MainConfig.SelectedIndex;
                this.MainConfig.SelectedIndex = tab + 1;
                this.MainConfig.TabPages[6].Parent.Focus();
                this.Refresh();                
            });
            return;
        }
        switch (data)   // switch was on buffer[0], with hex the cases, but did not work with NV9USB    - dim
        {
            case "7901": Display("NoteValidator 5 Euro Note " + data+ "\n");    // 01
                Payment = Payment + 500;
                //secondForm.Vprogress.Maximum = 30;
                GeneralCounter = 120;
                string five = ini.IniReadValue("SerialNV", "Paid05");
                int temp = Convert.ToInt16(five); temp++;
                ini.IniWriteValue("SerialNV", "Paid05", temp.ToString());
                break;
            case "7902": Display("NoteValidator 10 Euro Note " + data + "\n");  // 02
                Payment = Payment + 1000;
                //secondForm.Vprogress.Maximum = 30;
                GeneralCounter = 120;
                string ten = ini.IniReadValue("SerialNV", "Paid10");
                temp = Convert.ToInt16(ten); temp++;
                ini.IniWriteValue("SerialNV", "Paid10", temp.ToString());
                break;
            case "7903": Display("NoteValidator 20 Euro Note " + data + "\n");  // 03
                Payment = Payment + 2000;
                //secondForm.Vprogress.Maximum = 30;
                GeneralCounter = 120;
                string twenty = ini.IniReadValue("SerialNV", "Paid20");
                temp = Convert.ToInt16(twenty); temp++;
                ini.IniWriteValue("SerialNV", "Paid20", temp.ToString());
                break;
            case "7904": Display("NoteValidator 50 Euro Note " + data + "\n");  // 04
                Payment = Payment + 5000;
                //secondForm.Vprogress.Maximum = 30;
                GeneralCounter = 120;
                string fifty = ini.IniReadValue("SerialNV", "Paid50");
                temp = Convert.ToInt16(fifty); temp++;
                ini.IniWriteValue("SerialNV", "Paid50", temp.ToString());
                break;
            case "14": Display("NoteValidator: Note NOT recognized " + data + "\n");    // 20
                break;
            case "32": Display("\n\n\n\nNoteValidator: FRAUD !!!!!!!!!!!!!!!!!" + data+"\n\n\n");   // 50
                break;
            case "3C": Display("NoteValidator Full or Jammed\n");   // 60
                SM = 11;
                TCP_Send("ERROR: NV Full or Jammed");
                break;
            case "78": //Display("\nNV : Buzy " + data);    // 120
                break;
            case "79": //Display("\nNV : Ready " + data);   // 121
                break;
            case "B8": Display("NoteValidator: Enable ALL " + data + "\n"); // 184
                break;
            case "B9": Display("NoteValidator: Disable ALL " + data + "\n");   // 185
                break;
            default: Display("NoteValidator: Unknown Report " + data + "\n");
                break;
        }//switch

    }

/**************************************************************************************/
/**************************************************************************************/
//OPEN COIN PORT

    private void OpenCoins_Click(object sender, EventArgs e)
    {
        //Debugging.Clear();
        Int32 ComPort = 0x000001;
        string Port = ini.IniReadValue("SerialCOINS", "COM");
        Int32 temp = 0x100 * Convert.ToInt32(Port);
        Display("LOAD SETTINGS Coin Dispenser COM Port: " + Port + "\n");
        ComPort += temp;

        wndHandle = this.Handle.ToInt32();
        try
        {
            pm.open(ComPort); Thread.Sleep(100);
            CoinStatus.Enabled = true;
            OpenCoins.Enabled = false;
            OpenCoins.BackColor = Color.YellowGreen;
            CoinsTest.Enabled = true;
            //pm.start(wndHandle, WM_PAYMENTMESSAGE, 0, 0, 0);
            this.Refresh();
        }
        catch
        {            
            MessageBox.Show("Coins Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
            Display("Coins Error \n\nProblem While Opening Serial Port: " + Port + " \n");
        }
    }
/**************************************************************************************/
//COINS STATUS

    private void CoinStatus_Click(object sender, EventArgs e)
    {
        pm.start(wndHandle, WM_PAYMENTMESSAGE, 0, 0, 0);
        if (pm.set(0, 1, 0, 0) != 0)
            Display("CoinDisp: Disable all ERROR\n");
    }
/**************************************************************************************/
//FINAL REFRESH BUTTON

    private void Refresh_Click(object sender, EventArgs e)
    {
        byte[] cut = { 0x1c, 0xc0, 0x34 }; // <- cut and move back -- //byte[] cut = { 0x1b, 0x69 };
        byte[] Euro = { 0x1b, 0x74, 0xff, 0x24 };
        byte[] font_reset = { 0x1b, 0x74, 0x00 };
        string temp = "";

        //Debugging.Clear();
        int stat;
        Display("Coins Availability\n");
        
        // H for itan apo 2 ews 7, to idio kai ta cases - JIM
        for (int i = 0; i <= 5; i++)
        {
            stat = pm.set(2, 4, i, 0);
            Thread.Sleep(50);
            if (stat != -1)
            {
                Display(i.ToString() + ": " + stat.ToString());
                switch (i)
                {
                    case 0: this.Coin5.Text = stat.ToString();
                        break;
                    case 1: this.Coin10.Text = stat.ToString();
                        break;
                    case 2: this.Coin20.Text = stat.ToString();
                        break;
                    case 3: this.Coin50.Text = stat.ToString();
                        break;
                    case 4: this.Coin100.Text = stat.ToString();
                        break;
                    case 5: this.Coin200.Text = stat.ToString();
                        break;
                }
            }
            else
                Display("Coins ERROR: No Data\n");
        }
        Total_Coins = pm.set(1, 3, 0, 0);//TOTAL COINS CMD
        stat = Total_Coins;
        string tmp = stat.ToString();
        if(tmp.Length>2) tmp = tmp.Insert(tmp.Length - 2, ",");
        Display("TotalCoins:" + tmp + "\n");

        //Read INI values
        Avail10Notes.Text = ini.IniReadValue("SerialND", "Avail10");
        Avail05Notes.Text = ini.IniReadValue("SerialND", "Avail05");
        Paid50Notes.Text = ini.IniReadValue("SerialNV", "Paid50");
        Paid20Notes.Text = ini.IniReadValue("SerialNV", "Paid20");
        Paid10Notes.Text = ini.IniReadValue("SerialNV", "Paid10");
        Paid05Notes.Text = ini.IniReadValue("SerialNV", "Paid05");

        if ((serialPRINTER.IsOpen == true) && (p))
        {
            //Print Result
            serialPRINTER.WriteLine("\nDATA & CONTROL SYSTEMS\n\n");

            //serialPRINTER.WriteLine("--------------------");
            serialPRINTER.WriteLine("--------------------");
            serialPRINTER.WriteLine(" * CASHIER REPORT *");
            //serialPRINTER.WriteLine("--------------------");
            serialPRINTER.WriteLine("--------------------\n");
            serialPRINTER.WriteLine("* Available Coins *");
            serialPRINTER.WriteLine("--------------------");
            GR("      0,05"); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length); serialPRINTER.WriteLine(": " + Coin5.Text);
            GR("      0,10"); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length); serialPRINTER.WriteLine(": " + Coin10.Text);
            GR("      0,20"); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length); serialPRINTER.WriteLine(": " + Coin20.Text);
            GR("      0,50"); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length); serialPRINTER.WriteLine(": " + Coin50.Text);
            GR("      1,00"); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length); serialPRINTER.WriteLine(": " + Coin100.Text);
            GR("      2,00"); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length); serialPRINTER.WriteLine(": " + Coin200.Text);
            temp = stat.ToString();
            temp = temp.Insert(temp.Length - 2, ",");
            GR("Total Coins: " + temp); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);         
            
            serialPRINTER.WriteLine("\n\n* Available Notes *");
            serialPRINTER.WriteLine("--------------------");
            serialPRINTER.WriteLine(" 5-euro Notes: " + Avail05Notes.Text);
            serialPRINTER.WriteLine("10-euro Notes: " + Avail10Notes.Text);

            serialPRINTER.WriteLine("\n  ** Paid Notes **");
            serialPRINTER.WriteLine("--------------------");
            serialPRINTER.WriteLine(" 5-euro Notes: " + Paid05Notes.Text);
            serialPRINTER.WriteLine("10-euro Notes: " + Paid10Notes.Text);
            serialPRINTER.WriteLine("20-euro Notes: " + Paid20Notes.Text);
            serialPRINTER.WriteLine("50-euro Notes: " + Paid50Notes.Text+"\n");
            
            serialPRINTER.WriteLine("Name: ______________ ");
            serialPRINTER.WriteLine("Date: ___/___/______ \n");
            serialPRINTER.WriteLine("Signature:\n\n         ___________ ");
            serialPRINTER.WriteLine("\nEOF.-\n\n\n\n.");

            serialPRINTER.Write(cut, 0, cut.Length);
        }
        else
        {
            Display("Print Note&Coins Not Permited\n");
        }
    }

/**************************************************************************************/
//Return Coins Test   

    private void CoinsTest_Click(object sender, EventArgs e)
    {
        pm.set(1, 0, 385, 0);
    }

/**************************************************************************************/
// Print Cash & Coins Report

    private void button1_Click(object sender, EventArgs e)
    {
        p = true; Refresh_Click(this, e); p = false;
    }

/**************************************************************************************/
//FINAL SAVE BUTTON

    private void Save_Click(object sender, EventArgs e)
    {
        ini.IniWriteValue("SerialND", "Avail10", Avail10Notes.Text); Thread.Sleep(100);
        ini.IniWriteValue("SerialND", "Avail05", Avail05Notes.Text); Thread.Sleep(100);
        ini.IniWriteValue("SerialNV", "Paid50", Paid50Notes.Text); Thread.Sleep(100);
        ini.IniWriteValue("SerialNV", "Paid20", Paid20Notes.Text); Thread.Sleep(100);
        ini.IniWriteValue("SerialNV", "Paid10", Paid10Notes.Text); Thread.Sleep(100);
        ini.IniWriteValue("SerialNV", "Paid05", Paid05Notes.Text); Thread.Sleep(100);
        Display("\nAvailable Data Values Saved to Ini\n");
    }

/**************************************************************************************/
/**************************************************************************************/
/**************************************************************************************/
/**************************************************************************************/    
// INIT SYSTEM
    private Thread DW = new Thread(DiskWriter);
    private void Init_System_Click(object sender, EventArgs e)
    {        
        DW.Name = "Disk.Writer";
        DW.Start();

        Display("\n\n*** Application AUTO LOADING Started ***\n");
        this.Init_System.Enabled = false;
        MainConfig.Enabled = true;

        TCP_Connect_Click(this, e); this.Refresh(); Thread.Sleep(1000);

        if (TCP_Connect.BackColor != Color.YellowGreen)
            return;

        try
        {
            OpenBR_Click(this, e); this.Refresh(); Thread.Sleep(800);
            BRStatus_Click(this, e); this.Refresh(); Thread.Sleep(2000);
        }
        catch (Exception ex) { Display("BARCODE ERROR: "+ex.Message.ToString()); }

        try
        {
            OpenRFID_Click(this, e); this.Refresh(); Thread.Sleep(800);
            RFIDStatus_Click(this, e); this.Refresh(); Thread.Sleep(2000);
        }
        catch (Exception ex) { Display("RFID ERROR: " + ex.Message.ToString()); }
        
        try
        {
            OpenND_Click(this, e); this.Refresh(); Thread.Sleep(1000);
            NDStatus_Click(this, e); this.Refresh(); Thread.Sleep(2000);
        }
        catch (Exception ex) { Display("ND ERROR: " + ex.Message.ToString()); }
        
        try
        {
            OpenPRINTER_Click(this, e); this.Refresh(); Thread.Sleep(800);
            PRINTERStatus_Click(this, e); this.Refresh(); Thread.Sleep(2000);
        }
        catch (Exception ex) { Display("ERROR: " + ex.Message.ToString()); }
        
        try
        {
            OpenNV_Click(this, e); this.Refresh(); Thread.Sleep(800);
            NVStatus_Click(this, e); this.Refresh(); Thread.Sleep(2000);
        }
        catch (Exception ex) { Display("NV ERROR: " + ex.Message.ToString()); }
        
        try
        {
            OpenCoins_Click(this, e); this.Refresh(); Thread.Sleep(800);
            CoinStatus_Click(this, e); this.Refresh(); Thread.Sleep(2000);
        }
        catch (Exception ex) { Display("COINS ERROR: " + ex.Message.ToString()); }
        
        p = false; Refresh_Click(this, e);

        this.MainConfig.SelectedIndex = 6;
        this.MainConfig.TabPages[6].Parent.Focus();

        this.Init_System.BackColor = Color.GreenYellow;
        Thread.Sleep(2000);
        StartApplication.Enabled = true;
        this.Refresh();      

    }

/**************************************************************************************/
/**************************************************************************************/        
// START Application

    private void StartApplication_Click(object sender, EventArgs e)
    {
        Cursor.Hide();
        Save.Enabled = false;
        Avail05Notes.ReadOnly = true;
        Avail10Notes.ReadOnly = true;
        Paid05Notes.ReadOnly = true;
        Paid10Notes.ReadOnly = true;
        Paid20Notes.ReadOnly = true;
        Paid50Notes.ReadOnly = true;

        if (m_clientSocket == null)
        {
            Cursor.Show();
            MessageBox.Show("Server NOT Connected");
            return;
        }
        // DEVICE ERRORS 
        if (BRStatus.BackColor != Color.YellowGreen)
        {
            Cursor.Show();
            MessageBox.Show("Barcode Error\nCannot Start Application");
            return;
        }
        if (RFIDStatus.BackColor != Color.YellowGreen)
        {
           Cursor.Show();
            MessageBox.Show("RFID Error\nCannot Start Application");
            return;
        }
        if (NDStatus.BackColor != Color.YellowGreen)
        {
            Cursor.Show();
            MessageBox.Show("Note Dispenser Error\nCannot Start Application");
            return;
        }
        if (PRINTERStatus.BackColor != Color.YellowGreen)
        {
            Cursor.Show();
            MessageBox.Show("Printer Error\nCannot Start Application");
            return;
        }
        if (NVStatus.BackColor != Color.YellowGreen)
        {
            Cursor.Show();
            MessageBox.Show("Note Validator Error\nCannot Start Application");
            return;
        }
        if (CoinStatus.BackColor != Color.YellowGreen)
        {
            Cursor.Show();
            MessageBox.Show("Coin Recycler Error\nCannot Start Application");
            return;
        }        

        secondForm.Text = "PAY MACHINE";
        secondForm.Refresh();
        secondForm.WindowState = FormWindowState.Maximized;
        secondForm.FormBorderStyle = FormBorderStyle.None;
        Thread.Sleep(1);
        StartApplication.Enabled = false;
        secondForm.Messages2.Visible = false;
        LanguageTimer.Interval = 1000;
        LanguageTimer.Start();
        GeneralTimer.Interval = 500;//STATE MACHINE
        GeneralTimer.Start();
        
        MoneyStatus(2);
        secondForm.Show();
        if ((Total_Notes < 1000) || (Total_Coins < 1000))
        {
            Display("Money Very Low");
            SM = 11; TCP_Send("ERROR: Money Very Low");
        }
    }

/**************************************************************************************/
/**************************************************************************************/
//Form 2 closed
    private void Closing(object sender, FormClosedEventArgs e)
    {
        this.MainConfig.SelectedIndex = 6;
        p = false; Refresh_Click(this, e); p = true;
        this.MainConfig.TabPages[6].Parent.Focus(); this.Refresh();
        Display(" ");
        Display(" *** Application CLOSING!!! ***\n\n\n"); //MessageBox.Show("CLOSING");
        Display(" ");
        Display(" ");
        if (Init_System.BackColor == Color.GreenYellow)
        {
            Save_Click(this, e); Thread.Sleep(500);
        }
        else
        {
            Display("Cannot Save on exit,Values not set\n");
            //MessageBox.Show("Cannot Save on exit,Values not set");
        }
        Thread.Sleep(100);
    } 

/**************************************************************************************/
/**************************************************************************************/
/**************************************************************************************/
/**************************************************************************************/
//DISPLAY MESSAGES
    private static Queue<string> que = new Queue<string>();
    private static Object locker = new object();
    public void Display(string s)//just a function used to display messages on richedit box
    {
        lock (locker)
        {
            que.Enqueue(s);
        }                
    }

/**************************************************************************************/
/**************************************************************************************/
    private static void DiskWriter()
    {
        do
        {
            string s = "";
            while (que.Count > 0)
            {
                lock (locker)
                {
                    s = que.Dequeue();
                }
                try
                {
                    string CurrentDate = DateTime.Now.ToString("ddMMyy");
                    string CurrentFileName = "C:/POS/log/" + CurrentDate + "_POS_Log.txt";


                    if (System.IO.File.Exists(CurrentFileName) == false)
                    {
                        WRfile.Close(); Thread.Sleep(100);
                        WRfile = new System.IO.StreamWriter(CurrentFileName, true, System.Text.Encoding.UTF8, 100);
                        WRfile.AutoFlush = true;
                    }
                    if (s.Length == 0) s = "NULL_LOG_DATA";
                    POS_v20.Form1.instance.Invoke((MethodInvoker)delegate
                    {
                        //s = s.Replace('\n','_');          
                        POS_v20.Form1.instance.Debugging.AppendText(DateTime.Now.ToString("\ndd/MM/yy HH:mm:ss/> ") + s);
                        POS_v20.Form1.instance.Debugging.ScrollToCaret();
                    });

                    WRfile.WriteLine(DateTime.Now.ToString("dd/MM/yy HH:mm:ss/> ") + s);
                }
                catch (Exception e)
                {
                    try
                    {
                        if (WRfile != null) WRfile.Close();
                    }
                    catch (Exception) { }
                }
            }
            Thread.Sleep(4000);

        } while (true);
    }

/**************************************************************************************/
/**************************************************************************************/


/**************************************************************************************/
// ANOTHER COUNTER
    private void Language_Tick(object sender, EventArgs e)
    {
        GeneralCounter--;
        //secondForm.Refresh();
        secondForm.Vprogress.Value = GeneralCounter;
        //secondForm.Vprogress.Increment(1);
        if (GeneralCounter < 1)
        {
            GeneralCounter = 0;
            secondForm.Vprogress.Visible = false;
            SM = 10;
            GeneralCounter = 120;//30
            secondForm.Vprogress.Maximum = 120; //30           
        }
    }
/**************************************************************************************/
// MAIN MAIN MAIN MAIN MAIN MAIN
        private void GeneralTimer_Tick(object sender, EventArgs e)
        {
            secondForm.TopMost = true;
            
            secondForm.Update();
            string Langtemp;
            //byte[] byData;
            byte[] EnAll = { 184 };
            byte[] DisAll = { 185 };
            byte[] AccEscrow = { 172 };
            byte[] RejEscrow = { 173 };
            byte[] NDOneBill  = {0x01,0x10,0x00,0x10,0x01,0x22 };
            
            
            if (secondForm.Text != "")
                Thread.Sleep(1);
            else{
                GeneralTimer.Stop();
                Display("Form2 Closed");
                p = false;
                Refresh_Click(this, e);
                Cursor.Show();
                return;
            }
            if (secondForm.CancelButton){
                SM = 10;
                Display("CANCEL BUTTON PRESSED\n");
            }
            if (TCP_Connect.BackColor==Color.Tomato){
                //GeneralTimer.Stop(); //secondForm.Close(); 
                SM = 11;
                Display("FATAL NETWORK ERROR");
                //Cursor.Show();
                return;
            }
            
            switch(SM){
            
                case 1:
                    Display("SM: " + SM);
                    secondForm.Cancel.Visible = false;
                    ShowNotes("000");
                    InitalCost = 0;
                    Payment = 0;
                    ReturnMoney = 0;
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Vprogress.Visible = false;
                    secondForm.Messages2.Visible = false;
                    secondForm.btnYes.Visible = false;
                    secondForm.btnNo.Visible = false;
                    //secondForm.buttonUK.Visible = true;
                    //secondForm.buttonGRE.Visible = true;
                    //secondForm.buttonGER.Visible = true;
                    //secondForm.buttonFRA.Visible = true;
                    Langtemp = ini.IniReadValue("LANGUAGE", "insert" + secondForm.Language);
                    Langtemp = ini.IniReadValue("LANGUAGE", "insert" + secondForm.Language);
                    if (secondForm.POS_Messages.Text.IndexOf(Langtemp) != 0)
                    {
                        secondForm.POS_Messages.Clear();
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                    }

                    Image image;
                    secondForm.Ticket_Icon.Visible = true;

                    if (secondForm.Language.Length <= 1)
                    {
                        secondForm.Language = "GRE";
                        //secondForm.buttonGRE.PerformClick();
                        Display("Lang: auto GR\n");
                        secondForm.LangPictureBox.Visible = true;
                        image = Image.FromFile("C:/POS/GREC0001.GIF");
                        secondForm.LangPictureBox.Image = image;
                        secondForm.Refresh();
                    }
                    
                    if (secondForm.Language.StartsWith("GRE") == true)
                    {
                        secondForm.buttonUK.Visible = true;
                        secondForm.buttonGRE.Visible = false;
                        secondForm.buttonGER.Visible = true;
                        secondForm.buttonFRA.Visible = true;
                    }
                    else if (secondForm.Language.StartsWith("ENG") == true)
                    {
                        secondForm.buttonUK.Visible = false;
                        secondForm.buttonGRE.Visible = true;
                        secondForm.buttonGER.Visible = true;
                        secondForm.buttonFRA.Visible = true;
                    }
                    else if (secondForm.Language.StartsWith("FRA") == true)
                    {
                        secondForm.buttonUK.Visible = true;
                        secondForm.buttonGRE.Visible = true;
                        secondForm.buttonGER.Visible = true;
                        secondForm.buttonFRA.Visible = false;
                    }
                    else if (secondForm.Language.StartsWith("GER") == true)
                    {
                        secondForm.buttonUK.Visible = true;
                        secondForm.buttonGRE.Visible = true;
                        secondForm.buttonGER.Visible = false;
                        secondForm.buttonFRA.Visible = true;
                    }

                    if (BR_Card.Length != 0)
                    {
                        if (BR_Card.Length != BC_Lenght)
                        {
                            SM = 31;
                            //secondForm.Vprogress.Visible = false;
                            break;
                        }
                        secondForm.Messages2.Visible = false;
                        UserCode = BR_Card;
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();
                        secondForm.Cancel.Visible = true;
                        secondForm.buttonUK.Visible = false;
                        secondForm.buttonGRE.Visible = false;
                        secondForm.buttonGER.Visible = false;
                        secondForm.buttonFRA.Visible = false;
                        ShowNotes("000");
                        secondForm.Refresh();
                        SM = 4; BR_Card = ""; RF_Card = ""; //4    // REPLACE TO SM = 22 FOR COUPON USE!!!
                    }
                    if (RF_Card.Length != 0)
                    {
                        if (RF_Card == "8172635445362718")
                        {
                            secondForm.Close();
                            SM = 31; secondForm.Vprogress.Visible = false; break;
                        }
                        if (RF_Card.Length != RFID_Lenght)
                        {
                            SM = 31; secondForm.Vprogress.Visible = false; break;
                        }
                        secondForm.Messages2.Visible = true;
                        UserCode = RF_Card;
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Cancel.Visible = true;
                        secondForm.buttonUK.Visible = false;
                        secondForm.buttonGRE.Visible = false;
                        secondForm.buttonGER.Visible = false;
                        secondForm.buttonFRA.Visible = false;
                        ShowNotes("000");
                        secondForm.Refresh();
                        //secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();
                        SM = 41; BR_Card = ""; RF_Card = "";
                    }

                    /*
                    if (secondForm.POS_Messages.Text.IndexOf("SELECT LANGUAGE") != 0){
                        secondForm.POS_Messages.Clear();
                        //secondForm.POS_Messages.ForeColor = Color.Black; // RoyalBlue
                        secondForm.POS_Messages.AppendText("SELECT LANGUAGE"); 
                        secondForm.Refresh();
                    } */

                    /*
                    if (secondForm.Language.Length > 0){
                        Thread.Sleep(500); 
                        SM = 2; secondForm.POS_Messages.Clear();
                        secondForm.Messages2.Clear();
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                    } */
                    //BR_Card = ""; RF_Card = ""; //RF_Card = "-"

                    Thread.Sleep(100);
                    break;

                    /*
                case 2:
                    Display("SM: " + SM);
                    secondForm.Cancel.Visible = true;
                    ShowNotes("000");
                    BR_Card = ""; RF_Card = "";
                    InitalCost = 0;
                    Payment = 0;
                    ReturnMoney = 0;
                    secondForm.Vprogress.Visible = false;
                    secondForm.BackgroundImage = null;
                    //secondForm.BackColor = Color.Silver;    //Black
                    //secondForm.Cancel.BackColor = Color.Red;
                    secondForm.Refresh();
                    Langtemp = ini.IniReadValue("LANGUAGE", "welcome" + secondForm.Language);
                    secondForm.buttonUK.Visible = false;
                    secondForm.buttonGRE.Visible = false;
                    secondForm.buttonGER.Visible = false;
                    secondForm.buttonFRA.Visible = false;
                    secondForm.btnYes.Visible = false;
                    secondForm.btnNo.Visible = false;
                    if (secondForm.POS_Messages.Text.IndexOf(Langtemp) != 0){
                        secondForm.POS_Messages.Clear();
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //MediumSpringGreen
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                    }                    
                    if (secondForm.Language != null){
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        SM = 3; Thread.Sleep(1500);     //22                   
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();                        
                        //secondForm.Messages2.Visible = true;                        
                    }                    
                    BR_Card = ""; RF_Card = "";
                    break;
                    */
                case 22:
                    Display("SM: " + SM);
                    Langtemp = ini.IniReadValue("LANGUAGE", "Coupon" + secondForm.Language);
                    if (secondForm.POS_Messages.Text.IndexOf(Langtemp) != 0)
                    {
                        secondForm.btnYes.Visible = true;
                        secondForm.btnNo.Visible = true;
                        secondForm.POS_Messages.Clear();
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Lavender
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                    }
                    if (secondForm.btnYN != 0)
                    {                        
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        SM = 4; //3
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();
                        if (secondForm.btnYN == 1) //Yes
                        {
                            UserHasCpn = true;
                            SM = 23;
                            secondForm.POS_Messages.Clear();
                            Langtemp = ini.IniReadValue("LANGUAGE", "ScanCpn" + secondForm.Language);
                            //secondForm.POS_Messages.ForeColor = Color.Black; //Color.PaleGreen;
                            secondForm.POS_Messages.AppendText(Langtemp);

                            Image im;   // = Image.FromFile("C:/POS/Discount1.jpg");
                            //secondForm.ReaderPictureBox.BackgroundImage = im;
                            //secondForm.ReaderPictureBox.Visible = true;                            
                            secondForm.Refresh();
                        }
                        else
                        {
                            UserHasCpn = false;
                        }
                        secondForm.btnYN = 0;                        
                    }
                    break;

                case 23:
                    Display("SM: " + SM);
                    if (RF_Card.Length != 0)
                    {                       
                        SM = 31; secondForm.Vprogress.Visible = false; break;
                    }

                    if (BR_Card.Length != 0)
                    {
                        if (BR_Card.Length != CPN_Length)//BC_Lenght
                        {
                            SM = 31; secondForm.Vprogress.Visible = false; break;
                        }
                        secondForm.Messages2.Visible = true;
                        UserCoupon = BR_Card;
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();
                        SM = 4; BR_Card = ""; RF_Card = ""; //3
                    }                    

                    break;

                    /*
                case 3://WAITING FOR TICKET
                    Display("SM: " + SM);
                    //secondForm.BackColor = Color.Silver;    //Black
                    //secondForm.Cancel.BackColor = Color.Red;

                    InitalCost = 0;
                    Payment = 0;
                    ReturnMoney = 0;

                    //Image image;  // = Image.FromFile("C:/POS/ticket.jpg");
                    //secondForm.ReaderPictureBox.BackgroundImage = image;
                    //secondForm.ReaderPictureBox.Visible = true;
                    secondForm.Ticket_Icon.Visible = true;

                    Langtemp = ini.IniReadValue("LANGUAGE", "insert" + secondForm.Language);
                    if (secondForm.POS_Messages.Text.IndexOf(Langtemp) != 0)
                    {
                        secondForm.POS_Messages.Clear();
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                    }            
                    if (BR_Card.Length != 0){
                        if (BR_Card.Length != BC_Lenght){
                            SM = 31; secondForm.Vprogress.Visible = false; break;
                        }
                        secondForm.Messages2.Visible = false;
                        UserCode = BR_Card;
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false; 
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();                        
                        SM = 4; BR_Card = ""; RF_Card = ""; //4    // REPLACE TO SM = 22 FOR COUPON USE!!!
                    }
                    if (RF_Card.Length != 0){
                        if (RF_Card == "8172635445362718")
                        {
                            secondForm.Close();
                            SM = 31; secondForm.Vprogress.Visible = false; break;
                        }
                        if (RF_Card.Length != RFID_Lenght){
                            SM = 31; secondForm.Vprogress.Visible = false;  break;
                        }
                        secondForm.Messages2.Visible = true;
                        UserCode = RF_Card;
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        //secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();                        
                        SM = 41; BR_Card = ""; RF_Card = "";
                    } 
                    break;
                    */
                case 31:
                    Display("SM: " + SM);
                    secondForm.POS_Messages.Clear();
                    InitalCost = 0;
                    Payment = 0;
                    ReturnMoney = 0;
                    Langtemp = ini.IniReadValue("LANGUAGE", "Try" + secondForm.Language);
                    //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                    secondForm.POS_Messages.Text = Langtemp;
                    LanguageTimer.Stop();
                    secondForm.Vprogress.Maximum = 120;
                    secondForm.Vprogress.Value = 120;
                    GeneralCounter = 120;
                    LanguageTimer.Start();
                    //secondForm.POS_Messages.Clear(); 
                    secondForm.Refresh();
                    Thread.Sleep(2500);
                    secondForm.POS_Messages.Clear();
                    BR_Card = ""; RF_Card = "";
                    SM = 1;  //SM = 2;                    
                    break;

                case 4://SHOW TICKET AND SEND #TIC PACKET TO SERVER       BARCODE
                    Display("SM: " + SM);
                    secondForm.Messages2.Clear();
                    secondForm.Ticket_Icon.Visible = false; 
                    secondForm.Messages2.Visible = true;
                    
                    
                    if (UserCode.Length != 0)
                    {
                        secondForm.Messages2.AppendText("\n TICKET: " + UserCode);
                        //TCP
                        TCP_Data = "";
                        TCP_Send("#TIC@" + UserCode + "$"); //PIC
                        //UserCode = "";
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;                        
                        LanguageTimer.Start();
                        SM = 5;                        
                        secondForm.POS_Messages.Clear();
                    }
                    break;

                case 41://SHOW CARD AND SEND #CARD PACKET TO SERVER     RFID
                    Display("SM: " + SM);
                    secondForm.Messages2.Clear();
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Messages2.Visible = true;
                    
                    if (UserCode.Length != 0){
                        secondForm.Messages2.AppendText("\nCARD: " + UserCode); Refresh();
                        //TCP
                        TCP_Data = "";
                        TCP_Send("#CARD@" + UserCode + "$");                        
                        UserCoupon = "";                        
                        //UserCode = "";
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;                        
                        LanguageTimer.Start();
                        SM = 51;
                        secondForm.POS_Messages.Clear();
                    }
                    break;                

                case 5://GET VALUE from SERVER AND START PAYMENT
                    Display("SM: " + SM);
                    //ShowNotes("05,10,20");
                    //#TIC@0@<value>@<time_start>@<time_end>@<plate_number>$
                    if (TCP_Data.StartsWith("#TIC@0") == true)
                    {
                        string[] packet = TCP_Data.Split('@');
                        try
                        {
                            type = packet[0];
                            state = packet[1];
                            value = packet[2];
                            tstart = packet[3];
                            tend = packet[4];
                            plate = packet[5];
                        }
                        catch (Exception ex) { value = "0"; }

                        //string end =    packet[6];
                        plate = plate.TrimEnd('\0', '$');
                        value = value.Trim('.');
                        InitalCost = Convert.ToInt32(value);
                        secondForm.Messages2.Clear();
                        
                        //value = Payment.ToString();
                        if (InitalCost != 0)//Convert.ToInt16(value)
                        {                            
                            if (value.Length > 2) { value = value.Insert(value.Length - 2, ","); }
                            else if (value.Length == 2) { value = "0," + value; }
                            else { value = "0,0" + value; }
                        }
                        else
                        {
                            value = "0,00";
                        }
                        value = value + "€";
                        secondForm.Messages2.AppendText("\n Customer: " + plate + "\n Entry: " + tstart + "\n Exit: " + tend + "\n Value: " + value);

                        /*if (InitalCost == 0) //User will not get receipt
                        {
                            Langtemp = ini.IniReadValue("LANGUAGE", "TktOK" + secondForm.Language);
                            secondForm.POS_Messages.ForeColor = Color.MediumSpringGreen;
                            //secondForm.POS_Messages.Clear();
                            secondForm.POS_Messages.Text = Langtemp;
                            LanguageTimer.Stop();
                            secondForm.Vprogress.Visible = false;
                            secondForm.Vprogress.Maximum = 30;
                            secondForm.Vprogress.Value = 30;
                            GeneralCounter = 30;//20  
                            LanguageTimer.Start();
                            secondForm.Refresh();
                            Thread.Sleep(5500);
                            TCP_Data = "";
                            TCP_Send("#TICOK@" + UserCode + "@" + InitalCost.ToString() + "$");
                            SM = 10;
                            break;
                        }*/
                        
                        if (UserHasCpn)
                        {
                            secondForm.Messages2.AppendText("\n Coupon: " + UserCoupon);
                            TCP_Data = "";
                            TCP_Send("#CPN@" + UserCoupon + "$");
                            UserCoupon = "";
                            SM = 52;
                            LanguageTimer.Stop();
                            secondForm.Vprogress.Visible = false;
                            secondForm.Vprogress.Maximum = 120;
                            secondForm.Vprogress.Value = 120;
                            GeneralCounter = 120;
                            LanguageTimer.Start();                            
                            break;
                        }
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.ReaderPictureBox.Visible = true;
                        secondForm.ValueText.Text = value; //secondForm.ValueText.AppendText
                        secondForm.ReaderPictureBox.Visible = true;
                        //secondForm.ValueText.ForeColor = Color.Black;   //Yellow
                        Langtemp = ini.IniReadValue("LANGUAGE", "pay" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //MediumSpringGreen
                        secondForm.POS_Messages.Clear();
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;//20 
                        SM = 6;
                        secondForm.Vprogress.Visible = true;
                        if (pm.set(0, 0, 0, 0) == 0)//ACCEPT COINS
                            Display("START PAYMENT_ACCEPT Coins");
                        if (serialNV.IsOpen && !value.Equals("0,00€"))
                        {
                            serialNV.Write(EnAll, 0, EnAll.Length); Thread.Sleep(100);
                        }
                        ShowNotes("05,10,20");
                        
                    }
                    else if(TCP_Data.StartsWith("#TIC@1") == true){
                        secondForm.POS_Messages.Clear();
                        secondForm.Ticket_Icon.Visible = false;
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC1" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        //secondForm.POS_Messages.AppendText("Ο αριθμός εισιτηρίου δεν είναι έγκυρος ");
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        secondForm.Refresh(); SM = 10;
                    }
                    else if (TCP_Data.StartsWith("#TIC@2") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        //secondForm.POS_Messages.AppendText("Δεν υπάρχει στο parking όχημα με αυτό τον αριθμό εισιτηρίου");
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        secondForm.Refresh(); SM = 10;
                    }
                    else if (TCP_Data.StartsWith("#TIC@3") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC3" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        //secondForm.POS_Messages.AppendText("Υπήρξε πρόβλημα κατά την ανάγνωση");
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        SM = 10;
                    }
                    else if (TCP_Data.StartsWith("#TIC@4") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC4" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        //secondForm.POS_Messages.AppendText("Το εισιτήριο έχει ήδη πληρωθεί");
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();

                        SM = 10;
                    }
                    else
                    {
                        Display("Did not get #TIC@ response from server");
                        SM = 10;
                    }
                    break;

                case 51://GET VALUE AND START PAYMENT CARD
                    Display("SM: " + SM);
                    //ShowNotes("05,10,20");
                    //#TIC@0@<value>@<time_start>@<time_end>@<plate_number>$
                    if (TCP_Data.StartsWith("#CARD@0") == true)
                    {
                        string[] packet = TCP_Data.Split('@');
                        try
                        {
                            type = packet[0];
                            state = packet[1];
                            value = packet[2];
                            tstart = packet[3];
                            tend = packet[4];
                            plate = packet[5];
                            //string end =    packet[6];
                        }
                        catch (Exception ex) { value = "0"; }

                        plate = plate.TrimEnd('\0', '$');
                        value = value.Trim('.');
                        InitalCost = Convert.ToInt32(value);
                        secondForm.Messages2.Clear();
                        secondForm.Ticket_Icon.Visible = false; 
                        if (InitalCost != 0)//Convert.ToInt16(value)
                        {
                            if (value.Length > 2) { value = value.Insert(value.Length - 2, ","); }
                            else if (value.Length == 2) { value = "0," + value; }
                            else { value = "0,0" + value; }
                        }
                        else
                        {
                            value = "0,00";
                        }
                        value = value + "€";
                        secondForm.Messages2.AppendText("\n Customer: " + plate + "\n\n Last Action:\n     " + tstart + "\n\n New Expiring Date:\n          " + tend + "\n\n Renew Value: " + value);
                        secondForm.ValueText.Text = value; //secondForm.ValueText.AppendText
                        secondForm.ReaderPictureBox.Visible = true;
                        //secondForm.ValueText.ForeColor = Color.Black;   //Yellow
                        Langtemp = ini.IniReadValue("LANGUAGE", "pay" + secondForm.Language);
                        secondForm.POS_Messages.Clear();
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //MediumSpringGreen
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;//20 
                        SM = 61;
                        secondForm.Vprogress.Visible = true;
                        if (pm.set(0, 0, 0, 0) == 0)//ACCEPT COINS
                            Display("START PAYMENT_ACCEPT Coins");
                        if (serialNV.IsOpen)
                        {
                            serialNV.Write(EnAll, 0, EnAll.Length); Thread.Sleep(100);
                        }
                        ShowNotes("05,10,20"); 
                    }
                    else if (TCP_Data.StartsWith("#CARD@1") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "CARD1" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        SM = 10;
                    }
                    else if (TCP_Data.StartsWith("#CARD@2") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "CARD2" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        SM = 10;
                    }
                    else if (TCP_Data.StartsWith("#CARD@3") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC3" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        SM = 10;
                    }
                    else if (TCP_Data.StartsWith("#CARD@4") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC4" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        SM = 10;
                    }
                    else if (TCP_Data.StartsWith("#CARD@5") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 10;
                    }
                    else if (TCP_Data.StartsWith("#CARD@6") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC6" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        
                        SM = 10;
                    }
                    break;

                case 52:
                    Display("SM: " + SM);
                    //Display("I 52\n" + TCP_Data.ToString() + "\n");
                    if (TCP_Data.StartsWith("#RCP@") == true)
                    {
                        //Display("I am IN 52\n" + TCP_Data.ToString() + "\n");
                        string[] packet = TCP_Data.Split('@');
                        try
                        {
                            value = packet[1];
                            freemin = packet[7];
                            freemin = freemin.TrimEnd('\0', '$');
                            value = value.Trim('.');
                            InitalCost = Convert.ToInt32(value);
                        }
                        catch (Exception ex) { value = "0"; }

                        if (InitalCost != 0)//Convert.ToInt16(value)
                        {
                            if (value.Length > 2) { value = value.Insert(value.Length - 2, ","); }
                            else if (value.Length == 2) { value = "0," + value; }
                            else { value = "0,0" + value; }
                        }
                        else
                        {
                            value = "0,00";
                        }
                        value = value + "€";
                        secondForm.Messages2.AppendText("\n\n New Value: " + value + "\n Free Minutes: " + freemin);
                        secondForm.Ticket_Icon.Visible = false;
                       /* if (InitalCost == 0) //User will not get receipt
                        {
                            Langtemp = ini.IniReadValue("LANGUAGE", "TktOK" + secondForm.Language);
                            secondForm.POS_Messages.ForeColor = Color.MediumSpringGreen;
                            //secondForm.POS_Messages.Clear();
                            secondForm.POS_Messages.Text = Langtemp;
                            LanguageTimer.Stop();
                            secondForm.Vprogress.Visible = false;
                            secondForm.Vprogress.Maximum = 30;
                            secondForm.Vprogress.Value = 30;
                            GeneralCounter = 30;//20  
                            LanguageTimer.Start();
                            secondForm.Refresh();
                            Thread.Sleep(5500);
                            TCP_Data = "";
                            TCP_Send("#TICOK@" + UserCode + "@" + InitalCost.ToString() + "$");
                            SM = 10;
                            break;
                        }*/

                        secondForm.ValueText.Text = value;
                        secondForm.ReaderPictureBox.Visible = true;
                        //secondForm.ValueText.ForeColor = Color.YellowGreen;
                        Langtemp = ini.IniReadValue("LANGUAGE", "pay" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //MediumSpringGreen
                        secondForm.POS_Messages.Clear();
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;//20 
                        SM = 6;
                        secondForm.Vprogress.Visible = true;
                        if (pm.set(0, 0, 0, 0) == 0)//ACCEPT COINS
                            Display("START PAYMENT_ACCEPT Coins");
                        if (serialNV.IsOpen)
                        {
                            serialNV.Write(EnAll, 0, EnAll.Length); Thread.Sleep(100);
                        }
                        ShowNotes("05,10,20");

                    }
                    else if (TCP_Data.StartsWith("#CPN@0") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "CPN0" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        secondForm.Messages2.Visible = false;
                        secondForm.Refresh(); SM = 31;

                    }
                    else if (TCP_Data.StartsWith("#CPN@1") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "CPN1" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(4000);
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        GeneralCounter = 120;
                        LanguageTimer.Start();
                        secondForm.Messages2.Visible = false;
                        secondForm.Refresh(); SM = 31;
                    }                    
                    break;

                case 6://ACCEPT COINS AND NOTES ticket
                    Display("SM: " + SM);
                    string value1 = "";
                    //secondForm.Vprogress.Maximum = 30;
                    //GeneralCounter = 30;
                    //secondForm.Vprogress.Visible = true;
                    secondForm.Ticket_Icon.Visible = false;

                    if (Payment == InitalCost){ //AKRIBWS
                        ReturnMoney = 0;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.Vprogress.Visible = false;
                        //secondForm.ValueText.ForeColor = Color.PaleGreen;
                        secondForm.ValueText.AppendText("0,00€");
                        SM = 7; Display("Payment OK"); Thread.Sleep(100);
                        //Payment = 0;
                        if (pm.set(0, 1, 0, 0) != 0){
                            Display("Disable Coin ERROR 6\n"); SM = 11; ShowNotes("000");
                            TCP_Send("ERROR: Disable Coin ERROR 6"); break;
                        }                        
                        //#TICOK@<ticket_number>@<value>$
                        //value = value.Trim('€');
                        //if (value.IndexOf(",") == 0) value = "0" + value;//BUG otan value <= 0.05
                        //int c = (int)(Convert.ToDouble(value));
                        //value = c.ToString(); 
                        value = "0,00";
                        TCP_Data = "";
                        //ShowNotes("000");
                        TCP_Send("#TICOK@" + UserCode + "@" + InitalCost.ToString() + "$");
                        break;
                    }

                    if (InitalCost-Payment< 0){ // RESTA 
                        ReturnMoney = 0;
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.Vprogress.Visible = false;
                        //secondForm.ValueText.ForeColor = Color.Aquamarine;
                        string TempPayment = Math.Abs(InitalCost - Payment).ToString();                            

                        if (TempPayment.Length > 2) { TempPayment = TempPayment.Insert(TempPayment.Length - 2, ","); }
                        else if (TempPayment.Length == 2) { TempPayment = "0," + TempPayment; }
                        else { TempPayment = "0,0" + TempPayment; }

                        TempPayment = TempPayment + "€";

                        secondForm.ValueText.AppendText(TempPayment);
                        Langtemp = ini.IniReadValue("LANGUAGE", "change" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Lavender
                        secondForm.POS_Messages.Clear();
                        //secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); //Thread.Sleep(1000);

                        if ((Math.Abs(InitalCost - Payment) / 500) > 0)
                        {
                            secondForm.Ticket_Icon.Visible = false;
                            GeneralTimer.Stop();
                            int x = ReturnNotes(Math.Abs(InitalCost - Payment) / 500);
                            //Thread.Sleep(1000); //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                            //if (x > 0) { ReturnMoney = ReturnMoney + (x * 500); }                            
                            if (x == (Math.Abs(InitalCost - Payment) / 500))
                            {
                                Display("Correctly Returned Notes:" + x.ToString() + " " + ReturnMoney.ToString());                                
                                //value = ReturnMoney.ToString();
                            }else {
                                Display("FAILURE Returned Notes:" + x.ToString() + " " + ReturnMoney.ToString());
                                //if (x > 0) { ReturnMoney = ReturnMoney + (x * 500); }
                                if (Math.Abs(InitalCost - Payment) > (Total_Coins - 1000)) // /2
                                {
                                    Display("OUT OF AMMO\n");
                                    ShowNotes("000");
                                    Display("ERROR_INCOMPLETE Pay: " + (InitalCost - Payment).ToString() + "\n");
                                    SM = 80; TCP_Send("ERROR: Out Of Money");
                                    value = (InitalCost - Payment).ToString();
                                    TCP_Data = "";
                                    TCP_Send("#TICOK@" + UserCode + "@" + InitalCost.ToString() + "$");//value + "$");
                                    secondForm.POS_Messages.Clear();
                                    Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                                    //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                                    secondForm.POS_Messages.AppendText(Langtemp);
                                    secondForm.Refresh();
                                    GeneralTimer.Start();
                                    /*int l = 10;
                                    while (l > 0)
                                    {
                                        if (TCP_Data.StartsWith("#TICOK@0") == true)
                                        {
                                            TCP_Data = TCP_Data.Trim();
                                            string[] packet = TCP_Data.Split('@');
                                            try
                                            {
                                                value = packet[2];
                                                tstart = packet[3];
                                                tend = packet[4];
                                                plate = packet[5];
                                                receipt = packet[7] + packet[6];
                                                ticket = packet[8].TrimEnd('$');
                                            }
                                            catch (Exception ex) { receipt = "--"; }
                                            break;
                                        }
                                        l--;
                                        Thread.Sleep(1000);
                                    }
                                    PrintReceipt_INCOMPLETE("@" + InitalCost.ToString() + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");
                                    Thread.Sleep(2000); */                                  
                                    break;
                                }                                
                            }
                            GeneralTimer.Start();                            
                        }

                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();

                        Display("Inital " + InitalCost.ToString() + "\n");
                        Display("Payment " + Payment.ToString() + "\n");
                        Display("Return " + ReturnMoney.ToString() + "\n");

                        int res = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                        //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                        ReturnMoney = ReturnMoney + res;

                        if ((InitalCost - Payment + ReturnMoney) < 0)
                        {
                            res = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                            //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                            ReturnMoney = ReturnMoney + res;
                        }

                        if ((InitalCost - Payment + ReturnMoney) >= -5)
                        {//DINW RESTA KERMATA
                            secondForm.Ticket_Icon.Visible = false;
                            //ReturnMoney = ReturnMoney + res;
                            Display("\nCoin Change OK: " + res+"\n");                            
                            SM = 7;
                            //value = value.Trim('€');
                            //if (value.IndexOf(",") == 0) value = "0" + value;//BUG otan value <= 0.05
                            //int c = (int)(Convert.ToDouble(value));
                            //value = c.ToString(); 
                            TCP_Data = "";
                            TCP_Send("#TICOK@" + UserCode + "@" + InitalCost.ToString() + "$");
                        }else {//DEN MPORW NA DWSW RESTA
                            secondForm.Ticket_Icon.Visible = false;
                            //ReturnMoney = ReturnMoney + res;                            
                            Display("ERROR_INCOMPLETE Pay: " + ReturnMoney.ToString() + "\n");
                            
                            //SM = 7; //Payment = 0;
                            SM = 80; TCP_Send("ERROR: Out Of Money");
                            //value = value.Trim('€');
                            value = (InitalCost - Payment + ReturnMoney).ToString();
                            Display("ERROR_INCOMPLETE value: " + value + "\n");
                            TCP_Data = "";
                            TCP_Send("#TICOK@" + UserCode + "@" + InitalCost.ToString() + "$"); //value + "$");
                            secondForm.POS_Messages.Clear();
                            Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                            //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                            secondForm.POS_Messages.AppendText(Langtemp);
                            secondForm.Refresh();
                            if (pm.set(0, 1, 0, 0) != 0)
                            {
                                Display("\nDisable Coins ERROR "); SM = 11; ShowNotes("000");
                                TCP_Send("ERROR: Coin Machine NOT Responding"); break;
                            }
                            /*int l = 10;
                            while (l > 0)
                            {
                                if (TCP_Data.StartsWith("#TICOK@0") == true)
                                {
                                    TCP_Data = TCP_Data.Trim();
                                    string[] packet = TCP_Data.Split('@');
                                    try
                                    {
                                        value = packet[2];
                                        tstart = packet[3];
                                        tend = packet[4];
                                        plate = packet[5];
                                        receipt = packet[7] + packet[6];
                                        ticket = packet[8].TrimEnd('$');
                                    }
                                    catch (Exception ex) { receipt = "--"; }
                                    break;
                                }
                                l--;
                                Thread.Sleep(1000);
                            }
                            PrintReceipt_INCOMPLETE("@" + InitalCost.ToString() + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");                                                        
                            Thread.Sleep(2000);*/
                           
                        }

                        if (pm.set(0, 1, 0, 0) != 0){
                            Display("\nDisable Coins ERROR "); SM = 11; ShowNotes("000");
                            TCP_Send("ERROR: Coin Machine NOT Responding"); break;
                        }
                        //ShowNotes("000");
                        break;
                    }

                    value1 = (InitalCost-Payment+ReturnMoney).ToString();
                    if ((InitalCost - Payment+ReturnMoney) < 10)
                        value1 = "0,0"+value1;
                    else if (((InitalCost - Payment + ReturnMoney) > 10) && ((InitalCost - Payment+ ReturnMoney) <= 99))
                        value1 = "0,"+value1;
                    else
                        value1 = value1.Insert(value1.Length - 2, ",");

                    value1 = value1 + "€";
                    secondForm.ValueText.Clear();
                    secondForm.ValueText.AppendText(value1);
                    break;

                case 61://CARD ACCEPT COINS AND NO---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------TES 
                    Display("SM: " + SM);
                    value1 = "";
                    //secondForm.Vprogress.Maximum = 30;
                    //GeneralCounter = 30;
                    //secondForm.Vprogress.Visible = true;

                    if (Payment == InitalCost)
                    { //AKRIBWS
                        ReturnMoney = 0;
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.Vprogress.Visible = false;
                        //secondForm.ValueText.ForeColor = Color.PaleGreen;
                        secondForm.ValueText.AppendText("0,00€");
                        SM = 71; Display("Payment OK"); Thread.Sleep(100);
                        //Payment = 0;
                        if (pm.set(0, 1, 0, 0) != 0)
                        {
                            Display("Disable Coin ERROR 6\n"); SM = 11; ShowNotes("000");
                            TCP_Send("ERROR: Disable Coin ERROR 6"); break;
                        }
                        //#TICOK@<ticket_number>@<value>$
                        //value = value.Trim('€');
                        //if (value.IndexOf(",") == 0) value = "0" + value;//BUG otan value <= 0.05
                        //int c = (int)(Convert.ToDouble(value));
                        //value = c.ToString(); 
                        value = "0,00";
                        TCP_Data = "";
                        //ShowNotes("000");
                        TCP_Send("#CARDOK@" + UserCode + "@" + InitalCost.ToString() + "$");
                        break;
                    }

                    if (InitalCost - Payment < 0)
                    { // RESTA 
                        ReturnMoney = 0;
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.Vprogress.Visible = false;
                        //secondForm.ValueText.ForeColor = Color.Aquamarine;
                        string TempPayment = Math.Abs(InitalCost - Payment).ToString();
                        
                        if (TempPayment.Length > 2) { TempPayment = TempPayment.Insert(TempPayment.Length - 2, ","); }
                        else if (TempPayment.Length == 2) { TempPayment = "0," + TempPayment; }
                        else { TempPayment = "0,0" + TempPayment; }

                        TempPayment = TempPayment + "€";

                        secondForm.ValueText.AppendText(TempPayment);
                        Langtemp = ini.IniReadValue("LANGUAGE", "change" + secondForm.Language);
                        secondForm.POS_Messages.Clear();
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Lavender
                        //secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); //Thread.Sleep(100);//1000

                        if ((Math.Abs(InitalCost - Payment) / 500) > 0)
                        {
                            secondForm.Ticket_Icon.Visible = false; 
                            GeneralTimer.Stop();
                            int x = ReturnNotes(Math.Abs(InitalCost - Payment) / 500);
                            //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                            //if (x > 0) { ReturnMoney = ReturnMoney + (x * 500); }
                            if (x == (Math.Abs(InitalCost - Payment) / 500))
                            {
                                Display("Correctly Returned Notes:" + x.ToString() + " " + ReturnMoney.ToString());
                                //value = ReturnMoney.ToString();
                            }
                            else
                            {
                                Display("FAILURE Returned Notes:" + x.ToString() + " " + ReturnMoney.ToString());
                                //if (x > 0) { ReturnMoney = ReturnMoney + (x * 500); }
                                if (Math.Abs(InitalCost - Payment) > (Total_Coins - 1000)) // /2
                                {
                                    Display("OUT OF AMMO\n");
                                    ShowNotes("000");
                                    Display("ERROR_INCOMPLETE Pay: " + (InitalCost - Payment).ToString() + "\n");
                                    SM = 81; TCP_Send("ERROR: Out Of Money");
                                    value = (InitalCost - Payment).ToString();
                                    TCP_Data = "";
                                    TCP_Send("#CARDOK@" + UserCode + "@" + InitalCost.ToString() + "$"); //value + "$");
                                    secondForm.POS_Messages.Clear();
                                    Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                                    //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                                    secondForm.POS_Messages.AppendText(Langtemp);
                                    secondForm.Refresh();
                                    GeneralTimer.Start();
                                    if (pm.set(0, 1, 0, 0) != 0)
                                    {
                                        Display("\nDisable Coins ERROR "); SM = 11; ShowNotes("000");
                                        TCP_Send("ERROR: Coin Machine NOT Responding"); break;
                                    }
                                    /*int l = 10;
                                    while (l > 0)
                                    {
                                        if (TCP_Data.StartsWith("#CARDOK@0") == true)
                                        {
                                            TCP_Data = TCP_Data.Trim();
                                            string[] packet = TCP_Data.Split('@');
                                            try
                                            {
                                                value = packet[2];
                                                tstart = packet[3];
                                                tend = packet[4];
                                                plate = packet[5];
                                                receipt = packet[7] + packet[6];
                                                ticket = packet[8].TrimEnd('$');
                                            }
                                            catch (Exception ex) { receipt = "--"; }
                                            break;
                                        }
                                        l--;
                                        Thread.Sleep(1000);
                                    }                                    
                                    PrintReceipt_INCOMPLETE("@" + InitalCost.ToString() + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");
                                    Thread.Sleep(2000); */                                  
                                    break;
                                }
                            }
                            GeneralTimer.Start();
                        }

                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();

                        Display("Inital " + InitalCost.ToString() + "\n");
                        Display("Payment " + Payment.ToString() + "\n");
                        Display("Return " + ReturnMoney.ToString() + "\n");
                        //int res = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                        //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                        
                        //if ((res != Math.Abs(InitalCost - Payment + ReturnMoney)) && res > 0)
                        //{
                        //    res = res + pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney + res), 0);
                        //    Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                        //
                        //}
                        
                        //if ((res == Math.Abs(InitalCost - Payment + ReturnMoney)) || (Math.Abs(InitalCost - Payment + ReturnMoney) - res <= 5))

                        int res = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                        //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                        ReturnMoney = ReturnMoney + res;

                        if ((InitalCost - Payment + ReturnMoney) < 0)
                        {
                            res = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                            //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                            ReturnMoney = ReturnMoney + res;
                        }

                        if ((InitalCost - Payment + ReturnMoney) >= -5)
                        {//DINW RESTA KERMATA
                            //ReturnMoney = ReturnMoney + res;
                            Display("\nCoin Change OK: " + res + "\n");
                            secondForm.Ticket_Icon.Visible = false;
                            SM = 71;
                            //value = value.Trim('€');
                            //if (value.IndexOf(",") == 0) value = "0" + value;//BUG otan value <= 0.05
                            //int c = (int)(Convert.ToDouble(value));
                            //value = c.ToString(); 
                            TCP_Data = "";
                            TCP_Send("#CARDOK@" + UserCode + "@" + InitalCost.ToString() + "$");
                        }
                        else
                        {//DEN MPORW NA DWSW RESTA
                            //ReturnMoney = ReturnMoney + res;
                            Display("ERROR_INCOMPLETE Pay: " + ReturnMoney.ToString() + "\n");
                            secondForm.Ticket_Icon.Visible = false;
                            //SM = 7; //Payment = 0;
                            SM = 81; TCP_Send("ERROR: Out Of Money");
                            //value = value.Trim('€');
                            value = (InitalCost - Payment + ReturnMoney).ToString();
                            Display("ERROR_INCOMPLETE value: " + value + "\n");
                            //TCP_Data = "";
                            //TCP_Send("#CARDOK@" + UserCode + "@" + value + "$");
                            //PrintReceipt_INCOMPLETE("@" + InitalCost.ToString() + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@"); 
                            //Thread.Sleep(1000);
                            //secondForm.POS_Messages.Clear();
                            //Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                            //secondForm.POS_Messages.ForeColor = Color.PaleTurquoise;
                            //secondForm.POS_Messages.AppendText(Langtemp);
                            //secondForm.Refresh();
                            //Thread.Sleep(2500);


                            TCP_Data = "";
                            TCP_Send("#CARDOK@" + UserCode + "@" + InitalCost.ToString() + "$"); //+ value + "$");
                            secondForm.POS_Messages.Clear();
                            Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                            //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                            secondForm.POS_Messages.AppendText(Langtemp);
                            secondForm.Refresh();
                            /*int l = 10;
                            while (l > 0)
                            {
                                if (TCP_Data.StartsWith("#CARDOK@0") == true)
                                {
                                    TCP_Data = TCP_Data.Trim();
                                    string[] packet = TCP_Data.Split('@');
                                    try
                                    {
                                        value = packet[2];
                                        tstart = packet[3];
                                        tend = packet[4];
                                        plate = packet[5];
                                        receipt = packet[7] + packet[6];
                                        ticket = packet[8].TrimEnd('$');
                                    }
                                    catch (Exception ex) { receipt = "--"; }
                                    break;
                                }
                                l--;
                                Thread.Sleep(1000);
                            }
                            PrintReceipt_INCOMPLETE("@" + InitalCost.ToString() + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");
                            Thread.Sleep(2000); */
                        }

                        if (pm.set(0, 1, 0, 0) != 0)
                        {
                            Display("\nDisable Coins ERROR "); SM = 11; ShowNotes("000");
                            TCP_Send("ERROR: Coin Machine NOT Responding"); break;
                        }
                        //ShowNotes("000");
                        break;
                    }

                    value1 = (InitalCost - Payment + ReturnMoney).ToString();
                    if ((InitalCost - Payment + ReturnMoney) < 10)
                        value1 = "0,0" + value1;
                    else if (((InitalCost - Payment + ReturnMoney) > 10) && ((InitalCost - Payment + ReturnMoney) <= 99))
                        value1 = "0," + value1;
                    else
                        value1 = value1.Insert(value1.Length - 2, ",");

                    value1 = value1 + "€";
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.ValueText.AppendText(value1);
                    break;

                    

                case 7://PERIMENW APODEIXH APO SERVER
                    Display("SM: " + SM);
                     
                    //secondForm.POS_Messages.Clear();
                    //Langtemp = ini.IniReadValue("LANGUAGE", "RECEIPT_" + secondForm.Language);
                    //secondForm.POS_Messages.ForeColor = Color.PaleTurquoise;
                    //secondForm.POS_Messages.AppendText(Langtemp);
                    secondForm.Refresh(); //Thread.Sleep(100); //1000                   
                    if (TCP_Data.StartsWith("#TICOK@0") == true)
                    {
                        TCP_Data = TCP_Data.Trim();
                        string[] packet = TCP_Data.Split('@');
                        try
                        {
                            value = packet[2];
                            tstart = packet[3];
                            tend = packet[4];
                            plate = packet[5];
                            receipt = packet[7] + packet[6];
                            ticket = packet[8].TrimEnd('$');
                        }
                        catch (Exception ex) { value = "0"; }

                        value = value.Trim('.');
                        Display("Value " + value.ToString());
                        PrintReceipt_OK("@" + value + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");               
                        //Thread.Sleep(1000);
                        Payment = 0;
                        TCP_Data = "";
                        secondForm.Vprogress.Visible = false;
                        secondForm.ValueText.Clear();
                        //secondForm.ValueText.ForeColor = Color.Yellow;
                        ShowNotes("000");
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); 
                        Thread.Sleep(2500);
                        SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#TICOK@1") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC1" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#TICOK@2") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#TICOK@3") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC3" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#TICOK@4") == true)
                    {
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC4" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }                    
                    break;

                case 71://PERIMENW APODEIXH APO SERVER CARD
                    Display("SM: " + SM);
                    //secondForm.Vprogress.Visible = false;
                    //secondForm.ValueText.Clear();
                    //secondForm.ValueText.ForeColor = Color.Yellow; 
                    //ShowNotes("000"); 
                    //secondForm.POS_Messages.Clear();
                    //Langtemp = ini.IniReadValue("LANGUAGE", "RECEIPT_" + secondForm.Language);
                    //secondForm.POS_Messages.ForeColor = Color.PaleTurquoise;
                    //secondForm.POS_Messages.AppendText(Langtemp);
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Refresh(); //Thread.Sleep(100);//1000
                    if (TCP_Data.StartsWith("#CARDOK@0") == true)
                    {
                        TCP_Data = TCP_Data.Trim();
                        string[] packet = TCP_Data.Split('@');
                        try
                        {
                            //receipt = packet[7] + packet[6];
                            //ticket = packet[8].TrimEnd('$');

                            value = packet[2];
                            tstart = packet[3];
                            tend = packet[4];
                            plate = packet[5];
                            receipt = packet[7] + packet[6];
                            ticket = packet[8].TrimEnd('$');
                        }
                        catch (Exception ex) { value = "0"; }

                        value = value.Trim('.');
                        Display("Value " + value.ToString());
                        PrintReceipt_OK("@" + value + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");
                        //Thread.Sleep(2000);
                        Payment = 0;
                        TCP_Data = "";
                        secondForm.Vprogress.Visible = false;
                        secondForm.ValueText.Clear();
                        //secondForm.ValueText.ForeColor = Color.Yellow;
                        ShowNotes("000");
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); 
                        Thread.Sleep(2500);
                        SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#CARDOK@1") == true)
                    {//AKYROS ARI8MOS EISHTHRIOU/POSO
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC1" + secondForm.Language); //CARDOK@1
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#CARDOK@2") == true)
                    {//DEN EINAI KATAXVRHMENH H KARTA
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language); //CARDOK@2
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#CARDOK@3") == true)
                    {//PROBLHMA DATABASE
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC3" + secondForm.Language); //CARDOK@3
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 10;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                                       
                    break;


                case 80:
                    Display("SM: " + SM);
                    secondForm.Refresh();
                    if (TCP_Data.StartsWith("#TICOK@0") == true)
                    {
                        TCP_Data = TCP_Data.Trim();
                        string[] packet = TCP_Data.Split('@');
                        try
                        {
                            value = packet[2];
                            tstart = packet[3];
                            tend = packet[4];
                            plate = packet[5];
                            receipt = packet[7] + packet[6];
                            ticket = packet[8].TrimEnd('$');
                        }
                        catch (Exception ex) { value = "0"; }

                        value = value.Trim('.');
                        Display("Value " + value.ToString()); 
                        PrintReceipt_INCOMPLETE("@" + InitalCost.ToString() + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");
                        //Thread.Sleep(1000);
                        Payment = 0;
                        TCP_Data = "";
                        secondForm.Vprogress.Visible = false;
                        secondForm.ValueText.Clear();
                        //secondForm.ValueText.ForeColor = Color.Yellow;
                        ShowNotes("000");
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(2500);
                        SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#TICOK@1") == true)
                    {
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC1" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#TICOK@2") == true)
                    {
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#TICOK@3") == true)
                    {
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC3" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#TICOK@4") == true)
                    {
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC4" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    } 
                    break;

                case 81:
                    Display("SM: " + SM);
                    secondForm.Refresh(); //Thread.Sleep(100);//1000
                    if (TCP_Data.StartsWith("#CARDOK@0") == true)
                    {
                        TCP_Data = TCP_Data.Trim();
                        string[] packet = TCP_Data.Split('@');
                        try
                        {
                            //receipt = packet[7] + packet[6];
                            //ticket = packet[8].TrimEnd('$');

                            value = packet[2];
                            tstart = packet[3];
                            tend = packet[4];
                            plate = packet[5];
                            receipt = packet[7] + packet[6];
                            ticket = packet[8].TrimEnd('$');
                        }
                        catch (Exception ex) { value = "0"; }

                        value = value.Trim('.');
                        Display("Value " + value.ToString());
                        //PrintReceipt_OK("@" + value + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");
                        PrintReceipt_INCOMPLETE("@" + InitalCost.ToString() + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@" + ReturnMoney.ToString() + "@");
                        //Thread.Sleep(2000);
                        Payment = 0;
                        TCP_Data = "";
                        secondForm.Vprogress.Visible = false;
                        secondForm.ValueText.Clear();
                        //secondForm.ValueText.ForeColor = Color.Yellow;
                        ShowNotes("000");
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //PaleTurquoise
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                        Thread.Sleep(2500);
                        SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#CARDOK@1") == true)
                    {//AKYROS ARI8MOS EISHTHRIOU/POSO
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC1" + secondForm.Language); //CARDOK@1
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#CARDOK@2") == true)
                    {//DEN EINAI KATAXVRHMENH H KARTA
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language); //CARDOK@2
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                    else if (TCP_Data.StartsWith("#CARDOK@3") == true)
                    {//PROBLHMA DATABASE
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "TIC3" + secondForm.Language); //CARDOK@3
                        //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh(); Thread.Sleep(4000); SM = 11;
                        //MoneyStatus(1);//MONEY UPDATE STON SERVER
                    }
                      
                    break;

                case 10://CANCEL
                    Display("SM: " + SM);
                    Display("End of transaction, or cancel button pressed");
                    BR_Card = ""; RF_Card = "";
                    ShowNotes("000");
                    secondForm.btnYes.Visible = false;
                    secondForm.btnNo.Visible = false;                    

                    secondForm.POS_Messages.Clear();
                    //secondForm.POS_Messages.ForeColor = Color.Black;    //LawnGreen
                    Langtemp = ini.IniReadValue("LANGUAGE", "Thank" + secondForm.Language);
                    secondForm.POS_Messages.AppendText(Langtemp);
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Cancel.Visible = false;
                    secondForm.Vprogress.Visible = false;
                    secondForm.Messages2.Visible = false;
                    secondForm.buttonUK.Visible = false;
                    secondForm.buttonGRE.Visible = false;
                    secondForm.buttonGER.Visible = false;
                    secondForm.buttonFRA.Visible = false;
                    secondForm.Vprogress.Maximum = 120;
                    secondForm.Vprogress.Value = 120;
                    GeneralCounter = 120;
                    //secondForm.BackColor = Color.Silver;    //Black
                    //secondForm.POS_Messages.BackColor = Color.Silver;   // Black
                    secondForm.LangPictureBox.Visible = false;
                    secondForm.ReaderPictureBox.Visible = false;
                    secondForm.Messages2.Visible = false;
                    
                    secondForm.Language = "";
                    SM = 1;
                    //secondForm.Cancel.Enabled = false;
                    secondForm.CancelButton = false;
                    //secondForm.Cancel.BackColor = Color.GreenYellow;
                    secondForm.ValueText.Clear();
                    //secondForm.ValueText.ForeColor = Color.Yellow;                                   
                    secondForm.Refresh();

                    if (pm.set(0, 1, 0, 0) != 0){
                        Display("Disable Coin ERROR 100\n");
                        secondForm.CancelButton = false; SM = 11;
                        TCP_Send("ERROR: Coin Machine NOT Responding"); break;
                    }
                    if (serialNV.IsOpen){
                        serialNV.Write(DisAll, 0, DisAll.Length); Thread.Sleep(100);
                    }

                    if (Payment > 0){//GIVE HIM HIS MONEY BACK TODO check if > or < zero
                        //TCP_Send("#TICOK$"); 
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        secondForm.POS_Messages.AppendText("WAIT\n");
                        secondForm.Refresh();
                        Thread.Sleep(500);
                        //InitalCost-

                        if (((Payment-ReturnMoney) / 500) > 0)
                        {//Prwth epafh me ND / 500 
                            secondForm.Ticket_Icon.Visible = false;
                            GeneralTimer.Stop();
                            int x = ReturnNotes((Payment - ReturnMoney) / 500); //  / 500                            
                            //Thread.Sleep(5000);
                            GeneralTimer.Start();
                            Display("Returned Notes:" + x.ToString());
                            //ReturnMoney = ReturnMoney + (x * 500);
                            //break;
                        }
                        int res=0;
                        if ((Payment - ReturnMoney) > 0)
                        {
                            Display("Go Return Coins\n");
                            res = pm.set(1, 0, (Payment - ReturnMoney), 0);
                            //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                            ReturnMoney = ReturnMoney + res;
                         
                            if ((Payment - ReturnMoney)>0)
                            {
                                res = pm.set(1, 0, (Payment - ReturnMoney), 0);
                                //Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000); Thread.Sleep(1000);
                                ReturnMoney = ReturnMoney + res;
                            }
                        }
                        //if (res == Math.Abs((Payment - ReturnMoney)))


                        if ((Payment - ReturnMoney) <= 5)
                        {
                            //ReturnMoney = ReturnMoney + res;
                            Display("Return Coin Change OK:" + ReturnMoney.ToString() + "\n");
                            SM = 1; 
                        }
                        else{
                            Display("INCOMPLETE Pay: " +Payment.ToString()+" "+ReturnMoney.ToString()+" "+ res.ToString()+"\n");
                        }
                    }
                    
                    //image = Image.FromFile("F:/Projects/POS/POS_v20/POS_v20/m25.bmp");
                    //secondForm.BackgroundImage = image;

                    if (pm.set(0, 1, 0, 0) != 0){
                        Display("Disable Coin ERROR 101\n"); secondForm.CancelButton = false;
                        SM = 11; TCP_Send("ERROR: Coin Machine NOT Responding"); break;
                    }

                    type = ""; state = ""; value = ""; tstart = ""; 
                    tend = ""; plate = ""; ticket = ""; receipt = "";
                    UserCode = "";
                    
                    MoneyStatus(1);//MONEY UPDATE STON SERVER
                    
                    if ((Total_Notes < 1000) || (Total_Coins < 1000)){
                        Display("Money Very Low");
                        SM = 11; TCP_Send("ERROR: Money Very Low");
                    }
                    secondForm.POS_Messages.Clear();
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Debugging.Clear();
                    });
                    secondForm.Refresh();
                    Thread.Sleep(200);
                    break;

                case 11: //SERIOUS ERROR
                    Display("SM: " + SM);
                    //secondForm.Vprogress.Maximum = 30;
                    //GeneralCounter = 30;//60                    
                    secondForm.Cancel.Visible = false;
                    Cursor.Show();
                    MoneyStatus(1);
                    p = false;
                    Refresh_Click(this, e);
                    LanguageTimer.Stop();
                    GeneralTimer.Stop();
                    ShowNotes("000");
                    pm.set(0, 1, 0, 0);                                                                                  
                    if (serialNV.IsOpen)
                    {
                        serialNV.Write(DisAll, 0, DisAll.Length);
                    }

                    secondForm.POS_Messages.Font = new Font("Arial", 14,FontStyle.Bold);
                    secondForm.POS_Messages.Clear();
                    Langtemp = ini.IniReadValue("LANGUAGE", "ERROR" + secondForm.Language);
                    //secondForm.POS_Messages.ForeColor = Color.Black;    //Coral
                    secondForm.POS_Messages.AppendText(Langtemp);
                    secondForm.Refresh(); 

                    Thread.Sleep(15000);
                    GeneralTimer.Interval = 1000;

                    secondForm.btnYes.Visible = false;
                    secondForm.btnNo.Visible = false;
                    secondForm.LangPictureBox.Visible = false; //secondForm.Refresh();
                    secondForm.ReaderPictureBox.Visible = false; //secondForm.Refresh();
                    secondForm.buttonUK.Visible = false; //secondForm.Refresh();
                    secondForm.buttonGRE.Visible = false; //secondForm.Refresh();
                    secondForm.buttonGER.Visible = false; //secondForm.Refresh();
                    secondForm.buttonFRA.Visible = false; //secondForm.Refresh();
                    secondForm.Vprogress.Visible = false; //secondForm.Refresh();
                    secondForm.ValueText.Visible = false; //secondForm.Refresh();
                    secondForm.POS_Messages.Visible = false; //secondForm.Refresh();
                    secondForm.Messages2.Visible = false;
                    secondForm.pictureBox1.Visible = false;
                    secondForm.richTextBox1.Visible = false;
                    secondForm.Ticket_Icon.Visible = false;

                    image = Image.FromFile("C:/POS/caution.png");
                    secondForm.BackgroundImage = image;
                    //GeneralTimer.Stop();
                    secondForm.Refresh();
                    //secondForm.Update();
                    SM = 12; GeneralTimer.Start();
                    break;

                case 12:
                    Display("SM: " + SM); 
                    secondForm.Refresh();
                    if (RF_Card.Length != 0 && RF_Card == "8172635445362718")
                    {
                        secondForm.Close();
                    }
                    BR_Card = ""; RF_Card = "";
                    break;
                default:
                    break;
                            
            }//switch            
        }

/**************************************************************************************/
    public void MoneyStatus(int state)
    {
        string CCoin5 = "";
        string CCoin10 = "";
        string CCoin20 = "";
        string CCoin50 = "";
        string CCoin100 = "";
        string CCoin200 = "";
        int temp = 0;

        /* #DENOM@<position>@<value>$
         * 0-5
         * 1-10
         * 2-20
         * 3-50
         * 4-100
         * 5-200
         * 6-500
         * 7-1000
         * 8-2000
         */
        if (m_clientSocket == null)
        {
            Display("Money status function exit with error\n");
            return;
        }
        if (state == 2)
        {
            TCP_Send("#DENOM@0@000005$"); Thread.Sleep(10);
            TCP_Send("#DENOM@1@000010$"); Thread.Sleep(10);
            TCP_Send("#DENOM@2@000020$"); Thread.Sleep(10);
            TCP_Send("#DENOM@3@000050$"); Thread.Sleep(10);
            TCP_Send("#DENOM@4@000100$"); Thread.Sleep(10);
            TCP_Send("#DENOM@5@000200$"); Thread.Sleep(10);
            TCP_Send("#DENOM@6@000500$"); Thread.Sleep(10);
            TCP_Send("#DENOM@7@001000$"); Thread.Sleep(10);
            TCP_Send("#DENOM@8@002000$"); Thread.Sleep(10);
        }

        for (int i = 0; i <= 5; i++)
        {
            temp = pm.set(2, 4, i, 0); Thread.Sleep(50);
            if (temp != -1)
            {
                Display(i.ToString() + ": " + temp.ToString());
                switch (i)
                {
                    case 0: CCoin5 = temp.ToString();
                        break;
                    case 1: CCoin10 = temp.ToString();
                        break;
                    case 2: CCoin20 = temp.ToString();
                        break;
                    case 3: CCoin50 = temp.ToString();
                        break;
                    case 4: CCoin100 = temp.ToString();
                        break;
                    case 5: CCoin200 = temp.ToString();
                        break;
                }
            }
            else
                Display("No Data\n");
        }

        string Avail10Notes = ini.IniReadValue("SerialND", "Avail10");
        if (Avail10Notes.Length == 0)
            Avail10Notes = "0";
        string Avail05Notes = ini.IniReadValue("SerialND", "Avail05");
        if (Avail05Notes.Length == 0)
            Avail05Notes = "0";
        Total_Notes = Convert.ToInt32(Avail10Notes) * 1000 + Convert.ToInt32(Avail05Notes) * 500;
        Paid20Notes.Text = ini.IniReadValue("SerialNV", "Paid20");
        Paid10Notes.Text = ini.IniReadValue("SerialNV", "Paid10");
        Paid05Notes.Text = ini.IniReadValue("SerialNV", "Paid05");

        if (state == 1)
        {
            TCP_Send("#CHAV@" + CCoin5 + "@" + CCoin10 + "@" + CCoin20 + "@" + CCoin50 + "@" + CCoin100 + "@" + CCoin200 + "@" + Avail05Notes + "@" + Avail10Notes + "@0$");
            Thread.Sleep(1000);
            TCP_Send("#CASH@" + CCoin5 + "@" + CCoin10 + "@" + CCoin20 + "@" + CCoin50 + "@" + CCoin100 + "@" + CCoin200 + "@" + Paid05Notes.Text + "@" + Paid10Notes.Text + "@" + Paid20Notes.Text + "$");
        }
        Total_Coins = pm.set(1, 3, 0, 0);//TOTAL COINS CMD
        string tmp1 = Total_Coins.ToString();
        if (tmp1.Length > 2)
        {
            tmp1 = tmp1.Insert(tmp1.Length - 2, ",");
        }        
        string tmp2 = Total_Notes.ToString();
        if (tmp2.Length > 2)
        {
            tmp2 = tmp2.Insert(tmp2.Length - 2, ",");
        }
        Display("TotalCoins:" + tmp1 + " TotalNotes:" + tmp2 + "\n");
        Display("Paid05Note: " + Paid05Notes.Text + " Paid10Note: " + Paid10Notes.Text + " Paid20Note: " + Paid20Notes.Text + "\n");

    }

/**************************************************************************************/
// DISPENCE THE 05 NOTES 
        public int ReturnNotes(int Notes){
            
            byte[] NDOneBill = { 0x01, 0x10, 0x00, 0x10, 0x01, 0x22 };
            int xount = 10;
            int NNotes = Notes;
            byte[] buffer = new byte[256];
            DNote = 0;
            GeneralTimer.Stop();
            try
            {
                while (Notes > 0)
                {
                    //Debugging.Clear();
                    serialPortND.Write(NDOneBill, 0, NDOneBill.Length); Thread.Sleep(3000);
                    xount = 10;
                    while (xount > 0)
                    {
                        xount--;                        
                        Display("Wait ND Serial data"); Debugging.Refresh();
                        if (DNote > 0)// buffer[3] == 0xAA)  Debugging.Text.IndexOf("ND Succesful Pay") != -1
                        {
                            DNote--;
                            Notes--;
                            ReturnMoney = ReturnMoney + 500;
                            //Payment = Payment + 500;
                            //Debugging.Clear();
                            //Debugging.Refresh();
                            this.Refresh();
                            secondForm.Refresh(); Thread.Sleep(1000);
                            string five = ini.IniReadValue("SerialND", "Avail05");
                            int temp = Convert.ToInt16(five); temp--;
                            ini.IniWriteValue("SerialND", "Avail05", temp.ToString());
                            break;
                        }
                        else
                        {
                            Thread.Sleep(2000);
                            this.Refresh();
                            Display("ND_ERROR\n");
                            //GeneralTimer.Start();
                            //return (NNotes - Notes);
                        }
                        if (xount <= 0) 
                        {
                            GeneralTimer.Start();
                            return (NNotes - Notes);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            if (Notes == 0)
            {
                GeneralTimer.Start();
                return NNotes;
            }
            
            Display("SERIOUS ERROR with ND\n");

            return 0;
    }

/**************************************************************************************/
    public void ShowNotes(string Notes) {
        
        if (Notes.IndexOf("05")!=-1)
            secondForm.pictureBox05.Visible = true;
        else
            secondForm.pictureBox05.Visible = false;
        if (Notes.IndexOf("10") != -1)
            secondForm.pictureBox10.Visible = true;
        else
            secondForm.pictureBox10.Visible = false;
        if (Notes.IndexOf("20") != -1)
            secondForm.pictureBox20.Visible = true;
        else
            secondForm.pictureBox20.Visible = false;
        secondForm.Refresh();
    }

/**************************************************************************************/
    public void PrintReceipt_OK(string print) {

        byte[] reset = { 0x1b, 0x40 };
        byte[] cut = { 0x1c, 0xc0, 0x34 }; // <- cut and move back -- //byte[] cut = { 0x1b, 0x69 };
        byte[] Bold = { 0x1b, 0x45, 3 };//2-No Bold
        byte[] Pos = { 0x1D, 0x4c, 20, 0 };//Gia na mhn tupwnei sthn akrh tou xartiou
        byte[] VeryLarge = { 0x1d, 0x21, 0x22 };
        byte[] Large = { 0x1d, 0x21, 0x01 };
        byte[] Normal = { 0x1d, 0x21, 0x00 };
        byte[] AbsPos = { 0x1b, 0x64, 0 };
        byte[] Status = { 0x10, 0x04, 0x04 };
        byte[] Euro = { 0x1b, 0x74, 0xff, 0x24 }; //byte[] Euro = { 0x80 };
        byte[] font_reset = { 0x1b, 0x74, 0x00 };
        
        if (!serialPRINTER.IsOpen)
            serialPRINTER.Open();
        else
            Thread.Sleep(1);

        string[] packet = print.Split('@');

        int FPA = Convert.ToInt16(ini.IniReadValue("LANGUAGE","FPA"));
        string temp = Convert.ToInt32(InitalCost).ToString();
        
        if (temp.Length > 2) { temp = temp.Insert(temp.Length - 2, ","); }
        else if (temp.Length == 2) { temp = "0," + temp; }
        else { temp = "0,0" + temp; }
        float AxiaFPA = (float)(Convert.ToDouble (temp) * FPA) / (100 + FPA);        
        float KatharhAxia = (float)Convert.ToDouble(temp) - AxiaFPA;



        serialPRINTER.Write(Status, 0, Status.Length); Thread.Sleep(500);      //1500      
        //PrintReceipt_OK("@"+value+"@"+tstart+"@"+tend+"@"+plate+"@"+receipt+"@"+ticket+"@"); 
        //"@" + value + "@" + tstart + "@" + tend + "@" + plate + "@" + receipt + "@" + ticket + "@" + Payment.ToString() + "@"
        serialPRINTER.Write(reset, 0, reset.Length); Thread.Sleep(100);
        serialPRINTER.Write(AbsPos, 0, AbsPos.Length); Thread.Sleep(100);
        serialPRINTER.Write(Bold, 0, Bold.Length); Thread.Sleep(100);
        serialPRINTER.Write(Pos, 0, Pos.Length); Thread.Sleep(100);        
        serialPRINTER.Write(Large, 0, Large.Length);
        GR("ΤΣΟΥΦΛΙΔΟΥ ΕΛΕΝΗ\n");
        //GR("ΔΗΜ.ΚΗ Α.Ε");
        //serialPRINTER.WriteLine("\n");
        serialPRINTER.Write(Normal, 0, Normal.Length); Thread.Sleep(100);

        GR("ΕΚΜΕΤΑΛΛΕΥΣΗ ΧΩΡΟΥ ΣΤΑΘΜΕΥΣΗΣ\n");
        GR("ΥΠΟΚ/ΜΑ: PARKING ΤΕΛΩΝΕΙΟΥ\n");
        //GR("ΠΕΡΙΔΟΥ & ΥΨΗΛΑΝΤΩΝ 73135\n");
        GR("Τ.Κ. 65500\n");
        
        //GR("ΥΠΗΡΕΣΙΕΣ ΣΤΑΘΜΕΥΣΗΣ\n");
        //GR("    ΑΥΤΟΚΙΝΗΤΩΝ    "); serialPRINTER.WriteLine(" ");

        GR("ΑΦΜ: 142212131, ΔΟΥ: ΚΑΒΑΛΑΣ\n");
        //GR("ΔΟΥ: ΧΑΝΙΩΝ\n");
        GR("ΤΗΛ: 6932 383139\n");

        GR("ΑΠΟΔΕΙΞΗ ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ\n");
        //GR("ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ");
        //GR("--------------------");
        GR("\nΑ.Α.ΑΠΟΔΕΙΞΗΣ ");

        string tmp ="";
        try
        {
            if ((byte)packet[5][0] == 145)
            {
                GR("Α");
                tmp = Convert.ToInt16((packet[5].Substring(1))).ToString("0000000000");
            }
            else if ((byte)packet[5][0] == 147)
            {
                GR("Γ");
                tmp = Convert.ToInt16((packet[5].Substring(1))).ToString("0000000000");
            }
            else
            {
                tmp = packet[5][0] + Convert.ToInt16((packet[5].Substring(1))).ToString("0000000000");
            }
        }
        catch (Exception ex) { }

        serialPRINTER.WriteLine(string.Format("{0}", tmp));

        GR("\nΑΡΙΘΜΟΣ ΚΥΚΛΟΦΟΡΙΑΣ: "); serialPRINTER.WriteLine(packet[4]);
        //GR("\nΑΡΙΘΜΟΣ ΕΙΣΙΤΗΡΙΟΥ: "); serialPRINTER.WriteLine(packet[6]);
        //GR("\n--------------------");
        GR("\nΑΞΙΑ ΕΙΣΙΤΗΡΙΟΥ: " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
        GR("\nΕΙΣΟΔΟΣ: "); serialPRINTER.WriteLine(packet[2]);
        GR("ΕΞΟΔΟΣ : "); serialPRINTER.WriteLine(packet[3]);
        //GR("\n--------------------");
        GR("\nΣΥΝΟΛΟ       " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);

        temp = (KatharhAxia).ToString(".00");
        if (temp.Length == 3) { temp = "0" + temp; }
        GR("\nΚΑΘΑΡΗ ΑΞΙΑ  " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);//(KatharhAxia).ToString(".00") + "€");

        temp = (AxiaFPA).ToString(".00");
        if (temp.Length == 3) { temp = "0" + temp; }
        GR("\nΦΠΑ          " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length); //(AxiaFPA).ToString(".00") + "€");

        temp = packet[7];
        if (temp.Length>2) temp = temp.Insert(temp.Length - 2, ",");
        else if (temp.Length == 2) { temp = "0," + temp; }
        else { temp = "0,0" + temp; }
        GR("\nΜΕΤΡΗΤΑ      " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
        temp = packet[8];
        if (temp.Length > 2) temp = temp.Insert(temp.Length - 2, ",");
        else if (temp.Length == 2) { temp = "0," + temp; }
        else { temp = "0,0" + temp; }
        GR("\nΡΕΣΤΑ        " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
        //GR("\n--------------------");
        GR("\nΓΙΑ ΤΟΝ ΠΕΛΑΤΗ");
        GR("\nΑΘΕΩΡΗΤΑ ΒΑΣΕΙ ΤΗΣ\nΠΟΛ. 1083/2003\n");
        GR("  ********************"); serialPRINTER.WriteLine(" ");
        GR("  *   ΕΥΧΑΡΙΣΤΟΥΜΕ   *"); serialPRINTER.WriteLine(" ");
        GR("  ********************"); serialPRINTER.WriteLine(" ");
        serialPRINTER.WriteLine("\n"); Thread.Sleep(100);
        serialPRINTER.Write(cut, 0, cut.Length);


    }

/**************************************************************************************/
    public void PrintReceipt_INCOMPLETE(string print){

        byte[] reset = { 0x1b, 0x40 };
        byte[] cut = { 0x1c, 0xc0, 0x34 }; // <- cut and move back -- //byte[] cut = { 0x1b, 0x69 };
        byte[] Bold = { 0x1b, 0x45, 3 };//2-No Bold
        byte[] Pos = { 0x1D, 0x4c, 20, 0 };//Gia na mhn tupwnei sthn akrh tou xartiou
        byte[] VeryLarge = { 0x1d, 0x21, 0x22 };
        byte[] Large = { 0x1d, 0x21, 0x01 };
        byte[] Normal = { 0x1d, 0x21, 0x00 };
        byte[] AbsPos = { 0x1b, 0x64, 0 };
        byte[] Status = { 0x10, 0x04, 0x04 };
        byte[] Euro = { 0x1b, 0x74, 0xff, 0x24 }; //byte[] Euro = { 0x80 };
        byte[] font_reset = { 0x1b, 0x74, 0x00 };

        if (!serialPRINTER.IsOpen)
            serialPRINTER.Open();
        else
            Thread.Sleep(1);

        string[] packet = print.Split('@');
        //string xvalue = packet[4].Trim('€');
        //string resta = packet[5].Trim('€');
        //resta = resta.TrimStart('-');
        string resta = Math.Abs(((float)(Convert.ToDouble(packet[1])) - (float)(Convert.ToDouble(packet[7])) + (float)(Convert.ToDouble(packet[8])))).ToString();
        if (resta.Length > 2) resta = resta.Insert(resta.Length - 2, ",");
        else if (resta.Length == 2) { resta = "0," + resta; }
        else { resta = "0,0" + resta; }

        int FPA = Convert.ToInt16(ini.IniReadValue("LANGUAGE", "FPA"));
        string temp = packet[1];
        if (temp.Length > 2) { temp = temp.Insert(temp.Length - 2, ","); }
        else if (temp.Length == 2) { temp = "0," + temp; }
        else { temp = "0,0" + temp; }
        float AxiaFPA = (float)(Convert.ToDouble(temp) * FPA) / (100 + FPA);
        float KatharhAxia = (float)Convert.ToDouble(temp) - AxiaFPA;

        serialPRINTER.Write(Status, 0, Status.Length); Thread.Sleep(500);   //1500
        //PrintReceipt_OK("@"+value+"@"+tstart+"@"+tend+"@"+plate+"@"+receipt+"@"+ticket+"@"); 
        serialPRINTER.Write(Bold, 0, Bold.Length); Thread.Sleep(50);
        serialPRINTER.Write(Large, 0, Large.Length); Thread.Sleep(50);
        serialPRINTER.Write(Pos, 0, Pos.Length); Thread.Sleep(50);
        serialPRINTER.Write(AbsPos, 0, AbsPos.Length); Thread.Sleep(50);

        GR("ΤΣΟΥΦΛΙΔΟΥ ΕΛΕΝΗ\n");
        //GR("ΔΗΜ.ΚΗ Α.Ε");
        //serialPRINTER.WriteLine("\n");
        serialPRINTER.Write(Normal, 0, Normal.Length); Thread.Sleep(100);

        GR("ΕΚΜΕΤΑΛΛΕΥΣΗ ΧΩΡΟΥ ΣΤΑΘΜΕΥΣΗΣ\n");
        //GR("ΣΤΑΘΜΟΥ ΑΥΤΟΚΙΝΗΤΩΝ\n");
        GR("ΥΠΟΚ/ΜΑ: PARKING ΤΕΛΩΝΕΙΟΥ\n");
        GR("Τ.Κ. 65500\n");

        //GR("ΥΠΗΡΕΣΙΕΣ ΣΤΑΘΜΕΥΣΗΣ\n");
        //GR("    ΑΥΤΟΚΙΝΗΤΩΝ    "); serialPRINTER.WriteLine(" ");

        GR("ΑΦΜ: 142212131, ΔΟΥ: ΚΑΒΑΛΑΣ\n");
        //GR("ΔΟΥ: ΧΑΝΙΩΝ\n");
        GR("ΤΗΛ: 6932 383139\n");

        GR("ΑΠΟΔΕΙΞΗ ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ\n");
        //GR("ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ");
        GR("--------------------");
        GR("\nΑ.Α.ΑΠΟΔΕΙΞΗΣ \n");

        string tmp = "";
        try
        {
            if ((byte)packet[5][0] == 145)
            {
                GR("Α ");
                tmp = Convert.ToInt16((packet[5].Substring(1))).ToString("0000000000");
            }
            else if ((byte)packet[5][0] == 147)
            {
                GR("Γ ");
                tmp = Convert.ToInt16((packet[5].Substring(1))).ToString("0000000000");
            }
            else
            {
                tmp = packet[5][0] + Convert.ToInt16((packet[5].Substring(1))).ToString("0000000000");
            }
        }
        catch (Exception ex) { }
        serialPRINTER.WriteLine(string.Format("{0}", tmp));

        GR("\nΑΡΙΘΜΟΣ ΚΥΚΛΟΦΟΡΙΑΣ:\n"); serialPRINTER.WriteLine(packet[4]);
        //GR("\nΑΡΙΘΜΟΣ ΕΙΣΙΤΗΡΙΟΥ: "); serialPRINTER.WriteLine(packet[6]);
        GR("\n--------------------");
        GR("\nΑΞΙΑ ΕΙΣΙΤΗΡΙΟΥ: " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
        GR("\nΕΙΣΟΔΟΣ:"); serialPRINTER.WriteLine(packet[2]);
        GR("\nΕΞΟΔΟΣ : "); serialPRINTER.WriteLine(packet[3]);
        GR("\n--------------------");
        GR("\nΣΥΝΟΛΟ:      " + temp + "€");

        temp = (KatharhAxia).ToString(".00");
        if (temp.Length == 3) { temp = "0" + temp; }
        GR("\nΚΑΘΑΡΗ ΑΞΙΑ  " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);//(KatharhAxia).ToString(".00") + "€");

        temp = (AxiaFPA).ToString(".00");
        if (temp.Length == 3) { temp = "0" + temp; }
        GR("\nΦΠΑ          " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length); //(AxiaFPA).ToString(".00") + "€");

        temp = packet[7];
        if (temp.Length > 2) temp = temp.Insert(temp.Length - 2, ",");
        else if (temp.Length == 2) { temp = "0," + temp; }
        else { temp = "0,0" + temp; }
        GR("\nΜΕΤΡΗΤΑ      " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
        temp = packet[8];
        if (temp.Length > 2) temp = temp.Insert(temp.Length - 2, ",");
        else if (temp.Length == 2) { temp = "0," + temp; }
        else { temp = "0,0" + temp; }
        GR("\nΡΕΣΤΑ:       " + temp/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
        GR("\n--------------------");
        GR("\n      ΠΑΡΑΚΑΛΩ     ");
        GR("\n    ΟΠΩΣ ΠΕΡΑΣΕΤΕ   ");
        GR("\n    ΑΠΟ ΤΟ ΤΑΜΕΙΟ   ");
        GR("\n ΩΣΤΕ ΝΑ ΠΑΡΑΛΑΒΕΤΕ");
        GR("\n    ΤΑ ΡΕΣΤΑ ΣΑΣ\n\n ΥΠΟΛΟΙΠΟ: " + resta/* + "€"*/); serialPRINTER.Write(Euro, 0, Euro.Length); serialPRINTER.Write(font_reset, 0, font_reset.Length);
        GR("\n--------------------");
        GR("\nΓΙΑ ΤΟΝ ΠΕΛΑΤΗ");
        GR("\nΑΘΕΩΡΗΤΑ ΒΑΣΕΙ ΤΗΣ\nΠΟΛ. 1083/2003\n");
        GR("********************"); serialPRINTER.WriteLine(" ");
        GR("*   ΕΥΧΑΡΙΣΤΟΥΜΕ   *"); serialPRINTER.WriteLine(" ");
        GR("********************"); serialPRINTER.WriteLine(" ");
        serialPRINTER.WriteLine("\n"); Thread.Sleep(100);
        serialPRINTER.Write(cut, 0, cut.Length);        
    }
     
/**************************************************************************************/
       public void GR(string GR) {

           byte[] dummy = { 0x80 };
           int GR_size = GR.Length;
           
           for(int i=0;i<GR_size;i++){
               switch (GR[i]) { 
                   /*case 'Α': dummy[0] = 193; serialPRINTER.Write(dummy,0,dummy.Length);break;
                   case 'Β': dummy[0] = 194; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Γ': dummy[0] = 195; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Δ': dummy[0] = 196; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ε': dummy[0] = 197; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ζ': dummy[0] = 198; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Η': dummy[0] = 199; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Θ': dummy[0] = 200; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ι': dummy[0] = 201; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Κ': dummy[0] = 202; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Λ': dummy[0] = 203; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Μ': dummy[0] = 204; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ν': dummy[0] = 205; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ξ': dummy[0] = 206; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ο': dummy[0] = 207; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Π': dummy[0] = 208; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ρ': dummy[0] = 209; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Σ': dummy[0] = 211; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Τ': dummy[0] = 212; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Υ': dummy[0] = 213; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Φ': dummy[0] = 214; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Χ': dummy[0] = 215; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ψ': dummy[0] = 216; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ω': dummy[0] = 217; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case '€': dummy[0] = 0x80; serialPRINTER.Write(dummy, 0, dummy.Length); break;*/


                   case 'Α': dummy[0] = 0x80; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Β': dummy[0] = 0x81; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Γ': dummy[0] = 0x82; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Δ': dummy[0] = 0x83; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ε': dummy[0] = 0x84; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ζ': dummy[0] = 0x85; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Η': dummy[0] = 0x86; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Θ': dummy[0] = 0x87; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ι': dummy[0] = 0x88; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Κ': dummy[0] = 0x89; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Λ': dummy[0] = 0x8A; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Μ': dummy[0] = 0x8B; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ν': dummy[0] = 0x8C; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ξ': dummy[0] = 0x8D; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ο': dummy[0] = 0x8E; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Π': dummy[0] = 0x8F; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ρ': dummy[0] = 0x90; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Σ': dummy[0] = 0x91; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Τ': dummy[0] = 0x92; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Υ': dummy[0] = 0x93; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Φ': dummy[0] = 0x94; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Χ': dummy[0] = 0x95; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ψ': dummy[0] = 0x96; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case 'Ω': dummy[0] = 0x97; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   case '€': dummy[0] = 0xB0; serialPRINTER.Write(dummy, 0, dummy.Length); break;
                   default: dummy[0] = (byte)GR[i]; /*Convert.ToByte(GR[i]);*//* Convert.ToByte('_'); */ serialPRINTER.Write(dummy, 0, dummy.Length); break;

               }//switch
           }//for
       }

       private void quitBtn_Click(object sender, EventArgs e)
       {
           DW.Abort();
           this.Close();   
       }       

/**************************************************************************************/
    }//public partial class Form1 : Form
}
