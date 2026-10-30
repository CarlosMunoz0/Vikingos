using Vikingos.Api.Models;

namespace Vikingos.Api.Repositories;

public sealed class InMemoryVikingoRepository : IVikingoRepository
{
    private readonly Lock _lock = new();
    private readonly List<Vikingo> _vikingos = [];

    public InMemoryVikingoRepository()
    {
        _vikingos.Add(Crear(Guid.Parse("6f1c0a2e-1b7a-4c1d-9a11-0a6d5e2b1001"), "Ragnar", 55, ArmaFavorita.Hacha, NivelHonor.Alto, CausaMuerte.BatallaEnCampoAbierto));
        _vikingos.Add(Crear(Guid.Parse("6f1c0a2e-1b7a-4c1d-9a11-0a6d5e2b1002"), "Astrid", 12, ArmaFavorita.Espada, NivelHonor.Alto, CausaMuerte.DueloUnoContraUno));
        _vikingos.Add(Crear(Guid.Parse("6f1c0a2e-1b7a-4c1d-9a11-0a6d5e2b1003"), "Floki", 8, ArmaFavorita.Martillo, NivelHonor.Medio, CausaMuerte.ProtegiendoSuAldea));
    }

    public IReadOnlyList<Vikingo> GetAll()
    {
        lock (_lock)
        {
            return _vikingos.Select(Clonar).ToList();
        }
    }

    public Vikingo? GetById(Guid id)
    {
        lock (_lock)
        {
            var vikingo = _vikingos.FirstOrDefault(item => item.Id == id);
            return vikingo is null ? null : Clonar(vikingo);
        }
    }

    public Vikingo Add(Vikingo vikingo)
    {
        var copia = Clonar(vikingo);
        copia.Id = Guid.NewGuid();

        lock (_lock)
        {
            _vikingos.Add(copia);
            return Clonar(copia);
        }
    }

    public bool Update(Vikingo vikingo)
    {
        lock (_lock)
        {
            var indice = _vikingos.FindIndex(item => item.Id == vikingo.Id);
            if (indice < 0)
            {
                return false;
            }

            _vikingos[indice] = Clonar(vikingo);
            return true;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var indice = _vikingos.FindIndex(item => item.Id == id);
            if (indice < 0)
            {
                return false;
            }

            _vikingos.RemoveAt(indice);
            return true;
        }
    }

    private static Vikingo Crear(
        Guid id,
        string nombre,
        int batallasGanadas,
        ArmaFavorita armaFavorita,
        NivelHonor nivelHonor,
        CausaMuerte causaMuerte) => new()
    {
        Id = id,
        Nombre = nombre,
        BatallasGanadas = batallasGanadas,
        ArmaFavorita = armaFavorita,
        NivelHonor = nivelHonor,
        CausaMuerte = causaMuerte
    };

    private static Vikingo Clonar(Vikingo vikingo) => new()
    {
        Id = vikingo.Id,
        Nombre = vikingo.Nombre,
        BatallasGanadas = vikingo.BatallasGanadas,
        ArmaFavorita = vikingo.ArmaFavorita,
        NivelHonor = vikingo.NivelHonor,
        CausaMuerte = vikingo.CausaMuerte
    };
}
