using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_GET
    {
        public static void Map(WebApplication app, List<Tarefa> tarefas)
        {
            app.MapGet("/api/tarefas", () => tarefas);

            app.MapGet("/api/tarefas/{id}", (int id) =>
            {
                var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
                return tarefa is not null ? Results.Ok(tarefa) : Results.NotFound();
            });
        }
    }
}
