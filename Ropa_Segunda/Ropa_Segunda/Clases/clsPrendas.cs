using Ropa_Segunda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                    FotoPrenda foto = new FotoPrenda();
                    foto.idPrenda = idPrenda;

                    dbexamen.FotoPrendas.Add(foto);
                    dbexamen.SaveChanges();
                }
                return "Imagenes guardadas correctamente";
            }
            catch (Exception ex)
            {
                return "Error al guardar las imagenes: " + ex.Message;
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
