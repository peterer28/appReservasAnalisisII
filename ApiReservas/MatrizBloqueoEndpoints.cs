using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class MatrizBloqueoEndpoints
{
    public static void MapMatrizBloqueoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/MatrizBloqueo").WithTags(nameof(MatrizBloqueo));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.MatrizBloqueo.ToListAsync();
        })
        .WithName("GetAllMatrizBloqueos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<MatrizBloqueo>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.MatrizBloqueo.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is MatrizBloqueo model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetMatrizBloqueoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, MatrizBloqueo matrizBloqueo, ApiReservasContext db) =>
        {
            var affected = await db.MatrizBloqueo
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, matrizBloqueo.Id)
                    .SetProperty(m => m.IdHabitacion, matrizBloqueo.IdHabitacion)
                    .SetProperty(m => m.ReservationYear, matrizBloqueo.ReservationYear)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateMatrizBloqueo")
        .WithOpenApi();

        group.MapPost("/", async (MatrizBloqueo matrizBloqueo, ApiReservasContext db) =>
        {
            db.MatrizBloqueo.Add(matrizBloqueo);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/MatrizBloqueo/{matrizBloqueo.Id}",matrizBloqueo);
        })
        .WithName("CreateMatrizBloqueo")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.MatrizBloqueo
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteMatrizBloqueo")
        .WithOpenApi();
    }
}
