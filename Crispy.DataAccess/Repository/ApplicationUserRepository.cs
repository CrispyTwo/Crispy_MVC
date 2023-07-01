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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public readonly ApplicationDBContext _db;
        public ApplicationUserRepository(ApplicationDBContext db) : base(db) 
        {
            _db = db;
        }
    }
}
