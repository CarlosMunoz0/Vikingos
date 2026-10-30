using Vikingos.Api.Models;
using Vikingos.Api.Services;

namespace Vikingos.Api.Tests;

public class ValhallaClassifierTests
{
    private readonly ValhallaClassifier _clasificador = new();

    [Fact]
    public void Clasifica_como_Thor_cuando_hay_40_batallas_o_mas()
    {
        var resultado = _clasificador.Clasificar(40, NivelHonor.Bajo);

        Assert.Equal(CategoriaGuerrero.Thor, resultado.Categoria);
        Assert.Equal(400, resultado.ValhallaPoints);
    }

    [Fact]
    public void Clasifica_como_Odin_cuando_el_honor_es_alto_y_hay_menos_de_40_batallas()
    {
        var resultado = _clasificador.Clasificar(39, NivelHonor.Alto);

        Assert.Equal(CategoriaGuerrero.Odin, resultado.Categoria);
        Assert.Equal(470, resultado.ValhallaPoints);
    }

    [Fact]
    public void Clasifica_como_Freyja_cuando_no_alcanza_fuerza_ni_honor_alto()
    {
        var resultado = _clasificador.Clasificar(39, NivelHonor.Medio);

        Assert.Equal(CategoriaGuerrero.Freyja, resultado.Categoria);
        Assert.Equal(430, resultado.ValhallaPoints);
        Assert.False(resultado.ListoParaElBanquete);
    }

    [Fact]
    public void Aprueba_el_banquete_con_honor_alto_y_al_menos_150_puntos()
    {
        var resultado = _clasificador.Clasificar(7, NivelHonor.Alto);

        Assert.Equal(150, resultado.ValhallaPoints);
        Assert.Equal(CategoriaGuerrero.Odin, resultado.Categoria);
        Assert.True(resultado.ListoParaElBanquete);
    }

    [Fact]
    public void Rechaza_el_banquete_si_el_honor_no_es_alto_aunque_los_puntos_superen_150()
    {
        var resultado = _clasificador.Clasificar(80, NivelHonor.Bajo);

        Assert.Equal(800, resultado.ValhallaPoints);
        Assert.Equal(CategoriaGuerrero.Thor, resultado.Categoria);
        Assert.False(resultado.ListoParaElBanquete);
    }
}
