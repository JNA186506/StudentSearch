using Microsoft.Extensions.DependencyInjection;

namespace StudentSearch;

public partial class App : Application {
    private const int WindowHeight = 800;
    private const int WindowWidth = 800;
    public App() {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        var window = new Window(new AppShell());
        window.Height = WindowHeight;
        window.Width = WindowWidth;
        return window;
    }
}