using Microsoft.EntityFrameworkCore;
using xAzubiLog.Data;
using xAzubiLog.Models;

namespace xAzubiLog.Services
{
    public class AuthService
    {
        private readonly IDbContextFactory<AzubiLog_BlazorContext> _dbFactory;
        private static User? _currentUser;

        public User? CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;

        public AuthService(IDbContextFactory<AzubiLog_BlazorContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            using var db = _dbFactory.CreateDbContext();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswortHash)) return null;
            _currentUser = user;
            return user;
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}