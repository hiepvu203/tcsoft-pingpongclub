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
    public class FundController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public FundController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: Fund
        public async Task<IActionResult> Index()
        {
            return View(await _context.Funds.ToListAsync());
        }

        // GET: Fund/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fund = await _context.Funds
                .FirstOrDefaultAsync(m => m.IdFund == id);
            if (fund == null)
            {
                return NotFound();
            }

            return View(fund);
        }

        public void getData()
        {
            ViewData["Status"] = new List<SelectListItem> { new SelectListItem { Value = "false", Text = "Ngừng" }, new SelectListItem { Value = "true", Text = "Hoạt động" } };
        }

        // GET: Fund/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fund/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFund,FundName,Total,Status")] Fund fund)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fund);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fund);
        }

        // GET: Fund/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fund = await _context.Funds.FindAsync(id);
            if (fund == null)
            {
                return NotFound();
            }
            getData();
            return View(fund);
        }

        // POST: Fund/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFund,FundName,Total,Status")] Fund fund)
        {
            if (id != fund.IdFund)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fund);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FundExists(fund.IdFund))
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
            getData();
            return View(fund);
        }

        // GET: Fund/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fund = await _context.Funds
                .FirstOrDefaultAsync(m => m.IdFund == id);
            if (fund == null)
            {
                return NotFound();
            }

            return View(fund);
        }

        // POST: Fund/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fund = await _context.Funds.FindAsync(id);
            if (fund != null)
            {
                fund.Status = true;
                _context.Funds.Update(fund);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool FundExists(int id)
        {
            return _context.Funds.Any(e => e.IdFund == id);
        }
    }
}
