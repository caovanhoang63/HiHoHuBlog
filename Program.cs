using HiHoHuBlog.Components;
using HiHoHuBlog.Modules.User.Biz;
using HiHoHuBlog.Modules.User.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped<IUserSignUpStore, DummyData>();
builder.Services.AddScoped<IUserLoginStore, DummyData>();

builder.Services.AddScoped<IUserSignUpBiz, UserSignUpBiz>();
builder.Services.AddScoped<IUserLoginBiz, UserLoginBiz>();

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