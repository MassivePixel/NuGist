using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGist.Model;
using NuGist.Web.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NuGist.Web.Controllers
{
    public class NewGistViewModel
    {
        public string Name { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public string Filename { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Type { get; set; }
    }

    public class GistFileViewModel
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }

        public GistFileViewModel(GistFile file)
        {
            Id = file.Id;
            Filename = file.FileName;
            Content = file.Content;
            Type = file.Type;
        }
    }

    public class GistViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public List<GistFileViewModel> Files { get; set; }

        public GistViewModel(Gist gist, ICollection<GistFile> files)
        {
            Id = gist.Id;
            Name = gist.Name;
            Version = gist.Version;
            Files = files.Select(f => new GistFileViewModel(f)).ToList();
        }
    }

    [Authorize]
    public class GistController : BaseController
    {
        public GistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            : base(context, userManager)
        {
        }

        public ActionResult New() => View();

        public async Task<ActionResult> Detail(int id)
        {
            var userId = GetUserId();
            var gist = await (from g in context.Gists
                              where g.Id == id &&
                                    g.CreatedById == userId
                              select g)
                              .FirstOrDefaultAsync();

            if (gist == null)
                return NotFound();

            var files = await (from file in context.GistFiles
                               where file.GistId == gist.Id &&
                                     file.InternalVersion == gist.InternalVersion
                               select file)
                              .ToListAsync();


            return View(new GistViewModel(gist, files));
        }

        [HttpPost("~/api/gist/create")]
        public async Task<ActionResult> Create([FromBody]NewGistViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else if (model == null)
                return BadRequest();

            var name = model.Name ?? Path.GetFileNameWithoutExtension(model.Filename);
            var ns = "namespace";

            var gist = new Gist
            {
                Name = $"{ns}.{name}",
                Version = model.Version ?? "0.0.1",
                InternalVersion = 1,
                Metadata = JsonConvert.SerializeObject(new NugetMetadata())
            };
            OnCreateEntity(gist);
            context.Gists.Add(gist);

            var file = new GistFile
            {
                Type = "csharp",
                FileName = model.Filename,
                Content = model.Content,
                InternalVersion = 1,
                Gist = gist
            };
            OnCreateEntity(file);
            context.GistFiles.Add(file);

            await context.SaveChangesAsync();

            return Ok(new
            {
                id = gist.Id
            });
        }
    }
}
