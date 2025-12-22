using System;

namespace NT106_BattleshipClient
{
    public static class GlobalData
    {
        // Lưu ID người dùng sau khi đăng nhập
        public static int UserId { get; set; }

        // Lưu tên đăng nhập
        public static string Username { get; set; }

        // Lưu email người dùng
        public static string Email { get; set; }

        public static int SoSao { get; set; }
        public static int TongSoTran { get; set; }
        public static double TiLeThang { get; set; }

        public static event Action UserInfoUpdated;

        public static void NotifyUserInfoUpdated()
        {
            UserInfoUpdated?.Invoke();
        }
    }
}
