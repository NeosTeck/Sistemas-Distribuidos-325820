namespace PokedexApi.Expections;

public class PokemonNotFoundException : Exception
{
    public PokemonNotFoundException(Guid id) : base($"Pokemon {id} not found")
    {
    }
}