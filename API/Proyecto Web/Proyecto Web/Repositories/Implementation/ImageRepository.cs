using Microsoft.EntityFrameworkCore;
using Proyecto_Web.Data;
using Proyecto_Web.Models.Domain;
using Proyecto_Web.Repositories.Interface;

namespace Proyecto_Web.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, 
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await dbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
           
            // 1- Upload the Image to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath,"Images",$"{blogImage.Filename}{blogImage.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // 2- Update the database
            // https://codepulse.com/images/somefiles.jpg
            var  httRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httRequest.Scheme}://{httRequest.Host}{httRequest.PathBase}/Images/{blogImage.Filename}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await dbContext.BlogImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();

            return blogImage;

        }
    }
}
