namespace POS_v20
{
    partial class Form2
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
            this.Cancel = new System.Windows.Forms.Button();
            this.Messages2 = new System.Windows.Forms.RichTextBox();
            this.POS_Messages = new System.Windows.Forms.RichTextBox();
            this.Vprogress = new System.Windows.Forms.ProgressBar();
            this.ValueText = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.Ticket_Icon = new System.Windows.Forms.PictureBox();
            this.LangPictureBox = new System.Windows.Forms.PictureBox();
            this.buttonGER = new System.Windows.Forms.Button();
            this.buttonFRA = new System.Windows.Forms.Button();
            this.buttonUK = new System.Windows.Forms.Button();
            this.buttonGRE = new System.Windows.Forms.Button();
            this.ReaderPictureBox = new System.Windows.Forms.PictureBox();
            this.pictureBox20 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox05 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Ticket_Icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReaderPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox05)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.Silver;
            this.Cancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Font = new System.Drawing.Font("Verdana", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Cancel.Location = new System.Drawing.Point(124, 492);
            this.Cancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(326, 60);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "CANCEL";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Visible = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Messages2
            // 
            this.Messages2.BackColor = System.Drawing.Color.Gainsboro;
            this.Messages2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Messages2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Messages2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Messages2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Messages2.Location = new System.Drawing.Point(456, 257);
            this.Messages2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Messages2.Name = "Messages2";
            this.Messages2.ReadOnly = true;
            this.Messages2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.Messages2.Size = new System.Drawing.Size(322, 192);
            this.Messages2.TabIndex = 7;
            this.Messages2.Text = "";
            this.Messages2.Visible = false;
            // 
            // POS_Messages
            // 
            this.POS_Messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.POS_Messages.BackColor = System.Drawing.Color.Gainsboro;
            this.POS_Messages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.POS_Messages.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.POS_Messages.DetectUrls = false;
            this.POS_Messages.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Bold);
            this.POS_Messages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.POS_Messages.HideSelection = false;
            this.POS_Messages.Location = new System.Drawing.Point(24, 186);
            this.POS_Messages.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.POS_Messages.Name = "POS_Messages";
            this.POS_Messages.ReadOnly = true;
            this.POS_Messages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.POS_Messages.Size = new System.Drawing.Size(426, 127);
            this.POS_Messages.TabIndex = 8;
            this.POS_Messages.Text = "";
            // 
            // Vprogress
            // 
            this.Vprogress.BackColor = System.Drawing.Color.LightGray;
            this.Vprogress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Vprogress.Location = new System.Drawing.Point(481, 541);
            this.Vprogress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Vprogress.Maximum = 121;
            this.Vprogress.Name = "Vprogress";
            this.Vprogress.Size = new System.Drawing.Size(277, 11);
            this.Vprogress.Step = -1;
            this.Vprogress.TabIndex = 9;
            this.Vprogress.Value = 120;
            this.Vprogress.Visible = false;
            // 
            // ValueText
            // 
            this.ValueText.BackColor = System.Drawing.Color.Gainsboro;
            this.ValueText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ValueText.Font = new System.Drawing.Font("Verdana", 14.2F, System.Drawing.FontStyle.Bold);
            this.ValueText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ValueText.Location = new System.Drawing.Point(571, 462);
            this.ValueText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ValueText.Multiline = false;
            this.ValueText.Name = "ValueText";
            this.ValueText.ReadOnly = true;
            this.ValueText.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ValueText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.ValueText.Size = new System.Drawing.Size(187, 57);
            this.ValueText.TabIndex = 10;
            this.ValueText.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Black;
            this.richTextBox1.Location = new System.Drawing.Point(24, 453);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(434, 20);
            this.richTextBox1.TabIndex = 15;
            this.richTextBox1.Text = "";
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.Silver;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Verdana", 11.2F, System.Drawing.FontStyle.Bold);
            this.btnYes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnYes.Location = new System.Drawing.Point(526, 362);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(86, 49);
            this.btnYes.TabIndex = 16;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Silver;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Verdana", 11.2F, System.Drawing.FontStyle.Bold);
            this.btnNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnNo.Location = new System.Drawing.Point(640, 362);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(86, 49);
            this.btnNo.TabIndex = 17;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // Ticket_Icon
            // 
            this.Ticket_Icon.BackgroundImage = global::POS_v20.Properties.Resources.ticket_icon;
            this.Ticket_Icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Ticket_Icon.Location = new System.Drawing.Point(526, 149);
            this.Ticket_Icon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Ticket_Icon.Name = "Ticket_Icon";
            this.Ticket_Icon.Size = new System.Drawing.Size(200, 200);
            this.Ticket_Icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Ticket_Icon.TabIndex = 18;
            this.Ticket_Icon.TabStop = false;
            // 
            // LangPictureBox
            // 
            this.LangPictureBox.Location = new System.Drawing.Point(24, 492);
            this.LangPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LangPictureBox.Name = "LangPictureBox";
            this.LangPictureBox.Size = new System.Drawing.Size(88, 60);
            this.LangPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LangPictureBox.TabIndex = 4;
            this.LangPictureBox.TabStop = false;
            this.LangPictureBox.Visible = false;
            // 
            // buttonGER
            // 
            this.buttonGER.BackgroundImage = global::POS_v20.Properties.Resources.GERM0001;
            this.buttonGER.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonGER.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonGER.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGER.ForeColor = System.Drawing.Color.Silver;
            this.buttonGER.Location = new System.Drawing.Point(348, 289);
            this.buttonGER.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGER.Name = "buttonGER";
            this.buttonGER.Size = new System.Drawing.Size(84, 60);
            this.buttonGER.TabIndex = 3;
            this.buttonGER.UseVisualStyleBackColor = true;
            this.buttonGER.Click += new System.EventHandler(this.buttonGER_Click);
            // 
            // buttonFRA
            // 
            this.buttonFRA.BackgroundImage = global::POS_v20.Properties.Resources.FRAN0001;
            this.buttonFRA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonFRA.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonFRA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFRA.ForeColor = System.Drawing.Color.Silver;
            this.buttonFRA.Location = new System.Drawing.Point(240, 289);
            this.buttonFRA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonFRA.Name = "buttonFRA";
            this.buttonFRA.Size = new System.Drawing.Size(84, 60);
            this.buttonFRA.TabIndex = 2;
            this.buttonFRA.UseVisualStyleBackColor = true;
            this.buttonFRA.Click += new System.EventHandler(this.buttonFRA_Click);
            // 
            // buttonUK
            // 
            this.buttonUK.BackgroundImage = global::POS_v20.Properties.Resources.UNKG0001;
            this.buttonUK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonUK.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonUK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUK.ForeColor = System.Drawing.Color.Silver;
            this.buttonUK.Location = new System.Drawing.Point(132, 289);
            this.buttonUK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonUK.Name = "buttonUK";
            this.buttonUK.Size = new System.Drawing.Size(84, 60);
            this.buttonUK.TabIndex = 1;
            this.buttonUK.UseVisualStyleBackColor = true;
            this.buttonUK.Click += new System.EventHandler(this.buttonUK_Click);
            // 
            // buttonGRE
            // 
            this.buttonGRE.BackgroundImage = global::POS_v20.Properties.Resources.GREC0001;
            this.buttonGRE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonGRE.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonGRE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGRE.ForeColor = System.Drawing.Color.Silver;
            this.buttonGRE.Location = new System.Drawing.Point(24, 289);
            this.buttonGRE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGRE.Name = "buttonGRE";
            this.buttonGRE.Size = new System.Drawing.Size(84, 60);
            this.buttonGRE.TabIndex = 0;
            this.buttonGRE.UseVisualStyleBackColor = true;
            this.buttonGRE.Click += new System.EventHandler(this.buttonGRE_Click);
            // 
            // ReaderPictureBox
            // 
            this.ReaderPictureBox.BackgroundImage = global::POS_v20.Properties.Resources.hand_coin;
            this.ReaderPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ReaderPictureBox.Location = new System.Drawing.Point(668, 107);
            this.ReaderPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ReaderPictureBox.Name = "ReaderPictureBox";
            this.ReaderPictureBox.Size = new System.Drawing.Size(115, 111);
            this.ReaderPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ReaderPictureBox.TabIndex = 6;
            this.ReaderPictureBox.TabStop = false;
            // 
            // pictureBox20
            // 
            this.pictureBox20.Image = global::POS_v20.Properties.Resources._20noteU;
            this.pictureBox20.Location = new System.Drawing.Point(597, 107);
            this.pictureBox20.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox20.Name = "pictureBox20";
            this.pictureBox20.Size = new System.Drawing.Size(65, 129);
            this.pictureBox20.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox20.TabIndex = 13;
            this.pictureBox20.TabStop = false;
            this.pictureBox20.Visible = false;
            // 
            // pictureBox10
            // 
            this.pictureBox10.Image = global::POS_v20.Properties.Resources._10noteU;
            this.pictureBox10.Location = new System.Drawing.Point(526, 107);
            this.pictureBox10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(65, 129);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox10.TabIndex = 12;
            this.pictureBox10.TabStop = false;
            this.pictureBox10.Visible = false;
            // 
            // pictureBox05
            // 
            this.pictureBox05.Image = global::POS_v20.Properties.Resources._5noteU;
            this.pictureBox05.Location = new System.Drawing.Point(455, 107);
            this.pictureBox05.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox05.Name = "pictureBox05";
            this.pictureBox05.Size = new System.Drawing.Size(65, 129);
            this.pictureBox05.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox05.TabIndex = 11;
            this.pictureBox05.TabStop = false;
            this.pictureBox05.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::POS_v20.Properties.Resources.pann01;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(796, 90);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(790, 566);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Ticket_Icon);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Vprogress);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.LangPictureBox);
            this.Controls.Add(this.buttonGER);
            this.Controls.Add(this.buttonFRA);
            this.Controls.Add(this.buttonUK);
            this.Controls.Add(this.buttonGRE);
            this.Controls.Add(this.ReaderPictureBox);
            this.Controls.Add(this.Messages2);
            this.Controls.Add(this.POS_Messages);
            this.Controls.Add(this.ValueText);
            this.Controls.Add(this.pictureBox20);
            this.Controls.Add(this.pictureBox10);
            this.Controls.Add(this.pictureBox05);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.Ticket_Icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReaderPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox05)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button buttonGRE;
        public System.Windows.Forms.Button buttonUK;
        public System.Windows.Forms.Button buttonFRA;
        public System.Windows.Forms.Button buttonGER;
        public System.Windows.Forms.PictureBox LangPictureBox;
        public System.Windows.Forms.PictureBox ReaderPictureBox;
        public System.Windows.Forms.Button Cancel;
        public System.Windows.Forms.RichTextBox Messages2;
        public System.Windows.Forms.RichTextBox POS_Messages;
        public System.Windows.Forms.ProgressBar Vprogress;
        public System.Windows.Forms.RichTextBox ValueText;
        public System.Windows.Forms.PictureBox pictureBox05;
        public System.Windows.Forms.PictureBox pictureBox10;
        public System.Windows.Forms.PictureBox pictureBox20;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.RichTextBox richTextBox1;
        public System.Windows.Forms.Button btnYes;
        public System.Windows.Forms.Button btnNo;
        public System.Windows.Forms.PictureBox Ticket_Icon;

    }
}