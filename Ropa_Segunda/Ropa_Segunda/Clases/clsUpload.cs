using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Ropa_Segunda.Clases
{
	public class clsUpload
	{
		public HttpRequestMessage request { get; set; }
        public string Datos { get; set; }
        public string Proceso { get; set; }
        public List<string> Archivos { get; set; }
        public async Task<HttpResponseMessage> CargarArchivos(bool Actualizar)
        {
            if (!request.Content.IsMimeMultipartContent())
            {
                return request.CreateResponse(System.Net.HttpStatusCode.UnsupportedMediaType, "No se envio un archivo para procesar");
            }
            string root = HttpContext.Current.Server.MapPath("~/Archivos");
            var provider = new MultipartFormDataStreamProvider(root);
            bool Existe = false;
            try
            {
                //lEER EL ARCHIVO
                await request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileData.Count > 0)
                {
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        string fileName = file.Headers.ContentDisposition.FileName;
                        //Procesamiento al nombre del archivo
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }
                        if (File.Exists(Path.Combine(root, fileName)))
                        {
                            if (Actualizar)
                            {
                                //El archivo ya existe en el servidor,se elimina el original y se premite el cambio de nombre
                                File.Delete(Path.Combine(root, fileName));
                                File.Move(file.LocalFileName, Path.Combine(root, fileName));
                                Existe = true;
                            }
                            else
                            {
                                //El archivo ya existe en el servidor, no se va a cargar, se va a eliminar el temporal y se evolvera un erro
                                File.Delete(Path.Combine(root, file.LocalFileName));
                                Existe = true;
                            }
                        }
                        else
                        {
                            //Agrego en una lista el nombre de los archivos que se cargaron
                            Archivos.Add(fileName);
                            //Renombra el archivo temporal
                            File.Move(file.LocalFileName, Path.Combine(root, fileName));

                        }
                    }
                    if (!Existe)
                    {
                        //Se genero el proceso de gestion en la base de datos
                        string RptaBD = GrabarInfoBD();
                        //Termina el ciclo, responde que se cargo el archivo correctamente
                        return request.CreateResponse(HttpStatusCode.OK, "Archivos cargados con éxito" + RptaBD);
                    }
                    else
                    {
                        return request.CreateErrorResponse(System.Net.HttpStatusCode.Conflict, "El archivo ya existe en el servidor");
                    }
                }
                else
                {
                    return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "No" +
                        " se envio un archivo para procesar");
                }
            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public HttpResponseMessage LeerArchivo(string archivo)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                string Ruta = HttpContext.Current.Server.MapPath("~/Archivos");
                string Archivo = Path.Combine(Ruta, archivo);
                if (File.Exists(Archivo))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(Archivo, FileMode.Open, FileAccess.Read);
                    response.Content = new StreamContent(stream);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = archivo;
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    return response;
                }
                else
                {
                    return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "El archivo no se encuentra en el servidor");
                }
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "No se pudo procesar el archivo. " + ex.Message);
            }
        }
        private string GrabarInfoBD()
        {
            switch (Proceso.ToUpper())
            {
                case "PRENDAS":
                    clsPrendas prendas = new clsPrendas();
                    return prendas.GrabarImagen(Convert.ToInt32(Datos), Archivos);
                default:
                    return "No se ha definido la opción";
            }
        }
    }
}