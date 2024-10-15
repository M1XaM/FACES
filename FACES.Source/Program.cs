using FACES.Repositories;
using FACES.Data;
using FACES.Models;

// For db
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;

// For JWT auth
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// For key management (CSRF for docker)
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection;

// For HTTPS
using Microsoft.AspNetCore.HttpOverrides;

namespace FACES;
public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));

        // For bussines logic layer
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddTransient<IEmailService, EmailService>();

        // For data access layer
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IUserProjectRepository, UserProjectRepository>();
        builder.Services.AddScoped<IProjectClientRepository, ProjectClientRepository>();

        builder.Services.AddControllersWithViews();

        // For logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Load connection string from environment variable if available
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection") 
            ?? builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        // For JWT authorization
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "FACES",
                ValidAudience = "FACES",
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] 
                ?? throw new InvalidOperationException("JWT Key is not configured. Please set the 'Jwt:Key' in the configuration."))),
            };
        });

        builder.Services.AddAuthorization();

        // Docker is a bitch
        // Check if the application is running in a Docker container
        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
        {
            // Docker-specific configuration
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"/root/.aspnet/DataProtection-Keys"))
                .SetApplicationName("faces");
        }

        var app = builder.Build();

        // For automatic creation and migration of the db (for Docker)
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                // var entities = context.Model.GetEntityTypes();
                // foreach (var entity in entities)
                // {
                //     var tableName = entity.GetTableName();
                //     if (!string.IsNullOrEmpty(tableName))
                //     {
                //         context.Database.ExecuteSqlRaw($"TRUNCATE TABLE \"{tableName}\" RESTART IDENTITY CASCADE");
                //     }
                // }
                // context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // Configure forwarded headers for reverse proxy
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.Use(async (context, next) =>
        {
            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
            await next();
        });


        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}