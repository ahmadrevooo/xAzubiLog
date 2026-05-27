using System.ComponentModel.DataAnnotations;

namespace xAzubiLog.Models;

public class StundenplanEintrag
{
    public int Id { get; set; }

    [Required]
    public int BenutzerId { get; set; }

    [Required]
    public int Wochentag { get; set; } // 0=Mo, 1=Di, 2=Mi, 3=Do, 4=Fr

    [Required]
    public string FachName { get; set; } = string.Empty;

    public int DauerMinuten { get; set; } = 90;
}