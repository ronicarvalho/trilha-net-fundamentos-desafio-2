# Sistema de Reserva de Hospedagem

## Visão Geral

O **Sistema de Reserva de Hospedagem** é uma aplicação console desenvolvida em .NET 6 que utiliza a biblioteca Terminal.Gui para criar uma interface de usuário rica em modo texto. O sistema permite gerenciar reservas de hotel, incluindo cadastro de hóspedes, suítes e efetivação de reservas.

## Arquitetura do Sistema

### Tecnologias Utilizadas

- **.NET 6.0**: Framework principal da aplicação
- **Terminal.Gui**: Biblioteca para criação de interfaces de usuário em modo texto
- **C# 10**: Linguagem de programação com recursos modernos

### Estrutura do Projeto

```
ReservaHospedagem/
├── Domain/                 # Entidades de domínio
│   ├── Hospede.cs         # Entidade Hóspede
│   ├── Suite.cs           # Entidade Suíte
│   ├── Reserva.cs         # Entidade Reserva
│   └── Estadia.cs         # Entidade Estadia
├── Screens/               # Telas da aplicação
│   ├── MainScreen.cs      # Tela principal
│   ├── CadastroHospedeScreen.cs
│   ├── CadastroSuiteScreen.cs
│   └── ReservaScreen.cs
├── Program.cs             # Ponto de entrada
└── ReservaHospedagem.csproj
```

## Entidades de Domínio

### Hóspede

Representa um hóspede do hotel com as seguintes propriedades:

- **Documento** (string): Documento de identificação (obrigatório)
- **Nome** (string): Nome do hóspede (obrigatório)
- **Sobrenome** (string?): Sobrenome do hóspede (opcional)
- **Email** (string?): Endereço de e-mail (opcional)
- **Celular** (string?): Número de celular (opcional)

### Suíte

Representa uma suíte do hotel:

- **Andar** (uint): Número do andar
- **Quarto** (uint): Número do quarto
- **Capacidade** (ushort): Capacidade máxima de pessoas
- **Diaria** (decimal): Valor da diária
- **Ocupado** (bool): Status de ocupação

### Reserva

Entidade principal que gerencia uma reserva:

- **Localizador** (string): Código único da reserva
- **Reservante** (Hospede): Hóspede responsável pela reserva
- **Hospedes** (List<Hospede>): Lista de hóspedes da reserva
- **Estadia** (Estadia): Período de estadia
- **Suites** (List<Suite>): Suítes reservadas
- **TempoEstadia** (ushort): Duração em dias
- **QuantidadeHospedes** (ushort): Número total de hóspedes
- **ValorReserva** (decimal): Valor total da reserva

#### Métodos da Reserva

- `AdicionarHospede(Hospede hospede)`: Adiciona um hóspede à reserva
- `AdicionarSuite(Suite suite)`: Adiciona uma suíte à reserva
- `AdicionarReservante(Hospede hospede)`: Define o responsável pela reserva
- `CalcularReserva()`: Calcula o valor total da reserva

### Estadia

Representa o período de hospedagem:

- **Entrada** (DateTime): Data e hora de entrada
- **Saida** (DateTime): Data e hora de saída

## Funcionalidades

### 1. Tela Principal (MainScreen)

A tela principal oferece:

- **Menu de navegação** com opções organizadas por categoria
- **Barra de status** com atalhos de teclado
- **Dados pré-carregados** para demonstração (9 hóspedes e 196 suítes)

#### Menus Disponíveis

**Cadastro:**

- Hóspedes (F2)
- Suítes

**Reservas:**

- Efetuar (F3)
- Consultar (F4)
- Alterar
- Cancelar

**Sair:**

- Finalizar aplicação

### 2. Cadastro de Hóspedes

**Campos do formulário:**

- Documento (obrigatório)
- Nome (obrigatório)
- Sobrenome
- E-mail
- Celular

**Validações:**

- Documento não pode estar vazio
- Nome não pode estar vazio

### 3. Cadastro de Suítes

**Campos do formulário:**

- Andar (obrigatório, numérico)
- Quarto (obrigatório, numérico)
- Capacidade (obrigatório, numérico)
- Diária (obrigatório, decimal)
- Ocupado (checkbox)

**Validações:**

- Todos os campos numéricos devem ter valores válidos
- Andar e Quarto devem ser números inteiros positivos

### 4. Sistema de Reservas

**Funcionalidades principais:**

- Geração automática de localizador (6 caracteres alfanuméricos)
- Seleção de reservante
- Adição de múltiplos hóspedes
- Seleção de múltiplas suítes
- Cálculo automático de valores
- Sistema de desconto para estadias longas

**Regras de Negócio:**

- Desconto de 10% para estadias superiores a 10 dias
- Validação de datas (saída deve ser posterior à entrada)
- Verificação de disponibilidade de hóspedes e suítes

## Interface de Usuário

### Características da UI

- **Interface em modo texto** com aparência profissional
- **Navegação por teclado** com atalhos funcionais
- **Diálogos modais** para operações específicas
- **Validação em tempo real** com mensagens de erro
- **Layout responsivo** que se adapta ao tamanho do terminal

### Atalhos de Teclado

- **ESC**: Sair da aplicação
- **F2**: Cadastrar hóspede
- **F3**: Efetuar reserva
- **F4**: Consultar reserva
- **F6**: Pesquisar/Selecionar itens
- **F7**: Calcular reserva
- **F8**: Salvar reserva

## Instalação e Execução

### Pré-requisitos

- .NET 6.0 SDK ou superior
- Terminal com suporte a caracteres Unicode

### Passos para Execução

1. **Clone o repositório:**

   ```bash
   git clone <url-do-repositorio>
   cd ReservaHospedagem
   ```

2. **Restaure as dependências:**

   ```bash
   dotnet restore
   ```

3. **Execute a aplicação:**

   ```bash
   dotnet run
   ```

### Compilação

```bash
# Compilar em modo Debug
dotnet build

# Compilar em modo Release
dotnet build -c Release

# Publicar aplicação
dotnet publish -c Release -o ./publish
```

## Dados de Demonstração

A aplicação inclui dados pré-carregados para facilitar os testes:

### Hóspedes (9 registros)

- Documento: Formato "26X78X7682X" (onde X é o índice)
- Nome: "Hospede X da Silva"
- Email: "<hospede.X@gmail.com>"
- Celular: "18 9234X6757X"

### Suítes (196 registros)

- **28 andares** (1 a 28)
- **7 quartos por andar** (1 a 7)
- **Capacidade**: Aleatória entre 1 e 9 pessoas
- **Diária**: Aleatória entre R$ 40,00 e R$ 150,00
- **Status**: Todas inicialmente disponíveis

## Limitações Conhecidas

1. **Persistência**: Os dados não são salvos permanentemente
2. **Funcionalidades incompletas**:
   - Consultar reserva
   - Alterar reserva
   - Cancelar reserva
3. **Validações**: Algumas validações de negócio podem ser aprimoradas
4. **Relatórios**: Não há funcionalidade de relatórios

## Possíveis Melhorias

### Funcionalidades

- Implementar persistência de dados (banco de dados)
- Completar funcionalidades de consulta e alteração
- Adicionar relatórios gerenciais
- Implementar sistema de check-in/check-out
- Adicionar gestão de pagamentos

### Técnicas

- Implementar padrão Repository
- Adicionar injeção de dependência
- Criar testes unitários
- Implementar logging
- Adicionar configurações externas

### Interface

- Melhorar responsividade
- Adicionar temas de cores
- Implementar busca avançada
- Adicionar paginação nas listas

## Contribuição

Para contribuir com o projeto:

1. Faça um fork do repositório
2. Crie uma branch para sua feature (`git checkout -b feature/nova-funcionalidade`)
3. Commit suas mudanças (`git commit -am 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/nova-funcionalidade`)
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

---

**Desenvolvido como parte do desafio da trilha .NET Fundamentals**
