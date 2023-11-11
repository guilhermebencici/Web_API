using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        public Categoria()
        {   // INICIALIZANDO A PROPRIEDADE PRODUTOS
            Produtos = new Collection<Produto>();
        }
        [Key]
        public int CategoriaId { get; set; }
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }
        [Required]
        [StringLength(300)]
        public string? ImgUrl { get; set; }
        // AQUI EU DEFINO QUE CATEGORIA POSSUI UMA COLEÇÃO DE PRODUTOS
        public ICollection<Produto>? Produtos { get; set; }
    }
}
