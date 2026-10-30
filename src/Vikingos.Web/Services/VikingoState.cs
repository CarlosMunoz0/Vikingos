using Vikingos.Web.Models;

namespace Vikingos.Web.Services;

public sealed class VikingoState
{
    private readonly IVikingoApiClient _cliente;

    public VikingoState(IVikingoApiClient cliente)
    {
        _cliente = cliente;
    }

    public event Action? OnChange;

    public IReadOnlyList<VikingoResponse> Vikingos { get; private set; } = [];

    public VikingoFormModel Formulario { get; private set; } = new();

    public int FormularioToken { get; private set; }

    public Guid? IdEnEdicion { get; private set; }

    public Guid? IdPendienteDeBorrar { get; private set; }

    public bool Cargando { get; private set; }

    public string? Error { get; private set; }

    public VikingoResponse? PendienteDeBorrar =>
        IdPendienteDeBorrar is Guid id
            ? Vikingos.FirstOrDefault(vikingo => vikingo.Id == id)
            : null;

    public async Task CargarAsync()
    {
        if (Cargando)
        {
            return;
        }

        Cargando = true;
        Error = null;
        Notify();

        try
        {
            Vikingos = await _cliente.GetAllAsync();
        }
        catch (VikingoApiException ex)
        {
            Error = ex.Message;
            Vikingos = [];
        }
        finally
        {
            Cargando = false;
            Notify();
        }
    }

    public async Task GuardarAsync()
    {
        if (Cargando)
        {
            return;
        }

        Cargando = true;
        Error = null;
        Notify();

        try
        {
            if (IdEnEdicion is Guid id)
            {
                var actualizado = await _cliente.UpdateAsync(id, Formulario);
                Vikingos = Vikingos.Select(vikingo => vikingo.Id == actualizado.Id ? actualizado : vikingo).ToList();
            }
            else
            {
                var creado = await _cliente.CreateAsync(Formulario);
                Vikingos = [.. Vikingos, creado];
            }

            ReiniciarFormulario();
        }
        catch (VikingoApiException ex)
        {
            Error = ex.Message;
            if (ex.NotFound)
            {
                await ResincronizarAsync();
            }
        }
        finally
        {
            Cargando = false;
            Notify();
        }
    }

    public void ComenzarEdicion(VikingoResponse vikingo)
    {
        IdEnEdicion = vikingo.Id;
        IdPendienteDeBorrar = null;
        Error = null;
        Formulario = new VikingoFormModel
        {
            Nombre = vikingo.Nombre,
            BatallasGanadas = vikingo.BatallasGanadas,
            ArmaFavorita = vikingo.ArmaFavorita.ToString(),
            NivelHonor = vikingo.NivelHonor.ToString(),
            CausaMuerte = vikingo.CausaMuerte.ToString()
        };
        FormularioToken++;
        Notify();
    }

    public void CancelarEdicion()
    {
        ReiniciarFormulario();
        Error = null;
        Notify();
    }

    public void PedirEliminacion(Guid id)
    {
        IdPendienteDeBorrar = id;
        Notify();
    }

    public void CancelarEliminacion()
    {
        IdPendienteDeBorrar = null;
        Notify();
    }

    public async Task ConfirmarEliminacionAsync()
    {
        if (IdPendienteDeBorrar is not Guid id || Cargando)
        {
            return;
        }

        Cargando = true;
        Error = null;
        Notify();

        try
        {
            await _cliente.DeleteAsync(id);
            Vikingos = Vikingos.Where(vikingo => vikingo.Id != id).ToList();
            IdPendienteDeBorrar = null;
            if (IdEnEdicion == id)
            {
                ReiniciarFormulario();
            }
        }
        catch (VikingoApiException ex)
        {
            Error = ex.Message;
            if (ex.NotFound)
            {
                await ResincronizarAsync();
                IdPendienteDeBorrar = null;
            }
        }
        finally
        {
            Cargando = false;
            Notify();
        }
    }

    private async Task ResincronizarAsync()
    {
        try
        {
            Vikingos = await _cliente.GetAllAsync();
        }
        catch (VikingoApiException)
        {
        }
    }

    private void ReiniciarFormulario()
    {
        IdEnEdicion = null;
        Formulario = new VikingoFormModel();
        FormularioToken++;
    }

    private void Notify() => OnChange?.Invoke();
}
