namespace BibliotecaAPI.Models
{
    public class Livro
    {
        public int LivroID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int? AnoPublicacao { get; set; }
        public int AutorID { get; set; }
        public Autor Autor { get; set; } = null!;
        public int GeneroID { get; set; }
        public Genero Genero { get; set; } = null!;

    }
}
