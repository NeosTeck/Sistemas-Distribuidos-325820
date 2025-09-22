using System.Runtime.Serialization;

namespace DinoApi.Dtos;

[DataContract(Name = "UpdateDinoDto", Namespace = "http://dino-api/dino-service")]
public class UpdateDinoDto
{
    [DataMember(Name = "Id", Order = 1)]
    public Guid Id { get; set; }

    [DataMember(Name = "Nombre", Order = 2)]
    public string? Nombre { get; set; }

    [DataMember(Name = "Orden", Order = 3)]
    public string? Orden { get; set; }

    [DataMember(Name = "Postura", Order = 4)]
    public string? Postura { get; set; }

    [DataMember(Name = "PeriodoPpl", Order = 5)]
    public string? PeriodoPpl { get; set; }

    [DataMember(Name = "Dieta", Order = 6)]
    public string? Dieta { get; set; }

    [DataMember(Name = "Continente", Order = 7)]
    public string? Continente { get; set; }
}
