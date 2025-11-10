using Microsoft.EntityFrameworkCore;
using TAREFASAPI.Models;

namespace TAREFASAPI.Data;

public class TarefasDbContext : DbContext
{
    public TarefasDbContext(DbContextOptions<TarefasDbContext> options) : base(options)
    {
    }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.ToTable("Tarefas");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Status).HasMaxLength(100);
            entity.Property(t => t.Responsavel).HasMaxLength(200);
            entity.Property(t => t.TagsPersistidos).HasColumnName("TagsJson");
        });
    }
}
