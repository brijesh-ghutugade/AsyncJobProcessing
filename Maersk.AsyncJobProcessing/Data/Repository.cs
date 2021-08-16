using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Maersk.AsyncJobProcessing.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbContext context;
        public DbSet<T> dbset;
        public Repository(DbContext context)
        {
            this.context = context;
            dbset = context.Set<T>();
        }
       

        public void Edit(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }


        public void Delete(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }

        public T GetWithChildren(int id, string child)
        {
            var model = dbset.Find(id);
            context.Entry(model).Collection(child).Load();
            return model;
        }

       

        Task<IQueryable<T>> IRepository<T>.FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = dbset.Where(predicate).AsQueryable();           

            return Task.FromResult(query);
        }

        public async Task<T> GetById(string id)
        {
            return await dbset.FindAsync(id);
        }

      
        async Task IRepository<T>.Insert(T entity)
        {
           await dbset.AddAsync(entity);
        }

        public Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> predicate, List<string> includes)
        {
            IQueryable<T> query = dbset.Where(predicate).AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return Task.FromResult(query);
        }

        Task<IQueryable<T>> IRepository<T>.GetAll()
        {
            return Task.FromResult(dbset.AsQueryable());
        }
    }
}
