using kairosApp.Domain.Services;
using kairosApp.Resources.Support;
using RegistroCivilService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace kairosApp.Services
{
    public class RGService : IRGService
    {
        public async Task<bool> ValidateIdentificationNumber(string identificacion)
        {
            var respuesta = await this.GetCiudadano(identificacion);
            if(respuesta != null)
            {
                Debug.WriteLine("Aqui va el error: " + respuesta.CodigoError);
                if (respuesta.CodigoError == "000")
                {
                    Debug.WriteLine("Aqui va el nombre: " + respuesta.Nombre);
                    Debug.WriteLine("Aqui va la fecha: " + respuesta.FechaCedulacion);
                    return true;
                }
                
            }
            Debug.WriteLine("No encontro a la persona");
            return false; 
        }

        public async Task<RGDatos> VerifyIdentification(string identificacion, string fecha, string tipo)
        {
            var respuesta = await this.GetCiudadano(identificacion);
            if (tipo == "cedula")
            {
                if (respuesta != null && respuesta.CodigoError == "000")
                {
                    Debug.WriteLine("Cedula: " + respuesta.TipoCedula);
                    Debug.WriteLine("Fecha de Emision: " + respuesta.FechaCedulacion);
                    Debug.WriteLine("Fecha de Emision despues del parseo: " + ParseDate(respuesta.FechaCedulacion));
                    Debug.WriteLine("Fecha pasada: " + fecha);
                    
                    if (ParseDate(respuesta.FechaCedulacion) == fecha)
                    {
                        return new RGDatos { Identificacion = identificacion, Apellidos = respuesta.Nombre, Nombres = respuesta.Nombre};
                    }
                    return null;
                }
                
            }else if(tipo == "pasaporte")
            {
                if (respuesta != null && respuesta.CodigoError == "000")
                {
                    Debug.WriteLine("Cedula: " + respuesta.TipoCedula);
                    Debug.WriteLine("Fecha de Nacimiento: " + respuesta.FechaNacimiento);
                    if (ParseDate(respuesta.FechaNacimiento) == fecha)
                    {
                        return new RGDatos { Identificacion = identificacion, Apellidos = respuesta.Apellidos, Nombres = respuesta.Nombres };
                    }
                    return null;
                }
            }
            return null; 
        }
        private string ParseDate(string fecha)
        {
            var fechaParseada = DateTime.Parse(fecha);
            Debug.WriteLine("Fecha antes de pasar el parseo: " + fechaParseada.ToString());
            return fechaParseada.ToString("yyyy-MM-dd");
        }
        private async Task<ciudadano> GetCiudadano(string identificacion)
        {
            ciudadano respuesta;
            MethodInfo method = typeof(XmlSerializer).GetMethod("set_Mode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { 1 });
            Debug.WriteLine("Antes de hacer la peticion");
            wsBSGProxySoapClient clientws = new wsBSGProxySoapClient(0);
            AuthSoapHeaders header = new AuthSoapHeaders();
            header.usr = "meldaban";
            header.pwd = "melaBa029702";

            try
            {
                Debug.WriteLine("Entra al try de la peticion");
                GetCiudadanoResponse data = await clientws.GetCiudadanoAsync(header, identificacion);
                respuesta = data.GetCiudadanoResult;
                Debug.WriteLine("Despues de la peticion");
            }
            catch(Exception e)
            {
                Debug.WriteLine("En el catch: "+e.Message);
                respuesta = null; 
            }

            return respuesta;
        }
    }
}
