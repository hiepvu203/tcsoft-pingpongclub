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
    public class NhaTaiTroController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public NhaTaiTroController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: NhaTaiTro
        public async Task<IActionResult> Index()
        {
            return View(await _context.NhaTaiTros.ToListAsync());
        }

        // GET: NhaTaiTro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaTaiTro = await _context.NhaTaiTros
                .FirstOrDefaultAsync(m => m.IdSponor == id);
            if (nhaTaiTro == null)
            {
                return NotFound();
            }

            return View(nhaTaiTro);
        }

        // GET: NhaTaiTro/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaTaiTro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSponor,NameSponer,UrlLogo")] NhaTaiTro nhaTaiTro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhaTaiTro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhaTaiTro);
        }

        // GET: NhaTaiTro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaTaiTro = await _context.NhaTaiTros.FindAsync(id);
            if (nhaTaiTro == null)
            {
                return NotFound();
            }
            return View(nhaTaiTro);
        }

        // POST: NhaTaiTro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSponor,NameSponer,UrlLogo")] NhaTaiTro nhaTaiTro)
        {
            if (id != nhaTaiTro.IdSponor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaTaiTro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaTaiTroExists(nhaTaiTro.IdSponor))
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
            return View(nhaTaiTro);
        }

        // GET: NhaTaiTro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaTaiTro = await _context.NhaTaiTros
                .FirstOrDefaultAsync(m => m.IdSponor == id);
            if (nhaTaiTro == null)
            {
                return NotFound();
            }

            return View(nhaTaiTro);
        }

        // POST: NhaTaiTro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaTaiTro = await _context.NhaTaiTros.FindAsync(id);
            if (nhaTaiTro != null)
            {
                _context.NhaTaiTros.Remove(nhaTaiTro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhaTaiTroExists(int id)
        {
            return _context.NhaTaiTros.Any(e => e.IdSponor == id);
        }
    }
}
