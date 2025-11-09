namespace NT106_BattleshipClient
{
    partial class frmShip_Sorting
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
            this.pnlYourGrid = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlYourGrid
            // 
            this.pnlYourGrid.Location = new System.Drawing.Point(92, 115);
            this.pnlYourGrid.Name = "pnlYourGrid";
            this.pnlYourGrid.Size = new System.Drawing.Size(500, 500);
            this.pnlYourGrid.TabIndex = 0;
            // 
            // frmShip_Sorting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlYourGrid);
            this.Name = "frmShip_Sorting";
            this.Text = "frmShip_Sorting";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlYourGrid;
    }
}