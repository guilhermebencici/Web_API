using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : Controller // para uma classe ser controller precisa da herança
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // MÉTODOS ACTIONS

        [HttpGet("primeiro")]
        public ActionResult<Produto> GetPrimeiro()
        {
            var produto = _context.Produtos.FirstOrDefault();
            if(produto is null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpGet] // IActionResult é uma Interface que é implementada em ActionResult, entretando, é mais interessante utilizar IAction, pois teremos flexibilidade (mais retornos)
        public IActionResult GetExemple()
        {
            var produto = _context.Produtos.FirstOrDefault();
            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }
        
        [HttpGet] //ActionResult<Tipo> é o mais performátco 
        public ActionResult<IEnumerable<Produto>> Get() // método pra retornar uma lista de objetos produto
        {//com o ActionResult, a actions espera o retorno de uma lista OU de qualquer tipo Action (ex: NotFound)
            var produtos = _context.Produtos.AsNoTracking().ToList();

            if(produtos is null)
            {
                return NotFound("Produtos não encontrados!");
            }
            return produtos;
        }

        //restringindo a rota, recebendo apenas se for int > 0 action ASSINCRONA
        [HttpGet("{id:int:min(1)}", Name ="ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p=> p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return produto;
        }
        // deixando a action ASSINCRONA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAssync()
        {
            return await  _context.Produtos.AsNoTracking().ToListAsync();
        }

        [HttpPost]
        public ActionResult Post(Produto produto)// estou passando um objeto do tipo PRODUTO
        {
            if(produto is null)
            {
                return BadRequest("Reveja as infos. Produto não salvo!");
            }
            _context.Produtos.Add(produto);// incluindo o objeto no contexto (memória)
            _context.SaveChanges(); // salvando na tabela do banco

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto) //id por parametro e produto pelo body
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            //entiddade Produto, em estado modificado, precisa ser alterado:
            _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //var prodtu = _context.Produtos.Find(id); FIND(), utilizado quando a coluna for PK
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            _context.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
