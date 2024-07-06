using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyBa.Models;
using StudyBaProject.Data;
using StudyBaProject.Models;

namespace StudyBaProject.Controllers
{
    public class TutorSubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TutorSubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> SendSessionRequest([FromBody] SessionRequest sessionRequest)
        {
            if (sessionRequest == null)
            {
                return BadRequest();
            }

            var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (identityId == null)
            {
                return Unauthorized();
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdentityId == identityId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            sessionRequest.SenderId = currentUser.UserId;

            _context.SessionRequests.Add(sessionRequest);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> Tutors(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorSubjects = await _context.TutorsSubjects
                .Include(ts => ts.User) // Uključujemo podatke o tutoru
                .Where(ts => ts.SubjectId == id)
                .ToListAsync();

            if (tutorSubjects == null || tutorSubjects.Count == 0)
            {
                ViewData["Message"] = "Trenutno nema tutora za ovaj predmet.";
                return View("Index");
            }

            return View("Index", tutorSubjects);
        }


        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            if (review == null)
            {
                return BadRequest();
            }

            // Retrieve the current user's identity
            var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (identityId == null)
            {
                return Unauthorized();
            }

            // Find the user in the database
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdentityId == identityId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Set the current user's ID to the review
            review.StudentId = currentUser.UserId;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok();
        }


        // GET: TutorSubjects
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TutorsSubjects.Include(t => t.Subject).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TutorSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorSubject = await _context.TutorsSubjects
                .Include(t => t.Subject)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TutorId == id);

            if (tutorSubject == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.Student)
                .Where(r => r.TutorId == id)
                .ToListAsync();

            var averageGrade = reviews.Any() ? Math.Round(reviews.Average(r => r.Grade), 2) : 0;

            ViewData["Reviews"] = reviews;
            ViewData["AverageGrade"] = averageGrade;

            return View(tutorSubject);
        }





        // GET: TutorSubjects/Create
        public IActionResult Create()
        {
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId");
            ViewData["TutorId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: TutorSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TutorSubjectId,TutorId,SubjectId")] TutorSubject tutorSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutorSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", tutorSubject.SubjectId);
            ViewData["TutorId"] = new SelectList(_context.Users, "UserId", "UserId", tutorSubject.TutorId);
            return View(tutorSubject);
        }

        // GET: TutorSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorSubject = await _context.TutorsSubjects.FindAsync(id);
            if (tutorSubject == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", tutorSubject.SubjectId);
            ViewData["TutorId"] = new SelectList(_context.Users, "UserId", "UserId", tutorSubject.TutorId);
            return View(tutorSubject);
        }

        // POST: TutorSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TutorSubjectId,TutorId,SubjectId")] TutorSubject tutorSubject)
        {
            if (id != tutorSubject.TutorSubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorSubjectExists(tutorSubject.TutorSubjectId))
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
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "SubjectId", tutorSubject.SubjectId);
            ViewData["TutorId"] = new SelectList(_context.Users, "UserId", "UserId", tutorSubject.TutorId);
            return View(tutorSubject);
        }

        // GET: TutorSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorSubject = await _context.TutorsSubjects
                .Include(t => t.Subject)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TutorSubjectId == id);
            if (tutorSubject == null)
            {
                return NotFound();
            }

            return View(tutorSubject);
        }

        // POST: TutorSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutorSubject = await _context.TutorsSubjects.FindAsync(id);
            if (tutorSubject != null)
            {
                _context.TutorsSubjects.Remove(tutorSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorSubjectExists(int id)
        {
            return _context.TutorsSubjects.Any(e => e.TutorSubjectId == id);
        }
    }
}