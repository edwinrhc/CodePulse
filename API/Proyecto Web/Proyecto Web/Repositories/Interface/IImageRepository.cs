using Proyecto_Web.Models.Domain;

namespace Proyecto_Web.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

       Task<IEnumerable<BlogImage>> GetAll();
    }
}
