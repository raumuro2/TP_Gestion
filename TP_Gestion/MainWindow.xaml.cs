using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TP_Gestion.Acceso_a_datos;
using TP_Gestion.Views.Cuotas;
using TP_Gestion.Views.R._Vecinos;

namespace TP_Gestion
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SQLService _sqlService;

        public static IEnumerable tiposPersona;

        public MainWindow(SQLService sqlService)
        {
            InitializeComponent();
            _sqlService = sqlService;
            tiposPersona = _sqlService.SelectTiposPersona();

        }

        private void menuItemSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void menuItem_AltaVecino_Click(object sender, RoutedEventArgs e)
        {
            AltaVecinoWindow altaVecinoWindow = new AltaVecinoWindow(_sqlService);
            altaVecinoWindow.Show();
        }

        private void menuItem_RegistroVecinos_Click(object sender, RoutedEventArgs e)
        {
            VerRegistroControl verRegistroControl = new VerRegistroControl(_sqlService);
            contentControl.Content = verRegistroControl;
            
        }

        private void menuItem_Cuotas_Click(object sender, RoutedEventArgs e)
        {
            CuotasControl cuotasContral = new CuotasControl(_sqlService);
            contentControl.Content = cuotasContral;
        }
    }
}
