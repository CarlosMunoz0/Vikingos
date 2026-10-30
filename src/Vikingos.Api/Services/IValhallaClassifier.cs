using Vikingos.Api.Models;

namespace Vikingos.Api.Services;

public interface IValhallaClassifier
{
    Clasificacion Clasificar(int batallasGanadas, NivelHonor nivelHonor);
}
