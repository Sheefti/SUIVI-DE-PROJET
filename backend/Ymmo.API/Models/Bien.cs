namespace Ymmo.API.Models;

public enum TypeBien
{
    Appartement,
    Maison,
    Bureau,
    Commerce,
    Terrain
}

public enum StatutBien
{
    Disponible,
    SousCompromis,
    Vendu
}

public class Bien
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public double Surface { get; set; }
    public int NombrePieces { get; set; }
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
    public TypeBien Type { get; set; }
    public StatutBien Statut { get; set; } = StatutBien.Disponible;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relations
    public int AgenceId { get; set; }
    public Agence Agence { get; set; } = null!;
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
}