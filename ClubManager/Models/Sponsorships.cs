using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Sponsorships
    {
        public long SponsorshipId { get; set; }
        public long ClubId { get; set; }
        public DateTime ApplyTime { get; set; }
        public string Sponsor { get; set; }
        public decimal Amount { get; set; }
        public string Requirement { get; set; }
        public long? AdminId { get; set; }
        public bool? Status { get; set; }
        public string Suggestion { get; set; }

        public virtual Administrators Admin { get; set; }
        public virtual Clubs Club { get; set; }
    }
}
