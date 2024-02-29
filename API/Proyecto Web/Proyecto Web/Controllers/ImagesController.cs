using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web.Models.Domain;
using Proyecto_Web.Models.DTO.BlogImages;
using Proyecto_Web.Repositories.Interface;

namespace Proyecto_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // GET: {apibaseurl}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            // call image repository to get all images
            var images = await imageRepository.GetAll();

            // Convert Domain model to DTO
            var response = new List<BlogImageDTO>();
            foreach (var image in images)
            {
                response.Add(new BlogImageDTO
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    Filename = image.Filename,
                    Url = image.Url
                });
            }

            return Ok(response);

        }



        // POST: {apibaseurl}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName,
            [FromForm] string title)
        {
            ValidateFileUpload(file);

            if(ModelState.IsValid)
            {
                //File upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    Filename = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

                blogImage =  await imageRepository.Upload(file, blogImage);

                // Convert Domain Model to DTO
                var response = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    Filename = blogImage.Filename,
                    Url = blogImage.Url
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowdExtensions = new string[] { ".jpg", ".jpeg", "png" };

            if (allowdExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) )
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }

        }

    }
}
