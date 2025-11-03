using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_POST
    {
        public static void Map(WebApplication app, List<Tarefa> tarefas)
        {
            app.MapPost("/api/tarefas", (Tarefa novaTarefa) =>
            {
                var novoId = tarefas.Any() ? tarefas.Max(t => t.Id) + 1 : 1;
                novaTarefa.Id = novoId;
                tarefas.Add(novaTarefa);
                return Results.Created($"/api/tarefas/{novaTarefa.Id}", novaTarefa);
            });
        }
    }
}
