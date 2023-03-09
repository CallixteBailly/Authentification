using System.ComponentModel.DataAnnotations;

namespace Blazor.Models;
public record LoginRequest
{
    [Required(ErrorMessage = "L'adresse email est requise.")]
    [EmailAddress(ErrorMessage = "L'adresse email n'est pas valide.")]
    [StringLength(25, ErrorMessage = "L'adresse email ne peut pas dépasser 25 caractères.")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Mot de passe valide : \"Passw0rd!\"")]
    public string? Password { get; set; }
}
