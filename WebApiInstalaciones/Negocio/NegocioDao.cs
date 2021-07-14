using Entidades;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class NegocioDao
    {
        private static string db = ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public static Usuario GetOne(Query q)
        {
            try
            {
                Usuario u = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_M_GetLogin", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = q.login;
                        cmd.Parameters.Add("@version", SqlDbType.VarChar).Value = q.version;

                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            u = new Usuario();
                            if (q.pass == dr.GetString(7).ToLower())
                            {
                                u.usuarioId = dr.GetInt32(0);
                                u.nroDoc = dr.GetString(1);
                                u.apellidos = dr.GetString(2);
                                u.nombres = dr.GetString(3);
                                u.email = dr.GetString(4);
                                u.tipoUsuarioId = dr.GetInt32(5);
                                u.perfilId = dr.GetInt32(6);
                                u.pass = dr.GetString(7);
                                u.estado = dr.GetInt32(8);
                                u.personalId = dr.GetInt32(9);
                                u.empresaId = dr.GetInt32(10);
                                u.nombreEmpresa = dr.GetString(11);
                                u.mensaje = "Go";

                                // Accesos
                                SqlCommand cmdA = cn.CreateCommand();
                                cmdA.CommandTimeout = 0;
                                cmdA.CommandType = CommandType.StoredProcedure;
                                cmdA.CommandText = "DSIGE_PROY_M_ListaMenus";
                                cmdA.Parameters.Add("@id_Usuario", SqlDbType.Int).Value = u.usuarioId;
                                SqlDataReader drV = cmdA.ExecuteReader();
                                if (drV.HasRows)
                                {
                                    List<Accesos> a = new List<Accesos>();
                                    while (drV.Read())
                                    {
                                        a.Add(new Accesos()
                                        {
                                            opcionId = drV.GetInt32(0),
                                            nombre = drV.GetString(1),
                                            usuarioId = drV.GetInt32(2)
                                        });
                                    }
                                    u.accesos = a;
                                }
                            }
                            else
                            {
                                u.mensaje = "Pass";
                            }
                        }
                    }
                    cn.Close();
                }
                return u;

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public static string EncriptarClave(string cExpresion, bool bEncriptarCadena)
        {
            string cResult = "";
            string cPatron = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwwyz";
            string cEncrip = "^çºªæÆöûÿø£Ø×ƒ¬½¼¡«»ÄÅÉêèï7485912360^çºªæÆöûÿø£Ø×ƒ¬½¼¡«»ÄÅÉêèï";


            if (bEncriptarCadena == true)
            {
                cResult = CHRTRAN(cExpresion, cPatron, cEncrip);
            }
            else
            {
                cResult = CHRTRAN(cExpresion, cEncrip, cPatron);
            }

            return cResult;

        }
        public static string CHRTRAN(string cExpresion, string cPatronBase, string cPatronReemplazo)
        {
            string cResult = "";

            int rgChar;
            int nPosReplace;

            for (rgChar = 1; rgChar <= Strings.Len(cExpresion); rgChar++)
            {
                nPosReplace = Strings.InStr(1, cPatronBase, Strings.Mid(cExpresion, rgChar, 1));

                if (nPosReplace == 0)
                {
                    nPosReplace = rgChar;
                    cResult = cResult + Strings.Mid(cExpresion, nPosReplace, 1);
                }
                else
                {
                    if (nPosReplace > cPatronReemplazo.Length)
                    {
                        nPosReplace = rgChar;
                        cResult = cResult + Strings.Mid(cExpresion, nPosReplace, 1);
                    }
                    else
                    {
                        cResult = cResult + Strings.Mid(cPatronReemplazo, nPosReplace, 1);
                    }
                }
            }
            return cResult;
        }
        public static Sync GetSync(Query q)
        {
            try
            {
                Sync s = new Sync();

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    // Version
                    SqlCommand cmdVersion = con.CreateCommand();
                    cmdVersion.CommandTimeout = 0;
                    cmdVersion.CommandType = CommandType.StoredProcedure;
                    cmdVersion.CommandText = "Movil_GetVersion";
                    cmdVersion.Parameters.Add("@version", SqlDbType.VarChar).Value = q.version;

                    SqlDataReader drVersion = cmdVersion.ExecuteReader();
                    if (!drVersion.HasRows)
                    {
                        drVersion.Close();
                        return null;
                    }
                    else
                    {
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "DSIGE_PROY_M_Lista_OT";
                        cmd.Parameters.Add("@id_Empresa", SqlDbType.Int).Value = q.empresaId;
                        cmd.Parameters.Add("@id_Personal", SqlDbType.Int).Value = q.personalId;
                        var drV = cmd.ExecuteReader();
                        if (drV.HasRows)
                        {
                            List<Ot> v = new List<Ot>();
                            while (drV.Read())
                            {
                                var o = new Ot
                                {
                                    identity = drV.GetInt32(0),
                                    otId = drV.GetInt32(0),
                                    tipoOrdenId = drV.GetInt32(1),
                                    nombreTipoOrden = drV.GetString(2),
                                    servicioId = drV.GetInt32(3),
                                    nombreArea = drV.GetString(4),
                                    nroObra = drV.GetString(5),
                                    direccion = drV.GetString(6),
                                    distritoId = drV.GetInt32(7),
                                    nombreDistritoId = drV.GetString(8),
                                    referenciaOt = drV.GetString(9),
                                    descripcionOt = drV.GetString(10),
                                    fechaRegistro = drV.GetDateTime(11).ToString("dd/MM/yyyy"),
                                    fechaAsignacion = drV.GetDateTime(12).ToString("dd/MM/yyyy"),
                                    horaAsignacion = drV.GetString(13),
                                    empresaId = drV.GetInt32(14),
                                    nombreEmpresa = drV.GetString(15),
                                    tipoEmpresa = drV.GetString(16),
                                    personalJCId = drV.GetInt32(17),
                                    nombreJO = drV.GetString(18),
                                    otOrigenId = drV.GetInt32(19),
                                    estadoId = drV.GetInt32(20),
                                    nombreEstado = drV.GetString(21),
                                    vencimiento = drV.GetString(22),
                                    latitud = drV.GetString(23),
                                    longitud = drV.GetString(24),

                                    fotoCabecera = drV.GetString(25),
                                    fotoAnterior = drV.GetString(26),
                                    distritoIdGps = drV.GetInt32(27),
                                    suministroTD = drV.GetString(28),
                                    nroSed = drV.GetString(29),
                                    viajeIndebido = drV.GetInt32(30),


                                    observacion = "",
                                    motivoPrioridadId = 0,
                                    nombrePrioridad = "",
                                    observaciones = "",
                                    ordenamientoOt = 0,
                                    usuarioId = 0,
                                    estado = 2,
                                    fechaInicioTrabajo = "",
                                    fechaFinTrabajo = "",
                                    urlPdf = ""
                                    

                                };

                                SqlCommand cmdOD = con.CreateCommand();
                                cmdOD.CommandTimeout = 0;
                                cmdOD.CommandType = CommandType.StoredProcedure;
                                cmdOD.CommandText = "DSIGE_PROY_M_Lista_OT_Detalle";
                                cmdOD.Parameters.Add("@otId", SqlDbType.Int).Value = o.identity;
                                var drOD = cmdOD.ExecuteReader();
                                if (drOD.HasRows)
                                {
                                    List<OtDetalle> ot = new List<OtDetalle>();
                                    while (drOD.Read())
                                    {
                                        var detalle = new OtDetalle
                                        {
                                            otDetalleId = drOD.GetInt32(0),
                                            otId = drOD.GetInt32(1),
                                            tipoTrabajoId = drOD.GetInt32(2),
                                            tipoMaterialId = drOD.GetInt32(3),
                                            tipoDesmonteId = drOD.GetInt32(4),
                                            largo = drOD.GetDecimal(5),
                                            ancho = drOD.GetDecimal(6),
                                            espesor = drOD.GetDecimal(7),
                                            total = drOD.GetDecimal(8),
                                            nroPlaca = drOD.GetString(9),
                                            m3Vehiculo = drOD.GetDecimal(10),
                                            estado = 3,
                                            latitud = drOD.GetString(12),
                                            longitud = drOD.GetString(13),
                                            nombreTipoMaterial = drOD.GetString(14),
                                            cantPanos = (float)drOD.GetDouble(15),
                                            medHorizontal = drOD.GetDecimal(16),
                                            medVertical = drOD.GetDecimal(17)
                                        };

                                        SqlCommand cmdF = con.CreateCommand();
                                        cmdF.CommandTimeout = 0;
                                        cmdF.CommandType = CommandType.StoredProcedure;
                                        cmdF.CommandText = "DSIGE_PROY_M_Lista_OT_Photo";
                                        cmdF.Parameters.Add("@otDetalleId", SqlDbType.Int).Value = detalle.otDetalleId;
                                        var drF = cmdF.ExecuteReader();
                                        if (drF.HasRows)
                                        {
                                            List<OtPhoto> f = new List<OtPhoto>();
                                            while (drF.Read())
                                            {
                                                f.Add(new OtPhoto()
                                                {
                                                    otPhotoId = drF.GetInt32(0),
                                                    otDetalleId = drF.GetInt32(1),
                                                    nombrePhoto = drF.GetString(2),
                                                    urlPhoto = drF.GetString(3),                                                   
                                                    estado = 0
                                                });
                                            }
                                            detalle.photos = f;
                                        }
                                        drF.Close();
                                        ot.Add(detalle);
                                    }
                                    o.detalles = ot;
                                }
                                drOD.Close();
                                v.Add(o);
                            }
                            s.ots = v;
                        }
                        drV.Close();

                        SqlCommand cmdC = con.CreateCommand();
                        cmdC.CommandTimeout = 0;
                        cmdC.CommandType = CommandType.StoredProcedure;
                        cmdC.CommandText = "DSIGE_PROY_M_GetGrupo_New";
                        cmdC.Parameters.Add("@id_Empresa", SqlDbType.Int).Value = q.empresaId;
                        var drC = cmdC.ExecuteReader();
                        if (drC.HasRows)
                        {
                            List<Grupo> p = new List<Grupo>();
                            while (drC.Read())
                            {
                                p.Add(new Grupo()
                                {
                                    grupoId = drC.GetInt32(0),
                                    descripcion = drC.GetString(1),
                                    servicioId = drC.GetInt32(2)
                                });
                            }
                            s.groups = p;
                        }
                        drC.Close();

                        SqlCommand cmdE = con.CreateCommand();
                        cmdE.CommandTimeout = 0;
                        cmdE.CommandType = CommandType.StoredProcedure;
                        cmdE.CommandText = "DSIGE_PROY_M_GetEstado";
                        var drE = cmdE.ExecuteReader();
                        if (drE.HasRows)
                        {
                            List<Estado> e = new List<Estado>();
                            while (drE.Read())
                            {
                                e.Add(new Estado()
                                {
                                    estadoId = drE.GetInt32(0),
                                    abreviatura = drE.GetString(1)
                                });
                            }
                            s.estados = e;
                        }
                        drE.Close();

                        SqlCommand cmdD = con.CreateCommand();
                        cmdD.CommandTimeout = 0;
                        cmdD.CommandType = CommandType.StoredProcedure;
                        cmdD.CommandText = "DSIGE_PROY_M_GetDistritos";
                        var drD = cmdD.ExecuteReader();
                        if (drD.HasRows)
                        {
                            List<Distrito> p = new List<Distrito>();
                            while (drD.Read())
                            {
                                p.Add(new Distrito()
                                {
                                    distritoId = drD.GetInt32(0),
                                    nombreDistrito = drD.GetString(1),
                                    estado = drD.GetInt32(2)
                                });
                            }
                            s.distritos = p;
                        }
                        drD.Close();

                        SqlCommand cmdT = con.CreateCommand();
                        cmdT.CommandTimeout = 0;
                        cmdT.CommandType = CommandType.StoredProcedure;
                        cmdT.CommandText = "DSIGE_PROY_M_GetTipoMaterial";
                        var drT = cmdT.ExecuteReader();
                        if (drT.HasRows)
                        {
                            List<TipoMaterial> p = new List<TipoMaterial>();
                            while (drT.Read())
                            {
                                p.Add(new TipoMaterial()
                                {
                                    detalleId = drT.GetInt32(0),
                                    grupoId = drT.GetInt32(1),
                                    codigo = drT.GetString(2),
                                    descripcion = drT.GetString(3),
                                    estado = drT.GetInt32(4)
                                });
                            }
                            s.materials = p;
                        }
                        drT.Close();

                        SqlCommand cmdS = con.CreateCommand();
                        cmdS.CommandTimeout = 0;
                        cmdS.CommandType = CommandType.StoredProcedure;
                        cmdS.CommandText = "DSIGE_PROY_WM_Combo_Usuarios_Servicios";
                        cmdS.Parameters.Add("@Usuario", SqlDbType.Int).Value = q.usuarioId;
                        var drS = cmdS.ExecuteReader();
                        if (drS.HasRows)
                        {
                            List<Servicio> p = new List<Servicio>();
                            while (drS.Read())
                            {
                                p.Add(new Servicio()
                                {
                                    usuarioId = drS.GetInt32(0),
                                    servicioId = drS.GetInt32(1),
                                    nombreServicio = drS.GetString(2)
                                });
                            }
                            s.servicios = p;
                        }
                        drS.Close();

                        SqlCommand cmd7 = con.CreateCommand();
                        cmd7.CommandTimeout = 0;
                        cmd7.CommandType = CommandType.StoredProcedure;
                        cmd7.CommandText = "DSIGE_PROY_M_Sed";
                        var dr7 = cmd7.ExecuteReader();
                        if (dr7.HasRows)
                        {
                            List<Sed> p = new List<Sed>();
                            while (dr7.Read())
                            {
                                p.Add(new Sed()
                                {
                                    codigo = dr7.GetString(0),
                                    distrito = dr7.GetString(1),
                                    distritoId = dr7.GetInt32(2)
                                });
                            }
                            s.seds = p;
                        }
                        dr7.Close();


                        SqlCommand cmd8 = con.CreateCommand();
                        cmd8.CommandTimeout = 0;
                        cmd8.CommandType = CommandType.StoredProcedure;
                        cmd8.CommandText = "DSIGE_PROY_M_NROOBRA_BAJA";
                        var dr8 = cmd8.ExecuteReader();
                        if (dr8.HasRows)
                        {
                            List<CodigoOts> p = new List<CodigoOts>();
                            while (dr8.Read())
                            {
                                p.Add(new CodigoOts()
                                {
                                    codigo = dr8.GetString(0),
                                });
                            }
                            s.codigos = p;
                        }
                        dr8.Close();
                    }

                    con.Close();
                }
                return s;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Mensaje SaveRegistro(Ot t)
        {
            try
            {
                int otId;
                int detalleId;
                Mensaje m = null;

                List<MensajeDetalle> de = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    SqlCommand cmdO = con.CreateCommand();
                    cmdO.CommandTimeout = 0;
                    cmdO.CommandType = CommandType.StoredProcedure;
                    cmdO.CommandText = "DSIGE_PROY_M_SAVE_TRABAJOCAB";
                    cmdO.Parameters.Add("@otId", SqlDbType.Int).Value = t.identity;
                    cmdO.Parameters.Add("@id_tipoordentrabajo", SqlDbType.Int).Value = t.tipoOrdenId;
                    cmdO.Parameters.Add("@id_servicios", SqlDbType.Int).Value = t.servicioId;
                    cmdO.Parameters.Add("@nroobratd", SqlDbType.VarChar).Value = t.nroObra;
                    cmdO.Parameters.Add("@direccion_ot", SqlDbType.VarChar).Value = t.direccion;
                    cmdO.Parameters.Add("@id_distrito", SqlDbType.Int).Value = t.distritoId;
                    cmdO.Parameters.Add("@referencia_ot", SqlDbType.VarChar).Value = t.referenciaOt;
                    cmdO.Parameters.Add("@descripcion_ot", SqlDbType.VarChar).Value = t.descripcionOt;
                    cmdO.Parameters.Add("@fecharegistro_ot", SqlDbType.VarChar).Value = t.fechaRegistro;
                    cmdO.Parameters.Add("@fechaasignacion_ot", SqlDbType.VarChar).Value = t.fechaAsignacion;
                    cmdO.Parameters.Add("@id_empresa", SqlDbType.Int).Value = t.empresaId;
                    cmdO.Parameters.Add("@id_personaljefecuadrilla", SqlDbType.Int).Value = t.personalJCId;
                    cmdO.Parameters.Add("@id_ot_origen", SqlDbType.Int).Value = t.otOrigenId;
                    cmdO.Parameters.Add("@obsreasignacion_ot", SqlDbType.VarChar).Value = t.observacion;
                    cmdO.Parameters.Add("@id_motivoprioridad", SqlDbType.Int).Value = t.motivoPrioridadId;
                    cmdO.Parameters.Add("@obsmotivoprioridad_ot", SqlDbType.VarChar).Value = t.nombrePrioridad;
                    cmdO.Parameters.Add("@observaciones_ot", SqlDbType.VarChar).Value = t.observaciones;
                    cmdO.Parameters.Add("@ordenamiento_ot", SqlDbType.Int).Value = t.ordenamientoOt;
                    cmdO.Parameters.Add("@latitud_ot", SqlDbType.VarChar).Value = t.latitud;
                    cmdO.Parameters.Add("@longitud_ot", SqlDbType.VarChar).Value = t.longitud;
                    cmdO.Parameters.Add("@estado", SqlDbType.Int).Value = t.estadoId;
                    cmdO.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;

                    SqlDataReader drO = cmdO.ExecuteReader();

                    m = new Mensaje();
                    if (drO.HasRows)
                    {
                        while (drO.Read())
                        {
                            otId = drO.GetInt32(0);

                            m.mensaje = "Enviado";
                            m.codigoRetorno = drO.GetInt32(0);
                            m.codigoBase = t.otId;

                            de = new List<MensajeDetalle>();
                            foreach (var d in t.detalles)
                            {
                                SqlCommand cmdD = con.CreateCommand();
                                cmdD.CommandTimeout = 0;
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "DSIGE_PROY_M_SAVETRABAJODET";
                                cmdD.Parameters.Add("@otDetalleId", SqlDbType.Int).Value = d.otDetalleId;
                                cmdD.Parameters.Add("@id_ot", SqlDbType.Int).Value = otId;
                                cmdD.Parameters.Add("@id_tipotrabajo", SqlDbType.Int).Value = d.tipoTrabajoId;
                                cmdD.Parameters.Add("@id_tipomaterial", SqlDbType.Int).Value = d.tipoMaterialId;
                                cmdD.Parameters.Add("@id_tipodesmonte", SqlDbType.VarChar).Value = d.tipoDesmonteId;
                                cmdD.Parameters.Add("@largo_otdet", SqlDbType.Decimal).Value = d.largo;
                                cmdD.Parameters.Add("@ancho_otdet", SqlDbType.Decimal).Value = d.ancho;
                                cmdD.Parameters.Add("@espesor_otdet", SqlDbType.Decimal).Value = d.espesor;
                                cmdD.Parameters.Add("@total_otdet", SqlDbType.Decimal).Value = d.total;
                                cmdD.Parameters.Add("@nroplacavehiculo", SqlDbType.VarChar).Value = d.nroPlaca;
                                cmdD.Parameters.Add("@m3vehiculo", SqlDbType.Decimal).Value = d.m3Vehiculo;
                                cmdD.Parameters.Add("@estado", SqlDbType.Int).Value = d.estado;
                                cmdD.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;
                                SqlDataReader drD = cmdD.ExecuteReader();

                                if (drD.HasRows)
                                {
                                    while (drD.Read())
                                    {
                                        detalleId = drD.GetInt32(0);

                                        foreach (var p in d.photos)
                                        {
                                            SqlCommand cmdP = con.CreateCommand();
                                            cmdP.CommandTimeout = 0;
                                            cmdP.CommandType = CommandType.StoredProcedure;
                                            cmdP.CommandText = "DSIGE_PROY_M_SAVETRABAJOPHOTO";
                                            cmdP.Parameters.Add("@id_otdet", SqlDbType.Int).Value = detalleId;
                                            cmdP.Parameters.Add("@nombre_otdet_foto", SqlDbType.VarChar).Value = p.nombrePhoto;
                                            cmdP.Parameters.Add("@url_otdet_foto", SqlDbType.VarChar).Value = p.urlPhoto;
                                            cmdP.Parameters.Add("@estado", SqlDbType.Int).Value = p.estado;
                                            cmdP.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;
                                            cmdP.ExecuteNonQuery();
                                        }

                                        de.Add(new MensajeDetalle()
                                        {
                                            detalleId = d.otDetalleId,
                                            detalleRetornoId = detalleId
                                        });
                                    }
                                }
                            }
                            m.detalle = de;
                        }
                    }
                    else
                    {
                        m.mensaje = "Error";
                        return m;
                    }

                    con.Close();
                    return m;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Mensaje SaveGps(EstadoOperario e)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_EstadoGps";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                    cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = 1,
                            mensaje = "Enviado"
                        };
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Mensaje SaveMovil(EstadoMovil e)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "DSIGE_PROY_M_EstadoMovil";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@gpsActivo", SqlDbType.Int).Value = e.gpsActivo;
                    cmd.Parameters.Add("@estadoBateria", SqlDbType.Int).Value = e.estadoBateria;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@modoAvion", SqlDbType.Int).Value = e.modoAvion;
                    cmd.Parameters.Add("@planDatos", SqlDbType.Int).Value = e.planDatos;

                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = 1,
                            mensaje = "Enviado"
                        };
                    }

                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Proveedor> GetProveedores(Query e)
        {
            try
            {
                List<Proveedor> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_Reporte_ResumenProveedor";
                    cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@TipoRepor", SqlDbType.VarChar).Value = e.imei;
                    cmd.Parameters.Add("@Servicio", SqlDbType.Int).Value = e.servicioId;
                    cmd.Parameters.Add("@TipoOrden", SqlDbType.Int).Value = e.tipo;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<Proveedor>();
                        while (dr.Read())
                        {
                            p.Add(new Proveedor()
                            {
                                id = dr.GetInt32(0),
                                empresaId = dr.GetInt32(1),
                                razonSocial = dr.GetString(2),
                                totalMes = dr.GetDecimal(3),
                                asignado = dr.GetDecimal(4),
                                terminado = dr.GetDecimal(5),
                                totaldia = dr.GetDecimal(6)
                            });
                        }
                    }

                    cn.Close();
                }
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<JefeCuadrilla> GetJefeCuadrillas(Query e)
        {
            try
            {
                List<JefeCuadrilla> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_Reporte_ResumenJefeCuadrilla";
                    cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@Servicio", SqlDbType.Int).Value = e.servicioId;
                    cmd.Parameters.Add("@TipoOrden", SqlDbType.Int).Value = e.tipo;
                    cmd.Parameters.Add("@id_proveedor", SqlDbType.Int).Value = e.empresaId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<JefeCuadrilla>();
                        while (dr.Read())
                        {
                            p.Add(new JefeCuadrilla()
                            {
                                cuadrillaId = dr.GetInt32(0),
                                empresaId = dr.GetInt32(1),
                                empresa = dr.GetString(2),
                                jefeCuadrillaId = dr.GetInt32(3),
                                nombreJefe = dr.GetString(4),
                                asignado = dr.GetDecimal(5),
                                terminado = dr.GetDecimal(6),
                                pendiente = dr.GetDecimal(7),
                                latitud = dr.GetString(8),
                                longitud = dr.GetString(9),
                                icono = dr.GetString(10)
                            });
                        }
                    }

                    cn.Close();
                }
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<OtReporte> GetOtReporte(Query e)
        {
            try
            {
                List<OtReporte> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_Lista_OT_Empresa";
                    cmd.Parameters.Add("@id_Empresa", SqlDbType.Int).Value = e.empresaId;
                    cmd.Parameters.Add("@id_Personal", SqlDbType.Int).Value = e.personalId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<OtReporte>();
                        while (dr.Read())
                        {
                            p.Add(new OtReporte()
                            {
                                otId = dr.GetInt32(0),
                                nombreTipoOrdenTrabajo = dr.GetString(1),
                                nombreArea = dr.GetString(2),
                                nroObra = dr.GetString(3),
                                direccion = dr.GetString(4),
                                nombreDistrito = dr.GetString(5),
                                fechaAsignacion = dr.GetDateTime(6).ToString("dd/MM/yyyy"),
                                nombreEmpresa = dr.GetString(7),
                                personalJefeCuadrillaId = dr.GetInt32(8),
                                nombreJC = dr.GetString(9),
                                estado = dr.GetString(10),
                                nombreEstado = dr.GetString(11),
                                vencimiento = dr.GetString(12),
                                latitud = dr.GetString(13),
                                longitud = dr.GetString(14)
                            });
                        }
                    }

                    cn.Close();
                }
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<OtPlazo> GetOtPlazos(Query e)
        {
            try
            {
                List<OtPlazo> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_Reporte_FueraPlazo_Resumen";
                    cmd.Parameters.Add("@idServicio", SqlDbType.VarChar).Value = e.servicioId;
                    cmd.Parameters.Add("@idTipoOT", SqlDbType.Int).Value = e.tipo;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = e.usuarioId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<OtPlazo>();
                        while (dr.Read())
                        {
                            p.Add(new OtPlazo()
                            {
                                empresaId = dr.GetInt32(0),
                                razonSocial = dr.GetString(1),
                                cantidad = dr.GetInt32(2)
                            });
                        }
                    }

                    cn.Close();
                }
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<OtPlazoDetalle> GetOtPlazoDetalles(Query e)
        {
            try
            {
                List<OtPlazoDetalle> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_Reporte_FueraPlazo_Empresa";
                    cmd.Parameters.Add("@idServicio", SqlDbType.VarChar).Value = e.servicioId;
                    cmd.Parameters.Add("@idTipoOT", SqlDbType.Int).Value = e.tipo;
                    cmd.Parameters.Add("@idProveedor", SqlDbType.Int).Value = e.empresaId;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = e.usuarioId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<OtPlazoDetalle>();
                        while (dr.Read())
                        {
                            p.Add(new OtPlazoDetalle()
                            {
                                otId = dr.GetInt32(0),
                                descripcionEstado = dr.GetString(1),
                                tipoOt = dr.GetString(2),
                                nroObra = dr.GetString(3),
                                direccion = dr.GetString(4),
                                distrito = dr.GetString(5),
                                latitud = dr.GetString(6),
                                longitud = dr.GetString(7),
                                fechaAsignacion = dr.GetDateTime(8).ToString("dd/MM/yyyy"),
                                fechaMovil = dr.GetDateTime(9).ToString("dd/MM/yyyy"),
                                empresaContratista = dr.GetString(10),
                                jefeCuadrilla = dr.GetString(11),
                                fueraPlazoHoras = dr.GetString(12),
                                fueraPlazoDias = dr.GetString(13),
                                tipoTrabajoId = dr.GetInt32(14),
                                distritoId = dr.GetInt32(15),
                                referencia = dr.GetString(16),
                                descripcionOt = dr.GetString(17),
                                estadoId = dr.GetInt32(18)
                            });
                        }
                    }

                    cn.Close();
                }
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        public static Mensaje SaveRegistroNew4(Ot t)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    SqlCommand cmdO = con.CreateCommand();
                    cmdO.CommandTimeout = 0;
                    cmdO.CommandType = CommandType.StoredProcedure;
                    cmdO.CommandText = "DSIGE_PROY_M_SAVE_TRABAJOCAB_NEW_3";
                    cmdO.Parameters.Add("@otId", SqlDbType.Int).Value = t.identity;
                    cmdO.Parameters.Add("@id_tipoordentrabajo", SqlDbType.Int).Value = t.tipoOrdenId;
                    cmdO.Parameters.Add("@id_servicios", SqlDbType.Int).Value = t.servicioId;
                    cmdO.Parameters.Add("@nroobratd", SqlDbType.VarChar).Value = t.nroObra;
                    cmdO.Parameters.Add("@direccion_ot", SqlDbType.VarChar).Value = t.direccion;
                    cmdO.Parameters.Add("@id_distrito", SqlDbType.Int).Value = t.distritoId;
                    cmdO.Parameters.Add("@referencia_ot", SqlDbType.VarChar).Value = t.referenciaOt;
                    cmdO.Parameters.Add("@descripcion_ot", SqlDbType.VarChar).Value = t.descripcionOt;
                    cmdO.Parameters.Add("@fecharegistro_ot", SqlDbType.VarChar).Value = t.fechaRegistro;
                    cmdO.Parameters.Add("@fechaasignacion_ot", SqlDbType.VarChar).Value = t.fechaAsignacion;
                    cmdO.Parameters.Add("@id_empresa", SqlDbType.Int).Value = t.empresaId;
                    cmdO.Parameters.Add("@id_personaljefecuadrilla", SqlDbType.Int).Value = t.personalJCId;
                    cmdO.Parameters.Add("@id_ot_origen", SqlDbType.Int).Value = t.otOrigenId;
                    cmdO.Parameters.Add("@obsreasignacion_ot", SqlDbType.VarChar).Value = t.observacion;
                    cmdO.Parameters.Add("@id_motivoprioridad", SqlDbType.Int).Value = t.motivoPrioridadId;
                    cmdO.Parameters.Add("@obsmotivoprioridad_ot", SqlDbType.VarChar).Value = t.nombrePrioridad;
                    cmdO.Parameters.Add("@observaciones_ot", SqlDbType.VarChar).Value = t.observaciones;
                    cmdO.Parameters.Add("@ordenamiento_ot", SqlDbType.Int).Value = t.ordenamientoOt;
                    cmdO.Parameters.Add("@latitud_ot", SqlDbType.VarChar).Value = t.latitud;
                    cmdO.Parameters.Add("@longitud_ot", SqlDbType.VarChar).Value = t.longitud;
                    cmdO.Parameters.Add("@estado", SqlDbType.Int).Value = t.estadoId;
                    cmdO.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;

                    cmdO.Parameters.Add("@distritoIdGps", SqlDbType.Int).Value = t.distritoIdGps;
                    cmdO.Parameters.Add("@suministroTD", SqlDbType.VarChar).Value = t.suministroTD;
                    cmdO.Parameters.Add("@nroSed", SqlDbType.VarChar).Value = t.nroSed;
                    cmdO.Parameters.Add("@viajeIndebido", SqlDbType.Int).Value = t.viajeIndebido;

                    SqlDataReader drO = cmdO.ExecuteReader();

                    m = new Mensaje();
                    if (drO.HasRows)
                    {
                        while (drO.Read())
                        {
                            m.mensaje = "Enviado";
                            m.codigoRetorno = drO.GetInt32(0);
                            m.codigoBase = t.otId;

                            List<MensajeDetalle> de = new List<MensajeDetalle>();

                            foreach (var d in t.detalles)
                            {
                                SqlCommand cmdD = con.CreateCommand();
                                cmdD.CommandTimeout = 0;
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "DSIGE_PROY_M_SAVETRABAJODET_NEW_2";
                                cmdD.Parameters.Add("@otDetalleId", SqlDbType.Int).Value = d.otDetalleId;
                                cmdD.Parameters.Add("@id_ot", SqlDbType.Int).Value = drO.GetInt32(0);
                                cmdD.Parameters.Add("@id_tipotrabajo", SqlDbType.Int).Value = d.tipoTrabajoId;
                                cmdD.Parameters.Add("@id_tipomaterial", SqlDbType.Int).Value = d.tipoMaterialId;
                                cmdD.Parameters.Add("@id_tipodesmonte", SqlDbType.VarChar).Value = d.tipoDesmonteId;
                                cmdD.Parameters.Add("@largo_otdet", SqlDbType.Decimal).Value = d.largo;
                                cmdD.Parameters.Add("@ancho_otdet", SqlDbType.Decimal).Value = d.ancho;
                                cmdD.Parameters.Add("@espesor_otdet", SqlDbType.Decimal).Value = d.espesor;
                                cmdD.Parameters.Add("@total_otdet", SqlDbType.Decimal).Value = d.total;
                                cmdD.Parameters.Add("@nroplacavehiculo", SqlDbType.VarChar).Value = d.nroPlaca;
                                cmdD.Parameters.Add("@m3vehiculo", SqlDbType.Decimal).Value = d.m3Vehiculo;
                                cmdD.Parameters.Add("@estado", SqlDbType.Int).Value = d.estado;
                                cmdD.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;
                                cmdD.Parameters.Add("@latitud", SqlDbType.VarChar).Value = t.latitud;
                                cmdD.Parameters.Add("@longitud", SqlDbType.VarChar).Value = t.longitud;
                                cmdD.Parameters.Add("@Cant_Panos", SqlDbType.Float).Value = d.cantPanos;
                                cmdD.Parameters.Add("@Med_Horizontal", SqlDbType.Decimal).Value = d.medHorizontal;
                                cmdD.Parameters.Add("@Med_Vertical", SqlDbType.Decimal).Value = d.medVertical;

                                SqlDataReader drD = cmdD.ExecuteReader();

                                if (drD.HasRows)
                                {
                                    while (drD.Read())
                                    {
                                        foreach (var p in d.photos)
                                        {
                                            SqlCommand cmdP = con.CreateCommand();
                                            cmdP.CommandTimeout = 0;
                                            cmdP.CommandType = CommandType.StoredProcedure;
                                            cmdP.CommandText = "DSIGE_PROY_M_SAVETRABAJOPHOTO";
                                            cmdP.Parameters.Add("@id_otdet", SqlDbType.Int).Value = drD.GetInt32(0);
                                            cmdP.Parameters.Add("@nombre_otdet_foto", SqlDbType.VarChar).Value = p.nombrePhoto;
                                            cmdP.Parameters.Add("@url_otdet_foto", SqlDbType.VarChar).Value = p.urlPhoto;
                                            cmdP.Parameters.Add("@estado", SqlDbType.Int).Value = p.estado;
                                            cmdP.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;
                                            cmdP.ExecuteNonQuery();
                                        }

                                        de.Add(new MensajeDetalle()
                                        {
                                            detalleId = d.otDetalleId,
                                            detalleRetornoId = drD.GetInt32(0)
                                        });
                                    }
                                }
                            }
                            m.detalle = de;
                        }
                    }
                    else
                    {
                        m.mensaje = "Error";
                        return m;
                    }

                    con.Close();
                    return m;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveRegistroNew5(Ot t)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    SqlCommand cmdO = con.CreateCommand();
                    cmdO.CommandTimeout = 0;
                    cmdO.CommandType = CommandType.StoredProcedure;
                    cmdO.CommandText = "DSIGE_PROY_M_SAVE_TRABAJOCAB_NEW_4";
                    cmdO.Parameters.Add("@otId", SqlDbType.Int).Value = t.identity;
                    cmdO.Parameters.Add("@id_tipoordentrabajo", SqlDbType.Int).Value = t.tipoOrdenId;
                    cmdO.Parameters.Add("@id_servicios", SqlDbType.Int).Value = t.servicioId;
                    cmdO.Parameters.Add("@nroobratd", SqlDbType.VarChar).Value = t.nroObra;
                    cmdO.Parameters.Add("@direccion_ot", SqlDbType.VarChar).Value = t.direccion;
                    cmdO.Parameters.Add("@id_distrito", SqlDbType.Int).Value = t.distritoId;
                    cmdO.Parameters.Add("@referencia_ot", SqlDbType.VarChar).Value = t.referenciaOt;
                    cmdO.Parameters.Add("@descripcion_ot", SqlDbType.VarChar).Value = t.descripcionOt;
                    cmdO.Parameters.Add("@fecharegistro_ot", SqlDbType.VarChar).Value = t.fechaRegistro;
                    cmdO.Parameters.Add("@fechaasignacion_ot", SqlDbType.VarChar).Value = t.fechaAsignacion;
                    cmdO.Parameters.Add("@id_empresa", SqlDbType.Int).Value = t.empresaId;
                    cmdO.Parameters.Add("@id_personaljefecuadrilla", SqlDbType.Int).Value = t.personalJCId;
                    cmdO.Parameters.Add("@id_ot_origen", SqlDbType.Int).Value = t.otOrigenId;
                    cmdO.Parameters.Add("@obsreasignacion_ot", SqlDbType.VarChar).Value = t.observacion;
                    cmdO.Parameters.Add("@id_motivoprioridad", SqlDbType.Int).Value = t.motivoPrioridadId;
                    cmdO.Parameters.Add("@obsmotivoprioridad_ot", SqlDbType.VarChar).Value = t.nombrePrioridad;
                    cmdO.Parameters.Add("@observaciones_ot", SqlDbType.VarChar).Value = t.observaciones;
                    cmdO.Parameters.Add("@ordenamiento_ot", SqlDbType.Int).Value = t.ordenamientoOt;
                    cmdO.Parameters.Add("@latitud_ot", SqlDbType.VarChar).Value = t.latitud;
                    cmdO.Parameters.Add("@longitud_ot", SqlDbType.VarChar).Value = t.longitud;
                    cmdO.Parameters.Add("@estado", SqlDbType.Int).Value = t.estadoId;
                    cmdO.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;

                    cmdO.Parameters.Add("@distritoIdGps", SqlDbType.Int).Value = t.distritoIdGps;
                    cmdO.Parameters.Add("@suministroTD", SqlDbType.VarChar).Value = t.suministroTD;
                    cmdO.Parameters.Add("@nroSed", SqlDbType.VarChar).Value = t.nroSed;
                    cmdO.Parameters.Add("@viajeIndebido", SqlDbType.Int).Value = t.viajeIndebido;
                    cmdO.Parameters.Add("@fechaInicio", SqlDbType.VarChar).Value = t.fechaInicioTrabajo;
                    cmdO.Parameters.Add("@fechaFin", SqlDbType.VarChar).Value = t.fechaFinTrabajo;

                    SqlDataReader drO = cmdO.ExecuteReader();

                    m = new Mensaje();
                    if (drO.HasRows)
                    {
                        while (drO.Read())
                        {
                            m.mensaje = "Enviado";
                            m.codigoRetorno = drO.GetInt32(0);
                            m.codigoBase = t.otId;

                            List<MensajeDetalle> de = new List<MensajeDetalle>();

                            foreach (var d in t.detalles)
                            {
                                SqlCommand cmdD = con.CreateCommand();
                                cmdD.CommandTimeout = 0;
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "DSIGE_PROY_M_SAVETRABAJODET_NEW_2";
                                cmdD.Parameters.Add("@otDetalleId", SqlDbType.Int).Value = d.otDetalleId;
                                cmdD.Parameters.Add("@id_ot", SqlDbType.Int).Value = drO.GetInt32(0);
                                cmdD.Parameters.Add("@id_tipotrabajo", SqlDbType.Int).Value = d.tipoTrabajoId;
                                cmdD.Parameters.Add("@id_tipomaterial", SqlDbType.Int).Value = d.tipoMaterialId;
                                cmdD.Parameters.Add("@id_tipodesmonte", SqlDbType.VarChar).Value = d.tipoDesmonteId;
                                cmdD.Parameters.Add("@largo_otdet", SqlDbType.Decimal).Value = d.largo;
                                cmdD.Parameters.Add("@ancho_otdet", SqlDbType.Decimal).Value = d.ancho;
                                cmdD.Parameters.Add("@espesor_otdet", SqlDbType.Decimal).Value = d.espesor;
                                cmdD.Parameters.Add("@total_otdet", SqlDbType.Decimal).Value = d.total;
                                cmdD.Parameters.Add("@nroplacavehiculo", SqlDbType.VarChar).Value = d.nroPlaca;
                                cmdD.Parameters.Add("@m3vehiculo", SqlDbType.Decimal).Value = d.m3Vehiculo;
                                cmdD.Parameters.Add("@estado", SqlDbType.Int).Value = d.estado;
                                cmdD.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;
                                cmdD.Parameters.Add("@latitud", SqlDbType.VarChar).Value = t.latitud;
                                cmdD.Parameters.Add("@longitud", SqlDbType.VarChar).Value = t.longitud;
                                cmdD.Parameters.Add("@Cant_Panos", SqlDbType.Float).Value = d.cantPanos;
                                cmdD.Parameters.Add("@Med_Horizontal", SqlDbType.Decimal).Value = d.medHorizontal;
                                cmdD.Parameters.Add("@Med_Vertical", SqlDbType.Decimal).Value = d.medVertical;

                                SqlDataReader drD = cmdD.ExecuteReader();

                                if (drD.HasRows)
                                {
                                    while (drD.Read())
                                    {
                                        foreach (var p in d.photos)
                                        {
                                            SqlCommand cmdP = con.CreateCommand();
                                            cmdP.CommandTimeout = 0;
                                            cmdP.CommandType = CommandType.StoredProcedure;
                                            cmdP.CommandText = "DSIGE_PROY_M_SAVETRABAJOPHOTO";
                                            cmdP.Parameters.Add("@id_otdet", SqlDbType.Int).Value = drD.GetInt32(0);
                                            cmdP.Parameters.Add("@nombre_otdet_foto", SqlDbType.VarChar).Value = p.nombrePhoto;
                                            cmdP.Parameters.Add("@url_otdet_foto", SqlDbType.VarChar).Value = p.urlPhoto;
                                            cmdP.Parameters.Add("@estado", SqlDbType.Int).Value = p.estado;
                                            cmdP.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;
                                            cmdP.ExecuteNonQuery();
                                        }

                                        de.Add(new MensajeDetalle()
                                        {
                                            detalleId = d.otDetalleId,
                                            detalleRetornoId = drD.GetInt32(0)
                                        });
                                    }
                                }
                            }
                            m.detalle = de;
                        }
                    }
                    else
                    {
                        m.mensaje = "Error";
                        return m;
                    }

                    con.Close();
                    return m;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveRegistroNew6(Ot t)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    SqlCommand cmdO = con.CreateCommand();
                    cmdO.CommandTimeout = 0;
                    cmdO.CommandType = CommandType.StoredProcedure;
                    cmdO.CommandText = "DSIGE_PROY_M_SAVE_TRABAJOCAB_NEW_5";
                    cmdO.Parameters.Add("@otId", SqlDbType.Int).Value = t.identity;
                    cmdO.Parameters.Add("@id_tipoordentrabajo", SqlDbType.Int).Value = t.tipoOrdenId;
                    cmdO.Parameters.Add("@id_servicios", SqlDbType.Int).Value = t.servicioId;
                    cmdO.Parameters.Add("@nroobratd", SqlDbType.VarChar).Value = t.nroObra;
                    cmdO.Parameters.Add("@direccion_ot", SqlDbType.VarChar).Value = t.direccion;
                    cmdO.Parameters.Add("@id_distrito", SqlDbType.Int).Value = t.distritoId;
                    cmdO.Parameters.Add("@referencia_ot", SqlDbType.VarChar).Value = t.referenciaOt;
                    cmdO.Parameters.Add("@descripcion_ot", SqlDbType.VarChar).Value = t.descripcionOt;
                    cmdO.Parameters.Add("@fecharegistro_ot", SqlDbType.VarChar).Value = t.fechaRegistro;
                    cmdO.Parameters.Add("@fechaasignacion_ot", SqlDbType.VarChar).Value = t.fechaAsignacion;
                    cmdO.Parameters.Add("@id_empresa", SqlDbType.Int).Value = t.empresaId;
                    cmdO.Parameters.Add("@id_personaljefecuadrilla", SqlDbType.Int).Value = t.personalJCId;
                    cmdO.Parameters.Add("@id_ot_origen", SqlDbType.Int).Value = t.otOrigenId;
                    cmdO.Parameters.Add("@obsreasignacion_ot", SqlDbType.VarChar).Value = t.observacion;
                    cmdO.Parameters.Add("@id_motivoprioridad", SqlDbType.Int).Value = t.motivoPrioridadId;
                    cmdO.Parameters.Add("@obsmotivoprioridad_ot", SqlDbType.VarChar).Value = t.nombrePrioridad;
                    cmdO.Parameters.Add("@observaciones_ot", SqlDbType.VarChar).Value = t.observaciones;
                    cmdO.Parameters.Add("@ordenamiento_ot", SqlDbType.Int).Value = t.ordenamientoOt;
                    cmdO.Parameters.Add("@latitud_ot", SqlDbType.VarChar).Value = t.latitud;
                    cmdO.Parameters.Add("@longitud_ot", SqlDbType.VarChar).Value = t.longitud;
                    cmdO.Parameters.Add("@estado", SqlDbType.Int).Value = t.estadoId;
                    cmdO.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;

                    cmdO.Parameters.Add("@distritoIdGps", SqlDbType.Int).Value = t.distritoIdGps;
                    cmdO.Parameters.Add("@suministroTD", SqlDbType.VarChar).Value = t.suministroTD;
                    cmdO.Parameters.Add("@nroSed", SqlDbType.VarChar).Value = t.nroSed;
                    cmdO.Parameters.Add("@viajeIndebido", SqlDbType.Int).Value = t.viajeIndebido;
                    cmdO.Parameters.Add("@fechaInicio", SqlDbType.VarChar).Value = t.fechaInicioTrabajo;
                    cmdO.Parameters.Add("@fechaFin", SqlDbType.VarChar).Value = t.fechaFinTrabajo;
                    cmdO.Parameters.Add("@urlPdf", SqlDbType.VarChar).Value = t.urlPdf;

                    SqlDataReader drO = cmdO.ExecuteReader();

                    m = new Mensaje();
                    if (drO.HasRows)
                    {
                        while (drO.Read())
                        {
                            m.mensaje = "Enviado";
                            m.codigoRetorno = drO.GetInt32(0);
                            m.codigoBase = t.otId;

                            List<MensajeDetalle> de = new List<MensajeDetalle>();

                            foreach (var d in t.detalles)
                            {
                                SqlCommand cmdD = con.CreateCommand();
                                cmdD.CommandTimeout = 0;
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "DSIGE_PROY_M_SAVETRABAJODET_NEW_2";
                                cmdD.Parameters.Add("@otDetalleId", SqlDbType.Int).Value = d.otDetalleId;
                                cmdD.Parameters.Add("@id_ot", SqlDbType.Int).Value = drO.GetInt32(0);
                                cmdD.Parameters.Add("@id_tipotrabajo", SqlDbType.Int).Value = d.tipoTrabajoId;
                                cmdD.Parameters.Add("@id_tipomaterial", SqlDbType.Int).Value = d.tipoMaterialId;
                                cmdD.Parameters.Add("@id_tipodesmonte", SqlDbType.VarChar).Value = d.tipoDesmonteId;
                                cmdD.Parameters.Add("@largo_otdet", SqlDbType.Decimal).Value = d.largo;
                                cmdD.Parameters.Add("@ancho_otdet", SqlDbType.Decimal).Value = d.ancho;
                                cmdD.Parameters.Add("@espesor_otdet", SqlDbType.Decimal).Value = d.espesor;
                                cmdD.Parameters.Add("@total_otdet", SqlDbType.Decimal).Value = d.total;
                                cmdD.Parameters.Add("@nroplacavehiculo", SqlDbType.VarChar).Value = d.nroPlaca;
                                cmdD.Parameters.Add("@m3vehiculo", SqlDbType.Decimal).Value = d.m3Vehiculo;
                                cmdD.Parameters.Add("@estado", SqlDbType.Int).Value = d.estado;
                                cmdD.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;
                                cmdD.Parameters.Add("@latitud", SqlDbType.VarChar).Value = t.latitud;
                                cmdD.Parameters.Add("@longitud", SqlDbType.VarChar).Value = t.longitud;
                                cmdD.Parameters.Add("@Cant_Panos", SqlDbType.Float).Value = d.cantPanos;
                                cmdD.Parameters.Add("@Med_Horizontal", SqlDbType.Decimal).Value = d.medHorizontal;
                                cmdD.Parameters.Add("@Med_Vertical", SqlDbType.Decimal).Value = d.medVertical;

                                SqlDataReader drD = cmdD.ExecuteReader();

                                if (drD.HasRows)
                                {
                                    while (drD.Read())
                                    {
                                        foreach (var p in d.photos)
                                        {
                                            SqlCommand cmdP = con.CreateCommand();
                                            cmdP.CommandTimeout = 0;
                                            cmdP.CommandType = CommandType.StoredProcedure;
                                            cmdP.CommandText = "DSIGE_PROY_M_SAVETRABAJOPHOTO";
                                            cmdP.Parameters.Add("@id_otdet", SqlDbType.Int).Value = drD.GetInt32(0);
                                            cmdP.Parameters.Add("@nombre_otdet_foto", SqlDbType.VarChar).Value =p.nombrePhoto;
                                            cmdP.Parameters.Add("@url_otdet_foto", SqlDbType.VarChar).Value = p.urlPhoto;
                                            cmdP.Parameters.Add("@estado", SqlDbType.Int).Value = p.estado;
                                            cmdP.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = t.usuarioId;
                                            cmdP.ExecuteNonQuery();
                                        }

                                        de.Add(new MensajeDetalle()
                                        {
                                            detalleId = d.otDetalleId,
                                            detalleRetornoId = drD.GetInt32(0)
                                        });
                                    }
                                }
                            }
                            m.detalle = de;
                        }
                    }
                    else
                    {
                        m.mensaje = "Error";
                        return m;
                    }

                    con.Close();
                    return m;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Mensaje SaveEstadoMovil(EstadoMovil e)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "DSIGE_PROY_M_EstadoMovil";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@gpsActivo", SqlDbType.Bit).Value = e.gpsActivo;
                    cmd.Parameters.Add("@estadoBateria", SqlDbType.Int).Value = e.estadoBateria;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@modoAvion", SqlDbType.Int).Value = e.modoAvion;
                    cmd.Parameters.Add("@planDatos", SqlDbType.Bit).Value = e.planDatos;

                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = (e.id == 0) ? 0 : e.id,
                            mensaje = "Enviado"
                        };
                    }

                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveOperarioGps(EstadoOperario e)
        {
            try
            {
                Mensaje m = new Mensaje();
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_EstadoGps";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                    cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;

                    int a = cmd.ExecuteNonQuery();

                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = (e.id == 0) ? 0 : e.id,
                            mensaje = "Enviado"
                        };
                    }

                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}