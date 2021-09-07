using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

//using System.Globalization;

namespace POS_v20
{
    public partial class Form2 : Form
    {
        public int PMaxX;
        public int PMaxY;
        public int GREX,UKX,FRAX,GERX;
        public int GREY,UKY,FRAY,GERY;
        public string Language;
        public bool CancelButton = false;
        public int btnYN = 0;
        
        public Form2()
        {
            InitializeComponent();
            PMaxX = this.Height;
            PMaxY = this.Width;
            GREX = this.buttonGRE.Location.X;
            GREY = this.buttonGRE.Location.Y;
            UKX = this.buttonUK.Location.X;
            UKY = this.buttonUK.Location.Y;
            Language = "";
            LangPictureBox.Visible = false;
            ReaderPictureBox.Visible = false;
        }
/*****************************************************/
        private void buttonUK_Click(object sender, EventArgs e)
        {
            LangPictureBox.Visible = true;
            Image image = Image.FromFile("C:/POS/UNKG0001.GIF");
            LangPictureBox.Image = image;
            this.Update();
            Language = "ENG";
        }
/*****************************************************/
        private void buttonGRE_Click(object sender, EventArgs e)
        {
            LangPictureBox.Visible = true;
            Image image = Image.FromFile("C:/POS/GREC0001.GIF");
            LangPictureBox.Image = image;
            this.Update();
            Language = "GRE";
        }
/*****************************************************/
        private void buttonFRA_Click(object sender, EventArgs e)
        {
            LangPictureBox.Visible = true;
            Image image = Image.FromFile("C:/POS/FRAN0001.GIF");
            LangPictureBox.Image = image;
            this.Update();
            Language = "FRA";
        }
/*****************************************************/
        private void buttonGER_Click(object sender, EventArgs e)
        {
            LangPictureBox.Visible = true;
            Image image = Image.FromFile("C:/POS/GERM0001.GIF");
            LangPictureBox.Image = image;
            this.Update();
            Language = "GER";
        }
/*****************************************************/
        private void Cancel_Click(object sender, EventArgs e)
        {
            CancelButton = true;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            btnYN = 1;
            btnYes.Visible = false;
            btnNo.Visible = false;
            this.Update();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            btnYN = 2;
            btnYes.Visible = false;
            btnNo.Visible = false;
            this.Update();
        }

/*****************************************************/
        
    }
}
