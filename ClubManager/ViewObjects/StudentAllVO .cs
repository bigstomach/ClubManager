using System;
using System.Reflection.Metadata;

namespace ClubManager.ViewObjects
{
    public class StudentAllVO
    {
        //public long StudentId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public string Major { get; set; }
        public bool Status { get; set; }
        public string Phone { get; set; }
        public string Signature { get; set; }
        public string Mail { get; set; }
        public DateTime Birthday { get; set; }
    }
}
