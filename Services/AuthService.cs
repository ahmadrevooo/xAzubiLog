using Microsoft.EntityFrameworkCore;
using xAzubiLog.Data;
using xAzubiLog.Models;

namespace xAzubiLog.Services
{
    public class AuthService
    {
        private readonly IDbContextFactory<xAzubiLogContext> _dbFactory;

        private User? _currentUser;
        public User? CurrentUser => _currentUser;

        public bool IsAuthenticated => _currentUser != null;

        // 🔥 EVENT für UI Updates (SEHR WICHTIG)
        public event Action? OnChange;

        private void Notify()
        {
            OnChange?.Invoke();
        }

        public AuthService(IDbContextFactory<xAzubiLogContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // ✅ LOGIN
        public async Task<User?> LoginAsync(string email, string password)
        {
            using var db = _dbFactory.CreateDbContext();

            var user = await db.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswortHash))
                return null;

            _currentUser = user;

            Notify(); // ✅ UI updaten

            return user;
        }

        // ✅ SESSION WIEDERHERSTELLEN
        public async Task<bool> RestoreSessionAsync(int userId)
        {
            using var db = _dbFactory.CreateDbContext();

            var user = await db.User.FirstOrDefaultAsync(u => u.ID == userId);
            if (user == null) return false;

            _currentUser = user;

            Notify(); // ✅ UI updaten

            return true;
        }

        // ✅ LOGOUT
        public void Logout()
        {
            _currentUser = null;

            Notify(); // ✅ UI updaten
        }
    }
}