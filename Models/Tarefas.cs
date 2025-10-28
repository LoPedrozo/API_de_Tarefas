using System;
using System.Collections.Generic;

namespace TAREFASAPI.Models
{
    public class Tarefa
    {
        /// Identificador único da tarefa.
        public int Id { get; set; }


        /// Título curto da tarefa.
        public required string Titulo { get; set; }


        /// Descrição detalhada da tarefa.
        public string? Descricao { get; set; }


        /// Indica se a tarefa foi concluída.
        public bool Concluida { get; set; }


        /// Data e hora em que a tarefa foi criada (UTC).
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;


        /// Data limite para conclusão da tarefa (opcional).
        public DateTime? DataVencimento { get; set; }


        /// Data em que a tarefa foi efetivamente concluída (opcional).
        public DateTime? DataConclusao { get; set; }


        /// Prioridade da tarefa.
        public Prioridade Prioridade { get; set; } = Prioridade.Medio;


        /// Pessoa responsável/atribuída à tarefa.
        public string? Responsavel { get; set; }


        /// Etiquetas ou tags associadas à tarefa.
        public List<string>? Tags { get; set; } = new();


        /// Estimativa de horas para completar a tarefa (opcional).
        public double? EstimativaHoras { get; set; }


        /// Indica se a tarefa está arquivada.
        public bool Arquivada { get; set; }
    }
    

    /// Níveis de prioridade que podem ser associados a uma tarefa.
    public enum Prioridade
    {
        Baixa,
        Medio,
        Alta,
        Critica
    }
}
