using Crispy.DataAccess.Data;
using Crispy.Models;
using Crispy.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crispy.DataAccess.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        readonly ApplicationDBContext _db;
        readonly UserManager<IdentityUser> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;
        public DBInitializer(ApplicationDBContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }
            if (!_roleManager.RoleExistsAsync(SD.RoleCustomer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.RoleCustomer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleEmployee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleCompany)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "krj270111@gmail.com",
                    Email = "krj270111@gmail.com",
                    Name = "Artem Huzhvii",
                    PhoneNumber = "1112223333",
                    Street = "test 123 Ave",
                    Region = "IL",
                    Country = "USA",
                    City = "Chicago",
                    EmailConfirmed = true
                }, "130484***Ar").GetAwaiter().GetResult();
                ApplicationUser user = _db.User.FirstOrDefault(x => x.Email == "krj270111@gmail.com");
                _userManager.AddToRoleAsync(user, SD.RoleAdmin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
