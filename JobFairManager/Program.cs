using JobFairManager.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// ע�� DbContext ������ SQL Server ����
builder.Services.AddDbContext<JobFairDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
//    if (dbContext.Database.CanConnect())
//    {
//        Console.WriteLine("Connected to the database.");
//    }
//    else
//    {
//        Console.WriteLine("Failed to connect to the database.");
//    }

//    if (dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Open)
//    {
//        Console.WriteLine("Database connection is open.");
//    }

//    var articlesCount = dbContext.Articles.Count();
//    Console.WriteLine($"Articles count: {articlesCount}");
//}
app.Run();
