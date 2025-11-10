using Microsoft.EntityFrameworkCore;
using TAREFASAPI.Data;
using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_GET
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/api/tarefas", async (TarefasDbContext db) =>
            {
                var lista = await db.Tarefas.AsNoTracking().ToListAsync();
                return Results.Ok(lista);
            });

            app.MapGet("/api/tarefas/{id}", async (int id, TarefasDbContext db) =>
            {
                var tarefa = await db.Tarefas.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                return tarefa is not null ? Results.Ok(tarefa) : Results.NotFound();
            });
        }
    }
}
