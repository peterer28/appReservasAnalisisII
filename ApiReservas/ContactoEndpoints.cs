using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class ContactoEndpoints
{
    public static void MapContactoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Contacto").WithTags(nameof(Contacto));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.Contacto.ToListAsync();
        })
        .WithName("GetAllContactos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Contacto>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.Contacto.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Contacto model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetContactoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Contacto contacto, ApiReservasContext db) =>
        {
            var affected = await db.Contacto
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, contacto.Id)
                    .SetProperty(m => m.Email, contacto.Email)
                    .SetProperty(m => m.Phone, contacto.Phone)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateContacto")
        .WithOpenApi();

        group.MapPost("/", async (Contacto contacto, ApiReservasContext db) =>
        {
            db.Contacto.Add(contacto);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Contacto/{contacto.Id}",contacto);
        })
        .WithName("CreateContacto")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.Contacto
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteContacto")
        .WithOpenApi();
    }
}
