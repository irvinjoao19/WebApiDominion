using Entidades;
using Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApiInstalaciones.Controllers
{

    [RoutePrefix("api/Dominion")]
    public class InstalacionController : ApiController
    {
        private static readonly string path = ConfigurationManager.AppSettings["uploadFile"];
        private static readonly string pathPdf = ConfigurationManager.AppSettings["uploadPdf"];

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult GetLogin(Query q)
        {
            Usuario u = NegocioDao.GetOne(q);

            if (u != null)
            {
                if (u.mensaje == "Pass")
                    return BadRequest("Contraseña Incorrecta");
                else
                    return Ok(u);
            }
            else return BadRequest("Usuario no existe");

        }

        [HttpPost]
        [Route("Sync")]
        public IHttpActionResult GetSincronizar(Query q)
        {
            try
            {
                Sync s = NegocioDao.GetSync(q);
                if (s != null)
                    return Ok(NegocioDao.GetSync(q));
                else return BadRequest("Actualizar ultima versión");
            }
            catch (Exception)
            {
                return BadRequest("No puedes Sincronizar");
            }
        }

        [HttpPost]
        [Route("SaveGps")]
        public IHttpActionResult SaveOperarioGps(EstadoOperario estadoOperario)
        {
            Mensaje mensaje = NegocioDao.SaveGps(estadoOperario);
            if (mensaje != null)
                return Ok(mensaje);
            else return BadRequest("Error de Envio");

        }

        [HttpPost]
        [Route("SaveMovil")]
        public IHttpActionResult SaveMovil(EstadoMovil e)
        {
            Mensaje mensaje = NegocioDao.SaveMovil(e);
            if (mensaje != null)
                return Ok(mensaje);
            else return BadRequest("Error de Envio");
        }

        [HttpPost]
        [Route("Proveedores")]
        public IHttpActionResult GetProveedores(Query e)
        {
            List<Proveedor> p = NegocioDao.GetProveedores(e);
            if (p != null)
                return Ok(p);
            else return BadRequest("No hay datos");
        }

        [HttpPost]
        [Route("Empresas")]
        public IHttpActionResult GetEmpresas(Query e)
        {
            List<OtReporte> p = NegocioDao.GetOtReporte(e);
            if (p != null)
                return Ok(p);
            else return BadRequest("No hay datos");
        }


        [HttpPost]
        [Route("JefeCuadrillas")]
        public IHttpActionResult GetJefeCuadrillas(Query e)
        {
            List<JefeCuadrilla> p = NegocioDao.GetJefeCuadrillas(e);
            if (p != null)
                return Ok(p);
            else return BadRequest("No hay datos");
        }

        [HttpPost]
        [Route("OtPlazo")]
        public IHttpActionResult GetOtPlazo(Query e)
        {
            List<OtPlazo> p = NegocioDao.GetOtPlazos(e);
            if (p != null)
                return Ok(p);
            else return BadRequest("No hay datos");
        }

        [HttpPost]
        [Route("OtPlazoDetalle")]
        public IHttpActionResult GetOtPlazoDetalle(Query e)
        {
            List<OtPlazoDetalle> p = NegocioDao.GetOtPlazoDetalles(e);
            if (p != null)
                return Ok(p);
            else return BadRequest("No hay datos");
        }

        // nuevo para evitar problemas con las fotos
        [HttpPost]
        [Route("SaveOtPhotos")]
        public IHttpActionResult SaveInspeccionesPhoto()
        {
            try
            {
                var files = HttpContext.Current.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    string fileName = Path.GetFileName(files[i].FileName);
                    var data = fileName.Substring(fileName.Length - 3);
                    files[i].SaveAs(((data == "pdf") ? pathPdf : path)  + fileName);
                }
                return Ok("Enviado");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // ultima modificacion detalle agregando 3 parametros mas ...
        [HttpPost]
        [Route("SaveOtNew4")]
        public IHttpActionResult SaveOtNew4(Ot o)
        {
            Mensaje m = NegocioDao.SaveRegistroNew4(o);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error");
        }


        //agregando 2 parametros mas fecha inicial y final
        [HttpPost]
        [Route("SaveOtNew5")]
        public IHttpActionResult SaveOtNew5(Ot o)
        {
            Mensaje m = NegocioDao.SaveRegistroNew5(o);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error");
        }

        //nuevo formato con pdf
        [HttpPost]
        [Route("SaveOtNew6")]
        public IHttpActionResult SaveOtNew6(Ot o)
        {
            Mensaje m = NegocioDao.SaveRegistroNew6(o);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error");
        }


        [HttpPost]
        [Route("SaveOperarioGps")]
        public IHttpActionResult SaveGps(EstadoOperario estadoOperario)
        {
            Mensaje m = NegocioDao.SaveOperarioGps(estadoOperario);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error");

        }

        [HttpPost]
        [Route("SaveOperarioBattery")]
        public IHttpActionResult SaveEstadoMovil(EstadoMovil estadoMovil)
        {
            Mensaje m = NegocioDao.SaveEstadoMovil(estadoMovil);
            if (m != null)
                return Ok(m);
            else return BadRequest("Error");
        }

    }
}
