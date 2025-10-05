namespace BibliotecaAPI.Models.DTO
{
    public class LivroDTO
    {
        public int LivroID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int? AnoPublicacao { get; set; }

        // IDs simples (sem objetos aninhados)
        public int AutorID { get; set; }
        public string? NomeAutor { get; set; }

        public int GeneroID { get; set; }
        public string? NomeGenero { get; set; }
    }
}
