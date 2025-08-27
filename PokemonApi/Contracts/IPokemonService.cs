using System.ServiceModel;
namespace PokemonApi.services;
using PokemonApi.Dtos;

[ServiceContract(Name = "PokemonService", Namespace = "http://pokemonapi.com/services")]

public interface IPokemonService
{
    [OperationContract]

    Task<PokemonResponseDto> CreatePokemon(CreatePokemonDto pokemon, CancellationToken cancellationToken);
}