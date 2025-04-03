using Ropa_Segunda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ropa_Segunda.Clases
{
	public class clsCliente
	{
		private DBExamenEntities dbexamen = new DBExamenEntities();
		public Cliente cliente { get; set; }
		public string Insertar()
		{
			try
			{
				dbexamen.Clientes.Add(cliente);
                dbexamen.SaveChanges();
				return "Cliente registrado correctamente";
            }
			catch(Exception ex)
            {
                return "Error al registrar el cliente: " + ex.Message;
            }
		}

		public List<Cliente> ConsultarTodos()
		{
			return dbexamen.Clientes
				.OrderBy(p => p.Nombre)
				.ToList();

        }



    }
}