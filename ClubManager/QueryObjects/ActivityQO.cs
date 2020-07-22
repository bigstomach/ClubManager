using System;

namespace ClubManager.QueryObjects
{
    public class ActQO
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public decimal Fund { get; set; }
        public decimal Cost { get; set; }
        public string Place { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}