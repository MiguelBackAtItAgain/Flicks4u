using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DBApp.Models
{
    class peliculaTransformada
    {
        public peliculaTransformada(int peliculaId, string nombre, string sinopsis, DateTime lanzamiento, string escritores, string descripcion, ImageSource imagentransformada, string generoNombre, int cantVotos)
        {
            peliculaID = peliculaId;
            Nombre = nombre;
            Sinopsis = sinopsis;
            Lanzamiento = lanzamiento;
            Escritores = escritores;
            Descripcion = descripcion;
            Imagen = imagentransformada;
            GeneroNombre = generoNombre;
            CantVotos = cantVotos;
        }

        public int peliculaID { get; set; }
        public string Nombre { get; set; }
        public string Sinopsis { get; set; }
        public DateTime Lanzamiento { get; set; }
        public string Escritores { get; set; }
        public string Descripcion { get; set; }
        public ImageSource Imagen { get; set; }
        public string GeneroNombre { get; set; }
        public int CantVotos { get; set; }
    }
}
