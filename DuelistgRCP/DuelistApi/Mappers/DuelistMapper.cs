using DuelistApi.Infrastructure.Documents;
using DuelistApi.Models;

namespace DuelistApi.Mappers;

public static class DuelistMapper
{
    public static Duelist ToDomain(this DuelistDocument document)
    {
        if (document is null) return null;
        return new Duelist
        {
            Id = document.Id,
            Name = document.Name,
            Nickname = document.Nickname,
            Series = document.Series,
            Archetype = document.Archetype,
            SignatureCard = document.SignatureCard,
            FavoriteMechanic = document.FavoriteMechanic
        };
    }

    public static DuelistDocument ToDocument(this Duelist duelist)
    {
        return new DuelistDocument
        {
            Id = duelist.Id,
            Name = duelist.Name,
            Nickname = duelist.Nickname,
            Series = duelist.Series,
            Archetype = duelist.Archetype,
            SignatureCard = duelist.SignatureCard,
            FavoriteMechanic = duelist.FavoriteMechanic
        };
    }

    public static DuelistResponse ToResponse(this Duelist duelist)
    {
        return new DuelistResponse
        {
            Id = duelist.Id,
            Name = duelist.Name,
            Nickname = duelist.Nickname,
            Series = duelist.Series,
            Archetype = duelist.Archetype,
            SignatureCard = duelist.SignatureCard,
            FavoriteMechanic = duelist.FavoriteMechanic
        };
    }

    public static Duelist ToModel(this CreateDuelistRequest request)
    {
        return new Duelist
        {
            Name = request.Name,
            Nickname = request.Nickname,
            Series = request.Series,
            Archetype = request.Archetype,
            SignatureCard = request.SignatureCard,
            FavoriteMechanic = request.FavoriteMechanic
        };
    }
}
