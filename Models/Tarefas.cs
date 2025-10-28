using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

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


        /// Status textual da tarefa.
        public string Status { get; set; } = "Não concluída";


        /// Data e hora em que a tarefa foi criada (UTC).
        [JsonIgnore]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;


        /// Data formatada de criação, exibida sem horário.
        [JsonPropertyName("dataCriacao")]
        public string DataCriacaoFormatada
        {
            get => DataCriacao.ToString("yyyy-MM-dd");
            set
            {
                if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
                    DataCriacao = parsed;
            }
        }


        /// Data limite para conclusão da tarefa (opcional).
        [JsonIgnore]
        public DateTime? DataVencimento { get; set; }


        /// Texto amigável para data de vencimento.
        [JsonPropertyName("dataVencimento")]
        public string DataVencimentoDescricao
        {
            get => DataVencimento.HasValue ? DataVencimento.Value.ToString("yyyy-MM-dd") : "Sem data de vencimento";
            set
            {
                if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
                {
                    DataVencimento = parsed;
                    return;
                }

                DataVencimento = null;
            }
        }


        /// Data em que a tarefa foi efetivamente concluída (opcional).
        [JsonIgnore]
        public DateTime? DataConclusao { get; set; }


        /// Texto amigável para data de conclusão.
        [JsonPropertyName("dataConclusao")]
        public string DataConclusaoDescricao
        {
            get => DataConclusao.HasValue ? DataConclusao.Value.ToString("yyyy-MM-dd") : "Sem data de conclusão";
            set
            {
                if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
                {
                    DataConclusao = parsed;
                    return;
                }

                DataConclusao = null;
            }
        }


        /// Prioridade da tarefa.
        public Prioridade Prioridade { get; set; } = Prioridade.Medio;


        /// Pessoa responsável/atribuída à tarefa.
        public string? Responsavel { get; set; }


        /// Etiquetas ou tags associadas à tarefa.
        public List<string>? Tags { get; set; } = new();


        /// Estimativa de horas para completar a tarefa (opcional).
        [JsonIgnore]
        public double? EstimativaHoras { get; set; }


        /// Texto amigável para estimativa de horas.
        [JsonPropertyName("estimativaHoras")]
        public string EstimativaHorasDescricao
        {
            get => EstimativaHoras.HasValue
                ? $"{EstimativaHoras.Value.ToString("0.##", CultureInfo.InvariantCulture)} horas restantes"
                : "Sem estimativa";
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    EstimativaHoras = null;
                    return;
                }

                var sanitized = value
                    .Replace("horas", string.Empty, StringComparison.OrdinalIgnoreCase)
                    .Replace("restantes", string.Empty, StringComparison.OrdinalIgnoreCase)
                    .Trim();

                if (double.TryParse(sanitized, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                {
                    EstimativaHoras = parsed;
                    return;
                }

                EstimativaHoras = null;
            }
        }


        /// Indica se a tarefa está arquivada.
        [JsonIgnore]
        public bool Arquivada { get; set; }


        /// Texto amigável para status de arquivamento.
        [JsonPropertyName("arquivada")]
        public string ArquivadaDescricao
        {
            get => Arquivada ? "Arquivada" : "Não arquivada";
            set => Arquivada = string.Equals(value, "Arquivada", StringComparison.OrdinalIgnoreCase);
        }
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
