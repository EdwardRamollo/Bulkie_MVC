using Bulkie.DataAccess.Repository.IRepository;
using Bulkie.Models;
using Bulkie.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            private readonly IWebHostEnvironment _webhostEnvironment; // we need this to access the wwwroot/images/product folder. This is provided by default with the dotnet project, so we do not have to register it. Inject this using dependency injection
            public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
            {
                _unitOfWork = unitOfWork;
                _webhostEnvironment = webHostEnvironment;
            }
            public IActionResult Index()
            {
                //var objProduct = _db.Categories.ToList();
                //List<Product> categories = _db.Categories.ToList();
                List<Product> products = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
                return View(products);
        }

            public IActionResult Upsert(int? id)
            {
                // Using projections in EF Core to convert an IEnumerable of Category to an IEnumerable of SelectListItem. ViewBag
                IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
           
                // we want to pass multiple objects from the controller to a view, so we can use a viewbag to do that.
                /* VIEWBAG 
                 -- Transfers data from controller to view, and not vice-versa. Ideal for situations in which the temporary data is not in a model

                 -- viewbag is a dynamic property that takes advantage of the new dynamic features in C# 4.0
                 -- Any number of properties and values can be assigned to ViewBag.
                 -- The ViewBag's life only lasts during the current http request. ViewBag values will be null if redirection occurs.
                 -- ViewBag is actually a wrapper around ViewData.
                 */

                //ViewBag.CategoryList = CategoryList; // use viewbag to display data to the view.

                /* We can also use a VIEWDATA as an alternative to pass data to a view
                 * ViewData transfers data from the Controller to the View, not vice-versa. Ideal for situations in which the temporary data
                 * is not in a model.
                 * ViewData is derived from ViewDataDictionary which is a dictionary type.
                 * ViewData value must be type cast before use.
                 * The ViewData's life only lasts during the current http request. ViewData values will be null if redirection occurs. 
                 */

                 /* VIEWBAG internally inserts data into ViewData dictionary. So the key of ViewData and property of ViewBag must not match */

                 /* TEMPDATA
                    TempData can be used to store data between two consecutive requests.
                    TempData internally use Session to store the data. So think ofit as a short lived session.
                    TempData value must be type cast before use. Check for null values to avoid runtime error. 
                    TempData can be used to store only one time messages like error messages, validation messages. */
                //ViewData["CategoryList"] = CategoryList;

                ProductVM productVM = new()
                {
                    CategoryList = CategoryList,
                    Product = new Product()
                };
                if(id==null || id==0)
                {
                    // create
                    return View(productVM);
                }
                else
                {
                    // update
                    productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                    return View(productVM);
                }
                
            }

            [HttpPost]
            public IActionResult Upsert(ProductVM productVM, IFormFile ? file /*When a file is uploaded we will get that in the IformFile*/)
            {
                if (productVM.Product.Title== productVM.Product.ISBN.ToString())
                {
                    ModelState.AddModelError("name", "The ISBN cannot exactly match the Title");
                }

                if (productVM.Product.Title.ToLower() == "test")
                {
                    ModelState.AddModelError("", "Test is an invalid value");
                }
                if (ModelState.IsValid)
                {
                    //_db.Categories.Add(productVM); -- with application dbcontext
                    
                //_db.SaveChanges(); -- with application dbcontext
                    string wwwRootPath = _webhostEnvironment.WebRootPath; // this will give us the wwwroot folder
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // random guid -- this will give us a random name with its extension for our file
                        string productPath = Path.Combine(wwwRootPath, @"images\product"); // combine the root path with the product path

                        if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                        {
                            // delete the old image
                            var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                            if(System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productVM.Product.ImageUrl = @"\images\product\" + fileName;
                    }

                    if(productVM.Product.Id == 0)
                    {
                        _unitOfWork.Product.add(productVM.Product);
                    }
                    else
                    {
                        _unitOfWork.Product.Update(productVM.Product);
                    }
                    
                    _unitOfWork.Save();
                    TempData["success"] = "Object created Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });

                    return View(productVM);
                }
               

            }

            

           

        #region API CALLS
        // MVC Architecture has the support for API
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products }); // return new Json object
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webhostEnvironment.WebRootPath,
                               productToBeDeleted.ImageUrl.TrimStart('\\'));

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
