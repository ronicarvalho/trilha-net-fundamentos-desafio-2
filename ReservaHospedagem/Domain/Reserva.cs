namespace ReservaHospedagem.Domain;

public sealed class Reserva
{
    public string Localizador { get; set; }
    public Hospede Reservante { get; private set; } = new();
    public List<Hospede> Hospedes { get; private set; } = new();
    public Estadia Estadia { get; set; } = new();
    public List<Suite> Suites { get; private set; } = new();
    public ushort TempoEstadia { get; set; }
    public ushort QuantidadeHospedes { get; set; }
    public decimal ValorReserva { get; set; }
    
    public void AdicionarHospede(Hospede hospede) => Hospedes.Add(hospede);
    public void AdicionarSuite(Suite suite) => Suites.Add(suite);
    public void AdicionarReservante(Hospede hospede) => Reservante = hospede;
    public void CalcularReserva() => ValorReserva = 0;
}