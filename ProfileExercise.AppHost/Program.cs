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

var api = builder.AddProject<ProfileExercise_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(db)
    .WaitFor(db);

builder.AddNpmApp("angular", "../profile-exercise-angular")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();