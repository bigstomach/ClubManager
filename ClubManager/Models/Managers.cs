using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Managers
    {
        public long ClubId { get; set; }
        public long StudentId { get; set; }
        public int Term { get; set; }

        public virtual Clubs Club { get; set; }
        public virtual Students Student { get; set; }
    }
}
