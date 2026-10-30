using Vikingos.Api.Models;

namespace Vikingos.Api.Contracts;

public sealed class VikingoResponse
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int BatallasGanadas { get; set; }

    public ArmaFavorita ArmaFavorita { get; set; }

    public NivelHonor NivelHonor { get; set; }

    public CausaMuerte CausaMuerte { get; set; }

    public int ValhallaPoints { get; set; }

    public CategoriaGuerrero Categoria { get; set; }

    public bool ListoParaElBanquete { get; set; }
}
