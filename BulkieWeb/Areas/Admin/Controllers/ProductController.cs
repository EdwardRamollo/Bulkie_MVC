using Bulkie.DataAccess.Repository.IRepository;
using Bulkie.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkieWeb.Areas.Admin.Controllers
{

    /* Find and Replace in VS Ctrl+Shift+F */
   
        [Area("Admin")] // tell a controller this controller that it belongs to the Admin Area
        public class ProductController : Controller
        {
            /* Replace DBContext with Product Repository*/
            //private ApplicationDbContext _db; -- we do not need to use this, we can access the category repository.
            //private readonly IProductRepository _categoryRepo; // we are asking dependency injection to provide the implementation of category repository

            // Using UnitOfWork insted of Product
            private readonly IUnitOfWork _unitOfWork;
            public ProductController(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public IActionResult Index()
            {
                //var objProduct = _db.Categories.ToList();
                //List<Product> categories = _db.Categories.ToList();
                List<Product> products = _unitOfWork.Product.GetAll().ToList();
                return View(products);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Create(Product obj)
            {
                if (obj.Title== obj.ISBN.ToString())
                {
                    ModelState.AddModelError("name", "The ISBN cannot exactly match the Title");
                }

                if (obj.Title.ToLower() == "test")
                {
                    ModelState.AddModelError("", "Test is an invalid value");
                }
                if (ModelState.IsValid)
                {
                    //_db.Categories.Add(obj); -- with application dbcontext
                    _unitOfWork.Product.add(obj);
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
                //Product? categoryFromDb = _db.Categories.Find(id); 
                //Product? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id == id);
                //Product? categoryFromDb2 = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();

                Product? productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

                if (productFromDb == null)
                {
                    return NotFound();
                }

                return View(productFromDb);
            }

            [HttpPost]
            public IActionResult Edit(Product obj)
            {

                if (ModelState.IsValid)
                {
                    //_db.Categories.Update(obj); -- from application DBContext
                    _unitOfWork.Product.Update(obj);
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
                //Product? categoryFromDb = _db.Categories.Find(id);
                //Product? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id == id);
                //Product? categoryFromDb2 = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();

                Product productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

                if (productFromDb == null)
                {
                    return NotFound();
                }

                return View(productFromDb);
            }

            [HttpPost, ActionName("Delete")] //Explicitly state that the DeletePost endpoint name is 'Delete'.
            public IActionResult DeletePost(int? id)
            {
                //Product? obj = _db.Categories.Find(id); Application DB Context -- from application DBContext
                Product? obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    return NotFound();
                }

                //_db.Categories.Remove(obj); -- from application DBContext
                _unitOfWork.Product.Remove(obj);
                //_db.SaveChanges(); -- from application DBContext
                _unitOfWork.Save();
                TempData["success"] = "Object deleted Successfully";
                return RedirectToAction("Index");
            }
        }
}
