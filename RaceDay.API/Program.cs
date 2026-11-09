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

// 6. Register the Azure Blob Storage service
builder.Services.AddScoped<BlobStorageService>();

// 7. Enable controllers
builder.Services.AddControllers();

// 8. Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 9. Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 10. Enable HTTPS
app.UseHttpsRedirection();

// 11. Enable sessions
app.UseSession();

// 12. Enable authorization middleware
app.UseAuthorization();

// 13. Map API controllers
app.MapControllers();

app.Run();
