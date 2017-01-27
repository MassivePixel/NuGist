using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGist.Model;
using NuGist.Nuget;
using NuGist.Services;
using NuGist.Web.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NuGist.Web.Services.Gists
{
    public static class GistsService
    {
        public static async Task<ResultOrError<List<GistViewModel>>> GetGistsForUserAsync(this ApplicationDbContext context, string userId)
        {
            var gists = await (from g in context.Gists
                               where g.CreatedById == userId
                               select g).ToListAsync();

            return gists.Select(g => new GistViewModel(g)).ToList();
        }

        public static async Task<ResultOrError<GistViewModel>> GetGistDetailsAsync(this ApplicationDbContext context, int id, string userId)
        {
            var gist = await (from g in context.Gists
                              where g.Id == id &&
                                    g.CreatedById == userId
                              select g)
                              .FirstOrDefaultAsync();

            if (gist == null)
                return CommonErrors.NotFound;

            var files = await (from file in context.GistFiles
                               where file.GistId == gist.Id &&
                                     file.InternalVersion == gist.InternalVersion
                               select file)
                              .ToListAsync();

            return new GistViewModel(gist, files);
        }

        public static async Task<ResultOrError<NewGistResponse>> CreateNewGistAsync(this ApplicationDbContext context, NewGistRequest model, string userId)
        {
            var name = model.Name ?? Path.GetFileNameWithoutExtension(model.Filename);
            var ns = "namespace";

            var gist = new Gist
            {
                Name = $"{ns}.{name}",
                Version = model.Version ?? "0.0.1",
                InternalVersion = 1,
                Metadata = JsonConvert.SerializeObject(new NugetMetadata())
            };
            context.Gists.AddNewEntity(gist, userId);

            var file = new GistFile
            {
                Type = "csharp",
                FileName = model.Filename,
                Content = model.Content,
                InternalVersion = 1,
                Gist = gist
            };
            context.GistFiles.AddNewEntity(file, userId);

            await context.SaveChangesAsync();

            return new NewGistResponse
            {
                Id = gist.Id
            };
        }

        public static async Task<ResultOrError<UpdateGistResponse>> UpdateGistAsync(this ApplicationDbContext context, int id, UpdateGistRequest model, string userId)
        {
            var gist = await (from g in context.Gists
                              where g.Id == id &&
                                    g.CreatedById == userId
                              select g)
                              .Include(g => g.Files)
                              .FirstOrDefaultAsync();

            if (gist == null)
                return $"No gist with id {id} found";

            if (!NewVersionIsGreaterThanCurrent(model.Version, gist.Version))
                return $"New version '{model.Version}' must be greater than the current version '{gist.Version}'";

            gist.Version = model.Version;
            gist.InternalVersion++;

            var files = model.Files
                .Select(file => new
                {
                    file.Id,
                    newFile = file,
                    currentFile = gist.Files.FirstOrDefault(x => x.Id == file.Id)
                })
                .ToList();

            if (files.Any(x => x.currentFile == null))
                return $"Invalid file id {files.First(x => x.currentFile == null).Id}";

            foreach (var file in files)
            {
                file.currentFile.Content = file.newFile.Content;
                file.currentFile.InternalVersion++;
            }

            files.Select(x => (BaseEntity)x.currentFile).Union(new[] { gist }).UpdateModified();

            await context.SaveChangesAsync();

            return new UpdateGistResponse { };
        }

        public static async Task<ResultOrError<NugetPackage>> BuildNugetAsync(this ApplicationDbContext context, string root, int id, string userId)
        {
            var gist = await (from g in context.Gists
                              where g.Id == id &&
                                    g.CreatedById == userId
                              select new
                              {
                                  g,
                                  files = g.Files.Where(f => f.InternalVersion == g.InternalVersion).ToList()
                              }).FirstOrDefaultAsync();

            if (gist == null)
                return CommonErrors.NotFound;

            Directory.CreateDirectory(root);
            var tempFolderName = Guid.NewGuid().ToString();
            var dir = Path.Combine(root, tempFolderName);
            var inputFolder = Path.Combine(root, tempFolderName, "input");
            var outputFolder = Path.Combine(root, tempFolderName, "output");

            var slug = $"{gist.g.Name}";

            var file = gist.files.FirstOrDefault();
            if (file == null)
                return $"Gist doesn't contain files";
            var filename = file.FileName;

            // create temporary folders
            Directory.CreateDirectory(inputFolder);
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(Path.Combine(root, "packages"));

            File.WriteAllText(Path.Combine(inputFolder, filename), file.Content);
            File.WriteAllText(Path.Combine(inputFolder, $"{slug}.nuspec"),
$@"<?xml version=""1.0""?>
<package >
  <metadata>
    <id>{slug}</id>
    <version>{gist.g.Version}</version>
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

            Commands.Pack(dir, new PackParams
            {
                BasePath = Path.Combine(dir, "input"),
                NuspecFileName = slug + ".nuspec",
                Version = gist.g.Version,
                OutputDirectory = Path.Combine(dir, "output")
            });

            var packageFile = Directory.EnumerateFiles(outputFolder)
                .FirstOrDefault(x => Path.GetFileName(x) == $"{slug}.{gist.g.Version}.nupkg");

            if (!string.IsNullOrEmpty(packageFile))
                File.Copy(packageFile, Path.Combine(root, "packages", Path.GetFileName(packageFile)), true);

            return new NugetPackage
            {
                Id = tempFolderName
            };
        }

        private static bool NewVersionIsGreaterThanCurrent(string @new, string current)
        {
            // dummy check for now just to ensure that the new version is not the same as the old one
            return @new != current;
        }
    }
}
