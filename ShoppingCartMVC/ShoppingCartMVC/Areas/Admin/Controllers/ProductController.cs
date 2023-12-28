using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCartMVC.DataAccess.Data;
using ShoppingCartMVC.DataAccess.Repository.IRepository;
using ShoppingCartMVC.Models;
using ShoppingCartMVC.Models.ViewModels;
using ShoppingCartMVC.Utility;
using System.Collections;
using System.Data;

namespace ShoppingCartMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
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
            List<Product> objList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            IEnumerable result = objList.Select(u => new
            {
                author = u.Author,
                title = u.Title,
            });
            return View(objList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().
                    Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
            //ViewBag.categoryList = categoryList;
            //ViewData["categoryList"] = categoryList;
            ProductVM productVM = new ProductVM()
            {
                CategoryList = categoryList,
                Product = new Product()
            };
            if(id == 0 || id == null)
            {
                // For Create 
                return View(productVM);
            }
            else
            {
                // For update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM objVM, IFormFile? file)
        {
            var id = objVM.Product.Id;
            if (!ModelState.IsValid)
            {
                // To handle the exception without using ValidateNever Data annotation
                objVM.CategoryList = _unitOfWork.Category.GetAll().
                Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(objVM);
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(objVM.Product.ImageURL))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, objVM.Product.ImageURL.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                objVM.Product.ImageURL = @"\images\product\" + fileName;
            }
            if(objVM.Product.Id == 0)
            {
                _unitOfWork.Product.Add(objVM.Product);
            }
            else
            {
                _unitOfWork.Product.Update(objVM.Product);
            }
            _unitOfWork.Save();
            if (id == 0)
            {
                TempData["success"] = "Product created successfully";
            }
            else
            {
                TempData["success"] = "Product updated successfully";
            }
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product obj = _unitOfWork.Product.Get(item => item.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(obj);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    _unitOfWork.Product.Update(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Category updated successfully";
        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product obj = _unitOfWork.Product.Get(item => item.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(obj);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product obj = _unitOfWork.Product.Get(item => item.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    string wwwRootPath = _webHostEnvironment.WebRootPath;
        //    if (!string.IsNullOrEmpty(obj.ImageURL))
        //    {
        //        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageURL.TrimStart('\\'));
        //        if (System.IO.File.Exists(oldImagePath))
        //        {
        //            System.IO.File.Delete(oldImagePath);
        //        }
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product obj = _unitOfWork.Product.Get(item => item.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(obj.ImageURL))
            {
                var oldImagePath = Path.Combine(wwwRootPath, obj.ImageURL.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion

    }
}
