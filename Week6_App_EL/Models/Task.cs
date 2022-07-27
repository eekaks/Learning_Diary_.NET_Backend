using System;
using System.Collections.Generic;

namespace Learning_Diary_Backend.Models
{
    public partial class Task
    {
        public int Id { get; set; }
        public int Topic { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int? Priority { get; set; }
        public bool? Done { get; set; }
        public string Notes { get; set; }
    }
}
