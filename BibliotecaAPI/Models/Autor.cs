﻿namespace BibliotecaAPI.Models
{
    public class Autor
    {
        public int AutorID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public string? Nacionalidade { get; set; }
        public ICollection<Livro> Livros { get; set; } = new List<Livro>();

    }
}
