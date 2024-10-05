using HiHoHuBlog.Components;
using HiHoHuBlog.Modules.User.Biz;
using HiHoHuBlog.Modules.User.Data;

namespace HiHoHuBlog;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register your services here
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddScoped<IUserSignUpStore, DummyData>();
        services.AddScoped<IUserSignUpBiz, UserSignUpBiz>();
    }

    public void Configure(WebApplication app)
    {
        // Configure the HTTP request pipeline here
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
    }
}