using System;
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
using TP_Gestion.ViewModels;

namespace TP_Gestion.Views.Cuotas
{
    /// <summary>
    /// Lógica de interacción para CuotasControl.xaml
    /// </summary>
    public partial class CuotasControl : UserControl
    {
        public CuotasControl(SQLService _sqlService)
        {
            InitializeComponent();
            DataContext = new CuotasViewModel(_sqlService);
        }
    }
}
