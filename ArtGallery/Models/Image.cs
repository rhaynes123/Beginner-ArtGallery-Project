using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
	public class Image
	{
		[Key]
		public int Id { get; set; }
		public string? FilePath { get; set; }
		[Required]
		public Guid Guid { get; set; } = Guid.NewGuid();
		[NotMapped,Required(ErrorMessage = "Image Must be Provided"), Display(Name ="File To Upload")]
        public IFormFile? FormFile { get; set; }
		public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
	}
}

