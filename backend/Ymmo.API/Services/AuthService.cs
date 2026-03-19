using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ymmo.API.Data;
using Ymmo.API.DTOs;
using Ymmo.API.Models;

namespace Ymmo.API.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDTO?> Register(RegisterDTO dto)
    {
        // Vérifie si l'email existe déjà
        if (await _context.Utilisateurs.AnyAsync(u => u.Email == dto.Email))
            return null;

        // Parse le rôle
        if (!Enum.TryParse<Role>(dto.Role, out var role))
            return null;

        var utilisateur = new Utilisateur
        {
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Email = dto.Email,
            MotDePasseHash = BCrypt.Net.BCrypt.HashPassword(dto.MotDePasse),
            Role = role,
            AgenceId = dto.AgenceId
        };

        _context.Utilisateurs.Add(utilisateur);
        await _context.SaveChangesAsync();

        return GenerateResponse(utilisateur);
    }

    public async Task<AuthResponseDTO?> Login(LoginDTO dto)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (utilisateur == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.MotDePasse, utilisateur.MotDePasseHash))
            return null;

        return GenerateResponse(utilisateur);
    }

    private AuthResponseDTO GenerateResponse(Utilisateur utilisateur)
    {
        var token = GenerateJwtToken(utilisateur);
        return new AuthResponseDTO
        {
            Token = token,
            Nom = utilisateur.Nom,
            Prenom = utilisateur.Prenom,
            Email = utilisateur.Email,
            Role = utilisateur.Role.ToString()
        };
    }

    private string GenerateJwtToken(Utilisateur utilisateur)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
            new Claim(ClaimTypes.Email, utilisateur.Email),
            new Claim(ClaimTypes.Role, utilisateur.Role.ToString()),
            new Claim("agenceId", utilisateur.AgenceId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                double.Parse(_configuration["Jwt:ExpirationHours"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}