using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.FAQ.Models
{
    [Table("GIBSFAQ_Category")]
    public class Category : ModelBase
    {
        [Key]
        public int CategoryId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int ParentId { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
