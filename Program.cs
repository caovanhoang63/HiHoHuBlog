using HiHoHuBlog;
using HiHoHuBlog.Components;
using HiHoHuBlog.Modules.User.Biz;
using HiHoHuBlog.Modules.User.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);
var connectionString = new MySqlConnectionStringBuilder();

// connectionString.Server = "database-1.c7k00k46qm5n.ap-southeast-1.rds.amazonaws.com";
// connectionString.UserID = "admin";
// connectionString.Password = "GVo22W0i6Niv94xXB4lt";
// connectionString.Database = "blog";



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IUserSignUpStore, EFData>();
builder.Services.AddScoped<IUserLoginStore, EFData>();

builder.Services.AddScoped<IUserSignUpBiz, UserSignUpBiz>();
builder.Services.AddScoped<IUserLoginBiz, UserLoginBiz>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString.ToString(), new MySqlServerVersion("8.0.35"))
    // The following three options help with debugging, but should
    // be changed or removed for production.
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();