using Microsoft.AspNetCore.StaticAssets;
using PokemonApi.Dtos;
using PokemonApi.Infrastructure.Entities;
using PokemonApi.Models;

namespace PokemonApi.Mappers;

public static class PokemonMapper
{
    //extension method
    public static Pokemon ToModel(this PokemonEntity pokemonEntity)
    {
        if (pokemonEntity is null)
        {
            return null;
        }

        return new Pokemon
        {
            Id = pokemonEntity.Id,
            Name = pokemonEntity.Name,
            Type = pokemonEntity.Type,
            Level = pokemonEntity.Level,
            stats = new Stats
            {
                Attack = pokemonEntity.Attack,
                Speed = pokemonEntity.Speed,
                Defense = pokemonEntity.Defense
            }
        };
    }

    public static PokemonEntity ToEntity(this Pokemon pokemon)
    {
        return new PokemonEntity
        {
            Id = pokemon.Id,
            Level = pokemon.Level,
            Type = pokemon.Type,
            Name = pokemon.Name,
            Attack = pokemon.stats.Attack,
            Speed = pokemon.stats.Speed,
            Defense = pokemon.stats.Defense
        };
    }

    public static PokemonResponseDto ToReponseDto(this Pokemon pokemon)
    {
        return new PokemonResponseDto
        {
            Id = pokemon.Id,
            Level = pokemon.Level,
            Type = pokemon.Type,
            Name = pokemon.Name,
            Stats = new StatsDto
            {
                Attack = pokemon.stats.Attack,
                Speed = pokemon.stats.Speed,
                Defense = pokemon.stats.Defense,
                HP = pokemon.stats.HP
            }
        };
    }

    public static Pokemon ToModel(this CreatePokemonDto requestPokemonDto)
    {
        return new Pokemon
        {
            Level = requestPokemonDto.Level,
            Type = requestPokemonDto.Type,
            Name = requestPokemonDto.Name,
            stats = new Stats
            {
                Attack = requestPokemonDto.Stats.Attack,
                Speed = requestPokemonDto.Stats.Speed,
                Defense = requestPokemonDto.Stats.Defense
            }
        };
    }

    public static IList<PokemonResponseDto> ToResponseDto(this IReadOnlyList<Pokemon> pokemons)
    {
        return pokemons.Select(s => s.ToReponseDto()).ToList();
    }

    public static IReadOnlyList<Pokemon> ToModel(this IReadOnlyList<PokemonEntity> pokemons)
    {
        return pokemons.Select(s => s.ToModel()).ToList();
    }
}