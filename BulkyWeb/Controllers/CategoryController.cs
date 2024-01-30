using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
	public class CategoryController : Controller
	{
		// we're using .Net Core hence no need to create the object on AppliationDbContext here separately as we've mentioned already that while adding services to the container that we'll need object of ApplicationDbContext, hence, the dependency injection system will automatically provide it here.
		private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
			_db = db;
        }
        public IActionResult Index()
		{
			//no need to write separate SQL query, the object of ApplicationDbContext has methods and access to the database to fetch data. (We can perform any CRUD operations with this object on any table)

			//.ToList() method retrives the data here
			List<Category> objCategoryList = _db.Categories.ToList();

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
			_db.Categories.Add(obj);
			_db.SaveChanges();

			return RedirectToAction("Index", "Category");
		}
	}
}
