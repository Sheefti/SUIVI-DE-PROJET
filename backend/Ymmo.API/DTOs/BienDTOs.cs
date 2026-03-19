namespace Ymmo.API.DTOs;

public class CreerBienDTO
{
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public double Surface { get; set; }
    public int NombrePieces { get; set; }
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class ModifierBienDTO
{
    public string? Titre { get; set; }
    public string? Description { get; set; }
    public decimal? Prix { get; set; }
    public double? Surface { get; set; }
    public int? NombrePieces { get; set; }
    public string? Adresse { get; set; }
    public string? Ville { get; set; }
    public string? CodePostal { get; set; }
    public string? Type { get; set; }
    public string? Statut { get; set; }
}

public class BienResponseDTO
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
    public string Type { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string NomAgence { get; set; } = string.Empty;
}