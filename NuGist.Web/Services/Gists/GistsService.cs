using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGist.Model;
using NuGist.Services;
using NuGist.Web.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace NuGist.Web.Services.Gists
{
    public static class GistsService
    {
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

        private static bool NewVersionIsGreaterThanCurrent(string @new, string current)
        {
            // dummy check for now just to ensure that the new version is not the same as the old one
            return @new != current;
        }
    }
}
