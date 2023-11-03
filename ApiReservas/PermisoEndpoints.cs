using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class PermisoEndpoints
{
    public static void MapPermisoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Permiso").WithTags(nameof(Permiso));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.Permiso.ToListAsync();
        })
        .WithName("GetAllPermisos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Permiso>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.Permiso.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Permiso model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPermisoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Permiso permiso, ApiReservasContext db) =>
        {
            var affected = await db.Permiso
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, permiso.Id)
                    .SetProperty(m => m.TipoUsuario, permiso.TipoUsuario)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePermiso")
        .WithOpenApi();

        group.MapPost("/", async (Permiso permiso, ApiReservasContext db) =>
        {
            db.Permiso.Add(permiso);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Permiso/{permiso.Id}",permiso);
        })
        .WithName("CreatePermiso")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.Permiso
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePermiso")
        .WithOpenApi();
    }
}
