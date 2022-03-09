﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DTOS;
using ejemploapiflcks.Models;



namespace ejemploapiflcks.Controllers
{
    public class UsuariosController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Usuarios
        // GET: api/Usuarios?email={correoElectronico}
        public IQueryable<DTOSUsuario> GetUsuarios()
        {
            var x = Request.GetQueryNameValuePairs().Where(e => e.Key == "email").FirstOrDefault();
            var usuario = db.Usuarios;
            List<DTOSUsuario> usuarios = new List<DTOSUsuario>();
            // Recuperar un único usuario para la validación en login.
            if (x.Value != null)
            {
                foreach(Usuarios aux in usuario)
                {
                    if(aux.CorreoElectronico==x.Value)
                    {
                        DTOSUsuario user = new DTOSUsuario(aux.Id, aux.Contrasenia, aux.CorreoElectronico, aux.TipoSubscripcionID, aux.Esactivo);
                        usuarios.Add(user);
                        var uniqueuser = usuarios.AsQueryable();
                        return uniqueuser;
                    }
                }
            }
            else
            {
                //Recuperar todos los usuarios para otras funcionalidades.
                foreach (Usuarios aux in usuario)
                {
                    DTOSUsuario user = new DTOSUsuario(aux.Id, aux.Contrasenia, aux.CorreoElectronico, aux.TipoSubscripcionID, aux.Esactivo);
                    usuarios.Add(user);
                }
            }
            var query = usuarios.AsQueryable();
            return query;
        }
        [HttpGet]
       
        // GET: api/Usuarios?email=dan
        [ResponseType(typeof(Usuarios))]
        public IHttpActionResult GetUsuarios(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return Ok(usuarios);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuarios(int id, Usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarios.Id)
            {
                return BadRequest();
            }

            db.Entry(usuarios).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Usuarios
        [ResponseType(typeof(Usuarios))]
        public IHttpActionResult PostUsuarios(Usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuarios.Add(usuarios);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = usuarios.Id }, usuarios);
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuarios))]
        public IHttpActionResult DeleteUsuarios(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuarios);
            db.SaveChanges();

            return Ok(usuarios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuariosExists(int id)
        {
            return db.Usuarios.Count(e => e.Id == id) > 0;
        }
    }
}