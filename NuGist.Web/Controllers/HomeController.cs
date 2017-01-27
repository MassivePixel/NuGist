using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Mime;

namespace NuGist.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index() => View();
        public IActionResult Error() => View();
        public IActionResult React() => View();

        public string Root => Path.Combine(_hostingEnvironment.ContentRootPath, "temp");

        public ActionResult List()
        {
            Directory.CreateDirectory(Root);
            var directories = Directory.GetDirectories(Root);

            return Json(new
            {
                directories
            });
        }

        public ActionResult GetPackage(string id)
        {
            var dir = Path.Combine(Root, id, "output");
            var file = Directory.GetFiles(dir)[0];
            //using (var stream = System.IO.File.OpenRead(file))
                //return File(stream, "application/x-compressed");
                return File(System.IO.File.ReadAllBytes(file), MediaTypeNames.Text.Plain, Path.GetFileName(file));
        }
    }
}
