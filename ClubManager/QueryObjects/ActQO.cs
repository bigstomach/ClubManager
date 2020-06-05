using System;

namespace ClubManager.QueryObjects
{
    public class ActQO
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public int Fund { get; set; }
        public int Cost { get; set; }
        public string Place { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}