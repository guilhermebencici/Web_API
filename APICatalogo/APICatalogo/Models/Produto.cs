using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }
        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }
        [Required]
        [Column(TypeName ="decimal(10,2)")]
        public decimal Preco { get; set; }
        [Required]
        [StringLength(300)]
        public string ImgUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        // CategoriaId, vai mapear a coluna de mesmo nome criada na entidade Produto!
        public int CategoriaId { get; set; }
        [JsonIgnore] // ao utilizar este atributo, as propriedades da Entidade Categoria, será ignorada 
        public Categoria? Categoria { get; set; }//dinfinindo o relacionamento entre protudo e categoria, com essa prop de navegação
    }
}
