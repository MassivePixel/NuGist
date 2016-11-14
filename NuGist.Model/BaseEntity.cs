using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuGist.Model
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public virtual ApplicationUser ModifiedBy { get; set; }
    }
}
