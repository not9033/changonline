using Microsoft.AspNetCore.Mvc;
using ChangOnline.Models.Admin;
using ChangOnline.Models.ViewModels;
using System;
using System.Linq;

namespace ChangOnline.Controllers.Admin
{
    public class AdminController : Controller
    {
        // Dashboard
        public IActionResult Dashboard()
        {
            var jobsToday = AdminMockData.Jobs.Count(j => j.Date.Date == DateTime.Today);
            var jobsThisWeek = AdminMockData.Jobs.Count(j =>
                System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(j.Date,
                System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday) ==
                System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Today,
                System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday));

            var vm = new AdminDashboardViewModel
            {
                TotalUsers = AdminMockData.Users.Count,
                TotalTechnicians = AdminMockData.Technicians.Count,
                PendingTechnicians = AdminMockData.Technicians.Count(t => t.Status == "Pending"),
                JobsToday = jobsToday,
                JobsThisWeek = jobsThisWeek,
                JobsInProgress = AdminMockData.Jobs.Count(j => j.Status == "In Progress"),
                JobsCompleted = AdminMockData.Jobs.Count(j => j.Status == "Completed"),
                JobsPending = AdminMockData.Jobs.Count(j => j.Status == "Pending"),
                JobsCancelled = AdminMockData.Jobs.Count(j => j.Status == "Cancelled"),
                RecentJobs = AdminMockData.Jobs.OrderByDescending(j => j.Date).Take(5).ToList()
            };

            return View(vm);
        }

        // Users
        public IActionResult Users()
        {
            return View(AdminMockData.Users);
        }

        public IActionResult UserProfile(string id)
        {
            var user = AdminMockData.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        public IActionResult DeleteUser(string id)
        {
            var user = AdminMockData.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
                AdminMockData.Users.Remove(user);

            return RedirectToAction("Users");
        }

        // Technicians
        public IActionResult Technicians()
        {
            return View(AdminMockData.Technicians);
        }

        public IActionResult TechnicianProfile(int id)
        {
            var tech = AdminMockData.Technicians.FirstOrDefault(t => t.Id == id);
            if (tech == null)
                return NotFound();

            return View(tech);
        }

        public IActionResult ApproveTechnician(int id)
        {
            var tech = AdminMockData.Technicians.FirstOrDefault(t => t.Id == id);
            if (tech != null)
                tech.Status = "Approved";

            return RedirectToAction("Technicians");
        }

        public IActionResult DeleteTechnician(int id)
        {
            var tech = AdminMockData.Technicians.FirstOrDefault(t => t.Id == id);
            if (tech != null)
                AdminMockData.Technicians.Remove(tech);

            return RedirectToAction("Technicians");
        }
    } // <-- ปิด class
}
