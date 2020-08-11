using System;

namespace ClubManager.ViewObjects
{
    public class MessageVO
    {
        public long MessageId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool Read { get; set; }
    }
}