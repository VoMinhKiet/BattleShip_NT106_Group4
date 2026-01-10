namespace NT106_BattleshipClient
{
    partial class frmIn_Battle
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
            this.btnTinNhan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTinNhan
            // 
            this.btnTinNhan.BackColor = System.Drawing.Color.Bisque;
            this.btnTinNhan.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTinNhan.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnTinNhan.Location = new System.Drawing.Point(950, 67);
            this.btnTinNhan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTinNhan.Name = "btnTinNhan";
            this.btnTinNhan.Size = new System.Drawing.Size(118, 85);
            this.btnTinNhan.TabIndex = 1;
            this.btnTinNhan.Text = "Tin nhắn";
            this.btnTinNhan.UseVisualStyleBackColor = false;
            this.btnTinNhan.Click += new System.EventHandler(this.btnTinNhan_Click);
            // 
            // frmIn_Battle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.btnTinNhan);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmIn_Battle";
            this.Text = "frmIn_Battle";
            this.Load += new System.EventHandler(this.frmIn_Battle_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTinNhan;
    }
}