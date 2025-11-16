using TAREFASAPI.Data;
using TAREFASAPI.Models;

namespace TAREFASAPI.Routes
{
    public static class ROTA_PUT
    {
        public static void Map(WebApplication app)
        {
            app.MapPut("/api/tarefas/{id}", async (int id, Tarefa tarefaAtualizada, TarefasDbContext db) =>
            {
                var tarefaExistente = await db.Tarefas.FindAsync(id);
                if (tarefaExistente is null)
                    return Results.NotFound("Tarefa não encontrada!");

                tarefaExistente.Titulo = tarefaAtualizada.Titulo;
                tarefaExistente.Descricao = tarefaAtualizada.Descricao;
                tarefaExistente.Status = tarefaAtualizada.Status;
                tarefaExistente.Prioridade = tarefaAtualizada.Prioridade;
                tarefaExistente.Responsavel = tarefaAtualizada.Responsavel;
                tarefaExistente.Tags = tarefaAtualizada.Tags ?? new List<string>();
                tarefaExistente.Arquivada = tarefaAtualizada.Arquivada;
                tarefaExistente.DataVencimento = tarefaAtualizada.DataVencimento;
                tarefaExistente.DataConclusao = tarefaAtualizada.DataConclusao;
                tarefaExistente.EstimativaHoras = tarefaAtualizada.EstimativaHoras;

                await db.SaveChangesAsync();
                return Results.Ok(tarefaExistente);
            });
        }
    }
}
