using ReservaHospedagem.Domain;
using ReservaHospedagem.Screens;
using Terminal.Gui;

namespace ReservaHospedagem;

public class MainScreen: Window
{
    private List<Hospede> _hospedes = new();
    private List<Suite>  _suites = new();
    private List<Reserva> _reservas = new();
    
    public MainScreen() : base("")
    {
        GerarDadosHospedes();
        GerarDadosSuites();
        
        X = 0;
        Y = 1;
        
        Width = Dim.Fill();
        Height = Dim.Fill();
        
        var menuBar = new MenuBar(new[]
        {
            new MenuBarItem("_Cadastro", new MenuItem[]
            {
                new("_Hóspedes", "", CadastrarHospede),
                new("_Suítes", "", CadastrarSuite)
            }),
            new MenuBarItem("_Reservas", new MenuItem[]
            {
                new("_Efetuar", "", EfetuarReserva),
                new("_Consultar", "", ConsultarReserva),
                new("_Alterar", "", AlterarReserva),
                new("_Cancelar", "", CancelarReserva)
            }),
            new MenuBarItem("_Sair", new MenuItem[]
            {
                new("_Finalizar", "", () =>
                {
                    if (MessageBox.Query("Finalizar", "Deseja realmente sair?", "Sim", "Não") == 0)
                        Application.RequestStop();
                })
            })
        });
        
        var statusBar = new StatusBar(new []
        {
            new StatusItem(Key.Esc, "~ESC~ Sair", () =>
            {
                if (MessageBox.Query("Finalizar", "Deseja realmente sair?", "Sim", "Não") == 0)
                    Application.RequestStop();
            }),
            new StatusItem(Key.F2, "~F2~ Cadastrar Hospede", () => CadastrarHospede()),
            new StatusItem(Key.F3, "~F3~ Efetuar Reserva", () => EfetuarReserva()),
            new StatusItem(Key.F4, "~F4~ Consultar Reserva", () => ConsultarReserva())
        });
        
        Add(new Label("Bem-vindo(a) ao Sistema de Reservas de Hospedagem. ") { X = Pos.Center(), Y = Pos.Center() });
        
        Application.Top.Add(menuBar, statusBar);
    }

    public sealed override void Add(View view)
    {
        base.Add(view);
    }

    private void GerarDadosHospedes()
    {
        for (var i = 0; i < 9; i++)
        {
            _hospedes.Add(new Hospede()
            {
                Documento = $"26{i}78{i}7682{i}",
                Nome = $"Hospede {i}",
                Sobrenome = "da Silva",
                Email = $"hospede.{i}@gmail.com",
                Celular = $"18 9234{i}6757{i}",
            });
        }
    }

    private void GerarDadosSuites()
    {
        var random = new Random();
        
        for (uint andar = 1; andar < 29; andar++)
        {
            for (uint quarto = 1; quarto < 8; quarto++)
            {
                _suites.Add(new Suite()
                {
                    Andar = andar,
                    Quarto = quarto,
                    Capacidade = (ushort) random.Next(1, 10),
                    Diaria = random.Next(40, 150),
                    Ocupado = false
                });
            }
        }
    }

    private void CadastrarHospede()
    {
        var dialog = new CadastroHospedeScreen();
        Application.Run(dialog);

        if (dialog.Hospede == null) return;
        
        _hospedes.Add(dialog.Hospede);
        MessageBox.Query("", "Hóspede cadastrado com sucesso!", "OK");
    }

    private void CadastrarSuite()
    {
        var dialog = new CadastroSuiteScreen();
        Application.Run(dialog);

        if (dialog.Suite == null) return;
        
        _suites.Add(dialog.Suite);
        MessageBox.Query("", "Suite cadastrada com sucesso!", "OK");
    }

    private void EfetuarReserva()
    {
        if (!_hospedes.Any())
        {
            MessageBox.ErrorQuery("Erro", "Nenhum hóspede cadastrado.", "Ok");
            return;
        }

        if (!_suites.Any())
        {
            MessageBox.ErrorQuery("Erro", "Nenhuma suíte cadastrada ou disponível.", "Ok");
            return;
        }

        var reservaScreen = new ReservaScreen(_hospedes, _suites);
        Application.Run(reservaScreen);
        
        if (reservaScreen.Reserva == null) return;
        _reservas.Add(reservaScreen.Reserva);
        MessageBox.Query("", "Reserva efetuada com sucesso!", "OK");
    }
    
    private void ConsultarReserva() {}
    private void AlterarReserva() {}
    private void CancelarReserva() {}
}