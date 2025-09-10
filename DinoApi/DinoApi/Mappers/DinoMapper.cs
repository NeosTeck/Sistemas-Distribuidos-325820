using DinoApi.Dtos;
using DinoApi.Models;
using DinosaurApi.Infrastructure.Entities;

namespace DinoApi.Mappers;

public static class DinoMapper
{
    public static Dino ToModel(this CreateDinoDto dto) => new Dino
    {
        Id = Guid.NewGuid(),
        Nombre = dto.Nombre,
        Orden = dto.Orden,
        Postura = dto.Postura,
        PeriodoPpl = dto.PeriodoPpl,
        Dieta = dto.Dieta,
        Continente = dto.Continente
    };

    public static DinoEntity ToEntity(this Dino model) => new DinoEntity
    {
        Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
        Nombre = model.Nombre,
        Orden = model.Orden,
        Postura = model.Postura,
        PeriodoPpl = model.PeriodoPpl,
        Dieta = model.Dieta,
        Continente = model.Continente
    };

    public static Dino ToModel(this DinoEntity entity) => new Dino
    {
        Id = entity.Id,
        Nombre = entity.Nombre,
        Orden = entity.Orden,
        Postura = entity.Postura,
        PeriodoPpl = entity.PeriodoPpl,
        Dieta = entity.Dieta,
        Continente = entity.Continente
    };

    public static DinoResponseDto ToResponseDto(this Dino model) => new DinoResponseDto
    {
        Id = model.Id,
        Nombre = model.Nombre,
        Orden = model.Orden,
        Postura = model.Postura,
        PeriodoPpl = model.PeriodoPpl,
        Dieta = model.Dieta,
        Continente = model.Continente
    };
    public static Dino ToModel(this UpdateDinoDto dto) => new Dino
    {
    Id = dto.Id,
    Nombre = dto.Nombre,
    Orden = dto.Orden,
    Postura = dto.Postura,
    PeriodoPpl = dto.PeriodoPpl,
    Dieta = dto.Dieta,
    Continente = dto.Continente
    };

    
}
