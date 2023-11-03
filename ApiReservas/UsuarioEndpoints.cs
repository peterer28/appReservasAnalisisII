using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Usuario").WithTags(nameof(Usuario));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.Usuario.ToListAsync();
        })
        .WithName("GetAllUsuarios")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Usuario>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.Usuario.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Usuario model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUsuarioById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Usuario usuario, ApiReservasContext db) =>
        {
            var affected = await db.Usuario
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, usuario.Id)
                    .SetProperty(m => m.Nombre, usuario.Nombre)
                    .SetProperty(m => m.AMaterno, usuario.AMaterno)
                    .SetProperty(m => m.APaterno, usuario.APaterno)
                    .SetProperty(m => m.IdContacto, usuario.IdContacto)
                    .SetProperty(m => m.IdCredenciales, usuario.IdCredenciales)
                    .SetProperty(m => m.IdPermisos, usuario.IdPermisos)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUsuario")
        .WithOpenApi();

        group.MapPost("/", async (Usuario usuario, ApiReservasContext db) =>
        {
            db.Usuario.Add(usuario);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Usuario/{usuario.Id}",usuario);
        })
        .WithName("CreateUsuario")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.Usuario
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUsuario")
        .WithOpenApi();
    }
}
