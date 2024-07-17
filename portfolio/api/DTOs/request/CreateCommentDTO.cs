using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.request
{
    public class CreateCommentDTO
    {
        [Required]
        [MinLength(5, ErrorMessage = "Titlte must be at least 5 characters")]
        [MaxLength(280, ErrorMessage = "Titlte must be at most 280 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters")]
        [MaxLength(280, ErrorMessage = "Content must be at most 280 characters")]
        public string Content { get; set; } = string.Empty;
        // Reference to "Stock" models
    }
}