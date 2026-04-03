using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmlaProductCatalog
{
    public class UserRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string TemplateName { get; set; }

        public string? Request { get; set; }

        public string? Response { get; set; }  

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
