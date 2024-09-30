using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TP_Gestion.Models;
using TP_Gestion.ViewModels;

namespace TP_Gestion.Views.R._Vecinos
{
    /// <summary>
    /// Lógica de interacción para VerRegistroControl.xaml
    /// </summary>
    public partial class VerRegistroControl : UserControl
    {
        public VerRegistroControl(SQLService sqlService)
        {
            InitializeComponent();
            DataContext = new PersonaViewModel(sqlService);
        }
    }
}
