using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            // we want to display Category Name and CategoryId in product view so that Admin can change the categoryId of a product, to achieve that we've created a SelectListItem as we'll display the Ids as dropdown list item. But the retrived data will be Category data so we'll have to convert it to SelectListItem data, to achieve that we'll use Projections in EF core through which we'll use .Select() method to tranform the data according to our need.
            // IEnumerable<SelectListItem> CategoryList = 

            // we use ViewBag to pass down multiple data terms to the view easily.

            //ViewBag.CategoryList = CategoryList;

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
            };

            // direct populating required properties while creating the object itself
            // when there are multiple ViewBag data points to be shared, it becomes hard to keep track of it, hence instead of it we use ViewModel Mechanism where we store all the necessary data at one place and is passed as object to View.

            return View(productVM);

            //here, after implemeting the ViewModel we've used the object of ProductVM while returning a object as it'll be used in Views also.
        }

        //this action-method actually gets the data and posts it in database. Hence, HttpPost mentioned explicitly
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product);
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

            // now as the ProductVM has all the data from both the entities (including ForeignKey), it is easy to use in view and create a new data of productVM only, hence using object of ProductVM only
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
