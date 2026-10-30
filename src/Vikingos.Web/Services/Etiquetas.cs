using Vikingos.Web.Models;

namespace Vikingos.Web.Services;

public static class Etiquetas
{
    public const string Banquete = "¡Listo para el banquete en Valhalla!";

    public static string Arma(ArmaFavorita arma) => arma switch
    {
        ArmaFavorita.Hacha => "Hacha",
        ArmaFavorita.Espada => "Espada",
        ArmaFavorita.Martillo => "Martillo",
        _ => arma.ToString()
    };

    public static string Honor(NivelHonor honor) => honor switch
    {
        NivelHonor.Bajo => "Bajo",
        NivelHonor.Medio => "Medio",
        NivelHonor.Alto => "Alto",
        _ => honor.ToString()
    };

    public static string Causa(CausaMuerte causa) => causa switch
    {
        CausaMuerte.BatallaEnCampoAbierto => "Batalla en campo abierto",
        CausaMuerte.DueloUnoContraUno => "Duelo uno contra uno",
        CausaMuerte.ProtegiendoSuAldea => "Protegiendo su aldea",
        _ => causa.ToString()
    };

    public static string Categoria(CategoriaGuerrero categoria) => categoria switch
    {
        CategoriaGuerrero.Thor => "Guerrero de Thor",
        CategoriaGuerrero.Freyja => "Guerrero de Freyja",
        CategoriaGuerrero.Odin => "Guerrero de Odín",
        _ => categoria.ToString()
    };
}
