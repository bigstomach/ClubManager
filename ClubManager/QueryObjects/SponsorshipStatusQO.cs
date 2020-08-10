using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.QueryObjects
{
    public class SponsorshipStatusQO
    {
        public long sponsorshipId { get; set; }
        public int? status { get; set; }////0表示待审核，1表示审核通过，2表示审核未通过
    }
}
