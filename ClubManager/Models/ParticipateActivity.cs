using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class ParticipateActivity
    {
        public long ActivityId { get; set; }
        public long StudentId { get; set; }
        public bool? Status { get; set; }
        public DateTime ApplyDate { get; set; }

        public virtual Activities Activity { get; set; }
        public virtual Students Student { get; set; }
    }
}
