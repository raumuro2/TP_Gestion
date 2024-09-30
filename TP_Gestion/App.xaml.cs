using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TP_Gestion.Acceso_a_datos;

namespace TP_Gestion
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Registro de servicios
            services.AddSingleton<DBConnection>(provider => new DBConnection("Server=LAPTOP-8MIHGSJL\\SQLEXPRESS;Database=TP_Gestion;Trusted_Connection=True;"));
            services.AddSingleton<SQLService>();
            services.AddSingleton<MainWindow>();
        }
    }
}
