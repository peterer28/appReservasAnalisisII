using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class MatrizBloqueoDetalleEndpoints
{
    public static void MapMatrizBloqueoDetalleEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/MatrizBloqueoDetalle").WithTags(nameof(MatrizBloqueoDetalle));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.MatrizBloqueoDetalle.ToListAsync();
        })
        .WithName("GetAllMatrizBloqueoDetalles")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<MatrizBloqueoDetalle>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.MatrizBloqueoDetalle.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is MatrizBloqueoDetalle model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetMatrizBloqueoDetalleById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, MatrizBloqueoDetalle matrizBloqueoDetalle, ApiReservasContext db) =>
        {
            var affected = await db.MatrizBloqueoDetalle
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, matrizBloqueoDetalle.Id)
                    .SetProperty(m => m.IdMatrizBloqueo, matrizBloqueoDetalle.IdMatrizBloqueo)
                    .SetProperty(m => m.DayOfYear, matrizBloqueoDetalle.DayOfYear)
                    .SetProperty(m => m.IsAvailable, matrizBloqueoDetalle.IsAvailable)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateMatrizBloqueoDetalle")
        .WithOpenApi();

        group.MapPost("/", async (MatrizBloqueoDetalle matrizBloqueoDetalle, ApiReservasContext db) =>
        {
            db.MatrizBloqueoDetalle.Add(matrizBloqueoDetalle);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/MatrizBloqueoDetalle/{matrizBloqueoDetalle.Id}",matrizBloqueoDetalle);
        })
        .WithName("CreateMatrizBloqueoDetalle")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.MatrizBloqueoDetalle
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteMatrizBloqueoDetalle")
        .WithOpenApi();
    }
}
