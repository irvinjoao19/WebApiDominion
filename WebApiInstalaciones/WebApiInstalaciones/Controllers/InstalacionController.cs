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

        private static string path = ConfigurationManager.AppSettings["uploadFile"];

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
                {
                    return Ok(NegocioDao.GetSync(q));
                }
                else return BadRequest("Actualizar ultima versión");                
            }
            catch (Exception)
            {
                return BadRequest("No puedes Sincronizar");
            }
        }


        [HttpPost]
        [Route("SaveRegistro")]
        public IHttpActionResult SaveRegistro()
        {
            try
            {
                //string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                var files = HttpContext.Current.Request.Files;
                var testValue = HttpContext.Current.Request.Form["data"];
                Ot r = JsonConvert.DeserializeObject<Ot>(testValue);
                Mensaje m = NegocioDao.SaveRegistro(r);
                if (m != null)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fileName = Path.GetFileName(files[i].FileName);
                        files[i].SaveAs(path + fileName);
                    }

                    return Ok(m);
                }
                else
                    return BadRequest("Error");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("SaveRegistroNew")]
        public async Task<IHttpActionResult> SaveRegistroNewAsync()
        {
            try
            {
                var files = HttpContext.Current.Request.Files;
                var testValue = HttpContext.Current.Request.Form["data"];
                Ot r = JsonConvert.DeserializeObject<Ot>(testValue);
                Mensaje m = NegocioDao.SaveRegistroNew(r);
                if (m != null)
                {
                    await SaveImage(files);
                    return Ok(m);
                }
                else
                    return BadRequest("Error");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task SaveImage(HttpFileCollection files)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < files.Count; i++)
                {
                    string fileName = Path.GetFileName(files[i].FileName);

                    files[i].SaveAs(path + fileName);
                }
            });
        }


        [HttpPost]
        [Route("SaveGps")]
        public IHttpActionResult SaveOperarioGps(EstadoOperario estadoOperario)
        {
            Mensaje mensaje = NegocioDao.SaveGps(estadoOperario);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else
                return BadRequest("Error de Envio");

        }

        [HttpPost]
        [Route("SaveMovil")]
        public IHttpActionResult SaveMovil(EstadoMovil e)
        {
            Mensaje mensaje = NegocioDao.SaveMovil(e);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else
                return BadRequest("Error de Envio");

        }

        [HttpPost]
        [Route("Proveedores")]
        public IHttpActionResult GetProveedores(Query e)
        {
            List<Proveedor> p = NegocioDao.GetProveedores(e);
            if (p != null)
            {
                return Ok(p);
            }
            else
                return BadRequest("No hay datos");
        }

        [HttpPost]
        [Route("Empresas")]
        public IHttpActionResult GetEmpresas(Query e)
        {
            List<OtReporte> p = NegocioDao.GetOtReporte(e);
            if (p != null)
            {
                return Ok(p);
            }
            else
                return BadRequest("No hay datos");

        }


        [HttpPost]
        [Route("JefeCuadrillas")]
        public IHttpActionResult GetJefeCuadrillas(Query e)
        {
            List<JefeCuadrilla> p = NegocioDao.GetJefeCuadrillas(e);
            if (p != null)
            {
                return Ok(p);
            }
            else
                return BadRequest("No hay datos");

        }

        [HttpPost]
        [Route("OtPlazo")]
        public IHttpActionResult GetOtPlazo(Query e)
        {
            List<OtPlazo> p = NegocioDao.GetOtPlazos(e);
            if (p != null)
            {
                return Ok(p);
            }
            else
                return BadRequest("No hay datos");
        }

        [HttpPost]
        [Route("OtPlazoDetalle")]
        public IHttpActionResult GetOtPlazoDetalle(Query e)
        {
            List<OtPlazoDetalle> p = NegocioDao.GetOtPlazoDetalles(e);
            if (p != null)
            {
                return Ok(p);
            }
            else
                return BadRequest("No hay datos");
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
                    files[i].SaveAs(path + fileName);
                }
                return Ok("Enviado");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("SaveOt")]
        public IHttpActionResult SaveOt(Ot o)
        {
            Mensaje m = NegocioDao.SaveRegistroNew(o);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error");
        }

        // nuevas modificaciones
        [HttpPost]
        [Route("SaveOtNew")]
        public IHttpActionResult SaveOtNew(Ot o)
        {
            Mensaje m = NegocioDao.SaveRegistroNew2(o);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error");
        }

        // ultima modificacion
        [HttpPost]
        [Route("SaveOtNew3")]
        public IHttpActionResult SaveOtNew3(Ot o)
        {
            Mensaje m = NegocioDao.SaveRegistroNew3(o);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error");
        }


        // ultima modificacion detalle agregando 3 parametros mas ...
        [HttpPost]
        [Route("SaveOtNew4")]
        public IHttpActionResult SaveOtNew4(Ot o)
        {
            Mensaje m = NegocioDao.SaveRegistroNew4(o);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error");
        }


        //agregando 2 parametros mas fecha inicial y final
        [HttpPost]
        [Route("SaveOtNew5")]
        public IHttpActionResult SaveOtNew5(Ot o)
        {
            Mensaje m = NegocioDao.SaveRegistroNew5(o);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error");
        }

    }
}
