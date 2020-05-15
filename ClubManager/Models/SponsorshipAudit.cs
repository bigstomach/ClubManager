using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class SponsorshipAudit
    {
        public long SponsorshipAuditId { get; set; }
        public long SponsorshipsId { get; set; }
        public long? UserId { get; set; }
        public bool? Status { get; set; }
        public string Suggestion { get; set; }

        public virtual Sponsorships Sponsorships { get; set; }
        public virtual Administrators User { get; set; }
    }
}
