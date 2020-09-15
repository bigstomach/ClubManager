using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Clubs
    {
        public Clubs()
        {
            Activities = new HashSet<Activities>();
            JoinClub = new HashSet<JoinClub>();
            Managers = new HashSet<Managers>();
            Sponsorships = new HashSet<Sponsorships>();
        }

        public long ClubId { get; set; }
        public string Name { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public int Type { get; set; }//0-学术科技类，1-传统文化与文学类，2-公益实践类，3-文化艺术类，4-体育竞技类，5-创新创业类
        public string Logo { get; set; }
        public int Status { get; set; }////解散0，运行1，未审核2
        public string Description { get; set; }

        public virtual Users Club { get; set; }
        public virtual ICollection<Activities> Activities { get; set; }
        public virtual ICollection<JoinClub> JoinClub { get; set; }
        public virtual ICollection<Managers> Managers { get; set; }
        public virtual ICollection<Sponsorships> Sponsorships { get; set; }
    }
}
