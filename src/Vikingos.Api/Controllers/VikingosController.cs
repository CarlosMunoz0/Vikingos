using Microsoft.AspNetCore.Mvc;
using Vikingos.Api.Contracts;
using Vikingos.Api.Models;
using Vikingos.Api.Repositories;
using Vikingos.Api.Services;

namespace Vikingos.Api.Controllers;

[ApiController]
[Route("api/vikingos")]
public class VikingosController : ControllerBase
{
    private readonly IVikingoRepository _repositorio;
    private readonly IValhallaClassifier _clasificador;

    public VikingosController(IVikingoRepository repositorio, IValhallaClassifier clasificador)
    {
        _repositorio = repositorio;
        _clasificador = clasificador;
    }

    [HttpGet]
    public ActionResult<IEnumerable<VikingoResponse>> GetAll()
    {
        return Ok(_repositorio.GetAll().Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<VikingoResponse> GetById(Guid id)
    {
        var vikingo = _repositorio.GetById(id);
        if (vikingo is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(vikingo));
    }

    [HttpPost]
    public ActionResult<VikingoResponse> Create(VikingoRequest request)
    {
        var creado = _repositorio.Add(ToEntity(Guid.Empty, request));
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, ToResponse(creado));
    }

    [HttpPut("{id:guid}")]
    public ActionResult<VikingoResponse> Update(Guid id, VikingoRequest request)
    {
        var actualizado = ToEntity(id, request);
        if (!_repositorio.Update(actualizado))
        {
            return NotFound();
        }

        return Ok(ToResponse(actualizado));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if (!_repositorio.Delete(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    private VikingoResponse ToResponse(Vikingo vikingo)
    {
        var clasificacion = _clasificador.Clasificar(vikingo.BatallasGanadas, vikingo.NivelHonor);

        return new VikingoResponse
        {
            Id = vikingo.Id,
            Nombre = vikingo.Nombre,
            BatallasGanadas = vikingo.BatallasGanadas,
            ArmaFavorita = vikingo.ArmaFavorita,
            NivelHonor = vikingo.NivelHonor,
            CausaMuerte = vikingo.CausaMuerte,
            ValhallaPoints = clasificacion.ValhallaPoints,
            Categoria = clasificacion.Categoria,
            ListoParaElBanquete = clasificacion.ListoParaElBanquete
        };
    }

    private static Vikingo ToEntity(Guid id, VikingoRequest request) => new()
    {
        Id = id,
        Nombre = request.Nombre.Trim(),
        BatallasGanadas = request.BatallasGanadas,
        ArmaFavorita = request.ArmaFavorita!.Value,
        NivelHonor = request.NivelHonor!.Value,
        CausaMuerte = request.CausaMuerte!.Value
    };
}
