using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Students
    {
        public Students()
        {
            Clubs = new HashSet<Clubs>();
            JoinClubs = new HashSet<JoinClubs>();
            ParticipateActivity = new HashSet<ParticipateActivity>();
        }

        public long StudentId { get; set; }
        public string Number { get; set; }
        public long? UserId { get; set; }
        public string Name { get; set; }
        public byte? Grade { get; set; }
        public string Major { get; set; }
        public string Phone { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Clubs> Clubs { get; set; }
        public virtual ICollection<JoinClubs> JoinClubs { get; set; }
        public virtual ICollection<ParticipateActivity> ParticipateActivity { get; set; }
    }
}
