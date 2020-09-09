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
        public string Type { get; set; }
    }
}