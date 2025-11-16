using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CodeBidder.Data;
using System;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Logging configuration (for troubleshooting)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Register SQL Server DbContext with additional configurations
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions
            .CommandTimeout(120) // Command timeout set to 120 seconds
            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null) // Retry on transient failures
    )
    .LogTo(Console.WriteLine, LogLevel.Information) // Log SQL commands
    .EnableSensitiveDataLogging() // Enable sensitive data logging (for debugging only)
);

// Add support for controllers and views (MVC)
builder.Services.AddControllersWithViews();

// Enable session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Prevent JavaScript access to cookies
    options.Cookie.IsEssential = true; // Required for GDPR compliance
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Custom error handler in production
    app.UseHsts(); // Use HSTS in production for security
}
else
{
    app.UseDeveloperExceptionPage(); // Detailed error pages in development
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles(); // Serve static files (e.g., CSS, JS, images)

app.UseRouting();

// Add session middleware before using sessions in controllers
app.UseSession();

app.UseAuthorization(); // Authorization middleware

// Map default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
app.Run();
