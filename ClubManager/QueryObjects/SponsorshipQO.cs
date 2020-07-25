using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.QueryObjects
{
    public class SponsorshipQO
    {
        public long SponsorshipId { get; set; }
        public long ClubId { get; set; }
        public string ClubName { get; set; }
        public string Sponsor { get; set; }
        public decimal Amount { get; set; }



    }
}
