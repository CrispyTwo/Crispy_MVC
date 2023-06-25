using Crispy.DataAccess.Repository.IRepository;
using Crispy.Models;
using Crispy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrispyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            var productsList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(productsList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) 
                { 
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var productPath = Path.Combine(wwwRootPath, @"Images/Product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageURL)) 
                    { 
                        var oldPath = Path.Combine(wwwRootPath, productVM.Product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageURL = @"Images\Product\" + fileName;
                }
                if (productVM.Product.Id is 0)
                    _unitOfWork.Product.Add(productVM.Product);
                else
                    _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = productVM.Product.Id is 0 ? "Product has been created" : "Product has been updated";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productVM);
            }
        }
        //public IActionResult Delete(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(x => x.Id == id);
        //    if (obj is null) return NotFound();
        //    if (!string.IsNullOrEmpty(obj.ImageURL))
        //    {
        //        var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageURL.TrimStart('\\'));
        //        if (System.IO.File.Exists(oldPath))
        //            System.IO.File.Delete(oldPath);
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product has been deleted";
        //    return RedirectToAction("Index");
        //}
        #region APICalls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToDelete = _unitOfWork.Product.Get(x => x.Id == id);
            if (productToDelete == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var productPath = Path.Combine(wwwRootPath, @"Images/Product");
            if (!string.IsNullOrEmpty(productToDelete.ImageURL))
            {
                var oldPath = Path.Combine(wwwRootPath, productToDelete.ImageURL.TrimStart('\\'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }
            _unitOfWork.Product.Remove(productToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted successfully" });
        }
        #endregion
    }
}
