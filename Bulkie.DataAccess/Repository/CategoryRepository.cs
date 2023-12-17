using Bulkie.DataAccess.Data;
using Bulkie.DataAccess.Repository.IRepository;
using Bulkie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulkie.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db; // For CategoryRepository, we want to get the ApplicationDbContext using dependency injection 
        public CategoryRepository(ApplicationDbContext db):base(db) // pass our ApplicationDbContext db to the base class 'Repository'
        {
            _db = db;
        }
        

        /* We do not want to add this implementations again because we already have them in our generic repository, we want to implement the base functionality
* 
We want to implement the Repository class along with interface ICategoryRepository*/
        //public void add(Category entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Category> GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public Category GetFirstOrDefault(Expression<Func<Category, bool>> filter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Remove(Category entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void removeRange(IEnumerable<Category> entity)
        //{
        //    throw new NotImplementedException();
        //}

       
        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
