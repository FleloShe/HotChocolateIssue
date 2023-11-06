using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHeaderPropagation(c =>
    {
        c.Headers.Add("GraphQL-Preflight");
        c.Headers.Add("Authorization");
    });

var clients = new List<(string SchemaName, string BaseUrl)>()
{
    ("Book", "http://localhost:5097/graphql"),
    ("Author", "http://localhost:5099/graphql"),
};

foreach (var client in clients)
{
    builder.Services.AddHttpClient(client.SchemaName, (sp, c) =>
    {
        c.BaseAddress = new Uri(client.BaseUrl);
    }).AddHeaderPropagation();
}

builder.Services
    // This is the connection multiplexer that redis will use
    .AddSingleton(ConnectionMultiplexer.Connect("localhost:6379"))
    .AddGraphQLServer()
    .AddRemoteSchemasFromRedis("Test",
        sp => sp.GetRequiredService<ConnectionMultiplexer>());

var app = builder.Build();


app.UseHeaderPropagation();
app.MapGraphQL();

app.RunWithGraphQLCommands(args);
