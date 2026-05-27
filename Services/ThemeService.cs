namespace xAzubiLog.Services
{
    public class ThemeService
    {
        public bool IsDark { get; private set; }

        public event Action? OnChange;

        public async Task InitializeAsync(bool isDark)
        {
            IsDark = isDark;
            await Task.CompletedTask;
        }

        public void Toggle()
        {
            IsDark = !IsDark;
            OnChange?.Invoke();
        }

    }
}
