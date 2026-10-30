namespace Vikingos.Api.Models;

public sealed class Vikingo
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int BatallasGanadas { get; set; }

    public ArmaFavorita ArmaFavorita { get; set; }

    public NivelHonor NivelHonor { get; set; }

    public CausaMuerte CausaMuerte { get; set; }
}
