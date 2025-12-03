using Microsoft.EntityFrameworkCore;
using PokemonApi.Infrastructure;
using PokemonApi.Models;
using PokemonApi.Mappers;
using PokemonApi.Dtos;

namespace PokemonApi.Repositories;

public class PokemonRepository : IPokemonRepository
{
    private readonly RelationalDbContext _context;

    public PokemonRepository(RelationalDbContext context)
    {
        _context = context;
    }

    public async Task UpdatePokemonAsync(Pokemon pokemon, CancellationToken cancellationToken)
    {
        _context.Pokemons.Update(pokemon.ToEntity());
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePokemonAsync(Pokemon pokemon, CancellationToken cancellationToken)
    {
        _context.Pokemons.Remove(pokemon.ToEntity());
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Pokemon>> GetPokemonsByNameAsync(string name, CancellationToken cancellationToken)
    {
        var pokemons = await _context.Pokemons.AsNoTracking().Where(s => s.Name.Contains(name)).ToListAsync(cancellationToken);

        return pokemons.ToModel();
    }

    public async Task<Pokemon> GetPokemonByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        //Selectr * from pokemons where id is = "asdasd"
        var pokemon = await _context.Pokemons.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return pokemon.ToModel();
    }

    public async Task<Pokemon> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        //select * from pokemons where name like '%TEXTO%'
        var pokemon = await _context.Pokemons.AsNoTracking().FirstOrDefaultAsync(s => s.Name.Contains(name));
        return pokemon.ToModel();
    }

    public async Task<Pokemon> CreateAsync(Pokemon pokemon, CancellationToken cancellationToken)
    {
        var pokemonToCreate = pokemon.ToEntity();
        pokemonToCreate.Id = Guid.NewGuid();
        await _context.Pokemons.AddAsync(pokemonToCreate, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return pokemonToCreate.ToModel();
    }
    public async Task<PagedPokemonResponseDto> GetPokemonsAsync(QueryParameters queryParameters, CancellationToken cancellationToken)
    {
        IQueryable<Infrastructure.Entities.PokemonEntity> query = _context.Pokemons.AsNoTracking();

        // Filtrado por tipo
        if (!string.IsNullOrEmpty(queryParameters.Type))
        {
            query = query.Where(p => p.Type.ToLower() == queryParameters.Type.ToLower());
        }

        // Filtrado por nombre (búsqueda parcial)
        if (!string.IsNullOrEmpty(queryParameters.Name))
        {
            query = query.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
        }

        // Ordenamiento dinámico
        var orderByField = queryParameters.OrderBy.ToLower();
        var isAscending = queryParameters.OrderDirection.ToLower() == "asc";

        query = orderByField switch
        {
            "name" => isAscending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
            "attack" => isAscending ? query.OrderBy(p => p.Attack) : query.OrderByDescending(p => p.Attack),
            "defense" => isAscending ? query.OrderBy(p => p.Defense) : query.OrderByDescending(p => p.Defense),
            _ => isAscending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name)
        };

        // Conteo total
        var totalRecords = await query.CountAsync(cancellationToken);

        // Paginación
        var pokemons = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync(cancellationToken);

        // Mapeo a DTO
        var pokemonDtos = pokemons.ToModel().ToResponseDto().ToList();

        return new PagedPokemonResponseDto
        {
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)queryParameters.PageSize),
            Data = pokemonDtos
        };
    }
}