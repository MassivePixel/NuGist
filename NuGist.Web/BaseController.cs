using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGist.Model;
using NuGist.Web.Data;
using System;

namespace NuGist.Web.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext context;
        protected UserManager<ApplicationUser> userManager;

        public BaseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        protected void OnCreateEntity(BaseEntity entity)
        {
            var now = DateTimeOffset.UtcNow;

            entity.CreatedOn = now;
            entity.CreatedById = GetUserId();

            entity.ModifiedOn = now;
            entity.ModifiedById = GetUserId();
        }

        protected void OnModifiedEntity(BaseEntity entity)
        {
            var now = DateTimeOffset.UtcNow;

            entity.ModifiedOn = now;
            entity.ModifiedById = GetUserId();
        }

        protected string GetUserId() => userManager.GetUserId(User);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
