using System.ServiceModel;
using PokedexApi.Models;
using PokedexApi.Mappers;
using PokedexApi.Infrastructure.Soap.Contrats;
using PokedexApi.Expections;
using PokedexApi.Dtos;
using PokedexApi.Infrastructure.Soap.Dtos;

namespace PokedexApi.Gateways;

public class PokemonGateway : IPokemonGateway
{
    private readonly IPokemonContract _pokemonContract;
    private readonly ILogger<PokemonGateway> _logger;

    public PokemonGateway(IConfiguration configuration, ILogger<PokemonGateway> logger)
    {
        var binding = new BasicHttpBinding();
        var endopoint = new EndpointAddress(configuration.GetValue<string>("PokemonService:Url"));
        _pokemonContract = new ChannelFactory<IPokemonContract>(binding, endopoint).CreateChannel();
        _logger = logger;
    }

    public async Task DeletePokemonAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _pokemonContract.DeletePokemon(id, cancellationToken);
        }
        catch (FaultException ex) when (ex.Message == "Pokemon not found")
        {
            _logger.LogWarning(ex, "Pokemon not found");
            throw new PokemonNotFoundException(id);
        }
    }

    public async Task<Pokemon> GetPokemonByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var pokemon = await _pokemonContract.GetPokemonById(id, cancellationToken);
            return pokemon.ToModel();
        }
        catch (FaultException ex) when (ex.Message == "Pokemon not found")
        {
            _logger.LogWarning(ex, "Pokemon Not Found");
            return null;
        }
    }

    public async Task<IList<Pokemon>> GetPokemonsByNameAsync(string name, CancellationToken cancellationToken)
    {
        var pokemons = await _pokemonContract.GetPokemonsByName(name, cancellationToken);
        return pokemons.ToModel();
    }

    public async Task<Pokemon> CreatePokemonAsync(Pokemon pokemon, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending request to SOAP API, with pokemon: {name}", pokemon.Name);
            var createdPokemon = await _pokemonContract.CreatePokemon(pokemon.ToRequest(), cancellationToken);
            return createdPokemon.ToModel();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Algo no funciono en el create pokemon a soap");
            throw;
        }
    }

    public async Task UpdatePokemonAsync(Pokemon pokemon, CancellationToken cancellationToken)
    {
        try
        {
            await _pokemonContract.UpdatePokemon(pokemon.ToUpdateRequest(), cancellationToken);
        }
        catch (FaultException ex) when (ex.Message == "Pokemon not found")
        {
            throw new PokemonNotFoundException(pokemon.Id);
        }
        catch (FaultException ex) when (ex.Message == "Pokemon with the same name already exists")
        {
            throw new PokemonAlreadyExistsException(pokemon.Name);
        }
    }

    public async Task<Pokemon> PatchPokemonAsync(Guid id, string? name, string? type, int? attack, int? speed, int? defense, CancellationToken cancellationToken)
    {
        try
        {
            var existingPokemon = await _pokemonContract.GetPokemonById(id, cancellationToken);
            if (existingPokemon == null)
            {
                throw new PokemonNotFoundException(id);
            }

            var updatedPokemon = new UpdatePokemonDto
            {
                Id = id,
                Name = name ?? existingPokemon.Name,
                Type = type ?? existingPokemon.Type,
                Stats = new StatsDto
                {
                    Attack = attack ?? existingPokemon.Stats.Attack,
                    Defense = defense ?? existingPokemon.Stats.Defense,
                    Speed = speed ?? existingPokemon.Stats.Speed
                }
            };
            var result = await _pokemonContract.UpdatePokemon(updatedPokemon, cancellationToken);
            return result.ToModel();
        }
        catch (FaultException ex) when (ex.Message == "Pokemon not found")
        {
            throw new PokemonNotFoundException(id);
        }
        catch (FaultException ex) when (ex.Message == "Pokemon with the same name already exists")
        {
            throw new PokemonAlreadyExistsException(name ?? "unknown");
        }
    }

    public async Task<PagedResult<Pokemon>> GetPokemonsAsync(string name, string type, int pageSize, int pageNumber, string orderBy, string orderDirection, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting pokemons with filters: Name={Name}, Type={Type}, PageSize={PageSize}, PageNumber={PageNumber}", 
                name, type, pageSize, pageNumber);

            // USAR EL MÉTODO QUE SÍ EXISTE: GetPokemonsByName
            var allPokemons = await _pokemonContract.GetPokemonsByName(name ?? "", cancellationToken);
            var pokemonsModel = allPokemons.ToModel();
            
            _logger.LogInformation("Retrieved {Count} pokemons from SOAP service", pokemonsModel.Count);

            // Filtrar por tipo del lado del cliente
            var filteredPokemons = pokemonsModel
                .Where(p => p.Type.ToLower().Contains(type.ToLower()))
                .ToList();
            
            _logger.LogInformation("Found {Total} pokemons after filtering by type '{Type}'", filteredPokemons.Count, type);
            
            // Ordenar del lado del cliente
            var orderedPokemons = OrderPokemons(filteredPokemons, orderBy, orderDirection);
            
            // Paginar del lado del cliente
            var paginatedData = orderedPokemons
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            _logger.LogInformation("Returning page {PageNumber} of {TotalPages} with {Count} items", 
                pageNumber, (int)Math.Ceiling(filteredPokemons.Count / (double)pageSize), paginatedData.Count);

            return new PagedResult<Pokemon>
            {
                Data = paginatedData,
                TotalRecords = filteredPokemons.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPokemonsAsync");
            throw;
        }
    }

    private static List<Pokemon> OrderPokemons(List<Pokemon> pokemons, string orderBy, string orderDirection)
    {
        if (pokemons == null || !pokemons.Any())
            return pokemons;

        var isAscending = orderDirection.ToLower() == "asc";
        
        return orderBy.ToLower() switch
        {
            "name" => isAscending ? 
                pokemons.OrderBy(p => p.Name).ToList() : 
                pokemons.OrderByDescending(p => p.Name).ToList(),
            "attack" => isAscending ? 
                pokemons.OrderBy(p => p.Stats.Attack).ToList() : 
                pokemons.OrderByDescending(p => p.Stats.Attack).ToList(),
            "defense" => isAscending ? 
                pokemons.OrderBy(p => p.Stats.Defense).ToList() : 
                pokemons.OrderByDescending(p => p.Stats.Defense).ToList(),
            _ => isAscending ? pokemons.OrderBy(p => p.Name).ToList() : pokemons.OrderByDescending(p => p.Name).ToList()
        };
    }
}