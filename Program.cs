using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using xAzubiLog.Data;
using xAzubiLog.Components;
using xAzubiLog.Services;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load("/var/www/xazubilog/publish/.env");
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys")));

// Datenbank
builder.Services.AddDbContextFactory<xAzubiLog.Data.xAzubiLogContext>(options =>
    options.UseSqlite("Data Source=xAzubiLog.db"));
builder.Services.AddScoped<xAzubiLog.Services.AuthService>();

builder.Services.AddScoped<xAzubiLog.Services.ReportBookService>();

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<xAzubiLog.Services.EmailService>();
builder.Services.AddScoped<xAzubiLog.Services.ThemeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
