using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NuGist.Web.Services.Gists
{
    public class UpdateGistRequest
    {
        public class FileUpdate
        {
            [Required]
            public int Id { get; set; }
            [Required]
            public string Content { get; set; }
        }

        [Required]
        public string Version { get; set; }

        [Required]
        public List<FileUpdate> Files { get; set; }
    }

    public class UpdateGistResponse
    {
    }
}
