namespace ReservaHospedagem.Domain;

public sealed class Suite
{
    public uint Andar { get; set; }
    public uint Quarto { get; set; }
    public ushort Capacidade { get; set; }
    public decimal Diaria { get; set; }
    public bool Ocupado { get; set; }
}