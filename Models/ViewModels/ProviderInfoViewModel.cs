using ChangOnline.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChangOnline.Models.ViewModels
{
    public class ProviderInfoViewModel
    {
        public ProviderProfile Provider { get; set; }
        public List<SelectListItem> Provinces { get; set; } = new List<SelectListItem>();
        public List<ProviderReview> Reviews { get; set; } = new List<ProviderReview>();
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
