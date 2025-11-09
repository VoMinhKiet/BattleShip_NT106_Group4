using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NT106_BattleshipClient
{
    internal class UserRepository
    {
        // App.config -> <connectionStrings><add name="Db" .../></connectionStrings>
        private readonly string _cs = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;

        public bool UsernameExists(string username)
        {
            const string sql = "SELECT 1 FROM NguoiDung WHERE TenDangNhap = @u";
            using (var con = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@u", SqlDbType.VarChar, 100).Value = username;
                con.Open();
                var r = cmd.ExecuteScalar();
                return r != null;
            }
        }
        public bool EmailExists(string email)
        {
            const string sql = "SELECT 1 FROM NguoiDung WHERE Email = @e";
            using (var con = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@e", SqlDbType.VarChar, 100).Value = email;
                con.Open();
                var r = cmd.ExecuteScalar();
                return r != null;
            }
        }

        public void CreateUser(string username, string password, string email)
        {
            // Hash mật khẩu bằng bcrypt (tự sinh salt)
            string hash = BCrypt.Net.BCrypt.HashPassword(password);

            const string sql = @"INSERT INTO NguoiDung (TenDangNhap, MatKhau, Email)
                                 VALUES (@u, @p, @e)";
            using (var con = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@u", SqlDbType.VarChar, 100).Value = username;
                cmd.Parameters.Add("@p", SqlDbType.VarChar, 100).Value = hash;
                cmd.Parameters.Add("@e", SqlDbType.VarChar, 100).Value =
                    string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool VerifyLogin(string username, string password)
        {
            const string sql = "SELECT MatKhau FROM NguoiDung WHERE TenDangNhap = @u";
            using (var con = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@u", SqlDbType.VarChar, 100).Value = username;
                con.Open();
                var obj = cmd.ExecuteScalar();
                if (obj == null) return false; // không có user
                string storedHash = Convert.ToString(obj);
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
        }
        public bool UpdatePassword(string email, string newPassword)
        {
            const string sql = "UPDATE NguoiDung SET MatKhau = @p WHERE Email = @e";
            using (var con = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@p", SqlDbType.VarChar, 100).Value = newPassword;
                cmd.Parameters.Add("@e", SqlDbType.VarChar, 100).Value = email;

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0; // Trả về true nếu cập nhật thành công
            }
        }
    }
}
