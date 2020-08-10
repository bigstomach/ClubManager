using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Students
    {
        public Students()
        {
            JoinClub = new HashSet<JoinClub>();
            Managers = new HashSet<Managers>();
            ParticipateActivity = new HashSet<ParticipateActivity>();
        }

        public long StudentId { get; set; }
        public int Number { get; set; }
        public string Phone { get; set; }
        public byte[] Avatar { get; set; }
        public string Signature { get; set; }
        public string Mail { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual StudentMeta NumberNavigation { get; set; }
        public virtual Users Student { get; set; }
        public virtual ICollection<JoinClub> JoinClub { get; set; }
        public virtual ICollection<Managers> Managers { get; set; }
        public virtual ICollection<ParticipateActivity> ParticipateActivity { get; set; }
    }
}
