using System.Collections.Generic;
using System.Linq;
using Learning_Diary_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Diary_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Task>> GetAll()
        {
            using (var db = new Learning_DiaryContext())
            {
                List<Task> tasks = db.Task.Select(task => task).ToList();
                if(!tasks.Any())
                {
                    return NotFound();
                }
                return tasks;
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            using (var db = new Learning_DiaryContext())
            {
                var task = db.Task.FirstOrDefault(task => task.Id == id);
                if (task == null)
                {
                    return NotFound();
                }
                return task;
            }
        }
        [HttpPost]
        public IActionResult Create(Task task)
        {
            using (var db = new Learning_DiaryContext())
            {
                if (!db.Task.Any())
                {
                    task.Id = 1;
                }
                else
                {
                    task.Id = db.Task.Max(task => task.Id) + 1;
                }
                db.Task.Add(task);
                db.SaveChanges();
            }

            return CreatedAtAction(nameof(Create), new { id = task.Id }, task);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var db = new Learning_DiaryContext())
            {
                var taskToDelete = db.Task.FirstOrDefault(task => task.Id == id);
                if (taskToDelete is null)
                {
                    return NotFound();
                }
                db.Task.Remove(taskToDelete);
                db.Note.RemoveRange(db.Note.Where(note => note.Task == taskToDelete.Id));
                db.SaveChanges();
            }

            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }
            using (var db = new Learning_DiaryContext())
            {
                Task taskToUpdate = db.Task.FirstOrDefault(task => task.Id == id);
                if (taskToUpdate is null)
                {
                    return NotFound();
                }
                taskToUpdate.Id = task.Id;
                taskToUpdate.Topic = task.Topic;
                taskToUpdate.Title = task.Title;
                taskToUpdate.Description = task.Description;
                taskToUpdate.Deadline = task.Deadline;
                taskToUpdate.Priority = task.Priority;
                taskToUpdate.Done = task.Done;
                taskToUpdate.Notes = task.Notes;
                db.SaveChanges();
            }

            return NoContent();
        }
    }
}