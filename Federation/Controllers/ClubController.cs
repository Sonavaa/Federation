using Federation.ViewModels;
using FederationTask.Data;
using FederationTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FederationTask.Controllers
{
    public class ClubController : Controller
    {
        private AppDbContext _context;

        public ClubController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ClubPage(int pageNumber = 1)
        {
            int pageSize = 5;

            var clubs = await _context.Clubs
                .Where(c => !c.isDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalClubs = await _context.Clubs.CountAsync();
            var totalPages = (int)Math.Ceiling(totalClubs / (double)pageSize);

            var pagination = new PaginationVM
            {
                Clubs = clubs,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };

            return View(pagination);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(club club)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "The blank must be filled!");
                return View(club);
            }

            club newClub = new club
            {
                NameOfClubCreator = club.NameOfClubCreator,
                PhoneNumberOfClubCreator = club.PhoneNumberOfClubCreator,
                ClubCreatorsEmail = club.ClubCreatorsEmail,
                Region = club.Region,
                ClubName = club.ClubName,
                Director = club.Director,
                DirectorsEmail = club.DirectorsEmail,
                Vöen = club.Vöen,
                HallName = club.HallName,
                TeamCategory = club.TeamCategory,
                Gender = club.Gender
            };

            await _context.Clubs.AddAsync(newClub);
            await _context.SaveChangesAsync();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            club? clubs = await _context.Clubs.FirstOrDefaultAsync(x => x.Id == id);
            if (clubs == null)
            {
                return NotFound();
            }

            clubs.isDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ClubPage));
        }

    }
}
