using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CrispyRazor.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30, ErrorMessage = "Name must be between 1 and 30 symbols")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
        public int DisplayOrder { get; set; }
    }
}
