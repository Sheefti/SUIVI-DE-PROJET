namespace Ymmo.API.DTOs;

public class RegisterDTO
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int AgenceId { get; set; }
}

public class LoginDTO
{
    public string Email { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty;
}

public class AuthResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}