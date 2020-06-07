using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.QueryObjects
{
    public class PostSponsorAuditQO
    {
        public long SponsorshipAuditId { get; set; }
        public bool? Status { get; set; }
        public string Suggestion { get; set; }
        public long SponsorshipsId { get; set; }
    }
}
