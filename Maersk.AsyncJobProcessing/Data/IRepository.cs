using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Maersk.AsyncJobProcessing.Data
{
    public interface IRepository<T>
    {
        Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> predicate);

        Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> predicate, List<string> includes);

        Task<T> GetById(string id);

        Task<IQueryable<T>> GetAll();

        void Edit(T entity);

        Task Insert(T entity);

        void Delete(T entity);
    }
}
