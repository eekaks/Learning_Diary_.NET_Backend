using System.Collections.Generic;
using System.Linq;
using Learning_Diary_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Diary_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Topic>> GetAll()
        {
            using (var db = new Learning_DiaryContext())
            {
                List<Topic> topics = db.Topic.Select(topic => topic).ToList();
                if (!topics.Any())
                {
                    return NotFound();
                }
                return topics;
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Topic> Get(int id)
        {
            using (var db = new Learning_DiaryContext())
            {
                var topic = db.Topic.FirstOrDefault(topic => topic.Id == id);
                if (topic == null)
                {
                    return NotFound();
                }
                return topic;
            }
        }

        [HttpPost]
        public IActionResult Create(Topic topic)
        {
            using (var db = new Learning_DiaryContext())
            {
                if (!db.Topic.Any())
                {
                    topic.Id = 1;
                }
                else
                {
                    topic.Id = db.Topic.Max(topic => topic.Id) + 1;
                }
                db.Topic.Add(topic);
                db.SaveChanges();
            }

            return CreatedAtAction(nameof(Create), new { id = topic.Id }, topic);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var db = new Learning_DiaryContext())
            {
                var topicToDelete = db.Topic.FirstOrDefault(topic => topic.Id == id);
                if (topicToDelete is null)
                {
                    return NotFound();
                }
                db.Topic.Remove(topicToDelete);
                List<int> taskIdsToRemove = new List<int>();
                foreach(Task task in db.Task.Select(task => task))
                {
                    taskIdsToRemove.Add(task.Id);
                }
                db.Task.RemoveRange(db.Task.Where(task => task.Topic == topicToDelete.Id));
                db.Note.RemoveRange(db.Note.Where(note => taskIdsToRemove.Contains(note.Task)));
                db.SaveChanges();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Topic topic)
        {
            if (id != topic.Id)
            {
                return BadRequest();
            }
            using (var db = new Learning_DiaryContext())
            {
                Topic topicToUpdate = db.Topic.FirstOrDefault(topic => topic.Id == id);
                if (topicToUpdate is null)
                {
                    return NotFound();
                }
                topicToUpdate.Id = topic.Id;
                topicToUpdate.Title = topic.Title;
                topicToUpdate.Description = topic.Description;
                topicToUpdate.Source = topic.Source;
                topicToUpdate.EstimatedTimeToMaster = topic.EstimatedTimeToMaster;
                topicToUpdate.CompletionDate = topic.CompletionDate;
                topicToUpdate.InProgress = topic.InProgress;
                topicToUpdate.StartLearningDate = topic.StartLearningDate;
                topicToUpdate.TimeSpent = topic.TimeSpent;
                db.SaveChanges();
            }

            return NoContent();
        }
    }
}
