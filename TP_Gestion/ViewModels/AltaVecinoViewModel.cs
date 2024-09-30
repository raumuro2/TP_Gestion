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
using GalaSoft.MvvmLight.Command;
using TP_Gestion.Views;

namespace TP_Gestion.ViewModels
{
    public class AltaVecinoViewModel : INotifyPropertyChanged
    {
        private readonly SQLService _sqlService;
        private Persona _persona;
        private bool _isEditMode;
        private string _statusMessage;
        public ObservableCollection<Tipo_Persona> TiposPersona { get; }

        public string WindowTitle => IsEditMode ? "Editar Vecino" : "Alta de Vecino";
        public string ActionButtonText => IsEditMode ? "Actualizar" : "Crear";

        public Tipo_Persona SelectedTipoPersona
        {
            get => TiposPersona.FirstOrDefault(t => t.TipoId == Persona?.Tipo_Persona?.TipoId);
            set
            {
                if (Persona != null && value != null)
                {
                    Persona.Tipo_Persona = value;
                    OnPropertyChanged(nameof(SelectedTipoPersona));
                }
            }
        }
        public Persona Persona
        {
            get => _persona;
            set
            {
                _persona = value;
                OnPropertyChanged(nameof(Persona));
                OnPropertyChanged(nameof(SelectedTipoPersona));
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(WindowTitle));
                OnPropertyChanged(nameof(ActionButtonText));
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }


        public ICommand ActionCommand { get; private set; }
        public ICommand CerrarCommand { get; private set; }

        public AltaVecinoViewModel(SQLService sqlService, Persona personaToEdit = null)
        {
            _sqlService = sqlService;
            TiposPersona = new ObservableCollection<Tipo_Persona>((IEnumerable<Tipo_Persona>)MainWindow.tiposPersona);

            if (personaToEdit != null)
            {
                Persona = new Persona
                {
                    Id = personaToEdit.Id,
                    Nombre = personaToEdit.Nombre,
                    Apellidos = personaToEdit.Apellidos,
                    Casa = personaToEdit.Casa,
                    FechaRegistro = personaToEdit.FechaRegistro,
                    FechaBaja = personaToEdit.FechaBaja,
                    FechaUltimaAlta = personaToEdit.FechaUltimaAlta,
                    Tipo_Persona = personaToEdit.Tipo_Persona,
                    EnAlta = personaToEdit.EnAlta
                };
                IsEditMode = true;
            }
            else
            {
                Persona = new Persona
                {
                    FechaRegistro = DateTime.Now,
                    EnAlta = true
                };
                IsEditMode = false;
            }

            ActionCommand = new RelayCommand(ExecuteAction, CanExecuteAction);
            CerrarCommand = new RelayCommand(Cerrar);
        }

        private bool CanExecuteAction()
        {
            return !string.IsNullOrWhiteSpace(Persona.Nombre) &&
                   !string.IsNullOrWhiteSpace(Persona.Apellidos) &&
                   Persona.Tipo_Persona != null;
        }

        private void ExecuteAction()
        {
            string message = IsEditMode ? "¿Desea actualizar al vecino?" : "¿Desea crear al vecino?";
            PopUp popUp = new PopUp(message, "Cancelar", "Aceptar");
            bool? result = popUp.ShowDialog();
            if (result == true)
            {
                bool ok;
                if (IsEditMode)
                {
                    ok = _sqlService.ActualizarVecino(Persona);
                }
                else
                {
                    Persona.FechaUltimaAlta = Persona.EnAlta ? Persona.FechaRegistro : null;
                    ok = _sqlService.CrearVecino(Persona);
                }

                if (!ok)
                {
                    StatusMessage = IsEditMode ? "Ha surgido un problema al actualizar el vecino" : "Ha surgido un problema al crear el vecino";
                }
                else
                {
                    StatusMessage = IsEditMode ? "Vecino actualizado con éxito" : "Vecino creado con éxito";
                    if (!IsEditMode)
                    {
                        // Reset the form for a new entry
                        Persona = new Persona
                        {
                            FechaRegistro = DateTime.Now,
                            EnAlta = true
                        };
                    }
                    OnActionCompleted();
                }
            }
        }

        private void Cerrar()
        {
            OnCloseRequested();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ActionCompleted;
        public event EventHandler CloseRequested;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnActionCompleted()
        {
            ActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCloseRequested()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    } 
}
