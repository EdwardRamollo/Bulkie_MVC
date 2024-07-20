using Bulkie.DataAccess.Data;
using Bulkie.DataAccess.Repository.IRepository;
using Bulkie.Models;
using Bulkie.Models.ViewModels;
using Bulkie.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkieWeb.Areas.Admin.Controllers
{

    /* Find and Replace in VS Ctrl+Shift+F */
   
    [Area("Admin")] // tell a controller this controller that it belongs to the Admin Area
    [Authorize(Roles = SD.Role_Admin)] // Only the Admin can access the Controllers action methods. You can also apply this manually on top of individual action methods
    public class UserController : Controller
    {
        /* Replace DBContext with Company Repository*/
        //private ApplicationDbContext _db; -- we do not need to use this, we can access the category repository.
        //private readonly ICompanyRepository _categoryRepo; // we are asking dependency injection to provide the implementation of category repository

        // Using UnitOfWork insted of Company
            private readonly ApplicationDbContext _db;
            public UserController(ApplicationDbContext db)
            {
                _db = db;
            }
            public IActionResult Index()
            {
                return View();
            }

           
           
           

        #region API CALLS
        // MVC Architecture has the support for API
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u=>u.Company).ToList();

            return Json(new { data = objUserList }); // return new Json object
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            return Json(new { success = true, message="Delete Successful" });
        }
       
        #endregion
    }
}
