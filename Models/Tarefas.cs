namespace TAREFASAPI.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public bool Concluida { get; set; }
    }
}
