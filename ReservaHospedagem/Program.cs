using ReservaHospedagem;
using Terminal.Gui;

Application.Init();

try
{
    Application.Top.Add(new MainScreen());
    Application.Run();
}
finally
{
    Application.Shutdown();
}