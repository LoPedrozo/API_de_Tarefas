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
               new Tarefa { Id = 1, Titulo = "Estudar C#", Descricao = "Praticar programação orientada a objetos", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "André", Tags = new() { "Estudo", "Programação" }, EstimativaHoras = 4.5 },
            new Tarefa { Id = 2, Titulo = "Fazer café", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddHours(-3), Prioridade = Prioridade.Baixa, Responsavel = "Claudio" },
            new Tarefa { Id = 3, Titulo = "Enviar relatório mensal", Descricao = "Enviar o relatório de desempenho para o gerente", Status = "Não concluída", DataVencimento = DateTime.UtcNow.AddDays(2), Prioridade = Prioridade.Critica, Responsavel = "Maria", Tags = new() { "Trabalho" } },
            new Tarefa { Id = 4, Titulo = "Revisar código do projeto", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddDays(-1), Prioridade = Prioridade.Medio, Responsavel = "Felipe", EstimativaHoras = 2 },
            new Tarefa { Id = 5, Titulo = "Ir ao supermercado", Status = "Não concluída", DataVencimento = DateTime.UtcNow.AddDays(1), Prioridade = Prioridade.Baixa, Responsavel = "Claudio", Tags = new() { "Casa" } },
            new Tarefa { Id = 6, Titulo = "Treinar na academia", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddDays(-2), Prioridade = Prioridade.Medio, Responsavel = "André", Tags = new() { "Saúde" } },
            new Tarefa { Id = 7, Titulo = "Lavar o carro", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "João", EstimativaHoras = 1.5 },
            new Tarefa { Id = 8, Titulo = "Criar landing page do cliente", Descricao = "Desenvolver página de captação para campanha de marketing", Status = "Não concluída", DataVencimento = DateTime.UtcNow.AddDays(5), Prioridade = Prioridade.Critica, Responsavel = "Claudio", Tags = new() { "Trabalho", "Web" }, EstimativaHoras = 10 },
            new Tarefa { Id = 9, Titulo = "Assistir aula de Engenharia de Software", Status = "Não concluída", DataVencimento = DateTime.UtcNow.AddHours(6), Prioridade = Prioridade.Medio, Responsavel = "André", Tags = new() { "Faculdade" } },
            new Tarefa { Id = 10, Titulo = "Fazer backup dos arquivos", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddDays(-7), Prioridade = Prioridade.Alta, Responsavel = "Carlos" },

            new Tarefa { Id = 11, Titulo = "Preparar apresentação", Status = "Não concluída", DataVencimento = DateTime.UtcNow.AddDays(3), Prioridade = Prioridade.Alta, Responsavel = "Mariana", Tags = new() { "Trabalho", "Slides" }, EstimativaHoras = 3 },
            new Tarefa { Id = 12, Titulo = "Configurar servidor Node.js", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Claudio", Tags = new() { "Backend" }, EstimativaHoras = 6 },
            new Tarefa { Id = 13, Titulo = "Limpar a casa", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddDays(-1), Prioridade = Prioridade.Baixa, Responsavel = "Ana" },
            new Tarefa { Id = 14, Titulo = "Organizar documentos pessoais", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Claudio" },
            new Tarefa { Id = 15, Titulo = "Estudar padrões de projeto", Descricao = "Ler sobre Singleton, Factory e Repository", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "André", Tags = new() { "Estudo", "C#" } },
            new Tarefa { Id = 16, Titulo = "Participar de reunião de equipe", Status = "Não concluída", DataVencimento = DateTime.UtcNow.AddHours(2), Prioridade = Prioridade.Critica, Responsavel = "Carlos" },
            new Tarefa { Id = 17, Titulo = "Atualizar currículo", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddDays(-5), Prioridade = Prioridade.Medio, Responsavel = "André" },
            new Tarefa { Id = 18, Titulo = "Ler artigo sobre Inteligência Artificial", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felix", Tags = new() { "Pesquisa", "IA" } },
            new Tarefa { Id = 19, Titulo = "Planejar viagem de férias", Status = "Não concluída", DataVencimento = DateTime.UtcNow.AddDays(15), Prioridade = Prioridade.Baixa, Responsavel = "Mariana" },
            new Tarefa { Id = 20, Titulo = "Instalar atualizações do sistema", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddDays(-3), Prioridade = Prioridade.Medio, Responsavel = "Claudio" },

            new Tarefa { Id = 21, Titulo = "Ajustar layout responsivo", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "André", Tags = new() { "Frontend", "CSS" } },
            new Tarefa { Id = 22, Titulo = "Responder e-mails", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddHours(-10), Prioridade = Prioridade.Baixa, Responsavel = "Felipe" },
            new Tarefa { Id = 23, Titulo = "Fazer orçamento para cliente", Status = "Não concluída", DataVencimento = DateTime.UtcNow.AddDays(1), Prioridade = Prioridade.Alta, Responsavel = "Claudio", Tags = new() { "Financeiro" } },
            new Tarefa { Id = 24, Titulo = "Testar API REST", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "André", Tags = new() { "Backend", "API" }, EstimativaHoras = 3.5 },
            new Tarefa { Id = 25, Titulo = "Pagar contas do mês", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddDays(-4), Prioridade = Prioridade.Alta, Responsavel = "Claudio" },
            new Tarefa { Id = 26, Titulo = "Ajustar banco de dados", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Carlos", Tags = new() { "SQL" }, EstimativaHoras = 5 },
            new Tarefa { Id = 27, Titulo = "Criar portfólio online", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "André", Tags = new() { "Web", "Pessoal" } },
            new Tarefa { Id = 28, Titulo = "Refatorar código legado", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "Claudio", Tags = new() { "Manutenção" }, EstimativaHoras = 8 },
            new Tarefa { Id = 29, Titulo = "Fazer compras do mês", Status = "Concluída", DataConclusao = DateTime.UtcNow.AddDays(-1), Prioridade = Prioridade.Medio, Responsavel = "Mariana" },
            new Tarefa { Id = 30, Titulo = "Implementar autenticação JWT", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "André", Tags = new() { "Backend", "Segurança" }, EstimativaHoras = 4.5 }
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
