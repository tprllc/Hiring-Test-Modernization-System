using LegacyTestPlayer.Data;
using LegacyTestPlayer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TestDb") ?? "Data Source=testplayer.db";
var dataSource = connectionString.Split(';')[0].Replace("Data Source=", "").Trim();
var directory = Path.GetDirectoryName(dataSource);
if (!string.IsNullOrEmpty(directory))
    Directory.CreateDirectory(directory);

builder.Services.AddDbContext<TestDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<GradingService>();
builder.Services.AddScoped<IQuestionSelector, SequentialQuestionSelector>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TestDbContext>();
    db.Database.EnsureCreated();
    CatalogSeeder.Seed(db);
}

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
