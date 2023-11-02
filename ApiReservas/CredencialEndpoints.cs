using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class CredencialEndpoints
{
    public static void MapCredencialEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Credencial").WithTags(nameof(Credencial));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.Credencial.ToListAsync();
        })
        .WithName("GetAllCredencials")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Credencial>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.Credencial.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Credencial model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCredencialById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Credencial credencial, ApiReservasContext db) =>
        {
            var affected = await db.Credencial
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, credencial.Id)
                    .SetProperty(m => m.Usuario, credencial.Usuario)
                    .SetProperty(m => m.Hash, credencial.Hash)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCredencial")
        .WithOpenApi();

        group.MapPost("/", async (Credencial credencial, ApiReservasContext db) =>
        {
            db.Credencial.Add(credencial);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Credencial/{credencial.Id}",credencial);
        })
        .WithName("CreateCredencial")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.Credencial
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCredencial")
        .WithOpenApi();
    }
}
