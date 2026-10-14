
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Enable session storage
builder.Services.AddDistributedMemoryCache();

// 2. Configure session management
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Allow access to the current HTTP context
builder.Services.AddHttpContextAccessor();

// 4. Connect to the RaceDay SQL Server database
builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RaceDayConnection")));

// 5. Register the authentication service
builder.Services.AddScoped<AuthService>();

// 6. Enable controllers
builder.Services.AddControllers();

// 7. Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 8. Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 9. Enable HTTPS
app.UseHttpsRedirection();

// 10. Enable sessions
app.UseSession();

// 11. Enable authorization middleware
app.UseAuthorization();

// 12. Map API controllers
app.MapControllers();

app.Run();
