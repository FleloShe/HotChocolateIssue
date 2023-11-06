using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton(ConnectionMultiplexer.Connect("localhost:6379"))
    .AddGraphQLServer()
    .AddTypes()
    // We initialize the schema on startup so it is published to the redis as soon as possible
    .InitializeOnStartup()
    // We configure the publish definition
    .PublishSchemaDefinition(c => c
        // The name of the schema. This name should be unique
        .SetName("Author")
        //.AddTypeExtensionsFromFile("./Stitching.graphql")
        .PublishToRedis(
            // The configuration name under which the schema should be published
            "Test",
            // The connection multiplexer that should be used for publishing
            sp => sp.GetRequiredService<ConnectionMultiplexer>()));

var app = builder.Build();

app.MapGraphQL();

app.RunWithGraphQLCommands(args);
