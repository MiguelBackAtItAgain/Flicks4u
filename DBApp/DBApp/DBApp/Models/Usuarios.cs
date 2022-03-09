using System;
using System.Collections.Generic;
using System.Text;

namespace DBApp.Models
{
    class Usuarios
    {
        public int Id { get; set; }
        public string Contrasenia { get; set; }
        public string CorreoElectronico { get; set; }
        public int tipoSuscripcion { get; set; }
        public bool EsActivo { get; set; }

    }
}
