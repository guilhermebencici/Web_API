using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace APICatalogo.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _context;

        // CONSTRUTOR
        public Repository(AppDbContext contexto)
        {
            _context = contexto;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        // RETORNA UMA LISTA DE ENTIDADES
        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();// DESABILITANDO O RASTREAMENTO DE ENTIDADES (ASNOTRACKING)
        }
        //RETORNANDO UMA ENTIDADE
        public T GetById(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().SingleOrDefault(predicate);// PREDICATE VALIDA O CRITÉRIO
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;// INFORMANDO O CONTEXTO QUE A ENTIDADE FOI ALTERADA
            _context.Set<T>().Update(entity);
        }
    }
}
