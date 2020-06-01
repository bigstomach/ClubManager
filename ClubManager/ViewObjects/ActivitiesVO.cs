using System;

namespace ClubManager.ViewObjects
{
    public class ActivitiesVO
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public int Fund { get; set; }
        public int Cost { get; set; }
        public string Place { get; set; }
        public DateTime Time { get; set; }
        public DateTime ApplyDate { get; set; }
        public bool? Status { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}