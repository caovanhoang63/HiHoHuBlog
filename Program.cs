using HiHoHuBlog;
using HiHoHuBlog.Components;
using HiHoHuBlog.Modules.User;
using HiHoHuBlog.Modules.User.Service;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Repository.Implementation;
using HiHoHuBlog.Modules.User.Service.Implementation;
using HiHoHuBlog.Modules.User.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IUserRepository, EfRepo>();

builder.Services.AddScoped<IUserSignUpService, UserSignUpService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();

builder.Services.AddAutoMapper(typeof(UserMappingProfile));


Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion("8.0.35"))
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