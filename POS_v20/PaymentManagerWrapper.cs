using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NRI.PaymenentManager
{
	public class PaymentManagerWrapper
	{
		#region PM Abstraction layer with some common methods

		/// <summary>
		/// Enables all coins/cash items
		/// </summary>
		public void enableAllCashItems() {
			set(0, 0, 0, 0);
		}

		/// <summary>
		/// Inhibits all coins/cash items
		/// </summary>
		public void disableAllCashItems() {
			set(0, 1, 0, 0);
		}

		/// <summary>
		/// Accepts an escrow 
		/// </summary>
		public void openEscrowAccept() {
			set(3, 4, 0, 1);
		}

		/// <summary>
		/// Rejects an escrow
		/// </summary>
		public void openEscrowReject() {
			set(3, 4, 0, 2);
		}

		/// <summary>
		/// Pays out the given residual
		/// </summary>
		public void payout(int residual) {
			if (residual>0) {
				set(1, 0, residual, 0);
			}
		}

		/// <summary>
		/// Prints a formated credit message on a connected display
		/// </summary>
		public void updateDisplay(int cents) {
			string s = String.Format("Total: {0,-9:C}", (cents/100d));
			updateDisplay(s);
		}

		/// <summary>
		/// Prints a message on a connected display
		/// </summary>
		/// <param name="msg"></param>
		public void updateDisplay(string msg) {
			string s = String.Format("\r\n{0}\0", msg);		// Clear display with newline
			IntPtr p = Marshal.StringToHGlobalAnsi(s);		// Set a pointer to char* on the heap
			int r = set(5, 1, 0, p.ToInt32());						// Print message
			Marshal.FreeHGlobal(p);												// Free the used heap
		}

		#endregion


		#region PM DLL-Interface

		/// <summary>
		/// Opens the PaymentManager
		/// </summary>
		public void open() {
			int res = openpaymentmanager();
			if (res != 0)
				throw newException("Error while opening PaymentManager.", res);
		}

		/// <summary>
		/// Opens the PaymentManager on specified ports
		/// </summary>
		/// <param name="ports"></param>
		public void open(int ports) {
			int res = openpaymentmanagerex(ports);
			if (res != 0)
				throw newException("Error while opening PaymentManager ports.", res);
		}

		/// <summary>
		/// Searches devices and starts the PaymentManager communication
		/// </summary>
		/// <param name="winHandle">ID of the handle that receives PaymentManager messages</param>
		/// <param name="winMsgID">ID of PaymenentManager messages (wParam)</param>
		/// <param name="devices">Devices to search for</param>
		/// <param name="protocol">Protocols to search for</param>
		/// <param name="message">Not used yet</param>
		public void start(int winHandle, int winMsgID, int devices, int protocol, int message) {
			//wndHandle = winHandle;
			int res = startpaymentmanager(winHandle, winMsgID, devices, protocol, message);
            if (res >= 0x2000)
                MessageBox.Show("Error while starting PaymentManager");
				//throw newException("Error while starting PaymentManager.", res);
            switch(res){
                case 1:
                    //MessageBox.Show("Coin changer/validator found");
                    break;
                default:
                    MessageBox.Show("UnknownMessage: "+res.ToString());
                    break;
            }
		}

		/// <summary>
		/// Stops the PaymentManager communication 
		/// </summary>
		public void stop() {
			int res = stoppaymentmanager();
			if (res != 0)
				throw newException("Error while stopping PaymentManager.", res);
		}

		/// <summary>
		/// Closes the PaymentManager
		/// </summary>
		public void close() {
			int res = closepaymentmanager();
			if (res > 1)			// 0=OK, 1=Already closed
				throw newException("Error while closing PaymentManager.", res);
		}

		/// <summary>
		/// Sends commands over the PaymentManager (see PaymentManager documentation)
		/// </summary>
		/// <param name="command">Command ID</param>
		/// <param name="info1">Parameter 1</param>
		/// <param name="info2">Parameter 2</param>
		/// <param name="info3">Parameter 3</param>
		/// <returns>Result of the command (see PaymentManager documentation)</returns>
		public int set(int command, int info1, int info2, int info3) {
			int res = setpaymentmanager(command, info1, info2, info3);
			return res;
		}
		
		[DllImport("C:/POS/PaymentManager.dll", CharSet=CharSet.Auto)]
		static extern int openpaymentmanager();
        [DllImport("C:/POS/PaymentManager.dll", CharSet = CharSet.Auto)]
		static extern int openpaymentmanagerex(int ports);
        [DllImport("C:/POS/PaymentManager.dll", CharSet = CharSet.Auto)]
		static extern int startpaymentmanager(int windowid, int messid, int device, int protocol, int message);
        [DllImport("C:/POS/PaymentManager.dll", CharSet = CharSet.Auto)]
		static extern int stoppaymentmanager();
        [DllImport("C:/POS/PaymentManager.dll", CharSet = CharSet.Auto)]
		static extern int closepaymentmanager();
        [DllImport("C:/POS/PaymentManager.dll", CharSet = CharSet.Auto)]
		static extern int setpaymentmanager(int command, int info1, int info2, int info3);

		#endregion

		/// <summary>
		/// Custom exception factory method
		/// </summary>
		protected Exception newException(string msg, int resultCode) {
			return new PaymentManagerException(msg + " (Error code 0x" + resultCode.ToString("X") + ")");
		}
	}

	/// <summary>
	/// Custom exception class
	/// </summary>
	[Serializable]
	public class PaymentManagerException : Exception
	{
		public PaymentManagerException()
			: base() { }

		public PaymentManagerException(string msg)
			: base(msg) { }
	}
}
