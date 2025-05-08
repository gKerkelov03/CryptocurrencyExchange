using System.Reflection;
using System.Windows;
using Application.Abstractions;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Windows;
using SmartSalon.Application.ResultObject;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Services;

namespace Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IHost _host;

    public App()
    {
        var presentationAssembly = Assembly.GetExecutingAssembly();

        var dataLayer = typeof(DatabaseContext).Assembly;
        var applicationLayer = typeof(Result).Assembly;
        var presentationLayer = typeof(App).Assembly;
        var assemblies = new Assembly[] { dataLayer, applicationLayer, presentationLayer };

        var test1 = new PasswordHasher().HashPassword("admin123");
        var test2 = new PasswordHasher().HashPassword("user123");

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) => {
                services.SetupSqlServer();
                services.AddHttpClient();

                services.Scan(types =>
                {
                    var allTypes = types.FromAssemblies(assemblies);

                    allTypes
                        .AddClasses(@class => @class.AssignableTo(typeof(ISingletonLifetime)))
                        .AsSelfWithInterfaces()
                        .WithSingletonLifetime();

                    allTypes
                        .AddClasses(@class => @class.AssignableTo(typeof(ITransientLifetime)))
                        .AsSelfWithInterfaces()
                        .WithTransientLifetime();
                });

                services.AddTransient(typeof(IEfRepository<>), typeof(Repository<>));
            })
            .Build();
    }


    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var dbContext = _host.Services.GetRequiredService<DatabaseContext>();
        dbContext.Database.Migrate();
        

        await _host.StartAsync();

        var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
        loginWindow.Show();
    }
}
