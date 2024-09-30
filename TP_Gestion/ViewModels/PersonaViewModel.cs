using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TP_Gestion.Acceso_a_datos;
using TP_Gestion.Models;
using TP_Gestion.Views;
using GalaSoft.MvvmLight.Command;
using TP_Gestion.Views.R._Vecinos;

namespace TP_Gestion.ViewModels
{
    public class PersonaViewModel : INotifyPropertyChanged
    {
        private readonly SQLService _sqlService;
        private ObservableCollection<Persona> _personas;
        private Persona _selectedPersona;
        private string _searchText;
        private bool? _altaChecked=true;
        private Tipo_Persona _selectedTipoPersona;

        public ObservableCollection<Persona> Personas
        {
            get => _personas;
            set
            {
                _personas = value;
                OnPropertyChanged(nameof(Personas));
            }
        }

        public Persona SelectedPersona
        {
            get => _selectedPersona;
            set
            {
                _selectedPersona = value;
                OnPropertyChanged(nameof(SelectedPersona));
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

        public bool? AltaChecked
        {
            get => _altaChecked;
            set
            {
                _altaChecked = value;
                OnPropertyChanged(nameof(AltaChecked));
            }
        }

        public Tipo_Persona SelectedTipoPersona
        {
            get => _selectedTipoPersona;
            set
            {
                _selectedTipoPersona = value;
                OnPropertyChanged(nameof(SelectedTipoPersona));
            }
        }

        public ObservableCollection<Tipo_Persona> TiposPersona { get; set; }

        public ICommand SearchCommand { get; private set; }
        public ICommand ClearSearchCommand { get; private set; }
        public ICommand CreateVecinoCommand { get; private set; }
        public ICommand DeleteVecinoCommand { get; private set; }
        public ICommand SaveRegistroCommand { get; private set; }
        public ICommand DataGridDoubleClickCommand { get; private set; }

        public PersonaViewModel(SQLService sqlService)
        {
            _sqlService = sqlService;
            TiposPersona = new ObservableCollection<Tipo_Persona>((IEnumerable<Tipo_Persona>)MainWindow.tiposPersona);
            if(TiposPersona.Count > 0)
            {
                _selectedTipoPersona = TiposPersona.First();    
            }
            InitializeCommands();
            LoadPersonas();
        }

        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(Search);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            CreateVecinoCommand = new RelayCommand(CreateVecino);
            DeleteVecinoCommand = new RelayCommand(DeleteVecino, CanDeleteVecino);
            SaveRegistroCommand = new RelayCommand(SaveRegistro);
            DataGridDoubleClickCommand = new RelayCommand<Persona>(OnDataGridItemDoubleClick);

        }

        private void LoadPersonas()
        {
            int idTipo = SelectedTipoPersona?.TipoId ?? 0;
            var personas = _sqlService.SelectRegistroVecinos(idTipo, SearchText, AltaChecked);
            Personas = new ObservableCollection<Persona>(personas);
        }

        private void Search()
        {
            LoadPersonas();
            ResetModifiedStates();
        }

        private void ClearSearch()
        {
            SearchText = string.Empty;
            AltaChecked = null;
            SelectedTipoPersona = null;
            LoadPersonas();
        }

        private void CreateVecino()
        {
            AltaVecinoWindow av = new AltaVecinoWindow(_sqlService);
            av.ShowDialog();
        }

        private void DeleteVecino()
        {
            if (SelectedPersona == null) return;

            PopUp popUp = new PopUp("¿Desea borrar al vecino?", "Cancelar", "Aceptar");
            bool? result = popUp.ShowDialog();
            bool ok = true;

            if (result == true)
            {
                ok = _sqlService.EliminarVecino(SelectedPersona.Id);
            }

            if (!ok)
            {
                MessageBox.Show("Ha surgido un problema al eliminar el vecino");
            }
        }

        private bool CanDeleteVecino()
        {
            return SelectedPersona != null;
        }

        private void SaveRegistro()
        {
            var modifiedPersons = GetModifiedPersonas();
            bool ok = _sqlService.ActualizarEnAltaVecinos(modifiedPersons);
            if (ok)
            {
                ResetModifiedStates();
            }
        }

        private void OnDataGridItemDoubleClick(Persona persona)
        {
            if (persona != null)
            {
                var window = new AltaVecinoWindow(_sqlService, persona);
                window.ShowDialog();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private List<Persona> GetModifiedPersonas()
        {
            return Personas.Where(p => p.IsModified).ToList();
        }
        private void ResetModifiedStates()
        {
            foreach (var persona in Personas)
            {
                persona.ResetModifiedState();
            }
        }
    }
}
