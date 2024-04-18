using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAccionesWebJob.Data
{
    public class AccionContext: DbContext
    {
        public AccionContext(DbContextOptions<AccionContext> options)
            : base(options) { }

        public DbSet<Accion> Acciones { get; set; }

    }
}
