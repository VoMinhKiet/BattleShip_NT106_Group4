namespace NT106_BattleshipClient
{
    partial class frmNoteGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNoteGame));
            this.lblNotegame = new System.Windows.Forms.Label();
            this.pnlNotegame = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.rchtxtNote = new System.Windows.Forms.RichTextBox();
            this.pnlNotegame.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNotegame
            // 
            this.lblNotegame.AutoSize = true;
            this.lblNotegame.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotegame.Location = new System.Drawing.Point(15, 17);
            this.lblNotegame.Name = "lblNotegame";
            this.lblNotegame.Size = new System.Drawing.Size(607, 54);
            this.lblNotegame.TabIndex = 0;
            this.lblNotegame.Text = "Hướng dẫn chơi game Battleship";
            // 
            // pnlNotegame
            // 
            this.pnlNotegame.BackColor = System.Drawing.Color.LightBlue;
            this.pnlNotegame.Controls.Add(this.btnExit);
            this.pnlNotegame.Controls.Add(this.rchtxtNote);
            this.pnlNotegame.Controls.Add(this.lblNotegame);
            this.pnlNotegame.Location = new System.Drawing.Point(12, 12);
            this.pnlNotegame.Name = "pnlNotegame";
            this.pnlNotegame.Size = new System.Drawing.Size(746, 669);
            this.pnlNotegame.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Red;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(284, 592);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(178, 62);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Trở về";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // rchtxtNote
            // 
            this.rchtxtNote.BackColor = System.Drawing.SystemColors.Info;
            this.rchtxtNote.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rchtxtNote.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rchtxtNote.Location = new System.Drawing.Point(24, 74);
            this.rchtxtNote.Name = "rchtxtNote";
            this.rchtxtNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rchtxtNote.Size = new System.Drawing.Size(698, 512);
            this.rchtxtNote.TabIndex = 1;
            this.rchtxtNote.Text = "";
            this.rchtxtNote.TextChanged += new System.EventHandler(this.rchtxtNote_TextChanged);
            // 
            // frmNoteGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1162, 694);
            this.Controls.Add(this.pnlNotegame);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmNoteGame";
            this.Text = "frmNoteGame";
            this.Load += new System.EventHandler(this.frmNoteGame_Load);
            this.pnlNotegame.ResumeLayout(false);
            this.pnlNotegame.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNotegame;
        private System.Windows.Forms.Panel pnlNotegame;
        private System.Windows.Forms.RichTextBox rchtxtNote;
        private System.Windows.Forms.Button btnExit;
    }
}