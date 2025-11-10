using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TAREFASAPI.Data;
using TAREFASAPI.Models;
using TAREFASAPI.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TarefasDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           ?? "Data Source=tarefas.db";
    options.UseSqlite(connectionString);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TarefasDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    if (!await dbContext.Tarefas.AnyAsync())
    {
        var prioridades = Enum.GetValues<Prioridade>();

        var statusOpcoes = new[] { "Não concluída", "Em progresso", "Concluída" };

        var tarefasSeed = Enumerable.Range(1, 25).Select(i => new Tarefa
        {
            Titulo = $"Tarefa {i}",
            Descricao = $"Descrição inicial da tarefa {i}.",
            Status = statusOpcoes[(i - 1) % statusOpcoes.Length],
            Prioridade = prioridades[(i - 1) % prioridades.Length],
            Responsavel = $"Responsável {((i - 1) % 5) + 1}",
            Tags = new() { $"Tag{(i % 3) + 1}", "Inicial" },
            DataVencimento = DateTime.UtcNow.AddDays(i % 7),
            EstimativaHoras = i % 4 == 0 ? (double?)(i % 6 + 1) : null,
            DataConclusao = i % 3 == 0 ? DateTime.UtcNow.AddDays(-(i % 5)) : null
        }).ToList();

        dbContext.Tarefas.AddRange(tarefasSeed);
        await dbContext.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

ROTA_GET.Map(app);
ROTA_POST.Map(app);
ROTA_DELETE.Map(app);
ROTA_PUT.Map(app);

app.Run();
