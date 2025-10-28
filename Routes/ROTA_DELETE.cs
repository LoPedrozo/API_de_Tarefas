using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_DELETE
    {
        public static void Map(WebApplication app)
        {
            List<Tarefa> tarefas = new List<Tarefa>
            {
                new Tarefa { Id = 1, Titulo = "Estudar C#", Status = "Não concluída" },
                new Tarefa { Id = 2, Titulo = "Fazer café", Status = "Concluída" }
            };

            app.MapDelete("/api/tarefas/{id}", (int id) =>
            {
                var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
                if (tarefa == null)
                    return Results.NotFound("Tarefa não encontrada!");

                tarefas.Remove(tarefa);
                return Results.Ok($"Tarefa {id} removida com sucesso!");
            });
        }
    }
}
