using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.response
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreateOn { get; set; } = DateTime.Now;
        // Reference to "Stock" models
        public int? StockId { get; set; }
    }
}