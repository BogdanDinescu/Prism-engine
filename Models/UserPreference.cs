using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class UserPreference
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [MaxLength(60)]
        public string City { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<NewsSource> NewsSources { get; set; }
    }
}
