using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_POST
    {
        public static void Map(WebApplication app)
        {
            List<Tarefa> tarefas = new List<Tarefa>(); // Simples lista local

            app.MapPost("/api/tarefas", (Tarefa novaTarefa) =>
            {
                novaTarefa.Id = tarefas.Count + 1;
                tarefas.Add(novaTarefa);
                return Results.Created($"/api/tarefas/{novaTarefa.Id}", novaTarefa);
            });
        }
    }
}
