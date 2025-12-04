namespace ChangOnline.Models.Admin
{
    public class JobModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string TechnicianName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; } = string.Empty; // Pending, In Progress, Completed
    }
}
