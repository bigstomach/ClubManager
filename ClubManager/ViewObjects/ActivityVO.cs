using System;

namespace ClubManager.ViewObjects
{
    public class ActivityVO
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }//活动名称
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public string Place { get; set; }
        public bool IsPublic { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime ApplyDate { get; set; }
        public int Status { get; set; }
        public string Suggestion { get; set; }
        
        public string ClubName { get; set; }//社团名
        public string AdminName { get; set; }
    }
}