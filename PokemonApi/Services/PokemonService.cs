using PokemonApi.Dtos;
using PokemonApi.services;

namespace PokemonApi.Services
{
    public class PokemonService : IPokemonService
    {
        public Task<PokemonResponseDto> CreatePokemon(CreatePokemonDto pokemon, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }
    }
}