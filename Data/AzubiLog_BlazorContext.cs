using Microsoft.EntityFrameworkCore;
using xAzubiLog.Models;

namespace xAzubiLog.Data
{
    public class xAzubiLogContext : DbContext
    {
        public xAzubiLogContext(DbContextOptions<xAzubiLogContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Wochenbericht> Wochenbericht { get; set; } = default!;
        public DbSet<BerichtEintraege> BerichtEintraege { get; set; } = default!;

        // Compatibility wrappers for older code that referenced pluralized DbSet names
        public IQueryable<User> Users => User;
        public IQueryable<Wochenbericht> Wochenberichte => Wochenbericht;
        public DbSet<Kategorie> Kategorie { get; set; } = default!;
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