using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class NhaTaiTroController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public NhaTaiTroController(ThuctapKtktcn2024Context context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: NhaTaiTro
        public async Task<IActionResult> Index()
        {
            return View(await _context.NhaTaiTros.Where(e => e.Status == false).ToListAsync());
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSponor,NameSponer,UrlLogo, ImageFile")] NhaTaiTro nhaTaiTro)
        {
            if (ModelState.IsValid)
            {
                if (nhaTaiTro.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(nhaTaiTro.ImageFile.FileName);
                    string extension = Path.GetExtension(nhaTaiTro.ImageFile.FileName);
                    nhaTaiTro.UrlLogo = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await nhaTaiTro.ImageFile.CopyToAsync(fileStream);
                    }
                }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSponor,NameSponer,UrlLogo, ImageFile")] NhaTaiTro nhaTaiTro)
        {
            if (id != nhaTaiTro.IdSponor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSponsor = await _context.NhaTaiTros.FindAsync(nhaTaiTro.IdSponor);
                    existingSponsor.NameSponer = nhaTaiTro.NameSponer;

                    if (nhaTaiTro.ImageFile != null)
                    {
                        if (!string.IsNullOrEmpty(existingSponsor.UrlLogo))
                        {
                            string oldImagePath = Path.Combine(_hostEnvironment.WebRootPath + "/images/", existingSponsor.UrlLogo);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string fileName = Path.GetFileNameWithoutExtension(nhaTaiTro.ImageFile.FileName);
                        string extension = Path.GetExtension(nhaTaiTro.ImageFile.FileName);
                        string newFileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(_hostEnvironment.WebRootPath + "/images/", newFileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await nhaTaiTro.ImageFile.CopyToAsync(fileStream);
                        }

                        existingSponsor.UrlLogo = newFileName;
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else if (nhaTaiTro.ImageFile == null)
                    {
                        ModelState.AddModelError("", "No file uploaded. Please select a file.");
                        return View(nhaTaiTro);
                    }
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
                //if (!string.IsNullOrEmpty(nhaTaiTro.UrlLogo))
                //{
                //    string wwwRootPath = _hostEnvironment.WebRootPath;
                //    string path = Path.Combine(wwwRootPath + "/images/", nhaTaiTro.UrlLogo);
                //    if (System.IO.File.Exists(path))
                //    {
                //        System.IO.File.Delete(path);
                //    }
                //}
                nhaTaiTro.Status = true;
                _context.Update(nhaTaiTro);
                //_context.NhaTaiTros.Remove(nhaTaiTro);
                await _context.SaveChangesAsync();
            }

            
            return RedirectToAction(nameof(Index));
        }

        private bool NhaTaiTroExists(int id)
        {
            return _context.NhaTaiTros.Any(e => e.IdSponor == id);
        }
    }
}
