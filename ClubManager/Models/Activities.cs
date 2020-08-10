using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Activities
    {
        public Activities()
        {
            ParticipateActivity = new HashSet<ParticipateActivity>();
        }

        public long ActivityId { get; set; }
        public long ClubId { get; set; }
        public long? AdminId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public string Place { get; set; }
        public bool IsPublic { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime ApplyDate { get; set; }
        public bool Status { get; set; }
        public string Suggestion { get; set; }

        public virtual Administrators Admin { get; set; }
        public virtual Clubs Club { get; set; }
        public virtual ICollection<ParticipateActivity> ParticipateActivity { get; set; }
    }
}
