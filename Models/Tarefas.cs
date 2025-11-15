using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json;
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
        public DateTime DataCriacao { get; set; } = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Unspecified);


        /// Data formatada de criação, exibida sem horário.
        [NotMapped]
        [JsonPropertyName("dataCriacao")]
        public string DataCriacaoFormatada
        {
            get => DataCriacao.ToString("yyyy-MM-dd");
            set
            {
                if (DateTime.TryParseExact(value,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var parsed))
                {
                    DataCriacao = DateTime.SpecifyKind(parsed, DateTimeKind.Unspecified);
                }
            }
        }


        /// Data limite para conclusão da tarefa (opcional).
        [JsonIgnore]
        public DateTime? DataVencimento { get; set; }


        /// Texto amigável para data de vencimento.
        [NotMapped]
        [JsonPropertyName("dataVencimento")]
        public string DataVencimentoDescricao
        {
            get => DataVencimento.HasValue ? DataVencimento.Value.ToString("yyyy-MM-dd") : "Sem data de vencimento";
            set
            {
                if (DateTime.TryParseExact(value,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var parsed))
                {
                    DataVencimento = DateTime.SpecifyKind(parsed, DateTimeKind.Unspecified);
                    return;
                }

                DataVencimento = null;
            }
        }


        /// Data em que a tarefa foi efetivamente concluída (opcional).
        [JsonIgnore]
        public DateTime? DataConclusao { get; set; }


        /// Texto amigável para data de conclusão.
        [NotMapped]
        [JsonPropertyName("dataConclusao")]
        public string DataConclusaoDescricao
        {
            get => DataConclusao.HasValue ? DataConclusao.Value.ToString("yyyy-MM-dd") : "Sem data de conclusão";
            set
            {
                if (DateTime.TryParseExact(value,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var parsed))
                {
                    DataConclusao = DateTime.SpecifyKind(parsed, DateTimeKind.Unspecified);
                    return;
                }

                DataConclusao = null;
            }
        }


        /// Prioridade da tarefa.
        public Prioridade Prioridade { get; set; } = Prioridade.Medio;


        /// Pessoa responsável/atribuída à tarefa.
        public string? Responsavel { get; set; }


        /// Etiquetas ou tags associadas à tarefa (não mapeadas diretamente no banco).
        [NotMapped]
        public List<string> Tags { get; set; } = new();

        /// Representação persistida das tags no banco como JSON.
        [JsonIgnore]
        public string TagsPersistidos
        {
            get
            {
                Tags ??= new List<string>();
                return JsonSerializer.Serialize(Tags, (JsonSerializerOptions?)null);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Tags = new List<string>();
                    return;
                }

                Tags = JsonSerializer.Deserialize<List<string>>(value) ?? new List<string>();
            }
        }


        /// Estimativa de horas para completar a tarefa (opcional).
        [JsonIgnore]
        public double? EstimativaHoras { get; set; }


        /// Texto amigável para estimativa de horas.
        [NotMapped]
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
        [JsonPropertyName("arquivada")]
        [JsonConverter(typeof(FlexibleBoolJsonConverter))]
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

    /// <summary>
    /// Converte valores booleanos vindos como bool, número ou textos amigáveis.
    /// </summary>
    internal sealed class FlexibleBoolJsonConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Number => reader.TryGetInt64(out var number) && number != 0,
                JsonTokenType.String => ParseString(reader.GetString()),
                _ => false
            };
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }

        private static bool ParseString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var normalized = value.Trim().ToLowerInvariant();

            if (normalized is "true" or "1")
                return true;
            if (normalized is "false" or "0")
                return false;

            if (normalized.Contains("nao") || normalized.Contains("não"))
                return false;

            return normalized.Contains("arquiv") || normalized.Contains("sim");
        }
    }
}
