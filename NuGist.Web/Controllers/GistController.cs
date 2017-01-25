using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGist.Model;
using NuGist.Web.Data;
using NuGist.Web.Services.Gists;
using System.Threading.Tasks;

namespace NuGist.Web.Controllers
{
    [Authorize]
    public class GistController : BaseController
    {
        public GistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            : base(context, userManager)
        {
        }

        public ActionResult New() => View();

        public async Task<ActionResult> Detail(int id)
            => HandleView(await context.GetGistDetailsAsync(id, GetUserId()));

        [HttpPost("~/api/gist/create"), Produces(typeof(NewGistResponse))]
        public async Task<ActionResult> Create([FromBody]NewGistRequest model)
            => HandleOk(await context.CreateNewGistAsync(model, GetUserId()));

        [HttpPost("~/api/gist/{id}"), Produces(typeof(UpdateGistResponse))]
        public async Task<ActionResult> Update(int id, [FromBody]UpdateGistRequest model)
            => HandleOk(await context.UpdateGistAsync(id, model, GetUserId()));
    }
}
