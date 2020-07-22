using System;

namespace ClubManager.ViewObjects
{
    public class ActivitiesVO
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public decimal Fund { get; set; }
        public decimal Cost { get; set; }
        public string Place { get; set; }
        public DateTime Time { get; set; }
        public DateTime ApplyDate { get; set; }
        public bool? Status { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Suggestion { get; set; }
    }
}