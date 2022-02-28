using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net.Security;
using Ini;
using System.Security.Cryptography.X509Certificates;

//using PC;
using System.IO;
using System.IO.Ports;
using System.Threading;
using NRI.PaymenentManager;

//Printer
using System.Drawing.Printing;
using Newtonsoft.Json.Linq;
using eSSP_example;

namespace POS_v20
{
    /// <summary>
    /// POS application version 3.7//
    /// new features: connection with web based server on Laravel Framework,
    /// mixed type of payments (cash & credit), replace NV9 USB+ validator with NV11 note recycler
    /// Note: if you are using x64 platform you must change project options to x86 to 
    /// handle coin recycler (currenza c^2) dll file.
    /// 03/05/2019 usb printing enchanment
    /// 17/02/2021 automatic start of application when the pc goes down
    /// </summary>
    public partial class Form1 : Form
    {
        static Form1 instance = null;
        /// <summary>
        /// Enchancement for usb printing
        /// </summary>
        private Font printFont;
        private StreamReader streamToPrint;
        /// <summary>
        /// end of
        /// </summary>
        string IniFilePath = "C:/POS/POS.ini"; 
        IniFile ini;
        string BC_Data = "";
        string RF_Card = "";
        string BR_Card = "";
        string HTTPURLTICKET = "";
        string HTTPURLCARD = "";
        string HTTPURLRENEW = "";
        string HTTPURL = "";
        string phpresponse = "";
        string MyIP = "";
        string ApiToken = "";
        string BatchTotalAmount = "";
        string BatchTotalTransactions = "";
        string RRN = "";
        string Auth_Code = "";
        string CardProdNumber = "";
        string CardHolderName = "";
        string CardProductName = "";
        string PaymentMethod = "";

        int SM = 1;
        
        Form2 secondForm = new Form2();
        
        public string UserCode;
        public string UserCoupon;
        int GeneralCounter = 120;

        // Variables for SmartPayout.
        bool Running = false;
        int pollTimer = 300; // timer in ms
        int reconnectionAttempts = 5;
        CPayout Payout; // the class used to interface with the validator
        bool FormSetup = false;
        frmPayoutByDenom payoutByDenomFrm;
        List<CheckBox> recycleBoxes = new List<CheckBox>();
        string FiveEuroNotesLevel = "";
        string TenEuroNotesLevel = "";
        string TwentyEuroNotesLevel = "";

        //int DNote = 0;
        int DNote10 = 0;
        int DNote5 = 0;
        int Total_Coins = 0;
        uint Total_Notes = 0;
        int RFID_Lenght = 0;
        int BC_Lenght = 0;
        int CPN_Length = 0;
        int NotesFive = 0;
        int NotesTen = 0;
        int NotesTwenty = 0;

        Boolean UserHasCpn = false;

        ///UX300 handling variables
        byte[] STX = { 0x02 };
        byte[] ETX = { 0x03 };
        byte[] LRC = { 0x00 };
        byte[] FieldSeparator = { 0x1C };
        byte[] SystemIdentification = new byte[32];
        byte[] TransactionType = new byte[2];
        byte[] OrigTransactionAmount = new byte[10];
        byte[] AccountNumber = new byte[22];
        byte[] ExpirationDate = new byte[4];
        byte[] ValidFrom = new byte[4];
        byte[] OrigReferenceNumber = new byte[4];
        byte[] AuthCode = new byte[6];
        byte[] CVV2 = new byte[3];
        byte[] ContractRoomNo = new byte[9];
        byte[] StartDate = new byte[8];
        byte[] ExtTerminalID = new byte[0];
        byte[] ExtMerchantID = new byte[0];
        byte[] SerialNumber = new byte[0];
        byte[] ExtReferenceNumber = new byte[16];
        byte[] ReceiptContent = new byte[32];
        byte[] PrepareReceiptTicket = { 0x00 };   //no printing receipt on unattended system
        byte[] Language = new byte[2];
        byte[] CurrencyCode = new byte[3];
        byte[] LoyaltyTransactionType = new byte[2];
        byte[] LoyaltyIdentificationMethod = new byte[22];
        byte[] OrigLoyaltyReferenceNumber = new byte[4];
        byte[] CashierID = new byte[4];
        byte[] Acknowledgement = new byte[2];
        byte[] BytestoSend = new byte[0];

        //TCP
        IAsyncResult m_result;
        public Socket m_clientSocket;
        public AsyncCallback m_pfnCallBack;
        string TCP_Data = "";
        int Payment = 0;
        int ReturnMoney = 0;
        int InitalCost = 0;
        int FiveCent = 0;
        int TenCent = 0;
        int TwentyCent = 0;
        int FiftyCent = 0;
        int OneHundredCent = 0;
        int TwoHundredCent = 0;
        string FileName = "C:/POS/log/POS_Log.txt";
        string type = "";
        string validto = "";
        string validfrom = "";
        string renewaldate = "";
        string duration = "";
        string durationtype = "";
        string state = "";
        string value = "";
        string tstart = "";
        string tend = "";
        string plate = "";
        string entered_at = "";
        string ticket = "";
        string receipt = "";
        string freemin = "";
        string charge = "";
        double charge_double = 0;
        string charge_until = "";
        string date_time_now = "";
        string invoice_number = "";
        string invoice_sequence = "";
        string exception = "";
        string is_paid = "";
        string customer = "";
        string subscriptionId = "";
        string snapshot = "";
        string TerminalCard = "";
        bool ticket_scanned = false;
        bool card_scanned = false;
        bool cardpresense = false;
        bool p = false;
        bool FiveCentIn = false;
        bool TenCentIn = false;
        bool TwentyCentIn = false;
        bool FiftyCentIn = false;
        bool OneHundredCentIn = false;
        bool TwoHundredCentIn = false;
        bool FiveEuroNoteIn = false;
        bool TenEuroNoteIn = false;
        bool TwentyEuroNoteIn = false;
        bool DisableChangeNotes = false;

        /// <summary>
        /// Following 3 lines added @18/09/2020 for automatic UX300 close batch every day after midnight
        /// </summary>
        bool isMidnight;
        bool batchIsClosed = false;
        DateTime A, B;

        static StreamWriter WRfile;

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
                    if (lParam == 16)
                    {
                        if (FiveCentIn)
                        {
                            Paid05Coins.Text = ini.IniReadValue("PaidCOINS", "5cents");
                            int five = int.Parse(Paid05Coins.Text);
                            five++;
                            ini.IniWriteValue("PaidCOINS", "5cents", five.ToString());
                            FiveCentIn = false;
                        }
                        else if (TenCentIn)
                        {
                            Paid10Coins.Text = ini.IniReadValue("PaidCOINS", "10cents");
                            int ten = int.Parse(Paid10Coins.Text);
                            ten++;
                            ini.IniWriteValue("PaidCOINS", "10cents", ten.ToString());
                            TenCentIn = false;
                        }
                        else if (TwentyCentIn)
                        {
                            Paid20Coins.Text = ini.IniReadValue("PaidCOINS", "20cents");
                            int twenty = int.Parse(Paid20Coins.Text);
                            twenty++;
                            ini.IniWriteValue("PaidCOINS", "20cents", twenty.ToString());
                            TwentyCentIn = false;
                        }
                        else if (FiftyCentIn)
                        {
                            Paid50Coins.Text = ini.IniReadValue("PaidCOINS", "50cents");
                            int fifty = int.Parse(Paid50Coins.Text);
                            fifty++;
                            ini.IniWriteValue("PaidCOINS", "50cents", fifty.ToString());
                            FiftyCentIn = false;
                        }
                        else if (OneHundredCentIn)
                        {
                            Paid100Coins.Text = ini.IniReadValue("PaidCOINS", "100cents");
                            int onehundred = int.Parse(Paid100Coins.Text);
                            onehundred++;
                            ini.IniWriteValue("PaidCOINS", "100cents", onehundred.ToString());
                            OneHundredCentIn = false;
                        }
                        else if (TwoHundredCentIn)
                        {
                            Paid200Coins.Text = ini.IniReadValue("PaidCOINS", "200cents");
                            int twohundred = int.Parse(Paid200Coins.Text);
                            twohundred++;
                            ini.IniWriteValue("PaidCOINS", "200cents", twohundred.ToString());
                            TwoHundredCentIn = false;
                        }
                    }
                    else if (lParam == 17)
                    {
                        if (FiveCentIn)
                            FiveCentIn = false;
                        else if (TenCentIn)
                            TenCentIn = false;
                        else if (TwentyCentIn)
                            TwentyCentIn = false;
                        else if (FiftyCentIn)
                            FiftyCentIn = false;
                        else if (OneHundredCentIn)
                            OneHundredCentIn = false;
                        else if (TwoHundredCentIn)
                            TwoHundredCentIn = false;
                    }
                    break;
                case 0x11://W17										// Coin accepted
                    Display("Coins Accepted" + "W" + wParam.ToString() + "L " + lParam.ToString()+"\n");
                    switch (lParam){
                        case 5:
                            Payment = Payment + 5;
                            GeneralCounter = 120;
                            FiveCentIn = true;
                            FiveCent++;
                            break;
                        case 10:
                            Payment = Payment + 10;
                            GeneralCounter = 120;
                            TenCentIn = true;
                            TenCent++;
                            break;
                        case 20:
                            Payment = Payment + 20;
                            GeneralCounter = 120;
                            TwentyCentIn = true;
                            TwentyCent++;
                            break;
                        case 50:
                            Payment = Payment + 50;
                            GeneralCounter = 120;
                            FiftyCentIn = true;
                            FiftyCent++;
                            break;
                        case 100:
                            Payment = Payment + 100;
                            GeneralCounter = 120;
                            OneHundredCentIn = true;
                            OneHundredCent++;
                            break;
                        case 200:
                            Payment = Payment + 200;
                            GeneralCounter = 120;
                            TwoHundredCentIn = true;
                            TwoHundredCent++;
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
                case 1:
                    Display("Coins Device Ready\n");
                    CoinStatus.Enabled = false;
                    CoinStatus.BackColor = Color.YellowGreen;
                    Coins.Text = "Coins_OK";
                    int tab = MainConfig.SelectedIndex;
                    MainConfig.SelectedIndex = tab + 1;
                    MainConfig.TabPages[6].Parent.Focus();
                    break;
                case 8:
                    Display("No Configuration file\n");
                    break;
            }
        }

        #endregion

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = pollTimer;
            this.Text = "POS Configuration";
            if (instance == null)
            {
                instance = this;
            }
            FileName = "C:/POS/log/" + DateTime.Now.ToString("ddMMyy") + "_POS_Log.txt";
            WRfile = new StreamWriter(FileName, true, Encoding.UTF8, 100);
            WRfile.AutoFlush = true;
            
            //Open Config File
            if (System.IO.File.Exists(IniFilePath) == true){
                ini = new IniFile(IniFilePath);
            }else{
                MessageBox.Show("Ini NOT Found\nFix Error and restart application");
                return;
            }            

            //Buttons Status
            UX300Status.Enabled = false;
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
            secondForm.cashButton.Visible = false;
            secondForm.creditButton.Visible = false;
            
            secondForm.Messages2.Visible = false;

            secondForm.Vprogress.Maximum = 120;

            BC_Lenght   = Convert.ToInt16(ini.IniReadValue("Params", "BC_Length"));
            CPN_Length = Convert.ToInt16(ini.IniReadValue("Params", "CPN_Length"));
            RFID_Lenght = Convert.ToInt16(ini.IniReadValue("Params", "RFID_Lenght"));
            HTTPURLTICKET = ini.IniReadValue("Params", "HTTPURLTICKET");
            HTTPURLCARD = ini.IniReadValue("Params", "HTTPURLCARD");
            HTTPURLRENEW = ini.IniReadValue("Params", "HTTPURLRENEW");
            MyIP = ini.IniReadValue("Params", "MyIP");
            ExtMerchantID = Encoding.ASCII.GetBytes(ini.IniReadValue("Params", "Ext_Merchant_ID"));
            ExtTerminalID = Encoding.ASCII.GetBytes(ini.IniReadValue("Params", "Ext_Terminal_ID"));
            SerialNumber = Encoding.ASCII.GetBytes(ini.IniReadValue("Params", "Serial_Number"));
            ApiToken = ini.IniReadValue("Params", "api_token");

            WindowState = FormWindowState.Maximized;

        }
        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
        public class Json
        {
            public string code { get; set; }
            public string data { get; set; }
            public string finalStatus { get; set; }
        }
    private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = 0;   //ev.MarginBounds.Left;
            float topMargin = 0;    // ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
        static private byte CalculateLRC(byte[] _PacketData, int PacketLength)
        {
            Byte _CheckSumByte = 0x00;
            for (int i = 0; i < PacketLength; i++)
                _CheckSumByte ^= _PacketData[i];
            return _CheckSumByte;
        }

/**************************************************************************************/
/**************************************************************************************/
//TCP Handling    
    #region TCP handling
    private string PHPHandling(string message)
    {
        //Here we will enter the data to send, just like if we where to go to a webpage and enter variables,
        string requestmethod = "POST";
        string postData = message;
        //The Byte Array that will be used for writing the data to the stream.
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //The URL of the webpage to send the data to.
        string URL = HTTPURL;
        //The type of content being send, this is almost always "application/x-www-form-urlencoded".
        string contenttype = "application/x-www-form-urlencoded";
        //What the server sends back:
        string responseFromServer = null;

        //Here we will create the WebRequest object, and enter the URL as soon as it is created.

        WebRequest request = WebRequest.Create(URL);
        //We also need a Stream:
        Stream dataStream;
        //...And a webResponce,
        WebResponse response;
        //don't forget the streamreader either!
        StreamReader reader;

            try
            {
                //We will need to set the method used to send the data.
                request.Method = requestmethod;
                //Then the contenttype:
                request.ContentType = contenttype;
                //request.Headers.Add("Authorization", "Bearer sMw29YjvBrjZQWIwFoexKCQHhAIfY59xKG2rMBxkQhbWi7u7YnrQVj2Z7HDT"); 
                //content length
                request.ContentLength = byteArray.Length;
                //ok, now get the request from the webRequest object, and put it into our Stream:
                dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                //Get the responce
                response = request.GetResponse();
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                //Open the responce stream:
                reader = new StreamReader(dataStream);
                //read the content into the responcefromserver string
                responseFromServer = reader.ReadToEnd();
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
                //Now, display the responce!
                this.Invoke((System.Threading.ThreadStart)delegate
                {
                    Display("Message to server:" + URL + postData);

                });
                //Done!
            }
            catch
            {
                SM = 11;
                Display("FATAL ERROR: Connection to server was lost");
            }
        return responseFromServer;
    }

        private void TCP_Connect_Click(object sender, EventArgs e)
        {

            string IpAddress = ini.IniReadValue("Params", "ServerIP");
            string IpPort = ini.IniReadValue("Params", "ServerPort");

            // Create the socket instance
            m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Set the remote IP address
            IPAddress ip = IPAddress.Parse(IpAddress);
            int iPortNo = Convert.ToInt16(IpPort);
            // Create the end point 
            IPEndPoint ipEnd = new IPEndPoint(ip, iPortNo);

            // Connect to the remote host
            try
            {
                m_clientSocket.Connect(ipEnd);
                if (m_clientSocket.Connected)
                {
                    //WaitForData();
                    Server_Connect.BackColor = Color.YellowGreen;
                    Server_Connect.Enabled = false;
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
            Server_Connect.BackColor = Color.YellowGreen;
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
                //TCP_Send("ERROR: Problem with connection");
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
            Server_Connect.BackColor = Color.Tomato;
        }
    }
    #endregion

/**************************************************************************************/
// TCP SEND FUNCTION
    public int TCP_Send(string HTTPHeader1, string send, string HTTPHeader2)
    {
        byte[] byDataHTTPHeader1;
        byte[] byDataSend;
        byte[] byDataHTTPHeader2;

        byDataHTTPHeader1 = Encoding.ASCII.GetBytes(HTTPHeader1);
        byDataSend = Encoding.ASCII.GetBytes(send);
        byDataHTTPHeader2 = Encoding.ASCII.GetBytes(HTTPHeader2);

        //m_clientSocket.
        if (send.Length == 0)
        {
            Display("Empty TCP data to send");
            return 0;
        }
        else if (!m_clientSocket.Connected)
        {
            Display("No Connection with Server");
            SM = 11;
            return 0;
        }
        else
        {
            int count = m_clientSocket.Send(byDataHTTPHeader1);
            int count1 = m_clientSocket.Send(byDataSend);
            int count2 = m_clientSocket.Send(byDataHTTPHeader2);
            Display("TCP_SEND:" + HTTPHeader1 + send + HTTPHeader2);
            Thread.Sleep(100);
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
        serialBR.Parity = Parity.None;
        serialBR.StopBits = StopBits.One;
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
                BRStatus.Enabled = false;
                BRStatus.BackColor = Color.YellowGreen;
                Display("Barcode Reader READY!\n");
                BarcodeReader.Text = "BarcodeReader_OK";
                Refresh();

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
                data = Encoding.UTF8.GetString(buffer);
                data = data.Trim('\0', '\n', ' ');
                BC_Data = "";
                BC_Data = BC_Data + data;
                Display("BC_Data SET: " + BC_Data + "\n");
                data = BC_Data;
                Display("BarCode_" + data.Length.ToString() + ": " + data + "\n");
                if (BR_Card == "") { BR_Card = data; }
            }
            catch
            {
                data = "---";
                Display("Barcode DATA ERROR: --- \n");
            }
            data = "";
        }

        /**************************************************************************************/
        /**************************************************************************************/
        //OPEN RFID PORT
        private void OpenRFID_Click_1(object sender, EventArgs e)
        {
            string Port = ini.IniReadValue("SerialRFID", "COM");

            Display("LOAD SETTINGS SerialRFID COM Port: " + Port + "\n");

            serialRFID.PortName = Port;
            serialRFID.BaudRate = 9600;
            serialRFID.DataBits = 8;
            serialRFID.Parity = Parity.None;
            serialRFID.StopBits = StopBits.One;
            serialRFID.Handshake = Handshake.None;

            try
            {
                serialRFID.Open();
                OpenRFID.BackColor = Color.GreenYellow;
                OpenRFID.Enabled = false;
                RFIDStatus.Enabled = true;
                Refresh();
            }
            catch
            {
                MessageBox.Show("SerialRFID Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
                Display("SerialRFID Error \n\nProblem While Opening Serial Port: " + Port + " \n");
                OpenRFID.BackColor = Color.Tomato;
                Refresh();
            }
            serialRFID.Close();
        }

        /**************************************************************************************/
        //RFID STATUS

        private void RFIDStatus_Click_1(object sender, EventArgs e)
        {
            byte[] reset = { 0x78 };//'x'

            if (!serialRFID.IsOpen)
                serialRFID.Open();
            else
                Thread.Sleep(1);

            Thread.Sleep(100);
            serialRFID.Write(reset, 0, reset.Length);
            Thread.Sleep(1000);
            serialRFID.Write(reset, 0, reset.Length);
        }

        /**************************************************************************************/
        //RECEIVE HANDLER FOR RFID

        private void ReceiveRFID(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] reset = { 0x78 };//'x'
            byte[] singleread = { 0x53 };//'S'
            string data = serialRFID.ReadLine();
            data = data.TrimEnd('\r');
            data = data.TrimEnd('\n');
            Display("Got RFID data: " + data + "\n");

            switch (data)
            {
                case "S":
                    Display("RFID Automatic reading NOT enabled\n");
                    Refresh();
                    break;
                case "N":
                    Display("No tag in range\n");
                    cardpresense = false;
                    break;
                case "MultiISO 1.0":
                    Display("RFID MultiISO 1.0 READY\n");
                    RFIDStatus.BackColor = Color.YellowGreen;
                    Thread.Sleep(1000);
                    int tab = MainConfig.SelectedIndex;
                    RFID.Text = "RFID_OK";
                    //MainConfig.SelectedIndex = tab + 1;
                    //MainConfig.TabPages[6].Parent.Focus();
                    RFIDStatus.Enabled = false;
                    Refresh();
                    break;
                case "MultiISO 1.2.5":
                    Display("RFID MultiISO 1.2.5 READY\n");
                    RFIDStatus.BackColor = Color.YellowGreen;
                    Thread.Sleep(1000);
                    tab = MainConfig.SelectedIndex;
                    RFID.Text = "RFID_OK";
                    //MainConfig.SelectedIndex = tab + 1;
                   //MainConfig.TabPages[6].Parent.Focus();
                    RFIDStatus.Enabled = false;
                    Refresh();
                    break;
                default:
                    if (data.Length > 0)
                    {
                        cardpresense = true;
                        data = data.TrimEnd('\r', '\n');
                        Display("RFID Card: " + data + "\n");
                        if (RF_Card == "")
                            RF_Card = data;
                    }
                    else
                    {
                        Display("RFID GOT UNKNOWN DATA:" + data + "\n");
                    }
                    data = "";
                    break;
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/
        //OPEN NOTE UX300 PORT.

        private void OpenUX300_Click(object sender, EventArgs e)
        {
            string Port = ini.IniReadValue("SerialEFT_POS", "COM");

            Display("LOAD SETTINGS SerialEFT_POS COM Port: " + Port + "\n");
            serialEFT_POS.PortName = Port;
            serialEFT_POS.BaudRate = 9600;
            serialEFT_POS.DataBits = 8;
            serialEFT_POS.Parity = Parity.None;
            serialEFT_POS.StopBits = StopBits.One;
            serialEFT_POS.Handshake = Handshake.None;
            serialEFT_POS.ReadTimeout = 500;
            try
            {
                serialEFT_POS.Open();
                OpenUX300.BackColor = Color.GreenYellow;
                OpenUX300.Enabled = false;
                UX300Status.Enabled = true;
                Refresh();
            }
            catch
            {
                MessageBox.Show("SerialEFT_POS Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
                Display("SerialEFT_POS Error \n\nProblem While Opening Serial Port: " + Port + " \n");
                OpenUX300.BackColor = Color.Tomato;
                Refresh();
            }
            serialEFT_POS.Close();
        }

/**************************************************************************************/
//UX300 STATUS

    private void UX300Status_Click(object sender, EventArgs e)
    {
        if (!serialEFT_POS.IsOpen)
            serialEFT_POS.Open();
        else
            Thread.Sleep(100);

            try
            {
                SystemIdentification = Encoding.ASCII.GetBytes(DateTime.Now.ToString("ddMMyyyyHHmmss"));
                TransactionType = new byte[2] { 0x32, 0x32 };
                OrigTransactionAmount = new byte[10] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 };
                Display("System ID: " + Encoding.UTF8.GetString(SystemIdentification));
                Display("Transaction Type: " + Encoding.UTF8.GetString(TransactionType));
                Display("Original Transaction Amount: " + Encoding.UTF8.GetString(OrigTransactionAmount));
                BytestoSend = new byte[SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length + ETX.Length];
                System.Buffer.BlockCopy(SystemIdentification, 0, BytestoSend, 0, SystemIdentification.Length);
                System.Buffer.BlockCopy(TransactionType, 0, BytestoSend, SystemIdentification.Length, TransactionType.Length);
                System.Buffer.BlockCopy(OrigTransactionAmount, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length), OrigTransactionAmount.Length);
                System.Buffer.BlockCopy(ETX, 0, BytestoSend, (OrigTransactionAmount.Length + SystemIdentification.Length + TransactionType.Length), ETX.Length);
                LRC[0] = CalculateLRC(BytestoSend, BytestoSend.Length);
                ///
                //start serial send
                ///
                serialEFT_POS.Write(STX, 0, STX.Length);
                Display("STX: " + Encoding.UTF8.GetString(STX));
                serialEFT_POS.Write(SystemIdentification, 0, SystemIdentification.Length);
                Display("SystemIdentification: " + Encoding.UTF8.GetString(SystemIdentification));
                serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                Display("FieldSeparator: " + Encoding.UTF8.GetString(FieldSeparator));
                serialEFT_POS.Write(TransactionType, 0, TransactionType.Length);
                Display("TransactionType: " + Encoding.UTF8.GetString(TransactionType));
                serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                Display("FieldSeparator: " + Encoding.UTF8.GetString(FieldSeparator));
                serialEFT_POS.Write(OrigTransactionAmount, 0, OrigTransactionAmount.Length);
                Display("OrigTransactionAmount: " + Encoding.UTF8.GetString(OrigTransactionAmount));
                for (int i = 0; i < 20; i++)
                {
                    serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                    Display("FieldSeparator: " + Encoding.UTF8.GetString(FieldSeparator));
                }
                serialEFT_POS.Write(ETX, 0, ETX.Length);
                Display("ETX: " + Encoding.UTF8.GetString(ETX));
                serialEFT_POS.Write(LRC, 0, LRC.Length);
                Display("LRC: " + Encoding.UTF8.GetString(LRC));
            }
            catch
            {
                MessageBox.Show("SerialUX300 Error \n\nProblem Writing Status \nCheck Ini Settings!");
                Display("SerialUX300 Error: Problem Writing Status\n");
            }
            ///temporary code segment
            /*UX300Status.Enabled = false;
            UX300Status.BackColor = Color.YellowGreen;
            Display("UX300 Reader READY!\n");
            this.UX300.Text = "UX300_OK";*/
            ///
        }

        /**************************************************************************************/
        //RECEIVE HANDLER FOR UX300
        private void ReceiveEFT_POS(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Display("incoming data from UX300:" + indata);
            indata = indata.Trim((char)6);//remove ACK
            indata = indata.Trim((char)2);//remove STX
            byte[] tmpData = Encoding.ASCII.GetBytes(indata);

            for (int i = 0; i < tmpData.Length; i++)
            {
                if (tmpData[i] == 0x1C)
                {
                    tmpData[i] = 0x7C;
                }
            }
            indata = Encoding.UTF8.GetString(tmpData);

            try
            {
                string[] packet = indata.Split((char)124);
                if (indata.Length >= 2)
                {
                    secondForm.Cancel.Visible = false;
                    secondForm.Refresh();
                    this.Invoke((MethodInvoker)delegate
                    {
                        Display("Receiving payment result data:");
                        Debugging.AppendText("System ID: ");
                        try
                        {
                            Debugging.AppendText(packet[0]);
                            Display("System ID: " + packet[0]);

                            Debugging.AppendText("\nTransaction Type: ");
                            try
                            {
                                Debugging.AppendText(packet[1].Substring(0, 2));
                                Display("Transaction Type: " + packet[1].Substring(0, 2));

                                Debugging.AppendText("\nNo. of Installments: ");
                                try
                                {
                                    Debugging.AppendText(packet[1].Substring(2, 2));
                                    Display("No. of Installments: " + packet[1].Substring(2, 2));

                                    Debugging.AppendText("\nNo. of Postdated Months: ");
                                    try
                                    {
                                        Debugging.AppendText(packet[1].Substring(4, 2));
                                        Display("No. of Postdated Months: " + packet[1].Substring(4, 2));

                                        Debugging.AppendText("\nTerminal Identification: ");
                                        try
                                        {
                                            Debugging.AppendText(packet[1].Substring(6, 12));
                                            Display("Terminal Identification: " + packet[1].Substring(6, 12));

                                            Debugging.AppendText("\nBatch Number: ");
                                            try
                                            {
                                                Debugging.AppendText(packet[2].Substring(0, 3));
                                                Display("Batch Number: " + packet[2].Substring(0, 3));

                                                Debugging.AppendText("\nResponse Code: ");
                                                try
                                                {
                                                    Debugging.AppendText(packet[2].Substring(3, 2));
                                                    Display("Response Code: " + packet[2].Substring(3, 2));

                                                    Debugging.AppendText("\nOriginal Amount or Batch Total Net Amount: ");
                                                    try
                                                    {
                                                        Debugging.AppendText(packet[3]);
                                                        Display("Original Amount or Batch Total Net Amount: " + packet[3]);

                                                        Debugging.AppendText("\nLoyalty Redemption Indicator: ");
                                                        try
                                                        {
                                                            Debugging.AppendText(packet[4]);
                                                            Display("Loyalty Redemption Indicator: " + packet[4]);

                                                            Debugging.AppendText("\nModified Amount: ");
                                                            try
                                                            {
                                                                Debugging.AppendText(packet[5]);
                                                                Display("Modified Amount: " + packet[5]);

                                                                Debugging.AppendText("\nRetrieval Reference Number: ");
                                                                try
                                                                {
                                                                    Debugging.AppendText(packet[6]);
                                                                    Display("Retrieval Reference Number: " + packet[6]);

                                                                    Debugging.AppendText("\nTrans. Refer. Number or Batch Total Counter: ");
                                                                    try
                                                                    {
                                                                        Debugging.AppendText(packet[7]);
                                                                        Display("Trans. Refer. Number or Batch Total Counter: " + packet[7]);

                                                                        Debugging.AppendText("\nAuth. Code: ");
                                                                        try
                                                                        {
                                                                            Debugging.AppendText(packet[8]);
                                                                            Display("Auth. Code: " + packet[8]);

                                                                            Debugging.AppendText("\nAccount number: ");
                                                                            try
                                                                            {
                                                                                Debugging.AppendText(packet[9]);
                                                                                Display("Account number: " + packet[9]);

                                                                                Debugging.AppendText("\nExpiration Date: ");
                                                                                try
                                                                                {
                                                                                    Debugging.AppendText(packet[10]);
                                                                                    Display("Expiration Date: " + packet[10]);

                                                                                    Debugging.AppendText("\nCardholder Name: ");
                                                                                    try
                                                                                    {
                                                                                        Debugging.AppendText(packet[11]);
                                                                                        Display("Cardholder Name: " + packet[11]);

                                                                                        Debugging.AppendText("\nDCC Transaction Amount: ");
                                                                                        try
                                                                                        {
                                                                                            Debugging.AppendText(packet[12]);
                                                                                            Display("DCC Transaction Amount: " + packet[12]);

                                                                                            Debugging.AppendText("\nDCC Currency: ");
                                                                                            try
                                                                                            {
                                                                                                Debugging.AppendText(packet[13]);
                                                                                                Display("DCC Currency: " + packet[13]);

                                                                                                Debugging.AppendText("\nDCC Exchange Rate icluding MarkUp: ");
                                                                                                try
                                                                                                {
                                                                                                    Debugging.AppendText(packet[14]);
                                                                                                    Display("DCC Exchange Rate icluding MarkUp: " + packet[14]);
                                                                                                }
                                                                                                catch (Exception ex)
                                                                                                {
                                                                                                    Debugging.AppendText("ERROR");
                                                                                                }

                                                                                                Debugging.AppendText("\nDCC MarkUp percentage: ");
                                                                                                try
                                                                                                {
                                                                                                    Debugging.AppendText(packet[15]);
                                                                                                    Display("DCC MarkUp percentage: " + packet[15]);

                                                                                                    Debugging.AppendText("\nDCC Exchange Date of Rate: ");
                                                                                                    try
                                                                                                    {
                                                                                                        Debugging.AppendText(packet[16]);
                                                                                                        Display("DCC Exchange Date of Rate: " + packet[16]);

                                                                                                        Debugging.AppendText("\nResponse Text Message: ");
                                                                                                        try
                                                                                                        {
                                                                                                            Debugging.AppendText(packet[17]);
                                                                                                            Display("Response Text Message: " + packet[17]);

                                                                                                            Debugging.AppendText("\nCard Product Name: ");
                                                                                                            try
                                                                                                            {
                                                                                                                Debugging.AppendText(packet[18]);
                                                                                                                Display("Card Product Name: " + packet[18]);

                                                                                                                Debugging.AppendText("\nOriginal Response Code: ");
                                                                                                                try
                                                                                                                {
                                                                                                                    Debugging.AppendText(packet[19]);
                                                                                                                    Display("Original Response Code: " + packet[19]);

                                                                                                                    Debugging.AppendText("\n1st Installment Date: ");
                                                                                                                    try
                                                                                                                    {
                                                                                                                        Debugging.AppendText(packet[20]);
                                                                                                                        Display("1st Installment Date: " + packet[20]);

                                                                                                                        Debugging.AppendText("\nNet Amount: ");
                                                                                                                        try
                                                                                                                        {
                                                                                                                            Debugging.AppendText(packet[21]);
                                                                                                                            Display("Net Amount: " + packet[21]);

                                                                                                                            Debugging.AppendText("\nTransaction Receipt Ticket: ");
                                                                                                                            try
                                                                                                                            {
                                                                                                                                Debugging.AppendText(packet[22]);
                                                                                                                                Display("Transaction Receipt Ticket: " + packet[22]);

                                                                                                                                Debugging.AppendText("\nBatch Status: ");
                                                                                                                                try
                                                                                                                                {
                                                                                                                                    Debugging.AppendText(packet[23]);
                                                                                                                                    Display("Batch Status: " + packet[23]);

                                                                                                                                    Debugging.AppendText("\nCurrency Code: ");
                                                                                                                                    try
                                                                                                                                    {
                                                                                                                                        Debugging.AppendText(packet[24]);
                                                                                                                                        Display("Currency Code: " + packet[24]);

                                                                                                                                        Debugging.AppendText("\nLoyalty Balance: ");
                                                                                                                                        try
                                                                                                                                        {
                                                                                                                                            Debugging.AppendText(packet[25]);
                                                                                                                                            Display("Loyalty Balance: " + packet[25]);

                                                                                                                                            Debugging.AppendText("\nLoyalty Transaction Points: ");
                                                                                                                                            try
                                                                                                                                            {
                                                                                                                                                Debugging.AppendText(packet[26]);
                                                                                                                                                Display("Loyalty Transaction Points: " + packet[26]);

                                                                                                                                                Debugging.AppendText("\nLoyalty Trans.Reference No: ");
                                                                                                                                                try
                                                                                                                                                {
                                                                                                                                                    Debugging.AppendText(packet[27]);
                                                                                                                                                    Display("Loyalty Trans.Reference No: " + packet[27]);
                                                                                                                                                }
                                                                                                                                                catch (Exception ex)
                                                                                                                                                {
                                                                                                                                                    Debugging.AppendText("ERROR");
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                            catch (Exception ex)
                                                                                                                                            {
                                                                                                                                                Debugging.AppendText("ERROR");
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                        catch (Exception ex)
                                                                                                                                        {
                                                                                                                                            Debugging.AppendText("ERROR");
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                    catch (Exception ex)
                                                                                                                                    {
                                                                                                                                        Debugging.AppendText("ERROR");
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                catch (Exception ex)
                                                                                                                                {
                                                                                                                                    Debugging.AppendText("ERROR");
                                                                                                                                }
                                                                                                                            }
                                                                                                                            catch (Exception ex)
                                                                                                                            {
                                                                                                                                Debugging.AppendText("ERROR");
                                                                                                                            }
                                                                                                                        }
                                                                                                                        catch (Exception ex)
                                                                                                                        {
                                                                                                                            Debugging.AppendText("ERROR");
                                                                                                                        }
                                                                                                                    }
                                                                                                                    catch (Exception ex)
                                                                                                                    {
                                                                                                                        Debugging.AppendText("ERROR");
                                                                                                                    }
                                                                                                                }
                                                                                                                catch (Exception ex)
                                                                                                                {
                                                                                                                    Debugging.AppendText("ERROR");
                                                                                                                }
                                                                                                            }
                                                                                                            catch (Exception ex)
                                                                                                            {
                                                                                                                Debugging.AppendText("ERROR");
                                                                                                            }
                                                                                                        }
                                                                                                        catch (Exception ex)
                                                                                                        {
                                                                                                            Debugging.AppendText("ERROR");
                                                                                                        }
                                                                                                    }
                                                                                                    catch (Exception ex)
                                                                                                    {
                                                                                                        Debugging.AppendText("ERROR");
                                                                                                    }
                                                                                                }
                                                                                                catch (Exception ex)
                                                                                                {
                                                                                                    Debugging.AppendText("ERROR");
                                                                                                }
                                                                                            }
                                                                                            catch (Exception ex)
                                                                                            {
                                                                                                Debugging.AppendText("ERROR");
                                                                                            }
                                                                                        }
                                                                                        catch (Exception ex)
                                                                                        {
                                                                                            Debugging.AppendText("ERROR");
                                                                                        }
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Debugging.AppendText("ERROR");
                                                                                    }
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    Debugging.AppendText("ERROR");
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                Debugging.AppendText("ERROR");
                                                                            }
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            Debugging.AppendText("ERROR");
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Debugging.AppendText("ERROR");
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Debugging.AppendText("ERROR");
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Debugging.AppendText("ERROR");
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Debugging.AppendText("ERROR");
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Debugging.AppendText("ERROR");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Debugging.AppendText("ERROR");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Debugging.AppendText("ERROR");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Debugging.AppendText("ERROR");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debugging.AppendText("ERROR");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debugging.AppendText("ERROR");
                                }
                            }
                            catch (Exception ex)
                            {
                                Debugging.AppendText("ERROR");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debugging.AppendText("ERROR");
                        }



                    });
                    //Response text message for close batch
                    Display("Response text message: " + packet[17]);
                    if (packet[17].IndexOf("CLOSE BATCH SUCCEEDED") != -1)
                    {
                        Display("Close Batch succesfully");
                        BatchTotalAmount = packet[3];
                        BatchTotalTransactions = packet[7];
                        BatchTotalAmount = BatchTotalAmount.Trim('+');
                        BatchTotalAmount = BatchTotalAmount.TrimStart('0');
                        if (BatchTotalAmount.Length > 2)
                        {
                            BatchTotalAmount = BatchTotalAmount.Insert(BatchTotalAmount.Length - 2, ",");
                        }
                        else if (BatchTotalAmount.Length == 2)
                        {
                            BatchTotalAmount = "0," + BatchTotalAmount;
                        }
                        else
                        {
                            BatchTotalAmount = "0,0" + BatchTotalAmount;
                        }
                        string batchdate = DateTime.Now.ToString("dd/MM/yyyy");
                        string batchtime = DateTime.Now.ToString("HH:mm:ss");
                        if (!batchIsClosed)
                        {
                            try
                            {
                                string[] lines = { "ΕΠΙΤΥΧΗΣ ΑΠΟΣΤΟΛΗ ΠΑΚΕΤΟΥ: " + packet[2].Substring(0, 3), "ΠΛΗΘΟΣ ΣΥΝΑΛΛΑΓΩΝ: " + BatchTotalTransactions, "ΣΥΝΟΛΟ ΣΥΝΑΛΛΑΓΩΝ: " + BatchTotalAmount, "ΗΜΕΡΟΜΗΝΙΑ: " + batchdate, "ΩΡΑ       : " + batchtime };
                                string FileName = "C:/CreditPOS/closeBatch/" + DateTime.Now.ToString("ddMMyy") + "_closeBatch.txt";
                                System.IO.File.WriteAllLines(FileName, lines);
                                batchIsClosed = true;
                            }
                            catch (Exception ex)
                            {
                                Display(ex.Message);
                            }
                            try
                            {
                                MailMessage mail = new MailMessage();
                                SmtpClient SmtpServer = new SmtpClient(ini.IniReadValue("Params", "SmtpClient"));
                                mail.From = new MailAddress(ini.IniReadValue("Params", "mailFrom"));
                                mail.To.Add(ini.IniReadValue("Params", "mailTo"));
                                //mail.CC.Add("gmaragkakis@gmail.com");
                                mail.Bcc.Add(ini.IniReadValue("Params", "mailBCC"));
                                mail.Subject = ini.IniReadValue("Params", "mailSubject");
                                mail.Body = ini.IniReadValue("Params", "mailBody");

                                Attachment attachment;
                                string FileName = "C:/CreditPOS/closeBatch/" + DateTime.Now.ToString("ddMMyy") + "_closeBatch.txt";
                                attachment = new Attachment(FileName);
                                mail.Attachments.Add(attachment);
                                SmtpServer.UseDefaultCredentials = false;
                                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                                SmtpServer.Port = Convert.ToInt32(ini.IniReadValue("Params", "SmtpPort"));
                                SmtpServer.Credentials = new System.Net.NetworkCredential(ini.IniReadValue("Params", "SmtpUsername"), ini.IniReadValue("Params", "SmtpPassword"));
                                SmtpServer.EnableSsl = true;

                                SmtpServer.Send(mail);
                                Display("mail Send");
                            }
                            catch (Exception ex)
                            {
                                Display(ex.ToString());
                            }
                            SM = 1;
                        }
                        else
                        {
                            try
                            {
                                string Filepath = "C:/CreditPOS/close_batch.txt";
                                string strFileName = Filepath;

                                string[] lines;
                                lines = File.ReadAllLines(Filepath);
                                lines[0] = string.Concat(lines[0], packet[2].Substring(0, 3));
                                lines[1] = string.Concat(lines[1], BatchTotalTransactions);
                                lines[2] = string.Concat(lines[2], BatchTotalAmount);
                                lines[3] = string.Concat(lines[3], batchdate);
                                lines[4] = string.Concat(lines[4], batchtime);
                                File.WriteAllLines("C:/CreditPOS/close_batch_temp.txt", lines);

                                streamToPrint = new StreamReader("C:/CreditPOS/close_batch_temp.txt");
                                try
                                {
                                    printFont = new Font("Arial", 10);
                                    PrintDocument pd = new PrintDocument();
                                    PrintController pc = new StandardPrintController();
                                    pd.PrintController = pc;
                                    pd.PrintPage += new PrintPageEventHandler
                                       (this.pd_PrintPage);
                                    pd.Print();
                                    Display("Printer READY!\n");
                                    this.Printer.Text = "Printer_OK";
                                    PRINTERStatus.BackColor = Color.YellowGreen;
                                }
                                finally
                                {
                                    streamToPrint.Close();
                                }

                            }
                            catch (Exception ex)
                            {
                                OpenPRINTER.BackColor = Color.Tomato;
                                Display(ex.Message);
                            }
                        }
                    }
                    else if (packet[17].IndexOf("BATCH IS EMPTY") != -1)
                    {
                        Display("Close Batch is empty");
                        if (!batchIsClosed)
                        {
                            try
                            {
                                string[] lines = { "ΔΕΝ ΠΕΡΙΕΧΕΙ ΣΥΝΑΛΛΑΓΕΣ" };
                                string FileName = "C:/CreditPOS/closeBatch/" + DateTime.Now.ToString("ddMMyy") + "_closeBatch.txt";
                                System.IO.File.WriteAllLines(FileName, lines);
                                batchIsClosed = true;
                            }
                            catch (Exception ex)
                            {
                                Display(ex.Message);
                            }
                            try
                            {
                                MailMessage mail = new MailMessage();
                                SmtpClient SmtpServer = new SmtpClient(ini.IniReadValue("Params", "SmtpClient"));
                                mail.From = new MailAddress(ini.IniReadValue("Params", "mailFrom"));
                                mail.To.Add(ini.IniReadValue("Params", "mailTo"));
                                //mail.CC.Add("gmaragkakis@gmail.com");
                                mail.Bcc.Add(ini.IniReadValue("Params", "mailBCC"));
                                mail.Subject = ini.IniReadValue("Params", "mailSubject");
                                mail.Body = ini.IniReadValue("Params", "mailBody");

                                Attachment attachment;
                                string FileName = "C:/CreditPOS/closeBatch/" + DateTime.Now.ToString("ddMMyy") + "_closeBatch.txt";
                                attachment = new Attachment(FileName);
                                mail.Attachments.Add(attachment);

                                SmtpServer.Port = Convert.ToInt32(ini.IniReadValue("Params", "SmtpPort"));
                                SmtpServer.Credentials = new System.Net.NetworkCredential(ini.IniReadValue("Params", "SmtpUsername"), ini.IniReadValue("Params", "SmtpPassword"));
                                SmtpServer.EnableSsl = true;

                                SmtpServer.Send(mail);
                                Display("mail Send");
                            }
                            catch (Exception ex)
                            {
                                Display(ex.ToString());
                            }
                            SM = 1;
                        }
                    }
                    else if (packet[17].IndexOf("USER BREAK - CANCEL") != -1)
                    {
                        try
                        {
                            streamToPrint = new StreamReader
                               ("C:/CreditPOS/cancel.txt");
                            try
                            {
                                printFont = new Font("Arial", 10);
                                PrintDocument pd = new PrintDocument();
                                PrintController pc = new StandardPrintController();
                                pd.PrintController = pc;
                                pd.PrintPage += new PrintPageEventHandler
                                   (this.pd_PrintPage);
                                pd.Print();
                                Display("Printer READY!\n");
                                Printer.Text = "Printer_OK";
                                PRINTERStatus.BackColor = Color.YellowGreen;
                            }
                            finally
                            {
                                streamToPrint.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            OpenPRINTER.BackColor = Color.Tomato;
                            Display(ex.Message);
                        }
                        SM = 10;
                    }
                    else if (packet[17].IndexOf("DECLINED") != -1)
                    {
                        try
                        {
                            streamToPrint = new StreamReader
                               ("C:/CreditPOS/declined.txt");
                            try
                            {
                                printFont = new Font("Arial", 10);
                                PrintDocument pd = new PrintDocument();
                                pd.PrintPage += new PrintPageEventHandler
                                   (this.pd_PrintPage);
                                pd.Print();
                                Display("Printer READY!\n");
                                this.Printer.Text = "Printer_OK";
                                PRINTERStatus.BackColor = Color.YellowGreen;
                            }
                            finally
                            {
                                streamToPrint.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            OpenPRINTER.BackColor = Color.Tomato;
                            Display(ex.Message);
                        }
                        SM = 10;
                    }
                    else if (packet[17].IndexOf("USER PRESSED CANCEL IN PIN DIALOG") != -1)
                    {
                        try
                        {
                            streamToPrint = new StreamReader
                               ("C:/CreditPOS/cancel_in_pin.txt");
                            try
                            {
                                printFont = new Font("Arial", 10);
                                PrintDocument pd = new PrintDocument();
                                pd.PrintPage += new PrintPageEventHandler
                                   (this.pd_PrintPage);
                                pd.Print();
                                Display("Printer READY!\n");
                                this.Printer.Text = "Printer_OK";
                                PRINTERStatus.BackColor = Color.YellowGreen;
                            }
                            finally
                            {
                                streamToPrint.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            OpenPRINTER.BackColor = Color.Tomato;
                            Display(ex.Message);
                        }
                        SM = 10;
                    }
                    else if (packet[17].IndexOf("CARD PRESENCE TIMEOUT") != -1)
                    {
                        try
                        {
                            streamToPrint = new StreamReader
                               ("C:/CreditPOS/card_removed.txt");
                            try
                            {
                                printFont = new Font("Arial", 10);
                                PrintDocument pd = new PrintDocument();
                                pd.PrintPage += new PrintPageEventHandler
                                   (this.pd_PrintPage);
                                pd.Print();
                                Display("Printer READY!\n");
                                this.Printer.Text = "Printer_OK";
                                PRINTERStatus.BackColor = Color.YellowGreen;
                            }
                            finally
                            {
                                streamToPrint.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            OpenPRINTER.BackColor = Color.Tomato;
                            Display(ex.Message);
                        }
                        SM = 10;
                    }
                    else if (packet[17].IndexOf("PIN DIALOG TIMEOUT") != -1)
                    {
                        try
                        {
                            streamToPrint = new StreamReader
                               ("C:/CreditPOS/pin_timeout.txt");
                            try
                            {
                                printFont = new Font("Arial", 10);
                                PrintDocument pd = new PrintDocument();
                                pd.PrintPage += new PrintPageEventHandler
                                   (this.pd_PrintPage);
                                pd.Print();
                                Display("Printer READY!\n");
                                this.Printer.Text = "Printer_OK";
                                PRINTERStatus.BackColor = Color.YellowGreen;
                            }
                            finally
                            {
                                streamToPrint.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            OpenPRINTER.BackColor = Color.Tomato;
                            Display(ex.Message);
                        }
                        SM = 10;
                    }
                    //Host response handling
                    Display("Host Response:");
                    switch (packet[19])
                    {
                        case "000":
                            if (SM != 1)
                            {
                                Display("Online Approved");
                                secondForm.Cancel.Visible = false;
                                secondForm.ValueText.Clear();
                                secondForm.Vprogress.Visible = false;
                                secondForm.ValueText.AppendText("0,00€");
                                RRN = packet[6];
                                Auth_Code = packet[8];
                                CardProdNumber = packet[18];
                                CardHolderName = packet[11];
                                CardProductName = packet[18];
                                if (CardProductName.IndexOf("VISA", StringComparison.CurrentCultureIgnoreCase) != -1)
                                {
                                    PaymentMethod = "2";
                                    Display("Card Type: VISA");
                                }
                                else if (CardProductName.IndexOf("MASTERCARD", StringComparison.CurrentCultureIgnoreCase) != -1)
                                {
                                    PaymentMethod = "3";
                                    Display("Card Type: MASTERCARD");
                                }
                                Display("Payment OK");
                                Thread.Sleep(100);
                                value = "0,00";
                                if (SM == 62)
                                {
                                    string url_1 = HTTPURLTICKET + UserCode + "?charge_until=" + charge_until + "&payment_method=" + PaymentMethod + "&bank_transaction_id=" + RRN + "&bank_approval_code=" + Auth_Code;
                                    Display("requesting url: " + url_1);
                                    HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                                    request_1.Method = "PUT";
                                    request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                                    try
                                    {
                                        HttpWebResponse response_1 = null;
                                        response_1 = (HttpWebResponse)request_1.GetResponse();
                                        Stream responseStream = response_1.GetResponseStream();
                                        string invoice_code;
                                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                        var jsonobject = reader.ReadToEnd();
                                        Display("JSON_object is: " + jsonobject);
                                        var responseData = JObject.Parse(jsonobject);
                                        Display("+++" + responseData.ToString());
                                        invoice_code = (string)responseData["invoice"]["code"];
                                        Display("invoice_code: " + invoice_code);
                                        invoice_sequence = (string)responseData["invoice"]["sequence"];
                                        Display("invoice_sequence: " + invoice_sequence);
                                        invoice_number = (string)responseData["invoice"]["number"];
                                        Display("invoice_number: " + invoice_number);
                                        plate = (string)responseData["ticket"]["ticket_entrance"]["vehicle"]["plate"];
                                        Display("vehicle_plate: " + plate);
                                        charge = (string)responseData["charge"];
                                        Display("charge: " + charge);
                                        entered_at = (string)responseData["ticket"]["ticket_entrance"]["entered_at"];
                                        Display("entered_at: " + entered_at);
                                        response_1.Close();
                                        reader.Close();
                                        responseStream.Close();
                                    }
                                    catch (WebException ex)
                                    {
                                        HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                        switch (response_1.StatusCode)
                                        {
                                            case HttpStatusCode.NotFound:
                                                Thread.Sleep(2000);
                                                SM = 71;
                                                Display("Error 404 on receipt request");
                                                break;
                                            case HttpStatusCode.InternalServerError:
                                                Thread.Sleep(2000);
                                                SM = 71;
                                                Display("Error 500 on receipt request");
                                                break;
                                            case HttpStatusCode.ServiceUnavailable:
                                                Thread.Sleep(2000);
                                                SM = 71;
                                                Display("Error 503 on receipt request");
                                                break;
                                        }
                                    }
                                    SM = 71;
                                    break;
                                }
                                else if (SM == 63)
                                {
                                    string url_1 = HTTPURLRENEW + subscriptionId + "?payment_method=" + PaymentMethod + "&bank_transaction_id=" + RRN + "&bank_approval_code=" + Auth_Code;
                                    Display("requesting url: " + url_1);
                                    HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                                    request_1.Method = "POST";
                                    request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                                    try
                                    {
                                        HttpWebResponse response_1 = null;
                                        response_1 = (HttpWebResponse)request_1.GetResponse();
                                        Stream responseStream = response_1.GetResponseStream();
                                        string invoice_code;
                                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                        var jsonobject = reader.ReadToEnd();
                                        Display("JSON_object is: " + jsonobject);
                                        var responseData = JObject.Parse(jsonobject);
                                        Display("+++" + responseData.ToString());
                                        invoice_code = (string)responseData["invoice"]["code"];
                                        Display("invoice_code: " + invoice_code);
                                        invoice_sequence = (string)responseData["invoice"]["sequence"];
                                        Display("invoice_sequence: " + invoice_sequence);
                                        invoice_number = (string)responseData["invoice"]["number"];
                                        Display("invoice_number: " + invoice_number);
                                        charge = (string)responseData["invoice"]["final_price"];
                                        Display("charge: " + charge);
                                        response_1.Close();
                                        reader.Close();
                                        responseStream.Close();
                                    }
                                    catch (WebException ex)
                                    {
                                        HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                        switch (response_1.StatusCode)
                                        {
                                            case HttpStatusCode.NotFound:
                                                Thread.Sleep(2000);
                                                SM = 73;
                                                Display("Error 404 on receipt request");
                                                break;
                                            case HttpStatusCode.InternalServerError:
                                                Thread.Sleep(2000);
                                                SM = 73;
                                                Display("Error 500 on receipt request");
                                                break;
                                            case HttpStatusCode.ServiceUnavailable:
                                                Thread.Sleep(2000);
                                                SM = 73;
                                                Display("Error 503 on receipt request");
                                                break;
                                        }
                                    }
                                    SM = 73;
                                    break;
                                }
                            }
                            break;
                        case "001":
                            if (SM != 1)
                            {
                                Display("Offline Approved");
                                secondForm.Cancel.Visible = false;
                                secondForm.ValueText.Clear();
                                secondForm.Vprogress.Visible = false;
                                secondForm.ValueText.AppendText("0,00€");
                                RRN = packet[6];
                                Auth_Code = packet[8];
                                CardProdNumber = packet[18];
                                CardHolderName = packet[11];
                                CardProductName = packet[18];
                                if (CardProductName.IndexOf("VISA", StringComparison.CurrentCultureIgnoreCase) != -1)
                                {
                                    PaymentMethod = "2";
                                    Display("Card Type: VISA");
                                }
                                else if (CardProductName.IndexOf("MASTERCARD", StringComparison.CurrentCultureIgnoreCase) != -1)
                                {
                                    PaymentMethod = "3";
                                    Display("Card Type: MASTERCARD");
                                }
                                Display("Payment OK");
                                Thread.Sleep(100);
                                value = "0,00";
                                if (SM == 62)
                                {
                                    string url_1 = HTTPURLTICKET + UserCode + "?charge_until=" + charge_until + "&payment_method=" + PaymentMethod + "&bank_transaction_id=" + RRN + "&bank_approval_code=" + Auth_Code;
                                    Display("requesting url: " + url_1);
                                    HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                                    request_1.Method = "PUT";
                                    request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                                    try
                                    {
                                        HttpWebResponse response_1 = null;
                                        response_1 = (HttpWebResponse)request_1.GetResponse();
                                        Stream responseStream = response_1.GetResponseStream();
                                        string invoice_code;
                                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                        var jsonobject = reader.ReadToEnd();
                                        Display("JSON_object is: " + jsonobject);
                                        var responseData = JObject.Parse(jsonobject);
                                        Display("+++" + responseData.ToString());
                                        invoice_code = (string)responseData["invoice"]["code"];
                                        Display("invoice_code: " + invoice_code);
                                        invoice_sequence = (string)responseData["invoice"]["sequence"];
                                        Display("invoice_sequence: " + invoice_sequence);
                                        invoice_number = (string)responseData["invoice"]["number"];
                                        Display("invoice_number: " + invoice_number);
                                        plate = (string)responseData["ticket"]["ticket_entrance"]["vehicle"]["plate"];
                                        Display("vehicle_plate: " + plate);
                                        charge = (string)responseData["charge"];
                                        Display("charge: " + charge);
                                        entered_at = (string)responseData["ticket"]["ticket_entrance"]["entered_at"];
                                        Display("entered_at: " + entered_at);
                                        response_1.Close();
                                        reader.Close();
                                        responseStream.Close();
                                    }
                                    catch (WebException ex)
                                    {
                                        HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                        switch (response_1.StatusCode)
                                        {
                                            case HttpStatusCode.NotFound:
                                                Thread.Sleep(2000);
                                                SM = 71;
                                                Display("Error 404 on receipt request");
                                                break;
                                            case HttpStatusCode.InternalServerError:
                                                Thread.Sleep(2000);
                                                SM = 71;
                                                Display("Error 500 on receipt request");
                                                break;
                                            case HttpStatusCode.ServiceUnavailable:
                                                Thread.Sleep(2000);
                                                SM = 71;
                                                Display("Error 503 on receipt request");
                                                break;
                                        }
                                    }
                                    SM = 71;
                                    break;
                                }
                                else if (SM == 63)
                                {
                                    string url_1 = HTTPURLRENEW + subscriptionId + "?payment_method=" + PaymentMethod + "&bank_transaction_id=" + RRN + "&bank_approval_code=" + Auth_Code;
                                    Display("requesting url: " + url_1);
                                    HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                                    request_1.Method = "POST";
                                    request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                                    try
                                    {
                                        HttpWebResponse response_1 = null;
                                        response_1 = (HttpWebResponse)request_1.GetResponse();
                                        Stream responseStream = response_1.GetResponseStream();
                                        string invoice_code;
                                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                        var jsonobject = reader.ReadToEnd();
                                        Display("JSON_object is: " + jsonobject);
                                        var responseData = JObject.Parse(jsonobject);
                                        Display("+++" + responseData.ToString());
                                        invoice_code = (string)responseData["invoice"]["code"];
                                        Display("invoice_code: " + invoice_code);
                                        invoice_sequence = (string)responseData["invoice"]["sequence"];
                                        Display("invoice_sequence: " + invoice_sequence);
                                        invoice_number = (string)responseData["invoice"]["number"];
                                        Display("invoice_number: " + invoice_number);
                                        charge = (string)responseData["invoice"]["final_price"];
                                        Display("charge: " + charge);
                                        response_1.Close();
                                        reader.Close();
                                        responseStream.Close();
                                    }
                                    catch (WebException ex)
                                    {
                                        HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                        switch (response_1.StatusCode)
                                        {
                                            case HttpStatusCode.NotFound:
                                                Thread.Sleep(2000);
                                                SM = 73;
                                                Display("Error 404 on receipt request");
                                                break;
                                            case HttpStatusCode.InternalServerError:
                                                Thread.Sleep(2000);
                                                SM = 73;
                                                Display("Error 500 on receipt request");
                                                break;
                                            case HttpStatusCode.ServiceUnavailable:
                                                Thread.Sleep(2000);
                                                SM = 73;
                                                Display("Error 503 on receipt request");
                                                break;
                                        }
                                    }
                                    SM = 73;
                                    break;
                                }
                            }
                            break;
                        case "005":
                            Display("Handshake Approved");
                            break;
                        case "006":
                            Display("PANCapture succeeded");
                            break;
                        case "007":
                            Display("Admin Approved (for Keep Alive)");
                            break;
                        case "10":
                            Display("Online Declined");
                            SM = 10;
                            break;
                        case "011":
                            Display("Batch Status Error");
                            break;
                        case "012":
                            Display("Offline Declined (Not Allowed)");
                            SM = 10;
                            break;
                        case "013":
                            Display("Offline Declined (Installments)");
                            SM = 10;
                            break;
                        case "014":
                            Display("No Transaction match");
                            break;
                        case "015":
                            Display("Close Batch Succeeded");
                            break;
                        case "016":
                            Display("Close Batch Rejected/Failed");
                            break;
                        case "017":
                            Display("Balance NOT Available");
                            break;
                        case "018":
                            Display("Handshake Declined");
                            break;
                        case "019":
                            Display("External MId/TId not available");
                            break;
                        case "020":
                            Display("Magn. Card not Allowed at PANCapture");
                            break;
                        case "021":
                            Display("PAN does NOT Match");
                            break;
                        case "022":
                            Display("Chip Card Removed");
                            break;
                        case "030":
                            Display("User Brake");
                            break;
                        case "031":
                            Display("Console Timeout");
                            SM = 10;
                            break;
                        case "032":
                            Display("Card Presence Timeout");
                            SM = 10;
                            break;
                        case "040":
                            Display("No Connection - Please Try Again");
                            SM = 10;
                            break;
                        case "050":
                            Display("HOLD - Reset Payment Result Timeout");
                            break;
                        case "51":
                            Display("Bad Magnetic Track Data");//see reference of ECR Interface Enrichment
                            break;
                        case "052":
                            Display("DCC Alert - EFT/POS alerts ECR operator that a DCC transaction is in progress");
                            break;
                        case "055":
                            Display("Enter PIN number");
                            break;
                        case "056":
                            Display("Wrong PIN number - offline");
                            break;
                        case "057":
                            Display("PIN inserted");
                            break;
                        case "070":
                            Display("Online Approved (Loyalty)");
                            break;
                        case "071":
                            Display("Offline Approved (Loyalty)");
                            break;
                        case "072":
                            Display("Online Declined (Loyalty)");
                            break;
                        case "073":
                            Display("Offline Declined (Loyalty)");
                            break;
                        case "090":
                            Display("System Menu Return");
                            break;
                        case "091":
                            Display("Transaction Declined from Host");
                            SM = 10;
                            break;
                        case "094":
                            Display("Request field data format error");
                            break;
                        case "095":
                            Display("SCR slot busy/Card presense");
                            break;
                        case "096":
                            Display("SCR Possible Tampered");
                            break;
                        case "097":
                            Display("SCR Disconnected from Vx700");
                            break;
                        case "098":
                            Display("Unknown Transaction/Function");
                            break;
                        case "099":
                            Display("Cashier Request Format Error");
                            break;
                        default:
                            Display("Host unreachable");
                            break;
                    }
                    //send Cashier Request ACK message
                    Display("Sending ACK message to EFT/POS...");
                    serialEFT_POS.Write("\u0006");
                }//end if inadata length > 2
                UX300Status.Enabled = false;
                UX300Status.BackColor = Color.YellowGreen;
                Display("UX300 Reader READY!\n");
                this.UX300.Text = "UX300_OK";
                this.Refresh();
            }
            catch (Exception ex)
            { }
            serialEFT_POS.DiscardOutBuffer();
            serialEFT_POS.DiscardInBuffer();
            serialEFT_POS.BaseStream.Flush();
            serialEFT_POS.Close();
            serialEFT_POS.Open();
            quitBtn.Enabled = true;
        }


        /**************************************************************************************/
        private void NotesTest_Click(object sender, EventArgs e)
        {
            byte[] EnAll = { 184 };
            byte[] NDOneBill = { 0x01, 0x10, 0x00, 0x10, 0x01, 0x22 };

            if (!serialEFT_POS.IsOpen)
            {
                serialEFT_POS.Open(); Thread.Sleep(100);
            }
            if (serialEFT_POS.IsOpen) { serialEFT_POS.Write(NDOneBill, 0, NDOneBill.Length); Thread.Sleep(100); }//1000
        }

/**************************************************************************************/
/**************************************************************************************/
    //OPEN PRINTER PORT
    private void OpenPRINTER_Click(object sender, EventArgs e)
    {
            string usb_printer = ini.IniReadValue("Params", "usb_printer");
            Display("Declared printer is: " + usb_printer); //as written in control panel
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                if (printer == usb_printer)
                {
                    OpenPRINTER.BackColor = Color.GreenYellow;
                    OpenPRINTER.Enabled = false;
                    PRINTERStatus.Enabled = true;
                    PrintCashValues.Enabled = true;
                    PrintTest.Enabled = true;
                    this.Refresh();
                    break;
                }
            }
        }
    /**************************************************************************************/
    //PRINTER STATUS
    private void PRINTERStatus_Click(object sender, EventArgs e)
    {
            try
            {
                streamToPrint = new StreamReader
                   ("C:/POS/test_print.txt");
                try
                {
                    printFont = new Font("Arial", 8);
                    PrintDocument pd = new PrintDocument();
                    PrintController pc = new StandardPrintController();
                    pd.PrintController = pc;
                    pd.PrintPage += new PrintPageEventHandler
                       (this.pd_PrintPage);
                    pd.Print();
                    Display("Printer READY!\n");
                    this.Printer.Text = "Printer_OK";
                    PRINTERStatus.BackColor = Color.YellowGreen;
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception ex)
            {
                OpenPRINTER.BackColor = Color.Tomato;
                Display(ex.Message);
            }            
        }
    
    /**************************************************************************************/
    //RECEIVE HANDLER FOR PRINTER
    private void ReceivePRINTER(object sender, SerialDataReceivedEventArgs e)
    {
        byte[] buffer = new byte[100];

        int size = serialPRINTER.Read(buffer, 0, 60);
        string data = BitConverter.ToString(buffer);
        data = data.Replace("-", "");
        data = data.Substring(0, 2 * size);
        
        if (size > 0)
            this.Invoke((MethodInvoker)delegate
            { Display("Got Printer Message_" + size.ToString() + ": " + data + "\n"); });

        if (size >= 1 && ((buffer[0]) == 0x7E))
        {
            this.Invoke((MethodInvoker)delegate
            {
                Display("Printer ERROR: Paper End\nReplace paper\n"); SM = 11;
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
                //if (tab < 4) this.MainConfig.SelectedIndex = tab + 1;
                //this.MainConfig.TabPages[6].Parent.Focus();
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
                //if (tab < 4) this.MainConfig.SelectedIndex = tab + 1;
                //this.MainConfig.TabPages[6].Parent.Focus();
            });
            Thread.Sleep(1);
        }
        data = "";
    }

/**************************************************************************************/
// PrintTEST Receipt 

    private void test_Click(object sender, EventArgs e)
    {
            try
            {
                streamToPrint = new StreamReader
                   ("C:/POS/receipt_test.txt");
                try
                {
                    printFont = new Font("Arial", 8);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler
                       (this.pd_PrintPage);
                    PrintController pc = new StandardPrintController();
                    pd.PrintController = pc;

                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/
        //OPEN NV PORT

        private void OpenNV_Click(object sender, EventArgs e)
        {
            string Port = ini.IniReadValue("SerialNV", "COM");
            string SSP_Address = ini.IniReadValue("SerialNV", "SSP");
            FiveEuroNotesLevel = ini.IniReadValue("SerialNV", "Avail05");
            TenEuroNotesLevel = ini.IniReadValue("SerialNV", "Avail10");

            Global.ComPort = Port;
            Global.SSPAddress = Byte.Parse(SSP_Address);
            Display("LOAD SETTINGS Note Validator COM Port: " + Port + "\n");
            Display("LOAD SETTINGS Note Validator SSP Address: " + SSP_Address + "\n");

            try
            {
                OpenNV.BackColor = Color.GreenYellow;
                OpenNV.Enabled = false;
                NVStatus.Enabled = true;
                Refresh();
            }
            catch
            {
                MessageBox.Show("NoteValidator Error \n\nProblem While Opening Serial Port: " + Port + " \nCheck Ini Settings!");
                Display("NoteValidator Error \n\nProblem While Opening Serial Port: " + Port + " \n");
                OpenNV.BackColor = Color.Tomato;
                Refresh();
            }
        }

        /**************************************************************************************/
        //NV STATUS

        private void NVStatus_Click(object sender, EventArgs e)
        {
            // create an instance of the validator info class
            Payout = new CPayout();
            btnHalt.Enabled = false;
            string Port = ini.IniReadValue("SerialNV", "COM");
            string SSP_Address = ini.IniReadValue("SerialNV", "SSP");
            Global.ComPort = Port;
            Global.SSPAddress = Byte.Parse(SSP_Address);
            Point p = this.Location;
            p.X += this.Width;
            Payout.CommsLog.Location = p;
            NVStatus.Enabled = false;
            NVStatus.BackColor = Color.YellowGreen;
            NoteRecycler.Text = "NoteValidator_OK";
            int tab = MainConfig.SelectedIndex;
            MainConfig.SelectedIndex = tab + 1;
            MainConfig.TabPages[5].Parent.Focus();
            Refresh();
        }


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
                Refresh();
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
                        case 0:
                            this.Coin5.Text = stat.ToString();
                            break;
                        case 1:
                            this.Coin10.Text = stat.ToString();
                            break;
                        case 2:
                            this.Coin20.Text = stat.ToString();
                            break;
                        case 3:
                            this.Coin50.Text = stat.ToString();
                            break;
                        case 4:
                            this.Coin100.Text = stat.ToString();
                            break;
                        case 5:
                            this.Coin200.Text = stat.ToString();
                            break;
                    }
                }
                else
                    Display("Coins ERROR: No Data\n");
            }
            Total_Coins = pm.set(1, 3, 0, 0);//TOTAL COINS CMD
            stat = Total_Coins;
            string tmp = stat.ToString();
            if (tmp.Length > 2) tmp = tmp.Insert(tmp.Length - 2, ",");
            Display("TotalCoins:" + tmp + "\n");

            //Read INI values
            Avail10Notes.Text = ini.IniReadValue("SerialND", "Avail10");
            Avail05Notes.Text = ini.IniReadValue("SerialNV", "Avail05");
            Paid50Notes.Text = ini.IniReadValue("SerialNV", "Paid50");
            Paid20Notes.Text = ini.IniReadValue("SerialNV", "Paid20");
            Paid10Notes.Text = ini.IniReadValue("SerialNV", "Paid10");
            Paid05Notes.Text = ini.IniReadValue("SerialNV", "Paid05");
            Paid05Coins.Text = ini.IniReadValue("PaidCOINS", "5cents");
            Paid10Coins.Text = ini.IniReadValue("PaidCOINS", "10cents");
            Paid20Coins.Text = ini.IniReadValue("PaidCOINS", "20cents");
            Paid50Coins.Text = ini.IniReadValue("PaidCOINS", "50cents");
            Paid100Coins.Text = ini.IniReadValue("PaidCOINS", "100cents");
            Paid200Coins.Text = ini.IniReadValue("PaidCOINS", "200cents");

            if ((OpenPRINTER.BackColor == Color.GreenYellow) && (p))
            {
                try
                {
                    string Filepath = "C:/POS/cashier_report.txt";
                    string strFileName = Filepath;
                    string cashier_report_date = DateTime.Now.ToString("dd/MM/yyyy");
                    string cashier_report_time = DateTime.Now.ToString("HH:mm:ss");

                    string[] lines;
                    lines = File.ReadAllLines(Filepath);
                    lines[6] = string.Concat(lines[6], Coin5.Text);
                    lines[7] = string.Concat(lines[7], Coin10.Text);
                    lines[8] = string.Concat(lines[8], Coin20.Text);
                    lines[9] = string.Concat(lines[9], Coin50.Text);
                    lines[10] = string.Concat(lines[10], Coin100.Text);
                    lines[11] = string.Concat(lines[11], Coin200.Text);
                    lines[12] = string.Concat(lines[12], tmp);
                    lines[16] = string.Concat(lines[16], Avail05Notes.Text);
                    lines[20] = string.Concat(lines[20], Paid05Notes.Text);
                    lines[21] = string.Concat(lines[21], Paid10Notes.Text);
                    lines[22] = string.Concat(lines[22], Paid20Notes.Text);
                    //lines[23] = string.Concat(lines[23], Paid50Notes.Text);
                    lines[26] = string.Concat(lines[26], Paid05Coins.Text);
                    lines[27] = string.Concat(lines[27], Paid10Coins.Text);
                    lines[28] = string.Concat(lines[28], Paid20Coins.Text);
                    lines[29] = string.Concat(lines[29], Paid50Coins.Text);
                    lines[30] = string.Concat(lines[30], Paid100Coins.Text);
                    lines[31] = string.Concat(lines[31], Paid200Coins.Text);
                    float PaidCoinsTotal = (float.Parse(Paid05Coins.Text) * 0.05f) + (float.Parse(Paid10Coins.Text) * 0.1f) + (float.Parse(Paid20Coins.Text) * 0.2f) +
                        (float.Parse(Paid50Coins.Text) * 0.5f) + (float.Parse(Paid100Coins.Text) * 1.0f) + (float.Parse(Paid200Coins.Text) * 2.0f);
                    lines[32] = string.Concat(lines[32], PaidCoinsTotal.ToString("R"));
                    lines[35] = string.Concat(lines[35], cashier_report_date);
                    lines[36] = string.Concat(lines[36], cashier_report_time);
                    File.WriteAllLines("C:/POS/cashier_report_temp.txt", lines);

                    streamToPrint = new StreamReader("C:/POS/cashier_report_temp.txt");
                    try
                    {
                        printFont = new Font("Arial", 10);
                        PrintDocument pd = new PrintDocument();
                        PrintController pc = new StandardPrintController();
                        pd.PrintController = pc;
                        pd.PrintPage += new PrintPageEventHandler
                           (this.pd_PrintPage);
                        pd.Print();
                        Display("Printer READY!\n");
                        this.Printer.Text = "Printer_OK";
                        PRINTERStatus.BackColor = Color.YellowGreen;
                    }
                    finally
                    {
                        streamToPrint.Close();
                    }
                }
                catch (Exception ex)
                {
                    OpenPRINTER.BackColor = Color.Tomato;
                    Display(ex.Message);
                }
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
            
            ini.IniWriteValue("SerialNV", "Avail05", Avail05Notes.Text); Thread.Sleep(100);
            NotesFive = int.Parse(Avail05Notes.Text);
            ini.IniWriteValue("SerialNV", "Paid50", Paid50Notes.Text); Thread.Sleep(100);
            NotesTen = int.Parse(Avail10Notes.Text);
            ini.IniWriteValue("SerialNV", "Paid20", Paid20Notes.Text); Thread.Sleep(100);
            ini.IniWriteValue("SerialNV", "Paid10", Paid10Notes.Text); Thread.Sleep(100);
            ini.IniWriteValue("SerialNV", "Paid05", Paid05Notes.Text); Thread.Sleep(100);
            ini.IniWriteValue("PaidCOINS", "5cents", Paid05Coins.Text); Thread.Sleep(100);
            ini.IniWriteValue("PaidCOINS", "10cents", Paid10Coins.Text); Thread.Sleep(100);
            ini.IniWriteValue("PaidCOINS", "20cents", Paid20Coins.Text); Thread.Sleep(100);
            ini.IniWriteValue("PaidCOINS", "50cents", Paid50Coins.Text); Thread.Sleep(100);
            ini.IniWriteValue("PaidCOINS", "100cents", Paid100Coins.Text); Thread.Sleep(100);
            ini.IniWriteValue("PaidCOINS", "200cents", Paid200Coins.Text); Thread.Sleep(100);
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
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                TCP_Connect_Click(this, e);
                this.Refresh();
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Display("SERVER ERROR:" + ex.Message.ToString());
                Cursor.Current = Cursors.Default;
            }
            if (Server_Connect.BackColor != Color.YellowGreen)
                return;
            try
            {
                OpenBR_Click(this, e); this.Refresh(); Thread.Sleep(800);
                BRStatus_Click(this, e); this.Refresh(); Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Display("BARCODE ERROR: " + ex.Message.ToString());
                Cursor.Current = Cursors.Default;
            }

            try
            {
                OpenRFID_Click_1(this, e); this.Refresh(); Thread.Sleep(500);
                RFIDStatus_Click_1(this, e); this.Refresh(); Thread.Sleep(500);
            }
            catch (Exception ex) { Display("RFID ERROR: " + ex.Message.ToString()); Cursor.Current = Cursors.Default; }

            try
            {
                OpenUX300_Click(this, e); this.Refresh(); Thread.Sleep(1000);
                UX300Status_Click(this, e); this.Refresh(); Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Display("UX300 ERROR: " + ex.Message.ToString());
                Cursor.Current = Cursors.Default;
            }

            try
            {
                OpenPRINTER_Click(this, e); this.Refresh(); Thread.Sleep(800);
                PRINTERStatus_Click(this, e); this.Refresh(); Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Display("ERROR: " + ex.Message.ToString());
                Cursor.Current = Cursors.Default;
            }

            try
            {
                OpenNV_Click(this, e); this.Refresh(); Thread.Sleep(800);
                NVStatus_Click(this, e); this.Refresh(); Thread.Sleep(1000);
                
            }
            catch (Exception ex)
            {
                Display("NV ERROR: " + ex.Message.ToString());
                Cursor.Current = Cursors.Default;
            }

            try
            {
                OpenCoins_Click(this, e); this.Refresh(); Thread.Sleep(800);
                CoinStatus_Click(this, e); this.Refresh(); Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Display("COINS ERROR: " + ex.Message.ToString());
                Cursor.Current = Cursors.Default;
            }

            p = false; Refresh_Click(this, e);

            //this.MainConfig.SelectedIndex = 4;
            this.MainConfig.TabPages[6].Parent.Focus();

            this.Init_System.BackColor = Color.GreenYellow;
            Thread.Sleep(1000);
            StartApplication.Enabled = true;
            Cursor.Current = Cursors.Default;
            btnRun_Click(this, e);
            UpdateUI();
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
            if (UX300Status.BackColor != Color.YellowGreen)
            {
                Cursor.Show();
                MessageBox.Show("UX300 Error\nCannot Start Application");
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
            GeneralTimer.Interval = 1000;//STATE MACHINE
            GeneralTimer.Start();
            Save_Click(this, e);
            //NV11.storedNoteValue = 0;
            MoneyStatus(2);
            secondForm.Show();
            if (Total_Coins < 10)//1000
            {
                Display("Money Very Low");
                SM = 11;
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/
        //Form 2 closed
        private void Closing(object sender, FormClosedEventArgs e)
        {
            //this.MainConfig.SelectedIndex = 4;
            p = false; Refresh_Click(this, e); p = true;
            //this.MainConfig.TabPages[6].Parent.Focus(); 
            this.Refresh();
            Display(" ");
            Display(" *** Application CLOSING!!! ***\n\n\n");
            Display(" ");
            Display(" ");
            if (Init_System.BackColor == Color.GreenYellow)
            {
                Save_Click(this, e); Thread.Sleep(500);
            }
            else
            {
                Display("Cannot Save on exit,Values not set\n");
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
                        if (s.Length == 0)
                            s = "NULL_LOG_DATA";
                        POS_v20.Form1.instance.Invoke((MethodInvoker)delegate
                        {
                        //s = s.Replace('\n','_');          
                        POS_v20.Form1.instance.Debugging.AppendText(DateTime.Now.ToString("\ndd/MM/yy HH:mm:ss/> ") + s);
                            POS_v20.Form1.instance.Debugging.ScrollToCaret();
                        });

                        WRfile.WriteLine(DateTime.Now.ToString("dd/MM/yy HH:mm:ss/> ") + s);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (WRfile != null) WRfile.Close();
                        }
                        catch (Exception e) { }
                    }
                }
                Thread.Sleep(1000);

            } while (true);
        }

        /**************************************************************************************/
        /**************************************************************************************/


        /**************************************************************************************/
        // ANOTHER COUNTER
        private void Language_Tick(object sender, EventArgs e)
        {
            GeneralCounter--;
            secondForm.Vprogress.Value = GeneralCounter;
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
            GeneralTimer.Stop();
            secondForm.Update();
            string Langtemp;
            byte[] EnAll = { 184 };
            byte[] DisAll = { 185 };
            byte[] AccEscrow = { 172 };
            byte[] RejEscrow = { 173 };
            byte[] NDOneBill = { 0x01, 0x10, 0x00, 0x10, 0x01, 0x22 };
            byte[] singleread = { 0x53 };//'S'

            if (secondForm.Text != "")
                Thread.Sleep(1);
            else {
                GeneralTimer.Stop();
                Display("Form2 Closed");
                p = false;
                Refresh_Click(this, e);
                Cursor.Show();
                return;
            }
            if (secondForm.CancelButton)
            {
                secondForm.CancelButton = false;
                if (secondForm.CreditCardPayment)
                {
                    TransactionType = new byte[2] { 0x30, 0x35 };//reversal
                    string SystemId = Encoding.UTF8.GetString(SystemIdentification);
                    Display("System ID: " + Encoding.UTF8.GetString(SystemIdentification));
                    Display("Transaction Type: " + Encoding.UTF8.GetString(TransactionType));
                    SystemIdentification = Encoding.UTF8.GetBytes(SystemId);
                    BytestoSend = new byte[SystemIdentification.Length + TransactionType.Length + ETX.Length];
                    Buffer.BlockCopy(SystemIdentification, 0, BytestoSend, 0, SystemIdentification.Length);
                    Buffer.BlockCopy(TransactionType, 0, BytestoSend, SystemIdentification.Length, TransactionType.Length);
                    Buffer.BlockCopy(ETX, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length), ETX.Length);
                    LRC[0] = CalculateLRC(BytestoSend, BytestoSend.Length);
                    //////
                    //////start serial send
                    //////
                    serialEFT_POS.Write(STX, 0, STX.Length);
                    serialEFT_POS.Write(SystemIdentification, 0, SystemIdentification.Length);
                    serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                    serialEFT_POS.Write(TransactionType, 0, TransactionType.Length);
                    serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);

                    for (int i = 0; i < 20; i++)
                    {
                        serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                    }

                    serialEFT_POS.Write(ETX, 0, ETX.Length);
                    serialEFT_POS.Write(LRC, 0, LRC.Length);
                    SM = 2;
                    Display("CANCEL BUTTON PRESSED @ CREDIT PAYMENT\n");                    
                }
                else if (secondForm.CashPayment)
                {
                    SM = 10;
                    Display("CANCEL BUTTON PRESSED @ CASH PAYMENT\n");
                }
                else
                {
                    SM = 10;
                    Display("CANCEL BUTTON PRESSED\n");
                }
            }
            if (Server_Connect.BackColor == Color.Tomato) {
                SM = 11;
                Display("FATAL NETWORK ERROR");
                return;
            }
            switch (SM) {

                case 1: //idle state - Welcome Screen
                    Display("SM: " + SM);
                    if (!cardpresense)
                        serialRFID.Write(singleread, 0, singleread.Length);
                    DisableChangeNotes = false;
                    FiveEuroNoteIn = false;
                    TenEuroNoteIn = false;
                    TwentyEuroNoteIn = false;
                    Payout.DisableValidator();
                    textBox1.Text = "";
                    textBox1.ScrollToCaret();
                    secondForm.Cancel.Visible = false;
                    ShowNotes("000");
                    InitalCost = 0;
                    Payment = 0;
                    ReturnMoney = 0;
                    secondForm.Card_Icon.Visible = true;
                    secondForm.Card_Icon.BringToFront();
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Vprogress.Visible = false;
                    secondForm.Messages2.Visible = false;
                    secondForm.btnYes.Visible = false;
                    secondForm.btnNo.Visible = false;
                    secondForm.lostButton.Visible = true;
                    Langtemp = ini.IniReadValue("LANGUAGE", "insert" + secondForm.Language);

                    if (secondForm.POS_Messages.Text.IndexOf(Langtemp) != 0)
                    {
                        secondForm.POS_Messages.Clear();
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                    }

                    Image image;
                    secondForm.Ticket_Icon.Visible = true;

                    if (secondForm.Language.Length <= 1)
                    {
                        secondForm.Language = "GRE";
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
                        secondForm.lostButton.Text = "Έαν έχετε χάσει το εισιτήριό σας,πατήστε την ενδοεπικοινωνία.";
                        secondForm.cashButton.Text = "Μετρητά";
                        secondForm.creditButton.Text = "Πιστωτική - Χρεωστική Κάρτα";
                        secondForm.Cancel.Text = "ΑΚΥΡΩΣΗ";
                        secondForm.ValueLabel.Text = "Σύνολο: ";
                        secondForm.PaymentLabel.Text = "Ληφθέν: ";
                        secondForm.ResidualLabel.Text = "Υπόλοιπο: ";
                        secondForm.Refresh();
                    }
                    else if (secondForm.Language.StartsWith("ENG") == true)
                    {
                        secondForm.buttonUK.Visible = false;
                        secondForm.buttonGRE.Visible = true;
                        secondForm.buttonGER.Visible = true;
                        secondForm.buttonFRA.Visible = true;
                        secondForm.lostButton.Text = "If you have lost your ticket,press the intercom.";
                        secondForm.cashButton.Text = "Cash";
                        secondForm.creditButton.Text = "Credit - Debit Card";
                        secondForm.Cancel.Text = "CANCEL";
                        secondForm.ValueLabel.Text = "Total: ";
                        secondForm.PaymentLabel.Text = "Paid: ";
                        secondForm.ResidualLabel.Text = "Residual: ";
                        secondForm.Refresh();
                    }
                    else if (secondForm.Language.StartsWith("FRA") == true)
                    {
                        secondForm.buttonUK.Visible = true;
                        secondForm.buttonGRE.Visible = true;
                        secondForm.buttonGER.Visible = true;
                        secondForm.buttonFRA.Visible = false;
                        secondForm.lostButton.Text = "Si vous avez perdu votre billet,appuyez sur l'interphone.";
                        secondForm.Cancel.Text = "ANNULER";
                        secondForm.ValueLabel.Text = "Ensemble: ";
                        secondForm.PaymentLabel.Text = "Reçu: ";
                        secondForm.ResidualLabel.Text = "Repos: ";
                        secondForm.Refresh();
                    }
                    else if (secondForm.Language.StartsWith("GER") == true)
                    {
                        secondForm.buttonUK.Visible = true;
                        secondForm.buttonGRE.Visible = true;
                        secondForm.buttonGER.Visible = false;
                        secondForm.buttonFRA.Visible = true;
                        secondForm.lostButton.Text = "Wenn Sie Ihr Ticket verloren haben,drücken Sie die Gegensprechanlage.";
                        secondForm.cashButton.Text = "Bargeld";
                        secondForm.creditButton.Text = "Kredit - Debitkarte";
                        secondForm.Cancel.Text = "STORNIEREN";
                        secondForm.ValueLabel.Text = "Insgesamt: ";
                        secondForm.PaymentLabel.Text = "Erhalten: ";
                        secondForm.ResidualLabel.Text = "Ruhe: ";
                        secondForm.Refresh();
                    }
                    if(secondForm.LostButton)
                    {
                        secondForm.lostButton.Visible = false;
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "lost" + secondForm.Language);
                        secondForm.POS_Messages.AppendText(Langtemp);
                        lostTimer.Enabled = true;
                        SM = 23;
                        break;
                    }

                    if (BR_Card.Length > 0)
                    {

                        if (BR_Card.Length != BC_Lenght)
                        {
                            if (BR_Card == "ADMIN")
                            {
                                BR_Card = "";
                                secondForm.Close();
                                SM = 31;
                                break;
                            }
                            SM = 31;
                            break;
                        }
                        secondForm.POS_Messages.Clear();
                        Langtemp = ini.IniReadValue("LANGUAGE", "wait" + secondForm.Language);
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Messages2.Visible = false;
                        UserCode = BR_Card;
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();
                        secondForm.buttonUK.Visible = false;
                        secondForm.buttonGRE.Visible = false;
                        secondForm.buttonGER.Visible = false;
                        secondForm.buttonFRA.Visible = false;
                        secondForm.lostButton.Visible = false;
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Card_Icon.Visible = false;
                        ShowNotes("000");
                        secondForm.Refresh();
                        SM = 4;
                        BR_Card = ""; 
                        RF_Card = "";
                        break;
                    }
                    if (RF_Card.Length != 0)
                    {
                        secondForm.Messages2.Visible = true;
                        UserCode = RF_Card;
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Cancel.Visible = true;
                        secondForm.lostButton.Visible = false;
                        secondForm.buttonUK.Visible = false;
                        secondForm.buttonGRE.Visible = false;
                        secondForm.buttonGER.Visible = false;
                        secondForm.buttonFRA.Visible = false;
                        secondForm.Refresh();
                        LanguageTimer.Start();
                        SM = 41;
                        BR_Card = "";
                        RF_Card = "";
                    }
                    A = DateTime.Now;
                    Thread.Sleep(100);
                    isMidnight = A.Date == B.Date;
                    if (!isMidnight && !batchIsClosed)
                    {
                        CloseBatchButton_Click(this, e);
                        SM = 21;
                        break;
                    }
                    break;

                case 2: //Codeless state to wait UX300 close batch event
                    Display("SM: " + SM);
                    Display("waiting for the UX300 to cancel the transaction");
                    break;

                case 21:    //Codeless state to wait UX300 close batch event
                    Display("SM: " + SM);
                    Display("waiting for the UX300 to close batch...");
                    break;

                case 22:    //Code for coupons - Not used
                    Display("SM: " + SM);
                    Langtemp = ini.IniReadValue("LANGUAGE", "Coupon" + secondForm.Language);
                    if (secondForm.POS_Messages.Text.IndexOf(Langtemp) != 0)
                    {
                        secondForm.btnYes.Visible = true;
                        secondForm.btnNo.Visible = true;
                        secondForm.POS_Messages.Clear();
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();
                    }
                    if (secondForm.btnYN != 0)
                    {
                        LanguageTimer.Stop();
                        secondForm.Vprogress.Visible = false;
                        SM = 4;
                        secondForm.Vprogress.Maximum = 120;
                        secondForm.Vprogress.Value = 120;
                        GeneralCounter = 120;
                        secondForm.Vprogress.Visible = true;
                        LanguageTimer.Start();
                        if (secondForm.btnYN == 1) 
                        {
                            UserHasCpn = true;
                            SM = 23;
                            secondForm.POS_Messages.Clear();
                            Langtemp = ini.IniReadValue("LANGUAGE", "ScanCpn" + secondForm.Language);
                            secondForm.POS_Messages.AppendText(Langtemp);

                            Image im;
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
                    if (BR_Card.Length != 0)
                    {
                        if (BR_Card.Length != BC_Lenght)
                        {
                            if (BR_Card == "ADMIN")
                            {
                                BR_Card = "";
                                secondForm.Close();
                                SM = 31;
                                break;
                            }
                            break;
                        }
                        else
                        {
                            BR_Card = "";
                        }
                        BR_Card = ""; 
                        RF_Card = ""; //3
                    }
                    break;

            case 31:    //Display handler for wrong barcode length
                    Display("SM: " + SM);
                    secondForm.POS_Messages.Clear();
                    InitalCost = 0;
                    Payment = 0;
                    ReturnMoney = 0;
                    Langtemp = ini.IniReadValue("LANGUAGE", "Try" + secondForm.Language);
                    secondForm.POS_Messages.Text = Langtemp;
                    LanguageTimer.Stop();
                    secondForm.Vprogress.Maximum = 120;
                    secondForm.Vprogress.Value = 120;
                    GeneralCounter = 120;
                    LanguageTimer.Start();
                    secondForm.Refresh();
                    Thread.Sleep(2500);
                    secondForm.POS_Messages.Clear();
                    BR_Card = ""; 
                    RF_Card = "";
                    SM = 1;                 
                    break;

                case 4:     //Send ticket (barcode) to server
                    Display("SM: " + SM);
                    secondForm.Messages2.Clear();
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Messages2.Visible = true;

                    if (UserCode.Length > 0)
                    {
                        if (secondForm.Language.StartsWith("GRE"))
                            secondForm.Messages2.AppendText("\n Εισιτήριο: " + UserCode);
                        else if (secondForm.Language.StartsWith("ENG"))
                            secondForm.Messages2.AppendText("\n Ticket: " + UserCode);
                        else if (secondForm.Language.StartsWith("FRA"))
                            secondForm.Messages2.AppendText("\n Billet: " + UserCode);
                        else if (secondForm.Language.StartsWith("GER"))
                            secondForm.Messages2.AppendText("\n Farhkarte: " + UserCode);
                        string url = HTTPURLTICKET + UserCode;
                        Display("requesting url: " + url);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        Display("webrequest url ok");
                        request.Method = "GET";
                        request.Headers.Add("Authorization", "Bearer " + ApiToken);
                        try
                        {
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            Display("web response ok");
                            Stream responseStream = response.GetResponseStream();
                            Display("get response ok");
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            Display("stream reader ok");
                            string entry_time;
                            string plate;
                            var jsonobject = reader.ReadToEnd();
                            Display("JSON_object is: " + jsonobject);
                            var responseData = JObject.Parse(jsonobject);
                            Display("+++" + responseData.ToString());
                            exception = (string)responseData["exception"];
                            if (exception == "Symfony\\Component\\HttpKernel\\Exception\\NotFoundHttpException")
                            {
                                //not found in parking
                                secondForm.Ticket_Icon.Visible = false;
                                secondForm.POS_Messages.Clear();
                                Langtemp = ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language);
                                secondForm.POS_Messages.AppendText(Langtemp);
                                secondForm.Refresh(); Thread.Sleep(2000);
                                SM = 10;
                                Display("not found exception reached...");
                                break;
                            }
                            is_paid = (string)responseData["ticket"]["is_paid"];
                            charge = (string)responseData["charge"];
                            Display("is_paid = " + is_paid);
                            if (is_paid == "True" && charge == "0")
                            {
                                string final_exit_time = (string)responseData["ticket"]["final_exit_time"];
                                secondForm.Ticket_Icon.Visible = false;
                                secondForm.POS_Messages.Clear();
                                Langtemp = ini.IniReadValue("LANGUAGE", "TIC4" + secondForm.Language);
                                secondForm.POS_Messages.AppendText(Langtemp + " " + final_exit_time);
                                secondForm.Refresh();
                                SM = 10;
                                Thread.Sleep(4000);
                                Display("already paid exception reached...");
                                break;
                            }
                            else
                            {
                                charge = (string)responseData["charge"];
                                charge_double = (double)responseData["charge"];
                                Display("charge : " + charge);
                                Display("charge_double : " + charge_double);
                                charge_double = Math.Round(charge_double, 2, MidpointRounding.AwayFromZero);
                                Display("charge_double rounded : " + charge_double);
                                value = charge;
                                charge_until = (string)responseData["charge_until"];
                                Display("charge_until: " + charge_until);
                                plate = (string)responseData["ticket"]["ticket_entrance"]["vehicle"]["plate"];
                                Display("vehicle_plate: " + plate);
                                entry_time = (string)responseData["ticket"]["ticket_entrance"]["entered_at"];
                                Display("entered_at: " + entry_time);
                                if (secondForm.Language.StartsWith("GRE"))
                                    secondForm.Messages2.AppendText("\n\n Αριθμός Κυκλοφορίας: " + plate + "\n\n Είσοδος: " + entry_time + "\n\n Έξοδος: " + charge_until + "\n\n Αξία: " + value);
                                else if (secondForm.Language.StartsWith("ENG"))
                                    secondForm.Messages2.AppendText("\n\n Vehicle Plate: " + plate + "\n\n Entry: " + entry_time + "\n\n Exit: " + charge_until + "\n\n Value: " + value);
                                else if (secondForm.Language.StartsWith("FRA"))
                                    secondForm.Messages2.AppendText("\n\n Plaque de Véhicule: " + plate + "\n\n Entrée: " + entry_time + "\n\n Sortie: " + charge_until + "\n\n Valeur: " + value);
                                else if (secondForm.Language.StartsWith("GER"))
                                    secondForm.Messages2.AppendText("\n\n Fahrzeugplatte: " + plate + "\n\n Eintrag: " + entry_time + "\n\n Ausfahrt: " + charge_until + "\n\n Wert: " + value);
                                try
                                {
                                    snapshot = (string)responseData["ticket"]["ticket_entrance"]["snapshot"];
                                    Display("snapshot address: " + snapshot);
                                }
                                catch (WebException ex)
                                {
                                    Display("No available snapshot");
                                    Display(ex.ToString());
                                }
                                response.Close();
                                LanguageTimer.Stop();
                                secondForm.Vprogress.Visible = false;
                                secondForm.Vprogress.Maximum = 120;
                                secondForm.Vprogress.Value = 120;
                                GeneralCounter = 120;
                                LanguageTimer.Start();
                                SM = 5;
                                ticket_scanned = true;
                                secondForm.POS_Messages.Clear();
                                secondForm.Cancel.Visible = true;
                                secondForm.cashButton.Location = new Point(24, 257);
                                secondForm.creditButton.Location = new Point(24, 362);
                                secondForm.cashButton.Visible = true;
                                secondForm.creditButton.Visible = true;
                                secondForm.Vprogress.Maximum = 120;
                                secondForm.Vprogress.Value = 120;
                                GeneralCounter = 120;
                                secondForm.Vprogress.Visible = true;
                                break;
                            }

                        }
                        catch (WebException ex)
                        {
                            HttpWebResponse response = (HttpWebResponse)ex.Response;
                            switch (response.StatusCode)
                            {
                                case HttpStatusCode.NotFound:
                                    //not found in parking
                                    secondForm.Ticket_Icon.Visible = false;
                                    secondForm.POS_Messages.Clear();
                                    Langtemp = ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language);
                                    secondForm.POS_Messages.AppendText(Langtemp);
                                    secondForm.Refresh(); Thread.Sleep(2000);
                                    SM = 10;
                                    Display("not found exception reached...");
                                    break;
                                case HttpStatusCode.InternalServerError:
                                    SM = 31;
                                    break;
                                case HttpStatusCode.ServiceUnavailable:
                                    SM = 31;
                                    break;
                            }

                        }

                    }
                    break;

                case 41:    //Send card (rfid) to server
                    Display("SM: " + SM);
                    secondForm.Messages2.Clear();
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Card_Icon.Visible = false;
                    secondForm.Messages2.Visible = true;

                    if (UserCode.Length != 0) {
                        if (secondForm.Language.StartsWith("GRE"))
                            secondForm.Messages2.AppendText("\nΑριθμός Κάρτας: " + UserCode);
                        else if (secondForm.Language.StartsWith("ENG"))
                            secondForm.Messages2.AppendText("\n Card number: " + UserCode);
                        else if (secondForm.Language.StartsWith("FRA"))
                            secondForm.Messages2.AppendText("\n Numéro de carte: " + UserCode);
                        else if (secondForm.Language.StartsWith("GER"))
                            secondForm.Messages2.AppendText("\n Kartennummer: " + UserCode);
                        Refresh();
                        string requestUriString = HTTPURLCARD + UserCode;
                        Display("requesting url: " + requestUriString);
                        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                        Display("webrequest url ok");
                        httpWebRequest.Method = "GET";
                        httpWebRequest.Headers.Add("Authorization", "Bearer " + ApiToken);
                        try
                        {
                            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                            Display("web response ok");
                            Stream responseStream = response.GetResponseStream();
                            Display("get response ok");
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            Display("stream reader ok");
                            var jsonobject = reader.ReadToEnd();
                            Display("JSON_object is: " + jsonobject);
                            var responseData = JObject.Parse(jsonobject);
                            Display("+++" + responseData.ToString());
                            customer = (string)responseData["customer"]["name"];
                            Display("Customer Info: " + customer);
                            validto = (string)responseData["last_subscription"]["end"];
                            Display("Valid subscription to: " + validto);
                            charge = (string)responseData["renewal"]["charge"];
                            Display("Charge: " + charge);
                            charge_double = (double)responseData["renewal"]["charge"];
                            Display("charge_double : " + charge_double);
                            charge_double = Math.Round(charge_double, 2, MidpointRounding.AwayFromZero);
                            Display("charge_double rounded : " + charge_double);
                            value = charge;
                            validfrom = (string)responseData["renewal"]["begin"];
                            Display("valid_from: " + validfrom);
                            validto = (string)responseData["renewal"]["end"];
                            Display("valid_to: " + validto);
                            subscriptionId = (string)responseData["last_subscription"]["id"];
                            Display("subscription_id: " + subscriptionId);
                            response.Close();
                            UserCoupon = "";
                            LanguageTimer.Stop();
                            secondForm.Vprogress.Visible = false;
                            secondForm.Vprogress.Maximum = 120;
                            secondForm.Vprogress.Value = 120;
                            GeneralCounter = 120;
                            LanguageTimer.Start();
                            LanguageTimer.Stop();
                            secondForm.POS_Messages.Clear();
                            secondForm.Cancel.Visible = true;
                            secondForm.cashButton.Location = new Point(24, 257);
                            secondForm.creditButton.Location = new Point(24, 362);
                            secondForm.cashButton.Visible = true;
                            secondForm.creditButton.Visible = true;
                            secondForm.Vprogress.Visible = true;
                            if (secondForm.Language.StartsWith("GRE"))
                                secondForm.Messages2.AppendText("\n\nΣυνδρομητής: " + customer + "\n\nΣυνδρομή έως: " + validto + "\n\nΑξία Ανανέωσης: " + value);
                            else if (secondForm.Language.StartsWith("ENG"))
                                secondForm.Messages2.AppendText("\n\nCustomer: " + customer + "\n\nValid to: " + validto + "\n\nRenewal Value: " + value);
                            else if (secondForm.Language.StartsWith("FRA"))
                                secondForm.Messages2.AppendText("\n\nClient/Cliente: " + customer + "\n\nValable pour: " + validto + "\n\nValeur de Renouvellement: " + value);
                            else if (secondForm.Language.StartsWith("GER"))
                                secondForm.Messages2.AppendText("\n\nKunde/Kundin: " + customer + "\n\nGültig bis: " + validto + "\n\nErneuerungswert: " + value);
                            SM = 51;
                            card_scanned = true;
                            secondForm.POS_Messages.Clear();
                            break;
                        }
                        catch (WebException ex)
                        {
                            switch (((HttpWebResponse)ex.Response).StatusCode)
                            {
                                case HttpStatusCode.NotFound:
                                    secondForm.Ticket_Icon.Visible = false;
                                    secondForm.POS_Messages.Clear();
                                    secondForm.POS_Messages.AppendText(ini.IniReadValue("LANGUAGE", "TIC2" + secondForm.Language));
                                    secondForm.Refresh();
                                    Thread.Sleep(2000);
                                    SM = 10;
                                    Display("not found exception reached...");
                                    break;
                                case HttpStatusCode.InternalServerError:
                                    SM = 31;
                                    break;
                                case HttpStatusCode.ServiceUnavailable:
                                    SM = 31;
                                    break;
                            }
                        }
                    }
                    break;

                case 5: //Get ticket cost - Select payment method
                    Display("SM: " + SM);                   
                    if (value != "0")
                    {
                        if (secondForm.CashPayment)
                        {
                            DNote10 = 0;
                            DNote5 = 0;
                            Payout.EnableValidator();   //enable note recycler
                            Payout.EnablePayout();
                            Display("Asking_for_money...");
                            if (pm.set(0, 0, 0, 0) == 0)//ACCEPT COINS
                                Display("START PAYMENT_ACCEPT Coins");
                            InitalCost = int.Parse((charge_double * 100) + "");
                            Display("InitalCost " + InitalCost.ToString());
                            string CheckLevelInfo = Payout.GetChannelLevelInfo();
                            string[] NoteLevel = CheckLevelInfo.Split('[', ']');
                            for (int count = 1; count < NoteLevel.Length; count = count += 2)
                            {
                                switch (count)
                                {
                                    case 1:
                                        Display("5 EUR Level: " + NoteLevel[count]);
                                        FiveEuroNotesLevel = NoteLevel[count];
                                        break;
                                    case 3:
                                        Display("10 EUR Level: " + NoteLevel[count]);
                                        TenEuroNotesLevel = NoteLevel[count];
                                        break;
                                    case 5:
                                        Display("20 EUR Level: " + NoteLevel[count]);
                                        TwentyEuroNotesLevel = NoteLevel[count];
                                        break;
                                    default:
                                        break;
                                }
                            }
                            Langtemp = ini.IniReadValue("LANGUAGE", "pay" + secondForm.Language);
                            secondForm.POS_Messages.Clear();
                            secondForm.POS_Messages.AppendText(Langtemp);
                            secondForm.ReaderPictureBox.Visible = true;
                            ShowNotes("05,10,20");
                            secondForm.Vprogress.Maximum = 120;
                            secondForm.Vprogress.Value = 120;
                            GeneralCounter = 120;
                            secondForm.Vprogress.Visible = true;
                            secondForm.ValueText.Visible = true;
                            secondForm.PaymentText.Visible = true;
                            secondForm.ResidualText.Visible = true;
                            SM = 6;
                            break;
                        }
                        else if (secondForm.CreditCardPayment)
                        {
                            Display("Asking_for_money ticket...");
                            InitalCost = int.Parse((charge_double * 100) + "");
                            Display("InitalCost " + InitalCost.ToString());
                            SystemIdentification = Encoding.ASCII.GetBytes(DateTime.Now.ToString("ddMMyyyyHHmmss"));
                            TransactionType = new byte[2] { 0x30, 0x30 };
                            OrigTransactionAmount = Encoding.ASCII.GetBytes(InitalCost.ToString());
                            Display("System ID: " + Encoding.UTF8.GetString(SystemIdentification));
                            Display("Transaction Type: " + Encoding.UTF8.GetString(TransactionType));
                            Display("Original Transaction Amount: " + Encoding.UTF8.GetString(OrigTransactionAmount));
                            ExtMerchantID = Encoding.ASCII.GetBytes(ini.IniReadValue("Params", "Ext_Merchant_ID"));
                            Display("External Merchant ID: " + ini.IniReadValue("Params", "Ext_Merchant_ID"));
                            ExtTerminalID = Encoding.ASCII.GetBytes(ini.IniReadValue("Params", "Ext_Terminal_ID"));
                            Display("External Terminal ID: " + ini.IniReadValue("Params", "Ext_Terminal_ID"));
                            BytestoSend = new byte[SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length + ExtMerchantID.Length + ExtTerminalID.Length + ETX.Length];
                            Buffer.BlockCopy(SystemIdentification, 0, BytestoSend, 0, SystemIdentification.Length);
                            Buffer.BlockCopy(TransactionType, 0, BytestoSend, SystemIdentification.Length, TransactionType.Length);
                            Buffer.BlockCopy(OrigTransactionAmount, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length), OrigTransactionAmount.Length);
                            Buffer.BlockCopy(ExtMerchantID, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length), ExtMerchantID.Length);
                            Buffer.BlockCopy(ExtTerminalID, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length + ExtMerchantID.Length), ExtTerminalID.Length);
                            Buffer.BlockCopy(ETX, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length + ExtMerchantID.Length + ExtTerminalID.Length), ETX.Length);
                            LRC[0] = CalculateLRC(BytestoSend, BytestoSend.Length);
                            //////
                            //////start serial send
                            //////
                            serialEFT_POS.Write(STX, 0, STX.Length);
                            serialEFT_POS.Write(SystemIdentification, 0, SystemIdentification.Length);
                            serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            serialEFT_POS.Write(TransactionType, 0, TransactionType.Length);
                            serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            serialEFT_POS.Write(OrigTransactionAmount, 0, OrigTransactionAmount.Length);

                            for (int i = 0; i < 9; i++)
                            {
                                serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            }

                            serialEFT_POS.Write(ExtTerminalID, 0, ExtTerminalID.Length);
                            serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            serialEFT_POS.Write(ExtMerchantID, 0, ExtMerchantID.Length);

                            for (int i = 0; i < 10; i++)
                            {
                                serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            }

                            serialEFT_POS.Write(ETX, 0, ETX.Length);
                            serialEFT_POS.Write(LRC, 0, LRC.Length);
                            secondForm.ReaderPictureBox.Visible = false;
                            Langtemp = ini.IniReadValue("LANGUAGE", "pay" + secondForm.Language);
                            secondForm.POS_Messages.Clear();
                            secondForm.POS_Messages.AppendText(Langtemp);
                            secondForm.ReaderPictureBox.Visible = false;
                            ShowNotes("0,0,0");
                            secondForm.Vprogress.Maximum = 120;
                            secondForm.Vprogress.Value = 120;
                            GeneralCounter = 120;
                            secondForm.Vprogress.Visible = true;
                            secondForm.ValueText.Visible = true;
                            SM = 62;
                            break;
                        }
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Card_Icon.Visible = false;
                        secondForm.ReaderPictureBox.Visible = false;
                        Langtemp = ini.IniReadValue("LANGUAGE", "paymethod" + secondForm.Language);
                        secondForm.POS_Messages.Clear();
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.POS_Messages.Visible = true;
                        InitalCost = int.Parse((charge_double * 100) + "");
                        value = InitalCost.ToString();
                        if (value.Length > 2)
                            value = value.Insert(value.Length - 2, ",");
                        else if (value.Length == 2)
                            value = "0," + value;
                        else
                            value = "0,0" + value;
                        secondForm.ValueLabel.Visible = true;
                        secondForm.ValueText.Text = value;
                        SM = 5;
                        break;
                    }
                    break;

                case 51:    //Get card cost - Select payment method
                    Display("SM: " + SM);
                    if (value != "0")
                    {
                        if (secondForm.CashPayment)
                        {
                            DNote10 = 0;
                            DNote5 = 0;
                            Payout.EnableValidator();
                            Payout.EnablePayout();
                            Display("Asking_for_money...");
                            if (pm.set(0, 0, 0, 0) == 0)//ACCEPT COINS
                                Display("START PAYMENT_ACCEPT Coins");
                            InitalCost = int.Parse((charge_double * 100) + "");
                            Display("InitalCost " + InitalCost.ToString());
                            string[] strArray = Payout.GetChannelLevelInfo().Split('[', ']');
                            int num2;
                            for (int index = 1; index < strArray.Length; index = num2 = index + 2)
                            {
                                switch (index)
                                {
                                    case 1:
                                        Display("5 EUR Level: " + strArray[index]);
                                        break;
                                    case 3:
                                        Display("10 EUR Level: " + strArray[index]);
                                        break;
                                    case 5:
                                        Display("20 EUR Level: " + strArray[index]);
                                        break;
                                }
                            }
                            secondForm.Ticket_Icon.Visible = false;
                            secondForm.Card_Icon.Visible = false;
                            secondForm.ValueText.Text = value;
                            Langtemp = ini.IniReadValue("LANGUAGE", "pay" + secondForm.Language);
                            secondForm.POS_Messages.Clear();
                            secondForm.POS_Messages.AppendText(Langtemp);
                            secondForm.Vprogress.Maximum = 120;
                            secondForm.Vprogress.Value = 120;
                            GeneralCounter = 120;
                            secondForm.Vprogress.Visible = true;
                            secondForm.PaymentText.Visible = true;
                            secondForm.ResidualText.Visible = true;
                            SM = 61;
                            break;
                        }
                        else if (secondForm.CreditCardPayment)
                        {
                            Display("Asking_for_money card...");
                            InitalCost = int.Parse((charge_double * 100) + "");
                            Display("InitalCost " + InitalCost.ToString());
                            SystemIdentification = Encoding.ASCII.GetBytes(DateTime.Now.ToString("ddMMyyyyHHmmss"));
                            TransactionType = new byte[2] { 0x30, 0x30 };
                            OrigTransactionAmount = Encoding.ASCII.GetBytes(InitalCost.ToString());
                            Display("System ID: " + Encoding.UTF8.GetString(SystemIdentification));
                            Display("Transaction Type: " + Encoding.UTF8.GetString(TransactionType));
                            Display("Original Transaction Amount: " + Encoding.UTF8.GetString(OrigTransactionAmount));
                            ExtMerchantID = Encoding.ASCII.GetBytes(ini.IniReadValue("Params", "Ext_Merchant_ID"));
                            Display("External Merchant ID: " + ini.IniReadValue("Params", "Ext_Merchant_ID"));
                            ExtTerminalID = Encoding.ASCII.GetBytes(ini.IniReadValue("Params", "Ext_Terminal_ID"));
                            Display("External Terminal ID: " + ini.IniReadValue("Params", "Ext_Terminal_ID"));
                            BytestoSend = new byte[SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length + ExtMerchantID.Length + ExtTerminalID.Length + ETX.Length];
                            Buffer.BlockCopy(SystemIdentification, 0, BytestoSend, 0, SystemIdentification.Length);
                            Buffer.BlockCopy(TransactionType, 0, BytestoSend, SystemIdentification.Length, TransactionType.Length);
                            Buffer.BlockCopy(OrigTransactionAmount, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length), OrigTransactionAmount.Length);
                            Buffer.BlockCopy(ExtMerchantID, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length), ExtMerchantID.Length);
                            Buffer.BlockCopy(ExtTerminalID, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length + ExtMerchantID.Length), ExtTerminalID.Length);
                            Buffer.BlockCopy(ETX, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length + OrigTransactionAmount.Length + ExtMerchantID.Length + ExtTerminalID.Length), ETX.Length);
                            LRC[0] = CalculateLRC(BytestoSend, BytestoSend.Length);
                            //////
                            //////start serial send
                            //////
                            serialEFT_POS.Write(STX, 0, STX.Length);
                            serialEFT_POS.Write(SystemIdentification, 0, SystemIdentification.Length);
                            serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            serialEFT_POS.Write(TransactionType, 0, TransactionType.Length);
                            serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            serialEFT_POS.Write(OrigTransactionAmount, 0, OrigTransactionAmount.Length);

                            for (int i = 0; i < 9; i++)
                            {
                                serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            }

                            serialEFT_POS.Write(ExtTerminalID, 0, ExtTerminalID.Length);
                            serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            serialEFT_POS.Write(ExtMerchantID, 0, ExtMerchantID.Length);

                            for (int i = 0; i < 10; i++)
                            {
                                serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
                            }

                            serialEFT_POS.Write(ETX, 0, ETX.Length);
                            serialEFT_POS.Write(LRC, 0, LRC.Length);
                            secondForm.Ticket_Icon.Visible = false;
                            secondForm.Card_Icon.Visible = false;
                            secondForm.ValueText.Text = value;
                            Langtemp = ini.IniReadValue("LANGUAGE", "pay" + secondForm.Language);
                            secondForm.POS_Messages.Clear();
                            secondForm.POS_Messages.AppendText(Langtemp);
                            secondForm.Vprogress.Maximum = 120;
                            secondForm.Vprogress.Value = 120;
                            secondForm.Vprogress.Visible = true;
                            GeneralCounter = 120;
                            SM = 63;
                            break;
                        }
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Card_Icon.Visible = false;
                        secondForm.ReaderPictureBox.Visible = false;
                        Langtemp = ini.IniReadValue("LANGUAGE", "paymethod" + secondForm.Language);
                        secondForm.POS_Messages.Clear();
                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.POS_Messages.Visible = true;
                        InitalCost = int.Parse((charge_double * 100) + "");
                        value = InitalCost.ToString();
                        if (value.Length > 2)
                            value = value.Insert(value.Length - 2, ",");
                        else if (value.Length == 2)
                            value = "0," + value;
                        else
                            value = "0,0" + value;
                        secondForm.ValueLabel.Visible = true;
                        secondForm.ValueText.Text = value;
                        SM = 51;
                        break;
                    }
                    break;

                case 52:
                    break;

                case 6: //Accept Notes and Coins - Ticket
                    Display("SM: " + SM);
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Card_Icon.Visible = false;

                    int PollNumber = Payout.DoPoll(textBox1).Item2;
                    Display("SmartPayout Poll response: " + PollNumber.ToString());
                    Display(textBox1.Text);
                    if (textBox1.Text.IndexOf("Credit 5.00") != -1)
                    {
                        FiveEuroNoteIn = true;
                        TenEuroNoteIn = false;
                        TwentyEuroNoteIn = false;
                        //textBox1.Text = "";
                        Display("5 Euro note added");
                    }
                    if (textBox1.Text.IndexOf("Credit 10.00") != -1)
                    {
                        FiveEuroNoteIn = false;
                        TenEuroNoteIn = true;
                        TwentyEuroNoteIn = false;
                        //textBox1.Text = "";
                        Display("10 Euro note added");
                    }
                    if (textBox1.Text.IndexOf("Credit 20.00") != -1)
                    {
                        FiveEuroNoteIn = false;
                        TenEuroNoteIn = false;
                        TwentyEuroNoteIn = true;
                        //textBox1.Text = "";
                        Display("20 Euro note added");
                    }
                    if (textBox1.Text.IndexOf("Note stacked") != -1)
                    {
                        if (FiveEuroNoteIn)
                        {
                            FiveEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            Payment = Payment + 500;
                            GeneralCounter = 120;
                            Paid05Notes.Text = ini.IniReadValue("SerialNV", "Paid05");
                            int five = int.Parse(Paid05Notes.Text);
                            five++;
                            ini.IniWriteValue("SerialNV", "Paid05", five.ToString());
                            textBox1.Text = "";
                            Display("Payment: " + Payment.ToString());
                        }
                        else if (TenEuroNoteIn)
                        {
                            FiveEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            Payment = Payment + 1000;
                            GeneralCounter = 120;
                            Paid10Notes.Text = ini.IniReadValue("SerialNV", "Paid10");
                            int ten = int.Parse(Paid10Notes.Text);
                            ten++;
                            ini.IniWriteValue("SerialNV", "Paid10", ten.ToString());
                            textBox1.Text = "";
                            Display("Payment: " + Payment.ToString());
                        }
                        else if (TwentyEuroNoteIn)
                        {
                            FiveEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            Payment = Payment + 2000;
                            GeneralCounter = 120;
                            Paid20Notes.Text = ini.IniReadValue("SerialNV", "Paid20");
                            int twenty = int.Parse(Paid20Notes.Text);
                            twenty++;
                            ini.IniWriteValue("SerialNV", "Paid20", twenty.ToString());
                            textBox1.Text = "";
                            Display("Payment: " + Payment.ToString());
                        }
                    }
                    if (textBox1.Text.IndexOf("Note stored") != -1)
                    {
                        if (FiveEuroNoteIn)
                        {
                            FiveEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            Payment = Payment + 500;
                            GeneralCounter = 120;
                            Avail05Notes.Text = ini.IniReadValue("SerialNV", "Avail05");
                            int five = int.Parse(Avail05Notes.Text);
                            five++;
                            ini.IniWriteValue("SerialNV", "Avail05", five.ToString());
                            textBox1.Text = "";
                            Display("Payment: " + Payment.ToString());
                        }
                        else if (TenEuroNoteIn)
                        {
                            FiveEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            Payment = Payment + 1000;
                            GeneralCounter = 120;
                            Avail10Notes.Text = ini.IniReadValue("SerialNV", "Avail10");
                            int ten = int.Parse(Avail10Notes.Text);
                            ten++;
                            ini.IniWriteValue("SerialNV", "Avail10", ten.ToString());
                            textBox1.Text = "";
                            Display("Payment: " + Payment.ToString());
                        }
                        else if (TwentyEuroNoteIn)
                        {
                            FiveEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            TenEuroNoteIn = false;
                            Payment = Payment + 2000;
                            GeneralCounter = 120;
                            Paid20Notes.Text = ini.IniReadValue("SerialNV", "Paid20");
                            int twenty = int.Parse(Paid20Notes.Text);
                            twenty++;
                            ini.IniWriteValue("SerialNV", "Paid20", twenty.ToString());
                            textBox1.Text = "";
                            Display("Payment: " + Payment.ToString());
                        }
                    }
                    if (textBox1.Text.IndexOf("Rejecting") != -1)
                        textBox1.Text = "";
                    switch (PollNumber)
                    {
                        case 0:
                            Display("SmartPayout idle state");
                            //DisableChangeNotes = false;
                            break;
                        case 1:
                            Display("SmartPayout reset");
                            break;
                        case 2:
                            Display("SmartPayout disabled");
                            break;
                        case 3:
                            Display("SmartPayout note in escrow, reading note...");
                            break;
                        case 4:
                            Display("SmartPayout credit");
                            break;
                        case 5:
                            Display("SmartPayout rejecting note");
                            break;
                        case 6:
                            Display("SmartPayout note rejected");
                            break;
                        case 7:
                            Display("SmartPayout stacking note");
                            break;
                        case 8:
                            Display("SmartPayout floating note");
                            break;
                        case 9:
                            Display("SmartPayout note stacked");

                            break;
                        case 10:
                            Display("SmartPayout completed floating");
                            break;
                        case 11:
                            Display("SmartPayout note stored");

                            break;
                        case 12:
                            Display("SmartPayout safe jam");
                            break;
                        case 13:
                            Display("SmartPayout unsafe jam");
                            break;
                        case 14:
                            Display("SmartPayout detect error with payout device");
                            break;
                        case 15:
                            Display("SmartPayout fraud attempt!!!");
                            break;
                        case 16:
                            Display("SmartPayout stacker full");
                            break;
                        case 17:
                            Display("SmartPayout note cleared from front of validator");
                            break;
                        case 18:
                            Display("SmartPayout note cleared to cashbox");
                            break;
                        case 19:
                            Display("SmartPayout note paid into payout on startup");
                            break;
                        case 20:
                            Display("SmartPayout note paid into cashbox on startup");
                            break;
                        case 21:
                            Display("SmartPayout cashbox removed");
                            break;
                        case 22:
                            Display("SmartPayout cashbox replaced");
                            break;
                        case 23:
                            Display("SmartPayout despensing notes");
                            break;
                        case 24:
                            Display("SmartPayout dispensed notes");
                            break;
                        case 25:
                            Display("SmartPayout emptying...");
                            break;
                        case 26:
                            Display("SmartPayout emptied");
                            break;
                        case 27:
                            Display("SmartPayout SMART Emptying");
                            break;
                        case 28:
                            Display("SmartPayout SMART Emptied, getting info...");
                            break;
                        case 29:
                            Display("SmartPayout unit jammed");
                            break;
                        case 30:
                            Display("SmartPayout halted");
                            break;
                        case 31:
                            Display("SmartPayout incomplete payout");
                            break;
                        case 32:
                            Display("SmartPayout incomplete float");
                            break;
                        case 33:
                            Display("SmartPayout note transferred to stacker");
                            break;
                        case 34:
                            Display("SmartPayout note in bezel");
                            break;
                        case 35:
                            Display("SmartPayout payout out of service");
                            break;
                        case 36:
                            Display("SmartPayout timeout searching for a note");
                            break;
                        case 37:
                            Display("SmartPayout unsupported poll response received");
                            break;
                        default:
                            //Display("Credit is: " + PollNumber.ToString());
                            break;
                    }

                    if (Payment == InitalCost)
                    { //AKRIBWS
                        ReturnMoney = 0;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.PaymentText.Clear();
                        secondForm.ResidualText.Clear();
                        secondForm.Vprogress.Visible = false;
                        secondForm.ValueText.AppendText("0,00€");
                        secondForm.PaymentText.AppendText("0,00€");
                        secondForm.ResidualText.AppendText("0,00€");
                        secondForm.ValueLabel.Visible = false;
                        secondForm.ValueText.Visible = false;
                        secondForm.PaymentLabel.Visible = false;
                        secondForm.PaymentText.Visible = false;
                        secondForm.ResidualLabel.Visible = false;
                        secondForm.ResidualText.Visible = false;
                        Display("Payment OK");
                        Thread.Sleep(100);
                        if (pm.set(0, 1, 0, 0) != 0)
                        {
                            Display("Disable Coin ERROR 6\n");
                            SM = 11; //ShowNotes("000");
                        }
                        value = "0,00";
                        string url_1 = HTTPURLTICKET + UserCode + "?charge_until=" + charge_until;
                        HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                        request_1.Method = "PUT";
                        request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                        try
                        {
                            HttpWebResponse response_1 = null;
                            response_1 = (HttpWebResponse)request_1.GetResponse();
                            //httpResponse = response_1;
                            Stream responseStream = response_1.GetResponseStream();
                            string invoice_code;
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            var jsonobject = reader.ReadToEnd();
                            Display("JSON_object is: " + jsonobject);
                            var responseData = JObject.Parse(jsonobject);
                            Display("+++" + responseData.ToString());
                            invoice_code = (string)responseData["invoice"]["code"];
                            Display("invoice_code: " + invoice_code);
                            invoice_sequence = (string)responseData["invoice"]["sequence"];
                            Display("invoice_sequence: " + invoice_sequence);
                            invoice_number = (string)responseData["invoice"]["number"];
                            Display("invoice_number: " + invoice_number);
                            plate = (string)responseData["ticket"]["ticket_entrance"]["vehicle"]["plate"];
                            Display("vehicle_plate: " + plate);
                            charge = (string)responseData["charge"];
                            Display("charge: " + charge);
                            entered_at = (string)responseData["ticket"]["ticket_entrance"]["entered_at"];
                            Display("entered_at: " + entered_at);
                            response_1.Close();
                            reader.Close();
                            responseStream.Close();
                            SM = 7;
                            break;
                        }
                        catch (WebException ex)
                        {
                            HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                            switch (response_1.StatusCode)
                            {
                                case HttpStatusCode.NotFound:
                                    secondForm.Ticket_Icon.Visible = false;
                                    secondForm.POS_Messages.Clear();
                                    Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                    secondForm.POS_Messages.AppendText(Langtemp);
                                    secondForm.Refresh(); Thread.Sleep(2000);
                                    SM = 10;
                                    Display("Error 404 on receipt request");
                                    break;
                                case HttpStatusCode.InternalServerError:
                                    secondForm.Ticket_Icon.Visible = false;
                                    secondForm.POS_Messages.Clear();
                                    Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                    secondForm.POS_Messages.AppendText(Langtemp);
                                    secondForm.Refresh(); Thread.Sleep(2000);
                                    SM = 10;
                                    Display("Error 500 on receipt request");
                                    break;
                                case HttpStatusCode.ServiceUnavailable:
                                    secondForm.Ticket_Icon.Visible = false;
                                    secondForm.POS_Messages.Clear();
                                    Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                    secondForm.POS_Messages.AppendText(Langtemp);
                                    secondForm.Refresh(); Thread.Sleep(2000);
                                    SM = 10;
                                    Display("Error 503 on receipt request");
                                    break;
                            }

                        }
                        break;
                    }

                    if (InitalCost - Payment < 0)
                    { // RESTA 
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Card_Icon.Visible = false;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.Vprogress.Visible = false;
                        string str1 = Payment.ToString();
                        string text2 = (Payment >= 10 ? (Payment <= 10 || Payment > 99 ? str1.Insert(str1.Length - 2, ",") : "0," + str1) : "0,0" + str1) + "€";
                        secondForm.PaymentText.Clear();
                        secondForm.PaymentText.AppendText(text2);
                        string str2 = Math.Abs(InitalCost - Payment).ToString();
                        string text3 = (str2.Length <= 2 ? (str2.Length != 2 ? "0,0" + str2 : "0," + str2) : str2.Insert(str2.Length - 2, ",")) + "€";
                        secondForm.ResidualLabel.Location = new Point(500, 513);
                        if (secondForm.Language.StartsWith("GRE"))
                            secondForm.ResidualLabel.Text = "     Ρέστα: ";
                        else if (secondForm.Language.StartsWith("ENG"))
                            secondForm.ResidualLabel.Text = "  Change: ";
                        else if (secondForm.Language.StartsWith("FRA"))
                            secondForm.ResidualLabel.Text = "    Bien: ";
                        else if (secondForm.Language.StartsWith("GER"))
                            secondForm.ResidualLabel.Text = "     Gut: ";
                        secondForm.ResidualText.Clear();
                        secondForm.ResidualText.AppendText(text3);
                        Langtemp = ini.IniReadValue("LANGUAGE", "change" + secondForm.Language);
                        secondForm.POS_Messages.Clear();
                        secondForm.Refresh();

                        if (Math.Abs(InitalCost - Payment + ReturnMoney) > 500 && !DisableChangeNotes)
                        {   //change should be in notes
                            secondForm.Ticket_Icon.Visible = false;
                            secondForm.Card_Icon.Visible = false;
                            GeneralTimer.Stop();
                            int x = Math.Abs(InitalCost - Payment) / 1000;

                            if (DNote10 < x)
                            {
                                tbPayoutAmount.Text = "10";
                                btnPayout_Click(this, e);
                                SM = 64;
                                break;
                            }
                            int y = Math.Abs(InitalCost - Payment) / 500;
                            if (DNote5 < y)
                            {
                                secondForm.Ticket_Icon.Visible = false;
                                secondForm.Card_Icon.Visible = false;
                                GeneralTimer.Stop();
                                tbPayoutAmount.Text = "5";
                                btnPayout_Click(this, e);
                                SM = 64;
                                break;
                            }
                            if (Math.Abs(InitalCost - Payment) > (Total_Coins - 1000))
                            {
                                Display("OUT OF AMMO\n");
                                ShowNotes("000");
                                Display("ERROR_INCOMPLETE Pay: " + (InitalCost - Payment).ToString() + "\n");
                                value = (InitalCost - Payment).ToString();

                                string url = HTTPURLTICKET + UserCode + "?charge_until=" + charge_until;
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                request.Method = "PUT";
                                request.Headers.Add("Authorization", "Bearer " + ApiToken);
                                HttpWebResponse response = null;
                                try
                                {
                                    response = (HttpWebResponse)request.GetResponse();
                                    Stream responseStream = response.GetResponseStream();
                                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                    response.Close();
                                    reader.Close();
                                    responseStream.Close();
                                    secondForm.POS_Messages.Clear();
                                    Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                                    secondForm.POS_Messages.AppendText(Langtemp);
                                    secondForm.Refresh();
                                    GeneralTimer.Start();
                                    SM = 80;
                                    break;
                                }
                                catch (WebException ex)
                                {
                                    HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                    switch (response_1.StatusCode)
                                    {
                                        case HttpStatusCode.NotFound:
                                            secondForm.Ticket_Icon.Visible = false;
                                            secondForm.POS_Messages.Clear();
                                            Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                            secondForm.POS_Messages.AppendText(Langtemp);
                                            secondForm.Refresh(); Thread.Sleep(2000);
                                            SM = 7;
                                            Display("Error 404 on receipt request");
                                            break;
                                        case HttpStatusCode.InternalServerError:
                                            secondForm.Ticket_Icon.Visible = false;
                                            secondForm.POS_Messages.Clear();
                                            Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                            secondForm.POS_Messages.AppendText(Langtemp);
                                            secondForm.Refresh(); Thread.Sleep(2000);
                                            SM = 7;
                                            Display("Error 500 on receipt request");
                                            break;
                                        case HttpStatusCode.ServiceUnavailable:
                                            secondForm.Ticket_Icon.Visible = false;
                                            secondForm.POS_Messages.Clear();
                                            Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                            secondForm.POS_Messages.AppendText(Langtemp);
                                            secondForm.Refresh(); Thread.Sleep(2000);
                                            SM = 10;
                                            Display("Error 503 on receipt request");
                                            break;
                                    }
                                }
                            }
                            GeneralTimer.Start();
                        }

                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();

                        Display("Inital " + InitalCost.ToString() + "\n");
                        Display("Payment " + Payment.ToString() + "\n");
                        Display("Return " + ReturnMoney.ToString() + "\n");

                        int res_ = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                        ReturnMoney = ReturnMoney + res_;

                        if ((InitalCost - Payment + ReturnMoney) < 0)
                        {
                            res_ = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                            ReturnMoney = ReturnMoney + res_;
                        }

                        if ((InitalCost - Payment + ReturnMoney) >= -5)
                        {//DINW RESTA KERMATA
                            secondForm.Ticket_Icon.Visible = false;
                            Display("\nCoin Change OK: " + res_ + "\n");
                            string url_1 = HTTPURLTICKET + UserCode + "?charge_until=" + charge_until;
                            HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                            request_1.Method = "PUT";
                            request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                            try
                            {
                                HttpWebResponse response_1 = null;
                                response_1 = (HttpWebResponse)request_1.GetResponse();
                                Stream responseStream = response_1.GetResponseStream();
                                string invoice_code;
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                var jsonobject = reader.ReadToEnd();
                                Display("JSON_object is: " + jsonobject);
                                var responseData = JObject.Parse(jsonobject);
                                Display("+++" + responseData.ToString());
                                invoice_code = (string)responseData["invoice"]["code"];
                                Display("invoice_code: " + invoice_code);
                                invoice_sequence = (string)responseData["invoice"]["sequence"];
                                Display("invoice_sequence: " + invoice_sequence);
                                invoice_number = (string)responseData["invoice"]["number"];
                                Display("invoice_number: " + invoice_number);
                                plate = (string)responseData["ticket"]["ticket_entrance"]["vehicle"]["plate"];
                                Display("vehicle_plate: " + plate);
                                charge = (string)responseData["charge"];
                                Display("charge: " + charge);
                                entered_at = (string)responseData["ticket"]["ticket_entrance"]["entered_at"];
                                Display("entered_at: " + entered_at);
                                response_1.Close();
                                reader.Close();
                                responseStream.Close();
                                SM = 7;
                                break;
                            }
                            catch (WebException ex)
                            {
                                HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                switch (response_1.StatusCode)
                                {
                                    case HttpStatusCode.NotFound:
                                        secondForm.Ticket_Icon.Visible = false;
                                        secondForm.POS_Messages.Clear();
                                        Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                        secondForm.POS_Messages.AppendText(Langtemp);
                                        secondForm.Refresh(); Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 404 on receipt request");
                                        break;
                                    case HttpStatusCode.InternalServerError:
                                        secondForm.Ticket_Icon.Visible = false;
                                        secondForm.POS_Messages.Clear();
                                        Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                        secondForm.POS_Messages.AppendText(Langtemp);
                                        secondForm.Refresh(); Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 500 on receipt request");
                                        break;
                                    case HttpStatusCode.ServiceUnavailable:
                                        secondForm.Ticket_Icon.Visible = false;
                                        secondForm.POS_Messages.Clear();
                                        Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                        secondForm.POS_Messages.AppendText(Langtemp);
                                        secondForm.Refresh(); Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 503 on receipt request");
                                        break;
                                }
                            }
                        }
                        else
                        {//DEN MPORW NA DWSW RESTA
                            secondForm.Ticket_Icon.Visible = false;
                            Display("ERROR_INCOMPLETE Pay: " + ReturnMoney.ToString() + "\n");
                            value = (InitalCost - Payment + ReturnMoney).ToString();
                            Display("ERROR_INCOMPLETE value: " + value + "\n");
                            string url_1 = HTTPURLTICKET + UserCode + "?charge_until=" + charge_until;
                            HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                            request_1.Method = "PUT";
                            request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                            try
                            {
                                HttpWebResponse response_1 = null;
                                response_1 = (HttpWebResponse)request_1.GetResponse();
                                Stream responseStream = response_1.GetResponseStream();
                                string invoice_code;
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                var jsonobject = reader.ReadToEnd();
                                Display("JSON_object is: " + jsonobject);
                                var responseData = JObject.Parse(jsonobject);
                                Display("+++" + responseData.ToString());
                                invoice_code = (string)responseData["invoice"]["code"];
                                Display("invoice_code: " + invoice_code);
                                invoice_sequence = (string)responseData["invoice"]["sequence"];
                                Display("invoice_sequence: " + invoice_sequence);
                                invoice_number = (string)responseData["invoice"]["number"];
                                Display("invoice_number: " + invoice_number);
                                plate = (string)responseData["ticket"]["ticket_entrance"]["vehicle"]["plate"];
                                Display("vehicle_plate: " + plate);
                                charge = (string)responseData["charge"];
                                Display("charge: " + charge);
                                entered_at = (string)responseData["ticket"]["ticket_entrance"]["entered_at"];
                                Display("entered_at: " + entered_at);
                                response_1.Close();
                                reader.Close();
                                responseStream.Close();
                                secondForm.POS_Messages.Clear();
                                Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                                secondForm.POS_Messages.AppendText(Langtemp);
                                secondForm.Refresh();
                                if (pm.set(0, 1, 0, 0) != 0)
                                {
                                    Display("\nDisable Coins ERROR ");
                                    SM = 11;
                                    ShowNotes("000");
                                    break;
                                }                               
                                SM = 80;
                                break;
                            }
                            catch (WebException ex)
                            {
                                HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                switch (response_1.StatusCode)
                                {
                                    case HttpStatusCode.NotFound:
                                        secondForm.Ticket_Icon.Visible = false;
                                        secondForm.POS_Messages.Clear();
                                        Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                        secondForm.POS_Messages.AppendText(Langtemp);
                                        secondForm.Refresh(); Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 404 on receipt request");
                                        break;
                                    case HttpStatusCode.InternalServerError:
                                        secondForm.Ticket_Icon.Visible = false;
                                        secondForm.POS_Messages.Clear();
                                        Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                        secondForm.POS_Messages.AppendText(Langtemp);
                                        secondForm.Refresh(); Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 500 on receipt request");
                                        break;
                                    case HttpStatusCode.ServiceUnavailable:
                                        secondForm.Ticket_Icon.Visible = false;
                                        secondForm.POS_Messages.Clear();
                                        Langtemp = ini.IniReadValue("LANGUAGE", "Noreceipt" + secondForm.Language);
                                        secondForm.POS_Messages.AppendText(Langtemp);
                                        secondForm.Refresh(); Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 503 on receipt request");
                                        break;
                                }
                            }
                        }
                        if (pm.set(0, 1, 0, 0) != 0)
                        {
                            Display("\nDisable Coins ERROR "); SM = 11; ShowNotes("000");
                            break;
                        }
                        break;
                    }
                    if (snapshot != null)
                    {
                        try
                        {
                            secondForm.SnapshotPictureBox.Location = new Point(12, 257);
                            secondForm.SnapshotPictureBox.Load("http://" + ini.IniReadValue("Params", "ServerIP") + "/storage/parking/" + snapshot);
                            secondForm.SnapshotPictureBox.Visible = true;
                            secondForm.Refresh();
                        }
                        catch (ArgumentNullException)
                        {
                            Display("Snapsot contains no image - Argument null Exception");
                        }
                        catch
                        {
                            Display("Snapsot contains no image");
                        }
                    }
                    else
                    {
                        secondForm.SnapshotPictureBox.Location = new Point(12, 257);
                        secondForm.SnapshotPictureBox.Visible = true;
                        secondForm.Refresh();
                    }
                    secondForm.ValueLabel.Visible = true;
                    value = InitalCost.ToString();
                    value = InitalCost >= 10 ? (InitalCost <= 10 || InitalCost > 99 ? value.Insert(value.Length - 2, ",") : "0," + value) : "0,0" + value;
                    value += "€";
                    secondForm.ValueText.Clear();
                    secondForm.ValueText.AppendText(value);
                    secondForm.PaymentLabel.Visible = true;
                    string str3 = Payment.ToString();
                    string text5 = (Payment >= 10 ? (Payment <= 10 || Payment > 99 ? str3.Insert(str3.Length - 2, ",") : "0," + str3) : "0,0" + str3) + "€";
                    secondForm.PaymentText.Clear();
                    secondForm.PaymentText.AppendText(text5);
                    secondForm.ResidualLabel.Visible = true;
                    string str4 = (InitalCost - Payment).ToString();
                    string text6 = (InitalCost - Payment + ReturnMoney >= 10 ? (InitalCost - Payment + ReturnMoney <= 10 || InitalCost - Payment + ReturnMoney > 99 ? str4.Insert(str4.Length - 2, ",") : "0," + str4) : "0,0" + str4) + "€";
                    secondForm.ResidualText.Clear();
                    secondForm.ResidualText.AppendText(text6);
                    break;

                case 61:    //Accept Notes and Coins - Card
                    Display("SM: " + SM);
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Card_Icon.Visible = false;

                    int PollNumberForCard = Payout.DoPoll(textBox1).Item2;
                    Display("SmartPayout Poll response: " + PollNumberForCard.ToString());
                    switch (PollNumberForCard)
                    {
                        case 0:
                            Display("SmartPayout idle state");
                            break;
                        case 1:
                            Display("SmartPayout reset");
                            break;
                        case 2:
                            Display("SmartPayout disabled");
                            break;
                        case 3:
                            Display("SmartPayout note in escrow, reading note...");
                            break;
                        case 4:
                            Display("SmartPayout credit");
                            break;
                        case 5:
                            Display("SmartPayout rejecting note");
                            break;
                        case 6:
                            Display("SmartPayout note rejected");
                            break;
                        case 7:
                            Display("SmartPayout stacking note");
                            break;
                        case 8:
                            Display("SmartPayout floating note");
                            break;
                        case 9:
                            Display("SmartPayout note stacked");
                            break;
                        case 10:
                            Display("SmartPayout completed floating");
                            break;
                        case 11:
                            Display("SmartPayout note stored");
                            break;
                        case 12:
                            Display("SmartPayout safe jam");
                            break;
                        case 13:
                            Display("SmartPayout unsafe jam");
                            break;
                        case 14:
                            Display("SmartPayout detect error with payout device");
                            break;
                        case 15:
                            Display("SmartPayout fraud attempt!!!");
                            break;
                        case 16:
                            Display("SmartPayout stacker full");
                            break;
                        case 17:
                            Display("SmartPayout note cleared from front of validator");
                            break;
                        case 18:
                            Display("SmartPayout note cleared to cashbox");
                            break;
                        case 19:
                            Display("SmartPayout note paid into payout on startup");
                            break;
                        case 20:
                            Display("SmartPayout note paid into cashbox on startup");
                            break;
                        case 21:
                            Display("SmartPayout cashbox removed");
                            break;
                        case 22:
                            Display("SmartPayout cashbox replaced");
                            break;
                        case 23:
                            Display("SmartPayout despensing notes");
                            break;
                        case 24:
                            Display("SmartPayout dispensed notes");
                            break;
                        case 25:
                            Display("SmartPayout emptying...");
                            break;
                        case 26:
                            Display("SmartPayout emptied");
                            break;
                        case 27:
                            Display("SmartPayout SMART Emptying");
                            break;
                        case 28:
                            Display("SmartPayout SMART Emptied, getting info...");
                            break;
                        case 29:
                            Display("SmartPayout unit jammed");
                            break;
                        case 30:
                            Display("SmartPayout halted");
                            break;
                        case 31:
                            Display("SmartPayout incomplete payout");
                            break;
                        case 32:
                            Display("SmartPayout incomplete float");
                            break;
                        case 33:
                            Display("SmartPayout notye transferred to stacker");
                            break;
                        case 34:
                            Display("SmartPayout note in bezel");
                            break;
                        case 35:
                            Display("SmartPayout payout out of service");
                            break;
                        case 36:
                            Display("SmartPayout timeout searching for a note");
                            break;
                        case 37:
                            Display("SmartPayout unsupported poll response received");
                            break;
                    }

                    if (Payment == InitalCost)
                    { //AKRIBWS
                        ReturnMoney = 0;
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.PaymentText.Clear();
                        secondForm.ResidualText.Clear();
                        secondForm.Vprogress.Visible = false;
                        secondForm.ValueText.AppendText("0,00€");
                        secondForm.PaymentText.AppendText("0,00€");
                        secondForm.ResidualText.AppendText("0,00€");
                        secondForm.ValueLabel.Visible = false;
                        secondForm.PaymentLabel.Visible = false;
                        secondForm.ResidualLabel.Visible = false;
                        secondForm.SnapshotPictureBox.Visible = false;
                        Display("Payment OK");
                        Thread.Sleep(100);
                        if (pm.set(0, 1, 0, 0) != 0)
                        {
                            Display("Disable Coin ERROR 6\n"); SM = 11; ShowNotes("000");
                            break;
                        }
                        value = "0,00";
                        string url_1 = HTTPURLRENEW + subscriptionId;
                        Display("requesting url: " + url_1);
                        HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                        request_1.Method = "POST";
                        request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                        try
                        {
                            HttpWebResponse response_1 = null;
                            response_1 = (HttpWebResponse)request_1.GetResponse();
                            Stream responseStream = response_1.GetResponseStream();
                            string invoice_code;
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            var jsonobject = reader.ReadToEnd();
                            Display("JSON_object is: " + jsonobject);
                            var responseData = JObject.Parse(jsonobject);
                            Display("+++" + responseData.ToString());
                            invoice_code = (string)responseData["invoice"]["code"];
                            Display("invoice_code: " + invoice_code);
                            invoice_sequence = (string)responseData["invoice"]["sequence"];
                            Display("invoice_sequence: " + invoice_sequence);
                            invoice_number = (string)responseData["invoice"]["number"];
                            Display("invoice_number: " + invoice_number);
                            duration = (string)responseData["renewal"]["do_renewal"];
                            Display("duration of renewal: " + duration);
                            charge = (string)responseData["invoice"]["final_price"];
                            Display("charge: " + charge);
                            response_1.Close();
                            reader.Close();
                            responseStream.Close();
                        }
                        catch (WebException ex)
                        {
                            HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                            switch (response_1.StatusCode)
                            {
                                case HttpStatusCode.NotFound:
                                    Thread.Sleep(2000);
                                    SM = 10;
                                    Display("Error 404 on receipt request");
                                    break;
                                case HttpStatusCode.InternalServerError:
                                    Thread.Sleep(2000);
                                    SM = 10;
                                    Display("Error 500 on receipt request");
                                    break;
                                case HttpStatusCode.ServiceUnavailable:
                                    Thread.Sleep(2000);
                                    SM = 10;
                                    Display("Error 503 on receipt request");
                                    break;
                            }
                        }
                        SM = 72;
                        break;
                    }

                    if ((InitalCost - Payment + ReturnMoney) < 0)
                    { // RESTA 
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Card_Icon.Visible = false;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.Vprogress.Visible = false;
                        string str1 = Payment.ToString();
                        string text2 = (Payment >= 10 ? (Payment <= 10 || Payment > 99 ? str1.Insert(str1.Length - 2, ",") : "0," + str1) : "0,0" + str1) + "€";
                        secondForm.PaymentText.Clear();
                        secondForm.PaymentText.AppendText(text2);
                        string str2 = Math.Abs(InitalCost - Payment).ToString();
                        string text3 = (str2.Length <= 2 ? (str2.Length != 2 ? "0,0" + str2 : "0," + str2) : str2.Insert(str2.Length - 2, ",")) + "€";
                        secondForm.ResidualLabel.Location = new Point(500, 513);
                        if (secondForm.Language.StartsWith("GRE"))
                            secondForm.ResidualLabel.Text = "     Ρέστα: ";
                        else if (secondForm.Language.StartsWith("ENG"))
                            secondForm.ResidualLabel.Text = "  Change: ";
                        else if (secondForm.Language.StartsWith("FRA"))
                            secondForm.ResidualLabel.Text = "    Bien: ";
                        else if (secondForm.Language.StartsWith("GER"))
                            secondForm.ResidualLabel.Text = "     Gut: ";
                        secondForm.ResidualText.Clear();
                        secondForm.ResidualText.AppendText(text3);
                        Langtemp = ini.IniReadValue("LANGUAGE", "change" + secondForm.Language);
                        secondForm.POS_Messages.Clear();
                        secondForm.Refresh();

                        if (Math.Abs(InitalCost - Payment) > 500)
                        {   //change should be in notes
                            secondForm.Ticket_Icon.Visible = false;
                            secondForm.Card_Icon.Visible = false;
                            GeneralTimer.Stop();
                            int x = Math.Abs(InitalCost - Payment) / 1000;
                            if (DNote10 < x)
                            {
                                tbPayoutAmount.Text = "10";
                                btnPayout_Click(this, e);
                                SM = 64;
                                break;
                            }
                            int y = Math.Abs(InitalCost - Payment) / 500;
                            if (DNote5 < y)
                            {
                                secondForm.Ticket_Icon.Visible = false;
                                secondForm.Card_Icon.Visible = false;
                                GeneralTimer.Stop();
                                tbPayoutAmount.Text = "5";
                                btnPayout_Click(this, e);
                                SM = 64;
                                break;
                            }
                            if (Math.Abs(InitalCost - Payment) > (Total_Coins - 1000))
                            {
                                Display("OUT OF AMMO\n");
                                ShowNotes("000");
                                Display("ERROR_INCOMPLETE Pay: " + (InitalCost - Payment).ToString() + "\n");
                                value = (InitalCost - Payment).ToString();

                                string url_1 = HTTPURLRENEW + subscriptionId;
                                Display("requesting url: " + url_1);
                                HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                                request_1.Method = "POST";
                                request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                                try
                                {
                                    HttpWebResponse response_1 = null;
                                    response_1 = (HttpWebResponse)request_1.GetResponse();
                                    Stream responseStream = response_1.GetResponseStream();
                                    string invoice_code;
                                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                    var jsonobject = reader.ReadToEnd();
                                    Display("JSON_object is: " + jsonobject);
                                    var responseData = JObject.Parse(jsonobject);
                                    Display("+++" + responseData.ToString());
                                    invoice_code = (string)responseData["invoice"]["code"];
                                    Display("invoice_code: " + invoice_code);
                                    invoice_sequence = (string)responseData["invoice"]["sequence"];
                                    Display("invoice_sequence: " + invoice_sequence);
                                    invoice_number = (string)responseData["invoice"]["number"];
                                    Display("invoice_number: " + invoice_number);
                                    duration = (string)responseData["renewal"]["do_renewal"];
                                    Display("duration of renewal: " + duration);
                                    charge = (string)responseData["invoice"]["final_price"];
                                    Display("charge: " + charge);
                                    response_1.Close();
                                    reader.Close();
                                    responseStream.Close();
                                }
                                catch (WebException ex)
                                {
                                    HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                    switch (response_1.StatusCode)
                                    {
                                        case HttpStatusCode.NotFound:
                                            Thread.Sleep(2000);
                                            SM = 72;
                                            Display("Error 404 on receipt request");
                                            break;
                                        case HttpStatusCode.InternalServerError:
                                            Thread.Sleep(2000);
                                            SM = 72;
                                            Display("Error 500 on receipt request");
                                            break;
                                        case HttpStatusCode.ServiceUnavailable:
                                            Thread.Sleep(2000);
                                            SM = 72;
                                            Display("Error 503 on receipt request");
                                            break;
                                    }
                                }
                                SM = 81;
                            }
                            GeneralTimer.Start();
                        }

                        secondForm.POS_Messages.AppendText(Langtemp);
                        secondForm.Refresh();

                        Display("Inital " + InitalCost.ToString() + "\n");
                        Display("Payment " + Payment.ToString() + "\n");
                        Display("Return " + ReturnMoney.ToString() + "\n");

                        int res = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                        ReturnMoney = ReturnMoney + res;

                        if ((InitalCost - Payment + ReturnMoney) < 0)
                        {
                            res = pm.set(1, 0, Math.Abs(InitalCost - Payment + ReturnMoney), 0);
                            ReturnMoney = ReturnMoney + res;
                        }

                        if ((InitalCost - Payment + ReturnMoney) >= -5)
                        {//DINW RESTA KERMATA
                            secondForm.Ticket_Icon.Visible = false;
                            Display("\nCoin Change OK: " + res + "\n");
                            string url_1 = HTTPURLRENEW + subscriptionId;
                            HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                            request_1.Method = "POST";
                            request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                            try
                            {
                                HttpWebResponse response_1 = null;
                                response_1 = (HttpWebResponse)request_1.GetResponse();
                                Stream responseStream = response_1.GetResponseStream();
                                string invoice_code;
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                var jsonobject = reader.ReadToEnd();
                                Display("JSON_object is: " + jsonobject);
                                var responseData = JObject.Parse(jsonobject);
                                Display("+++" + responseData.ToString());
                                invoice_code = (string)responseData["invoice"]["code"];
                                Display("invoice_code: " + invoice_code);
                                invoice_sequence = (string)responseData["invoice"]["sequence"];
                                Display("invoice_sequence: " + invoice_sequence);
                                invoice_number = (string)responseData["invoice"]["number"];
                                Display("invoice_number: " + invoice_number);
                                duration = (string)responseData["renewal"]["do_renewal"];
                                Display("duration of renewal: " + duration);
                                charge = (string)responseData["invoice"]["final_price"];
                                Display("charge: " + charge);
                                response_1.Close();
                                reader.Close();
                                responseStream.Close();
                                SM = 72;
                                break;
                            }
                            catch (WebException ex)
                            {
                                HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                switch (response_1.StatusCode)
                                {
                                    case HttpStatusCode.NotFound:
                                        Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 404 on receipt request");
                                        break;
                                    case HttpStatusCode.InternalServerError:
                                        Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 500 on receipt request");
                                        break;
                                    case HttpStatusCode.ServiceUnavailable:
                                        Thread.Sleep(2000);
                                        SM = 10;
                                        Display("Error 503 on receipt request");
                                        break;
                                }
                            }
                        }
                        else
                        {//DEN MPORW NA DWSW RESTA
                            secondForm.Ticket_Icon.Visible = false;
                            Display("ERROR_INCOMPLETE Pay: " + ReturnMoney.ToString() + "\n");
                            value = (InitalCost - Payment + ReturnMoney).ToString();
                            Display("ERROR_INCOMPLETE value: " + value + "\n");
                            string url_1 = HTTPURLRENEW + subscriptionId;
                            HttpWebRequest request_1 = (HttpWebRequest)WebRequest.Create(url_1);
                            request_1.Method = "POST";
                            request_1.Headers.Add("Authorization", "Bearer " + ApiToken);
                            try
                            {
                                HttpWebResponse response_1 = null;
                                response_1 = (HttpWebResponse)request_1.GetResponse();
                                Stream responseStream = response_1.GetResponseStream();
                                string invoice_code;
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                var jsonobject = reader.ReadToEnd();
                                Display("JSON_object is: " + jsonobject);
                                var responseData = JObject.Parse(jsonobject);
                                Display("+++" + responseData.ToString());
                                invoice_code = (string)responseData["invoice"]["code"];
                                Display("invoice_code: " + invoice_code);
                                invoice_sequence = (string)responseData["invoice"]["sequence"];
                                Display("invoice_sequence: " + invoice_sequence);
                                invoice_number = (string)responseData["invoice"]["number"];
                                Display("invoice_number: " + invoice_number);
                                duration = (string)responseData["renewal"]["do_renewal"];
                                Display("duration of renewal: " + duration);
                                charge = (string)responseData["invoice"]["final_price"];
                                Display("charge: " + charge);
                                response_1.Close();
                                reader.Close();
                                responseStream.Close();
                                secondForm.POS_Messages.Clear();
                                Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                                secondForm.POS_Messages.AppendText(Langtemp);
                                secondForm.Refresh();
                                if (pm.set(0, 1, 0, 0) != 0)
                                {
                                    Display("\nDisable Coins ERROR ");
                                    SM = 11;
                                    ShowNotes("000");
                                    break;
                                }
                                SM = 81;
                                break;
                            }
                            catch (WebException ex)
                            {
                                HttpWebResponse response_1 = (HttpWebResponse)ex.Response;
                                switch (response_1.StatusCode)
                                {
                                    case HttpStatusCode.NotFound:
                                        Thread.Sleep(2000);
                                        SM = 81;
                                        Display("Error 404 on receipt request");
                                        break;
                                    case HttpStatusCode.InternalServerError:
                                        Thread.Sleep(2000);
                                        SM = 81;
                                        Display("Error 500 on receipt request");
                                        break;
                                    case HttpStatusCode.ServiceUnavailable:
                                        Thread.Sleep(2000);
                                        SM = 81;
                                        Display("Error 503 on receipt request");
                                        break;
                                }
                            }
                        }
                        if (pm.set(0, 1, 0, 0) != 0)
                        {
                            Display("\nDisable Coins ERROR "); SM = 11; ShowNotes("000");
                            break;
                        }
                    }
                    if (snapshot != null)
                    {
                        secondForm.SnapshotPictureBox.Location = new Point(12, 257);
                        secondForm.SnapshotPictureBox.Load("http://" + ini.IniReadValue("Params", "ServerIP") + "/storage/parking/" + snapshot);
                        secondForm.SnapshotPictureBox.Visible = true;
                        secondForm.Refresh();
                    }
                    else
                    {
                        secondForm.SnapshotPictureBox.Location = new Point(12, 257);
                        secondForm.SnapshotPictureBox.Visible = true;
                        secondForm.Refresh();
                    }
                    secondForm.ValueLabel.Visible = true;
                    value = InitalCost.ToString();
                    value = InitalCost >= 10 ? (InitalCost <= 10 || InitalCost > 99 ? value.Insert(value.Length - 2, ",") : "0," + value) : "0,0" + value;
                    value += "€";
                    secondForm.ValueText.Clear();
                    secondForm.ValueText.AppendText(value);
                    secondForm.PaymentLabel.Visible = true;
                    string str33 = Payment.ToString();
                    string text55 = (Payment >= 10 ? (Payment <= 10 || Payment > 99 ? str33.Insert(str33.Length - 2, ",") : "0," + str33) : "0,0" + str33) + "€";
                    secondForm.PaymentText.Clear();
                    secondForm.PaymentText.AppendText(text55);
                    secondForm.ResidualLabel.Visible = true;
                    string str44 = (InitalCost - Payment).ToString();
                    string text66 = (InitalCost - Payment + ReturnMoney >= 10 ? (InitalCost - Payment + ReturnMoney <= 10 || InitalCost - Payment + ReturnMoney > 99 ? str44.Insert(str44.Length - 2, ",") : "0," + str44) : "0,0" + str44) + "€";
                    secondForm.ResidualText.Clear();
                    secondForm.ResidualText.AppendText(text66);
                    //textBox1.Text = "";
                break;

                case 611:    
                    Display("SM: " + SM);
                    /*if(notes05In)
                    {
                        Display("NoteValidator 05 Euro Note");
                        Payment = Payment + 500;
                        GeneralCounter = 120;
                        UpdateUI();
                        if (notesInStorageText.Text.IndexOf("30") != -1)
                        {
                            string five = ini.IniReadValue("SerialNV", "Avail05");
                            int temp = Convert.ToInt16(five); temp++;
                            ini.IniWriteValue("SerialNV", "Avail05", temp.ToString());
                            Display("5 euro note stock is full. Extra went into paid...");
                        }
                        //UpdateUI();
                        notes05In = false;
                        notesIn = false;
                        NV11.storedNoteValue = 0;
                        if (secondForm.CashPayment && ticket_scanned)
                        {
                            SM = 6;
                            break;
                        }
                        else if(secondForm.CashPayment && card_scanned)
                        {
                            SM = 61;
                            break;
                        }
                        //break;
                    }

                    if (notes10In)
                    {
                        Display("NoteValidator 10 Euro Note");
                        Payment = Payment + 1000;
                        GeneralCounter = 120;
                        string ten = ini.IniReadValue("SerialNV", "Paid10");
                        int temp = Convert.ToInt16(ten); temp++;
                        ini.IniWriteValue("SerialNV", "Paid10", temp.ToString());
                        notes10In = false;
                        notesIn = false;
                        NV11.storedNoteValue = 0;
                        if (secondForm.CashPayment && ticket_scanned)
                        {
                            SM = 6;
                            break;
                        }
                        else if (secondForm.CashPayment && card_scanned)
                        {
                            SM = 61;
                            break;
                        }
                    }

                    if(notes20In)
                    {
                        Display("NoteValidator 20 Euro Note");
                        Payment = Payment + 2000;
                        GeneralCounter = 120;
                        string twenty = ini.IniReadValue("SerialNV", "Paid20");
                        int temp = Convert.ToInt16(twenty); temp++;
                        ini.IniWriteValue("SerialNV", "Paid20", temp.ToString());
                        notes20In = false;
                        notesIn = false;
                        NV11.storedNoteValue = 0;
                        if (secondForm.CashPayment && ticket_scanned)
                        {
                            SM = 6;
                            break;
                        }
                        else if (secondForm.CashPayment && card_scanned)
                        {
                            SM = 61;
                            break;
                        }
                    }

                    if (notes50In)
                    {
                        Display("NoteValidator 50 Euro Note");
                        Payment = Payment + 5000;
                        GeneralCounter = 120;
                        string fifty = ini.IniReadValue("SerialNV", "Paid50");
                        int temp = Convert.ToInt16(fifty); temp++;
                        ini.IniWriteValue("SerialNV", "Paid50", temp.ToString());
                        notes50In = false;
                        notesIn = false;
                        NV11.storedNoteValue = 0;
                        if (secondForm.CashPayment && ticket_scanned)
                        {
                            SM = 6;
                            break;
                        }
                        else if (secondForm.CashPayment && card_scanned)
                        {
                            SM = 61;
                            break;
                        }
                    }*/
                    break;

                case 62:    //Accept Credit - Debit Card for Ticket
                    Display("SM: " + SM);
                    if (Payment == InitalCost)
                    {
                        ReturnMoney = 0;
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Card_Icon.Visible = false;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.Vprogress.Visible = false;
                        secondForm.SnapshotPictureBox.Visible = false;
                        secondForm.ValueText.AppendText("0,00€");
                        SM = 71;
                        Display("Payment OK");
                        Thread.Sleep(100);
                        break;
                    }
                    secondForm.Refresh();
                    if (snapshot != null)
                    {
                        secondForm.SnapshotPictureBox.Location = new Point(12, 257);
                        secondForm.SnapshotPictureBox.Load("http://" + ini.IniReadValue("Params", "ServerIP") + "/storage/parking/" + snapshot);
                        secondForm.SnapshotPictureBox.Visible = true;
                        secondForm.Refresh();
                    }
                    else
                    {
                        secondForm.SnapshotPictureBox.Location = new Point(12, 257);
                        secondForm.SnapshotPictureBox.Visible = true;
                        secondForm.Refresh();
                    }
                    Display("Inital " + InitalCost.ToString() + "\n");
                    Display("Payment " + Payment.ToString() + "\n");
                    Display("Return " + ReturnMoney.ToString() + "\n");
                    value = (InitalCost - Payment + ReturnMoney).ToString();
                    if (value.Length > 2)
                        value = value.Insert(value.Length - 2, ",");
                    else if (value.Length == 2)
                        value = "0," + value;
                    else
                        value = "0,0" + value;
                    value += "€";
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Card_Icon.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.ValueText.AppendText(value);
                    break;

                case 622:
                    Display("SM: " + SM);
                    /*if (pay05)
                    {
                        if (secondForm.CashPayment)
                        {
                            SM = 6;
                            pay05 = false;
                            break;
                        }
                        else if(secondForm.CreditCardPayment)
                        {
                            SM = 61;
                            pay05 = false;
                            break;
                        }
                    }*/
                    break;

                case 63:    //Accept Credit - Debit card for Card
                    Display("SM: " + SM);
                    if (Payment == InitalCost)
                    {
                        ReturnMoney = 0;
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.Card_Icon.Visible = false;
                        secondForm.Cancel.Visible = false;
                        secondForm.ValueText.Clear();
                        secondForm.Vprogress.Visible = false;
                        secondForm.ValueText.AppendText("0,00€");
                        SM = 73;
                        Display("Payment OK");
                        Thread.Sleep(100);
                        break;
                    }
                    value = (InitalCost - Payment + ReturnMoney).ToString();
                    if (value.Length > 2)
                        value = value.Insert(value.Length - 2, ",");
                    else if (value.Length == 2)
                        value = "0," + value;
                    else
                        value = "0,0" + value;
                    value += "€";
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Card_Icon.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.ValueText.AppendText(value);
                    break;

                case 64:    //WAIT TO RETRIVE NOTES - CHANGE
                    Display("SM: " + SM);
                    Display("Wait to retrive note(s)...");
                    int PollNumber_ = Payout.DoPoll(textBox1).Item2;
                    Display("SmartPayout Poll response: " + PollNumber_.ToString());
                    Display(textBox1.Text);
                    switch (PollNumber_)
                    {
                        case 0:
                            Display("SmartPayout idle state");
                            break;
                        case 1:
                            Display("SmartPayout reset");
                            break;
                        case 2:
                            Display("SmartPayout disabled");
                            break;
                        case 3:
                            Display("SmartPayout note in escrow, reading note...");
                            break;
                        case 4:
                            Display("SmartPayout credit");
                            break;
                        case 5:
                            Display("SmartPayout rejecting note");
                            break;
                        case 6:
                            Display("SmartPayout note rejected");
                            break;
                        case 7:
                            Display("SmartPayout stacking note");
                            break;
                        case 8:
                            Display("SmartPayout floating note");
                            break;
                        case 9:
                            Display("SmartPayout note stacked");
                            break;
                        case 10:
                            Display("SmartPayout completed floating");
                            break;
                        case 11:
                            Display("SmartPayout note stored");
                            break;
                        case 12:
                            Display("SmartPayout safe jam");
                            break;
                        case 13:
                            Display("SmartPayout unsafe jam");
                            break;
                        case 14:
                            Display("SmartPayout detect error with payout device");
                            break;
                        case 15:
                            Display("SmartPayout fraud attempt!!!");
                            break;
                        case 16:
                            Display("SmartPayout stacker full");
                            break;
                        case 17:
                            Display("SmartPayout note cleared from front of validator");
                            break;
                        case 18:
                            Display("SmartPayout note cleared to cashbox");
                            break;
                        case 19:
                            Display("SmartPayout note paid into payout on startup");
                            break;
                        case 20:
                            Display("SmartPayout note paid into cashbox on startup");
                            break;
                        case 21:
                            Display("SmartPayout cashbox removed");
                            break;
                        case 22:
                            Display("SmartPayout cashbox replaced");
                            break;
                        case 23:
                            Display("SmartPayout despensing notes");
                            break;
                        case 24:
                            Display("SmartPayout dispensed notes");
                            break;
                        case 25:
                            Display("SmartPayout emptying...");
                            break;
                        case 26:
                            Display("SmartPayout emptied");
                            break;
                        case 27:
                            Display("SmartPayout SMART Emptying");
                            break;
                        case 28:
                            Display("SmartPayout SMART Emptied, getting info...");
                            break;
                        case 29:
                            Display("SmartPayout unit jammed");
                            break;
                        case 30:
                            Display("SmartPayout halted");
                            break;
                        case 31:
                            Display("SmartPayout incomplete payout");
                            break;
                        case 32:
                            Display("SmartPayout incomplete float");
                            break;
                        case 33:
                            Display("SmartPayout note transferred to stacker");
                            break;
                        case 34:
                            Display("SmartPayout note in bezel");
                            break;
                        case 35:
                            Display("SmartPayout payout out of service");
                            break;
                        case 36:
                            Display("SmartPayout timeout searching for a note");
                            break;
                        case 37:
                            Display("SmartPayout unsupported poll response received");
                            break;
                        default:
                            //Display("Credit is: " + PollNumber.ToString());
                            break;
                    }
                    if (textBox1.Text.IndexOf("Command response is CANNOT PROCESS COMMAND") != -1)   //No remainder in notes
                    {
                        if (((Math.Abs(InitalCost - Payment) + ReturnMoney) / 1000 > 0) && int.Parse(TenEuroNotesLevel) > 0)
                        {
                            DNote10 = Math.Abs(InitalCost - Payment) / 1000;
                            Display("No more 10 euro notes in Note Float...");
                            SM = 6;
                            break;
                        }
                        if ((Math.Abs(InitalCost - Payment) + ReturnMoney) / 500 > 0)
                        {
                            DNote5 = Math.Abs(InitalCost - Payment) / 500;
                            Display("No more 5 euro notes in Note Float, go to return coins...");
                            SM = 6;
                            break;
                        }
                    }
                    if (textBox1.Text.IndexOf("Busy") != -1)
                    {
                        DisableChangeNotes = true;
                        SM = 6;
                        //Thread.Sleep(300);
                        //btnPayout_Click(this, e);
                        Display("Note Validator responded Busy condition. Rest of change in coins...");
                        break;
                    }
                    if (textBox1.Text.IndexOf("Unsafe jam") != -1)
                    {
                        DisableChangeNotes = true;
                        SM = 6;
                        break;
                    }
                    break;

                case 65:    //WAIT TO RETRIVE NOTES - CASH TRANSACTION WAS CANCELLED
                    Display("SM: " + SM);
                    Display("Wait to retrive note(s) from cancel...");
                    textBox1.ScrollToCaret();
                    if (textBox1.Text.IndexOf("Paying out 10.00") != -1 && textBox1.Text.IndexOf("Dispensed note(s)") != -1)
                    {
                        DNote10++;
                        ReturnMoney = ReturnMoney + 1000;
                        Refresh();
                        secondForm.Refresh(); Thread.Sleep(100);
                        string ten = ini.IniReadValue("SerialNV", "Avail10");
                        int temp = Convert.ToInt16(ten); temp--;
                        ini.IniWriteValue("SerialNV", "Avail10", temp.ToString());
                        textBox1.Text = "";
                        tbPayoutAmount.Text = "";
                        SM = 10;
                        break;
                    }
                    if (textBox1.Text.IndexOf("Paying out 5.00") != -1 && textBox1.Text.IndexOf("Dispensed note(s)") != -1)
                    {
                        DNote5++;
                        ReturnMoney = ReturnMoney + 500;
                        Refresh();
                        secondForm.Refresh(); Thread.Sleep(100);
                        string five = ini.IniReadValue("SerialNV", "Avail05");
                        int temp = Convert.ToInt16(five); temp--;
                        ini.IniWriteValue("SerialNV", "Avail05", temp.ToString());
                        textBox1.Text = "";
                        tbPayoutAmount.Text = "";
                        SM = 10;
                        break;
                    }
                    if (textBox1.Text.IndexOf("Command response is CANNOT PROCESS COMMAND") != -1)   //No remainder in notes
                    {
                        if ((Math.Abs(InitalCost - Payment) + ReturnMoney) / 1000 > 0)
                        {
                            DNote10 = (Math.Abs(InitalCost - Payment) + ReturnMoney) / 1000;
                            Display("No more 10 euro notes in Note Float...");
                            SM = 10;
                            break;
                        }
                        if ((Math.Abs(InitalCost - Payment) + ReturnMoney) / 500 > 0)
                        {
                            DNote5 = (Math.Abs(InitalCost - Payment) + ReturnMoney) / 500;
                            Display("No more 5 euro notes in Note Float, go to return coins...");
                            SM = 10;
                            break;
                        }
                    }
                    break;

                case 7:     //PRINT RECEIPT FOR TICKET - CASH
                    Display("SM: " + SM);
                    try
                    {
                        string[] contents = File.ReadAllLines("C:/POS/receipt_ticket_cash.txt");
                        contents[7] = contents[7] + invoice_number;
                        contents[8] = contents[8] + invoice_sequence;
                        contents[9] = contents[9] + DateTime.Now.ToString("dd/MM/yyyy");
                        contents[10] = contents[10] + DateTime.Now.ToString("HH:mm:ss");
                        contents[12] = contents[12] + plate;
                        contents[13] = contents[13] + entered_at;
                        contents[14] = contents[14] + entered_at;
                        contents[15] = contents[15] + charge_until;
                        contents[16] = contents[16] + charge_until;
                        contents[18] = contents[18] + charge;
                        contents[20] = contents[20] + charge;
                        string str1 = Payment.ToString().Length <= 2 ? (Payment.ToString().Length != 2 ? "0,0" + Payment.ToString() : "0," + Payment.ToString()) : Payment.ToString().Insert(Payment.ToString().Length - 2, ",");
                        contents[21] = contents[21] + str1;
                        string str2 = (Payment - InitalCost).ToString();
                        string str3__ = str2.Length <= 2 ? (str2.Length != 2 ? "0,0" + str2 : "0," + str2) : str2.Insert(str2.Length - 2, ",");
                        contents[22] = contents[22] + str3__;
                        File.WriteAllLines("C:/POS/receipt_temp_ticket_cash.txt", contents);
                        streamToPrint = new StreamReader("C:/POS/receipt_temp_ticket_cash.txt");
                        try
                        {
                            printFont = new Font("Arial", 8);
                            PrintDocument printDocument = new PrintDocument();
                            printDocument.PrintController = new StandardPrintController();
                            printDocument.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                            printDocument.Print();
                        }
                        finally
                        {
                            streamToPrint.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Display(ex.Message);
                    }
                    secondForm.Refresh();
                    Thread.Sleep(100);
                    if (InitalCost != 0)
                    {
                        value = Convert.ToString(InitalCost);
                        value = value.Length <= 2 ? (value.Length != 2 ? "0,0" + value : "0," + value) : value.Insert(value.Length - 2, ",");
                    }
                    else
                        value = "0,00";
                    value = value.Trim('.');
                    Payment = 0;
                    secondForm.Vprogress.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.PaymentText.Clear();
                    secondForm.ResidualText.Clear();
                    secondForm.POS_Messages.Clear();
                    secondForm.POS_Messages.AppendText(ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language));
                    secondForm.Refresh();
                    Thread.Sleep(1000);
                    SM = 10;
                    break;

                case 71://PRINT RECEIPT FOR TICKET - CREDIT/DEBIT
                    Display("SM: " + SM);
                    try
                    {
                        string[] contents = File.ReadAllLines("C:/POS/receipt_ticket_credit.txt");
                        contents[7] = contents[7] + invoice_number;
                        contents[8] = contents[8] + invoice_sequence;
                        contents[9] = contents[9] + DateTime.Now.ToString("dd/MM/yyyy");
                        contents[10] = contents[10] + DateTime.Now.ToString("HH:mm:ss");
                        contents[12] = contents[12] + plate;
                        contents[13] = contents[13] + entered_at;
                        contents[14] = contents[14] + entered_at;
                        contents[15] = contents[15] + charge_until;
                        contents[16] = contents[16] + charge_until;
                        contents[18] = contents[18] + charge;
                        string str1 = InitalCost.ToString();
                        string str2 = str1.Length <= 2 ? (str1.Length != 2 ? "0,0" + InitalCost.ToString() : "0," + str1) : str1.Insert(str1.Length - 2, ",");
                        contents[20] = contents[20] + str2;
                        contents[23] = contents[23] + ini.IniReadValue("Params", "Ext_Merchant_ID") + "/" + ini.IniReadValue("Params", "Ext_Terminal_ID");
                        contents[24] = contents[24] + ini.IniReadValue("Params", "Serial_Number");
                        contents[25] = contents[25] + Auth_Code;
                        contents[26] = contents[26] + RRN;
                        File.WriteAllLines("C:/POS/receipt_temp_ticket_credit.txt", contents);
                        streamToPrint = new StreamReader("C:/POS/receipt_temp_ticket_credit.txt");
                        try
                        {
                            this.printFont = new Font("Arial", 8f);
                            PrintDocument printDocument = new PrintDocument();
                            printDocument.PrintController = new StandardPrintController();
                            printDocument.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                            printDocument.Print();
                        }
                        finally
                        {
                            streamToPrint.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Display(ex.Message);
                    }
                    secondForm.Refresh();
                    Thread.Sleep(100);
                    value = value.Trim('.');
                    Payment = 0;
                    secondForm.Vprogress.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.ValueLabel.Visible = false;
                    secondForm.PaymentText.Clear();
                    secondForm.ResidualText.Clear();
                    secondForm.POS_Messages.Clear();
                    secondForm.POS_Messages.AppendText(ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language));
                    secondForm.Refresh();
                    Thread.Sleep(1000);
                    SM = 10;
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Card_Icon.Visible = false;
                    secondForm.Refresh();
                    break;

                case 72:    //PRINT RECEIPT FOR CARD - CASH
                    Display("SM: " + SM);
                    try
                    {
                        string[] contents = File.ReadAllLines("C:/POS/receipt_card_cash.txt");
                        contents[8] = contents[8] + invoice_number;
                        contents[9] = contents[9] + invoice_sequence;
                        contents[10] = contents[10] + DateTime.Now.ToString("dd/MM/yyyy");
                        contents[11] = contents[11] + DateTime.Now.ToString("HH:mm:ss");
                        contents[13] = contents[13] + customer;
                        contents[14] = contents[14] + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        contents[15] = contents[15] + validfrom;
                        contents[16] = contents[16] + validto;
                        contents[17] = contents[17] + duration;
                        contents[19] = contents[19] + charge;
                        string str1 = Payment.ToString().Length <= 2 ? (Payment.ToString().Length != 2 ? "0,0" + Payment.ToString() : "0," + Payment.ToString()) : Payment.ToString().Insert(Payment.ToString().Length - 2, ",");
                        contents[21] = contents[21] + str1;
                        string str2 = (Payment - InitalCost).ToString();
                        string str3_ = str2.Length <= 2 ? (str2.Length != 2 ? "0,0" + str2 : "0," + str2) : str2.Insert(str2.Length - 2, ",");
                        contents[22] = contents[22] + str3_;
                        File.WriteAllLines("C:/POS/receipt_temp_card_cash.txt", contents);
                        streamToPrint = new StreamReader("C:/POS/receipt_temp_card_cash.txt");
                        try
                        {
                            this.printFont = new Font("Arial", 8f);
                            PrintDocument printDocument = new PrintDocument();
                            printDocument.PrintController = (PrintController)new StandardPrintController();
                            printDocument.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                            printDocument.Print();
                        }
                        finally
                        {
                            streamToPrint.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Display(ex.Message);
                    }
                    secondForm.Refresh();
                    Thread.Sleep(100);
                    if (InitalCost != 0)
                    {
                        value = Convert.ToString(InitalCost);
                        value = value.Length <= 2 ? (value.Length != 2 ? "0,0" + value : "0," + value) : value.Insert(value.Length - 2, ",");
                    }
                    else
                        value = "0,00";
                    value = value.Trim('.');
                    Payment = 0;
                    secondForm.Vprogress.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.PaymentText.Clear();
                    secondForm.ResidualText.Clear();
                    secondForm.POS_Messages.Clear();
                    secondForm.POS_Messages.AppendText(ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language));
                    secondForm.Refresh();
                    Thread.Sleep(2000);
                    SM = 10;
                    break;

                case 73:    //PRINT RECEIPT FOR CARD - CREDIT/DEBIT
                    Display("SM: " + SM);
                    try
                    {
                        string[] contents = File.ReadAllLines("C:/POS/receipt_card_credit.txt");
                        contents[8] = contents[8] + invoice_number;
                        contents[9] = contents[9] + invoice_sequence;
                        contents[10] = contents[10] + DateTime.Now.ToString("dd/MM/yyyy");
                        contents[11] = contents[11] + DateTime.Now.ToString("HH:mm:ss");
                        contents[13] = contents[13] + customer;
                        contents[14] = contents[14] + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        contents[15] = contents[15] + validfrom;
                        contents[16] = contents[16] + validto;
                        contents[19] = contents[19] + charge;
                        contents[21] = contents[21] + charge;
                        contents[24] = contents[24] + ini.IniReadValue("Params", "Ext_Merchant_ID") + "/" + ini.IniReadValue("Params", "Ext_Terminal_ID");
                        contents[25] = contents[25] + ini.IniReadValue("Params", "Serial_Number");
                        contents[26] = contents[26] + Auth_Code;
                        contents[27] = contents[27] + RRN;
                        File.WriteAllLines("C:/POS/receipt_temp_card_credit.txt", contents);
                        streamToPrint = new StreamReader("C:/POS/receipt_temp_card_credit.txt");
                        try
                        {
                            this.printFont = new Font("Arial", 8f);
                            PrintDocument printDocument = new PrintDocument();
                            printDocument.PrintController = (PrintController)new StandardPrintController();
                            printDocument.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                            printDocument.Print();
                        }
                        finally
                        {
                            streamToPrint.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Display(ex.Message);
                    }
                    Payment = 0;
                    secondForm.Vprogress.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.POS_Messages.Clear();
                    secondForm.POS_Messages.AppendText(ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language));
                    secondForm.Refresh();
                    Thread.Sleep(2500);
                    SM = 10;
                    break;

                case 80:    //INCOMPLETE PAYMENT FOR TICKET - CASH
                    Display("SM: " + SM);
                    try
                    {
                        string Filepath = "C:/POS/incomplete_pay_ticket_receipt.txt";
                        string strFileName = Filepath;
                        string[] lines;
                        lines = File.ReadAllLines(Filepath);
                        lines[7] = string.Concat(lines[7], invoice_number);
                        lines[8] = string.Concat(lines[8], invoice_sequence);
                        lines[9] = string.Concat(lines[9], DateTime.Now.ToString("dd/MM/yyyy"));
                        lines[10] = string.Concat(lines[10], DateTime.Now.ToString("HH:mm:ss"));
                        lines[12] = string.Concat(lines[12], plate);
                        lines[13] = string.Concat(lines[13], entered_at);
                        lines[14] = string.Concat(lines[14], entered_at);
                        lines[15] = string.Concat(lines[15], charge_until);
                        lines[16] = string.Concat(lines[16], charge_until);
                        lines[18] = string.Concat(lines[18], charge);
                        lines[20] = string.Concat(lines[20], charge);
                        string IncompletePayTotal = Payment.ToString().Length <= 2 ? (Payment.ToString().Length != 2 ? "0,0" + Payment.ToString() : "0," + Payment.ToString()) : Payment.ToString().Insert(Payment.ToString().Length - 2, ",");
                        lines[21] = string.Concat(lines[21], IncompletePayTotal);
                        string IncompletePayAmount = ReturnMoney.ToString();
                        string IncompletePayAmountStr = IncompletePayAmount.Length <= 2 ? (IncompletePayAmount.Length != 2 ? "0,0" + IncompletePayAmount : "0," + IncompletePayAmount) : IncompletePayAmount.Insert(IncompletePayAmount.Length - 2, ",");
                        lines[22] = string.Concat(lines[22], IncompletePayAmountStr);
                        File.WriteAllLines("C:/POS/incomplete_pay_ticket_receipt_temp.txt", lines);

                        streamToPrint = new StreamReader("C:/incomplete_pay_ticket_receipt_temp.txt");
                        try
                        {
                            printFont = new Font("Arial", 8);
                            PrintDocument pd = new PrintDocument();
                            PrintController pc = new StandardPrintController();
                            pd.PrintController = pc;
                            pd.PrintPage += new PrintPageEventHandler
                               (this.pd_PrintPage);
                            pd.Print();
                        }
                        finally
                        {
                            streamToPrint.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Display(ex.Message);
                    }
                    secondForm.Refresh(); Thread.Sleep(100);
                    if (InitalCost != 0)
                    {
                        value = Convert.ToString(InitalCost);
                        if (value.Length > 2) { value = value.Insert(value.Length - 2, ","); }
                        else if (value.Length == 2) { value = "0," + value; }
                        else { value = "0,0" + value; }
                    }
                    else
                    {
                        value = "0,00";
                    }
                    value = value.Trim('.');
                    Payment = 0;
                    secondForm.Vprogress.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.POS_Messages.Clear();
                    Langtemp = ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language);
                    secondForm.POS_Messages.AppendText(Langtemp);
                    secondForm.Refresh();
                    Thread.Sleep(2500);
                    SM = 10;
                    break;

                case 81:    //INCOMPLETE PAYMENT FOR CARD - CASH
                    Display("SM: " + SM);
                    try
                    {
                        string[] lines = File.ReadAllLines("C:/POS/incomplete_pay_card_receipt.txt");
                        lines[8] = lines[8] + invoice_number;
                        lines[9] = lines[9] + invoice_sequence;
                        lines[10] = lines[10] + DateTime.Now.ToString("dd/MM/yyyy");
                        lines[11] = lines[11] + DateTime.Now.ToString("HH:mm:ss");
                        lines[13] = lines[13] + customer;
                        lines[14] = lines[14] + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        lines[15] = lines[15] + validfrom;
                        lines[16] = lines[16] + validto;
                        lines[17] = lines[17] + duration;
                        lines[19] = lines[19] + charge;
                        lines[21] = string.Concat(lines[21], charge);
                        string IncompletePayTotalCard = Payment.ToString().Length <= 2 ? (Payment.ToString().Length != 2 ? "0,0" + Payment.ToString() : "0," + Payment.ToString()) : Payment.ToString().Insert(Payment.ToString().Length - 2, ",");
                        lines[22] = string.Concat(lines[22], IncompletePayTotalCard);
                        string IncompletePayAmountCard = ReturnMoney.ToString();
                        string IncompletePayAmountCardStr = IncompletePayAmountCard.Length <= 2 ? (IncompletePayAmountCard.Length != 2 ? "0,0" + IncompletePayAmountCard : "0," + IncompletePayAmountCard) : IncompletePayAmountCard.Insert(IncompletePayAmountCard.Length - 2, ",");
                        lines[23] = string.Concat(lines[23], IncompletePayAmountCardStr);
                        File.WriteAllLines("C:/POS/incomplete_pay_card_receipt_temp.txt", lines);
                        streamToPrint = new StreamReader("C:/POS/incomplete_pay_card_receipt_temp.txt");
                        try
                        {
                            this.printFont = new Font("Arial", 8f);
                            PrintDocument printDocument = new PrintDocument();
                            printDocument.PrintController = (PrintController)new StandardPrintController();
                            printDocument.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                            printDocument.Print();
                        }
                        finally
                        {
                            streamToPrint.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Display(ex.Message);
                    }
                    secondForm.Refresh();
                    Thread.Sleep(100);
                    if (InitalCost != 0)
                    {
                        value = Convert.ToString(InitalCost);
                        value = value.Length <= 2 ? (value.Length != 2 ? "0,0" + value : "0," + value) : value.Insert(value.Length - 2, ",");
                    }
                    else
                        value = "0,00";
                    value = value.Trim('.');
                    Payment = 0;
                    secondForm.Vprogress.Visible = false;
                    secondForm.ValueText.Clear();
                    secondForm.PaymentText.Clear();
                    secondForm.ResidualText.Clear();
                    secondForm.POS_Messages.Clear();
                    secondForm.POS_Messages.AppendText(ini.IniReadValue("LANGUAGE", "Complete" + secondForm.Language));
                    secondForm.Refresh();
                    Thread.Sleep(2000);
                    SM = 10;
                    break;

                case 10:    //Cancel or end of transaction
                    Display("SM: " + SM);
                    Display("End of transaction, or cancel button pressed");
                    B = DateTime.Now;
                    batchIsClosed = false;
                    BR_Card = "";
                    RF_Card = "";
                    cardpresense = false;
                    ticket_scanned = false;
                    card_scanned = false;
                    ShowNotes("000");
                    secondForm.btnYes.Visible = false;
                    secondForm.btnNo.Visible = false;
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.Card_Icon.Visible = false;
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
                    secondForm.SnapshotPictureBox.Visible = false;
                    secondForm.LangPictureBox.Visible = false;
                    secondForm.ReaderPictureBox.Visible = false;
                    secondForm.Messages2.Visible = false;
                    secondForm.cashButton.Visible = false;
                    secondForm.creditButton.Visible = false;
                    secondForm.CashPayment = false;
                    secondForm.CreditCardPayment = false;
                    SM = 1;
                    secondForm.CancelButton = false;
                    secondForm.ValueText.Clear();
                    secondForm.ValueLabel.Visible = false;
                    secondForm.PaymentText.Clear();
                    secondForm.PaymentLabel.Visible = false;
                    secondForm.ResidualText.Clear();
                    secondForm.ResidualLabel.Visible = false;
                    secondForm.lostButton.Visible = false;
                    secondForm.Refresh();
                    if (pm.set(0, 1, 0, 0) != 0){
                        Display("Disable Coin ERROR 100\n");
                        secondForm.CancelButton = false; SM = 11;
                        break;
                    }
                    if (Payment > 0){//GIVE HIM HIS MONEY BACK TODO check if > or < zero
                        secondForm.Ticket_Icon.Visible = false;
                        secondForm.POS_Messages.Clear();
                        secondForm.POS_Messages.AppendText("WAIT\n");
                        secondForm.Refresh();
                        Thread.Sleep(500);
                        
                        if (((Payment - ReturnMoney) / 500) > 0)
                        {
                            secondForm.Ticket_Icon.Visible = false;
                            GeneralTimer.Stop();
                            int x = Math.Abs(Payment - ReturnMoney) / 1000;
                            if (x > 0)
                            {
                                if ((DNote10 <= x) && (int.Parse(TenEuroNotesLevel) > 0))
                                {
                                    if (x > int.Parse(TenEuroNotesLevel))
                                        Display("go return 10 euro notes on cancel: " + int.Parse(TenEuroNotesLevel));
                                    else if (x <= int.Parse(TenEuroNotesLevel))
                                        Display("go return 10 euro notes on cancel: " + (x - DNote10));
                                    tbPayoutAmount.Text = "10";
                                    btnPayout_Click(this, e);
                                    SM = 65;
                                    break;
                                }
                            }
                            int y = Math.Abs(Payment - ReturnMoney) / 500;
                            if (y > 0)
                            {
                                if ((DNote5 <= y) && (int.Parse(FiveEuroNotesLevel) > 0))
                                {
                                    if (y > int.Parse(FiveEuroNotesLevel))
                                        Display("go return 5 euro notes on cancel: " + int.Parse(FiveEuroNotesLevel));
                                    else if (y <= int.Parse(FiveEuroNotesLevel))
                                        Display("go return 5 euro notes on cancel: " + (y - DNote5));
                                    tbPayoutAmount.Text = "5";
                                    btnPayout_Click(this, e);
                                    SM = 65;
                                    break;
                                }
                            }
                            GeneralTimer.Start();
                        }
                        int res1 = 0;
                        if ((Payment - ReturnMoney) > 0)
                        {
                            Display("Go Return Coins\n");
                            res1 = pm.set(1, 0, (Payment - ReturnMoney), 0);
                            ReturnMoney = ReturnMoney + res1;
                         
                            if ((Payment - ReturnMoney)>0)
                            {
                                res1 = pm.set(1, 0, (Payment - ReturnMoney), 0);
                                ReturnMoney = ReturnMoney + res1;
                            }
                        }
                        if ((Payment - ReturnMoney) <= 5)
                        {
                            Display("Return Coin Change OK:" + ReturnMoney.ToString() + "\n");
                            SM = 1;
                        }
                        else
                        {
                            Display("INCOMPLETE Pay: " + Payment.ToString() + " " + ReturnMoney.ToString() + " " + res1.ToString() + "\n");
                        }
                    }
                    
                    if (pm.set(0, 1, 0, 0) != 0){
                        Display("Disable Coin ERROR 101\n"); secondForm.CancelButton = false;
                        SM = 11; 
                        break;
                    }

                    secondForm.POS_Messages.Clear();
                    Langtemp = ini.IniReadValue("LANGUAGE", "Thank" + secondForm.Language);
                    secondForm.POS_Messages.AppendText(Langtemp);
                    type = ""; state = ""; value = ""; tstart = "";
                    tend = ""; plate = ""; ticket = ""; receipt = "";
                    UserCode = "";
                    //BR_Card = "";
                    entered_at = "";
                    charge = "";
                    charge_double = 0;
                    charge_until = "";
                    date_time_now = "";
                    invoice_number = "";
                    invoice_sequence = "";

                    MoneyStatus(1);//MONEY UPDATE STON SERVER

                    if (Total_Coins < 10)
                    {
                        Display("Money Very Low");
                        SM = 11;
                    }
                    //secondForm.POS_Messages.Clear();
                    this.Debugging.Clear();
                    secondForm.Refresh();
                    Thread.Sleep(200);                    
                    break;

                case 11: //SERIOUS ERROR
                    Display("SM: " + SM);
                    secondForm.Cancel.Visible = false;
                    Cursor.Show();
                    p = false;
                    Refresh_Click(this, e);
                    LanguageTimer.Stop();
                    GeneralTimer.Stop();
                    ShowNotes("000");
                    pm.set(0, 1, 0, 0);                                                                                  
                    secondForm.POS_Messages.Font = new Font("Arial", 14,FontStyle.Bold);
                    secondForm.POS_Messages.Clear();
                    Langtemp = ini.IniReadValue("LANGUAGE", "ERROR" + secondForm.Language);
                    secondForm.POS_Messages.AppendText(Langtemp);
                    secondForm.Refresh(); 

                    Thread.Sleep(5000);
                    GeneralTimer.Interval = 1000;

                    secondForm.btnYes.Visible = false;
                    secondForm.btnNo.Visible = false;
                    secondForm.LangPictureBox.Visible = false; 
                    secondForm.ReaderPictureBox.Visible = false; 
                    secondForm.buttonUK.Visible = false; 
                    secondForm.buttonGRE.Visible = false; 
                    secondForm.buttonGER.Visible = false; 
                    secondForm.buttonFRA.Visible = false; 
                    secondForm.Vprogress.Visible = false; 
                    secondForm.ValueText.Visible = false; 
                    secondForm.POS_Messages.Visible = false;
                    secondForm.Messages2.Visible = false;
                    secondForm.pictureBox1.Visible = false;
                    secondForm.richTextBox1.Visible = false;
                    secondForm.Ticket_Icon.Visible = false;
                    secondForm.lostButton.Visible = false;

                    image = Image.FromFile("C:/POS/caution.png");
                    secondForm.BackgroundImage = image;
                    secondForm.Refresh();
                    SM = 12; GeneralTimer.Start();
                    break;

                case 12:    //Wait State on OUT OF ORDER
                    Display("SM: " + SM); 
                    secondForm.Refresh();
                    if (BR_Card.Length != 0 && BR_Card == "ADMIN")
                    {
                        secondForm.Close();
                    }
                    BR_Card = "";
                    RF_Card = "";
                    break;
                default:
                    break;
                            
            }//switch
            GeneralTimer.Start();
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
            /*TCP_Send("#DENOM@0@000005$"); Thread.Sleep(10);
            TCP_Send("#DENOM@1@000010$"); Thread.Sleep(10);
            TCP_Send("#DENOM@2@000020$"); Thread.Sleep(10);
            TCP_Send("#DENOM@3@000050$"); Thread.Sleep(10);
            TCP_Send("#DENOM@4@000100$"); Thread.Sleep(10);
            TCP_Send("#DENOM@5@000200$"); Thread.Sleep(10);
            TCP_Send("#DENOM@6@000500$"); Thread.Sleep(10);
            TCP_Send("#DENOM@7@001000$"); Thread.Sleep(10);
            TCP_Send("#DENOM@8@002000$"); Thread.Sleep(10);*/
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

        string Avail05Notes = ini.IniReadValue("SerialNV", "Avail05");
        if (Avail05Notes.Length == 0)
            Avail05Notes = "0";
        if (notesInStorageText.Text == "")
                notesInStorageText.Text = "0";
        Total_Notes = Convert.ToUInt32(notesInStorageText.Text) * 500;
        Paid20Notes.Text = ini.IniReadValue("SerialNV", "Paid20");
        Paid10Notes.Text = ini.IniReadValue("SerialNV", "Paid10");
        Paid05Notes.Text = ini.IniReadValue("SerialNV", "Paid05");

        if (state == 1)
        {
            /*TCP_Send("#CHAV@" + CCoin5 + "@" + CCoin10 + "@" + CCoin20 + "@" + CCoin50 + "@" + CCoin100 + "@" + CCoin200 + "@" + Avail05Notes + "@" + Avail10Notes + "@0$");
            Thread.Sleep(1000);
            TCP_Send("#CASH@" + CCoin5 + "@" + CCoin10 + "@" + CCoin20 + "@" + CCoin50 + "@" + CCoin100 + "@" + CCoin200 + "@" + Paid05Notes.Text + "@" + Paid10Notes.Text + "@" + Paid20Notes.Text + "$");*/
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
        public int ReturnNotes(int Notes, EventArgs e)
        {

            byte[] NDOneBill = { 0x01, 0x10, 0x00, 0x10, 0x01, 0x22 };
            int NNotes = Notes;
            byte[] buffer = new byte[256];
            DNote10 = 0;
            GeneralTimer.Stop();
            //payoutBtn_Click(this, e);
            Display("5 euro dispensed, wait to retrieve...");

            try
            {
                while (Notes > 0)
                {
                    if (DNote10 > 0)// buffer[3] == 0xAA)  Debugging.Text.IndexOf("ND Succesful Pay") != -1
                    {
                        Display("successful pay");
                        DNote10--;
                        Notes--;
                        ReturnMoney = ReturnMoney + 500;
                        this.Refresh();
                        secondForm.Refresh(); Thread.Sleep(100);
                        string five = ini.IniReadValue("SerialNV", "Avail05");
                        int temp = Convert.ToInt16(five); temp--;
                        ini.IniWriteValue("SerialNV", "Avail05", temp.ToString());
                        if (Notes > 0)
                        {
                            Display(" next 5 euro dispensed, wait to retrieve...");
                        }
                        break;
                    }
                    else
                    {
                        Thread.Sleep(500);
                        Refresh();
                        GeneralTimer.Start();
                        return NNotes;
                    }
                }
            }
            catch (Exception ex) { }

            if (Notes == 0)
            {
                GeneralTimer.Start();
                return NNotes;
            }

            Display("SERIOUS ERROR with NV\n");

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

        private void quitBtn_Click(object sender, EventArgs e)
        {
            DW.Abort();
            btnHalt_Click(this, e);
            Thread.Sleep(2000);
            //Running = false;
            Properties.Settings.Default.CommWindow = logTickBox.Checked;
            Properties.Settings.Default.ComPort = Global.ComPort;
            Properties.Settings.Default.Save();
            base.Dispose();
            this.Close();
        }

        private void CloseBatchButton_Click(object sender, EventArgs e)
        {
            SystemIdentification = Encoding.ASCII.GetBytes(DateTime.Now.ToString("ddMMyyyyHHmmss"));
            TransactionType = new byte[2] { 0x31, 0x30 };
            Display("System ID: " + Encoding.UTF8.GetString(SystemIdentification));
            Display("Transaction Type: " + Encoding.UTF8.GetString(TransactionType));
            Display("External Merchant ID: " + Encoding.UTF8.GetString(ExtMerchantID));
            Display("External Terminal ID: " + Encoding.UTF8.GetString(ExtTerminalID));
            BytestoSend = new byte[SystemIdentification.Length + TransactionType.Length + ExtMerchantID.Length + ExtTerminalID.Length + ETX.Length];
            System.Buffer.BlockCopy(SystemIdentification, 0, BytestoSend, 0, SystemIdentification.Length);
            System.Buffer.BlockCopy(TransactionType, 0, BytestoSend, SystemIdentification.Length, TransactionType.Length);
            System.Buffer.BlockCopy(ExtMerchantID, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length), ExtMerchantID.Length);
            System.Buffer.BlockCopy(ExtTerminalID, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length + ExtMerchantID.Length), ExtTerminalID.Length);
            System.Buffer.BlockCopy(ETX, 0, BytestoSend, (SystemIdentification.Length + TransactionType.Length + ExtMerchantID.Length + ExtTerminalID.Length), ETX.Length);
            LRC[0] = CalculateLRC(BytestoSend, BytestoSend.Length);
            //////
            //////start serial send
            //////
            serialEFT_POS.Write(STX, 0, STX.Length);
            serialEFT_POS.Write(SystemIdentification, 0, SystemIdentification.Length);
            serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
            serialEFT_POS.Write(TransactionType, 0, TransactionType.Length);
            for (int i = 0; i < 10; i++)
            {
                serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
            }
            serialEFT_POS.Write(ExtMerchantID, 0, ExtMerchantID.Length);
            serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
            serialEFT_POS.Write(ExtTerminalID, 0, ExtTerminalID.Length);
            for (int i = 0; i < 10; i++)
            {
                serialEFT_POS.Write(FieldSeparator, 0, FieldSeparator.Length);
            }
            serialEFT_POS.Write(ETX, 0, ETX.Length);
            serialEFT_POS.Write(LRC, 0, LRC.Length);
        }

        private void btnSmartEmpty_Click(object sender, EventArgs e)
        {
            Payout.SmartEmpty(textBox1);
            Save_Click(this, e);
            UpdateUI();
            Refresh();
        }

        private void btnHalt_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Poll loop stopped\r\n");
            this.Refresh();
            Running = false;
            btnRun.Enabled = true;
        }

        void MainLoop()
        {
            btnRun.Enabled = false;
            string Port = ini.IniReadValue("SerialNV", "COM");
            string SSP_Address = ini.IniReadValue("SerialNV", "SSP");
            Global.ComPort = Port;
            Global.SSPAddress = Byte.Parse(SSP_Address);
            Payout.CommandStructure.ComPort = Global.ComPort;
            Payout.CommandStructure.SSPAddress = Global.SSPAddress;
            Payout.CommandStructure.Timeout = 3000;

            // connect to validator
            if (ConnectToValidator(reconnectionAttempts, 2))
            {
                Running = true;
                textBox1.AppendText("\r\nPoll Loop\r\n*********************************\r\n");
                Refresh();
                Payout.ConfigureBezel(0x00, 0x00, 0xFF, textBox1);
                btnHalt.Enabled = true;

            }

            while (Running)
            {
                /// if the poll fails, try to reconnect
                if (Payout.DoPoll(textBox1).Item1 == false)
                {
                    textBox1.AppendText("Poll failed, attempting to reconnect...\r\n");
                    this.Refresh();
                    while (true)
                    {
                        Payout.SSPComms.CloseComPort(); // close com port
                        // attempt reconnect, pass over number of reconnection attempts
                        if (ConnectToValidator(reconnectionAttempts, 2) == true)
                            break; // if connection successful, break out and carry on
                        // if not successful, stop the execution of the poll loop
                        btnRun.Enabled = true;
                        btnHalt.Enabled = false;
                        Payout.SSPComms.CloseComPort(); // close com port before return
                        return;
                    }
                    textBox1.AppendText("Reconnected\r\n");
                    this.Refresh();
                }
                timer1.Enabled = true;
                // update form
                UpdateUI();
                // setup dynamic elements of win form once
                if (!bFormSetup)
                {
                    SetupFormLayout();
                    bFormSetup = true;
                }

                    while (timer1.Enabled)
                    {
                        Application.DoEvents();
                        Thread.Sleep(1); // Yield to free up CPU
                    }

                }

                //close com port
                Payout.SSPComms.CloseComPort();

                btnRun.Enabled = true;
                btnHalt.Enabled = false;
            }

        private void noteToRecycleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NV11 != null)
            {

                if (noteToRecycleComboBox.Text == "No Recycling")
                {
                    // switch all notes to stacking
                    textBox1.AppendText("Resetting note routing...\r\n");
                    this.Refresh();
                    NV11.RouteAllToStack(textBox1);
                }
                else if (noteToRecycleComboBox.Text == "Show Routing")
                {
                    textBox1.AppendText("Current note routing:\r\n");
                    this.Refresh();
                    NV11.ShowAllRouting(textBox1);
                    UpdateUI();
                }

                else
                {
                    // switch all notes to stacking first
                    //NV11.RouteAllToStack();
                    // make sure payout is switched on
                    NV11.EnablePayout();
                    // switch selected note to payout
                    string s = noteToRecycleComboBox.Items[noteToRecycleComboBox.SelectedIndex].ToString();
                    string[] sArr = s.Split(' ');
                    try
                    {
                        textBox1.AppendText("Changing note routing...\r\n");
                        this.Refresh();
                        NV11.ChangeNoteRoute(Int32.Parse(sArr[0]) * 100, sArr[1].ToCharArray(), false, textBox1);
                        NV11.ShowAllRouting(textBox1);
                        UpdateUI();
                    }
                    catch (Exception ex)
                    {
                        Display(ex.ToString());
                        return;
                    }
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            MainLoop();
        }
        // This updates UI variables such as textboxes etc.
        void UpdateUI()
        {
            // update text boxes
            totalAcceptedNumText.Text = NV11.NotesAccepted.ToString();
            totalNumNotesDispensedText.Text = NV11.NotesDispensed.ToString();
            notesInStorageText.Text = NV11.GetStorageInfo();
            if (notesInStorageText.Text == "")
                Avail05Notes.Text = "0";
            else
                Avail05Notes.Text = notesInStorageText.Text;
        }

        // This function opens the com port and attempts to connect with the validator. It then negotiates
        // the keys for encryption and performs some other setup commands.
        private bool ConnectToValidator(int attempts)
        {
            // setup timer
            System.Windows.Forms.Timer reconnectionTimer = new System.Windows.Forms.Timer();
            reconnectionTimer.Tick += new EventHandler(reconnectionTimer_Tick);
            reconnectionTimer.Interval = 500;//3000; // ms

            // run for number of attempts specified
            for (int i = 0; i < attempts; i++)
            {
                // close com port in case it was open
                NV11.SSPComms.CloseComPort();

                // turn encryption off for first stage
                NV11.CommandStructure.EncryptionStatus = false;

                // if the key negotiation is successful then set the rest up
                if (NV11.OpenComPort(textBox1) && NV11.NegotiateKeys(textBox1))
                {
                    NV11.CommandStructure.EncryptionStatus = true; // now encrypting
                    // find the max protocol version this validator supports
                    byte maxPVersion = FindMaxProtocolVersion();
                    if (maxPVersion >= 6)
                    {
                        NV11.SetProtocolVersion(maxPVersion, textBox1);
                    }
                    else
                    {
                        MessageBox.Show("This program does not support validators under protocol 6!", "ERROR");
                        return false;
                    }
                    // get info from the validator and store useful vars
                    NV11.ValidatorSetupRequest(textBox1);
                    // inhibits, this sets which channels can receive notes
                    NV11.SetInhibits(textBox1);
                    // enable, this allows the validator to operate
                    NV11.EnableValidator(textBox1);
                    // value reporting, set whether the validator reports channel or coin value in 
                    // subsequent requests
                    NV11.SetValueReportingType(false, textBox1);
                    // check for notes already in the float on startup
                    NV11.CheckForStoredNotes(textBox1);
                    // 
                    return true;
                }
                // reset timer
                reconnectionTimer.Enabled = true;
                while (reconnectionTimer.Enabled) Application.DoEvents();
            }
            return false;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            NV11.ReturnNote(textBox1);
        }

        // This is a one off function that is called the first time the MainLoop()
        // function runs, it just sets up a few of the UI elements that only need
        // updating once.
        private void SetupFormLayout()
        {
            // need validator class instance
            if (NV11 == null)
            {
                MessageBox.Show("NV11 class is null.", "ERROR");
                return;
            }

            // find number and value of channels and update combo box
            noteToRecycleComboBox.Items.Add("No Recycling");

            foreach (ChannelData d in NV11.UnitDataList)
            {
                string s = d.Value / 100 + " " + d.Currency[0] + d.Currency[1] + d.Currency[2];
                noteToRecycleComboBox.Items.Add(s);
            }

            noteToRecycleComboBox.Items.Add("Show Routing");

            // start on second choice which will always be 5 euro note recycling
            noteToRecycleComboBox.Text = noteToRecycleComboBox.Items[1].ToString();
        }

        private void reconnectionTimer_Tick(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.Timer)
            {
                System.Windows.Forms.Timer t = sender as System.Windows.Forms.Timer;
                t.Enabled = false;
            }
        }

        // This function finds the maximum protocol version that a validator supports. To do this
        // it attempts to set a protocol version starting at 6 in this case, and then increments the
        // version until error 0xF8 is returned from the validator which indicates that it has failed
        // to set it. The function then returns the version number one less than the failed version.
        private byte FindMaxProtocolVersion()
        {
            // not dealing with protocol under level 6
            // attempt to set in validator
            byte b = 0x06;
            while (true)
            {
                NV11.SetProtocolVersion(b);
                if (NV11.CommandStructure.ResponseData[0] == CCommands.SSP_RESPONSE_FAIL)
                    return --b;
                b++;
                if (b > 20) return 0x06; // return lowest if p version runs too high
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void cashboxBtn_Click(object sender, EventArgs e)
        {
            if (NV11 != null)
                NV11.StackNextNote(textBox1);
        }

        private void payoutBtn_Click(object sender, EventArgs e)
        {
            if (NV11 != null)
            {
                // make sure payout is switched on
                NV11.EnablePayout();
                NV11.PayoutNextNote(textBox1);
                //Display(textBox1.Text);
            }
            //DNote++;
        }

        private void resetValidatorBtn_Click(object sender, EventArgs e)
        {
            if (NV11 != null)
            {
                NV11.Reset(textBox1);
                NV11.SSPComms.CloseComPort(); // close com port to force reconnect
            }
        }

        private void ResetTotalsText_Click(object sender, EventArgs e)
        {
            if (NV11 != null)
            {
                NV11.NotesAccepted = 0;
                NV11.NotesDispensed = 0;
            }
        }

        private void logTickBox_CheckedChanged(object sender, EventArgs e)
        {
            if (logTickBox.Checked)
                NV11.CommsLog.Show();
            else
                NV11.CommsLog.Hide();
        }

        private void chkHold_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHold.Checked)
            {
                NV11.HoldNumber = 10;

            }
            else
            {
                NV11.HoldNumber = 0;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lostTimer_Tick(object sender, EventArgs e)
        {
            secondForm.LostButton = false;
            lostTimer.Enabled = false;
            SM = 1;
        }

        private void emptyNoteFloatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (NV11 != null)
            {
                // make sure payout is switched on
                NV11.EnablePayout();
                NV11.EmptyPayoutDevice(textBox1);
            }
            string five = ini.IniReadValue("SerialNV", "Paid05");
            string five1 = ini.IniReadValue("SerialNV", "Avail05");
            int temp = Convert.ToInt16(five);
            int temp1 = Convert.ToUInt16(five1);
            temp = temp + temp1;
            ini.IniWriteValue("SerialNV", "Paid05", temp.ToString());
            Paid05Notes.Text = temp.ToString();
        }

        private void totalAcceptedNumText_TextChanged(object sender, EventArgs e)
        {
            notesIn = true;
        }

        private void totalNumNotesDispensedText_TextChanged(object sender, EventArgs e)
        {
            if (SM == 64)
            {
                DNote++;
                Display("notes in storage text: " + notesInStorageText.Text);
                Display("5 euro note was retrieved!!!");
                string five = notesInStorageText.Text;
                ini.IniWriteValue("SerialNV", "Avail05", five);
                if (secondForm.CashPayment)
                    SM = 6;
                else if (secondForm.CreditCardPayment)
                    SM = 61;
            }
            if (SM == 65)
            {
                DNote++;
                Display("5 euro note was retrieved!!!");
                string five = notesInStorageText.Text;
                ini.IniWriteValue("SerialNV", "Avail05", five);
                SM = 10;
            }
        }
        private void ResetAllNotesbtn_Click(object sender, EventArgs e)
        {
            Paid05Notes.Text = "0";
            Paid10Notes.Text = "0";
            Paid20Notes.Text = "0";
            Paid50Notes.Text = "0";
        }

        private void ResetAllCoinsbtn_Click(object sender, EventArgs e)
        {
            Paid05Coins.Text = "0";
            Paid10Coins.Text = "0";
            Paid20Coins.Text = "0";
            Paid50Coins.Text = "0";
            Paid100Coins.Text = "0";
            Paid200Coins.Text = "0";
        }

        /**************************************************************************************/
    }//public partial class Form1 : Form
}
