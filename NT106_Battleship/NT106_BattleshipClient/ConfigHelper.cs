using System;
using System.IO;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public static class ConfigHelper
    {
        // IP mặc định nếu không tìm thấy file (để test localhost vẫn được)
        private static string _defaultUrl = "http://localhost:5074";

        public static string GetServerUrl()
        {
            // Tìm file server_ip.txt nằm cùng thư mục với file .exe
            string path = Path.Combine(Application.StartupPath, "server_ip.txt");

            if (File.Exists(path))
            {
                try
                {
                    string ip = File.ReadAllText(path).Trim();
                    if (!string.IsNullOrEmpty(ip))
                    {
                        // Nếu trong file text người dùng quên ghi http:// thì tự thêm vào
                        if (!ip.StartsWith("http")) ip = "http://" + ip;

                        // Xóa dấu / ở cuối nếu có (để tránh lỗi nối chuỗi 2 dấu //)
                        return ip.TrimEnd('/');
                    }
                }
                catch { }
            }
            return _defaultUrl;
        }
    }
}