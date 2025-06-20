using ReservaHospedagem.Domain;
using Terminal.Gui;

namespace ReservaHospedagem.Screens;

public class CadastroHospedeScreen : Dialog
{
    public Hospede? Hospede { get; private set; }

    private readonly TextField _textFieldDocumento;
    private readonly TextField _textFieldNome;
    private readonly TextField _textFieldSobrenome;
    private readonly TextField _textFieldEmail;
    private readonly TextField _textFieldCelular;

    public CadastroHospedeScreen() : base("Cadastro de Hóspede", 60, 20)
    {
        Hospede = null;

        int posY = 1;
        Add(new Label("Documento:") { X = 2, Y = posY });
        _textFieldDocumento = new TextField("") { X = 15, Y = posY, Width = 40 };

        posY += 2;
        Add(new Label("Nome:") { X = 2, Y = posY });
        _textFieldNome = new TextField("") { X = 15, Y = posY, Width = 40 };

        posY += 2;
        Add(new Label("Sobrenome:") { X = 2, Y = posY });
        _textFieldSobrenome = new TextField("") { X = 15, Y = posY, Width = 40 };

        posY += 2;
        Add(new Label("E-mail:") { X = 2, Y = posY });
        _textFieldEmail = new TextField("") { X = 15, Y = posY, Width = 40 };

        posY += 2;
        Add(new Label("Celular:") { X = 2, Y = posY });
        _textFieldCelular = new TextField("") { X = 15, Y = posY, Width = 40 };

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

        Add(_textFieldDocumento, _textFieldNome, _textFieldSobrenome, _textFieldEmail, _textFieldCelular, buttonSalvar, buttonCancelar);
    }

    public sealed override void Add(View view)
    {
        base.Add(view);
    }

    private void OnSalvar()
    {
        if (!DadosValidos()) return;
        
        Hospede = new Hospede
        {
            Documento = _textFieldDocumento.Text.ToString()!,
            Nome = _textFieldNome.Text.ToString()!,
            Sobrenome = _textFieldSobrenome.Text.ToString(),
            Email = _textFieldEmail.Text.ToString(),
            Celular = _textFieldCelular.Text.ToString()
        };

        Application.RequestStop();
    }

    private bool DadosValidos()
    {
        if (string.IsNullOrWhiteSpace(_textFieldDocumento.Text.ToString()))
        {
            MessageBox.ErrorQuery("Erro de Validação", "Documento é obrigatório.", "Ok");
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(_textFieldNome.Text.ToString()))
        {
            MessageBox.ErrorQuery("Erro de Validação", "Nome é obrigatórios.", "Ok");
            return false;
        }

        return true;
    }

    private void OnCancelar()
    {
        Hospede = null;
        Application.RequestStop();
    }
}