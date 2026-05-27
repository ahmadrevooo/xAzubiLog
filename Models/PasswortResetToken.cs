namespace xAzubiLog.Models
{
    public class PasswortResetToken
    {
        public int ID { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; } = "";

        public DateTime Ablauf { get; set; }
    }
}