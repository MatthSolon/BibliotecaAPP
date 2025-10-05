namespace BibliotecaAPI.Models
{
    public class Aluno
    {
        public int AlunoID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<Livro> LivrosEmprestados { get; set; } = new List<Livro>();
    }
}
