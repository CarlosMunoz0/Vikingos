using Vikingos.Api.Models;

namespace Vikingos.Api.Repositories;

public interface IVikingoRepository
{
    IReadOnlyList<Vikingo> GetAll();

    Vikingo? GetById(Guid id);

    Vikingo Add(Vikingo vikingo);

    bool Update(Vikingo vikingo);

    bool Delete(Guid id);
}
