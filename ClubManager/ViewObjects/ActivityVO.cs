using System;

namespace ClubManager.ViewObjects
{
    public class ActivityVO
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public string Place { get; set; }
        public bool IsPublic { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime ApplyDate { get; set; }
        public bool Status { get; set; }
        public string Suggestion { get; set; }
    }
}