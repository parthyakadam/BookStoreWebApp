using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        // we're using .Net Core hence no need to create the object on AppliationDbContext here separately as we've mentioned already that while adding services to the container that we'll need object of ApplicationDbContext, hence, the dependency injection system will automatically provide it here.

        //private readonly ApplicationDbcontext _db;
        //public ProductController(ApplicationDbcontext db)
        //{
        //	_db = db;
        //}

        //as we've implemented the Repository Pattern, we'll be getting the ApplicationDbContext object in ProductRepository and won't be dirctly accesible to any controller which will give us abstraction layer, and hence we'll be using the IProductRepository object here

        //private readonly IProductRepository categoryRepo;
        //now, the UnitOfWork internally creates the object/implementation of categoryRepo, hence, we'll supply object of UnitofWork to the controller to interact with the Db.

        private readonly IUnitOfWork _unitOfWork;


        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //no need to write separate SQL query, the object of ApplicationDbContext has methods and access to the database to fetch data. (We can perform any CRUD operations with this object on any table)

            //.ToList() method converts the data here
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            //passing the objProductList to the respective view so that data can be displayed in View
            return View(objProductList);
        }

        //this action-method only displays the create form to the Admin, acts as a get request by user
        public IActionResult Create()
        {
            return View();
        }

        //this action-method actually gets the data and posts it in database. Hence, HttpPost mentioned explicitly
        [HttpPost]
        public IActionResult Create(Product obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();

                TempData["success"] = "Product created successfully";

                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFromDb = _unitOfWork.Product.Get(u => u.ProductId == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        //this action-method actually gets the data and posts it in database. Hence, HttpPost mentioned explicitly
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();

                TempData["success"] = "Product updated successfully";

                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFromDb = _unitOfWork.Product.Get(u => u.ProductId == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }


        //here we've same method-name and method-signature hence chaning method name to DeletePost and explicitly telling the system that the ActionName here is "Delete"

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.ProductId == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();

            TempData["success"] = "Product deleted successfully";

            return RedirectToAction("Index", "Product");
        }
    }
}
