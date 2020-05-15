using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class JoinClubs
    {
        public long StudentId { get; set; }
        public long ClubId { get; set; }
        public bool? Status { get; set; }
        public DateTime ApplyDate { get; set; }
        public string Position { get; set; }
        public byte Payed { get; set; }

        public virtual Clubs Club { get; set; }
        public virtual Students Student { get; set; }
    }
}
