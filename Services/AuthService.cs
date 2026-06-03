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

        public AuthService(IDbContextFactory<xAzubiLogContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            using var db = _dbFactory.CreateDbContext();
            var user = await db.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswortHash)) return null;
            _currentUser = user;
            return user;
        }

        public async Task<bool> RestoreSessionAsync(int userId)
        {
            using var db = _dbFactory.CreateDbContext();
            var user = await db.User.FirstOrDefaultAsync(u => u.ID == userId);
            if (user == null) return false;
            _currentUser = user;
            return true;
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}