using Ropa_Segunda.Clases;
using Ropa_Segunda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ropa_Segunda.Controllers
{
    [RoutePrefix("api/Prendas")]
    public class PrendasController : ApiController
    {
        [HttpPost]
        [Route("Insertar")]
        public string Insertar([FromBody] Prenda prenda)
        {
            clsPrendas Prendas = new clsPrendas();
            Prendas.prenda = prenda;
            return Prendas.Insertar();
        }
        [HttpPost]
        [Route("")]
    }
}