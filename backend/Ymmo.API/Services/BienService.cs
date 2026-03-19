using Microsoft.EntityFrameworkCore;
using Ymmo.API.Data;
using Ymmo.API.DTOs;
using Ymmo.API.Models;

namespace Ymmo.API.Services;

public class BienService
{
    private readonly AppDbContext _context;

    public BienService(AppDbContext context)
    {
        _context = context;
    }

    // ─── Récupérer tous les biens ─────────────────────
    public async Task<List<BienResponseDTO>> GetAll(string? ville = null, string? type = null, decimal? prixMax = null)
    {
        var query = _context.Biens
            .Include(b => b.Agence)
            .AsQueryable();

        if (!string.IsNullOrEmpty(ville))
            query = query.Where(b => b.Ville.ToLower().Contains(ville.ToLower()));

        if (!string.IsNullOrEmpty(type) && Enum.TryParse<TypeBien>(type, out var typeBien))
            query = query.Where(b => b.Type == typeBien);

        if (prixMax.HasValue)
            query = query.Where(b => b.Prix <= prixMax.Value);

        return await query.Select(b => ToDTO(b)).ToListAsync();
    }

    // ─── Récupérer un bien par ID ──────────────────────
    public async Task<BienResponseDTO?> GetById(int id)
    {
        var bien = await _context.Biens
            .Include(b => b.Agence)
            .FirstOrDefaultAsync(b => b.Id == id);

        return bien == null ? null : ToDTO(bien);
    }

    // ─── Créer un bien ────────────────────────────────
    public async Task<BienResponseDTO?> Create(CreerBienDTO dto, int agenceId)
    {
        if (!Enum.TryParse<TypeBien>(dto.Type, out var type))
            return null;

        var bien = new Bien
        {
            Titre = dto.Titre,
            Description = dto.Description,
            Prix = dto.Prix,
            Surface = dto.Surface,
            NombrePieces = dto.NombrePieces,
            Adresse = dto.Adresse,
            Ville = dto.Ville,
            CodePostal = dto.CodePostal,
            Type = type,
            AgenceId = agenceId
        };

        _context.Biens.Add(bien);
        await _context.SaveChangesAsync();

        await _context.Entry(bien).Reference(b => b.Agence).LoadAsync();
        return ToDTO(bien);
    }

    // ─── Modifier un bien ─────────────────────────────
    public async Task<BienResponseDTO?> Update(int id, ModifierBienDTO dto, int agenceId, string role)
    {
        var bien = await _context.Biens
            .Include(b => b.Agence)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bien == null) return null;

        // Un commercial ne peut modifier que les biens de son agence
        if (role == "Commercial" && bien.AgenceId != agenceId)
            return null;

        if (dto.Titre != null) bien.Titre = dto.Titre;
        if (dto.Description != null) bien.Description = dto.Description;
        if (dto.Prix.HasValue) bien.Prix = dto.Prix.Value;
        if (dto.Surface.HasValue) bien.Surface = dto.Surface.Value;
        if (dto.NombrePieces.HasValue) bien.NombrePieces = dto.NombrePieces.Value;
        if (dto.Adresse != null) bien.Adresse = dto.Adresse;
        if (dto.Ville != null) bien.Ville = dto.Ville;
        if (dto.CodePostal != null) bien.CodePostal = dto.CodePostal;

        if (dto.Type != null && Enum.TryParse<TypeBien>(dto.Type, out var type))
            bien.Type = type;

        if (dto.Statut != null && Enum.TryParse<StatutBien>(dto.Statut, out var statut))
            bien.Statut = statut;

        await _context.SaveChangesAsync();
        return ToDTO(bien);
    }

    // ─── Supprimer un bien ────────────────────────────
    public async Task<bool> Delete(int id, int agenceId, string role)
    {
        var bien = await _context.Biens.FindAsync(id);
        if (bien == null) return false;

        // Un commercial ne peut supprimer que les biens de son agence
        if (role == "Commercial" && bien.AgenceId != agenceId)
            return false;

        _context.Biens.Remove(bien);
        await _context.SaveChangesAsync();
        return true;
    }

    // ─── Mapping Model → DTO ──────────────────────────
    private static BienResponseDTO ToDTO(Bien b) => new()
    {
        Id = b.Id,
        Titre = b.Titre,
        Description = b.Description,
        Prix = b.Prix,
        Surface = b.Surface,
        NombrePieces = b.NombrePieces,
        Adresse = b.Adresse,
        Ville = b.Ville,
        CodePostal = b.CodePostal,
        Type = b.Type.ToString(),
        Statut = b.Statut.ToString(),
        CreatedAt = b.CreatedAt,
        NomAgence = b.Agence?.Nom ?? string.Empty
    };
}