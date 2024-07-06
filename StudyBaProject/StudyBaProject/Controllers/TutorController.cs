using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBa.Models;
using StudyBaProject.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using StudyBaProject.Models;

namespace StudyBaProject.Controllers
{
    public class TutorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TutorController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ProfileTutor()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.IdentityId == userId);
            if (user == null)
            {
                return NotFound();
            }

            // Retrieve sessions for the current user
            var userSessions = await _context.Sessions
                .Where(s => s.UserId == user.UserId)
                .ToListAsync();

            // Retrieve subjects for the tutor
            var tutorSubjects = await _context.TutorsSubjects
                .Include(ts => ts.Subject)
                .Where(ts => ts.TutorId == user.UserId)
                .ToListAsync();

            // Retrieve reviews for the tutor
            var tutorReviews = await _context.Reviews
                .Where(r => r.TutorId == user.UserId)
                .ToListAsync();

            // Retrieve received session requests
            var receivedRequests = await _context.SessionRequests
                .Where(r => r.ReceiverId == user.UserId)
                .ToListAsync();

            // Retrieve sender names for received requests
            var senderIds = receivedRequests.Select(r => r.SenderId).Distinct().ToList();
            var senders = await _context.Users
                .Where(u => senderIds.Contains(u.UserId))
                .ToDictionaryAsync(u => u.UserId, u => u.FirstName + " " + u.LastName);

            // Pass data to the view
            ViewBag.Subjects = tutorSubjects.Select(ts => ts.Subject).ToList();
            ViewBag.Sessions = userSessions;
            ViewBag.Reviews = tutorReviews;
            ViewBag.ReceivedRequests = receivedRequests;
            ViewBag.Senders = senders;

            return View(user);
        }





        // GET: Tutor/GetSubjects/5
        // GET: Tutor/GetSubjects/5
        public async Task<IActionResult> GetSubjects(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorSubjects = await _context.TutorsSubjects
                .Include(ts => ts.Subject)
                .Where(ts => ts.TutorId == id)
                .ToListAsync();

            if (tutorSubjects == null)
            {
                return NotFound();
            }

            ViewBag.Subjects = tutorSubjects.Select(ts => ts.Subject).ToList();

            return View("ProfileTutor");
        }

        public async Task<IActionResult> AllSubjects(int tutorId)
        {
            ViewBag.TutorId = tutorId;
            var subjects = await _context.Subjects.ToListAsync();
            return View(subjects);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubject(int tutorId, int subjectId)
        {
            var tutorSubject = new TutorSubject
            {
                TutorId = tutorId,
                SubjectId = subjectId
            };

            _context.TutorsSubjects.Add(tutorSubject);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProfileTutor));
        }

        public IActionResult AddSession(int tutorId)
        {
            var session = new Session
            {
                UserId = tutorId,
                SessionDate = DateTime.Now // default to current date and time
            };
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSession(Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ProfileTutor));
            }
            return View(session);
        }






        // GET: Tutor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Tutor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Tutor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tutor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,FirstName,LastName,DateOfBirth,Email,ContactNumber,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Tutor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Tutor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,FirstName,LastName,DateOfBirth,Email,ContactNumber,Role")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Tutor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Tutor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
