using System.Globalization;
using Microsoft.EntityFrameworkCore;
using xAzubiLog.Data;
using xAzubiLog.Models;

namespace xAzubiLog.Services;

public sealed class ReportBookService
{
    private static readonly CultureInfo GermanCulture = CultureInfo.GetCultureInfo("de-DE");
    private static readonly string[] DefaultCategories = ["Intern", "Extern", "Entwicklung", "Support", "Meeting", "Dokumentation"];
    private readonly IDbContextFactory<AzubiLog_BlazorContext> dbFactory;

    public ReportBookService(IDbContextFactory<AzubiLog_BlazorContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public async Task<ReportBookData> LoadAsync(DateTime selectedDate)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        var workContext = await EnsureWorkContextAsync(context, selectedDate);
        await EnsureDefaultCategoriesAsync(context, workContext.UserId);

        var startOfDay = selectedDate.Date;
        var endOfDay = startOfDay.AddDays(1);
        var startOfWeek = GetStartOfWeek(selectedDate);
        var endOfWeek = startOfWeek.AddDays(7);

        var categories = await context.Kategorien
            .AsNoTracking()
            .Where(c => c.BenutzerId == workContext.UserId)
            .OrderBy(c => c.Reihenfolge)
            .ThenBy(c => c.Name)
            .ToListAsync();

        var dailyEntries = await context.BerichtEintraege
            .AsNoTracking()
            .Include(e => e.Kategorie)
            .Include(e => e.Ausbilder)
            .Where(e => e.BenutzerId == workContext.UserId && e.Datum >= startOfDay && e.Datum < endOfDay)
            .OrderBy(e => e.Startzeit)
            .ThenBy(e => e.Titel)
            .ToListAsync();

        var weeklyEntries = await context.BerichtEintraege
            .AsNoTracking()
            .Include(e => e.Kategorie)
            .Where(e => e.BenutzerId == workContext.UserId && e.Datum >= startOfWeek && e.Datum < endOfWeek)
            .OrderBy(e => e.Datum)
            .ThenBy(e => e.Startzeit)
            .ToListAsync();

        return new ReportBookData(categories, dailyEntries, weeklyEntries);
    }

    public async Task<BerichtEintraege?> GetEntryAsync(int entryId)
    {
        await using var context = await dbFactory.CreateDbContextAsync();

        return await context.BerichtEintraege
            .AsNoTracking()
            .Include(e => e.Ausbilder)
            .FirstOrDefaultAsync(e => e.Id == entryId);
    }

    public async Task<int> SaveEntryAsync(BerichtEintraege entry, string? trainerName, string? newCategoryName, string? newCategoryColor)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        var workContext = await EnsureWorkContextAsync(context, entry.Datum);

        entry.BenutzerId = workContext.UserId;
        entry.WochenberichtId = workContext.WeekReportId;
        entry.KategorieId = await ResolveCategoryIdAsync(context, workContext.UserId, entry.KategorieId, newCategoryName, newCategoryColor);
        entry.AusbilderId = await ResolveTrainerIdAsync(context, trainerName);
        entry.Datum = entry.Datum.Date;
        entry.Dauer = CalculateDuration(entry.Startzeit, entry.Endzeit);
        entry.Status = string.IsNullOrWhiteSpace(entry.Status) ? "Entwurf" : entry.Status;
        entry.Tagestyp = string.IsNullOrWhiteSpace(entry.Tagestyp) ? "Betrieb" : entry.Tagestyp;

        if (entry.Id == 0)
        {
            entry.ErstelltAm = DateTime.Now;
            entry.GeändertAm = DateTime.Now;
            context.BerichtEintraege.Add(entry);
        }
        else
        {
            var existing = await context.BerichtEintraege.FirstAsync(e => e.Id == entry.Id);
            existing.AusbilderId = entry.AusbilderId;
            existing.KategorieId = entry.KategorieId;
            existing.WochenberichtId = entry.WochenberichtId;
            existing.BenutzerId = entry.BenutzerId;
            existing.Datum = entry.Datum;
            existing.Startzeit = entry.Startzeit;
            existing.Endzeit = entry.Endzeit;
            existing.Dauer = entry.Dauer;
            existing.Titel = entry.Titel.Trim();
            existing.Beschreibung = entry.Beschreibung.Trim();
            existing.Auftragsnummer = entry.Auftragsnummer;
            existing.Notiz = entry.Notiz;
            existing.Tagestyp = entry.Tagestyp;
            existing.Status = entry.Status;
            existing.GeändertAm = DateTime.Now;
        }

        await context.SaveChangesAsync();
        return entry.Id;
    }

    public async Task DeleteEntryAsync(int entryId)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        var entry = await context.BerichtEintraege.FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry is null) return;

        context.BerichtEintraege.Remove(entry);
        await context.SaveChangesAsync();
    }

    public BerichtEintraege CreateDraftEntry(DateTime selectedDate)
    {
        var start = selectedDate.Date.AddHours(Math.Max(8, DateTime.Now.Hour));

        return new BerichtEintraege
        {
            Datum = selectedDate.Date,
            Startzeit = start,
            Endzeit = start.AddHours(1),
            Dauer = 1,
            Tagestyp = "Betrieb",
            Status = "Entwurf",
            Titel = "",
            Beschreibung = "",
            Notiz = ""
        };
    }

    public static decimal CalculateDuration(DateTime start, DateTime end)
    {
        return end <= start ? 0 : Math.Round((decimal)(end - start).TotalHours, 2);
    }

    public static DateTime GetStartOfWeek(DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }

    private static int GetCalendarWeek(DateTime date)
    {
        return GermanCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    private static async Task EnsureDefaultCategoriesAsync(AzubiLog_BlazorContext context, int userId)
    {
        var existingNames = await context.Kategorien
            .Where(c => c.BenutzerId == userId)
            .Select(c => c.Name)
            .ToListAsync();

        var missing = DefaultCategories
            .Where(name => !existingNames.Contains(name))
            .Select((name, i) => new Kategorie
            {
                BenutzerId = userId,
                Name = name,
                FarbeHex = "#334155",
                Reihenfolge = existingNames.Count + i + 1
            }).ToList();

        if (missing.Any())
        {
            context.Kategorien.AddRange(missing);
            await context.SaveChangesAsync();
        }
    }

    private static async Task<WorkContext> EnsureWorkContextAsync(AzubiLog_BlazorContext context, DateTime date)
    {
        var user = await context.Users.OrderBy(u => u.ID).FirstOrDefaultAsync();

        if (user is null)
        {
            user = new User
            {
                Vorname = "Demo",
                Nachname = "Azubi",
                Email = "demo@azubilog.local",
                Aktiv = true
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var week = GetCalendarWeek(date);

        var report = await context.Wochenberichte
            .FirstOrDefaultAsync(r => r.BenutzerId == user.ID && r.Jahr == date.Year && r.Kalenderwoche == week);

        if (report is null)
        {
            report = new Wochenbericht
            {
                BenutzerId = user.ID,
                Kalenderwoche = week,
                Jahr = date.Year,
                Status = "Entwurf"
            };

            context.Wochenberichte.Add(report);
            await context.SaveChangesAsync();
        }

        return new WorkContext(user.ID, report.Id);
    }

    private static async Task<int?> ResolveCategoryIdAsync(AzubiLog_BlazorContext context, int userId, int? selectedId, string? name, string? color)
    {
        if (string.IsNullOrWhiteSpace(name)) return selectedId;

        var existing = await context.Kategorien
            .FirstOrDefaultAsync(c => c.BenutzerId == userId && c.Name == name);

        if (existing != null) return existing.ID;

        var category = new Kategorie
        {
            BenutzerId = userId,
            Name = name,
            FarbeHex = color ?? "#334155"
        };

        context.Kategorien.Add(category);
        await context.SaveChangesAsync();

        return category.ID;
    }

    private static async Task<int?> ResolveTrainerIdAsync(AzubiLog_BlazorContext context, string? trainerName)
    {
        if (string.IsNullOrWhiteSpace(trainerName)) return null;

        var trainer = await context.Ausbilder.FirstOrDefaultAsync(a => a.Name == trainerName);
        if (trainer != null) return trainer.ID;

        trainer = new Ausbilder { Name = trainerName };
        context.Ausbilder.Add(trainer);
        await context.SaveChangesAsync();

        return trainer.ID;
    }

    private readonly record struct WorkContext(int UserId, int WeekReportId);
}

public sealed record ReportBookData(
    IReadOnlyList<Kategorie> Categories,
    IReadOnlyList<BerichtEintraege> DailyEntries,
    IReadOnlyList<BerichtEintraege> WeeklyEntries);