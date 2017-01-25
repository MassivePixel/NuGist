using System.ComponentModel.DataAnnotations;

namespace NuGist.Web.Services.Gists
{
    public class NewGistRequest
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

    public class NewGistResponse
    {
        public int Id { get; set; }
    }
}
