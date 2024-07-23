using Bulkie.DataAccess.Data;
using Bulkie.Models;
using Bulkie.Models.ViewModels;
using Bulkie.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkieWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            //Based on the userId, I want to retrieve the user roleid, and that I can retrieve from the UserRoles Table.
            string RoleID = _db.UserRoles.FirstOrDefault(u=>u.UserId== userId).RoleId;

            //Populate the ManagementRole View Model, and inside there we need Application users, and we will get that from
            RoleManagementVM RoleVM = new RoleManagementVM()
            {
                ApplicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                //Next we have to populate the dropdowns for rolelist and companylist
                RoleList = _db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _db.Companies.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

            };

            //Inside the RoleVM we have Application user, and there we have role of the user. That we will populate using db.roles. We want to extract name from there.
            RoleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;
            return View(RoleVM); // when we return back to the view, we have to pass role view model(RoleVM)
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            //Based on the userId, I want to retrieve the user roleid, and that I can retrieve from the UserRoles Table.
            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == roleManagementVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

            if(!(roleManagementVM.ApplicationUser.Role == oldRole))
            {
                // a role was updated
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u=>u.Id==roleManagementVM.ApplicationUser.Id);
                if(roleManagementVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                if(oldRole== SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }

             return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll() 
        {
            List<ApplicationUser> users = _db.ApplicationUsers.Include(u=>u.Company).ToList();

            // We can access any of the built-in identity tables using DbContext by removing the aspnet
            // We have to find the role of each user
            var userRoles = _db.UserRoles.ToList(); //In that way we will have the mapping table.
            var roles = _db.Roles.ToList();

            foreach (var user in users) 
            {
                
                var roleId  = userRoles.FirstOrDefault(u=>u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (user.Company == null)
                {
                    user.Company = new() { Name = "" };
                }
            }
            return Json(new {data = users});
        }

        [HttpPost] 
        public IActionResult LockUnlock([FromBody]string id)
        {
            //Based on that id, we want to retrieve the user record from the database, and we will return Json back if that object is empty
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error while Locking/Unlocking" });
            }

            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now)
            {
                // user is currently locked, and we need to unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                // user is currently unlocked, and we have to unlock them
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000); // user will be locked for the next thousand years.
            }
            _db.SaveChanges();
            return Json(new {success = true, message = "Operation Successful"});  
        }

        #endregion
    }



}
