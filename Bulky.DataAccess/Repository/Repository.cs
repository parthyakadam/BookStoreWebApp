using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        //while dealing with database now, the incoming entity <T> is generic and is yet unknown. Hence, the system doesn't directly know which entity might be present here. Hence we cannot use _db.Categories.Add(entity) --> (it wont be a generic method) or _db.T.Add(entities) also --> (we don't know what T as data-type is here)

        //So, as a solution, we use DbSet<T>, where, when we assign this.dbSet = _db.Set<T>(); we know the value to T and now, dbSet.Add(entity) will act as _db.Categories.Add(entity).

        //DbSet<T> is a class in Entity Framework that represents a collection is entites

        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);  //where dbSet == _db.Categories or _db.Products etc.
        }

        //here, we're retriving a entity using Get method where we're passsing the filter condition as the parameter and the function will return a entity of type 'T' which will be used as parameter for Get method

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);

            return query.FirstOrDefault();
        }

        //using .ToList() method to

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);   
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);            
        }

        // in the above code, IQueryable<T> represents a query that can be performed on data source, here database. We've used the query object of IQueryable to construct query before performing operations like .ToList() and .Where() on database. It performes the query on database and stores the fetched result in query. We use IQuerable query to construct the query against the data source dbSet, and doesn't have the ability to store the data in memory. Until we perform query.ToList() or query.Where() actual data is not fetched from data base.

        // Whereas, IEnumerable<T> represents collection of entites that can be iterated over. We've used it when returning the results of IQueryable<T>. IEnumerable<T> represents data and is present in the memory.
    }
}