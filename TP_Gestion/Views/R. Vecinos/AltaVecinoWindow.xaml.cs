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
using System.Windows.Shapes;
using TP_Gestion.Acceso_a_datos;
using TP_Gestion.Models;
using TP_Gestion.ViewModels;

namespace TP_Gestion.Views.R._Vecinos
{
    /// <summary>
    /// Lógica de interacción para AltaVecinoWindow.xaml
    /// </summary>
    public partial class AltaVecinoWindow : Window
    {
        private readonly AltaVecinoViewModel _viewModel;

        public AltaVecinoWindow(SQLService sqlService, Persona personaToEdit = null)
        {
            InitializeComponent();
            _viewModel = new AltaVecinoViewModel(sqlService, personaToEdit);
            DataContext = _viewModel;
            _viewModel.ActionCompleted += (sender, args) => Close();
            _viewModel.CloseRequested += (sender, args) => Close();
        }
    }
}
