using Amazon;
using Amazon.Internal;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using Blazored.Toast;
using HiHoHuBlog;
using HiHoHuBlog.Components;
using HiHoHuBlog.Modules.Admin;
using HiHoHuBlog.Modules.Admin.Repository.Implementation;
using HiHoHuBlog.Modules.Admin.Repository.Interface;
using HiHoHuBlog.Modules.Admin.Service.Implementation;
using HiHoHuBlog.Modules.Admin.Service.Interface;
using HiHoHuBlog.Modules.Blog;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Repository.Implementation;
using HiHoHuBlog.Modules.Blog.Service.Implementation;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Modules.Search;
using HiHoHuBlog.Modules.Search.Repository.Implementation;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Modules.Search.Service.Implementation;
using HiHoHuBlog.Modules.Search.Service.Interface;
using HiHoHuBlog.Modules.Tag.Repository.Implementation;
using HiHoHuBlog.Modules.Tag.Repository.Interface;
using HiHoHuBlog.Modules.Tag.Service.Implementation;
using HiHoHuBlog.Modules.Tag.Service.Interface;
using HiHoHuBlog.Modules.User;
using HiHoHuBlog.Modules.User.Service;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Repository.Implementation;
using HiHoHuBlog.Modules.User.Service.Implementation;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;
using HiHoHuBlog.Utils.MailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Nest;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);




builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var awsAccessKeyId = configuration["AWS:AccessKeyId"];
    var awsSecretAccessKey = configuration["AWS:SecretAccessKey"];
    var region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);
    return new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey,region);
});

builder.Services.AddSingleton<IAmazonSimpleEmailService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var awsAccessKeyId = configuration["AWS:AccessKeyId"];
    var awsSecretAccessKey = configuration["AWS:SecretAccessKey"];
    var region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);
    return new AmazonSimpleEmailServiceClient(awsAccessKeyId, awsSecretAccessKey, region);
});

builder.Services.AddSingleton<EsClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new EsClient(configuration);
});

builder.Services.AddScoped<IMailSender, SesMailService>();
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
builder.Services.AddScoped<IBlogBlockedRepository,BlogBlockedRepository>();
builder.Services.AddScoped<IReasonBlogBlockRepository,ReasonBlogBlockRepository>();
builder.Services.AddScoped<ISearchBlogRepository,EsSearchBlogRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<IBlogTagRepository, EfBlogTagRepo>();
builder.Services.AddScoped<IMigrationRepository, EsMigrationRepository>();
builder.Services.AddScoped<ISearchTagRepository, EsSearchTagRepository>();
builder.Services.AddScoped<IUserBlogActionRepository, EfUserBlogActionRepo>();
builder.Services.AddScoped<ISearchUserRepository, EsSearchUserRepository>();

builder.Services.AddScoped<IUserSignUpService, UserSignUpService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<IBlogBlockedService,BlogBlockedService>();
builder.Services.AddScoped<IReasonBlogBlockService,ReasonBlogBlockService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserBlogActionService, UserBlogActionService>();
builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();
builder.Services.AddScoped<ISearchUserService, SearchUserService>();

builder.Services.AddScoped<ICreateBlogService,CreateBlogService>();
builder.Services.AddScoped<IBlogUpdateService, BlogUpdateService>();
builder.Services.AddScoped<IBlogGetService, BlogGetService>();
builder.Services.AddScoped<IBlogDeleteService, BlogDeleteService>();
builder.Services.AddScoped<IMigrationSearchDataService,MigrationSearchDataService>();
builder.Services.AddScoped<ISearchBlogService, SearchBlogService>();
builder.Services.AddScoped<ISearchTagService, SearchTagService>();

builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(BlogMappingProfile));
builder.Services.AddAutoMapper(typeof(AdminMappingProfile));
builder.Services.AddAutoMapper(typeof(SearchMappingProfile));


builder.Services.AddSignalR(e => {
    e.MaximumReceiveMessageSize = 102400000;
});


Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion("8.0.35"))
    // .LogTo(Console.WriteLine, LogLevel.Information)
    // .EnableSensitiveDataLogging()
    // .EnableDetailedErrors()
    );

builder.Services.AddBlazoredToast();


builder.Services.AddAuthentication(AuthConstant.Scheme)
    .AddCookie(AuthConstant.Scheme, options =>
    {
        options.Cookie.Name = AuthConstant.CookieName;
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.SlidingExpiration = true;
        
    });

builder.Services.AddCascadingAuthenticationState();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();