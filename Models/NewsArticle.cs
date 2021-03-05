using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Prism.Models
{
    [Index(nameof(SimHash))]
    public class NewsArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public uint SimHash { get; set; }
        public string Source { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
        public int NewsSourceId { get; set; }
        public NewsSource NewsSource { get; set; }

    }
}
