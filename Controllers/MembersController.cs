using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class MembersController : Controller
    {
        private readonly ThuctapKtktcn2024Context context;
        private readonly IWebHostEnvironment environment;

        public MembersController(ThuctapKtktcn2024Context context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index(int pg = 1)
        {
            var members = context.Members.Include(n => n.IdLevelNavigation).ToList();
            const int pageSize = 5;
            if(pg < 1)
            {
                pg = 1;
            }    
            int recsCount = members.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = members.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.listRank = new SelectList(context.Levels.Select(f => new { idLevel = f.IdLevel, levelName = f.LevelName }), "idLevel", "levelName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Member member)
        {
            if(member.Gender == null)
            {
                ModelState.AddModelError("Gender", "Vui lòng chọn giới tính");
            } 
            if(member.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Vui lòng chọn ảnh");
            }
            if(!ModelState.IsValid)
            {
                return View(member);
            }


            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(member.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/img/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                member.ImageFile.CopyTo(stream);
            }
            Member member1 = new Member()
            {
                MemberName = member.MemberName,
                Address = member.Address,
                Phone = member.Phone,
                Emaill = member.Emaill,
                Gender = member.Gender,
                LinkAvatar = newFileName,
                IdLevel = member.IdLevel,
                Username = member.Username,
            };
            context.Members.Add(member1);
            context.SaveChanges();
            return RedirectToAction("Index", "Members");
        }
        public IActionResult Edit(int id)
        {
            var member = context.Members.Include(n => n.IdLevelNavigation).FirstOrDefault(m => m.IdMember == id);
            ViewBag.listRank = new SelectList(context.Levels.Select(f => new { idLevel = f.IdLevel, levelName = f.LevelName }), "idLevel", "levelName");
            if (member == null)
            {
                return RedirectToAction("Index", "Members");
            }
            var member1 = new Member()
            {
                MemberName = member.MemberName,
                Address = member.Address,
                Phone = member.Phone,
                Emaill = member.Emaill,
                Gender = member.Gender,
                IdLevel = member.IdLevel,
            };
            ViewData["MemberImg"] = member.LinkAvatar;
            return View(member1);
        }
        [HttpPost]
        public IActionResult Edit(int id, Member member)
        {
            var member1 = context.Members.Include(n => n.IdLevelNavigation).FirstOrDefault(m => m.IdMember == id);

            if (member1 == null)
            {
                return RedirectToAction("Index", "Members");
            }

            if (!ModelState.IsValid)
            {
                ViewData["MemberImg"] = member1.LinkAvatar;
                return View(member);
            }

            // Kiểm tra nếu có ảnh mới được tải lên
            if (member.ImageFile != null)
            {
                try
                {
                    // Tạo tên file mới
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(member.ImageFile.FileName);

                    // Xây dựng đường dẫn đầy đủ
                    string imageFullPath = Path.Combine(environment.WebRootPath, "img", newFileName);

                    // Lưu ảnh mới
                    using (var stream = System.IO.File.Create(imageFullPath))
                    {
                        member.ImageFile.CopyTo(stream);
                    }

                    // Xóa ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(member1.LinkAvatar))
                    {
                        string oldImgFullPath = Path.Combine(environment.WebRootPath, "img", member1.LinkAvatar);
                        if (System.IO.File.Exists(oldImgFullPath))
                        {
                            System.IO.File.Delete(oldImgFullPath);
                        }
                    }

                    // Cập nhật thông tin ảnh
                    member1.LinkAvatar = newFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi xử lý ảnh: " + ex.Message);
                    ViewData["MemberImg"] = member1.LinkAvatar;
                    return View(member);
                }
            }

            // Cập nhật thông tin thành viên
            member1.MemberName = member.MemberName;
            member1.Address = member.Address;
            member1.Phone = member.Phone;
            member1.Gender = member.Gender;
            member1.IdLevel = member.IdLevel;

            // Lưu thay đổi
            context.SaveChanges();

            return RedirectToAction("Index", "Members");
        }
        public IActionResult Delete(int id)
        {
            var member = context.Members.Include(n => n.IdLevelNavigation).FirstOrDefault(m => m.IdMember == id);
            if (member == null)
            {
                return RedirectToAction("Index", "Members");
            }

            string imageFullPath = environment.WebRootPath + "/img/" + member.LinkAvatar;
            System.IO.File.Delete(imageFullPath);

            context.Members.Remove(member);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Members");
        }

    }

}
