using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGist.Model;
using NuGist.Web.Data;
using NuGist.Web.Services.Gists;
using System.IO;
using System.Threading.Tasks;

namespace NuGist.Web.Controllers
{
    [Authorize]
    public class GistController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
            : base(context, userManager)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ActionResult> Index()
            => HandleView(await context.GetGistsForUserAsync(GetUserId()));

        public ActionResult New() => View();

        public async Task<ActionResult> Detail(int id)
            => HandleView(await context.GetGistDetailsAsync(id, GetUserId()));

        [HttpPost("~/api/gist/create"), Produces(typeof(NewGistResponse))]
        public async Task<ActionResult> Create([FromBody]NewGistRequest model)
            => HandleOk(await context.CreateNewGistAsync(model, GetUserId()));

        [HttpPost("~/api/gist/{id}"), Produces(typeof(UpdateGistResponse))]
        public async Task<ActionResult> Update(int id, [FromBody]UpdateGistRequest model)
            => HandleOk(await context.UpdateGistAsync(id, model, GetUserId()));

        public string Root => Path.Combine(_hostingEnvironment.ContentRootPath, "temp");

        // TODO: change to post
        [HttpGet("~/api/gist/build/{id}")]
        public async Task<ActionResult> Build(int id)
            => HandleOk(await context.BuildNugetAsync(Root, id, GetUserId()));
    }
}
