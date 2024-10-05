using HiHoHuBlog;

var builder = WebApplication.CreateBuilder(args);

// Create an instance of Startup and call ConfigureServices
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Call the Configure method from Startup
startup.Configure(app);

app.Run();