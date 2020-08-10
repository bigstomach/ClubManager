using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.QueryObjects
{
    public class MessageQO
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
