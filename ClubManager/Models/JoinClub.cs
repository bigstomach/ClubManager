﻿using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class JoinClub
    {
        public long ClubId { get; set; }
        public long StudentId { get; set; }
        public DateTime ApplyDate { get; set; }
        public string ApplyReason { get; set; }
        public bool Status { get; set; }//false 申请 true 加入 拒绝和推出直接删除记录

        public virtual Clubs Club { get; set; }
        public virtual Students Student { get; set; }
    }
}
