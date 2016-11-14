using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NuGist.Model
{
    /// <summary>
    /// Why serializable class? Because I am lazy.
    /// </summary>
    public class NugetMetadata
    {
    }

    public class Gist : BaseEntity
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [MaxLength(32)]
        public string Version { get; set; }

        /// <summary>
        /// Serialize as JSON string here.
        /// </summary>
        public string Metadata { get; set; }

        public int InternalVersion { get; set; }

        public virtual ICollection<GistFile> Files { get; set; }
    }

    public class GistFile : BaseEntity
    {
        [Required]
        [MaxLength(32)]
        public string Type { get; set; }

        [Required]
        [MaxLength(64)]
        public string FileName { get; set; }

        public string Content { get; set; }

        public int InternalVersion { get; set; }

        public int GistId { get; set; }
        public virtual Gist Gist { get; set; }
    }
}
