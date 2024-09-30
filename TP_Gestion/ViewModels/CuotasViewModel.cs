using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TP_Gestion.Acceso_a_datos;
using TP_Gestion.Models;

namespace TP_Gestion.ViewModels
{
    public class CuotasViewModel : INotifyPropertyChanged
    {
        private readonly SQLService _sqlService;
        private ObservableCollection<Cuota> _cuotas;
        private string _searchText;
        private string _selectedTipoFiltro;
        private int _selectedYear;
        private Cuota _selectedCuota;


        public ObservableCollection<Cuota> Cuotas
        {
            get => _cuotas;
            set
            {
                _cuotas = value;
                OnPropertyChanged(nameof(Cuotas));
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

        public string SelectedTipoFiltro
        {
            get => _selectedTipoFiltro;
            set
            {
                _selectedTipoFiltro = value;
                OnPropertyChanged(nameof(SelectedTipoFiltro));
            }
        }

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }

        public Cuota SelectedCuota
        {
            get => _selectedCuota;
            set
            {
                _selectedCuota = value;
                OnPropertyChanged(nameof(SelectedCuota));
            }
        }

        public ObservableCollection<int> ListYears { get; set; }
        public ObservableCollection<string> ListTipos {  get; set; }

        public ICommand SearchCommand { get; private set; }
        public ICommand ClearSearchCommand { get; private set; }
        public ICommand DataGridDoubleClickCommand { get; private set; }
        public ICommand PagarCommand { get; private set; }
        public ICommand DeshacerPagoCommand { get; private set; }

        public CuotasViewModel(SQLService sqlService)
        {
            _sqlService = sqlService;
            ListTipos = new ObservableCollection<string>() { "Todos", "Pagado", "No pagado" };
            SelectedTipoFiltro = ListTipos.First();
            LoadYears();
            if (ListYears.Count > 0)
            {
                SelectedYear = ListYears.First();
            }
            LoadCuotas();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(ExecuteSearch);
            ClearSearchCommand = new RelayCommand(ExecuteClearSearch);
            DataGridDoubleClickCommand = new RelayCommand<Cuota>(ExecuteDataGridDoubleClick);
            PagarCommand = new RelayCommand<Cuota>(ExecutePagar);
            DeshacerPagoCommand = new RelayCommand<Cuota>(ExecuteDeshacerPago);
        }

        private void LoadCuotas()
        {
            Cuotas = new ObservableCollection<Cuota>(_sqlService.SelectCuotasAnual(SelectedYear));
        }
        private void LoadYears()
        {
            List<int> years = new List<int>();
            int anio = DateTime.Now.Year;
            int anioFinal = 2019;
            while(anio != anioFinal)
            {
                years.Add(anio);
                anio--;
            }
            ListYears = new ObservableCollection<int>(years);
        }

        private void ExecuteSearch()
        {
            LoadCuotas();
        }

        private void ExecuteClearSearch()
        {
            SearchText = string.Empty;
            SelectedTipoFiltro = "Todos";
            SelectedYear = DateTime.Now.Year;
        }

        private void ExecuteDataGridDoubleClick(Cuota cuota)
        {
            // Implementar la lógica para el doble clic en una fila
        }

        private void ExecutePagar(Cuota cuota)
        {
            if (cuota != null)
            {
                cuota.Pagado = true;
                HacerPago();
                OnPropertyChanged(nameof(Cuotas));
            }
        }

        private void HacerPago()
        {
            
            
            //Se debería de obtener el producto mediante una consulta
            List<LineaVenta> lineaVentas = new List<LineaVenta>() 
            { 
                new LineaVenta()
                {
                    IdProducto = 1,
                    PrecioTotal = 10,
                    PrecioTotalIva = 10,
                    Cantidad = 1,
                    PrecioUd = 10,
                }
            };
            Venta venta = new Venta()
            {
                IdPersona = SelectedCuota.Id,
                FechaVenta = DateTime.Now.Date,
                LineaVentaList = lineaVentas
            };
            bool okVenta = _sqlService.CrearVenta(venta, out long id);
            SelectedCuota.Anio = SelectedYear;
            SelectedCuota.Fecha = DateTime.Now.Date;
            SelectedCuota.IdVenta = id;
            _sqlService.CrearCouta(SelectedCuota);
        }
        private void ExecuteDeshacerPago(Cuota cuota)
        {
            if (cuota != null)
            {
                cuota.Pagado = false;
                DeshacerPago();
                OnPropertyChanged(nameof(Cuotas));
            }
        }

        private void DeshacerPago()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    
}

