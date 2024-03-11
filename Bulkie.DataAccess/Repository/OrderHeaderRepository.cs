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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db; // For CategoryRepository, we want to get the ApplicationDbContext using dependency injection 
        public OrderHeaderRepository(ApplicationDbContext db):base(db) // pass our ApplicationDbContext db to the base class 'Repository'
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

       
        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null) 
            {
                orderFromDb.OrderStatus = orderStatus;
                if(!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId; // session id gets generated when a user tries to make a payment.
                                                   // When its successful then a payment intentId gets generated 
            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                orderFromDb.PaymentIntentId = paymentIntentId;  // If the payment intenid is not null, that means the payment 
                orderFromDb.PaymentDate = DateTime.Now;         // was successful. In that case we will update the payment intent id,
                                                                // along with the payment date.
            }
        }
    }
}
