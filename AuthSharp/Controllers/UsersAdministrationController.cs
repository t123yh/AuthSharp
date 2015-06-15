using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using AuthSharp.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AuthSharp.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class UsersAdministrationController : Controller
    {
        ApplicationDbContext _dbContext;

        ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationDbContext DbContext
        {
            get
            {
                return _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _dbContext = value;
            }
        }
        List<ApplicationUser> NormalUsers
        {
            get
            {
                //var roleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(HttpContext.GetOwinContext().Get<ApplicationDbContext>()));
                var userIds = DbContext.Roles.Single(role => role.Name == "Users").Users.Select(a => a.UserId);
                //roleManager.Roles.Single(role => role.Name == "Administrator").Users.Select(a=>a.UserId);
                return DbContext.Users.Where(user => userIds.Contains(user.Id)).ToList();
            }
        }
        public ApplicationUser CurrentApplicationUser
        {
            get
            {
                return DbContext.Users.Single(user => user.UserName == User.Identity.Name);
            }
        }

        // GET: UsersAdministration
        public ActionResult Index()
        {
            return View(NormalUsers);
        }

        public ActionResult Details(string id)
        {
            var user = GetApplicationUserById(id);
            return View(user);
        }


       
        private ApplicationUser GetApplicationUserById(string id)
        {
            return DbContext.Users.Single(u => u.Id == id);
        }

        private ApplicationUser GetApplicationUserByName(string username)
        {
            return DbContext.Users.Single(u => u.UserName == username);
        }

        [HttpPost]
        public ActionResult ConfirmedRequest(string uid, Guid rid)
        {
            RemoveRechargeRequestsWith(rid, request =>
            {
                GetApplicationUserById(uid).TrafficRemaining += request.Amount;
            });
            return RedirectToAction("Details", new { id = uid });
        }

        [HttpPost]
        public ActionResult CancelRequest(string uid, Guid rid)
        {
            RemoveRechargeRequestsWith(rid, null);
            return RedirectToAction("Details", new { id = uid });
        }

        private void RemoveRechargeRequestsWith(Guid id, Action<RechargeRequest> work)
        {
            var request = DbContext.RechargeRequests.Single(r => r.RequestID == id);
            if (work != null) work(request);
            DbContext.RechargeRequests.Remove(request);
            DbContext.SaveChanges();
        }

        public ActionResult SetPassword(string id)
        {
            ResetUserPasswordViewModel model = new ResetUserPasswordViewModel()
            {
                UserName = GetApplicationUserById(id).UserName
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SetPassword(ResetUserPasswordViewModel model)
        {
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "用户不存在。");
                model.NewPassword = model.ConfirmPassword = "";
                return View(model);
            }
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, code, model.NewPassword);
            return RedirectToAction("Details", new { id = user.Id });
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}