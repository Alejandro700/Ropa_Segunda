using Ropa_Segunda.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Ropa_Segunda.Clases
{
    public class clsPrendas
    {
        private DBExamenEntities dbexamen = new DBExamenEntities();
        public Prenda prenda { get; set; }

        public string Insertar()
        {
            try
            {
                var clienteExistente = dbexamen.Clientes.Find(prenda.Cliente);
                if (clienteExistente == null)
                {
                    return "Error: Cliente no encontrado";
                }

                dbexamen.Prendas.Add(prenda);
                dbexamen.SaveChanges();
                return "Prenda registrada correctamente";
            }
            catch (Exception ex)
            {
                return "Error al registrar la prenda: " + ex.Message;
            }
        }

        public string GrabarImagen(int idPrenda, List<string> imagenes)
        {
            try
            {
                foreach (var item in imagenes)
                {
                    FotoPrenda foto = new FotoPrenda
                    {
                        idPrenda = idPrenda,
                        FotoPrenda1 = item
                    };

                    dbexamen.FotoPrendas.Add(foto);
                }
                dbexamen.SaveChanges();
                return "Imágenes guardadas correctamente";
            }
            catch (Exception ex)
            {
                return "Error al guardar las imágenes: " + ex.Message;
            }
        }

        public IQueryable ListarImagenesXPrenda(int idPrenda)
        {
            return from P in dbexamen.Prendas
                   join F in dbexamen.FotoPrendas on P.IdPrenda equals F.idPrenda
                   where P.IdPrenda == idPrenda
                   select new
                   {
                       IdFoto = F.idFoto,
                       Foto = F.FotoPrenda1,
                       IdPrenda = P.IdPrenda,
                       TipoPrenda = P.TipoPrenda,
                       Descripcion = P.Descripcion,
                       Valor = P.Valor
                   };
        }
    }
}
