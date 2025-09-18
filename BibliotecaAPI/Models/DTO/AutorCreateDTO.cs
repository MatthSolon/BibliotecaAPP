namespace BibliotecaAPI.Models.DTO
{
    public class AutorCreateDTO
    {
        public string Nome { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public string? Nacionalidade { get; set; }
    }
}
