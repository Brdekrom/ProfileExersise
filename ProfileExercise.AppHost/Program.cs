var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.ProfileExercise_Api>("weatherapi")
    .WithExternalHttpEndpoints();

builder.AddNpmApp("angular", "../profile-exercise-angular")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();