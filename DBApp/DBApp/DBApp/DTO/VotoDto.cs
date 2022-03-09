using System;
using System.Collections.Generic;
using System.Text;

namespace DBApp.DTO
{
    class VotoDto
    {
        public int PeliculaID { get; set; }
        public string UsuarioID { get; set; }
        public bool Delete { get; set; }
        public bool EsActivo { get; set; }

    }
}
