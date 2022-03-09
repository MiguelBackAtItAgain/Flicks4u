﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ejemploapiflcks.DTOS
{
    public class VotoDto
    {
        public VotoDto(int peliculaid, string usuarioid, bool delete, bool esActivo) 
        {
            PeliculaID = peliculaid;
            UsuarioMail = usuarioid;
            Delete = delete;
            EsActivo = esActivo;
        }
        public int PeliculaID { get; set; }
        public string  UsuarioMail { get; set; }
        public bool Delete { get; set; }
        public bool EsActivo { get; set; }

    }
}