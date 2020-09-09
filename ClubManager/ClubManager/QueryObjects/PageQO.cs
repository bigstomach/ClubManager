namespace ClubManager.QueryObjects
{
    public class PageQO
    {
        public string Query { get; set; }
        public int? PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool Status { get; set; }
    }
}