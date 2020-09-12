using System;

namespace ClubManager.ViewObjects
{
    public class ClubVO
    {
        public long ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string PresidentName { get; set; }
        public int Type { get; set; }

        public long ManagerId { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public int Number { get; set; }
        public int Grade { get; set; }
        public string Major { get; set; }

        public int Status { get; set; } 
    }
}