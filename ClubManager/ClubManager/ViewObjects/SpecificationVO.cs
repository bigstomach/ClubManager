using System;

namespace ClubManager.ViewObjects
{
    public class SpecificationVO
    {
        public long SpecificationId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string AdminName { get; set; }
    }
}