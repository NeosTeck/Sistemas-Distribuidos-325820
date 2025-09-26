using System.ServiceModel.Channels;
using Microsoft.AspNetCore.Mvc;
using PokedexApi.Dtos;
using PokedexApi.Mappers;
using PokedexApi.Models;
using PokedexApi.Services;
using PokedexApi.Expections;


namespace PokedexApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")] //el controler lee el nombre de la clase y quita el controoler y deja el pokemons
public class PokemonsController : ControllerBase
{
    private readonly IPokemonService _pokemonService;

    public PokemonsController(IPokemonService pokemonsService)
    {
        _pokemonService = pokemonsService;
    }

    //HTTP status 
    //200 - OK
    //404 - Not Found
    //400 - Bad Request (si el formato del id no es guid o incorrecto)
    //500 - Internal Server Error
    //localhost:PORT/api/v1/pokemons (Al hace un get siempre mandar un id par ala ruta)


    [HttpGet("{id}", Name = "GetPokemonByIdAsync")]
    public async Task<ActionResult<PokemonResponse>> GetPokemonByIdAsync(Guid id, CancellationToken cancellationToken)
    {   //El okey se traduce a un http 200 basicamente nos facilita para el estado
        var pokemon = await _pokemonService.GetPokemonByIdAsync(id, cancellationToken);
        return pokemon is null ? NotFound() : Ok(pokemon.ToResponse());
    }

    //localhost:PORT/api/v1/pokemons
    //debe recibir un body request - JSON {}
    //Http verb - post
    //400-Bad Request (si el body request no es valido)
    //409 - Conflict (si el pokemon ya existe)
    // 422 - Unprocessable Entity (Regla de negocio interna no se cumple)
    //500 - Internal Server Error   
    //200 - OK (El pokemon se creo mas el id) -- no sigue muy bien las reglas de REST pero es valido
    //201-Created (El pokemon creado + el id) -- sigue las reglas de REST porque devuelve un href para devolder el recurso
    //key: localhost:PORT/api/v1/pokemons/ aqui va el id
    //202 - Accepted (Procesamiento asincrono) no hace nada en base de datos
    [HttpPost]
    public async Task<ActionResult<PokemonResponse>> CreatePokemonAsync([FromBody] CreatePokemonRequest createPokemon, CancellationToken cancellationToken)
    {
        try
        {
            if (!InvalidAttack(createPokemon))
            {
                return BadRequest(new { Message = "No puede tener 0 de ataque" });
            }

            var pokemon = await _pokemonService.CreatePokemonAsync(createPokemon.ToModel(), cancellationToken);
            return CreatedAtRoute(nameof(GetPokemonByIdAsync),
                new { id = pokemon.Id }, pokemon.ToResponse());
        }

        catch (PokemonAlreadyExistsException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    private static bool InvalidAttack(CreatePokemonRequest createPokemon)
    {
        return createPokemon.Stats.Attack > 0;
    }

    //localhost:PORT/api/v1/pokemons/{id}
    //Llamar al servicio para eliminar el pokemon
    //Si no existe el pokemon devolver un 404 Not Found
    //Si se elimina correctamente devolver un 204 No Content
    //Si hay un error interno devolver un 500 Internal Server Error

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePokemonAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _pokemonService.DeletePokemonAsync(id, cancellationToken);
            return NoContent();

        }
        catch (PokemonNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static bool IsValidAttack(int attack)
    {
        return attack > 0;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePokemonAsync(Guid id, [FromBody] UpdatePokemonRequest pokemon, CancellationToken cancellationToken)
    {
        try
        {
            if (!IsValidAttack(pokemon.Stats.Attack))
            {
                return BadRequest(new { Message = "Invalid attack value" }); //400
            }

            await _pokemonService.UpdatePokemonAsync(pokemon.ToModel(id), cancellationToken);
            return NoContent(); // 204
        }
        catch (PokemonNotFoundException)
        {
            return NotFound(); // 404
        }
        catch (PokemonAlreadyExistsException ex)
        {
            return Conflict(new { Message = ex.Message }); // 409
        }
    }
    [HttpPatch("{id}")]
    public async Task<ActionResult<PokemonResponse>> PatchPokemonAsync(Guid id, [FromBody] PatchPokemonRequest pokemonrequest, CancellationToken cancellationToken)
    {
        try
        {
            if (pokemonrequest.Attack.HasValue && !IsValidAttack(pokemonrequest.Attack.Value))
            {
                return BadRequest(new { Message = "Invalid attack value" }); //400
            }

            var pokemon = await _pokemonService.PatchPokemonAsync(id, pokemonrequest.Name, pokemonrequest.Type, pokemonrequest.Attack, pokemonrequest.Speed, pokemonrequest.Defense, cancellationToken);
            return Ok(pokemon.ToResponse());
        }
        catch (PokemonNotFoundException)
        {
            return NotFound(); // 404
        }
        catch (PokemonAlreadyExistsException ex)
        {
            return Conflict(new { Message = ex.Message }); // 409
        }
    }
    [HttpGet]
    public async Task<ActionResult<PagedPokemonResponse>> GetPokemonsAsync([FromQuery] string? name,[FromQuery] string? type, [FromQuery] int pageSize = 10,[FromQuery] int pageNumber = 1,[FromQuery] string orderBy = "name",[FromQuery] string orderDirection = "asc",CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(type))
        {
            return BadRequest(new { Message = "Type query parameter is required" });
        }

        var paginated = await _pokemonService.GetPokemonsAsync(
            name, type, pageSize, pageNumber, orderBy, orderDirection, cancellationToken);
            
        return Ok(paginated.ToPagedResponse());
    }
}