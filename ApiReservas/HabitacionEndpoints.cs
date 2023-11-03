using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class HabitacionEndpoints
{
    public static void MapHabitacionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Habitacion").WithTags(nameof(Habitacion));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.Habitacion.ToListAsync();
        })
        .WithName("GetAllHabitacions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Habitacion>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.Habitacion.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Habitacion model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetHabitacionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Habitacion habitacion, ApiReservasContext db) =>
        {
            var affected = await db.Habitacion
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, habitacion.Id)
                    .SetProperty(m => m.IdHotel, habitacion.IdHotel)
                    .SetProperty(m => m.IdTipoHabitacion, habitacion.IdTipoHabitacion)
                    .SetProperty(m => m.IdCamas, habitacion.IdCamas)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateHabitacion")
        .WithOpenApi();

        group.MapPost("/", async (Habitacion habitacion, ApiReservasContext db) =>
        {
            db.Habitacion.Add(habitacion);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Habitacion/{habitacion.Id}",habitacion);
        })
        .WithName("CreateHabitacion")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.Habitacion
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteHabitacion")
        .WithOpenApi();
    }
}
