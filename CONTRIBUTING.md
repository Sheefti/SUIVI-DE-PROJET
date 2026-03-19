# Guide de contribution — Ymmo

## Branches

- `main` → branche stable, **protégée**. On n'y pousse jamais directement.
- `develop` → branche d'intégration commune
- Toute nouvelle fonctionnalité se fait sur une branche dédiée :

| Type | Exemple |
|------|---------|
| Nouvelle fonctionnalité | `feat/liste-biens` |
| Correction de bug | `fix/calcul-prix` |
| Documentation | `docs/readme` |
| Refactoring | `refactor/service-biens` |

## Workflow

1. Créer ta branche depuis `develop`
2. Coder + committer
3. Ouvrir une Pull Request vers `develop`
4. Review par au moins 1 autre membre
5. Merge

## Convention de commits (Conventional Commits)

Format : `type: description courte en minuscules`

### Types autorisés

| Type | Usage |
|------|-------|
| `feat` | Nouvelle fonctionnalité |
| `fix` | Correction de bug |
| `docs` | Documentation uniquement |
| `refactor` | Restructuration sans changer le comportement |
| `test` | Ajout ou modification de tests |
| `chore` | Tâches techniques (config, dépendances...) |

### Exemples
```
feat: ajout de la recherche de biens par ville
fix: correction du calcul du prix au m²
docs: ajout du guide de contribution
refactor: restructuration du service d'authentification
chore: configuration de la CI GitHub Actions
```

### Règles

- Le message est en **français**
- Pas de majuscule au début de la description
- Pas de point à la fin
- Maximum 72 caractères