using Bulkie.DataAccess.Repository.IRepository;
using Bulkie.Models;
using Bulkie.Models.ViewModels;
using Bulkie.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkieWeb.Areas.Admin.Controllers
{

    /* Find and Replace in VS Ctrl+Shift+F */
   
    [Area("Admin")] // tell a controller this controller that it belongs to the Admin Area
    [Authorize(Roles = SD.Role_Admin)] // Only the Admin can access the Controllers action methods. You can also apply this manually on top of individual action methods
    public class CompanyController : Controller
    {
        /* Replace DBContext with Company Repository*/
        //private ApplicationDbContext _db; -- we do not need to use this, we can access the category repository.
        //private readonly ICompanyRepository _categoryRepo; // we are asking dependency injection to provide the implementation of category repository

        // Using UnitOfWork insted of Company
        private readonly IUnitOfWork _unitOfWork;
            public CompanyController(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public IActionResult Index()
            {
                //var objCompany = _db.Categories.ToList();
                //List<Company> categories = _db.Categories.ToList();
                List<Company> companies = _unitOfWork.Company.GetAll().ToList();
                return View(companies);
            }

            public IActionResult Upsert(int? id)
            {
               
                if(id==null || id==0)
                {
                    // create
                    return View(new Company());
                }
                else
                {
                    // update
                    Company companyObj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                    return View(companyObj);
                }
                
            }

            [HttpPost]
            public IActionResult Upsert(Company CompanyObj)
            {
                
                if (ModelState.IsValid)
                {
                    if(CompanyObj.Id == 0)
                    {
                        _unitOfWork.Company.add(CompanyObj);
                    }
                    else
                    {
                        _unitOfWork.Company.Update(CompanyObj);
                    }
                    
                    _unitOfWork.Save();
                    TempData["success"] = "Object created Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(CompanyObj);
                }
               

            }

            

           

        #region API CALLS
        // MVC Architecture has the support for API
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Company> companys = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companys }); // return new Json object
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
