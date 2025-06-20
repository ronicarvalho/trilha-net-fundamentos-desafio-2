namespace ReservaHospedagem.Domain;

public sealed class Hospede
{
    public string Documento { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Sobrenome { get; set; }
    public string? Email { get; set; }
    public string? Celular { get; set; }
}