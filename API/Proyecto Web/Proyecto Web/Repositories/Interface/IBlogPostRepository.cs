using Proyecto_Web.Models.Domain;

namespace Proyecto_Web.Repositories.Interface
{
    public interface IBlogPostRepository
    {
       Task<BlogPost> CreateAsync(BlogPost blogPost);
    }
}
