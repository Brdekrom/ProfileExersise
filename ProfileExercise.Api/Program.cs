using System.Text.Json.Serialization;
using MediatR;
using ProfileExercise.Application.Commands;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Application.Queries;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddAspirePersistenceIntegration();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices()
    .AddPersistence();

builder.Services.ConfigureHttpJsonOptions(o => { o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

app.UseHttpsRedirection();

app.MapPost("/profile", async (IMediator mediator, ProfileDto dto) =>
{
    var cmd = new CreateProfileCommand(dto);
    ProfileResponseDto result = await mediator.Send(cmd);
    return Results.Created($"/profiles/{result.Profile.Id}", result);
});

app.MapPut("/profile/{id:guid}", async (IMediator mediator, Guid id, ProfileDto dto) =>
{
    if (dto.Id == string.Empty || dto.Id != id.ToString())
        return Results.BadRequest("Id in body moet overeenkomen met route");

    await mediator.Send(new UpdateProfileCommand(dto));
    return Results.NoContent();
});

app.MapGet("/profile", async (IMediator mediator) =>
{
    var list = await mediator.Send(new GetAllProfilesQuery());
    return Results.Ok(list);
});

app.MapGet("/profile/{id:guid}", async (IMediator mediator, Guid id) =>
{
    var query = new GetProfileByIdQuery(id);
    ProfileResponseDto? result = await mediator.Send(query);
    return result is not null
        ? Results.Ok(result)
        : Results.NotFound();
});

app.MapDelete("/profile/{id:guid}", async (IMediator mediator, Guid id) =>
{
    bool deleted = await mediator.Send(new DeleteProfileCommand(id));
    return deleted ? Results.NoContent() : Results.NotFound();
});


app.Run();