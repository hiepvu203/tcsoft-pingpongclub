using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.Service;

namespace tcsoft_pingpongclub.Controllers
{
    public class RoleController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;
        private readonly AuthorizationService _authorizationService;
        public RoleController(ThuctapKtktcn2024Context context)
        {
            _context = context;
            _authorizationService=new AuthorizationService(context);
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            // Lấy IdRole từ Session
            int? idRole = HttpContext.Session.GetInt32("IdRole");

            // Kiểm tra nếu IdRole không tồn tại
            if (idRole == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Kiểm tra quyền truy cập
            bool hasPermission =  _authorizationService.hasPer(idRole.Value,"role");
            if (hasPermission)
            {
                var roles = await _context.Roles.ToListAsync();
                return View(roles);
            }

            // Trả về lỗi 403 nếu không có quyền
            return Forbid();
        }


        // GET: Role/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.IdRole == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRole,NameRole,Status")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRole,NameRole,Status")] Role role)
        {
            if (id != role.IdRole)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.IdRole))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.IdRole == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.IdRole == id);
        }
    }
}
