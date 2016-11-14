using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using NuGist.Nuget;
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

        public string Root => Path.Combine(_hostingEnvironment.ContentRootPath, "temp");

        public ActionResult Build()
        {
            Directory.CreateDirectory(Root);
            var commands = new Commands();
            var sub = Guid.NewGuid().ToString();
            var dir = Path.Combine(Root, sub);

            var slug = "namespace.package";
            var filename = "package.cs";

            Directory.CreateDirectory(Path.Combine(dir, "input"));
            Directory.CreateDirectory(Path.Combine(dir, "output"));

            System.IO.File.WriteAllText(Path.Combine(dir, "input", filename), "hello");
            System.IO.File.WriteAllText(Path.Combine(dir, "input", slug + ".nuspec"),
$@"<?xml version=""1.0""?>
<package >
  <metadata>
    <id>test.cs</id>
    <version>1.0.0</version>
    <authors>ToniP</authors>
    <owners>ToniP</owners>
    <licenseUrl>http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE</licenseUrl>
    <projectUrl>http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE</projectUrl>
    <iconUrl>http://ICON_URL_HERE_OR_DELETE_THIS_LINE</iconUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Package description</description>
    <releaseNotes>Summary of changes made in this release of the package.</releaseNotes>
    <copyright>Copyright 2016</copyright>
    <tags>Tag1 Tag2</tags>
  </metadata>
  <files>
    <file src=""{filename}"" target=""content"" />
  </files>
</package>");

            commands.Pack(dir, new PackParams
            {
                NuspecFileName = slug + ".nuspec",
                Version = "0.1.0-alpha"
            });
            return View();
        }

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
