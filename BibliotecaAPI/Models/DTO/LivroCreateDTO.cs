namespace BibliotecaAPI.Models.DTO
{
    public class LivroCreateDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public int? AnoPublicacao { get; set; }
        public int AutorID { get; set; }
        public int GeneroID { get; set; }
    }
}
