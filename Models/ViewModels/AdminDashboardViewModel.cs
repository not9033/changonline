using ChangOnline.Models.Admin;

namespace ChangOnline.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalTechnicians { get; set; }
        public int PendingTechnicians { get; set; }

        public int JobsToday { get; set; }
        public int JobsThisWeek { get; set; }
        public int JobsInProgress { get; set; }
        public int JobsCompleted { get; set; }
        public int JobsPending { get; set; }
        public int JobsCancelled { get; set; }
        public List<JobModel> RecentJobs { get; set; } = new List<JobModel>();
    }
}
