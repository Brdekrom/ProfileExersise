using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("cosmosdb")
    .RunAsEmulator(em =>
    {
        em.WithGatewayPort(8090)
            .WithDataVolume();
    });

var db = cosmos.AddCosmosDatabase("exercisedb");

var container = db.AddContainer("profiles", "/id");

var profileapi = builder.AddProject<ProfileExercise_Api>("profileapi")
    .WithReference(db)
    //.WaitFor(db)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("angular", "../profile-exercise-angular")
    .WithReference(profileapi)
    .WaitFor(profileapi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();