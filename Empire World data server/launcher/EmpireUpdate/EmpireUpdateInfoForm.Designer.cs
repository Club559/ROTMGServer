namespace EmpireUpdate
{
    partial class EmpireUpdateInfoForm
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lblVersions = new System.Windows.Forms.Label();
            this.txtDescrition = new System.Windows.Forms.RichTextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::EmpireUpdate.Properties.Resources.update;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(80, 80);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // lblVersions
            // 
            this.lblVersions.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersions.Location = new System.Drawing.Point(98, 25);
            this.lblVersions.Name = "lblVersions";
            this.lblVersions.Size = new System.Drawing.Size(164, 54);
            this.lblVersions.TabIndex = 1;
            this.lblVersions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDescrition
            // 
            this.txtDescrition.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescrition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescrition.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtDescrition.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.txtDescrition.Location = new System.Drawing.Point(12, 116);
            this.txtDescrition.Name = "txtDescrition";
            this.txtDescrition.ReadOnly = true;
            this.txtDescrition.Size = new System.Drawing.Size(250, 95);
            this.txtDescrition.TabIndex = 2;
            this.txtDescrition.TabStop = false;
            this.txtDescrition.Text = "";
            this.txtDescrition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDescrition_KeyDown);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblDescription.Location = new System.Drawing.Point(9, 100);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(66, 13);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description";
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.btnBack.Location = new System.Drawing.Point(102, 217);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(73, 23);
            this.btnBack.TabIndex = 4;
            this.btnBack.TabStop = false;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // EmpireUpdateInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 243);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescrition);
            this.Controls.Add(this.lblVersions);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmpireUpdateInfoForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label lblVersions;
        private System.Windows.Forms.RichTextBox txtDescrition;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnBack;
    }
}