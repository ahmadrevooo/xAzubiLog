using Microsoft.EntityFrameworkCore;
using xAzubiLog.Data;
using xAzubiLog.Components;
using xAzubiLog.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Datenbank
builder.Services.AddDbContextFactory<AzubiLog_BlazorContext>(options =>
    options.UseSqlite("Data Source=xAzubiLog.db"));
builder.Services.AddSingleton<xAzubiLog.Services.AuthService>();

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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