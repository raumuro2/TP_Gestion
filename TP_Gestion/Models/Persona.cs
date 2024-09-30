using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Models
{
    public class Persona : INotifyPropertyChanged
    {
        public Persona() { }

        public Persona(string nombre, string apellidos, string casa, DateTime? fecha_registro, 
            DateTime? fecha_baja, Tipo_Persona tipo_Persona, bool en_alta)
        {
            Nombre = nombre;
            Apellidos = apellidos;
            Casa = casa;
            FechaRegistro = fecha_registro;
            FechaBaja = fecha_baja;
            FechaUltimaAlta = en_alta ? fecha_registro : null; //Si esta en alta es igual a fecha_registro
            Tipo_Persona = tipo_Persona;
            EnAlta = en_alta;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        private bool _isModified = false;
        public bool IsModified
        {
            get => _isModified;
            private set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    OnPropertyChanged(nameof(IsModified));
                }
            }
        }

        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }

        private string _apellidos;
        public string Apellidos
        {
            get => _apellidos;
            set
            {
                if (_apellidos != value)
                {
                    _apellidos = value;
                    OnPropertyChanged(nameof(Apellidos));
                }
            }
        }

        private string _casa;
        public string Casa
        {
            get => _casa;
            set
            {
                if (_casa != value)
                {
                    _casa = value;
                    OnPropertyChanged(nameof(Casa));
                }
            }
        }

        private DateTime? _fechaRegistro;
        public DateTime? FechaRegistro
        {
            get => _fechaRegistro;
            set
            {
                if (_fechaRegistro != value)
                {
                    _fechaRegistro = value;
                    OnPropertyChanged(nameof(FechaRegistro));
                }
            }
        }

        private DateTime? _fechaBaja;
        public DateTime? FechaBaja
        {
            get => _fechaBaja;
            set
            {
                if (_fechaBaja != value)
                {
                    _fechaBaja = value;
                    OnPropertyChanged(nameof(FechaBaja));
                }
            }
        }

        private DateTime? _fechaUltimaAlta;
        public DateTime? FechaUltimaAlta
        {
            get => _fechaUltimaAlta;
            set
            {
                if (_fechaUltimaAlta != value)
                {
                    _fechaUltimaAlta = value;
                    OnPropertyChanged(nameof(FechaUltimaAlta));
                }
            }
        }

        private Tipo_Persona _tipo_Persona;
        public Tipo_Persona Tipo_Persona
        {
            get => _tipo_Persona;
            set
            {
                if (_tipo_Persona != value)
                {
                    _tipo_Persona = value;
                    OnPropertyChanged(nameof(Tipo_Persona));
                }
            }
        }

        private bool _enAlta;
        public bool EnAlta
        {
            get => _enAlta;
            set
            {
                if (_enAlta != value)
                {
                    _enAlta = value;
                    OnPropertyChanged(nameof(EnAlta));
                    IsModified = true;
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void ResetModifiedState()
        {
            IsModified = false;
        }
    }
}
