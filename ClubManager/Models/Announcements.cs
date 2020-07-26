using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Announcements
    {
        public long AnnouncementId { get; set; }
        public long ClubId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; }

        public virtual Clubs Club { get; set; }
    }
}
