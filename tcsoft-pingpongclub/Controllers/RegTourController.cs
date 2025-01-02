using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tcsoft_pingpongclub.Controllers
{
    public class RegTourController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;
        private dynamic searchTerm;

        public RegTourController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: Tournaments
        public async Task<IActionResult> Index(string searchTerm, int? selectedLevel, int page = 1)
        {
            int pageSize = 8; // Số giải đấu tối đa mỗi trang
            var tournamentsQuery = _context.Tournaments.AsQueryable();

            // Lọc theo từ khóa tìm kiếm không phân biệt chữ hoa chữ thường
            if (!string.IsNullOrEmpty(searchTerm))
            {
                tournamentsQuery = tournamentsQuery.Where(t => t.TournamentName.ToLower().Contains(searchTerm.ToLower()));
            }


            // Lọc theo cấp bậc (Rank)
            if (selectedLevel.HasValue)
            {
                tournamentsQuery = tournamentsQuery.Where(t => t.RankStart == selectedLevel || t.RankEnd == selectedLevel);
            }

            // Lấy danh sách cấp bậc để hiển thị trong dropdown
            ViewBag.Levels = await _context.Levels.ToListAsync();
            ViewBag.SelectedLevel = selectedLevel; // Không cần chuyển đổi kiểu
            ViewBag.SearchTerm = searchTerm;

            var totalTournaments = await tournamentsQuery.CountAsync();

            // Tính số trang
            int totalPages = (int)Math.Ceiling((double)totalTournaments / pageSize);

            // Lấy các giải đấu cho trang hiện tại
            var tournaments = await tournamentsQuery
                .Include(t => t.RankEndNavigation)
                .Include(t => t.RankStartNavigation)
                .OrderByDescending(t => t.IdTournament)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Truyền dữ liệu tới View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(tournaments);
        }


        // GET: Tournaments/MatchSchedule/{idTournament}
        // lịch thi đấu
        public async Task<IActionResult> MatchSchedule(int idTournament)
        {
            // Lấy ID thành viên từ Session
            int idMember = HttpContext.Session.GetInt32("IdMember") ?? 0;

            // Kiểm tra xem người dùng đã đăng ký giải đấu chưa
            var player = await _context.Players
                .Include(p => p.IdTournamentNavigation) // Bao gồm thông tin giải đấu
                .FirstOrDefaultAsync(p => p.IdTournament == idTournament && p.IdMember == idMember);

            if (player == null)
            {
                // Nếu chưa đăng ký giải đấu, hiển thị thông báo
                ViewBag.Message = "Bạn chưa đăng ký giải đấu này. Vui lòng đăng ký trước khi xem lịch thi đấu.";
                return View();  // Không cần truyền dữ liệu trận đấu nếu chưa đăng ký
            }

            // Lấy danh sách trận đấu liên quan đến giải đấu
            var matches = await _context.Matches
                .Include(m => m.IdMemberOneNavigation)  // Bao gồm thông tin thành viên 1
                .ThenInclude(m => m.IdMemberNavigation) // Thông tin chi tiết người chơi 1
                .Include(m => m.IdMemberTwoNavigation)  // Bao gồm thông tin thành viên 2
                .ThenInclude(m => m.IdMemberNavigation) // Thông tin chi tiết người chơi 2
                .Include(m => m.IdTournamentNavigation)  // Bao gồm thông tin giải đấu
                .Where(m => m.IdTournament == idTournament &&
                            (m.IdMemberOne == player.IdPlayer || m.IdMemberTwo == player.IdPlayer))
                .OrderBy(m => m.TimeStart)
                .ToListAsync();

            // Kiểm tra dữ liệu matches có null hoặc rỗng không
            if (matches == null)
            {
                matches = new List<Match>();  // Gán giá trị rỗng nếu null
            }

            // Truyền dữ liệu tới View
            ViewBag.TournamentName = player.IdTournamentNavigation?.TournamentName;
            ViewBag.IdPlayer = player.IdPlayer;

            return View(matches);  // Truyền danh sách trận đấu
        }


        [HttpPost]
        public IActionResult ChangeStatus(int id, bool status)
        {
            var member = _context.Members.Find(id);
            if (member != null)
            {
                member.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: Tournaments/RegisteredPlayers/{idTournament}
        // người đã đăng kí
        public async Task<IActionResult> RegisteredPlayers(int idTournament)
        {
            var players = await _context.Players
           .Include(p => p.IdMemberNavigation) // Bao gồm thông tin Member
           .Where(p => p.IdTournament == idTournament)
           .ToListAsync();

            return View(players);

        }

        [HttpPost]
        public IActionResult Register(int idTournament)
        {
            int idMember = HttpContext.Session.GetInt32("IdMember") ?? 0;


            // Kiểm tra nếu Player đã tồn tại trong Tournament
            var existingPlayer = _context.Players
                .FirstOrDefault(p => p.IdMember == idMember && p.IdTournament == idTournament);

            if (existingPlayer != null)
            {
                TempData["Error"] = "Member này đã đăng ký giải đấu trước đó!";
                return RedirectToAction("Details", "RegTour", new { id = idTournament });
            }

            // Kiểm tra giải đấu
            var tournament = _context.Tournaments.Find(idTournament);
            if (tournament == null)
            {
                TempData["Error"] = "Giải đấu không tồn tại. Vui lòng kiểm tra lại.";
                return RedirectToAction("Index");
            }

            // Tạo mới Player
            var player = new Player
            {
                IdTournament = idTournament,
                IdMember = idMember,
                Score = 0,
                Status = true
            };

            try
            {
                _context.Players.Add(player);
                _context.SaveChanges();
                TempData["Registered"] = true;
                TempData["Success"] = "Đăng ký thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Đăng ký thất bại: {ex.Message}";
            }

            // Chuyển hướng về trang Details của Tournament
            return RedirectToAction("Details", "RegTour", new { id = idTournament });
        }
        private void GenerateMatches(int idTournament)
        {
            // Lấy danh sách các người chơi đã đăng ký trong giải đấu
            var players = _context.Players
                .Where(p => p.IdTournament == idTournament)
                .ToList();

            if (players.Count < 2)
            {
                // Không đủ người chơi để tạo lịch thi đấu
                return;
            }

            // Kiểm tra loại giải đấu (vòng tròn hay loại trực tiếp)
            var tournament = _context.Tournaments.Find(idTournament);

            if (tournament.Type == true) // Đấu vòng tròn
            {
                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = i + 1; j < players.Count; j++)
                    {
                        var match = new Match
                        {
                            IdTournament = idTournament,
                            IdMemberOne = players[i].IdPlayer,
                            IdMemberTwo = players[j].IdPlayer,
                            TimeStart = DateTime.Now.AddDays(i + j), // Tùy chỉnh thời gian bắt đầu
                            Status = false
                        };

                        _context.Matches.Add(match);
                    }
                }
            }
            else // Đấu loại trực tiếp
            {
                // Tạo cặp đấu ngẫu nhiên (hoặc theo thứ tự)
                for (int i = 0; i < players.Count; i += 2)
                {
                    if (i + 1 < players.Count)
                    {
                        var match = new Match
                        {
                            IdTournament = idTournament,
                            IdMemberOne = players[i].IdPlayer,
                            IdMemberTwo = players[i + 1].IdPlayer,
                            TimeStart = DateTime.Now.AddDays(i), // Tùy chỉnh thời gian bắt đầu
                            Status = false
                        };

                        _context.Matches.Add(match);
                    }
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();
        }

        [HttpPost]
        // hủy đăng kí
        public IActionResult CancelRegistration(int idTournament)
        {
            int idMember = HttpContext.Session.GetInt32("IdMember") ?? 0;

            // Tìm Player gần nhất trong CSDL cho giải đấu này
            var player = _context.Players
                .Where(p => p.IdMember == idMember && p.IdTournament == idTournament)
                .OrderByDescending(p => p.IdMember) // Giả sử Id là khóa chính và tăng dần
                .FirstOrDefault();

            if (player != null)
            {
                _context.Players.Remove(player);
                _context.SaveChanges();
                TempData["Success"] = "Hủy đăng ký thành công!";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy đăng ký để hủy.";
            }

            // Chuyển hướng về trang Details của Tournament
            return RedirectToAction("Details", new { id = idTournament });
        }


        // GET: Tournaments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .Include(t => t.RankEndNavigation)
                .Include(t => t.RankStartNavigation)
                .FirstOrDefaultAsync(m => m.IdTournament == id);
            if (tournament == null)
            {
                return NotFound();
            }
            // Truyền searchTerm cho View để giữ giá trị tìm kiếm
         

            ViewBag.SearchTerm = searchTerm;
            return View(tournament);
        }

        private bool TournamentExists(int id)
        {
            return _context.Tournaments.Any(e => e.IdTournament == id);
        }
    }
}
