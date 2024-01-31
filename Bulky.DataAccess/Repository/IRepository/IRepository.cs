using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //IEnumerable<T> represents set/collection of various entities that can be retrived or removed in the given methods. It has the capability to hold multiple entites at a time. (We can use List<T> also instead of IEnumerable<T>)    
        IEnumerable<T> GetAll();

        T Get(Expression<Func<T, bool>> filter);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

         
    }
}
