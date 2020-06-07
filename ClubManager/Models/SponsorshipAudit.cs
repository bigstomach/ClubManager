using System;
using System.Collections.Generic;

namespace ClubManager
{
    public partial class SponsorshipAudit
    {
        public long SponsorshipAuditId { get; set; }
        public long SponsorshipsId { get; set; }
        public long? UserId { get; set; }//问号表示允许是空值的意思
        public bool? Status { get; set; }
        public string Suggestion { get; set; }//我觉得应该允许空值的，就是在已经提交申请但是管理员没有审核

        public virtual Sponsorships Sponsorships { get; set; }
        public virtual Administrators User { get; set; }
    }
}
