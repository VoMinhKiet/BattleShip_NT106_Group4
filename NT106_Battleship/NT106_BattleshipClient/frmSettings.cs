using System;

namespace NT106_BattleshipClient
{
    public partial class frmSettings : BaseForm
    {
        private int _volumeBackup;
        private bool _soundEnabledBackup;
        private bool _isSoundEnabled = true;

        public frmSettings()
        {
            InitializeComponent();
            // chống nháy form
            //EnableFormDoubleBuffering();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            mtbNhacNen.Minimum = 0;
            mtbNhacNen.Maximum = 100;

            _volumeBackup = MusicManager.GetVolume();
            _soundEnabledBackup = !MusicManager.IsMuted();

            _isSoundEnabled = _soundEnabledBackup;

            mtbNhacNen.Value = _volumeBackup;
            chkAmThanh.Checked = _isSoundEnabled;
        }

        private void mtbNhacNen_ValueChanged(object sender, EventArgs e)
        {
            MusicManager.SetVolume(mtbNhacNen.Value);
        }

        private void chkAmThanh_CheckedChanged(object sender, EventArgs e)
        {
            _isSoundEnabled = chkAmThanh.Checked;
            MusicManager.SetMute(!_isSoundEnabled);
        }


        private void btnHuy_Click(object sender, EventArgs e)
        {
            MusicManager.SetVolume(_volumeBackup);
            MusicManager.SetMute(!_soundEnabledBackup);

            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            _volumeBackup = MusicManager.GetVolume();
            _soundEnabledBackup = _isSoundEnabled;

            this.Close();
        }
    }
}
