using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {        
        private readonly IUnitOfWork _unitOfWork;

        // used to access wwwroot folder to store image of product. It is present by default in .Net Framework hence no need to add dependency of this field
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {            
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();           

            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //create
                //id is not present, hence user wants to create a new product
                return View(productVM);
            }
            else
            {
                //update
                //id is present, hence he wants to update it
                productVM.Product = _unitOfWork.Product.Get(u => u.ProductId == id);
                return View(productVM);
            }

                   
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                //line 65 to 77 is the code to add the image of the new product to the respective images folder
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if(file != null)
                {
                    // not neccessary. Guid adds a random name to incoming file instead of user's weird file name. Using .GetExtension to get exsiting file extension of file
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName) ;
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //deleting the old image if the user has entered a new image
                        var oldImagePath = Path.Combine(wwwRootPath , productVM.Product.ImageUrl.TrimStart('\\'));
                            
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    //storing the imageUrl to database through productVM
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.ProductId == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();

                TempData["success"] = "Product created successfully";

                return RedirectToAction("Index", "Product");

            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.CategoryId.ToString()
                    });

                return View(productVM);
            }
        }

        // using external API to add search, sort, etc. functionalities to product table

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.ProductId == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
