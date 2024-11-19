using Federation.ViewModels;
using FederationTask.Data;
using FederationTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

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

            var totalClubs = await _context.Clubs.Where(c => !c.isDeleted).CountAsync();
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

            string patternOfNameAndSurname = @"^[a-zA-Z]+ [a-zA-Z]+$";

            if (string.IsNullOrWhiteSpace(club.NameOfClubCreator))
            {
                ModelState.AddModelError("NameOfClubCreator", "Xana doldurulmalıdır.");
                return View(club);
            }

            if (!Regex.IsMatch(club.NameOfClubCreator, patternOfNameAndSurname))
            {
                ModelState.AddModelError("NameOfClubCreator", "Məsul şəxsin adı və soyadı mütləqdir.");
                return View(club);
            }

            if (!Regex.IsMatch(club.Director, patternOfNameAndSurname))
            {
                ModelState.AddModelError("Director", "Direktorun adı və soyadı mütləqdir.");
                return View(club);
            }

            string patternOfPhoneNumber = @"^(?:\+994|0)(50|51|55|70|77)[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$";

            if (!Regex.IsMatch(club.PhoneNumberOfClubCreator, patternOfPhoneNumber))
            {
                ModelState.AddModelError("PhoneNumberOfClubCreator", "Daxil etdiyiniz nömrə düzgün deyil.");
                return View(club);
            }

            string patternOfVoen = @"^\d{9,10}$";

            if (!Regex.IsMatch(club.Vöen, patternOfVoen))
            {
                ModelState.AddModelError("Vöen", "VÖEN kodu 9 yaxud 10 mərtəbəli ədəddən ibarət olmalıdır.");
                return View(club);
            }


            club newClub = new club
            {
                NameOfClubCreator = club.NameOfClubCreator.ToUpper().Trim(),
                PhoneNumberOfClubCreator = club.PhoneNumberOfClubCreator,
                ClubCreatorsEmail = club.ClubCreatorsEmail.ToUpper().Trim(),
                Region = club.Region,
                ClubName = club.ClubName.ToUpper().Trim(),
                Director = club.Director.ToUpper().Trim(),
                DirectorsEmail = club.DirectorsEmail.ToUpper().Trim(),
                Vöen = club.Vöen,
                HallName = club.HallName,
                TeamCategory = club.TeamCategory,
                Gender = club.Gender
            };

            await _context.Clubs.AddAsync(newClub);
            await _context.SaveChangesAsync();
            return RedirectToAction("Clubpage");

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            club? club = await _context.Clubs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, club club)
        {

            if (club == null) return NotFound();

            if (id != club.Id) return BadRequest();


            club? existClub = await _context.Clubs.FirstOrDefaultAsync(x => !x.isDeleted && x.Id == id);

            if (existClub == null) return NotFound();

            string patternOfNameAndSurname = @"^[a-zA-Z]+ [a-zA-Z]+$";

            if (string.IsNullOrWhiteSpace(club.NameOfClubCreator))
            {
                ModelState.AddModelError("NameOfClubCreator", "Xana doldurulmalıdır.");
                return View(club);
            }

            if (!Regex.IsMatch(club.NameOfClubCreator, patternOfNameAndSurname))
            {
                ModelState.AddModelError("NameOfClubCreator", "Məsul şəxsin adı və soyadı mütləqdir.");
                return View(club);
            }

            if (!Regex.IsMatch(club.Director, patternOfNameAndSurname))
            {
                ModelState.AddModelError("Director", "Direktorun adı və soyadı mütləqdir.");
                return View(club);
            }

            string patternOfPhoneNumber = @"^(?:\+994|0)(50|51|55|70|77)[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$";

            if (!Regex.IsMatch(club.PhoneNumberOfClubCreator, patternOfPhoneNumber))
            {
                ModelState.AddModelError("PhoneNumberOfClubCreator", "Daxil etdiyiniz nömrə düzgün deyil.");
                return View(club);
            }

            string patternOfVoen = @"^\d{9,10}$";

            if (!Regex.IsMatch(club.Vöen, patternOfVoen))
            {
                ModelState.AddModelError("Vöen", "VÖEN kodu 9 yaxud 10 mərtəbəli ədəddən ibarət olmalıdır.");
                return View(club);
            }


            bool isDuplicateName = await _context.Clubs
            .AnyAsync(c => !c.isDeleted
                   && c.ClubName.Trim().ToLower() == club.ClubName.Trim().ToLower()
                   && c.Id != id);

            if (isDuplicateName)
            {
                ModelState.AddModelError("ClubName", $"'{club.ClubName}' adlı klub artıq mövcuddur.");
                return View(club);
            }

            existClub.NameOfClubCreator = club.NameOfClubCreator.ToUpper().Trim();
            existClub.PhoneNumberOfClubCreator = club.PhoneNumberOfClubCreator;
            existClub.ClubCreatorsEmail = club.ClubCreatorsEmail.ToUpper().Trim();
            existClub.Region = club.Region;
            existClub.ClubName = club.ClubName.ToUpper().Trim();
            existClub.Director = club.Director.ToUpper().Trim();
            existClub.DirectorsEmail = club.DirectorsEmail.ToUpper().Trim();
            existClub.Vöen = club.Vöen;
            existClub.HallName = club.HallName;
            existClub.TeamCategory = club.TeamCategory;
            existClub.Gender = club.Gender;


            await _context.SaveChangesAsync();
            return RedirectToAction("Clubpage");
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
