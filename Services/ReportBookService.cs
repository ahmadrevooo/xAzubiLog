using System.Globalization;
using Microsoft.EntityFrameworkCore;
using xAzubiLog.Data;
using xAzubiLog.Models;

namespace xAzubiLog.Services;

/// <summary>
/// Provides data access and small workflow helpers for the report book module.
/// </summary>
public sealed class ReportBookService
{
    private static readonly CultureInfo GermanCulture = CultureInfo.GetCultureInfo("de-DE");
    private static readonly string[] DefaultCategories = new[] { "Intern", "Extern", "Entwicklung", "Support", "Meeting", "Dokumentation" };
    private readonly IDbContextFactory<xAzubiLogContext> dbFactory;

    /// <summary>
    /// Creates a service instance with an EF Core context factory.
    /// </summary>
    public ReportBookService(IDbContextFactory<xAzubiLogContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    /// <summary>
    /// Loads all data required by the daily and weekly overview.
    /// </summary>
    public async Task<ReportBookData> LoadAsync(DateTime selectedDate)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        var workContext = await EnsureWorkContextAsync(context, selectedDate);
        await EnsureDefaultCategoriesAsync(context, workContext.UserId);

        var startOfDay = selectedDate.Date;
        var endOfDay = startOfDay.AddDays(1);
        var startOfWeek = GetStartOfWeek(selectedDate);
        var endOfWeek = startOfWeek.AddDays(7);

        var categories = await context.Kategorie
            .AsNoTracking()
            .Where(category => category.BenutzerId == workContext.UserId)
            .OrderBy(category => category.Reihenfolge)
            .ThenBy(category => category.Name)
            .ToListAsync();

        var dailyEntries = await context.BerichtEintrag
            .AsNoTracking()
            .Include(entry => entry.Kategorie)
            .Include(entry => entry.Ausbilder)
            .Where(entry => entry.BenutzerId == workContext.UserId && entry.Datum >= startOfDay && entry.Datum < endOfDay)
            .OrderBy(entry => entry.Startzeit)
            .ThenBy(entry => entry.Titel)
            .ToListAsync();

        var weeklyEntries = await context.BerichtEintrag
            .AsNoTracking()
            .Include(entry => entry.Kategorie)
            .Where(entry => entry.BenutzerId == workContext.UserId && entry.Datum >= startOfWeek && entry.Datum < endOfWeek)
            .OrderBy(entry => entry.Datum)
            .ThenBy(entry => entry.Startzeit)
            .ToListAsync();

        return new ReportBookData(categories, dailyEntries, weeklyEntries);
    }

    /// <summary>
    /// Loads one existing report entry for editing.
    /// </summary>
    public async Task<BerichtEintrag?> GetEntryAsync(int entryId)
    {
        await using var context = await dbFactory.CreateDbContextAsync();

        return await context.BerichtEintrag
            .AsNoTracking()
            .Include(entry => entry.Ausbilder)
            .FirstOrDefaultAsync(entry => entry.Id == entryId);
    }

    /// <summary>
    /// Inserts or updates a report entry and resolves optional category and trainer data.
    /// </summary>
    public async Task<int> SaveEntryAsync(BerichtEintrag entry, string? trainerName, string? newCategoryName, string? newCategoryColor)
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
            context.BerichtEintrag.Add(entry);
        }
        else
        {
            var existing = await context.BerichtEintrag.FirstAsync(item => item.Id == entry.Id);
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

    /// <summary>
    /// Deletes a report entry by id if it still exists.
    /// </summary>
    public async Task DeleteEntryAsync(int entryId)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        var entry = await context.BerichtEintrag.FirstOrDefaultAsync(item => item.Id == entryId);

        if (entry is null)
        {
            return;
        }

        context.BerichtEintrag.Remove(entry);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Creates a detached draft entry with useful defaults for the selected day.
    /// </summary>
    public BerichtEintrag CreateDraftEntry(DateTime selectedDate)
    {
        var start = selectedDate.Date.AddHours(Math.Max(8, DateTime.Now.Hour));

        return new BerichtEintrag
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

    /// <summary>
    /// Calculates worked hours from start and end time.
    /// </summary>
    public static decimal CalculateDuration(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
        {
            return 0;
        }

        return Math.Round((decimal)(endTime - startTime).TotalHours, 2);
    }

    /// <summary>
    /// Returns the Monday of the selected calendar week.
    /// </summary>
    public static DateTime GetStartOfWeek(DateTime selectedDate)
    {
        var difference = ((7 + (selectedDate.Date.DayOfWeek - DayOfWeek.Monday)) % 7);
        return selectedDate.Date.AddDays(-difference);
    }

    private static int GetCalendarWeek(DateTime selectedDate)
    {
        return GermanCulture.Calendar.GetWeekOfYear(selectedDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    private static async Task EnsureDefaultCategoriesAsync(xAzubiLogContext context, int userId)
    {
        var existingNames = await context.Kategorie
            .Where(category => category.BenutzerId == userId)
            .Select(category => category.Name)
            .ToListAsync();

        var missingCategories = DefaultCategories
            .Where(name => !existingNames.Contains(name))
            .Select((name, index) => new Kategorie
            {
                BenutzerId = userId,
                Name = name,
                FarbeHex = PickCategoryColor(name),
                Reihenfolge = existingNames.Count + index + 1
            })
            .ToList();

        if (missingCategories.Count == 0)
        {
            return;
        }

        context.Kategorie.AddRange(missingCategories);
        await context.SaveChangesAsync();
    }

    private static string PickCategoryColor(string name) => name switch
    {
        "Extern" => "#64748b",
        "Entwicklung" => "#2563eb",
        "Support" => "#0f766e",
        "Meeting" => "#7c3aed",
        "Dokumentation" => "#b45309",
        _ => "#334155"
    };

    private static async Task<WorkContext> EnsureWorkContextAsync(xAzubiLogContext context, DateTime selectedDate)
    {
        var user = await context.User.OrderBy(item => item.ID).FirstOrDefaultAsync();
        if (user is null)
        {
            user = new User
            {
                Vorname = "Demo",
                Nachname = "Azubi",
                Email = "demo@azubilog.local",
                Schule = "Berufsschule",
                Klasse = "Demo",
                Ausbildungsberuf = "Fachinformatik",
                Aktiv = true
            };
            context.User.Add(user);
            await context.SaveChangesAsync();
        }

        var week = GetCalendarWeek(selectedDate);
        var weekReport = await context.Wochenbericht
            .FirstOrDefaultAsync(report => report.BenutzerId == user.ID && report.Jahr == selectedDate.Year && report.Kalenderwoche == week);

        if (weekReport is null)
        {
            weekReport = new Wochenbericht
            {
                BenutzerId = user.ID,
                Kalenderwoche = week,
                Jahr = selectedDate.Year,
                Gesamtstunden = 0,
                Status = "Entwurf",
                Kommentar = ""
            };
            context.Wochenbericht.Add(weekReport);
            await context.SaveChangesAsync();
        }

        return new WorkContext(user.ID, weekReport.Id);
    }

    private static async Task<int?> ResolveCategoryIdAsync(
        xAzubiLogContext context,
        int userId,
        int? selectedCategoryId,
        string? newCategoryName,
        string? newCategoryColor)
    {
        if (string.IsNullOrWhiteSpace(newCategoryName))
        {
            return selectedCategoryId;
        }

        var categoryName = newCategoryName.Trim();
        var existing = await context.Kategorie
            .FirstOrDefaultAsync(category => category.BenutzerId == userId && category.Name == categoryName);

        if (existing is not null)
        {
            return existing.ID;
        }

        var nextOrder = await context.Kategorie.CountAsync(category => category.BenutzerId == userId) + 1;
        var category = new Kategorie
        {
            BenutzerId = userId,
            Name = categoryName,
            FarbeHex = string.IsNullOrWhiteSpace(newCategoryColor) ? "#334155" : newCategoryColor,
            Reihenfolge = nextOrder
        };

        context.Kategorie.Add(category);
        await context.SaveChangesAsync();

        return category.ID;
    }

    private static async Task<int?> ResolveTrainerIdAsync(xAzubiLogContext context, string? trainerName)
    {
        if (string.IsNullOrWhiteSpace(trainerName))
        {
            return null;
        }

        var normalizedName = trainerName.Trim();
        var trainer = await context.Ausbilder.FirstOrDefaultAsync(item => item.Name == normalizedName);

        if (trainer is not null)
        {
            return trainer.ID;
        }

        trainer = new Ausbilder
        {
            Name = normalizedName
        };

        context.Ausbilder.Add(trainer);
        await context.SaveChangesAsync();

        return trainer.ID;
    }

    private readonly record struct WorkContext(int UserId, int WeekReportId);
}

/// <summary>
/// Groups the read data needed by the report book page.
/// </summary>
public sealed record ReportBookData(
    IReadOnlyList<Kategorie> Categories,
    IReadOnlyList<BerichtEintrag> DailyEntries,
    IReadOnlyList<BerichtEintrag> WeeklyEntries);
