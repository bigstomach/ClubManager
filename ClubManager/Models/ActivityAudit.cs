using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class ActivityAudit
    {
        public long ActivityAuditId { get; set; }
        public long ActivityId { get; set; }
        public long? UserId { get; set; }
        public bool? Status { get; set; }
        public string Suggestion { get; set; }

        public virtual Activities Activity { get; set; }
        public virtual Administrators User { get; set; }
    }
}
