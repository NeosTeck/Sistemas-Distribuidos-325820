using System.Runtime.Serialization;

namespace DinoApi.Dtos;

[DataContract(Name = "CreateDinoDto", Namespace = "http://dino-api/dino-service")]
public class CreateDinoDto
{
    [DataMember(Order = 1)] public string? Nombre { get; set; }
    [DataMember(Order = 2)] public string? Orden { get; set; }
    [DataMember(Order = 3)] public string? Postura { get; set; }
    [DataMember(Order = 4)] public string? PeriodoPpl { get; set; }
    [DataMember(Order = 5)] public string? Dieta { get; set; }
    [DataMember(Order = 6)] public string? Continente { get; set; }
}