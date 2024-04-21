using Appointments;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppointmentsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => Results.Redirect("/index.html"));
app.MapGet("/create", () => Results.Redirect("/create.html"));

app.MapGet("/api/appointments", async (AppointmentsDbContext db) =>
{
    var appointments = await db.Appointments.ToListAsync();
    return Results.Ok(appointments);
});

app.MapPost("/api/appointments", async (AppointmentsDbContext db, Appointment newAppointment) => {
    db.Appointments.Add(newAppointment);
    await db.SaveChangesAsync();
    return Results.Created($"/api/appointments/{newAppointment.Id}", newAppointment);
});

app.Run();