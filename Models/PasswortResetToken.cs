namespace xAzubiLog.Models
{
    public class PasswortResetToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = "";
        public int UserId { get; set; }
        public DateTime Ablauf { get; set; }
        public bool Verwendet { get; set; } = false;
    }
}