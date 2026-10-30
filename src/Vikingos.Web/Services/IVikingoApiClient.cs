using Vikingos.Web.Models;

namespace Vikingos.Web.Services;

public interface IVikingoApiClient
{
    Task<IReadOnlyList<VikingoResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<VikingoResponse> CreateAsync(VikingoFormModel formulario, CancellationToken cancellationToken = default);

    Task<VikingoResponse> UpdateAsync(Guid id, VikingoFormModel formulario, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
