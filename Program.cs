using System.Text.Json.Serialization;
using TAREFASAPI.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.MapGet("/", () => Results.Ok("Bem-vindo à API de Tarefas!"));

ROTA_GET.Map(app);
ROTA_POST.Map(app);
ROTA_DELETE.Map(app);

app.Run();

