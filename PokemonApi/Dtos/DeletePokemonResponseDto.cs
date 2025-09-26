using System.Runtime.Serialization;

namespace PokemonApi.Dtos;

[DataContract(Name = "DeletePokemonResponseDto", Namespace = "http://pokemon-api/pokemon-service")]
public class DeletePokemonResponseDto
{
    [DataMember(Name = "Succes", Order = 1)]
    public bool Succes { get; set; }
}