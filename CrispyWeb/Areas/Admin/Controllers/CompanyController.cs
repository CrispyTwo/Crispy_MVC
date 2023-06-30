using Crispy.DataAccess.Repository.IRepository;
using Crispy.Models;
using Crispy.Models.ViewModels;
using Crispy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace CrispyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            var CompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(CompanyList);
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id is 0)
                    _unitOfWork.Company.Add(company);
                else
                    _unitOfWork.Company.Update(company);
                _unitOfWork.Save();
                TempData["success"] = company.Id is 0 ? "Company has been created" : "Company has been updated";
                return RedirectToAction("Index");
            }
            else
            {
                return View(company);
            }
        }
        //public IActionResult Delete(int? id)
        //{
        //    Company? obj = _unitOfWork.Company.Get(x => x.Id == id);
        //    if (obj is null) return NotFound();
        //    if (!string.IsNullOrEmpty(obj.ImageURL))
        //    {
        //        var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageURL.TrimStart('\\'));
        //        if (System.IO.File.Exists(oldPath))
        //            System.IO.File.Delete(oldPath);
        //    }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company has been deleted";
        //    return RedirectToAction("Index");
        //}
        #region APICalls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToDelete = _unitOfWork.Company.Get(x => x.Id == id);
            if (CompanyToDelete == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }
            _unitOfWork.Company.Remove(CompanyToDelete);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted successfully" });
        }
        #endregion
    }
}
