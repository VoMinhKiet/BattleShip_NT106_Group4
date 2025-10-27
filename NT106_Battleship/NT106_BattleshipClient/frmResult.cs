using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmResult : Form
    {
        public frmResult()
        {
            InitializeComponent();
        }

        private void frmResult_Load(object sender, EventArgs e)
        {
            // Horizontally center the label
            lbResult.Left = (this.ClientSize.Width - lbResult.Width) / 2;
            lbPoint.Left = (this.ClientSize.Width - lbPoint.Width) / 2;
            btnPlay_Again.Left = (this.ClientSize.Width - btnPlay_Again.Width) / 8;
            btnReturn.Left = btnPlay_Again.Left * 7;
            // Vertically center the label (optional)
            //abel1.Top = (this.ClientSize.Height - label1.Height) / 2;

            // Keep it centered when resizing the form
            lbResult.Anchor = AnchorStyles.None;
            lbPoint.Anchor = AnchorStyles.None;
            btnPlay_Again.Anchor = AnchorStyles.None;
            btnReturn.Anchor = AnchorStyles.None;
        }
    }
}
