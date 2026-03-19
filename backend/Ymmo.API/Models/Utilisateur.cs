namespace Ymmo.API.Models;

public enum Role
{
    Direction,
    Commercial,
    CommunicationMarketing,
    AdministratifRHJuridique,
    ITSupport
}

public class Utilisateur
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MotDePasseHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public int AgenceId { get; set; }
    public Agence Agence { get; set; } = null!;
}