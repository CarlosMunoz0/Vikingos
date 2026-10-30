using Vikingos.Api.Models;

namespace Vikingos.Api.Services;

public sealed class ValhallaClassifier : IValhallaClassifier
{
    public const int UmbralBatallasThor = 40;
    public const int UmbralPuntosBanquete = 150;

    public Clasificacion Clasificar(int batallasGanadas, NivelHonor nivelHonor)
    {
        var puntos = (batallasGanadas * 10) + PuntosDeHonor(nivelHonor);
        var categoria = ResolverCategoria(batallasGanadas, nivelHonor);
        var listoParaElBanquete = nivelHonor == NivelHonor.Alto && puntos >= UmbralPuntosBanquete;

        return new Clasificacion(puntos, categoria, listoParaElBanquete);
    }

    private static int PuntosDeHonor(NivelHonor nivelHonor) => nivelHonor switch
    {
        NivelHonor.Bajo => 0,
        NivelHonor.Medio => 40,
        NivelHonor.Alto => 80,
        _ => 0
    };

    private static CategoriaGuerrero ResolverCategoria(int batallasGanadas, NivelHonor nivelHonor)
    {
        if (batallasGanadas >= UmbralBatallasThor)
        {
            return CategoriaGuerrero.Thor;
        }

        if (nivelHonor == NivelHonor.Alto)
        {
            return CategoriaGuerrero.Odin;
        }

        return CategoriaGuerrero.Freyja;
    }
}
