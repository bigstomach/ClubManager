using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.QueryObjects
{
    public class JoinClubStatusQO
    {
        public long StudentId { get; set; }

        public long ClubId { get; set; }
        public bool Status { get; set; }//false 待审核和拒绝   true 加入
    }
}
