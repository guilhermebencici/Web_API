using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class CategoriasControler : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasControler(AppDbContext context)
        {
            _context = context;
        }

        // MÉTODOS ACTIONS
        [HttpGet("Produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList(); //Include -> Carrega entidades relacionadas
        }
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuservico, string nome)
        {
            return meuservico.Saudacaco(nome);
        }


        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get() // método pra retornar uma lista de objetos produto
        {
            return _context.Categorias.AsNoTracking().ToList();//AsNoTracking, evita o rastreamento e com isso melhora a performace - só utilizar em métodos de leitura-
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não encontrada...");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest("Reveja as infos. Categoria não salva!");
            }
            _context.Categorias.Add(categoria);// incluindo o objeto no contexto (memória)
            _context.SaveChanges(); // salvando na tabela do banco

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria) //id por parametro e produto pelo body
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            //entiddade Categoria, em estado modificado, precisa ser alterada:
            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            //var categoria = _context.Categorias.Find(id); FIND(), utilizado quando a coluna for PK
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada...");
            }

            _context.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
