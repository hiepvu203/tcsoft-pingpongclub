using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Filter;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class PermissionRolesController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public PermissionRolesController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: PermissionRoles
        public async Task<IActionResult> Index(int ? id)
        {
            if (id==null)
            {
               return RedirectToAction("Index","Role");
            }

            var permissionRoles = await _context.PermissionRoles
        .Where(pr => pr.IdRole == id)
        .Include(pr => pr.IdPermissionNavigation)
        .Include(pr => pr.IdRoleNavigation)      
        .ToListAsync();
            
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.IdRole == id);
            ViewBag.role = role;
            return View(permissionRoles);
        }


        // GET: PermissionRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Role");
            }

            var permissionRole = await _context.PermissionRoles
                .Include(p => p.IdPermissionNavigation)
                .Include(p => p.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdPerRo == id);
            if (permissionRole == null)
            {
                return NotFound();
            }

            return View(permissionRole);
        }

        // GET: PermissionRoles/Create
        // GET: PermissionRoles/Create
        public IActionResult Create(int id)
        {
            var role = _context.Roles.FirstOrDefault(r => r.IdRole == id);
            if (role == null)
            {
                return RedirectToAction("Index", "Role");
            }

            ViewBag.Role = role;
            ViewBag.Permissions = _context.Permissions.ToList();
            return View();
        }

        // POST: PermissionRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, int permissionId)
        {
            var role = _context.Roles.FirstOrDefault(r => r.IdRole == id);
            if (role == null)
            {
                return RedirectToAction("Index", "Role");
            }

            // Kiểm tra xem có permissionId được chọn không
            if (permissionId != 0)
            {
                var permissionRole = new PermissionRole
                {
                    IdRole = id,
                    IdPermission = permissionId,
                    Status = true // Ví dụ trạng thái mặc định
                };
                _context.Add(permissionRole);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = permissionRole.IdRole });
            }

            // Trả về nếu không có permissionId được chọn
            ViewBag.Role = role;
            ViewBag.Permissions = _context.Permissions.ToList();
            ModelState.AddModelError(string.Empty, "Please select a permission.");

            return View();
        }
        // GET: PermissionRoles/Edit/5
        public async Task<IActionResult> Edit(int idRole, int? id)
        {
            var role = _context.Roles.FirstOrDefault(r => r.IdRole == idRole);
            if (role == null)
            {
                return RedirectToAction("Index", "Role");
            }

            var permissionRole = await _context.PermissionRoles.FindAsync(id);
            if (permissionRole == null)
            {
                return RedirectToAction("Index", "Role");
            }

            // Truyền danh sách các Permission và Role vào ViewBag
            ViewBag.Role = role;  // Truyền role hiện tại (không phải danh sách)
            ViewBag.Permissions = _context.Permissions.ToList();  // Truyền danh sách Permissions vào ViewBag

            return View(permissionRole);
        }


        // POST: PermissionRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,int idRole, int permissionId, bool status)
        {
            // Lấy thông tin Role
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.IdRole == idRole);
            if (role == null)
            {
                return RedirectToAction("Index", "Role");
            }

            // Lấy thông tin PermissionRole cần chỉnh sửa
            var permissionRole = await _context.PermissionRoles
                .FirstOrDefaultAsync(pr => pr.IdPerRo == id);

            if (permissionRole == null)
            {
                return NotFound("PermissionRole not found.");
            }

            // Nếu người dùng chọn một Permission mới, cập nhật IdPermission
            if (permissionId != 0)
            {
                permissionRole.IdPermission = permissionId;
            }

            // Cập nhật trạng thái
            permissionRole.Status = status;

            // Lưu thay đổi
            try
            {
                _context.Update(permissionRole);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PermissionRoles.Any(pr => pr.IdPerRo == id))
                {
                    return NotFound("PermissionRole does not exist.");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", new { id = permissionRole.IdRole });
        }


        // GET: PermissionRoles/Delete/5
        // GET: PermissionRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Role");
            }

            var permissionRole = await _context.PermissionRoles
                .Include(p => p.IdPermissionNavigation)
                .Include(p => p.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdPerRo == id);

            if (permissionRole == null)
            {
                return NotFound();
            }

            return View(permissionRole);
        }

        // POST: PermissionRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permissionRole = await _context.PermissionRoles.FindAsync(id);

            if (permissionRole != null)
            {
                var idRole = permissionRole.IdRole;

                _context.PermissionRoles.Remove(permissionRole);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = idRole });
            }

            return RedirectToAction("Index");
        }

        private bool PermissionRoleExists(int id)
        {
            return _context.PermissionRoles.Any(e => e.IdPerRo == id);
        }
    }
}
