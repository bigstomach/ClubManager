using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Administrators
    {
        public Administrators()
        {
            Activities = new HashSet<Activities>();
            Sponsorships = new HashSet<Sponsorships>();
        }

        public long AdminId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public virtual Users Admin { get; set; }
        public virtual ICollection<Activities> Activities { get; set; }
        public virtual ICollection<Sponsorships> Sponsorships { get; set; }
    }
}
