using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulkie.DataAccess.Data;
using Bulkie.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore; //Add this using statement to locate the IRepository Interface.

namespace Bulkie.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>(); // set the current generic dbset to the generic dbset. So when we set the generic class T to Category, the dbset will be set to categories.
            //_db.Categories == dbset -- this will be equivalent to the dbset that we have.
        }
        public void add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(string? includeProperties=null)
        {
            IQueryable<T> query = dbSet;
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties
                    .Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
                    
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }

            }
            return query.FirstOrDefault();
        }

      
        public void Remove(T entity)
        {
            dbSet.Remove(entity); // Inside EF Core
        }

        public void removeRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity); // Also inside EF Core
        }
    }
}
