using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Activities
    {
        public Activities()
        {
            ActivityAudit = new HashSet<ActivityAudit>();
            ParticipateActivity = new HashSet<ParticipateActivity>();
        }

        public long ActivityId { get; set; }
        public string Name { get; set; }
        public int Fund { get; set; }
        public int Cost { get; set; }
        public string Place { get; set; }
        public DateTime Time { get; set; }
        public DateTime ApplyDate { get; set; }
        public string Description { get; set; }
        public long ClubId { get; set; }
        public bool IsPublic { get; set; }

        public virtual Clubs Club { get; set; }
        public virtual ICollection<ActivityAudit> ActivityAudit { get; set; }
        public virtual ICollection<ParticipateActivity> ParticipateActivity { get; set; }
    }
}
