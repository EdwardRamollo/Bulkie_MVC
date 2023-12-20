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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext product): base(product) 
        {
            _db = product;
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}
