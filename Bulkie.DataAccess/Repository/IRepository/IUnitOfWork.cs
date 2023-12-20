using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkie.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        /* In here we will have all the repositories */
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }

        void Save();
    }
}
