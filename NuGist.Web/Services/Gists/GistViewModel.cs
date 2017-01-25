using NuGist.Model;
using System.Collections.Generic;
using System.Linq;

namespace NuGist.Web.Services.Gists
{
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
}
