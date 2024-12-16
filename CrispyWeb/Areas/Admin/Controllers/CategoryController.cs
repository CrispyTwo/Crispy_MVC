using Crispy.DataAccess.Data;
using Crispy.DataAccess.Repository.IRepository;
using Crispy.Models;
using Crispy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrispyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var objCategoryList = _unitOfWork.Category.GetAll().ToList();

            if (Request.Headers.Accept.ToString().Contains("application/json"))
            {
                return Json(objCategoryList);
            }
            else
            {
                return View(objCategoryList);
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj != null && obj.Name.Any(x => char.IsNumber(x)))
                ModelState.AddModelError("Name", "The Name mustn't include any numbers");
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category has been created";

                if (Request.Headers.Accept.ToString().Contains("application/json"))
                {
                    return Json(obj);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id < 1) return NotFound();
            Category? categoryFromDB = _unitOfWork.Category.Get(x => x.Id == id);
            if (categoryFromDB is null) return NotFound();
            return View(categoryFromDB);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj != null && obj.Name.Any(x => char.IsNumber(x)))
                ModelState.AddModelError("Name", "The Name mustn't include any numbers");
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category has been updated";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            DeletePOST(id);
            return RedirectToAction("Index");
        }
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(x => x.Id == id);
            if (obj is null) return NotFound();
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category has been deleted";
            return RedirectToAction("Index");
        }

    }
}
