using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class LoginController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;
        private readonly IMemoryCache _memoryCache;
        public LoginController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _context = new ThuctapKtktcn2024Context();
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdMember") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var user = _context.Members
    .FromSqlRaw("SELECT * FROM Member WHERE Username LIKE {0}", username)
    .FirstOrDefault();

            if (user != null)
            {
                var passwordHasher = new PasswordHasher<Member>();
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("IdMember", user.IdMember);
                    HttpContext.Session.SetInt32("IdRole", user.IdRole ?? 0);
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Identify()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Identify(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Email không được để trống!";
                return View();
            }

            var member = _context.Members
    .FromSqlRaw("SELECT * FROM Member WHERE Emaill Like {0}", email)
    .FirstOrDefault();

            if (member == null)
            {
                ViewBag.Error = "Không tìm thấy tài khoản với email này!";
                return View();
            }

            // Tạo mã xác nhận
            Guid code = Guid.NewGuid();
            string cacheKey = $"verificationCode_{email}";
            _memoryCache.Set(cacheKey, code.ToString(), new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5) // Thời gian tồn tại 5 phút
            });

            // Tạo link xác nhận
            string confirmationLink = Url.Action("ConfirmEmail", "Login", new { email, code }, Request.Scheme);

            try
            {
                // Gửi email chứa link xác nhận
                sendEmail(email, confirmationLink);
                ViewBag.Message = $"Link xác minh đã được gửi đến {email}!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi gửi email: {ex.Message}";
                return View();
            }

            return View();
        }

        private void sendEmail(string email, string confirmationLink)
        {
            try
            {
                string fromEmail = "t6983967@gmail.com";
                string password = "proj rbvr ursf wmow";

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Xác minh tài khoản",
                    Body = $"Vui lòng nhấp vào liên kết sau để xác minh tài khoản của bạn: <a href='{confirmationLink}'>Xác minh tài khoản</a>",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gửi email: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string email, string code)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
            {
                return BadRequest("Email hoặc mã xác nhận không hợp lệ!");
            }

            string cacheKey = $"verificationCode_{email}";
            if (!_memoryCache.TryGetValue(cacheKey, out string cachedCode))
            {
                return BadRequest("Mã xác nhận đã hết hạn hoặc không tồn tại!");
            }

            // Kiểm tra mã xác nhận
            if (code != cachedCode)
            {
                return BadRequest("Mã xác nhận không chính xác!");
            }

            

            return RedirectToAction("ResetPassword", "Login", new { email });
        }

        public IActionResult ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Email không hợp lệ!";
                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                ViewBag.Error = "Mật khẩu mới không được để trống!";
                return View();
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Emaill == email);
            if (member == null)
            {
                return NotFound("Không tìm thấy tài khoản với email này!");
            }

            var passwordHasher = new PasswordHasher<Member>();
            member.Password = passwordHasher.HashPassword(member, newPassword).ToString();
            await _context.SaveChangesAsync();
            string cacheKey = $"verificationCode_{email}";
            _memoryCache.Remove(cacheKey);
            ViewBag.Message = "Mật khẩu đã được đặt lại thành công!";
            return RedirectToAction("Index", "Login");
        }

        private bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

    }
}
