using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class CamaEndpoints
{
    public static void MapCamaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cama").WithTags(nameof(Cama));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.Cama.ToListAsync();
        })
        .WithName("GetAllCamas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cama>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.Cama.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Cama model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCamaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Cama cama, ApiReservasContext db) =>
        {
            var affected = await db.Cama
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, cama.Id)
                    .SetProperty(m => m.Disposicion, cama.Disposicion)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCama")
        .WithOpenApi();

        group.MapPost("/", async (Cama cama, ApiReservasContext db) =>
        {
            db.Cama.Add(cama);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cama/{cama.Id}",cama);
        })
        .WithName("CreateCama")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.Cama
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCama")
        .WithOpenApi();
    }
}
