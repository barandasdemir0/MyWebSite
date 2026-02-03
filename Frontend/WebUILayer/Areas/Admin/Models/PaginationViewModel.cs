namespace WebUILayer.Areas.Admin.Models
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}
