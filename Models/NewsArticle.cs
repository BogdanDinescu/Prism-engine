using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class NewsArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
        public int NewsSourceId { get; set; }
        public NewsSource NewsSource { get; set; }

    }
}
