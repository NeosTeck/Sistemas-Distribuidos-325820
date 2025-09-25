namespace DinoApiRest.Dtos;

public class DinoDto
{
    public Guid Id { get; set; }
    public string? Nombre { get; set; }
    public string? Orden { get; set; }
    public string? Postura { get; set; }
    public string? PeriodoPpl { get; set; }
    public string? Dieta { get; set; }
    public string? Continente { get; set; }
}
