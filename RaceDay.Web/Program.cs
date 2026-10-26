using RaceDay.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Register MVC controllers and views
builder.Services.AddControllersWithViews();

// Register memory cache for session storage
builder.Services.AddDistributedMemoryCache();

// Configure session-based state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register HTTP client for communication with RaceDay API
builder.Services.AddHttpClient<RaceDayApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]
        ?? "https://localhost:7001/");
});

// Build the application
var app = builder.Build();

// Configure error handling and security
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session before authorization
app.UseSession();

app.UseAuthorization();

// Configure default MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
