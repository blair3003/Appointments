using Appointments;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/appointments", () => Results.Ok());
app.MapPost("/api/appointments", (AppointmentsDbContext dbContext, Appointment appointment) => {
    dbContext.Add(appointment);
    dbContext.SaveChangesAsync();
    return Results.Ok();
});

app.Run();