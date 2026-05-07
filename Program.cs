using Microsoft.EntityFrameworkCore;
using xAzubiLog.Data;
using xAzubiLog.Models;
using xAzubiLog.Services;
using xAzubiLog.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AzubiLog_BlazorContext>(options =>
{
    options.UseSqlite("Data Source=xAzubiLog.db");
    options.ConfigureWarnings(w =>
        w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzubiLog_BlazorContext>();
    db.Database.Migrate();
    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Vorname = "Admin",
            Nachname = "Test",
            Email = "admin@test.de",
            PasswortHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Rolle = "Admin",
            Aktiv = true
        });
        db.SaveChanges();
    }
}

app.Run();