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
        public string Name { get; set; }
    }
}
