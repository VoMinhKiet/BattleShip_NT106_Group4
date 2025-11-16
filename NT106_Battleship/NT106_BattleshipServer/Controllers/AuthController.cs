using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Models;
using System.Net;
using System.Net.Mail;

namespace NT106_BattleshipServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] NguoiDung user)
        {
            // Kiểm tra trùng username
            var userByUsername = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TenDangNhap == user.TenDangNhap);

            if (userByUsername != null)
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại!" });

            // Kiểm tra trùng email
            var userByEmail = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userByEmail != null)
                return BadRequest(new { message = "Email đã được sử dụng!" });

            // Mã hóa mật khẩu
            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(user.MatKhau);
            user.NgayTao = DateTime.Now;

            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] NguoiDung request)
        {
            var existingUser = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TenDangNhap == request.TenDangNhap);

            // Username không tồn tại
            if (existingUser == null)
                return Unauthorized(new { message = "Tên đăng nhập không tồn tại!" });

            // Kiểm tra mật khẩu
            bool isValid = BCrypt.Net.BCrypt.Verify(request.MatKhau, existingUser.MatKhau);

            if (!isValid)
                return Unauthorized(new { message = "Mật khẩu không đúng!" });

            // Thành công
            return Ok(new
            {
                message = "Đăng nhập thành công!",
                user = new
                {
                    existingUser.Id,
                    existingUser.TenDangNhap,
                    existingUser.Email,
                    existingUser.NgayTao
                }
            });
        }
        // ============== QUÊN MẬT KHẨU – GỬI OTP ==============
        // Body: { "email": "abc@gmail.com" }
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
                return BadRequest(new { message = "Email không tồn tại trong hệ thống!" });

            // Tạo OTP 6 số
            var rnd = new Random();
            string code = rnd.Next(100000, 999999).ToString();

            user.ResetCode = code;
            user.ResetCodeExpire = DateTime.Now.AddMinutes(3); // OTP hết hạn sau 3 phút
            _context.SaveChanges();

            // Gửi email OTP
            try
            {
                SendOtpEmail(user.Email!, code);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Không gửi được email: " + ex.Message });
            }

            return Ok(new { message = "Mã OTP đã được gửi tới email của bạn!" });
        }

        // ============== KIỂM TRA OTP ==============
        // Body: { "email": "...", "code": "123456" }
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest model)
        {
            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
                return BadRequest(new { message = "Email không tồn tại!" });

            if (string.IsNullOrEmpty(user.ResetCode) || user.ResetCodeExpire == null)
                return BadRequest(new { message = "Không có yêu cầu đặt lại mật khẩu nào!" });

            if (user.ResetCodeExpire < DateTime.Now)
                return BadRequest(new { message = "Mã OTP đã hết hạn, vui lòng yêu cầu lại!" });

            if (!string.Equals(user.ResetCode, model.Code))
                return BadRequest(new { message = "Mã OTP không đúng!" });

            // OTP hợp lệ
            return Ok(new { message = "Mã OTP hợp lệ, bạn có thể đặt mật khẩu mới!" });
        }

        // ============== ĐẶT LẠI MẬT KHẨU ==============
        // Body: { "email": "...", "code": "123456", "newPassword": "abc123" }
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
                return BadRequest(new { message = "Email không tồn tại!" });

            if (string.IsNullOrEmpty(user.ResetCode) || user.ResetCodeExpire == null)
                return BadRequest(new { message = "Không có yêu cầu đặt lại mật khẩu nào!" });

            if (user.ResetCodeExpire < DateTime.Now)
                return BadRequest(new { message = "Mã OTP đã hết hạn, vui lòng yêu cầu lại!" });

            if (!string.Equals(user.ResetCode, model.Code))
                return BadRequest(new { message = "Mã OTP không đúng!" });

            // Đổi mật khẩu
            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.ResetCode = null;
            user.ResetCodeExpire = null;

            _context.SaveChanges();

            return Ok(new { message = "Đặt lại mật khẩu thành công!" });
        }

        // ============== HÀM GỬI EMAIL OTP ==============
        private void SendOtpEmail(string toEmail, string code)
        {
            // TODO: thay cấu hình bằng gmail của bạn
            string fromEmail = "battleship.resetpass@gmail.com";
            string appPassword = "utzcnalhuxtlnrzn";

            var mail = new MailMessage();
            mail.From = new MailAddress(fromEmail, "Battleship Support");
            mail.To.Add(toEmail);
            mail.Subject = "Mã OTP đặt lại mật khẩu";
            mail.Body = $"Mã OTP của bạn là: {code}\nMã có hiệu lực trong 3 phút.";
            mail.IsBodyHtml = false;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
    }

    // ========= CÁC CLASS REQUEST PHỤ =========
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class VerifyOtpRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
