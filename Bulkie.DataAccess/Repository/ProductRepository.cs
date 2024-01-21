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
            // We can be explicit and retrieve the product object from the database based on the obj id that we recieve
            var objFromDb = _db.Products.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFromDb != null) 
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if(obj.ImageUrl != null) 
                {
                    objFromDb.ImageUrl = obj.ImageUrl; // then only update the ImageUrl
                }
            }
        }
    }
}
