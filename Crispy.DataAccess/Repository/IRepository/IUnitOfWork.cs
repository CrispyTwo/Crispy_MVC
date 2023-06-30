using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crispy.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICompanyRepository Company {  get; }
        ICategoryRepository Category { get; }
        IProductRepository Product { get; } 
        void Save();
    }
}
