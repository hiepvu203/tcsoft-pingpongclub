﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class PermissionRolesController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public PermissionRolesController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: PermissionRoles
        public async Task<IActionResult> Index(int? idRole)
        {
            if (idRole==null)
            {
                return BadRequest("IdRole is required.");
            }

            var permissionRoles = await _context.PermissionRoles
        .Where(pr => pr.IdRole == idRole)    // Lọc theo IdRole
        .Include(pr => pr.IdPermissionNavigation)  // Nạp thông tin về Permission
        .Include(pr => pr.IdRoleNavigation)       // Nạp thông tin về Role
        .ToListAsync();  // Thực hiện truy vấn bất đồng bộ

            return View(permissionRoles);

        }


        // GET: PermissionRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
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
        public IActionResult Create()
        {
            ViewData["IdPermission"] = new SelectList(_context.Permissions, "IdPermission", "IdPermission");
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole");
            return View();
        }

        // POST: PermissionRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPerRo,IdRole,IdPermission,Status")] PermissionRole permissionRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permissionRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPermission"] = new SelectList(_context.Permissions, "IdPermission", "IdPermission", permissionRole.IdPermission);
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", permissionRole.IdRole);
            return View(permissionRole);
        }

        // GET: PermissionRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissionRole = await _context.PermissionRoles.FindAsync(id);
            if (permissionRole == null)
            {
                return NotFound();
            }
            ViewData["IdPermission"] = new SelectList(_context.Permissions, "IdPermission", "IdPermission", permissionRole.IdPermission);
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", permissionRole.IdRole);
            return View(permissionRole);
        }

        // POST: PermissionRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPerRo,IdRole,IdPermission,Status")] PermissionRole permissionRole)
        {
            if (id != permissionRole.IdPerRo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permissionRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionRoleExists(permissionRole.IdPerRo))
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
            ViewData["IdPermission"] = new SelectList(_context.Permissions, "IdPermission", "IdPermission", permissionRole.IdPermission);
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", permissionRole.IdRole);
            return View(permissionRole);
        }

        // GET: PermissionRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
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
                _context.PermissionRoles.Remove(permissionRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermissionRoleExists(int id)
        {
            return _context.PermissionRoles.Any(e => e.IdPerRo == id);
        }
    }
}