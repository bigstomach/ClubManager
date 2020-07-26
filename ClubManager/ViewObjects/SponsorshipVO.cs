using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.ViewObjects
{
    public class SponsorshipVO
    {
        public long SponsorshipId { get; set; }
        public string ClubName { get; set; }
        public DateTime ApplyTime { get; set; }
        public string Sponsor { get; set; }
        public decimal Amount { get; set; }
        public string adminName { get; set; }
        public string Requirement { get; set; }
        public string Suggestion { get; set; }
        public int? Status { get; set; }//0表示待审核，1表示审核通过，2表示审核未通过
    }
}
