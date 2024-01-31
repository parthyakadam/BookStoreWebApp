using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers
{
	public class CategoryController : Controller
    {
        // we're using .Net Core hence no need to create the object on AppliationDbContext here separately as we've mentioned already that while adding services to the container that we'll need object of ApplicationDbContext, hence, the dependency injection system will automatically provide it here.

        //private readonly ApplicationDbcontext _db;
        //public CategoryController(ApplicationDbcontext db)
        //{
        //	_db = db;
        //}

		//as we've implemented the Repository Pattern, we'll be getting the ApplicationDbContext object in CategoryRepository and won't be dirctly accesible to any controller which will give us abstraction layer, and hence we'll be using the ICategoryRepository object here

        private readonly ICategoryRepository categoryRepo;

        public CategoryController(ICategoryRepository db)
        {
			categoryRepo = db;
        }
        public IActionResult Index()
		{
			//no need to write separate SQL query, the object of ApplicationDbContext has methods and access to the database to fetch data. (We can perform any CRUD operations with this object on any table)

			//.ToList() method converts the data here
			List<Category> objCategoryList = categoryRepo.GetAll().ToList();

			//passing the objCategoryList to the respective view so that data can be displayed in View
			return View(objCategoryList);
		}

		//this action-method only displays the create form to the Admin, acts as a get request by user
		public IActionResult Create()
		{
			return View();
		}

		//this action-method actually gets the data and posts it in database. Hence, HttpPost mentioned explicitly
		[HttpPost]
		public IActionResult Create(Category obj)
		{

			if (ModelState.IsValid)
			{
				categoryRepo.Add(obj);
				categoryRepo.Save();

				TempData["success"] = "Category created successfully";

				return RedirectToAction("Index", "Category");
			}
			
			return View();
		}

		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Category? categoryFromDb = categoryRepo.Get(u => u.CategoryId == id);

			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		//this action-method actually gets the data and posts it in database. Hence, HttpPost mentioned explicitly
		[HttpPost]
		public IActionResult Edit(Category obj)
		{
			if (ModelState.IsValid)
			{
				categoryRepo.Update(obj);
				categoryRepo.Save();

				TempData["success"] = "Category updated successfully";

				return RedirectToAction("Index", "Category");
			}

			return View();
		}

		public IActionResult Delete(int? id)
		{
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = categoryRepo.Get(u => u.CategoryId==id);

            if (categoryFromDb.CategoryId == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
		}


		//here we've same method-name and method-signature hence chaning method name to DeletePost and explicitly telling the system that the ActionName here is "Delete"
		
		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Category? categoryFromDb = categoryRepo.Get(u => u.CategoryId == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            categoryRepo.Remove(categoryFromDb);
			categoryRepo.Save();

			TempData["success"] = "Category deleted successfully";

			return RedirectToAction("Index", "Category");
		}
	}
}
