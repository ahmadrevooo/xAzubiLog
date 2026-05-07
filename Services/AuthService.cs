using Microsoft.EntityFrameworkCore;
using xAzubiLog.Data;
using xAzubiLog.Models;

namespace xAzubiLog.Services
{
    public class AuthService
    {
        private readonly AzubiLog_BlazorContext _db;

        public User? CurrentUser { get; private set; }

        public bool IsAuthenticated => CurrentUser != null;

        public AuthService(AzubiLog_BlazorContext db)
        {
            _db = db;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswortHash))
                return null;

            CurrentUser = user;
            return user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}