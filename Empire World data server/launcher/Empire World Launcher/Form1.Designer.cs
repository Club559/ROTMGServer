namespace Empire_World_Launcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ver = new System.Windows.Forms.Label();
            this.olderButton = new System.Windows.Forms.Label();
            this.newerButton = new System.Windows.Forms.Label();
            this.MoreLink = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(11, 150);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loading...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Orange;
            this.label2.Location = new System.Drawing.Point(314, 150);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 16);
            this.label2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 37F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(359, 261);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 93);
            this.button1.TabIndex = 3;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ver
            // 
            this.ver.AutoSize = true;
            this.ver.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ver.ForeColor = System.Drawing.Color.DarkOrange;
            this.ver.Location = new System.Drawing.Point(0, 350);
            this.ver.Name = "ver";
            this.ver.Size = new System.Drawing.Size(0, 16);
            this.ver.TabIndex = 4;
            // 
            // olderButton
            // 
            this.olderButton.AutoSize = true;
            this.olderButton.ForeColor = System.Drawing.Color.Tomato;
            this.olderButton.Location = new System.Drawing.Point(214, 339);
            this.olderButton.Name = "olderButton";
            this.olderButton.Size = new System.Drawing.Size(71, 16);
            this.olderButton.TabIndex = 5;
            this.olderButton.Text = "OLDER >>";
            this.olderButton.Visible = false;
            this.olderButton.Click += new System.EventHandler(this.olderButton_Click);
            // 
            // newerButton
            // 
            this.newerButton.AutoSize = true;
            this.newerButton.ForeColor = System.Drawing.Color.Tomato;
            this.newerButton.Location = new System.Drawing.Point(94, 339);
            this.newerButton.Name = "newerButton";
            this.newerButton.Size = new System.Drawing.Size(76, 16);
            this.newerButton.TabIndex = 6;
            this.newerButton.Text = "<< NEWER";
            this.newerButton.Visible = false;
            this.newerButton.Click += new System.EventHandler(this.newerButton_Click);
            // 
            // MoreLink
            // 
            this.MoreLink.AutoSize = true;
            this.MoreLink.LinkColor = System.Drawing.Color.Chocolate;
            this.MoreLink.Location = new System.Drawing.Point(11, 314);
            this.MoreLink.Name = "MoreLink";
            this.MoreLink.Size = new System.Drawing.Size(187, 16);
            this.MoreLink.TabIndex = 7;
            this.MoreLink.TabStop = true;
            this.MoreLink.Text = "Click here for more information";
            this.MoreLink.Visible = false;
            this.MoreLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MoreLink_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Empire_World_Launcher.Properties.Resources.sitelogo;
            this.pictureBox1.Location = new System.Drawing.Point(6, 7);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(403, 121);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(528, 366);
            this.Controls.Add(this.MoreLink);
            this.Controls.Add(this.newerButton);
            this.Controls.Add(this.olderButton);
            this.Controls.Add(this.ver);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Empire World Launcher";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label ver;
        private System.Windows.Forms.Label olderButton;
        private System.Windows.Forms.Label newerButton;
        private System.Windows.Forms.LinkLabel MoreLink;
    }
}

