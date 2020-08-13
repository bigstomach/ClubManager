using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.QueryObjects
{
    public class JoinClubQO
    {
        public long ClubId { get; set; }
        public long StudentId { get; set; }
        public DateTime ApplyDate { get; set; }
        public string ApplyReason { get; set; }
        public bool Status { get; set; }//false 申请 true 加入 拒绝和推出直接删除记录
    }
}
