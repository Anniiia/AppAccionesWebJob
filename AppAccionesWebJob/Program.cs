// See https://aka.ms/new-console-template for more information
using AppAccionesWebJob.Data;
using AppAccionesWebJob.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


string connectionString =
    @"Data Source=sqlanniie.database.windows.net;Initial Catalog=AZURETAJAMAR;Persist Security Info=True;User ID=adminsql;Password=Admin123;Trust Server Certificate=True";

//NECESITAMOS UTILIZAR INYECCION DE DEPENDENCIAS
//PARA ELLO DEBEMOS CREAR UN PROVIDER
var provider = new ServiceCollection()
    .AddTransient<RepositoryAcciones>()
    .AddDbContext<AccionContext>
    (options => options.UseSqlServer(connectionString))
    .BuildServiceProvider();

//MEDIANTE EL PROVEEDOR, YA PODEMOS RECUPERAR NUESTRO REPOSITORIO
//PARA EJECUTAR EL METODO DE Populate
RepositoryAcciones repo = provider.GetService<RepositoryAcciones>();

await repo.InsertarAccionDia();


