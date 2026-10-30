using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Vikingos.Web.Models;

namespace Vikingos.Web.Services;

public sealed class VikingoApiClient : IVikingoApiClient
{
    private readonly HttpClient _httpClient;

    public VikingoApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IReadOnlyList<VikingoResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var response = await _httpClient.GetAsync("api/vikingos", cancellationToken);
            await EnsureSuccess(response);
            var lista = await response.Content.ReadFromJsonAsync<List<VikingoResponse>>(ApiJson.Options, cancellationToken);
            return (IReadOnlyList<VikingoResponse>)(lista ?? []);
        });
    }

    public Task<VikingoResponse> CreateAsync(VikingoFormModel formulario, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var response = await _httpClient.PostAsJsonAsync("api/vikingos", ToRequest(formulario), ApiJson.Options, cancellationToken);
            await EnsureSuccess(response);
            return await ReadVikingo(response, cancellationToken);
        });
    }

    public Task<VikingoResponse> UpdateAsync(Guid id, VikingoFormModel formulario, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var response = await _httpClient.PutAsJsonAsync($"api/vikingos/{id}", ToRequest(formulario), ApiJson.Options, cancellationToken);
            await EnsureSuccess(response);
            return await ReadVikingo(response, cancellationToken);
        });
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(async () =>
        {
            var response = await _httpClient.DeleteAsync($"api/vikingos/{id}", cancellationToken);
            await EnsureSuccess(response);
            return true;
        });
    }

    private static async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (VikingoApiException)
        {
            throw;
        }
        catch (JsonException)
        {
            throw new VikingoApiException("La respuesta del servicio no tiene el formato esperado.");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new VikingoApiException("No se pudo conectar con el servicio. Comprueba que la API esté en ejecución.");
        }
    }

    private static async Task EnsureSuccess(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new VikingoApiException("Ese vikingo ya no está en la lista.", notFound: true);
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new VikingoApiException(await LeerMensajeDeValidacion(response) ?? "Revisa los datos del formulario.");
        }

        throw new VikingoApiException($"El servicio respondió con un error ({(int)response.StatusCode}).");
    }

    private static async Task<string?> LeerMensajeDeValidacion(HttpResponseMessage response)
    {
        try
        {
            var problema = await response.Content.ReadFromJsonAsync<ValidationProblemBody>(ApiJson.Options);
            var mensajes = problema?.Errors?
                .SelectMany(item => item.Value)
                .Where(mensaje => !string.IsNullOrWhiteSpace(mensaje))
                .ToArray();

            if (mensajes is { Length: > 0 })
            {
                return string.Join(" ", mensajes);
            }

            return string.IsNullOrWhiteSpace(problema?.Title) ? null : problema.Title;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static async Task<VikingoResponse> ReadVikingo(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var vikingo = await response.Content.ReadFromJsonAsync<VikingoResponse>(ApiJson.Options, cancellationToken);
        if (vikingo is null)
        {
            throw new VikingoApiException("La respuesta del servicio no tiene el formato esperado.");
        }

        return vikingo;
    }

    private static VikingoRequestBody ToRequest(VikingoFormModel formulario)
    {
        return new VikingoRequestBody(
            formulario.Nombre.Trim(),
            formulario.BatallasGanadas,
            ParseEnum<ArmaFavorita>(formulario.ArmaFavorita, "arma"),
            ParseEnum<NivelHonor>(formulario.NivelHonor, "nivel de honor"),
            ParseEnum<CausaMuerte>(formulario.CausaMuerte, "causa de muerte"));
    }

    private static TEnum ParseEnum<TEnum>(string? value, string campo) where TEnum : struct, Enum
    {
        if (value is not null && Enum.TryParse<TEnum>(value, out var parsed))
        {
            return parsed;
        }

        throw new VikingoApiException($"Elige un {campo} válido.");
    }

    private sealed record VikingoRequestBody(
        string Nombre,
        int BatallasGanadas,
        ArmaFavorita ArmaFavorita,
        NivelHonor NivelHonor,
        CausaMuerte CausaMuerte);

    private sealed class ValidationProblemBody
    {
        public string? Title { get; set; }

        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
