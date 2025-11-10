using TAREFASAPI.Data;
using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_POST
    {
        public static void Map(WebApplication app)
        {
            app.MapPost("/api/tarefas", async (Tarefa novaTarefa, TarefasDbContext db) =>
            {
                await db.Tarefas.AddAsync(novaTarefa);
                await db.SaveChangesAsync();
                return Results.Created($"/api/tarefas/{novaTarefa.Id}", novaTarefa);
            });
        }
    }
}
