using System;

namespace DinoApiRest.Exceptions;

public class DinoNotFoundException : Exception
{
    public DinoNotFoundException(Guid id) : base($"Dino con la id: {id}, no encontrado")
    {
    }
}
