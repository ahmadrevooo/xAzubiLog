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
        public DbSet<BerichtEintrag> BerichtEintrag { get; set; } = default!;
        public DbSet<Kategorie> Kategorie { get; set; } = default!;
        public DbSet<Ausbilder> Ausbilder { get; set; } = default!;
    }
}