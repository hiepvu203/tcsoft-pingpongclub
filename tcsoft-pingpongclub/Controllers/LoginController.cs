using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public LoginController()
        {
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
            var user = _context.Members.FirstOrDefault(m => m.Username == username);
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
            var member = _context.Members.FirstOrDefault(m => m.Emaill == email);
            if (member == null)
            {
                ViewBag.Error = "Không tìm thấy tài khoản với email này!";
                return View();
            }
            Guid code = Guid.NewGuid();
            HttpContext.Session.SetString("verificationCode_" + email, code.ToString());
            try
            {
                
                sendEmail(email, code.ToString());
                ViewBag.Message = $"Mã xác minh đã được gửi đến {email}!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi gửi email: {ex.Message}";
            }

            return View();
        }

        private void sendEmail(string email, string code)
        {
            try
            {
                string fromEmail = "t6983967@gmail.com";
                string password = "proj rbvr ursf wmow";
                string verifyUrl = Url.Action("VerifyCode", "Login", new { email = email }, protocol: Request.Scheme);
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Mã xác minh tài khoản",
                    Body = $"Mã xác minh của bạn là: {code}. Bạn có thể xác minh tài khoản của mình bằng cách nhấn vào liên kết sau: {verifyUrl}.",
                    IsBodyHtml = false
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
        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyCode(string email, string enteredCode)
        {
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                ViewBag.Error = "Email không hợp lệ!";
                return View();
            }

            string sessionCode = HttpContext.Session.GetString("verificationCode_" + email);

            if (string.IsNullOrEmpty(sessionCode))
            {
                ViewBag.Error = "Mã xác minh đã hết hạn hoặc không tồn tại!";
                return View();
            }

            if (enteredCode == sessionCode)
            {
                var member = _context.Members.FirstOrDefault(m => m.Emaill == email);
                if (member != null)
                {
                    HttpContext.Session.SetInt32("member_" + email, member.IdMember);
                    return RedirectToAction("ResetPassword", "Login", new { email = email });
                }
                else
                {
                    ViewBag.Error = "Không tìm thấy thành viên với email này!";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Mã xác minh không chính xác!";
                return View();
            }
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
            
            int? id = HttpContext.Session.GetInt32("member_" + email);
            if (id == null)
            {
                    ViewBag.Error = "Không tìm thấy ID trong phiên làm việc.";
                return NotFound();
            }
            var member = await _context.Members.FirstOrDefaultAsync(m => m.IdMember == id);
            if (member == null)
            {
                return NotFound();
            }

            var passwordHasher = new PasswordHasher<Member>();
            member.Password = passwordHasher.HashPassword(member, newPassword).ToString();
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("member_" + email);
            HttpContext.Session.Remove("verificationCode_" + email);

            ViewBag.Message = "Mật khẩu đã được đặt lại thành công!";
            return RedirectToAction("Index", "Login"); 
        }

        private bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }


    }
}
