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

namespace TP_Gestion.ViewModels
{
    /// <summary>
    /// Lógica de interacción para CrearProductoWindow.xaml
    /// </summary>
    public partial class CrearProductoWindow : Window
    {
        private readonly CrearProductoViewModel _viewModel;
        public CrearProductoWindow(SQLService sqlService, Producto productoToEdit = null)
        {
            InitializeComponent();
            _viewModel = new CrearProductoViewModel(sqlService, productoToEdit);
            DataContext = _viewModel;
            _viewModel.ActionCompleted += (sender, args) => Close();
            _viewModel.CloseRequested += (sender, args) => Close();
        }
    }
}
