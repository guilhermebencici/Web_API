using APICatalogo.Context;
using APICatalogo.Models;
using System.Collections.Generic;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            throw new System.NotImplementedException();
        }
    }
}
