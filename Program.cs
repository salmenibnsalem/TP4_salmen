using TP4_salmen.Data;
using TP4_salmen.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<StudentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDBConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<StudentContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

await SeedRolesAndAdminUserAsync(app.Services);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    const string adminRole = "Admin";
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    const string adminEmail = "admin@gmail.com";
    const string adminPassword = "Admin@123";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}
