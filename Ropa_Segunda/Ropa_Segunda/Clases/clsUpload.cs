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
                return request.CreateResponse(HttpStatusCode.UnsupportedMediaType, "No se envió un archivo para procesar");
            }

            string root = HttpContext.Current.Server.MapPath("~/Archivos");
            var provider = new MultipartFormDataStreamProvider(root);
            bool existe = false;

            try
            {
                await request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileData.Count > 0)
                {
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        string fileName = Path.GetFileName(file.Headers.ContentDisposition.FileName.Trim('"'));
                        string filePath = Path.Combine(root, fileName);

                        if (File.Exists(filePath))
                        {
                            if (Actualizar)
                            {
                                File.Delete(filePath);
                                File.Move(file.LocalFileName, filePath);
                                existe = true;
                            }
                            else
                            {
                                File.Delete(file.LocalFileName);
                                existe = true;
                            }
                        }
                        else
                        {
                            Archivos.Add(fileName);
                            File.Move(file.LocalFileName, filePath);
                        }
                    }

                    if (!existe)
                    {
                        string RptaBD = GrabarInfoBD();
                        return request.CreateResponse(HttpStatusCode.OK, "Archivos cargados con éxito" + RptaBD);
                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.Conflict, "El archivo ya existe en el servidor");
                    }
                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se envió un archivo para procesar");
                }
            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private string GrabarInfoBD()
        {
            if (Proceso.ToUpper() == "PRENDAS")
            {
                clsPrendas prendas = new clsPrendas();
                return prendas.GrabarImagen(Convert.ToInt32(Datos), Archivos);
            }
            return "No se ha definido la opción";
        }
    }
}