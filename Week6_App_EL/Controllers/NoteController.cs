using System.Collections.Generic;
using System.Linq;
using Learning_Diary_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Diary_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Note>> GetAll()
        {
            using (var db = new Learning_DiaryContext())
            {
                List<Note> notes = db.Note.Select(note => note).ToList();
                if (!notes.Any())
                {
                    return NotFound();
                }
                return notes;
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Note> Get(int id)
        {
            using (var db = new Learning_DiaryContext())
            {
                var note = db.Note.FirstOrDefault(note => note.Id == id);
                if (note == null)
                {
                    return NotFound();
                }
                return note;
            }
        }
        [HttpPost]
        public IActionResult Create(Note note)
        {
            using (var db = new Learning_DiaryContext())
            {
                if (!db.Note.Any())
                {
                    note.Id = 1;
                }
                else
                {
                    note.Id = db.Note.Max(note => note.Id) + 1;
                }
                db.Note.Add(note);
                db.SaveChanges();
            }

            return CreatedAtAction(nameof(Create), new { id = note.Id }, note);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var db = new Learning_DiaryContext())
            {
                var noteToDelete = db.Note.FirstOrDefault(note => note.Id == id);
                if (noteToDelete is null)
                {
                    return NotFound();
                }
                db.Note.Remove(noteToDelete);
                db.SaveChanges();
            }

            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }
            using (var db = new Learning_DiaryContext())
            {
                Note noteToUpdate = db.Note.FirstOrDefault(note => note.Id == id);
                if (noteToUpdate is null)
                {
                    return NotFound();
                }
                noteToUpdate.Id = note.Id;
                noteToUpdate.Task = note.Task;
                noteToUpdate.Note1 = note.Note1;
                db.SaveChanges();
            }
            return NoContent();
        }
    }
}