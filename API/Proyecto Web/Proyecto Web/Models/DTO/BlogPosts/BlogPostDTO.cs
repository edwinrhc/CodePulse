namespace Proyecto_Web.Models.DTO.BlogPosts
{
    public class BlogPostDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Contet { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string UrlHandle { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Author { get; set; }

        public bool IsVisible { get; set; }
    }
}
