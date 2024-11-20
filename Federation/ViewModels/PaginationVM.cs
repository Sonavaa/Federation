using FederationTask.Models;

namespace Federation.ViewModels
{
    public class PaginationVM
    {
        public List<club> Clubs { get; set; }
        public List<team> Teams { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 5;
    }
}
