namespace Ymmo.API.Models;

public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool EstPrincipale { get; set; } = false;

    // Relations
    public int BienId { get; set; }
    public Bien Bien { get; set; } = null!;
}