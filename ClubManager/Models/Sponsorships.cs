using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Sponsorships
    {
        public Sponsorships()
        {
            SponsorshipAudit = new HashSet<SponsorshipAudit>();
        }

        public long SponsorshipId { get; set; }
        public long ClubId { get; set; }
        public DateTime ApplyTime { get; set; }
        public string Sponsor { get; set; }
        public decimal Amount { get; set; }
        public string Requirement { get; set; }

        public virtual Clubs Club { get; set; }
        public virtual ICollection<SponsorshipAudit> SponsorshipAudit { get; set; }
    }
}
