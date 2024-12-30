using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class AccountController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public AccountController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            var thuctapKtktcn2024Context = _context.Members.Include(m => m.IdLevelNavigation).Include(m => m.IdRoleNavigation);
            return View(await thuctapKtktcn2024Context.ToListAsync());
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.IdLevelNavigation)
                .Include(m => m.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdMember == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "IdLevel");
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole");
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMember,MemberName,Address,Phone,Emaill,Gender,IdLevel,Status,LinkAvatar,Username,Password,IdRole")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "IdLevel", member.IdLevel);
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", member.IdRole);
            return View(member);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "IdLevel", member.IdLevel);
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", member.IdRole);
            return View(member);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMember,MemberName,Address,Phone,Emaill,Gender,IdLevel,Status,LinkAvatar,Username,Password,IdRole")] Member member)
        {
            if (id != member.IdMember)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.IdMember))
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
            ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "IdLevel", member.IdLevel);
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", member.IdRole);
            return View(member);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.IdLevelNavigation)
                .Include(m => m.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdMember == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.IdMember == id);
        }
    }
}
