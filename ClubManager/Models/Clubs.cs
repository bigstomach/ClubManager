using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Clubs
    {
        public Clubs()
        {
            Activities = new HashSet<Activities>();
            JoinClub = new HashSet<JoinClub>();
            Managers = new HashSet<Managers>();
            Sponsorships = new HashSet<Sponsorships>();
        }

        public long ClubId { get; set; }
        public string Name { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public int Type { get; set; }
        public byte[] Logo { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }

        public virtual Users Club { get; set; }
        public virtual ICollection<Activities> Activities { get; set; }
        public virtual ICollection<JoinClub> JoinClub { get; set; }
        public virtual ICollection<Managers> Managers { get; set; }
        public virtual ICollection<Sponsorships> Sponsorships { get; set; }
    }
}
