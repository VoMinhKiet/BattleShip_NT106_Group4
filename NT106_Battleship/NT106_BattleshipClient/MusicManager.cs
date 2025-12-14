using System;
using WMPLib;

namespace NT106_BattleshipClient
{
    public static class MusicManager
    {
        private static WindowsMediaPlayer _player;
        private static int _volume = 50; // 0 - 100

        // Gọi 1 lần duy nhất khi app chạy
        public static void Init()
        {
            if (_player != null) return;

            _player = new WindowsMediaPlayer();
            _player.settings.volume = _volume;
            _player.settings.setMode("loop", true); // loop nhạc
        }

        // Phát nhạc nền menu
        public static void PlayMenuMusic()
        {
            Init();
            Play("Assets/Music/menu_theme.mp3");
        }

        // Phát nhạc trong game
        public static void PlayInGameMusic()
        {
            Init();
            Play("Assets/Music/ingame_theme.mp3");
        }

        // Hàm dùng chung
        private static void Play(string relativePath)
        {
            _player.URL = relativePath;
            _player.controls.play();
        }

        public static void Stop()
        {
            if (_player == null) return;
            _player.controls.stop();
        }

        public static void SetVolume(int volume)
        {
            _volume = Math.Max(0, Math.Min(100, volume));

            if (_player != null)
                _player.settings.volume = _volume;
        }

        public static int GetVolume()
        {
            return _volume;
        }

        public static void SetMute(bool mute)
        {
            if (_player == null) return;
            _player.settings.mute = mute;
        }

        public static bool IsMuted()
        {
            return _player != null && _player.settings.mute;
        }

    }
}
