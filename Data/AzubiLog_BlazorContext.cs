using Microsoft.EntityFrameworkCore;
using xAzubiLog.Models;

namespace xAzubiLog.Data
{
    public class AzubiLog_BlazorContext : DbContext
    {
        public AzubiLog_BlazorContext(DbContextOptions<AzubiLog_BlazorContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Wochenbericht> Wochenberichte { get; set; } = default!;
        public DbSet<BerichtEintraege> BerichtEintraege { get; set; } = default!;
        public DbSet<Kategorie> Kategorien { get; set; } = default!;
        public DbSet<Ausbilder> Ausbilder { get; set; } = default!;
        public DbSet<PasswortResetToken> PasswortResetTokens { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Wochenbericht>().ToTable("Wochenbericht");
            modelBuilder.Entity<BerichtEintraege>().ToTable("BerichtEintraege");
            modelBuilder.Entity<Kategorie>().ToTable("Kategorie");
            modelBuilder.Entity<Ausbilder>().ToTable("Ausbilder");
        }
    }
}