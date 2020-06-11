using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Administrators
    {
        public Administrators()
        {
            Activities = new HashSet<Activities>();
            Specifications = new HashSet<Specifications>();
            Sponsorships = new HashSet<Sponsorships>();
        }

        public long UserId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Activities> Activities { get; set; }
        public virtual ICollection<Specifications> Specifications { get; set; }
        public virtual ICollection<Sponsorships> Sponsorships { get; set; }
    }
}
