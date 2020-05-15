using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Administrators
    {
        public Administrators()
        {
            ActivityAudit = new HashSet<ActivityAudit>();
            Specifications = new HashSet<Specifications>();
            SponsorshipAudit = new HashSet<SponsorshipAudit>();
        }

        public long UserId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<ActivityAudit> ActivityAudit { get; set; }
        public virtual ICollection<Specifications> Specifications { get; set; }
        public virtual ICollection<SponsorshipAudit> SponsorshipAudit { get; set; }
    }
}
