using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TP_Gestion.Models;
using GalaSoft.MvvmLight.Command;
using TP_Gestion.Acceso_a_datos;
using TP_Gestion.Views;
using System.IO;

namespace TP_Gestion.ViewModels
{
    public class CrearProductoViewModel : INotifyPropertyChanged
    {
        private Producto _producto;
        private readonly SQLService _sqlService;

        private bool _isEditMode;
        public string WindowTitle => IsEditMode ? "Editar Producto" : "Crear Producto";
        public string ActionButtonText => IsEditMode ? "Actualizar" : "Crear";
        private string _statusMessage;


        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ActionCompleted;
        public event EventHandler CloseRequested;
        public ICommand CerrarCommand { get; private set; }
        public ICommand ActionCommand { get; private set; }
        public ICommand CargarImagenCommand { get; private set; }

        public CrearProductoViewModel(SQLService sqlService, Producto producto = null)
        {
            _sqlService = sqlService;
            if (producto != null)
            {
                Producto = new Producto
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    PrecioIva = producto.PrecioIva,
                    SegStock = producto.SegStock,
                    Baja = producto.Baja,
                    FechaCreacion = producto.FechaCreacion,
                    FechaModificacion = producto.FechaModificacion,
                    Stock = producto.Stock,
                    Imagen = producto.Imagen,
                };
                IsEditMode = true;
            }
            else
            {
                Producto = new Producto
                {
                    FechaCreacion = DateTime.Now,
                    Baja = false,
                    SegStock = false
                };
                IsEditMode = false;
            }

            InitializeCommands();
        }

        public Producto Producto
        {
            get => _producto;
            set
            {
                _producto = value;
                OnPropertyChanged();
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
        private void InitializeCommands()
        {
            CerrarCommand = new RelayCommand(CerrarVentana);
            ActionCommand = new RelayCommand(GuardarProducto, CanExecuteAction);
            CargarImagenCommand = new RelayCommand(CargarImagen);
        }

        private void CerrarVentana()
        {
            if (Application.Current.Windows.Count > 0)
            {
                Window currentWindow = Application.Current.Windows[Application.Current.Windows.Count - 1];
                currentWindow.Close();
            }
        }
        //TODO
        private bool CanExecuteAction()
        {
            return !string.IsNullOrWhiteSpace(Producto.Nombre) &&
                   !string.IsNullOrWhiteSpace(Producto.Precio.ToString());
        }
        private void GuardarProducto()
        {
            string message = IsEditMode ? "¿Desea actualizar el producto?" : "¿Desea crear el producto?";
            PopUp popUp = new PopUp(message, "Cancelar", "Aceptar");
            bool? result = popUp.ShowDialog();
            if (result == true)
            {
                bool ok;
                if (IsEditMode)
                {
                    ok = _sqlService.ActualizarProducto(Producto);
                }
                else
                {
                    ok = _sqlService.CrearProducto(Producto);
                }

                if (!ok)
                {
                    StatusMessage = IsEditMode ? "Ha surgido un problema al actualizar el producto" : "Ha surgido un problema al crear el producto";
                }
                else
                {
                    StatusMessage = IsEditMode ? "Producto actualizado con éxito" : "Producto creado con éxito";
                    if (!IsEditMode)
                    {
                        Producto = new Producto
                        {
                            FechaCreacion = DateTime.Now,
                            Baja = false
                        };
                    }
                    OnActionCompleted();
                }
            }
        }

        private void CargarImagen()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Seleccionar imagen del producto"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    if (Producto.Imagen == null)
                        Producto.Imagen = new Imagen();

                    Producto.Imagen.Image = imageBytes;
                    Producto.Imagen.Nombre = Path.GetFileName(openFileDialog.FileName);
                    Producto.Imagen.Ruta = openFileDialog.FileName;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar la imagen: {ex.Message}",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
