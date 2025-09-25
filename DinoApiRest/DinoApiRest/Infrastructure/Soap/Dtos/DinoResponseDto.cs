using System;
using System.Runtime.Serialization;

namespace DinoApiRest.Infrastructure.Soap.Dtos;

[DataContract(Name = "DinoResponseDto", Namespace = "http://dino-api/dino-service")]
public class DinoResponseDto
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }

    [DataMember(Order = 2)]
    public string? Nombre { get; set; }

    [DataMember(Order = 3)]
    public string? Orden { get; set; }

    [DataMember(Order = 4)]
    public string? Postura { get; set; }

    [DataMember(Order = 5)]
    public string? PeriodoPpl { get; set; }

    [DataMember(Order = 6)]
    public string? Dieta { get; set; }

    [DataMember(Order = 7)]
    public string? Continente { get; set; }
}
