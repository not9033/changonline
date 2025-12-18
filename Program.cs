using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChangOnline.Context;
using ChangOnline.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ★ ใช้ InMemory Database (ง่ายสุด ไม่ต้องมีไฟล์ .db)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ChangOnlineDB"));  // ใช้ DB ชื่อ ChangOnlineDB

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


// ★ เพิ่ม Route ให้เปิด /MyServiceRequest ได้เลย
app.MapControllerRoute(
    name: "MyServiceRequestRoute",
    pattern: "MyServiceRequest",
    defaults: new { controller = "MyServiceRequest", action = "Index" }
);

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


// ★ สร้าง Roles อัตโนมัติใน InMemory
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "User", "Provider", "Admin" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();
