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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public readonly ApplicationDBContext _db;
        public CategoryRepository(ApplicationDBContext db) : base(db) 
        {
            _db = db;
        }
        public void Update(Category obj)
        {
            _db.Category.Update(obj);
        }
    }
}
