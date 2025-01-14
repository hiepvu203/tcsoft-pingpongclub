using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;

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
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var thuctapKtktcn2024Context = await _context.ExpenseAndIncomes
                .Include(e => e.IdFundNavigation)
                .Include(e => e.IdPartyNavigation)
                .Include(e => e.IdReasonNavigation)
                .Include(e => e.IdTournamentNavigation)
                .Include(e => e.IdSponorDetailNavigation)
                //.ThenInclude(sd => sd.IdSponorNavigation)
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
                IdTournament = e.ExpenseAndIncome.IdTournament,
                IdSponorDetail = e.ExpenseAndIncome.IdSponorDetail,
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
                IdTournamentNavigation = e.ExpenseAndIncome.IdTournamentNavigation,
                IdSponorDetailNavigation = e.ExpenseAndIncome.IdSponorDetailNavigation,
                AccountantName = e.AccountantName,
                Amount = e.ExpenseAndIncome.Amount,
                //SponorName = e.ExpenseAndIncome.IdSponorDetailNavigation?.IdSponorNavigation?.NameSponer
            }).ToList();

            var paginatedList = model.ToPagedList(pageNumber, pageSize);
            return View(paginatedList);
        }

        public void getData()
        {
            ViewData["IdSponorDetail"] = new SelectList(_context.Sponors.Select(f => new { IdSponorDetail = f.IdSponorTour, Display = f.IdSponorNavigation.NameSponer + " - " + f.IdTournamentNavigation.TournamentName + " - " + (f.CreatedDate.HasValue ? f.CreatedDate.Value.ToString("dd/MM/yyyy") : "Không xác định") }), "IdSponorDetail", "Display");
            ViewData["IdTournament"] = new SelectList(_context.Tournaments.Select(f => new { IdTournament = f.IdTournament, Display = f.TournamentName }), "IdTournament", "Display");
            ViewData["IdFund"] = new SelectList(_context.Funds.Select(f => new { IdFund = f.IdFund, Display = f.FundName }), "IdFund", "Display");
            ViewData["IdAccountant"] = new SelectList(_context.Members.Select(f => new { IdAccountant = f.IdMember, Display = f.MemberName }), "IdAccountant", "Display");
            ViewData["IdParty"] = new SelectList(_context.Members.Select(f => new { IdParty = f.IdMember, Display = f.MemberName + " - " + f.Phone }), "IdParty", "Display");
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

            var tournaments = _context.Tournaments.Select(p => new SelectListItem { Value = p.IdTournament.ToString(), Text = p.TournamentName }).ToList();
            tournaments.Insert(0, new SelectListItem { Value = "", Text = "Chọn giải đấu" });
            ViewBag.IdTournament = tournaments;

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
        public async Task<IActionResult> Search(DateTime? createdDate, bool? type, int? idFund, int? idParty, int? idReason, int?idTournament, int page = 1)
        {
            int pageSize = 10; 
            int skip = (page - 1) * pageSize; 

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

            if (idTournament.HasValue)
                query = query.Where(e => e.IdTournament == idTournament.Value);

            // Tổng số bản ghi
            int totalRecords = await query.CountAsync();

            // Lấy dữ liệu theo trang
            var results = await query
                .Include(e => e.IdFundNavigation)
                .Include(e => e.IdPartyNavigation)
                .Include(e => e.IdReasonNavigation)
                .Include(e => e.IdTournamentNavigation)
                .Join(
                    _context.Members,
                    e => e.IdAccountant,
                    m => m.IdMember,
                    (e, m) => new ExpenseAndIncome
                    {
                        Id = e.Id,
                        IdFund = e.IdFund,
                        IdParty = e.IdParty,
                        IdTournament = e.IdTournament,
                        IdSponorDetail = e.IdSponorDetail,
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
                        IdReasonNavigation = e.IdReasonNavigation,
                        IdTournamentNavigation = e.IdTournamentNavigation,
                        IdSponorDetailNavigation = e.IdSponorDetailNavigation
                    }
                )
                .OrderByDescending(e => e.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            // Chuyển dữ liệu thành PagedList
            var pagedResults = new StaticPagedList<ExpenseAndIncome>(results, page, pageSize, totalRecords);
            return View("Index", pagedResults);
        }


        private async Task<List<StatisticsViewModel>> GetStatisticsAsync(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Today.AddMonths(-1);
            if (!endDate.HasValue)
                endDate = DateTime.Today;

            return await _context.ExpenseAndIncomes
                .Include(e => e.IdFundNavigation)
                .Where(e => e.CreatedDate.HasValue && e.IsDone == true)
                .GroupBy(e => new { e.IdFundNavigation.FundName, e.IdFundNavigation.Total })
                .Select(g => new StatisticsViewModel
                {
                    FundName = g.Key.FundName,
                    OpeningBalance = g.Key.Total +
                                     g.Where(e => e.CreatedDate >= startDate && e.CreatedDate <= endDate)
                                      .Sum(e => e.Type == true ? e.Amount : 0)
                                     - g.Where(e => e.CreatedDate >= startDate && e.CreatedDate <= endDate)
                                      .Sum(e => e.Type == false ? e.Amount : 0),
                    TotalIncome = g.Where(e => e.CreatedDate >= startDate && e.CreatedDate <= endDate && e.Type == false)
                                   .Sum(e => e.Amount),
                    TotalExpense = g.Where(e => e.CreatedDate >= startDate && e.CreatedDate <= endDate && e.Type == true)
                                   .Sum(e => e.Amount),
                })
                .Select(s => new StatisticsViewModel
                {
                    FundName = s.FundName,
                    OpeningBalance = s.OpeningBalance,
                    TotalIncome = s.TotalIncome,
                    TotalExpense = s.TotalExpense,
                    ClosingBalance = s.OpeningBalance + s.TotalIncome - s.TotalExpense
                })
                .ToListAsync();
        }

        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate)
        {
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd") ?? DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd");

            var statistics = await GetStatisticsAsync(startDate, endDate);
            return View(statistics);
        }

        public async Task<IActionResult> ExportToExcel(DateTime? startDate, DateTime? endDate)
        {
            // Lấy dữ liệu
            var statistics = await GetStatisticsAsync(startDate, endDate);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Statistics");

                // Thêm tiêu đề ngày bắt đầu và ngày kết thúc
                worksheet.Cells[1, 2].Value = "Thống kê từ ngày:";
                worksheet.Cells[1, 3].Value = startDate?.ToString("dd/MM/yyyy") ?? DateTime.Today.AddMonths(-1).ToString("dd/MM/yyyy");
                worksheet.Cells[2, 2].Value = "Đến ngày:";
                worksheet.Cells[2, 3].Value = endDate?.ToString("dd/MM/yyyy") ?? DateTime.Today.ToString("dd/MM/yyyy");

                // Header
                worksheet.Cells[4, 1].Value = "Quỹ";
                worksheet.Cells[4, 2].Value = "Số dư đầu kỳ";
                worksheet.Cells[4, 3].Value = "Tổng thu";
                worksheet.Cells[4, 4].Value = "Tổng chi";
                worksheet.Cells[4, 5].Value = "Số dư cuối kỳ";

                using (var range = worksheet.Cells[4, 1, 4, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }

                // Data rows
                int row = 5;
                foreach (var stat in statistics)
                {
                    worksheet.Cells[row, 1].Value = stat.FundName;
                    worksheet.Cells[row, 2].Value = stat.OpeningBalance;
                    worksheet.Cells[row, 3].Value = stat.TotalIncome;
                    worksheet.Cells[row, 4].Value = stat.TotalExpense;
                    worksheet.Cells[row, 5].Value = stat.ClosingBalance;
                    row++;
                }

                // Tổng cộng
                worksheet.Cells[row, 1].Value = "Tổng cộng";
                worksheet.Cells[row, 2].Formula = $"SUM(B5:B{row - 1})";
                worksheet.Cells[row, 3].Formula = $"SUM(C5:C{row - 1})";
                worksheet.Cells[row, 4].Formula = $"SUM(D5:D{row - 1})";
                worksheet.Cells[row, 5].Formula = $"SUM(E5:E{row - 1})";
                worksheet.Cells[row, 1, row, 5].Style.Font.Bold = true;

                // Tạo Table
                var tableRange = worksheet.Cells[4, 1, row, 5];
                var table = worksheet.Tables.Add(tableRange, "StatisticsTable");
                table.ShowHeader = true;
                table.TableStyle = OfficeOpenXml.Table.TableStyles.Light21;

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Lưu file Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"ThongKe_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


        public IActionResult Create()
        {
            getData();
            return View();
        }

        // POST: ExpenseAndIncome/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFund,IdParty,IdAccountant,IdTournament,IdSponorDetail,Type,IdReason,DaysOverdue,Status,IsDone,CreatedDate,Amount")] ExpenseAndIncome expenseAndIncome)
        {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(expenseAndIncome.IdTournament?.ToString()))
                        expenseAndIncome.IdTournament = null;
                    
                    if (string.IsNullOrEmpty(expenseAndIncome.IdSponorDetail?.ToString()))
                        expenseAndIncome.IdSponorDetail = null;

                    var fund = await _context.Funds.FindAsync(expenseAndIncome.IdFund);

                    if (expenseAndIncome.Type == true)
                    {
                        if (fund.Total >= expenseAndIncome.Amount)
                        {
                            fund.Total -= expenseAndIncome.Amount;

                            var record = new ExpenseAndIncome
                            {
                                IdFund = expenseAndIncome.IdFund,
                                IdAccountant = expenseAndIncome.IdAccountant,
                                Type = expenseAndIncome.Type,
                                IdReason = expenseAndIncome.IdReason,
                                CreatedDate = expenseAndIncome.CreatedDate,
                                IdTournament = expenseAndIncome.IdTournament,
                                IdSponorDetail = expenseAndIncome.IdSponorDetail,
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
                        var reason = await _context.Reasons.FirstOrDefaultAsync(r => r.IdReason == expenseAndIncome.IdReason);

                        if (reason.recurringFee == true)
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
                                    IdTournament = expenseAndIncome.IdTournament,
                                    IdSponorDetail = expenseAndIncome.IdSponorDetail,
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
                            fund.Total += expenseAndIncome.Amount;
                            var newRecord = new ExpenseAndIncome
                            {
                                IdFund = expenseAndIncome.IdFund,
                                IdAccountant = expenseAndIncome.IdAccountant,
                                Type = expenseAndIncome.Type,
                                IdReason = expenseAndIncome.IdReason,
                                CreatedDate = expenseAndIncome.CreatedDate,
                                IdParty = expenseAndIncome.IdParty,
                                IdTournament = expenseAndIncome.IdTournament,
                                IdSponorDetail = expenseAndIncome.IdSponorDetail,
                                IsDone = true,
                                DaysOverdue = 0,
                                Status = false,
                                Amount = expenseAndIncome.Amount
                            };

                            _context.Add(newRecord);
                            _context.Update(fund);
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
                .Where(r => r.Type == type) 
                .Select(r => new { idReason = r.IdReason, reasonName = r.ReasonName, recurringFee = r.recurringFee })
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdFund,IdParty,IdAccountant,IdTournament,IdSponorDetail,Type,IdReason,DaysOverdue,Status,IsDone,CreatedDate,Amount")] ExpenseAndIncome expenseAndIncome)
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
                        .Include(e => e.IdTournamentNavigation)
                        .Include(e => e.IdSponorDetailNavigation)
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
                            newFund.Total -= newAmount; 
                        }
                        else if (existingRecord.Type == false) 
                        {
                            newFund.Total += newAmount; 
                        }

                        existingRecord.IdFundNavigation = newFund;
                    }
                    else
                    {
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
                    existingRecord.IdTournament = expenseAndIncome.IdTournament;
                    existingRecord.IdSponorDetail = expenseAndIncome.IdSponorDetail;

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
                .Include(e => e.IdTournamentNavigation)
                .Include(e => e.IdSponorDetailNavigation)
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
