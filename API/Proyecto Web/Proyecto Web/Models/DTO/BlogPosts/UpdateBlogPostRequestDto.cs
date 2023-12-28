using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web.Models.DTO.BlogPosts
{
    public class UpdateBlogPostRequestDto
    {
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string UrlHandle { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set; }

        public string Author { get; set; }

        public bool IsVisible { get; set; }

        public List<Guid> Categories { get; set; } = new List<Guid>();
    }
}
