using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class ExpenseAndIncomeController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public ExpenseAndIncomeController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: ExpenseAndIncome
        public async Task<IActionResult> Index()
        {
            var thuctapKtktcn2024Context = await _context.ExpenseAndIncomes
                .Include(e => e.IdFundNavigation)
                .Include(e => e.IdPartyNavigation)
                .Include(e => e.IdReasonNavigation)
                .Join(
                    _context.Members,
                    e => e.IdAccountant,
                    m => m.IdMember,
                    (e, m) => new
                    {
                        ExpenseAndIncome = e,
                        AccountantName = m.MemberName
                    }
                )
                .Where(e => e.ExpenseAndIncome.Status == false)
                .OrderByDescending(e => e.ExpenseAndIncome.Id) 
                .ToListAsync();

            var model = thuctapKtktcn2024Context.Select(e => new ExpenseAndIncome
            {
                Id = e.ExpenseAndIncome.Id,
                IdFund = e.ExpenseAndIncome.IdFund,
                IdParty = e.ExpenseAndIncome.IdParty,
                IdAccountant = e.ExpenseAndIncome.IdAccountant,
                IsDone = e.ExpenseAndIncome.IsDone,
                Type = e.ExpenseAndIncome.Type,
                IdReason = e.ExpenseAndIncome.IdReason,
                DaysOverdue = e.ExpenseAndIncome.DaysOverdue,
                Status = e.ExpenseAndIncome.Status,
                CreatedDate = e.ExpenseAndIncome.CreatedDate,
                IdFundNavigation = e.ExpenseAndIncome.IdFundNavigation,
                IdPartyNavigation = e.ExpenseAndIncome.IdPartyNavigation,
                IdReasonNavigation = e.ExpenseAndIncome.IdReasonNavigation,
                AccountantName = e.AccountantName,
                Amount = e.ExpenseAndIncome.Amount
            }).ToList();

            return View(model);
        }

        public void getData()
        {
            ViewData["IdFund"] = new SelectList(_context.Funds.Select(f => new { IdFund = f.IdFund, Display = f.FundName }), "IdFund", "Display");
            ViewData["IdAccountant"] = new SelectList(_context.Members.Select(f => new { IdAccountant = f.IdMember, Display = f.MemberName }), "IdAccountant", "Display");
            ViewData["IdParty"] = new SelectList(_context.Members.Select(f => new { IdParty = f.IdMember, Display = f.MemberName }), "IdParty", "Display");
            ViewData["IdReason"] = new SelectList(_context.Reasons.Select(f => new { IdReason = f.IdReason, Display = f.ReasonName }), "IdReason", "Display");
            ViewData["Type"] = new List<SelectListItem> { new SelectListItem { Value = "false", Text = "Thu" }, new SelectListItem { Value = "true", Text = "Chi" } };
            ViewData["IsDone"] = new List<SelectListItem> { new SelectListItem { Value = "false", Text = "Chưa hoàn thành" }, new SelectListItem { Value = "true", Text = "Hoàn thành" } };
        }

        public void getDataSecond()
        {
            var funds = _context.Funds.Select(f => new SelectListItem { Value = f.IdFund.ToString(), Text = f.FundName }).ToList();
            funds.Insert(0, new SelectListItem { Value = "", Text = "Chọn quỹ" });
            ViewBag.IdFund = funds;

            var parties = _context.Members.Select(p => new SelectListItem { Value = p.IdMember.ToString(), Text = p.MemberName }).ToList();
            parties.Insert(0, new SelectListItem { Value = "", Text = "Chọn thành viên" });
            ViewBag.IdParty = parties;

            var reasons = _context.Reasons.Select(r => new SelectListItem { Value = r.IdReason.ToString(), Text = r.ReasonName }).ToList();
            reasons.Insert(0, new SelectListItem { Value = "", Text = "Chọn lý do" });
            ViewBag.IdReason = reasons;
        }

        [HttpGet]
        public IActionResult Search()
        {
            getDataSecond();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(DateTime? createdDate, bool? type, int? idFund, int? idParty, int? idReason)
        {
            // Tạo bộ lọc dữ liệu
            var query = _context.ExpenseAndIncomes.AsQueryable();

            if (createdDate.HasValue)
                query = query.Where(e => e.CreatedDate.HasValue && e.CreatedDate.Value.Date == createdDate.Value.Date);

            if (type.HasValue)
                query = query.Where(e => e.Type == type.Value);

            if (idFund.HasValue)
                query = query.Where(e => e.IdFund == idFund.Value);

            if (idParty.HasValue)
                query = query.Where(e => e.IdParty == idParty.Value);

            if (idReason.HasValue)
                query = query.Where(e => e.IdReason == idReason.Value);

            var results = await query
                .Include(e => e.IdFundNavigation)
                .Include(e => e.IdPartyNavigation)
                .Include(e => e.IdReasonNavigation)
                .Join(
                    _context.Members,
                    e => e.IdAccountant,
                    m => m.IdMember,
                    (e, m) => new ExpenseAndIncome
                    {
                        Id = e.Id,
                        IdFund = e.IdFund,
                        IdParty = e.IdParty,
                        IdAccountant = e.IdAccountant,
                        AccountantName = m.MemberName,
                        Type = e.Type,
                        IdReason = e.IdReason,
                        Amount = e.Amount,
                        CreatedDate = e.CreatedDate,
                        DaysOverdue = e.DaysOverdue,
                        Status = e.Status,
                        IsDone = e.IsDone,
                        IdFundNavigation = e.IdFundNavigation,
                        IdPartyNavigation = e.IdPartyNavigation,
                        IdReasonNavigation = e.IdReasonNavigation
                    }
                )
                .OrderByDescending(e => e.Id) 
                .ToListAsync();

            return View("Index", results);
        }

        // GET: ExpenseAndIncome/Create
        public IActionResult Create()
        {
            getData();
            return View();
        }

        // POST: ExpenseAndIncome/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFund,IdParty,IdAccountant,Type,IdReason,DaysOverdue,Status,IsDone,CreatedDate,Amount")] ExpenseAndIncome expenseAndIncome, bool isBulkAdd)
        {
            if (ModelState.IsValid)
            {
                var fund = await _context.Funds.FindAsync(expenseAndIncome.IdFund);

                if (fund == null)
                {
                    ModelState.AddModelError(string.Empty, "Quỹ được chọn không tồn tại!");
                    getData();
                    return View(expenseAndIncome);
                }

                if (expenseAndIncome.Type == true)
                {
                    if(fund.Total.HasValue && fund.Total.Value >= expenseAndIncome.Amount)
                    {
                        fund.Total -= expenseAndIncome.Amount;
                       
                        var record = new ExpenseAndIncome
                        {
                            IdFund = expenseAndIncome.IdFund,
                            IdAccountant = expenseAndIncome.IdAccountant,
                            Type = expenseAndIncome.Type,
                            IdReason = expenseAndIncome.IdReason,
                            CreatedDate = expenseAndIncome.CreatedDate,
                            IdParty = null,
                            IsDone = true,
                            DaysOverdue = 0,
                            Status = false,
                            Amount = expenseAndIncome.Amount
                        };

                        _context.Add(record);
                        _context.Update(fund);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Số tiền chi vượt quá số tiền trong quỹ!");
                        getData();
                        return View(expenseAndIncome);
                    }
                }
                else if (expenseAndIncome.Type == false) 
                {
                    var existingRecord = await _context.ExpenseAndIncomes
                        .FirstOrDefaultAsync(e => e.IdFund == expenseAndIncome.IdFund
                            && e.Type == false  
                            && e.IdReason == expenseAndIncome.IdReason
                            && e.CreatedDate.HasValue
                            && e.CreatedDate.Value.Month == expenseAndIncome.CreatedDate.Value.Month
                            && e.CreatedDate.Value.Year == expenseAndIncome.CreatedDate.Value.Year);

                    if (existingRecord != null)
                    {
                        var reasonName = await _context.Reasons
                            .Where(r => r.IdReason == expenseAndIncome.IdReason)
                            .Select(r => r.ReasonName)
                            .FirstOrDefaultAsync();

                        ModelState.AddModelError(string.Empty, $"Hoạt động với lý do \"{reasonName}\" trong tháng {expenseAndIncome.CreatedDate:MM/yyyy} đã được tạo!");
                        getData();
                        return View(expenseAndIncome);
                    }

                    if (isBulkAdd)
                    {
                        var members = await _context.Members.Select(m => m.IdMember).ToListAsync();

                        foreach (var memberId in members)
                        {
                            var newRecord = new ExpenseAndIncome
                            {
                                IdFund = expenseAndIncome.IdFund,
                                IdAccountant = expenseAndIncome.IdAccountant,
                                Type = expenseAndIncome.Type,
                                IdReason = expenseAndIncome.IdReason,
                                CreatedDate = expenseAndIncome.CreatedDate,
                                IdParty = memberId,
                                IsDone = false,
                                DaysOverdue = 0,
                                Status = false,
                                Amount = expenseAndIncome.Amount
                            };

                            _context.Add(newRecord);
                        }
                    }
                    else
                    {
                        var newRecord = new ExpenseAndIncome
                        {
                            IdFund = expenseAndIncome.IdFund,
                            IdAccountant = expenseAndIncome.IdAccountant,
                            Type = expenseAndIncome.Type,
                            IdReason = expenseAndIncome.IdReason,
                            CreatedDate = expenseAndIncome.CreatedDate,
                            IdParty = expenseAndIncome.IdParty,
                            IsDone = false,
                            DaysOverdue = 0,
                            Status = false,
                            Amount = expenseAndIncome.Amount
                        };

                        _context.Add(newRecord);
                    }
                }
                _context.Update(fund);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            getData();
            return View(expenseAndIncome);
        }

        [HttpGet]
        public JsonResult GetReasonsByType(bool type)
        {
            var reasons = _context.Reasons
                .Where(r => r.Type == type) // Lọc lý do theo loại
                .Select(r => new { idReason = r.IdReason, reasonName = r.ReasonName })
                .ToList();

            return Json(reasons);
        }

        // GET: ExpenseAndIncome/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseAndIncome = await _context.ExpenseAndIncomes.FindAsync(id);
            if (expenseAndIncome == null)
            {
                return NotFound();
            }
            getData();
            return View(expenseAndIncome);
        }

        // POST: ExpenseAndIncome/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdFund,IdParty,IdAccountant,Type,IdReason,DaysOverdue,Status,IsDone,CreatedDate,Amount")] ExpenseAndIncome expenseAndIncome)
        {
            if (id != expenseAndIncome.Id)
            {
                return NotFound();
            }    
            
            if (ModelState.IsValid)
            {
                try
                {
                    var existingRecord = await _context.ExpenseAndIncomes
                        .Include(e => e.IdFundNavigation)
                        .Include(e => e.IdPartyNavigation)
                        .FirstOrDefaultAsync(e => e.Id == expenseAndIncome.Id);

                    if (existingRecord == null)
                        return NotFound();

                    decimal oldAmount = existingRecord.Amount;
                    decimal newAmount = expenseAndIncome.Amount;
                    decimal amountDifference = newAmount - oldAmount;

                    if(existingRecord.IdFund != expenseAndIncome.IdFund)
                    {
                        var oldFund = existingRecord.IdFundNavigation;
                        var newFund = await _context.Funds.FirstOrDefaultAsync(f => f.IdFund == expenseAndIncome.IdFund);

                        if(newFund == null)
                        {
                            ModelState.AddModelError(string.Empty, "Quỹ mới không tồn tại.");
                            getData();
                            return View(expenseAndIncome);
                        }

                        // Hoàn nguyên số tiền trong quỹ cũ
                        if (existingRecord.Type == true) 
                            oldFund.Total += existingRecord.Amount; // Hoàn lại số tiền đã chi

                        else if (existingRecord.Type == false) 
                            oldFund.Total -= existingRecord.Amount; // Trừ số tiền đã thu

                        // Áp dụng số tiền vào quỹ mới
                        if (existingRecord.Type == true) 
                        {
                            if (newFund.Total < newAmount)
                            {
                                ModelState.AddModelError(string.Empty, "Số dư quỹ mới không đủ để chi!");
                                getData();
                                return View(expenseAndIncome);
                            }
                            newFund.Total -= newAmount; // Trừ số tiền chi vào quỹ mới
                        }
                        else if (existingRecord.Type == false) 
                        {
                            newFund.Total += newAmount; // Cộng số tiền thu vào quỹ mới
                        }

                        // Gán quỹ mới cho giao dịch
                        existingRecord.IdFundNavigation = newFund;
                    }
                    else
                    {
                        // Nếu không thay đổi quỹ, xử lý logic chi/thu như bình thường
                        if (existingRecord.Type == true)
                        {
                            if (amountDifference > 0)
                            {
                                if (existingRecord.IdFundNavigation.Total < amountDifference)
                                {
                                    ModelState.AddModelError(string.Empty, "Số dư quỹ không đủ để chi!");
                                    getData();
                                    return View(expenseAndIncome);
                                }
                                existingRecord.IdFundNavigation.Total -= amountDifference;
                            }
                            else if (amountDifference < 0)
                            {
                                existingRecord.IdFundNavigation.Total += Math.Abs(amountDifference);
                            }
                        }
                    }

                    existingRecord.Amount = newAmount;
                    existingRecord.IdFund = expenseAndIncome.IdFund;
                    existingRecord.IdReason = expenseAndIncome.IdReason;
                    existingRecord.CreatedDate = expenseAndIncome.CreatedDate;
                    existingRecord.Status = expenseAndIncome.Status;

                    if (existingRecord.Type == false)
                    {
                        existingRecord.IsDone = expenseAndIncome.IsDone;
                        existingRecord.IdFundNavigation.Total += existingRecord.Amount;
                    }    
                        
                    _context.Update(existingRecord);
                    _context.Update(existingRecord.IdFundNavigation); 
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseAndIncomeExists(expenseAndIncome.Id))
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
            return View(expenseAndIncome);
        }

        // GET: ExpenseAndIncome/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseAndIncome = await _context.ExpenseAndIncomes
                .Include(e => e.IdFundNavigation)
                .Include(e => e.IdPartyNavigation)
                .Include(e => e.IdReasonNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseAndIncome == null)
            {
                return NotFound();
            }

            getData();
            return View(expenseAndIncome);
        }

        // POST: ExpenseAndIncome/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenseAndIncome = await _context.ExpenseAndIncomes.FindAsync(id);
            if (expenseAndIncome != null)
            {
                expenseAndIncome.Status = true;
                _context.Update(expenseAndIncome);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseAndIncomeExists(int id)
        {
            return _context.ExpenseAndIncomes.Any(e => e.Id == id);
        }
    }
}
