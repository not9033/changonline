using ChangOnline.Models;
using ChangOnline.Models.Admin;

namespace ChangOnline.Models.Admin
{
    public static class AdminMockData
    {
        public static List<AppUser> Users = new List<AppUser>
        {
            new AppUser { Id="1", FirstName="Somchai", LastName="S", Email="somchai@example.com", Phone="0812345678" },
            new AppUser { Id="2", FirstName="Suda", LastName="L", Email="suda@example.com", Phone="0898765432" }
        };

        public static List<TechnicianModel> Technicians = new List<TechnicianModel>
        {
            new TechnicianModel { Id=1, Name="Tech A", Skill="Electrical", Status="Approved", Phone="0811111111" },
            new TechnicianModel { Id=2, Name="Tech B", Skill="Plumbing", Status="Pending", Phone="0822222222" }
        };

        public static List<JobModel> Jobs = new List<JobModel>
        {
            new JobModel { Id=1, UserName="Somchai S.", TechnicianName="Tech A", Type="Electrical", Date=DateTime.Now, Status="In Progress" },
            new JobModel { Id=2, UserName="Suda L.", TechnicianName="", Type="Plumbing", Date=DateTime.Now, Status="Pending" },
            new JobModel { Id=3, UserName="Somchai S.", TechnicianName="Tech B", Type="Door Repair", Date=DateTime.Now.AddDays(-1), Status="Completed" }
        };
    }
}
