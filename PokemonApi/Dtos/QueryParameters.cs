using System.Runtime.Serialization;

namespace PokemonApi.Dtos;

[DataContract(Name = "QueryParameters", Namespace = "http://pokemon-api/pokemon-service")]
public class QueryParameters
{
    [DataMember(Order = 1)]
    public string Name { get; set; }

    [DataMember(Order = 2)]
    public string Type { get; set; }

    [DataMember(Order = 3)]
    public int PageSize { get; set; }

    [DataMember(Order = 4)]
    public int PageNumber { get; set; }

    [DataMember(Order = 5)]
    public string OrderBy { get; set; } = "name";

    [DataMember(Order = 6)]
    public string OrderDirection { get; set; } = "asc";
}