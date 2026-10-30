using System.ComponentModel.DataAnnotations;
using Vikingos.Api.Models;

namespace Vikingos.Api.Contracts;

public sealed class VikingoRequest : IValidatableObject
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre no puede superar 80 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0, 10_000, ErrorMessage = "Las batallas deben estar entre 0 y 10.000.")]
    public int BatallasGanadas { get; set; }

    [Required(ErrorMessage = "Elige un arma.")]
    public ArmaFavorita? ArmaFavorita { get; set; }

    [Required(ErrorMessage = "Elige el nivel de honor.")]
    public NivelHonor? NivelHonor { get; set; }

    [Required(ErrorMessage = "Elige la causa de muerte.")]
    public CausaMuerte? CausaMuerte { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(Nombre) && Nombre.Trim().Length < 2)
        {
            yield return new ValidationResult(
                "El nombre debe tener al menos 2 caracteres.",
                [nameof(Nombre)]);
        }
    }
}
