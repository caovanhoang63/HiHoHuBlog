using Amazon.Internal;
using Amazon.Runtime;
using Amazon.S3;
using Blazored.Toast;
using HiHoHuBlog;
using HiHoHuBlog.Components;
using HiHoHuBlog.Modules.Blog;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Repository.Implementation;
using HiHoHuBlog.Modules.Blog.Service.Implementation;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Modules.User;
using HiHoHuBlog.Modules.User.Service;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Repository.Implementation;
using HiHoHuBlog.Modules.User.Service.Implementation;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);



builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var awsAccessKeyId = configuration["AWS:AccessKeyId"];
    var awsSecretAccessKey = configuration["AWS:SecretAccessKey"];
    return new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey);
});



builder.Services.AddScoped<IUploadProvider, S3UploadProvider>();
// Add services to the container.
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => 
    {
        options.DetailedErrors = builder.Environment.IsDevelopment();
    });

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped<IUserRepository, EfRepo>();
builder.Services.AddScoped<IBlogRepository, EfBlogRepo>();

builder.Services.AddScoped<IUserSignUpService, UserSignUpService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<ICreateBlogService,CreateBlogService>();
builder.Services.AddScoped<IBlogUpdateService, BlogUpdateService>();

builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(BlogMappingProfile));
builder.Services.AddSignalR(e => {
    e.MaximumReceiveMessageSize = 102400000;
});


Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion("8.0.35"))
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors());

builder.Services.AddBlazoredToast();

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