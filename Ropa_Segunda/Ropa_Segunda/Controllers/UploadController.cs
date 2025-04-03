using Ropa_Segunda.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ropa_Segunda.Controllers
{
    [RoutePrefix("api/UploadFiles")]
    public class UploadFilesController : ApiController
    {
        [HttpPost]
        [Route("CargarArchivo")]
        public async Task<HttpResponseMessage> CargarArchivo(HttpRequestMessage request, string Datos, string Proceso)
        {
            clsUpload upload = new clsUpload();
            upload.request = request;
            upload.Datos = Datos;
            upload.Proceso = Proceso;
            return await upload.CargarArchivos(false);

        }
        [HttpPost]
        [Route("Actualizar")]
        public async Task<HttpResponseMessage> Actualizar(HttpRequestMessage request, string Datos, string Proceso)
        {
            clsUpload upload = new clsUpload();
            upload.Datos = Datos;
            upload.Proceso = Proceso;
            upload.request = request;
            return await upload.CargarArchivos(true);
        }
    }
}