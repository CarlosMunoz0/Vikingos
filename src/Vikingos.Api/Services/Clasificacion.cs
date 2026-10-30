using Vikingos.Api.Models;

namespace Vikingos.Api.Services;

public readonly record struct Clasificacion(
    int ValhallaPoints,
    CategoriaGuerrero Categoria,
    bool ListoParaElBanquete);
