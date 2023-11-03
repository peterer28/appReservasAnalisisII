using Microsoft.EntityFrameworkCore;
using ApiReservas.Data;
using ApiReservas.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiReservas;

public static class HotelEndpoints
{
    public static void MapHotelEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Hotel").WithTags(nameof(Hotel));

        group.MapGet("/", async (ApiReservasContext db) =>
        {
            return await db.Hotel.ToListAsync();
        })
        .WithName("GetAllHotels")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Hotel>, NotFound>> (int id, ApiReservasContext db) =>
        {
            return await db.Hotel.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Hotel model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetHotelById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Hotel hotel, ApiReservasContext db) =>
        {
            var affected = await db.Hotel
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, hotel.Id)
                    .SetProperty(m => m.Nombre, hotel.Nombre)
                    .SetProperty(m => m.Direccion, hotel.Direccion)
                    .SetProperty(m => m.IdContacto, hotel.IdContacto)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateHotel")
        .WithOpenApi();

        group.MapPost("/", async (Hotel hotel, ApiReservasContext db) =>
        {
            db.Hotel.Add(hotel);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Hotel/{hotel.Id}",hotel);
        })
        .WithName("CreateHotel")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiReservasContext db) =>
        {
            var affected = await db.Hotel
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteHotel")
        .WithOpenApi();
    }
}
