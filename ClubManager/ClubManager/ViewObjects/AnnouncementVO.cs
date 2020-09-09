using System;

namespace ClubManager.ViewObjects
{
    public class AnnouncementVO
    {
        public long AnnouncementId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}