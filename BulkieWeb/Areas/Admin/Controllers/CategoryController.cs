
using Bulkie.DataAccess.Data;
using Bulkie.DataAccess.Repository.IRepository;
using Bulkie.Models;
using Bulkie.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using static System.Net.Mime.MediaTypeNames;

namespace BulkieWeb.Areas.Admin.Controllers
{
    [Area("Admin")] // tell a controller this controller that it belongs to the Admin Area
    [Authorize(Roles =SD.Role_Admin)] // Only the Admin can access the Controllers action methods. You can also apply this manually on top of individual action methods
    public class CategoryController : Controller
    {
        /* Replace DBContext with Category Repository*/
        //private ApplicationDbContext _db; -- we do not need to use this, we can access the category repository.
        //private readonly ICategoryRepository _categoryRepo; // we are asking dependency injection to provide the implementation of category repository

        // Using UnitOfWork insted of Category
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //var objCategory = _db.Categories.ToList();
            //List<Category> categories = _db.Categories.ToList();
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }

            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "Test is an invalid value");
            }
            if (ModelState.IsValid)
            {
                //_db.Categories.Add(obj); -- with application dbcontext
                _unitOfWork.Category.add(obj);
                //_db.SaveChanges(); -- with application dbcontext
                _unitOfWork.Save();
                TempData["success"] = "Object created Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            /* All the three implementations with application dbcontext */
            //Category? categoryFromDb = _db.Categories.Find(id); 
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();

            Category? categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                //_db.Categories.Update(obj); -- from application DBContext
                _unitOfWork.Category.Update(obj);
                //_db.SaveChanges(); -- from application DBContext
                _unitOfWork.Save();
                TempData["success"] = "Object edited Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            /*--from application DBContext*/
            //Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();

            Category categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")] //Explicitly state that the DeletePost endpoint name is 'Delete'.
        public IActionResult DeletePost(int? id)
        {
            //Category? obj = _db.Categories.Find(id); Application DB Context -- from application DBContext
            Category? obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            //_db.Categories.Remove(obj); -- from application DBContext
            _unitOfWork.Category.Remove(obj);
            //_db.SaveChanges(); -- from application DBContext
            _unitOfWork.Save();
            TempData["success"] = "Object deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
