using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Clubs
    {
        public Clubs()
        {
            Activities = new HashSet<Activities>();
            Announcements = new HashSet<Announcements>();
            JoinClubs = new HashSet<JoinClubs>();
            Sponsorships = new HashSet<Sponsorships>();
        }

        public long ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public long StudentId { get; set; }
        public long UserId { get; set; }
        public string Type { get; set; }

        public virtual Students Student { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Activities> Activities { get; set; }
        public virtual ICollection<Announcements> Announcements { get; set; }
        public virtual ICollection<JoinClubs> JoinClubs { get; set; }
        public virtual ICollection<Sponsorships> Sponsorships { get; set; }
    }
}
