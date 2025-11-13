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
            this.pnlYourGrid.Location = new System.Drawing.Point(123, 142);
            this.pnlYourGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlYourGrid.Name = "pnlYourGrid";
            this.pnlYourGrid.Size = new System.Drawing.Size(667, 615);
            this.pnlYourGrid.TabIndex = 0;
            // 
            // frmShip_Sorting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.pnlYourGrid);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmShip_Sorting";
            this.Text = "frmShip_Sorting";
            this.Load += new System.EventHandler(this.frmShip_Sorting_Load_1);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlYourGrid;
    }
}