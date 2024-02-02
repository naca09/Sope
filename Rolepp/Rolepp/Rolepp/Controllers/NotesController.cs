using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rolepp.Data;
using Rolepp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Rolepp.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notes
        public IActionResult Index()
        {
            return View(_context.Notes.ToList());
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("NoteId,NoteCode,CreateName,Customer,AddressCustomer,Reason,Status")] Note note)
        {
            if (ModelState.IsValid)
            {
                // Đặt status mặc định là 1 khi tạo mới
                note.Status = 1;

                _context.Add(note);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/{id}
        public IActionResult Details(int id)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // GET: Notes/{id}/Edit
        public IActionResult Edit(int id)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Notes/{id}/Edit
        [HttpPost]
        public IActionResult Edit(int id, Note note)
        {
            if (id != note.NoteId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Entry(note).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // DELETE: Notes/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }
            _context.Notes.Remove(note);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusAjax(int id, int newStatus)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            note.UpdateStatus(newStatus);

            _context.Entry(note).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }

        public IActionResult GetNewNoteCount()
        {
            int newNoteCount = _context.Notes.Count(n => n.Status == 2);
            return Json(newNoteCount);
        }

        public IActionResult CheckNoteStatus()
        {
            bool hasNoteStatus34 = _context.Notes.Any(n => n.Status == 3 || n.Status == 4);
            return Json(hasNoteStatus34);
        }
    }
}
