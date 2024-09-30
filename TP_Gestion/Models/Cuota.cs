using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Gestion.Models
{
    public class Cuota : INotifyPropertyChanged
    {
        public Cuota()
        {
        }

        public Cuota(int id, string nombre, string apellidos, string casa, DateTime fechaRegistro, 
            bool pagado, long idVenta)
        {
            Id = id;
            IdVenta = idVenta;
            Nombre = nombre;
            Apellidos = apellidos;
            Casa = casa;
            FechaRegistro = fechaRegistro;
            _pagado = pagado;
        }

        public int Id { get; set; }
        public long IdVenta { get; set; }
        public int Anio { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Casa { get; set; }
        public DateTime FechaRegistro { get; set; }
        private bool _pagado;

        public bool Pagado
        {
            get => _pagado;
            set
            {
                if (_pagado != value)
                {
                    _pagado = value;
                    OnPropertyChanged(nameof(Pagado));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
