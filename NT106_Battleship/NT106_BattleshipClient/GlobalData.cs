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

        public static bool IsInRoom { get; set; }  // Cờ báo đang ở trong frmRoom
    }
}
