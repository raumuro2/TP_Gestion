using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows;
using TP_Gestion.Models;
using GalaSoft.MvvmLight.Command;
using TP_Gestion.Acceso_a_datos;
using TP_Gestion.Views;
using TP_Gestion.Views.R._Vecinos;

namespace TP_Gestion.ViewModels
{
    public class VerProductosViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Producto> _productos;
        private Producto _selectedProducto;
        private string _searchText;
        private decimal? _precioMin;
        private decimal? _precioMax;
        private bool _bajaChecked;
        private readonly SQLService _sqlService;



        public ObservableCollection<Producto> Productos
        {
            get => _productos;
            set
            {
                _productos = value;
                OnPropertyChanged(nameof(Productos));
            }
        }

        public Producto SelectedProducto
        {
            get => _selectedProducto;
            set
            {
                _selectedProducto = value;
                OnPropertyChanged(nameof(SelectedProducto));
                //CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public decimal? PrecioMin
        {
            get => _precioMin;
            set
            {
                _precioMin = value;
                OnPropertyChanged(nameof(PrecioMin));
            }
        }

        public decimal? PrecioMax
        {
            get => _precioMax;
            set
            {
                _precioMax = value;
                OnPropertyChanged(nameof(PrecioMax));
            }
        }

        public bool BajaChecked
        {
            get => _bajaChecked;
            set
            {
                _bajaChecked = value;
                OnPropertyChanged(nameof(BajaChecked));
            }
        }

        // Comandos
        public ICommand SearchCommand { get; private set; }
        public ICommand ClearSearchCommand { get; private set; }
        public ICommand CreateProductoCommand { get; private set; }
        public ICommand DeleteProductoCommand { get; private set; }
        public ICommand UpdateProductoCommand { get; private set; }
        public ICommand DataGridDoubleClickCommand { get; private set; }

        public VerProductosViewModel(SQLService sqlService)
        {
            _sqlService = sqlService;
            InitializeCommands();
            LoadProductos();
        }

        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(Search);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            CreateProductoCommand = new RelayCommand(CreateProducto);
            DeleteProductoCommand = new RelayCommand(DeleteProducto, CanProductoAction);
            UpdateProductoCommand = new RelayCommand(UpdateProducto, CanProductoAction);
            DataGridDoubleClickCommand = new RelayCommand<Producto>(DataGridDoubleClick);

        }
        private void LoadProductos()
        {
            var productos = _sqlService.SelectVerProductos(SearchText, PrecioMin, PrecioMax, BajaChecked);
            Productos = new ObservableCollection<Producto>(productos);
        }

        private void Search()
        {
            LoadProductos();
        }

        private void ClearSearch()
        {
            SearchText = string.Empty;
            PrecioMin = null;
            PrecioMax = null;
            BajaChecked = false;
            LoadProductos();
        }

        private void CreateProducto()
        {
            //AltaVecinoWindow av = new AltaVecinoWindow(_sqlService);
            //av.ShowDialog();
        }

        private void DeleteProducto()
        {
            if (SelectedProducto == null) return;

            PopUp popUp = new PopUp("¿Desea borrar el producto?", "Cancelar", "Aceptar");
            bool? result = popUp.ShowDialog();
            bool ok = true;

            if (result == true)
            {
                ok = _sqlService.EliminarVecino(SelectedProducto.Id);
            }

            if (!ok)
            {
                MessageBox.Show("Ha surgido un problema al eliminar el producto");
            }
        }
        private bool CanProductoAction()
        {
            return SelectedProducto!= null;
        }
        private void UpdateProducto()
        {
            //AltaVecinoWindow av = new AltaVecinoWindow(_sqlService);
            //av.ShowDialog();
        }

        private void DataGridDoubleClick(Producto producto)
        {
            if (producto != null)
            {
                //var window = new altavecinowindow(_sqlservice, persona);
                //window.showdialog();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}