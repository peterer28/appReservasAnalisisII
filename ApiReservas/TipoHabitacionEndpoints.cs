using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class TipoHabitacionEndpoints
{
    public static void MapTipoHabitacionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TipoHabitacion").WithTags(nameof(TipoHabitacion));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.TipoHabitacion.ToListAsync();
        })
        .WithName("GetAllTipoHabitacions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<TipoHabitacion>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.TipoHabitacion.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is TipoHabitacion model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTipoHabitacionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, TipoHabitacion tipoHabitacion, ApiReservasContext db) =>
        {
            var affected = await db.TipoHabitacion
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, tipoHabitacion.Id)
                    .SetProperty(m => m.Tipo, tipoHabitacion.Tipo)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTipoHabitacion")
        .WithOpenApi();

        group.MapPost("/", async (TipoHabitacion tipoHabitacion, ApiReservasContext db) =>
        {
            db.TipoHabitacion.Add(tipoHabitacion);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/TipoHabitacion/{tipoHabitacion.Id}",tipoHabitacion);
        })
        .WithName("CreateTipoHabitacion")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.TipoHabitacion
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTipoHabitacion")
        .WithOpenApi();
    }
}
