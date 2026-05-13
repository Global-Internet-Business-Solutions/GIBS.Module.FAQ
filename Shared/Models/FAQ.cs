using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.FAQ.Models
{
    [Table("GIBSFAQ")]
    public class FAQ : ModelBase
    {
        [Key]
        public int FAQId { get; set; }
        public int ModuleId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
        public int SortOrder { get; set; }
        public string Status { get; set; } //(status IN ('draft', 'published', 'archived'))
        public int ViewCount { get; set; } = 0;
        [NotMapped]
        public string SubmitterName { get; set; }
        [NotMapped]
        public string SubmitterEmail { get; set; }

        [NotMapped]
        public string CategoryName { get; set; } // Not mapped, for display only
        [NotMapped]
        public int CategorySortOrder { get; set; } // Not mapped, for sorting only
    }
}
