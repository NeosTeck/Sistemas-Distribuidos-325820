using System.CodeDom;
using PokedexApi.Dtos;
using PokedexApi.Infrastructure.Soap.Contrats;
using PokedexApi.Models;
using PokedexApi.Expections;
using PokedexApi.Gateways;
using PokedexApi.Infrastructure.Soap.Dtos;

namespace PokedexApi.Mappers;

public static class PokemonMapper
{
    public static Pokemon ToModel(this PokemonResponseDto pokemonResposeDto)
    {
        return new Pokemon
        {
            Id = pokemonResposeDto.Id,
            Name = pokemonResposeDto.Name,
            Type = pokemonResposeDto.Type,
            Level = pokemonResposeDto.Level,
            Stats = new Stats
            {
                Attack = pokemonResposeDto.Stats.Attack,
                Defense = pokemonResposeDto.Stats.Defense,
                Speed = pokemonResposeDto.Stats.Speed
            }
        };
    }

    public static PokemonResponse ToResponse(this Pokemon pokemon)
    {
        return new PokemonResponse
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            Attack = pokemon.Stats.Attack,
            Type = pokemon.Type
        };
    }

    public static Pokemon ToModel(this CreatePokemonRequest createPokemonRequest)
    {
        return new Pokemon
        {
            Name = createPokemonRequest.Name,
            Type = createPokemonRequest.Type,
            Level = createPokemonRequest.Level,
            Stats = new Stats
            {
                Attack = createPokemonRequest.Stats.Attack,
                Defense = createPokemonRequest.Stats.Defense,
                Speed = createPokemonRequest.Stats.Speed
            }
        };
    }

    public static IList<Pokemon> ToModel(this IList<PokemonResponseDto> pokemonResponseDtos)
    {
        return pokemonResponseDtos.Select(s => s.ToModel()).ToList();
    }

    public static CreatePokemonDto ToRequest(this Pokemon pokemon)
    {
        return new CreatePokemonDto
        {
            Name = pokemon.Name,
            Type = pokemon.Type,
            Level = pokemon.Level,
            Stats = new StatsDto
            {
                Attack = pokemon.Stats.Attack,
                Defense = pokemon.Stats.Defense,
                Speed = pokemon.Stats.Speed
            }
        };
    }
    public static IList<PokemonResponse> ToResponse(this IList<Pokemon> pokemons)
    {
        return pokemons.Select(s => s.ToResponse()).ToList();
    }

    public static Pokemon ToModel(this UpdatePokemonRequest pokemon, Guid id)
    {
        return new Pokemon
        {
            Id = id,
            Name = pokemon.Name,
            Type = pokemon.Type,
            Stats = new Stats
            {
                Attack = pokemon.Stats.Attack,
                Defense = pokemon.Stats.Defense,
                Speed = pokemon.Stats.Speed
            }
        };
    }
    public static UpdatePokemonDto ToUpdateRequest(this Pokemon pokemon)
    {
        return new UpdatePokemonDto
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            Type = pokemon.Type,
            Stats = new StatsDto
            {
                Attack = pokemon.Stats.Attack,
                Defense = pokemon.Stats.Defense,
                Speed = pokemon.Stats.Speed
            }
        };
    }
     public static PagedPokemonResponse ToPagedResponse(this PagedResult<Pokemon> pagedResult)
    {
        return new PagedPokemonResponse
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalRecords = pagedResult.TotalRecords,
            TotalPages = pagedResult.TotalPages,
            Data = pagedResult.Data.ToResponse()
        };
    }

    public static PagedResult<Pokemon> ToPagedResult(this PagedPokemonResponseDto pagedDto)
    {
        if (pagedDto == null)
        {
            return new PagedResult<Pokemon>
            {
                TotalRecords = 0,
                PageNumber = 1,
                PageSize = 0,
                Data = new List<Pokemon>()
            };
        }

        return new PagedResult<Pokemon>
        {
            PageNumber = pagedDto.PageNumber,
            PageSize = pagedDto.PageSize,
            TotalRecords = pagedDto.TotalRecords,
            Data = pagedDto.Data?.ToModel() ?? new List<Pokemon>()
        };
    }
}