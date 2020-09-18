using System.Collections.Generic;

namespace ClubManager.ViewObjects
{
    public class ClubGraphVO
    {
        public List<string> GradeGraphDescription { get; set; }
        public List<int> GradeGraphData { get; set; }
        public List<KeyValue> MajorGraphData { get; set; }
    }
}