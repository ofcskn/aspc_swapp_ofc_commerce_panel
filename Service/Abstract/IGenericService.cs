using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IGenericService<T> where T : class
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
        public IQueryable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
