using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Users
    {
        public Users()
        {
            Announcements = new HashSet<Announcements>();
            Messages = new HashSet<Messages>();
        }

        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }

        public virtual Administrators Administrators { get; set; }
        public virtual Clubs Clubs { get; set; }
        public virtual Students Students { get; set; }
        public virtual ICollection<Announcements> Announcements { get; set; }
        public virtual ICollection<Messages> Messages { get; set; }
    }
}
