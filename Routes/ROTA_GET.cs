using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_GET
    {
        public static void Map(WebApplication app)
        {
            // Lista fixa simulando o "banco de dados"
            List<Tarefa> tarefas = new List<Tarefa>
            {
                new Tarefa { Id = 1, Titulo = "Estudar C#", Concluida = false },
                new Tarefa { Id = 2, Titulo = "Fazer café", Concluida = true }
            };

            // GET /api/tarefas → retorna todas
            app.MapGet("/api/tarefas", () => tarefas);

            // GET /api/tarefas/{id} → retorna tarefa específica
            app.MapGet("/api/tarefas/{id}", (int id) =>
            {
                var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
                return tarefa is not null ? Results.Ok(tarefa) : Results.NotFound();
            });
        }
    }
}
