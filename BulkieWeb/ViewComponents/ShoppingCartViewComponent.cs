using Bulkie.DataAccess.Repository.IRepository;
using Bulkie.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkieWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity; //Get user id of a logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null) // means a user is logged in.
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    // In that case we can get the session and set it
                    HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
