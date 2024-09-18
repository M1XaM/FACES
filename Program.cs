using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using FACES.Repositories;
using FACES.Data;
using FACES.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

// for logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Adding db configuration,
// also dont forget to add credentials in launchSettings.js,
// also dont forget to migrate all of this 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// For generic repositories pattern
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// For email notification
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddTransient<IEmailService, SendGridEmailService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
