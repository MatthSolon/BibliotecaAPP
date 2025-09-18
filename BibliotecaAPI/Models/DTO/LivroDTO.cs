namespace BibliotecaAPI.Models.DTO
{
    public class LivroDTO
    {
        public int LivroID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int? AnoPublicacao { get; set; }

        public  AutorDTO Autor { get; set; } = null!;
        public GeneroDTO Genero { get; set; } = null!;
    }
}
