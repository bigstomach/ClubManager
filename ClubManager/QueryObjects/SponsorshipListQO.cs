using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.QueryObjects
{
    public class SponsorshipListQO
    {
        public PageQO PageQO;
        public string Status;//unaudited, failed, pass, all分别表示待审核，审核不通过，审核通过和所有赞助
    }
}
