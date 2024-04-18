using AppAccionesWebJob.Data;
using ProyectoNetCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ScrapySharp.Extensions;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace AppAccionesWebJob.Repositories
{
    public class RepositoryAcciones
    {
        private AccionContext context;

        public RepositoryAcciones(AccionContext context)          {
            this.context = context;
        }

        //metodo para recuperar acciones de la bbdd
        public async Task<List<Accion>> PedirAccionesBBDD()
        {
            string fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fechaBusqueda = DateTime.ParseExact(fechaHoy, "dd/MM/yyyy", null);
            var consulta = from datos in this.context.Acciones where datos.Fecha.Date == fechaBusqueda select datos;

            return consulta.ToList();
        }

        //metodo para insertar acciones a la bbdd
        public async Task<string> InsertarAccionDia()
        {

            var acciones = await this.PedirAccionesPag();

            var consulta = await context.Acciones
                               .OrderByDescending(a => a.Fecha)
                               .Select(a => a.Fecha)
                               .FirstOrDefaultAsync();


            string respuesta = consulta.ToString();
            string[] cadena = respuesta.Split(' ');
            string fechaBBDD = cadena[0];
            string fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");

            //await BorrarEntradasMasDosSemanasAtras();


            if (fechaBBDD != fechaHoy)
            {
                foreach (var accion in acciones)
                {
                    this.context.Acciones.Add(accion);

                }
                this.context.SaveChanges();

            }

            return fechaBBDD;
        }

        //metodo para leer acciones de una pagina
        public async Task<List<Accion>> PedirAccionesPag()
        {

            string urlDatos = "https://www.investing.com/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument html = web.Load(urlDatos);

            List<Accion> acciones = new List<Accion>();

            //var nodes = html.DocumentNode.CssSelect("[class='datatable-v2_body__8TXQk' tr td div a]").Select(x => x.InnerText).Distinct();
            var nodes = html.DocumentNode.CssSelect("[class='block overflow-hidden text-ellipsis whitespace-nowrap']").Select(x => x.InnerText).Distinct();
            //var nodesCap = html.DocumentNode.CssSelect("[class='datatable-v2_cell__IwP1U dynamic-table-v2_col-other__zNU4A text-right rtl:text-right'] span").Select(x => x.InnerText).Distinct();
            var nodesMax = html.DocumentNode.CssSelect("[class='datatable-v2_cell__IwP1U dynamic-table-v2_col-other__zNU4A text-right rtl:text-right']").Select(x => x.InnerText).Distinct();
            var table = html.DocumentNode.CssSelect("[class='datatable-v2_row__hkEus dynamic-table-v2_row__ILVMx'] td").Select(x => x.InnerText);
            List<string> nodesCam = new List<string>();
            List<string> nodesCamPor = new List<string>();
            for (var i = 4; i <= table.Count() - 1; i += 7)
            {
                var porcentaje = table.ElementAt(i);
                nodesCam.Add(porcentaje);
                var porcentajePor = table.ElementAt(i + 1);
                nodesCamPor.Add(porcentajePor);
            }

            int contador = 0;

            for (int i = 0; i <= 9 - 1; i++)
            {

                Accion accion = new Accion();
                //accion.Id = 0;
                accion.Nombre = nodes.ElementAt(i).Replace("&amp;", "&"); ;
                accion.Ultimo = nodesMax.ElementAt(contador);
                accion.Maximo = nodesMax.ElementAt(contador + 1);
                accion.Minimo = nodesMax.ElementAt(contador + 2);
                accion.Cambio = nodesCam.ElementAt(i);
                accion.CambioPorcentaje = nodesCamPor.ElementAt(i);
                accion.Fecha = DateTime.Now;

                contador += 3;
                acciones.Add(accion);

            }

            return acciones;

        }

        //metodo para eliminar entradas con mas de dos semanas de antiguedad
        public async Task BorrarEntradasMasDosSemanasAtras()
        {
            // Calcular la fecha límite (hace dos semanas)
            var fechaLimite = DateTime.Today.AddDays(-15);

            // Obtener las entradas que tienen fecha de más de un mes atrás
            var entradasPorBorrar = await context.Acciones
                                                .Where(e => e.Fecha < fechaLimite)
                                                .ToListAsync();

            // Eliminar las entradas de la base de datos
            context.Acciones.RemoveRange(entradasPorBorrar);
            await context.SaveChangesAsync();
        }
    }
}
