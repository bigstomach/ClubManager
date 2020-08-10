using System;

namespace ClubManager.QueryObjects
{
    public class ActivityQO
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        
        public DateTime EventTime { get; set; }
        public decimal Budget { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}