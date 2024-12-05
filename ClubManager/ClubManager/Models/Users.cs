using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Users
    {
        public Users()
        {
            Clubs = new HashSet<Clubs>();
            Students = new HashSet<Students>();
        }

        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }

        public virtual Administrators Administrators { get; set; }
        public virtual ICollection<Clubs> Clubs { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}
