﻿using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class ParticipateActivity
    {
        public long StudentId { get; set; }
        public long ActivityId { get; set; }
        public DateTime ApplyDate { get; set; }
        public string ApplyReason { get; set; }
        public bool? Status { get; set; }

        public virtual Activities Activity { get; set; }
        public virtual Students Student { get; set; }
    }
}
