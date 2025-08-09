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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

app.UseHttpsRedirection();

app.MapPost("/profiles", async (IMediator mediator, ProfileDto dto) =>
{
    var cmd = new CreateProfileCommand(dto);
    ProfileResponseDto result = await mediator.Send(cmd);
    return Results.Created($"/profiles/{result.Profile.Id}", result);
});

app.MapGet("/profiles", async (IMediator mediator) =>
{
    var list = await mediator.Send(new GetAllProfilesQuery());
    return Results.Ok(list);
});

app.MapGet("/profiles/{id:guid}", async (IMediator mediator, Guid id) =>
{
    var query = new GetProfileByIdQuery(id);
    ProfileResponseDto? result = await mediator.Send(query);
    return result is not null
        ? Results.Ok(result)
        : Results.NotFound();
});

app.Run();