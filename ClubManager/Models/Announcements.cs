﻿using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Announcements
    {
        public long AnnouncementId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public virtual Users User { get; set; }
    }
}
