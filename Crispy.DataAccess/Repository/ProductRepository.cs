using Crispy.DataAccess.Data;
using Crispy.DataAccess.Repository.IRepository;
using Crispy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crispy.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public readonly ApplicationDBContext _db;
        public ProductRepository(ApplicationDBContext db) : base(db) 
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objFromDb = _db.Product.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price10 = obj.Price10;
                objFromDb.Price50 = obj.Price50;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if(obj.ImageURL != null)
                    objFromDb.ImageURL = obj.ImageURL;
            }
        }
    }
}
