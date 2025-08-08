var builder = DistributedApplication.CreateBuilder(args);

var weatherApi = builder.AddProject<Projects.ProfileExercise_Api>("weatherapi")
    .WithExternalHttpEndpoints();

builder.AddNpmApp("angular", "../profile-exercise-angular")
    .WithReference(weatherApi)
    .WaitFor(weatherApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();