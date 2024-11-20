using Federation.ViewModels;
using FederationTask.Data;
using FederationTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FederationTask.Controllers
{
    public class TeamController : Controller
    {
        private AppDbContext _context;

        public TeamController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> TeamPage(int pageNumber = 1)
        {
            int pageSize = 5;

            var teams = await _context.Teams
                .Include(x => x.Club)
                .Where(c => !c.isDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalClubs = await _context.Teams.Where(c => !c.isDeleted).CountAsync();
            var totalPages = (int)Math.Ceiling(totalClubs / (double)pageSize);

            var pagination = new PaginationVM
            {
                Teams = teams,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };

            return View(pagination);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Clubs = await _context.Clubs.Where(c => !c.isDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(team team)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "The blank must be filled!");
                return View(team);
            }

            string patternOfNameAndSurname = @"^[a-zA-Z]+ [a-zA-Z]+$";

            if (!Regex.IsMatch(team.Name, patternOfNameAndSurname))
            {
                ModelState.AddModelError("Name", "Komandanın adı və soyadı mütləqdir.");
                return View(team);
            }

            string patternOfPhoneNumber = @"^(?:\+994|0)(50|51|55|70|77)[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$";

            if (!Regex.IsMatch(team.PhoneNumber, patternOfPhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Daxil etdiyiniz nömrə düzgün deyil.");
                return View(team);
            }


            team newTeam = new team
            {
               Name = team.Name.ToUpper().Trim(),
               Email = team.Email.ToUpper().Trim(),
               PhoneNumber = team.PhoneNumber,
               ClubId = team.ClubId,
               TeamCategory = team.TeamCategory,
               Gender = team.Gender,
               Region = team.Region
            };

            await _context.Teams.AddAsync(newTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction("TeamPage");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            team? team = await _context.Teams.Include(t => t.Club).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (team == null)
            {
                return NotFound();
            }
            var clubs = _context.Clubs
             .Where(c => !c.isDeleted)
             .ToList();

            ViewBag.Clubs = clubs;

            return View(team);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, team team)
        {
            if (team == null) return NotFound();

            if (id != team.Id) return BadRequest();

            var existTeam = await _context.Teams.Include(t => t.Club).FirstOrDefaultAsync(x => !x.isDeleted && x.Id == id);

            if (existTeam == null) return NotFound();

            string patternOfNameAndSurname = @"^[a-zA-Z]+ [a-zA-Z]+$";
            if (!Regex.IsMatch(team.Name, patternOfNameAndSurname))
            {
                ModelState.AddModelError("Name", "Komandanın adı və soyadı mütləqdir.");
                return View(team);
            }

            string patternOfPhoneNumber = @"^(?:\+994|0)(50|51|55|70|77)[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$";
            if (!Regex.IsMatch(team.PhoneNumber, patternOfPhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Daxil etdiyiniz nömrə düzgün deyil.");
                return View(team);
            }

            bool isDuplicateName = await _context.Teams
                .AnyAsync(t => !t.isDeleted && t.Name.Trim().ToLower() == team.Name.Trim().ToLower() && t.Id != id);

            if (isDuplicateName)
            {
                ModelState.AddModelError("Name", $"'{team.Name}' adlı komanda artıq mövcuddur.");
                return View(team);
            }

            existTeam.Name = team.Name.ToUpper().Trim();
            existTeam.Email = team.Email.ToUpper().Trim();
            existTeam.PhoneNumber = team.PhoneNumber;
            existTeam.TeamCategory = team.TeamCategory;
            existTeam.Gender = team.Gender;
            existTeam.Region = team.Region;

            await _context.SaveChangesAsync();
            return RedirectToAction("TeamPage");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            team? team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            team.isDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TeamPage));
        }

    }
}
