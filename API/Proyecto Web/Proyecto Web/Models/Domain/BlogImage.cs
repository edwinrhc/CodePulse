namespace Proyecto_Web.Models.Domain
{
    public class BlogImage
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string FileExtension { get; set; }   
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }


    }
}
