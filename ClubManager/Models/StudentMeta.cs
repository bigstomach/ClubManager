using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class StudentMeta
    {
        public StudentMeta()
        {
            Students = new HashSet<Students>();
        }

        public int Number { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public string Major { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Students> Students { get; set; }
    }
}
