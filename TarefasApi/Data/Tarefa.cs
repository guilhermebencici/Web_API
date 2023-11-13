using Dapper.Contrib.Extensions;

namespace TarefasApi.Data
{
    public class Tarefa
    {
        [Table("Tarefas")]
        public record Task(int Id, string Atividade, string Status);
    }
}
