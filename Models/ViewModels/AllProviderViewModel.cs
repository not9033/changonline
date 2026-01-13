using ChangOnline.Models;

namespace ChangOnline.Models.ViewModels
{
    public class AllProviderViewModel
    {
        public IEnumerable<ProviderProfile> Providers { get; set; } = new List<ProviderProfile>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }

        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Provinces { get; set; } =
            new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

        public List<ProviderReview> RandomReviews { get; set; } = new List<ProviderReview>();
    }
}
