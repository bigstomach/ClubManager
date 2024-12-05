using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class Specifications
    {
        public long SpecificationId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public virtual Administrators User { get; set; }
    }
}
