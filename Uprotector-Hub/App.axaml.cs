using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using Uprotector_Hub.Services;
using Uprotector_Hub.Utils;
using Uprotector_Hub.ViewModels;
using Uprotector_Hub.Views;

namespace Uprotector_Hub;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Register(Locator.CurrentMutable, Locator.Current);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Locator.Current.GetService<MainWindowViewModel>().Required(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterServices(services, resolver);
        RegisterViewModels(services, resolver);
    }

    private static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            services.RegisterConstant<IEditorManagementService>(new WindowsEditorManagementService());
        }
    }

    private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.Register(() => new MainWindowViewModel(
            resolver.GetService<IEditorManagementService>().Required()
        ));
    }
}