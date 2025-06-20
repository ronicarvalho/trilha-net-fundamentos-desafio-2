using NStack;
using ReservaHospedagem.Domain;

namespace ReservaHospedagem.Screens;

using Terminal.Gui;
using System.Collections.Generic;

public sealed class ReservaScreen : Window
{
    public Reserva? Reserva { get; set; }

    private readonly List<Hospede> _hospedes;
    private readonly List<Suite> _suites;

    private Label _labelLocalizador = null!;
    private TextField _textFieldLocalizador = null!;
    private Label _labelFieldReservante = null!;
    private TextField _textFieldReservante = null!;
    private Label _labelFieldEntrada = null!;
    private TextField _textFieldEntrada = null!;
    private Label _labelFieldSaida = null!;
    private TextField _textFieldSaida = null!;
    private Label _labelQtadeHospedes = null!;
    private Label _labelTempoEstadia = null!;
    private Label _labelValorTotal = null!;
    private Label _labelTotalDesconto = null!;
    private Label _labelTotalReserva = null!;

    private ListView _listViewHospedes = null!;
    private ListView _listViewSuites = null!;

    public ReservaScreen(List<Hospede> hospedes, List<Suite> suites) : base("")
    {
        Reserva = new Reserva();
        _hospedes = hospedes;
        _suites = suites;

        X = 0;
        Y = 1;

        Width = Dim.Fill();
        Height = Dim.Fill();

        MontarLayout();
        GerarLocalizador();
    }

    private void MontarLayout()
    {
        // Área dos dados da reserva
        var frameDados = new FrameView("Dados da Reserva")
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(100),
            Height = 7
        };

        frameDados.Add(CriarControlesReserva());

        // Área da lista de hóspedes
        var frameHospedes = new FrameView("Hóspedes")
        {
            X = 0,
            Y = Pos.Bottom(frameDados),
            Width = Dim.Percent(50),
            Height = Dim.Fill() -1
        };

        frameHospedes.Add(CriarControlesHospedes());

        // Área da lista de suítes
        var frameSuites = new FrameView("Suítes")
        {
            X = Pos.Right(frameHospedes),
            Y = Pos.Bottom(frameDados),
            Width = Dim.Percent(50),
            Height = Dim.Fill() -1
        };

        frameSuites.Add(CriarControlesSuites());
        
        // Status bar com informações de uso

        var statusBar = new StatusBar(new []
        {
            new StatusItem(Key.Esc, "~ESC~ Sair", () =>
            {
                if (MessageBox.Query("Finalizar", "Deseja sair sem efetuar a reserva?", "Sim", "Não") !=
                    0) return;
                Reserva = null!;
                Application.RequestStop();
            }),
            new StatusItem(Key.F2, "~F2~ Novo Hospede", () => {}),
            new StatusItem(Key.F6, "~F6~ Pesquisar [Reservante, Hospedes, Suites]", () => {}),
            new StatusItem(Key.F7, "~F7~ Calcular Reserva]", CalcularReserva),
            new StatusItem(Key.F8, "~F8~ Salvar Reserva]", () => Application.RequestStop())
        });

        Add(frameDados, frameHospedes, frameSuites, statusBar);
    }

    private View[] CriarControlesReserva()
    {
        // Instancias

        _labelLocalizador = new Label("Localizador:") { X = 1, Y = 0 };
        _textFieldLocalizador = new TextField("") { X = 15, Y = 0, Width = 20, ReadOnly = true };
        _labelFieldReservante = new Label("Reservante:") { X = 40, Y = 0 };
        _textFieldReservante = new TextField("") { X = 52, Y = 0, Width = 25 };
        _labelQtadeHospedes = new Label("Hospedes: 0") { X = Pos.AnchorEnd(33), Y = 0 };
        _labelFieldEntrada = new Label("Entrada:") { X = 1, Y = 2 };
        _textFieldEntrada = new TextField(DateTime.Now.ToString("dd-MM-yyyy")) { X = 15, Y = 2, Width = 12 };
        _labelFieldSaida = new Label("Saída:") { X = 40, Y = 2 };
        _textFieldSaida = new TextField(DateTime.Now.AddDays(1).ToString("dd-MM-yyyy")) { X = 52, Y = 2, Width = 12 };
        _labelTempoEstadia = new Label("Dias: 1") { X = Pos.AnchorEnd(20), Y = 2 };
        _labelValorTotal = new Label("Valor: R$ 0,00") { X = Pos.AnchorEnd(67), Y = 4 };
        _labelTotalDesconto = new Label("Descontos: R$ 0,00") { X = Pos.AnchorEnd(45), Y = 4 };
        _labelTotalReserva = new Label("Total: R$ 0,00") { X = Pos.AnchorEnd(20), Y = 4 };

        // Eventos

        _textFieldReservante.KeyPress += (e) =>
        {
            if (e.KeyEvent.Key != Key.F6) return;
            var selecionado = SelecionarHospede("Selecionar Reservante");
            if (selecionado != null)
            {
                _textFieldReservante.Text = $"{selecionado.Nome} {selecionado.Sobrenome}";
                Reserva!.AdicionarReservante(selecionado);
                CalcularReserva();
            }
            e.Handled = true;
        };

        _textFieldEntrada.Leave += (e) =>
        {
            if (DateTime.TryParse(_textFieldEntrada.Text.ToString(), out _)) return;
            MessageBox.ErrorQuery("Erro", "Data inválida. Use o formato DD/MM/AAAA.", "Ok");
            e.Handled = false;
        };

        _textFieldSaida.Leave += (e) =>
        {
            if (DateTime.TryParse(_textFieldSaida.Text.ToString(), out _))
            {
                CalcularReserva();
                return;
            };
            MessageBox.ErrorQuery("Erro", "Data inválida. Use o formato DD/MM/AAAA.", "Ok");
            e.Handled = false;
        };

        // Objetos 

        return new View[]
        {
            _labelLocalizador,
            _textFieldLocalizador,
            _labelFieldReservante,
            _textFieldReservante,
            _labelQtadeHospedes,
            _labelFieldEntrada,
            _textFieldEntrada,
            _labelFieldSaida,
            _textFieldSaida,
            _labelTempoEstadia,
            _labelValorTotal,
            _labelTotalDesconto,
            _labelTotalReserva
        };
    }

    private View[] CriarControlesHospedes()
    {
        _listViewHospedes = new ListView(Reserva!.Hospedes)
        {
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        _listViewHospedes.KeyPress += (e) =>
        {
            if (e.KeyEvent.Key != Key.F6) return;
            var selecionado = SelecionarHospede("Selecionar Hóspedes");
            if (selecionado != null)
            {
                if (!Reserva!.Hospedes.Contains(selecionado))
                    Reserva!.AdicionarHospede(selecionado);
                
                _listViewHospedes.SetSource(Reserva.Hospedes.Select(h => 
                    $"{h.Nome} {h.Sobrenome}"
                ).ToList());
                
                CalcularReserva();
            }
            e.Handled = true;
        };

        return new View[] { _listViewHospedes };
    }

    private View[] CriarControlesSuites()
    {
        _listViewSuites = new ListView(Reserva!.Suites)
        {
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        
        _listViewSuites.KeyPress += (e) =>
        {
            if (e.KeyEvent.Key != Key.F6) return;
            var selecionado = SelecionarSuite();
            if (selecionado != null)
            {
                if (!Reserva!.Suites.Contains(selecionado))
                    Reserva!.AdicionarSuite(selecionado);
                
                _listViewSuites.SetSource(Reserva.Suites.Select(s => 
                    $"Quarto {s.Andar}{s.Quarto} - Capacidade: {s.Capacidade} - Diária: {s.Diaria:C}"
                ).ToList());
                
                CalcularReserva();
            }
            e.Handled = true;
        };
        
        return new View[] { _listViewSuites };
    }

    private Hospede? SelecionarHospede(String title)
    {
        var dialog = new Dialog(title, 60, 20);

        var lista = new ListView(_hospedes.Select(h => $"{h.Nome} {h.Sobrenome}").ToList())
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 2
        };

        Hospede? selecionado = null;

        var buttonOk = new Button("SELECIONAR")
        {
            X = Pos.Center() - 5,
            Y = Pos.AnchorEnd(1)
        };

        buttonOk.Clicked += () =>
        {
            if (lista.SelectedItem >= 0)
                selecionado = _hospedes[lista.SelectedItem];
            Application.RequestStop();
        };

        dialog.Add(lista, buttonOk);
        Application.Run(dialog);

        return selecionado;
    }

    private Suite? SelecionarSuite()
    {
        var dialog = new Dialog("Selecionar Suíte", 60, 20);

        var lista = new ListView(_suites
            .Select(s => $"Andar {s.Andar} - Quarto {s.Quarto} ({s.Capacidade} pessoas) - R$ {s.Diaria}")
            .ToList())
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 2
        };

        Suite? selecionado = null;

        var buttonOk = new Button("SELECIONAR")
        {
            X = Pos.Center() - 5,
            Y = Pos.AnchorEnd(1)
        };
        
        buttonOk.Clicked += () =>
        {
            if (lista.SelectedItem >= 0)
                selecionado = _suites[lista.SelectedItem];
            Application.RequestStop();
        };

        dialog.Add(lista, buttonOk);
        Application.Run(dialog);

        return selecionado;
    }

    private void CalcularReserva()
    {
        if (!DateTime.TryParse(_textFieldEntrada.Text.ToString(), out var entrada) ||
            !DateTime.TryParse(_textFieldSaida.Text.ToString(), out var saida))
        {
            MessageBox.ErrorQuery("Erro", "Datas inválidas.", "Ok");
            return;
        }

        if (saida <= entrada)
        {
            MessageBox.ErrorQuery("Erro", "Data de saída deve ser após a entrada.", "Ok");
            return;
        }

        var hospedes = _textFieldReservante.Text != ustring.Empty ? Reserva!.Hospedes.Count + 1 : Reserva!.Hospedes.Count;
        var dias = (ushort)(saida - entrada).Days;
        var total = Reserva!.Suites.Sum(s => s.Diaria) * dias;
        var calculado = (dias > 10) ? total * 0.9m : total; 

        Reserva.TempoEstadia = dias;
        Reserva.ValorReserva = total;
        
        _labelQtadeHospedes.Text = $"Hospedes: {hospedes}";
        _labelTempoEstadia.Text = $"Dias: {dias}";
        _labelValorTotal.Text = $"Total: {total:F}";
        _labelTotalDesconto.Text = $"Descontos: R$ {total - calculado:F2}";
        _labelTotalReserva.Text = $"Total: R$ {calculado:F2}";
    }

    private void GerarLocalizador()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        var random = new Random();
        var localizador = new string(Enumerable
            .Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());

        _textFieldLocalizador.Text = localizador;
        _textFieldReservante.SetFocus();
    }
}