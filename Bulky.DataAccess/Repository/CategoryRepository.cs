using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    //the ICategoryRepository interface implements IRepository<T> and also have its own methods. But the methods of IRepository<T> have been already implemented by Repository<T> class, hence, here, we're implementing both the Repository<T> class and ICategoryRepository. It'll fetch the IRepository<T> method implementations from Repository<T> automatically.

    //implementing Repository<Category> here because we know that the entity used here will be the Category entity only

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        //we've separately mentioned the update method/operation here as each entity can have different updatation style but other operations like Searching, deleting, creating will be same hence mentioned in common IRepository<T> file.

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
