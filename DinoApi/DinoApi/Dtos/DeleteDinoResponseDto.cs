using System.Runtime.Serialization;

namespace DinoApi.Dtos;

[DataContract(Name = "DeleteDinoResponseDto", Namespace = "http://dino-api/dino-service")]
public class DeleteDinoResponseDto
{
    [DataMember(Name = "Success", Order = 1)]
    public bool Success { get; set; }
}
