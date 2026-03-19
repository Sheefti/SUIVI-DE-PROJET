namespace Ymmo.API.Models;

public class Agence
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;

    // Relations
    public ICollection<Utilisateur> Utilisateurs { get; set; } = new List<Utilisateur>();
    public ICollection<Bien> Biens { get; set; } = new List<Bien>();
}