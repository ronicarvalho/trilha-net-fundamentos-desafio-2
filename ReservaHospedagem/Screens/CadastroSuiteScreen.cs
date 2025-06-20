using ReservaHospedagem.Domain;
using Terminal.Gui;

namespace ReservaHospedagem.Screens;

public class CadastroSuiteScreen: Dialog
{
    public Suite? Suite { get; private set; }

    private readonly TextField _textFieldAndar;
    private readonly TextField _textFieldQuarto;
    private readonly TextField _textFieldCapacidade;
    private readonly TextField _textFieldDiaria;
    private readonly CheckBox _checkBoxOcupado;

    public CadastroSuiteScreen() : base("Cadastro de Suíte", 60, 20)
    {
        Suite = null;

        int posY = 1;
        Add(new Label("Andar:") { X = 2, Y = posY });
        _textFieldAndar = new TextField("") { X = 15, Y = posY, Width = 40 };

        posY += 2;
        Add(new Label("Quarto:") { X = 2, Y = posY });
        _textFieldQuarto = new TextField("") { X = 15, Y = posY, Width = 40 };

        posY += 2;
        Add(new Label("Capacidade:") { X = 2, Y = posY });
        _textFieldCapacidade = new TextField("") { X = 15, Y = posY, Width = 40 };

        posY += 2;
        Add(new Label("Diária:") { X = 2, Y = posY });
        _textFieldDiaria = new TextField("") { X = 15, Y = posY, Width = 40 };

        posY += 2;
        Add(new Label("Ocupado:") { X = 2, Y = posY });
        _checkBoxOcupado = new CheckBox() { X = 15, Y = posY };

        var buttonSalvar = new Button("Salvar")
        {
            X = Pos.Center() - 10,
            Y = posY + 3,
            IsDefault = true
        };
        buttonSalvar.Clicked += OnSalvar;

        var buttonCancelar = new Button("Cancelar")
        {
            X = Pos.Center() + 2,
            Y = posY + 3
        };
        buttonCancelar.Clicked += OnCancelar;

        Add(_textFieldAndar, _textFieldQuarto, _textFieldCapacidade, _textFieldDiaria, _checkBoxOcupado, buttonSalvar, buttonCancelar);
    }

    public sealed override void Add(View view)
    {
        base.Add(view);
    }

    private void OnSalvar()
    {
        if (!DadosValidos()) return;
        
        uint.TryParse(_textFieldAndar.Text.ToString(), out var andar);
        uint.TryParse(_textFieldQuarto.Text.ToString(), out var quarto);
        ushort.TryParse(_textFieldCapacidade.Text.ToString(), out var capacidade);
        decimal.TryParse(_textFieldDiaria.Text.ToString(), out var diaria);

        Suite = new Suite
        {
            Andar = andar,
            Quarto = quarto,
            Capacidade = capacidade,
            Diaria = diaria,
            Ocupado = _checkBoxOcupado.Checked
        };

        Application.RequestStop();
    }

    private bool DadosValidos()
    {
        if (string.IsNullOrWhiteSpace(_textFieldAndar.Text.ToString()) || !uint.TryParse(_textFieldAndar.Text.ToString(), out _))
        {
            MessageBox.ErrorQuery("Erro de Validação", "Andar é um valor inteiro obrigatório.", "Ok");
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(_textFieldQuarto.Text.ToString()) || !uint.TryParse(_textFieldQuarto.Text.ToString(), out _))
        {
            MessageBox.ErrorQuery("Erro de Validação", "Quarto é um valor inteiro obrigatório.", "Ok");
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(_textFieldCapacidade.Text.ToString()) || !ushort.TryParse(_textFieldCapacidade.Text.ToString(), out _))
        {
            MessageBox.ErrorQuery("Erro de Validação", "Capacidade é obrigatório.", "Ok");
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(_textFieldDiaria.Text.ToString()) || !decimal.TryParse(_textFieldDiaria.Text.ToString(), out _))
        {
            MessageBox.ErrorQuery("Erro de Validação", "Diária é obrigatório.", "Ok");
            return false;
        }

        return true;
    }

    private void OnCancelar()
    {
        Suite = null;
        Application.RequestStop();
    }
}