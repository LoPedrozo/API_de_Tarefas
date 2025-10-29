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
            new Tarefa { Id = 30, Titulo = "Implementar autenticação JWT", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "André", Tags = new() { "Backend", "Segurança" }, EstimativaHoras = 4.5 },

            new Tarefa { Id = 31, Titulo = "Planejar processos internos", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "André", DataVencimento = DateTime.UtcNow.AddDays(2), EstimativaHoras = 2.5, Tags = new() { "Planejamento", "Estratégia" } },
            new Tarefa { Id = 32, Titulo = "Planejar pipeline de deploy", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Claudio", DataVencimento = DateTime.UtcNow.AddDays(3), EstimativaHoras = 3.5, Tags = new() { "DevOps", "CI" } },
            new Tarefa { Id = 33, Titulo = "Planejar dashboard analítico", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "Mariana", DataVencimento = DateTime.UtcNow.AddDays(4), Tags = new() { "Analytics", "BI" } },
            new Tarefa { Id = 34, Titulo = "Planejar campanha de marketing", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felipe", DataVencimento = DateTime.UtcNow.AddDays(5), EstimativaHoras = 5.5, Tags = new() { "Marketing", "Conteúdo" } },
            new Tarefa { Id = 35, Titulo = "Planejar manual do usuário", Status = "Concluída", Prioridade = Prioridade.Alta, Responsavel = "Ana", DataConclusao = DateTime.UtcNow.AddDays(-1), EstimativaHoras = 6.5, Tags = new() { "Documentação", "Manual" } },
            new Tarefa { Id = 36, Titulo = "Planejar relatório trimestral", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Carlos", DataVencimento = DateTime.UtcNow.AddDays(7), Tags = new() { "Financeiro", "Relatório" } },
            new Tarefa { Id = 37, Titulo = "Planejar treinamento da equipe", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "João", DataVencimento = DateTime.UtcNow.AddDays(8), EstimativaHoras = 2.5, Tags = new() { "RH", "Treinamento" } },
            new Tarefa { Id = 38, Titulo = "Planejar fluxo de atendimento", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felix", DataVencimento = DateTime.UtcNow.AddDays(9), EstimativaHoras = 3.5, Tags = new() { "Suporte", "Fluxo" } },
            new Tarefa { Id = 39, Titulo = "Planejar roadmap de produto", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "Bruna", DataVencimento = DateTime.UtcNow.AddDays(10), Tags = new() { "Produto", "Roadmap" } },
            new Tarefa { Id = 40, Titulo = "Planejar sprint atual", Status = "Concluída", Prioridade = Prioridade.Critica, Responsavel = "Ricardo", DataConclusao = DateTime.UtcNow.AddDays(-6), EstimativaHoras = 5.5, Tags = new() { "Ágil", "Sprint" } },
            new Tarefa { Id = 41, Titulo = "Documentar processos internos", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "André", DataVencimento = DateTime.UtcNow.AddDays(2), EstimativaHoras = 6.5, Tags = new() { "Planejamento", "Estratégia" } },
            new Tarefa { Id = 42, Titulo = "Documentar pipeline de deploy", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Claudio", DataVencimento = DateTime.UtcNow.AddDays(3), Tags = new() { "DevOps", "CI" } },
            new Tarefa { Id = 43, Titulo = "Documentar dashboard analítico", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "Mariana", DataVencimento = DateTime.UtcNow.AddDays(4), EstimativaHoras = 2.5, Tags = new() { "Analytics", "BI" } },
            new Tarefa { Id = 44, Titulo = "Documentar campanha de marketing", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Felipe", DataVencimento = DateTime.UtcNow.AddDays(5), EstimativaHoras = 3.5, Tags = new() { "Marketing", "Conteúdo" } },
            new Tarefa { Id = 45, Titulo = "Documentar manual do usuário", Status = "Concluída", Prioridade = Prioridade.Baixa, Responsavel = "Ana", DataConclusao = DateTime.UtcNow.AddDays(-4), Tags = new() { "Documentação", "Manual" } },
            new Tarefa { Id = 46, Titulo = "Documentar relatório trimestral", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Carlos", DataVencimento = DateTime.UtcNow.AddDays(7), EstimativaHoras = 5.5, Tags = new() { "Financeiro", "Relatório" } },
            new Tarefa { Id = 47, Titulo = "Documentar treinamento da equipe", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "João", DataVencimento = DateTime.UtcNow.AddDays(8), EstimativaHoras = 6.5, Tags = new() { "RH", "Treinamento" } },
            new Tarefa { Id = 48, Titulo = "Documentar fluxo de atendimento", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Felix", DataVencimento = DateTime.UtcNow.AddDays(9), Tags = new() { "Suporte", "Fluxo" } },
            new Tarefa { Id = 49, Titulo = "Documentar roadmap de produto", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "Bruna", DataVencimento = DateTime.UtcNow.AddDays(10), EstimativaHoras = 2.5, Tags = new() { "Produto", "Roadmap" } },
            new Tarefa { Id = 50, Titulo = "Documentar sprint atual", Status = "Concluída", Prioridade = Prioridade.Medio, Responsavel = "Ricardo", DataConclusao = DateTime.UtcNow.AddDays(-2), EstimativaHoras = 3.5, Tags = new() { "Ágil", "Sprint" } },
            new Tarefa { Id = 51, Titulo = "Revisar processos internos", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "André", DataVencimento = DateTime.UtcNow.AddDays(2), Tags = new() { "Planejamento", "Estratégia" } },
            new Tarefa { Id = 52, Titulo = "Revisar pipeline de deploy", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Claudio", DataVencimento = DateTime.UtcNow.AddDays(3), EstimativaHoras = 5.5, Tags = new() { "DevOps", "CI" } },
            new Tarefa { Id = 53, Titulo = "Revisar dashboard analítico", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "Mariana", DataVencimento = DateTime.UtcNow.AddDays(4), EstimativaHoras = 6.5, Tags = new() { "Analytics", "BI" } },
            new Tarefa { Id = 54, Titulo = "Revisar campanha de marketing", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felipe", DataVencimento = DateTime.UtcNow.AddDays(5), Tags = new() { "Marketing", "Conteúdo" } },
            new Tarefa { Id = 55, Titulo = "Revisar manual do usuário", Status = "Concluída", Prioridade = Prioridade.Alta, Responsavel = "Ana", DataConclusao = DateTime.UtcNow.AddDays(-7), EstimativaHoras = 2.5, Tags = new() { "Documentação", "Manual" } },
            new Tarefa { Id = 56, Titulo = "Revisar relatório trimestral", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Carlos", DataVencimento = DateTime.UtcNow.AddDays(7), EstimativaHoras = 3.5, Tags = new() { "Financeiro", "Relatório" } },
            new Tarefa { Id = 57, Titulo = "Revisar treinamento da equipe", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "João", DataVencimento = DateTime.UtcNow.AddDays(8), Tags = new() { "RH", "Treinamento" } },
            new Tarefa { Id = 58, Titulo = "Revisar fluxo de atendimento", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felix", DataVencimento = DateTime.UtcNow.AddDays(9), EstimativaHoras = 5.5, Tags = new() { "Suporte", "Fluxo" } },
            new Tarefa { Id = 59, Titulo = "Revisar roadmap de produto", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "Bruna", DataVencimento = DateTime.UtcNow.AddDays(10), EstimativaHoras = 6.5, Tags = new() { "Produto", "Roadmap" } },
            new Tarefa { Id = 60, Titulo = "Revisar sprint atual", Status = "Concluída", Prioridade = Prioridade.Critica, Responsavel = "Ricardo", DataConclusao = DateTime.UtcNow.AddDays(-5), Tags = new() { "Ágil", "Sprint" } },
            new Tarefa { Id = 61, Titulo = "Auditar processos internos", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "André", DataVencimento = DateTime.UtcNow.AddDays(2), EstimativaHoras = 2.5, Tags = new() { "Planejamento", "Estratégia" } },
            new Tarefa { Id = 62, Titulo = "Auditar pipeline de deploy", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Claudio", DataVencimento = DateTime.UtcNow.AddDays(3), EstimativaHoras = 3.5, Tags = new() { "DevOps", "CI" } },
            new Tarefa { Id = 63, Titulo = "Auditar dashboard analítico", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "Mariana", DataVencimento = DateTime.UtcNow.AddDays(4), Tags = new() { "Analytics", "BI" } },
            new Tarefa { Id = 64, Titulo = "Auditar campanha de marketing", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Felipe", DataVencimento = DateTime.UtcNow.AddDays(5), EstimativaHoras = 5.5, Tags = new() { "Marketing", "Conteúdo" } },
            new Tarefa { Id = 65, Titulo = "Auditar manual do usuário", Status = "Concluída", Prioridade = Prioridade.Baixa, Responsavel = "Ana", DataConclusao = DateTime.UtcNow.AddDays(-3), EstimativaHoras = 6.5, Tags = new() { "Documentação", "Manual" } },
            new Tarefa { Id = 66, Titulo = "Auditar relatório trimestral", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Carlos", DataVencimento = DateTime.UtcNow.AddDays(7), Tags = new() { "Financeiro", "Relatório" } },
            new Tarefa { Id = 67, Titulo = "Auditar treinamento da equipe", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "João", DataVencimento = DateTime.UtcNow.AddDays(8), EstimativaHoras = 2.5, Tags = new() { "RH", "Treinamento" } },
            new Tarefa { Id = 68, Titulo = "Auditar fluxo de atendimento", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Felix", DataVencimento = DateTime.UtcNow.AddDays(9), EstimativaHoras = 3.5, Tags = new() { "Suporte", "Fluxo" } },
            new Tarefa { Id = 69, Titulo = "Auditar roadmap de produto", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "Bruna", DataVencimento = DateTime.UtcNow.AddDays(10), Tags = new() { "Produto", "Roadmap" } },
            new Tarefa { Id = 70, Titulo = "Auditar sprint atual", Status = "Concluída", Prioridade = Prioridade.Medio, Responsavel = "Ricardo", DataConclusao = DateTime.UtcNow.AddDays(-1), EstimativaHoras = 5.5, Tags = new() { "Ágil", "Sprint" } },
            new Tarefa { Id = 71, Titulo = "Redigir processos internos", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "André", DataVencimento = DateTime.UtcNow.AddDays(2), EstimativaHoras = 6.5, Tags = new() { "Planejamento", "Estratégia" } },
            new Tarefa { Id = 72, Titulo = "Redigir pipeline de deploy", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Claudio", DataVencimento = DateTime.UtcNow.AddDays(3), Tags = new() { "DevOps", "CI" } },
            new Tarefa { Id = 73, Titulo = "Redigir dashboard analítico", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "Mariana", DataVencimento = DateTime.UtcNow.AddDays(4), EstimativaHoras = 2.5, Tags = new() { "Analytics", "BI" } },
            new Tarefa { Id = 74, Titulo = "Redigir campanha de marketing", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felipe", DataVencimento = DateTime.UtcNow.AddDays(5), EstimativaHoras = 3.5, Tags = new() { "Marketing", "Conteúdo" } },
            new Tarefa { Id = 75, Titulo = "Redigir manual do usuário", Status = "Concluída", Prioridade = Prioridade.Alta, Responsavel = "Ana", DataConclusao = DateTime.UtcNow.AddDays(-6), Tags = new() { "Documentação", "Manual" } },
            new Tarefa { Id = 76, Titulo = "Redigir relatório trimestral", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Carlos", DataVencimento = DateTime.UtcNow.AddDays(7), EstimativaHoras = 5.5, Tags = new() { "Financeiro", "Relatório" } },
            new Tarefa { Id = 77, Titulo = "Redigir treinamento da equipe", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "João", DataVencimento = DateTime.UtcNow.AddDays(8), EstimativaHoras = 6.5, Tags = new() { "RH", "Treinamento" } },
            new Tarefa { Id = 78, Titulo = "Redigir fluxo de atendimento", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felix", DataVencimento = DateTime.UtcNow.AddDays(9), Tags = new() { "Suporte", "Fluxo" } },
            new Tarefa { Id = 79, Titulo = "Redigir roadmap de produto", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "Bruna", DataVencimento = DateTime.UtcNow.AddDays(10), EstimativaHoras = 2.5, Tags = new() { "Produto", "Roadmap" } },
            new Tarefa { Id = 80, Titulo = "Redigir sprint atual", Status = "Concluída", Prioridade = Prioridade.Critica, Responsavel = "Ricardo", DataConclusao = DateTime.UtcNow.AddDays(-4), EstimativaHoras = 3.5, Tags = new() { "Ágil", "Sprint" } },
            new Tarefa { Id = 81, Titulo = "Desenvolver processos internos", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "André", DataVencimento = DateTime.UtcNow.AddDays(2), Tags = new() { "Planejamento", "Estratégia" } },
            new Tarefa { Id = 82, Titulo = "Desenvolver pipeline de deploy", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Claudio", DataVencimento = DateTime.UtcNow.AddDays(3), EstimativaHoras = 5.5, Tags = new() { "DevOps", "CI" } },
            new Tarefa { Id = 83, Titulo = "Desenvolver dashboard analítico", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "Mariana", DataVencimento = DateTime.UtcNow.AddDays(4), EstimativaHoras = 6.5, Tags = new() { "Analytics", "BI" } },
            new Tarefa { Id = 84, Titulo = "Desenvolver campanha de marketing", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Felipe", DataVencimento = DateTime.UtcNow.AddDays(5), Tags = new() { "Marketing", "Conteúdo" } },
            new Tarefa { Id = 85, Titulo = "Desenvolver manual do usuário", Status = "Concluída", Prioridade = Prioridade.Baixa, Responsavel = "Ana", DataConclusao = DateTime.UtcNow.AddDays(-2), EstimativaHoras = 2.5, Tags = new() { "Documentação", "Manual" } },
            new Tarefa { Id = 86, Titulo = "Desenvolver relatório trimestral", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Carlos", DataVencimento = DateTime.UtcNow.AddDays(7), EstimativaHoras = 3.5, Tags = new() { "Financeiro", "Relatório" } },
            new Tarefa { Id = 87, Titulo = "Desenvolver treinamento da equipe", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "João", DataVencimento = DateTime.UtcNow.AddDays(8), Tags = new() { "RH", "Treinamento" } },
            new Tarefa { Id = 88, Titulo = "Desenvolver fluxo de atendimento", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Felix", DataVencimento = DateTime.UtcNow.AddDays(9), EstimativaHoras = 5.5, Tags = new() { "Suporte", "Fluxo" } },
            new Tarefa { Id = 89, Titulo = "Desenvolver roadmap de produto", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "Bruna", DataVencimento = DateTime.UtcNow.AddDays(10), EstimativaHoras = 6.5, Tags = new() { "Produto", "Roadmap" } },
            new Tarefa { Id = 90, Titulo = "Desenvolver sprint atual", Status = "Concluída", Prioridade = Prioridade.Medio, Responsavel = "Ricardo", DataConclusao = DateTime.UtcNow.AddDays(-7), Tags = new() { "Ágil", "Sprint" } },
            new Tarefa { Id = 91, Titulo = "Avaliar processos internos", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "André", DataVencimento = DateTime.UtcNow.AddDays(2), EstimativaHoras = 2.5, Tags = new() { "Planejamento", "Estratégia" } },
            new Tarefa { Id = 92, Titulo = "Avaliar pipeline de deploy", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Claudio", DataVencimento = DateTime.UtcNow.AddDays(3), EstimativaHoras = 3.5, Tags = new() { "DevOps", "CI" } },
            new Tarefa { Id = 93, Titulo = "Avaliar dashboard analítico", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "Mariana", DataVencimento = DateTime.UtcNow.AddDays(4), Tags = new() { "Analytics", "BI" } },
            new Tarefa { Id = 94, Titulo = "Avaliar campanha de marketing", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felipe", DataVencimento = DateTime.UtcNow.AddDays(5), EstimativaHoras = 5.5, Tags = new() { "Marketing", "Conteúdo" } },
            new Tarefa { Id = 95, Titulo = "Avaliar manual do usuário", Status = "Concluída", Prioridade = Prioridade.Alta, Responsavel = "Ana", DataConclusao = DateTime.UtcNow.AddDays(-5), EstimativaHoras = 6.5, Tags = new() { "Documentação", "Manual" } },
            new Tarefa { Id = 96, Titulo = "Avaliar relatório trimestral", Status = "Não concluída", Prioridade = Prioridade.Critica, Responsavel = "Carlos", DataVencimento = DateTime.UtcNow.AddDays(7), Tags = new() { "Financeiro", "Relatório" } },
            new Tarefa { Id = 97, Titulo = "Avaliar treinamento da equipe", Status = "Não concluída", Prioridade = Prioridade.Baixa, Responsavel = "João", DataVencimento = DateTime.UtcNow.AddDays(8), EstimativaHoras = 2.5, Tags = new() { "RH", "Treinamento" } },
            new Tarefa { Id = 98, Titulo = "Avaliar fluxo de atendimento", Status = "Não concluída", Prioridade = Prioridade.Medio, Responsavel = "Felix", DataVencimento = DateTime.UtcNow.AddDays(9), EstimativaHoras = 3.5, Tags = new() { "Suporte", "Fluxo" } },
            new Tarefa { Id = 99, Titulo = "Avaliar roadmap de produto", Status = "Não concluída", Prioridade = Prioridade.Alta, Responsavel = "Bruna", DataVencimento = DateTime.UtcNow.AddDays(10), Tags = new() { "Produto", "Roadmap" } },
            new Tarefa { Id = 100, Titulo = "Avaliar sprint atual", Status = "Concluída", Prioridade = Prioridade.Critica, Responsavel = "Ricardo", DataConclusao = DateTime.UtcNow.AddDays(-3), EstimativaHoras = 5.5, Tags = new() { "Ágil", "Sprint" } }
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
