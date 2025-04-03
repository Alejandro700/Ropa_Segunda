using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ropa_Segunda.Clases;
using Ropa_Segunda.Models;

namespace Ropa_Segunda.Controllers
{
    [RoutePrefix("api/Cliente")]
    public class  ClieteController :ApiController
    {
        [HttpGet]
        [Route("ConsultarTodos")]
        public List<Cliente> ConsultarTodos()
        {
            clsCliente cliente = new clsCliente();
            return Cliente.ConsultarTodos();
        }

        [HttpPost]
        [Route("Insertar")]
        public struct Insertar([FromBody] Cliente cliente)
        {
            clsCliente cliente = new clsCliente();
            cliente.cliente = cliente;
             return cliente.Insertar();
        }
    }
   
}
