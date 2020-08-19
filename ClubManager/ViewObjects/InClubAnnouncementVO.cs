using System;
namespace ClubManager.ViewObjects
{
    public class InClubAnnouncementVO
    {
        public string Name { get; set; }
        public long AnnouncementId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}