using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NT106_BattleshipClient
{
    public partial class frmFriendlist : Form
    {
        public frmFriendlist()
        {
            InitializeComponent();
        }

        private void frmFriendlist_Load(object sender, EventArgs e)
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("All");
            cbStatus.Items.Add("Online");
            cbStatus.Items.Add("Offline");
            cbStatus.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
