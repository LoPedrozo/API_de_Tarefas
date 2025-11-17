using TAREFASAPI.Data;
using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_DELETE
    {
        public static void Map(WebApplication app)
        {
            app.MapDelete("/api/tarefas/{id}", async (int id, TarefasDbContext db) =>
            {
                var tarefa = await db.Tarefas.FindAsync(id);
                if (tarefa == null)
                    return Results.NotFound("Tarefa não encontrada!");

                db.Tarefas.Remove(tarefa);
                await db.SaveChangesAsync();
                return Results.Ok($"Tarefa {id} removida com sucesso!");
            });
        }
    }
}