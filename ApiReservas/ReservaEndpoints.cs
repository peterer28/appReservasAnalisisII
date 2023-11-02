using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class ReservaEndpoints
{
    public static void MapReservaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Reserva").WithTags(nameof(Reserva));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.Reserva.ToListAsync();
        })
        .WithName("GetAllReservas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Reserva>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.Reserva.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Reserva model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetReservaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Reserva reserva, ApiReservasContext db) =>
        {
            var affected = await db.Reserva
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, reserva.Id)
                    .SetProperty(m => m.IdUsuario, reserva.IdUsuario)
                    .SetProperty(m => m.IdHabitacion, reserva.IdHabitacion)
                    .SetProperty(m => m.Pago, reserva.Pago)
                    .SetProperty(m => m.FechaIngreso, reserva.FechaIngreso)
                    .SetProperty(m => m.FechaSalida, reserva.FechaSalida)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateReserva")
        .WithOpenApi();

        group.MapPost("/", async (Reserva reserva, ApiReservasContext db) =>
        {
            db.Reserva.Add(reserva);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Reserva/{reserva.Id}",reserva);
        })
        .WithName("CreateReserva")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.Reserva
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteReserva")
        .WithOpenApi();
    }
}
